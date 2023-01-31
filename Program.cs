
// See https://aka.ms/new-console-template for more information

using System.Collections.Concurrent;

Console.Clear();
Console.CursorVisible = false;

using BlockingCollection<FileProgress> progressEvents = new BlockingCollection<FileProgress>();
{
    var timer = new Timer(ProcessReports, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(200));

    var processor = new FileProcessor();
    var progress = new ConsoleProgress<FileProgress>(p => ReportProgress(p));
    int result = await processor.ProcessFiles(10, progress);

    await Task.Delay(TimeSpan.FromMilliseconds(200));
    Console.SetCursorPosition(0,17);
    Console.WriteLine($"Result of files processed: {result}");
    Console.CursorVisible = true;
}


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
    Console.Write($"{progress.WorkId} {progress.PctComplete:000}%: ");
    var progressChar = progress.PctComplete < 100 ? "-" : "#";
    for (int i = 0; i < progress.PctComplete / 10; i++)
    {
        Console.Write(progressChar);
    }

    if (progress.PctComplete < 100)
    {
        Console.Write(">");
    }
}
