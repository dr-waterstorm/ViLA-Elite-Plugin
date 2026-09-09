public interface IStatusTranslator
{
    /// <summary>Sends a value to ViLA, unless the same value was just sent.</summary>
    void FromStatusFile<T>(string status, T data);

    /// <summary>Forgets what was sent, so the next update sends every value again.</summary>
    void ForgetSentValues();
}
