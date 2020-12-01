using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MarketDataAggregator
{
    public class MarketDataStream : IMarketDataStream
    {
        private readonly List<IMarketDataObserver> _watchers;
        private readonly Random _randomizer;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken;

        public MarketDataStream()
        {
            _watchers = new List<IMarketDataObserver>();
            _randomizer = new Random();
            _cancellationTokenSource = new CancellationTokenSource();
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
                    while (true)
                    {
                        if (_cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }

                        Thread.Sleep(1);
                        
                        var update = new MarketDataUpdate();
                        update.InstrumentId = "AAPL_" + _randomizer.Next(1, 100);
                        update.Fields = new Dictionary<byte, long>();
                 
                        for (int i = 0; i < _randomizer.Next(1, 5); i++)
                        {
                            update.Fields[(byte)_randomizer.Next(1, 20)] = _randomizer.Next(1, 10000);
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
