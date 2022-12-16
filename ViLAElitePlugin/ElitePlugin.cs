using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ViLA.PluginBase;

public class ElitePlugin : PluginBase, IDisposable
{
    public const string ConfigPath = "Plugins/ViLAElitePlugin/config.json";
    private ILogger<ElitePlugin> ?_logger;
    private IStatusFileWatcher ?_watcher;
    
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

        _watcher = new StatusFileWatcher(LoggerFactory.CreateLogger<StatusFileWatcher>(), pluginConfig.StatusLocation, new Translator(LoggerFactory.CreateLogger<Translator>(), Send, this.ClearState));

        _watcher.Start();
        return true;
    }

    public override Task Stop()
    {
        Dispose();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        if (_watcher != null) {
            _watcher.Dispose();
        }
    }
}