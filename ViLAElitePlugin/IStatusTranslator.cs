public interface IStatusTranslator
{
    void FromStatusFile<T>(string status, T data);
}