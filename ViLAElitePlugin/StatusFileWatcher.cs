using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

public class StatusFileWatcher : IStatusFileWatcher, IDisposable
{
    private const string StatusFilter = @"Status*.json";
    private const string StatusFileName = "Status.json";

    // the watcher fires the moment the game starts writing, so give it a moment to finish
    private const int WriteSettleDelayMs = 50;

    private readonly FileSystemWatcher _fileSystemWatcher;
    private readonly ILogger<StatusFileWatcher> _logger;
    private readonly string _path;
    private readonly IStatusTranslator _translator;

    // the watcher can fire several times for a single write. Parsing one file at a time keeps a
    // slow read from applying an older snapshot on top of a newer one, and keeps us from handing
    // values to ViLA from two threads - its state dictionary is not synchronized.
    // Not disposed on purpose: a read queued behind us would throw on a disposed semaphore.
    private readonly SemaphoreSlim _parseLock = new(1, 1);

    private bool _gameWasRunning;
    private bool _disposed;

    public string StatusFilePath { get; }

    public StatusFileWatcher(ILogger<StatusFileWatcher> logger, string path, IStatusTranslator statusTranslator)
    {
        _logger = logger;
        _path = Environment.ExpandEnvironmentVariables(path);
        _translator = statusTranslator;
        _fileSystemWatcher = new FileSystemWatcher();
        StatusFilePath = Path.Combine(_path, StatusFileName);

        // read once up front, the game only writes the file again when something changes
        _ = ParseFileAsync(StatusFilePath);
    }

    public void Start()
    {
        if (!Directory.Exists(_path))
        {
            _logger.LogError("Status file directory {Path} does not exist. Check the StatusLocation setting in the plugin config", _path);
            return;
        }

        _fileSystemWatcher.Path = _path;
        _fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
        _fileSystemWatcher.Filter = StatusFilter;

        _fileSystemWatcher.Changed += new FileSystemEventHandler(OnChanged);

        _fileSystemWatcher.EnableRaisingEvents = true;

        _logger.LogInformation("Watching {StatusFileName} for changes ...", StatusFileName);
    }

    public void Stop ()
    {
        _fileSystemWatcher.Changed -= new FileSystemEventHandler(OnChanged);
        _fileSystemWatcher.EnableRaisingEvents = false;

        _logger.LogInformation("Stopping watcher...");
    }

    private void OnChanged(object source, FileSystemEventArgs e)
    {
        _ = ParseFileAsync(e.FullPath);
    }

    public async Task ParseFileAsync(string path)
    {
        if (_disposed)
        {
            return;
        }

        // wait a bit for the file to actually be written, the filewatcher calls this immediatly on
        // change! Waiting before taking the lock keeps the delay off the queue behind us.
        await Task.Delay(WriteSettleDelayMs).ConfigureAwait(false);

        await _parseLock.WaitAsync().ConfigureAwait(false);

        try
        {
            // checked again: this may have been queued behind other reads while we were disposed
            if (_disposed)
            {
                return;
            }

            ParseFile(path);
        }
        finally
        {
            _parseLock.Release();
        }
    }

    private void ParseFile(string path)
    {
        if (!File.Exists(path))
        {
            _logger.LogError("File does not exist {Path}", path);
            return;
        }

        try
        {
            // This locks the file
            // var statusFile = await File.ReadAllTextAsync(path);

            // Do basically the same, but without locking!
            string statusFile;

            using (StreamReader streamReader = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
            {
                statusFile = streamReader.ReadToEnd();
            }

            EliteStatusFile eliteStatusFile = JsonConvert.DeserializeObject<EliteStatusFile>(statusFile) ?? throw new JsonSerializationException("Result was null");

            eliteStatusFile.ParseFlags();
            eliteStatusFile.ParseVariables();

            // the game was closed: the next start has to send a full update, whatever ViLA still
            // holds from this session cannot be trusted
            if (_gameWasRunning && !eliteStatusFile.ExposedGameStarted)
            {
                _translator.ForgetSentValues();
            }

            _gameWasRunning = eliteStatusFile.ExposedGameStarted;

            eliteStatusFile.SendExposedValues(_translator);
        }
        catch (JsonException ex)
        {
            // catching half written files is normal, but a value the model cannot represent stops
            // every single variable from updating - that needs to be visible in the log
            _logger.LogWarning("Could not parse {Path}: {Message}", path, ex.Message);
        }
        catch (IOException ex)
        {
            _logger.LogWarning("Could not read {Path}: {Message}", path, ex.Message);
        }
        catch (Exception ex)
        {
            // this covers ViLA itself as well, the values are handed over inside this call
            _logger.LogError(ex, "Unexpected error while handling {Path}", path);
        }
    }

    public void Dispose()
    {
        _disposed = true;
        this.Stop();
        _fileSystemWatcher.Dispose();
    }
}
