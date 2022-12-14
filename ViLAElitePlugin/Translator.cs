using Microsoft.Extensions.Logging;

public class Translator : IStatusTranslator
{
    private readonly ILogger<Translator> _logger;

    private readonly Action<string, dynamic>? _onDataReceive;

    private readonly Action _clearStateAction;

    public Translator(ILogger<Translator> logger, Action<string, dynamic>? onDataReceive, Action clearStateAction)
    {
        _onDataReceive = onDataReceive;
        _logger = logger;
        _clearStateAction = clearStateAction;
    }

    public void FromStatusFile<T>(string status, T data)
    {
        _logger.LogTrace($"Got data {data} from status {status}");

        if (data != null)
        {
            _onDataReceive?.Invoke(status, data);
        }
    }
}