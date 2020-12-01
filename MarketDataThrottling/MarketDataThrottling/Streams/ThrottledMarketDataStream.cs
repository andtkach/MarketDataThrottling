namespace MarketDataAggregator
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class ThrottledMarketDataStream : IMarketDataObserver, IThrottledMarketDataStream
    {
        private readonly List<IAggregatedMarketDataObserver> watchers = new List<IAggregatedMarketDataObserver>();
        
        private readonly ConcurrentQueue<MarketDataUpdate> buffer = new ConcurrentQueue<MarketDataUpdate>();

        private readonly int sleepPeriod;

        private readonly CancellationTokenSource cancellationTokenSource;

        private CancellationToken cancellationToken;

        public ThrottledMarketDataStream(int sleep)
        {
            this.sleepPeriod = sleep;
            this.cancellationTokenSource = new CancellationTokenSource();
        }

        public ThrottledMarketDataStream() : this(200)
        {
        }

        public void AddWatcher(IAggregatedMarketDataObserver watcher)
        {
            this.watchers.Add(watcher);
        }

        public void RemoveWatcher(IAggregatedMarketDataObserver watcher)
        {
            this.watchers.Remove(watcher);
        }

        public void Start()
        {
            this.cancellationToken = this.cancellationTokenSource.Token;

            Task.Run(
                () =>
            {
                var ct = this.cancellationToken;

                while (true)
                {
                    if (ct.IsCancellationRequested)
                    {
                        break;
                    }

                    Thread.Sleep(this.sleepPeriod);

                    var slice = new DefaultSliceAggregator();
                    
                    while (this.buffer.TryDequeue(out var item))
                    {
                        slice.AddItem(item);
                    }

                    this.PublishSlice(slice.Data);
                }
            }, 
                this.cancellationToken);
        }

        public void End()
        {
            this.cancellationTokenSource.Cancel();
        }

        public void OnUpdate(MarketDataUpdate marketDataUpdate)
        {
            this.buffer.Enqueue(marketDataUpdate);
        }

        private void PublishSlice(IDictionary<string, Dictionary<byte, long>> data)
        {
            if (data == null)
            {
                return;
            }

            foreach (var (key, value) in data)
            {
                var itemToSend = new MarketDataUpdate(key, value);

                foreach (var watcher in this.watchers)
                {
                    watcher.OnUpdate(new[] { itemToSend });
                }
            }
        }
    }
}