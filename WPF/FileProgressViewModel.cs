using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace async_io;

public class FileProgressViewModel : INotifyPropertyChanged
{
    private int _progress;
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void Notify([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public int Progress
    {
        get => _progress;
        set
        {
            if (value == _progress) return;
            _progress = value;
            Notify();
        }
    }
}