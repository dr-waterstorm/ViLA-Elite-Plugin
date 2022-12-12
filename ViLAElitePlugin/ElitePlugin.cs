using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ViLA.PluginBase;

public class ElitePlugin : PluginBase, IDisposable
{
    public const string ConfigPath = "Plugins/ViLAElitePlugin/config.json";

    private Task _thread = null!;

    public override async Task<bool> Start()
    {
        var log = LoggerFactory.CreateLogger<ElitePlugin>();
        log.LogInformation("Starting Elite plugin...");

        var cfg = await GetConfiguration();

        _thread = Task.Run(() => TestInit(), default);
        return true;
    }

    public override Task Stop()
    {
        Dispose();
        return Task.CompletedTask;
    }
    
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

    private void TestInit()
    {
        var log = LoggerFactory.CreateLogger<ElitePlugin>();
        log.LogInformation("Init...");
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _thread.Dispose();
    }
}