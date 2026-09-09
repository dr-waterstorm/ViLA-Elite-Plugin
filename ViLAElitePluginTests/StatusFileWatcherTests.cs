using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace ViLAElitePluginTests;

public class StatusFileWatcherTests : IDisposable
{
    private readonly string _directory;

    /// <summary>How many values one parse of a running game sends: GameStarted plus all the rest.</summary>
    private static readonly int ExposedValueCount = typeof(EliteStatusFile)
        .GetProperties()
        .Count(property => property.Name.StartsWith("Exposed"));

    public StatusFileWatcherTests()
    {
        _directory = Path.Combine(Path.GetTempPath(), "vila-elite-tests-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_directory);
    }

    public void Dispose()
    {
        Directory.Delete(_directory, true);
    }

    // the file is deliberately not called Status.json: the watcher reads that one by itself on
    // construction, and these tests want to drive the parsing themselves
    private string WriteStatusFile(string content)
    {
        var path = Path.Combine(_directory, "status-under-test.json");
        File.WriteAllText(path, content);

        return path;
    }

    private StatusFileWatcher CreateWatcher(RecordingTranslator translator, string? path = null)
    {
        return new StatusFileWatcher(NullLogger<StatusFileWatcher>.Instance, path ?? _directory, translator);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Status_file_path_is_joined_properly(bool configuredWithTrailingSeparator)
    {
        // a StatusLocation without a trailing separator used to end up as ".../Elite DangerousStatus.json"
        var configured = Path.Combine(_directory, "elite");
        if (configuredWithTrailingSeparator)
        {
            configured += Path.DirectorySeparatorChar;
        }

        var watcher = CreateWatcher(new RecordingTranslator(), configured);

        Assert.Equal(Path.Combine(_directory, "elite", "Status.json"), watcher.StatusFilePath);
    }

    [Fact]
    public async Task Parsing_sends_the_values_to_the_translator()
    {
        var translator = new RecordingTranslator();
        var watcher = CreateWatcher(translator);

        await watcher.ParseFileAsync(WriteStatusFile(StatusFiles.DockedWithLargeBalance));

        Assert.Equal(4648069815L, translator.ValueOf("Balance"));
        Assert.Equal(true, translator.ValueOf("Docked"));
        Assert.Equal(0, translator.ForgetCalls);
        Assert.Equal(ExposedValueCount, translator.Sent.Count);
    }

    [Fact]
    public async Task Sent_values_are_forgotten_once_the_game_closes()
    {
        var translator = new RecordingTranslator();
        var watcher = CreateWatcher(translator);

        await watcher.ParseFileAsync(WriteStatusFile(StatusFiles.DockedWithLargeBalance));
        Assert.Equal(0, translator.ForgetCalls);

        // whatever ViLA still holds from this session is not worth trusting once the game is gone
        await watcher.ParseFileAsync(WriteStatusFile(StatusFiles.GameNotRunning));
        Assert.Equal(1, translator.ForgetCalls);
        Assert.Equal(false, translator.ValueOf("GameStarted"));

        // still closed, only the transition counts
        await watcher.ParseFileAsync(WriteStatusFile(StatusFiles.GameNotRunning));
        Assert.Equal(1, translator.ForgetCalls);
    }

    [Fact]
    public async Task A_half_written_file_does_not_throw()
    {
        var translator = new RecordingTranslator();
        var watcher = CreateWatcher(translator);

        await watcher.ParseFileAsync(WriteStatusFile(@"{ ""Flags"": 214748"));

        Assert.Empty(translator.Sent);
    }

    [Fact]
    public async Task A_missing_file_does_not_throw()
    {
        var translator = new RecordingTranslator();
        var watcher = CreateWatcher(translator);

        await watcher.ParseFileAsync(Path.Combine(_directory, "does-not-exist.json"));

        Assert.Empty(translator.Sent);
    }

    [Fact]
    public async Task Parsing_is_serialised()
    {
        var translator = new RecordingTranslator();
        var watcher = CreateWatcher(translator);
        var path = WriteStatusFile(StatusFiles.DockedWithLargeBalance);

        // the file watcher fires several times per write, so this happens constantly
        await Task.WhenAll(Enumerable.Range(0, 8).Select(_ => watcher.ParseFileAsync(path)));

        // one parse sends every exposed value exactly once, so cutting the recording into batches
        // of that size has to give batches of distinct ids - interleaved parses would duplicate
        // ids inside a batch
        Assert.Equal(8 * ExposedValueCount, translator.Sent.Count);
        Assert.All(
            translator.Sent.Chunk(ExposedValueCount),
            batch => Assert.Equal(ExposedValueCount, batch.Select(entry => entry.Key).Distinct().Count()));
    }

    [Fact]
    public async Task Disposing_stops_further_parsing()
    {
        var translator = new RecordingTranslator();
        var watcher = CreateWatcher(translator);
        var path = WriteStatusFile(StatusFiles.DockedWithLargeBalance);

        watcher.Dispose();
        await watcher.ParseFileAsync(path);

        Assert.Empty(translator.Sent);
    }

    [Fact]
    public async Task Disposing_stops_reads_that_are_already_queued()
    {
        var translator = new RecordingTranslator();
        var watcher = CreateWatcher(translator);
        var path = WriteStatusFile(StatusFiles.DockedWithLargeBalance);

        // queue a burst the way the file watcher does, then dispose while they are still waiting
        var queued = Enumerable.Range(0, 8).Select(_ => watcher.ParseFileAsync(path)).ToList();
        watcher.Dispose();
        await Task.WhenAll(queued);

        // whatever already started may finish, but the reads behind it stop
        Assert.True(
            translator.Sent.Count <= ExposedValueCount,
            $"expected at most one parse to get through, got {translator.Sent.Count} values");
    }
}
