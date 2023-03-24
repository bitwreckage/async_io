using System;
using System.Reactive.Linq;
using System.Threading;

namespace async_io
{
    public static class ThrottlingProgress
    {
        /// <summary>
        /// Call this Create method to setup an IProgress{T} --> IObservable{T} "channel".
        /// The idea is that your long-running background task calls the progressReporter.Report()
        /// method, and the progress report will then be available on the observeMe IObservable{T} instance.
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Tuple of IObservable{T} and IProgress{T}</returns>
        public static (IObservable<T> observeMe, IProgress<T> progressReporter) Create<T>()
        {
            var progress = new EventProgress<T>();
            var observable = Observable.FromEvent<T>(
                handler => progress.OnReport += handler,
                handler => progress.OnReport -= handler);
            return (observable, progress);
        }

        // Note: this must be called from the UI thread.
        public static (IObservable<T> observeMe, IProgress<T> progressReporter) CreateForUi<T>(TimeSpan? sampleInterval = null)
        {
            var (observable, progress) = Create<T>();
            observable = observable
                .Sample(sampleInterval ?? TimeSpan.FromMilliseconds(100))
                .ObserveOn(SynchronizationContext.Current);
            return (observable, progress);
        }

        private sealed class EventProgress<T> : IProgress<T>
        {
            public event Action<T> OnReport;
            void IProgress<T>.Report(T value) => this.OnReport?.Invoke(value);
        }
    }
}
