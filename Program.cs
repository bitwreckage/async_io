
// See https://aka.ms/new-console-template for more information

using System.Collections.Concurrent;

Console.Clear();
Console.CursorVisible = false;

using BlockingCollection<AggregatedProgress> progressEvents = new BlockingCollection<AggregatedProgress>();
{
    var timer = new Timer(ProcessReports, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(200));

    var processor = new FileProcessor();
    var progress = new ConsoleProgress<AggregatedProgress>(p => ReportProgress(p));
    int result = await processor.ProcessFiles(10, progress);

    await Task.Delay(TimeSpan.FromMilliseconds(200));
    Console.SetCursorPosition(0,17);
    Console.WriteLine($"Result of files processed: {result}");
    Console.CursorVisible = true;
}


void ReportProgress(AggregatedProgress aggregatedProgress)
{
    progressEvents.Add(aggregatedProgress);
}

void ProcessReports(Object whatever)
{
    while (progressEvents.TryTake(out var progress))
    {
        ProcessReport(progress);
    }
}

void ProcessReport(AggregatedProgress progress)
{
    var specProgress = progress.SpecificProgress;
    Console.SetCursorPosition(0, 5 + specProgress.WorkId);
    Console.Write($"{specProgress.WorkId} {specProgress.PctComplete:000}%: ");
    WriteProgressBar(specProgress.PctComplete);

    Console.SetCursorPosition(0, 5+11);
    Console.Write($"Overall {progress.OverallProgress:000}%: ");
    WriteProgressBar(progress.OverallProgress);
}

void WriteProgressBar(int pctComplete)
{
    var progressChar = pctComplete < 100 ? "-" : "#";
    for (int i = 0; i < pctComplete / 10; i++)
    {
        Console.Write(progressChar);
    }

    if (pctComplete < 100)
    {
        Console.Write(">");
    }
}
