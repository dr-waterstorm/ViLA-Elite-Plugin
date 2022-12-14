using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

public class FileWatcher : IFileWatcher, IDisposable
{
    FileSystemWatcher _fileSystemWatcher;
    ILogger<FileWatcher> _logger;
    string _path;

    public FileWatcher(ILogger<FileWatcher> logger, string path)
    {
        _logger = logger;
        _path = Environment.ExpandEnvironmentVariables(path);

        _fileSystemWatcher = new FileSystemWatcher(_path, "Status.json");
        // _fileParser = new FileParser();
        Task.Run(() => this.ParseFile(_path + "Status.json"));
    }

    public void Start()
    {
        _fileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess
                                | NotifyFilters.LastWrite
                                | NotifyFilters.Size;

        _fileSystemWatcher.Changed += _fileSystemWatcher_Changed;
        _fileSystemWatcher.Error += _fileSystemWatcher_Error;


        _fileSystemWatcher.EnableRaisingEvents = true;

        _logger.LogInformation("Actively watching Status.json for changes ...");
    }

    public void Stop ()
    {
        _fileSystemWatcher.Changed -= _fileSystemWatcher_Changed;
        _fileSystemWatcher.Error -= _fileSystemWatcher_Error;

    
        _fileSystemWatcher.EnableRaisingEvents = false;
    }

    private void _fileSystemWatcher_Error(object source, ErrorEventArgs e)
    {
        _logger.LogInformation($"File error event {e.GetException().Message}");
    }

    private void _fileSystemWatcher_Changed(object source, FileSystemEventArgs e)
    {
        _logger.LogInformation("Changed:" +  e.FullPath + " " + e.ChangeType);
        Task.Run(() => this.ParseFile(e.FullPath));
    }

    public async Task ParseFile(string path)
    {
        if(!File.Exists(path))
        {
            _logger.LogError($"File does not exist {path}");
            return;
        }

        _logger.LogInformation($"Start reading {path}");

        var statusFile = await File.ReadAllTextAsync(path);
        EliteStatusFile eliteStatusFile = JsonConvert.DeserializeObject<EliteStatusFile>(statusFile) ?? throw new JsonSerializationException("Result was null");

        _logger.LogInformation("Finished reading");
        _logger.LogInformation($"Flags: {eliteStatusFile.Flags}");
        eliteStatusFile.parseRawFlags();
    }

    public void Dispose()
    {
        _fileSystemWatcher.Dispose();
    }
}