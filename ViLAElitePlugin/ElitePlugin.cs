using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ViLA.PluginBase;

public class ElitePlugin : PluginBase, IDisposable
{
    public const string ConfigPath = "Plugins/ViLAElitePlugin/config.json";

    private Task _thread = null!;
    ILogger<ElitePlugin> ?_logger;
    IFileWatcher ?_watcher;
    
    private static async Task<PluginConfiguration> GetConfiguration()
    {
        if (File.Exists(ConfigPath))
        {
            var configString = await File.ReadAllTextAsync(ConfigPath);
            return JsonConvert.DeserializeObject<PluginConfiguration>(configString) ?? throw new JsonSerializationException("Result was null");
        }

        var pluginConfig = new PluginConfiguration();
        await File.WriteAllTextAsync(ConfigPath, JsonConvert.SerializeObject(pluginConfig));

        return pluginConfig;
    }

    public override async Task<bool> Start()
    {
        _logger = LoggerFactory.CreateLogger<ElitePlugin>();
        _logger.LogInformation("Starting Elite plugin...");

        var pluginConfig = await GetConfiguration();

        _logger.LogInformation("Status.json Path: " + pluginConfig.StatusLocation);

        _watcher = new FileWatcher(LoggerFactory.CreateLogger<FileWatcher>(), pluginConfig.StatusLocation);

        _thread = Task.Run(() => initElitePlugin(default), default);
        return true;
    }

    private void initElitePlugin(CancellationToken ct)
    {
        if (_watcher != null) {
            _watcher.Start();
            while (!ct.IsCancellationRequested)
            {

            }
        }
    }

    public override Task Stop()
    {
        Dispose();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _thread.Dispose();
    }
}