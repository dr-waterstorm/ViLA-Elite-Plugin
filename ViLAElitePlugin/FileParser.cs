using Microsoft.Extensions.Logging;

public class FileParser : IFileParser
{
    ILogger<FileParser> _logger;

    public FileParser(ILoggerFactory iLoggerFactory)
    {
        _logger = iLoggerFactory.CreateLogger<FileParser>();
    }

    public async Task ParseFile(string path)
    {
        if(!File.Exists(path))
        {
            _logger.LogError($"File does not exist {path}");
            return;
        }

        _logger.LogInformation($"Start reading {path}");

        var configString = await File.ReadAllTextAsync(path);
        // Console.WriteLine(configString);
        // _logger.LogInformation($"Content {configString}");

        _logger.LogInformation($"Completed read of {path}");
    }
}