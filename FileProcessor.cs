public class FileProcessor
{
    private Random _random = new Random(DateTime.Now.Millisecond);
    
    public async Task<int> ProcessFiles(int numberOfFiles,  IProgress<FileProgress> progress)
    {
        var tasks = new List<Task<int>>();
        for (int i = 0; i < numberOfFiles; i++)
        {
            tasks.Add(ProcessFile(i, progress));
        }

        await Task.WhenAll(tasks);

        return tasks.Sum(t => t.Result);
    }

    private async Task<int> ProcessFile(int workNumber, IProgress<FileProgress> progress)
    {
        var totalTime = _random.Next(2, 10);

        var iterations = 100;
        for (int i = 0; i <= iterations; i++)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(totalTime*1000/iterations));
            progress.Report(new FileProgress(workNumber, i*100/iterations));
        }
        
        return 1;
    }
}