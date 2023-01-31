public class ConsoleProgress<T> : IProgress<T>
{
    private Action<T> _handler;
    
    public ConsoleProgress(Action<T> handler)
    {
        _handler = handler;
    }

    public void Report(T value)
    {
        _handler(value);
    }
}