using Microsoft.Extensions.Logging;

public class Translator : IStatusTranslator
{
    /// <summary>
    /// How long a value is trusted to still be known to ViLA. ViLA keeps one state dictionary for
    /// all plugins and any plugin can clear it, so a value is sent again from time to time even
    /// when it did not change.
    /// </summary>
    private static readonly TimeSpan DefaultResendInterval = TimeSpan.FromSeconds(30);

    private readonly ILogger<Translator> _logger;

    private readonly Action<string, dynamic>? _onDataReceive;

    private readonly TimeSpan _resendInterval;

    // ViLA sends the LED command to the device every time a value arrives, whether it changed or
    // not. Status.json is rewritten roughly every second, so forwarding everything would keep the
    // USB connection busy with commands that change nothing.
    private readonly Dictionary<string, (object Value, DateTime SentAt)> _lastSent = new();

    public Translator(ILogger<Translator> logger, Action<string, dynamic>? onDataReceive, TimeSpan? resendInterval = null)
    {
        _onDataReceive = onDataReceive;
        _logger = logger;
        _resendInterval = resendInterval ?? DefaultResendInterval;
    }

    public void FromStatusFile<T>(string status, T data)
    {
        if (data is null)
        {
            return;
        }

        if (_lastSent.TryGetValue(status, out var lastSent)
            && lastSent.Value.Equals(data)
            && DateTime.UtcNow - lastSent.SentAt < _resendInterval)
        {
            return;
        }

        _logger.LogTrace("Got new data {Data} from status {Status}", data, status);

        try
        {
            _onDataReceive?.Invoke(status, data);
        }
        catch (Exception ex)
        {
            // ViLA evaluates the conditions and writes to the device inside this call and does not
            // handle errors itself. One broken LED must not stop the remaining values of this
            // update, and the value must not be remembered as sent.
            _logger.LogWarning("ViLA could not handle {Status} = {Data}: {Message}", status, data, ex.Message);
            return;
        }

        // only after ViLA actually took it
        _lastSent[status] = (data, DateTime.UtcNow);
    }

    public void ForgetSentValues()
    {
        _logger.LogDebug("Forgetting {Count} sent values", _lastSent.Count);

        _lastSent.Clear();
    }
}
