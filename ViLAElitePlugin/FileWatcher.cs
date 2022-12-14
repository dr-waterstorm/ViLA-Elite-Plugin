using Microsoft.Extensions.Logging;

public class FileWatcher : IFileWatcher
{
    FileSystemWatcher _fileSystemWatcher;
    FileParser _fileParser;
    ILogger<FileWatcher> _logger;
    string _path;

    public FileWatcher(ILoggerFactory iLoggerFactory, string path)
    {
        _logger = iLoggerFactory.CreateLogger<FileWatcher>();
        _path = Environment.ExpandEnvironmentVariables(path);

        _fileSystemWatcher = new FileSystemWatcher(_path, "Status.json");
        _fileParser = new FileParser(iLoggerFactory);
    }

    public void Start()
    {
        _fileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess
                                | NotifyFilters.LastWrite
                                | NotifyFilters.Size;

        _fileSystemWatcher.Changed += _fileSystemWatcher_Changed;
        _fileSystemWatcher.Error += _fileSystemWatcher_Error;


        _fileSystemWatcher.EnableRaisingEvents = true;
        _fileSystemWatcher.IncludeSubdirectories = true;

        _logger.LogInformation("Actively watching Status.json for changes ...");
    }

    private void _fileSystemWatcher_Error(object source, ErrorEventArgs e)
    {
        _logger.LogInformation($"File error event {e.GetException().Message}");
    }

    private void _fileSystemWatcher_Changed(object source, FileSystemEventArgs e)
    {
        _logger.LogInformation("Changed:" +  e.FullPath + " " + e.ChangeType);
        Task.Run(() => _fileParser.ParseFile(e.FullPath));
    }
}