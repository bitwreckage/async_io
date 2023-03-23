public class FileProgress
{
    public int WorkId { get; }

    public int ThreadId { get; }
    
    public int PctComplete { get; }

    public FileProgress(int workId, int threadId, int pctComplete)
    {
        WorkId = workId;
        ThreadId = threadId;
        PctComplete = pctComplete;
    }
}