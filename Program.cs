
// See https://aka.ms/new-console-template for more information

using System.Collections.Concurrent;

Console.Clear();

// var workIds = new int[]
// {
//     5, 3, 9, 6, 2, 8, 7, 1, 0, 4
// };
//
// var rand = new Random();
// for (int j = 0; j < 5; j++)
// {
//     for (int i = 0; i < 10; i++)
//     {
//         var pct = rand.Next(0, 100);
//         var pr = new FileProgress(workIds[i], pct);
//         ReportProgress(pr);
//     }
// }

BlockingCollection<FileProgress> progressEvents = new BlockingCollection<FileProgress>();

var timer = new Timer(ProcessReports, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(500));

var processor = new FileProcessor();
var progress = new ConsoleProgress<FileProgress>(p => ReportProgress(p));
int result = await processor.ProcessFiles(10, progress);

await Task.Delay(TimeSpan.FromMilliseconds(200));

Console.SetCursorPosition(0,17);
Console.WriteLine($"Result of files processed: {result}");


void ReportProgress(FileProgress fileProgress)
{
    progressEvents.Add(fileProgress);
}

void ProcessReports(Object whatever)
{
    while (progressEvents.TryTake(out var progress))
    {
        ProcessReport(progress);
    }
}

void ProcessReport(FileProgress progress)
{
    Console.SetCursorPosition(0, 5 + progress.WorkId);
    Console.Write($"{progress.WorkId}:");
    for (int i = 0; i < progress.PctComplete / 10; i++)
    {
        Console.Write(".");
    }

    Console.Write($" {progress.PctComplete}% done");

    // Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] {fileProgress.WorkId}: {fileProgress.PctComplete}% done");
}
