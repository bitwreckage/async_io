public class FileProgress
{
    public int WorkId { get; }
    public int PctComplete { get; }

    public FileProgress(int workId, int pctComplete)
    {
        WorkId = workId;
        PctComplete = pctComplete;
    }
}