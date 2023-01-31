using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace async_io;

public class FilesDownloadViewModel : List<FileProgressViewModel>, INotifyPropertyChanged
{
    private bool _isIdle = true;
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void Notify([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public bool IsIdle
    {
        get => _isIdle;
        set
        {
            if (value == _isIdle) return;
            _isIdle = value;
            Notify();
        }
    }
}