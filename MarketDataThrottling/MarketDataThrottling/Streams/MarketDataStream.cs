namespace MarketDataAggregator
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class MarketDataStream : IMarketDataStream
    {
        private readonly List<IMarketDataObserver> watchers;
        private readonly CancellationTokenSource cancellationTokenSource;
        private readonly IDataStreamStrategy dataStreamStrategy;
        private CancellationToken cancellationToken;

        public MarketDataStream() : this(new RandomDataStreamStrategy())
        {
        }

        public MarketDataStream(IDataStreamStrategy dataStreamStrategy)
        {
            this.dataStreamStrategy = dataStreamStrategy;
            this.watchers = new List<IMarketDataObserver>();
            this.cancellationTokenSource = new CancellationTokenSource();
        }

        public void AddWatcher(IMarketDataObserver watcher)
        {
            this.watchers.Add(watcher);
        }

        public void RemoveWatcher(IMarketDataObserver watcher)
        {
            this.watchers.Remove(watcher);
        }

        public void End()
        {
            this.cancellationTokenSource.Cancel();
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

                    Thread.Sleep(1);

                    var update = this.dataStreamStrategy.Next();

                    if (update == null)
                    {
                        continue;
                    }
                    
                    foreach (var watcher in watchers)
                    {
                        watcher.OnUpdate(update);
                    }
                }
            }, 
                this.cancellationToken);
        }
    }
}
