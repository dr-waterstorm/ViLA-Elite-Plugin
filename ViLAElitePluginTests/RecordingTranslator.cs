namespace ViLAElitePluginTests;

/// <summary>
/// Records what the status file hands to ViLA, without the change detection of the real translator.
/// </summary>
public class RecordingTranslator : IStatusTranslator
{
    public List<KeyValuePair<string, object?>> Sent { get; } = new();

    public int ForgetCalls { get; private set; }

    public void FromStatusFile<T>(string status, T data)
    {
        Sent.Add(new KeyValuePair<string, object?>(status, data));
    }

    public void ForgetSentValues()
    {
        ForgetCalls++;
    }

    public IEnumerable<string> SentIds => Sent.Select(entry => entry.Key);

    public bool WasSent(string id) => Sent.Any(entry => entry.Key == id);

    public object? ValueOf(string id) => Sent.Last(entry => entry.Key == id).Value;
}
