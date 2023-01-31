public class AggregatedProgress
{
    public int OverallProgress { get; }
    public FileProgress SpecificProgress { get; }

    public AggregatedProgress(int overallProgress, FileProgress specificProgress)
    {
        OverallProgress = overallProgress;
        SpecificProgress = specificProgress;
    }
}