using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class FileProcessor
{
    private Random _random = new Random(DateTime.Now.Millisecond);
    private int[] _progressPerFile;
    private IProgress<AggregatedProgress> _aggregateProgress;

    public async Task<int> ProcessFiles(int numberOfFiles,  IProgress<AggregatedProgress> progress)
    {
        _progressPerFile = new int[numberOfFiles];
        _aggregateProgress = progress;
        var detailProgress = new Progress<FileProgress>(p => AggregateProgress(p));
        var tasks = new List<Task<int>>();
        for (int i = 0; i < numberOfFiles; i++)
        {
            int x = i;
            tasks.Add( Task.Run(() => ProcessFile(x, detailProgress)));
        }

        await Task.WhenAll(tasks).ConfigureAwait(false);

        return tasks.Sum(t => t.Result);
    }

    private void AggregateProgress(FileProgress fileProgress)
    {
        if (fileProgress.WorkId < 0 || fileProgress.WorkId >= _progressPerFile.Length)
        {
            throw new InvalidOperationException($"FileProgress workId out of bounds: {fileProgress.WorkId}");
        }

        _progressPerFile[fileProgress.WorkId] = fileProgress.PctComplete;
        _aggregateProgress.Report(new AggregatedProgress(_progressPerFile.Sum()/_progressPerFile.Length, fileProgress));
    }

    private async Task<int> ProcessFile(int workNumber, IProgress<FileProgress> progress)
    {
        var totalTime = _random.Next(2, 10);
        var threadId = Environment.CurrentManagedThreadId;
        
        var iterations = 100;
        for (int i = 0; i <= iterations; i++)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(totalTime*1000/iterations)).ConfigureAwait(false);
            progress.Report(new FileProgress(workNumber, threadId, i*100/iterations));
        }
        
        return 1;
    }
}