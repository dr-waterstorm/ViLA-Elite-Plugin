public interface IStatusFileWatcher : IDisposable
{
    public void Start();

    public void Stop();
}
