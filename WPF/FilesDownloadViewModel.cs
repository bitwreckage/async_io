using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace async_io;

public class FilesDownloadViewModel : List<FileProgressViewModel>, INotifyPropertyChanged
{
    private bool _isIdle = true;
    private int _overallProgress;
    private bool _processOngoing;
    private bool _processComplete;
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

    public bool ProcessOngoing
    {
        get => _processOngoing;
        set
        {
            if (value == _processOngoing) return;
            _processOngoing = value;
            Notify();
        }
    }

    public bool ProcessComplete
    {
        get => _processComplete;
        set
        {
            if (value == _processComplete) return;
            _processComplete = value;
            Notify();
        }
    }

    public int OverallProgress
    {
        get => _overallProgress;
        set
        {
            if (value == _overallProgress) return;
            _overallProgress = value;
            Notify();
        }
    }

    public void StartDownload()
    {
        IsIdle = false;
        ProcessOngoing = true;
        ProcessComplete = false;

        foreach (var fileProgress in this)
        {
            fileProgress.Progress = 0;
        }

        OverallProgress = 0;
    }
}