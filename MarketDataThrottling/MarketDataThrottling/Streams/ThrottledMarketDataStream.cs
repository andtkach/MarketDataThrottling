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
            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(200);

                    Dictionary<string, Dictionary<byte, long>> slice = new Dictionary<string, Dictionary<byte, long>>();
                    MarketDataUpdate item;
                    
                    while (_buffer.TryDequeue(out item))
                    {
                        if (slice.ContainsKey(item.InstrumentId))
                        {
                            var existingItem = slice[item.InstrumentId];

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
                        else
                        {
                            slice.Add(item.InstrumentId, item.Fields);
                        }

                    }

                    if (slice.Any())
                    {
                        foreach (var itemInSlice in slice)
                        {
                            var itemToSend = new MarketDataUpdate
                            {
                                InstrumentId = itemInSlice.Key,
                                Fields = itemInSlice.Value
                            };

                            foreach (var watcher in _watchers)
                            {
                                watcher.OnUpdate(new[] { itemToSend });
                            }
                        }
                    }
                }
            });
        }

        public void End()
        {
            throw new NotImplementedException();
        }

        public void OnUpdate(MarketDataUpdate marketDataUpdate)
        {

            this._buffer.Enqueue(marketDataUpdate);

            //foreach (var watcher in _watchers)
            //{
            //    watcher.OnUpdate(new[] { marketDataUpdate });
            //}
        }
    }
}