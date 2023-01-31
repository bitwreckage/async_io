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
        for (int i = 0; i <= 100; i++)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(_random.Next(50,200)));
            progress.Report(new FileProgress(workNumber, i));
        }
        
        return 1;
    }
}