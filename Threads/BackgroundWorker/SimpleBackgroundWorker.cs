namespace Lab11_task2;

public class SimpleBackgroundWorker<T> : IBackgroundWorker
{
    // TODO: Implement SimpleBackgroundWorker here
    private Thread? workerThread;
    private CancellationTokenSource? cancellationTokenSource;
    private Action<IBackgroundWorker> doWorkAction;
    private int progress;
    private string name;
    private WorkerStatus status;
    private ManualResetEvent workerReadyEvent;

    public string Name => name;
    public WorkerStatus Status => status;
    public bool IsCancellationRequested => cancellationTokenSource?.Token.IsCancellationRequested ?? false;

    public event EventHandler<int>? ProgressChanged;
    public event EventHandler? WorkerStatusChanged;

    public SimpleBackgroundWorker()
    {
        name = $"Thread_{Thread.CurrentThread.ManagedThreadId}";
        status = WorkerStatus.NotStarted;
        workerReadyEvent = new ManualResetEvent(false);
    }

    public void SetDoWorkAction(Action<IBackgroundWorker> workAction)
    {
        doWorkAction = workAction;
    }
    public void RunWorkerAsync(string? workerName = null)
    {
        if (workerName != null)
        {
            name = workerName;
        }

        if (doWorkAction == null)
        {
            throw new InvalidOperationException("action is not set.");
        }

        status = WorkerStatus.IsRunning;
        cancellationTokenSource = new CancellationTokenSource();
        workerThread = new Thread(WorkerThreadProc)
        {
            IsBackground = true,  
            Name = name          
        };
        workerThread.Start();
    }
    private void WorkerThreadProc()
    {
        try
        {
            workerReadyEvent.WaitOne();
            doWorkAction?.Invoke(this);
            status = WorkerStatus.Completed;
        }
        catch(WorkerCanceled)
        {
            status = WorkerStatus.Canceled;
        }
        catch(Exception)
        {
            status = WorkerStatus.Failed;
        }
        finally
        {
            WorkerStatusChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Cancel()
    {
        cancellationTokenSource?.Cancel();
    }
    public void ReportProgress(int progress)
    {
        if(progress >= 100)
        {
            workerReadyEvent.Set();
        }
        ProgressChanged?.Invoke(this, progress);
    }
    public void SignalWorkerStart()
    {
        workerReadyEvent.Set();
    }
}