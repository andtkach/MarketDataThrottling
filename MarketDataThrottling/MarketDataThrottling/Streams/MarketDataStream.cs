namespace MarketDataAggregator
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class MarketDataStream : IMarketDataStream
    {
        private readonly List<IMarketDataObserver> _watchers;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IDataStreamStrategy _dataStreamStrategy;
        private CancellationToken _cancellationToken;

        public MarketDataStream() : this(new RandomDataStreamStrategy())
        {
        }

        public MarketDataStream(IDataStreamStrategy dataStreamStrategy)
        {
            this._dataStreamStrategy = dataStreamStrategy;
            this._watchers = new List<IMarketDataObserver>();
            this._cancellationTokenSource = new CancellationTokenSource();
        }

        public void AddWatcher(IMarketDataObserver watcher)
        {
            _watchers.Add(watcher);
        }

        public void RemoveWatcher(IMarketDataObserver watcher)
        {
            _watchers.Remove(watcher);
        }

        public void End()
        {
            _cancellationTokenSource.Cancel();
        }

        public void Start()
        {
            _cancellationToken = _cancellationTokenSource.Token;

            Task.Run(() =>
            {
                var ct = _cancellationToken;

                while (true)
                {
                    if (ct.IsCancellationRequested)
                    {
                        break;
                    }

                    Thread.Sleep(1);

                    var update = this._dataStreamStrategy.Next();

                    if (update == null)
                    {
                        continue;
                    }
                    
                    foreach (var watcher in _watchers)
                    {
                        watcher.OnUpdate(update);
                    }
                }
            }, _cancellationToken);
        }
    }
}
