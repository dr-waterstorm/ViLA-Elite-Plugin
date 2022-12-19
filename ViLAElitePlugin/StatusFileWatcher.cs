using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

public class StatusFileWatcher : IStatusFileWatcher, IDisposable
{
    private FileSystemWatcher _fileSystemWatcher;
    private ILogger<StatusFileWatcher> _logger;
    private string _path;
    private readonly IStatusTranslator _translator;
    private const string StatusFilter = @"Status*.json";

    public StatusFileWatcher(ILogger<StatusFileWatcher> logger, string path, IStatusTranslator statusTranslator)
    {
        _logger = logger;
        _path = Environment.ExpandEnvironmentVariables(path);
        _translator = statusTranslator;
        _fileSystemWatcher = new FileSystemWatcher();
        
        Task.Run(() => this.ParseFile(_path + "Status.json"));
    }

    public void Start()
    {
        _fileSystemWatcher.Path = _path;
        _fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
        _fileSystemWatcher.Filter = StatusFilter;

        _fileSystemWatcher.Changed += new FileSystemEventHandler(OnChanged);

        _fileSystemWatcher.EnableRaisingEvents = true;

        _logger.LogInformation("Watching Status.json for changes ...");
    }

    public void Stop ()
    {
        _fileSystemWatcher.Changed -= new FileSystemEventHandler(OnChanged);
        _fileSystemWatcher.EnableRaisingEvents = false;

        _logger.LogInformation("Stopping watcher...");
    }

    private void OnChanged(object source, FileSystemEventArgs e)
    {
        Task.Run(() => this.ParseFile(e.FullPath));
    }

    public void ParseFile(string path)
    {
        // wait a bit for the file to actually be written, the filewatcher calls this function immediatly on change!
        Thread.Sleep(50);

        if (!File.Exists(path))
        {
            _logger.LogError($"File does not exist {path}");
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
                _logger.LogInformation(statusFile);
            }

            EliteStatusFile eliteStatusFile = JsonConvert.DeserializeObject<EliteStatusFile>(statusFile) ?? throw new JsonSerializationException("Result was null");

            eliteStatusFile.parseRawFlags();
            eliteStatusFile.parseVariables();
            eliteStatusFile.updateAllIntProperties(_translator);
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Could not read / parse file!");
            _logger.LogInformation(ex.Message);
        }
    }

    public void Dispose()
    {
        this.Stop();
        _fileSystemWatcher.Dispose();
    }
}