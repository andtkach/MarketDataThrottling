using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MarketDataAggregator
{
    public class ThrottledMarketDataStream : IMarketDataObserver, IThrottledMarketDataStream
    {
        private readonly List<IAggregatedMarketDataObserver> _watchers = new List<IAggregatedMarketDataObserver>();
        
        private readonly ConcurrentQueue<MarketDataUpdate> _buffer = new ConcurrentQueue<MarketDataUpdate>();

        private readonly int _sleepPeriod;

        private readonly CancellationTokenSource _cancellationTokenSource;

        private CancellationToken _cancellationToken;

        public ThrottledMarketDataStream(int sleep)
        {
            this._sleepPeriod = sleep;
            this._cancellationTokenSource = new CancellationTokenSource();
        }

        public ThrottledMarketDataStream() : this(200)
        {
        }

        public void AddWatcher(IAggregatedMarketDataObserver watcher)
        {
            _watchers.Add(watcher);
        }

        public void RemoveWatcher(IAggregatedMarketDataObserver watcher)
        {
            _watchers.Remove(watcher);
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

                    Thread.Sleep(this._sleepPeriod);

                    var slice = new Slice();
                    
                    while (_buffer.TryDequeue(out var item))
                    {
                        slice.AddItem(item);
                    }

                    this.PublishSlice(slice);
                    
                }
            }, _cancellationToken);
        }

        private void PublishSlice(Slice slice)
        {
            if (!slice.Any())
            {
                return;
            }
            
            foreach (var (key, value) in slice.Data)
            {
                var itemToSend = new MarketDataUpdate(key, value);
                
                foreach (var watcher in _watchers)
                {
                    watcher.OnUpdate(new[] { itemToSend });
                }
            }
        }

        public void End()
        {
            _cancellationTokenSource.Cancel();
        }

        public void OnUpdate(MarketDataUpdate marketDataUpdate)
        {
            this._buffer.Enqueue(marketDataUpdate);
        }
    }

    public class Slice
    {
        private readonly Dictionary<string, Dictionary<byte, long>> _data;

        public Slice()
        {
            this._data = new Dictionary<string, Dictionary<byte, long>>();
        }

        public bool Any()
        {
            return this._data.Any();
        }

        public IDictionary<string, Dictionary<byte, long>> Data => this._data;

        public void AddItem(MarketDataUpdate item)
        {
            if (this.HasInstrument(item.InstrumentId))
            {
                this.Update(item);
            }
            else
            {
                this.AddNew(item);
            }
        }

        private void AddNew(MarketDataUpdate item)
        {
            this._data.Add(item.InstrumentId, item.Fields);
        }

        private bool HasInstrument(string instrumentId)
        {
            return this._data.ContainsKey(instrumentId);
        }

        private void Update(MarketDataUpdate item)
        {
            var existingItem = this._data[item.InstrumentId];

            foreach (var field in item.Fields)
            {
                if (existingItem.ContainsKey(field.Key))
                {
                    existingItem[field.Key] = field.Value;
                }
                else
                {
                    existingItem.Add(field.Key, field.Value);
                }
            }
        }
    }
}