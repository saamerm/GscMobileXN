using System;
using System.Threading;
using System.Threading.Tasks;

namespace MobileClaims.Core.Util
{
    public delegate void TimerCallback(object state);
    
    public class Timer
    {
        private double _interval;
        private Task task;
        private TimerCallback _cb;
        private CancellationTokenSource cancellationTokenSource;
        public Timer(TimerCallback cb, double interval)
        {
            _interval = interval;
            _cb = cb;
        }

        public void Start()
        {
            cancellationTokenSource = new CancellationTokenSource();
            task = Task.Delay(TimeSpan.FromMilliseconds(_interval), cancellationTokenSource.Token).ContinueWith((t, s) =>
            {
                var tuple = (Tuple<TimerCallback, object>)s;
                tuple.Item1(tuple.Item2);
            }, Tuple.Create<TimerCallback, object>(_cb, null), CancellationToken.None,
            TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,
                                                                                        TaskScheduler.Default);
        }


        public void Stop()
        {
            cancellationTokenSource?.Cancel();
        }

        public void Reset()
        {
            Stop();
            Start();
        }
    }
}
