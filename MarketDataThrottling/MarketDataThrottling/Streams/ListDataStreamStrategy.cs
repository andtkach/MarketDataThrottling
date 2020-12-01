using System.Collections.Generic;

namespace MarketDataAggregator
{
    public class ListDataStreamStrategy : IDataStreamStrategy
    {
        private readonly Queue<MarketDataUpdate> _queue;

        public ListDataStreamStrategy(IEnumerable<MarketDataUpdate> items)
        {
            this._queue = new Queue<MarketDataUpdate>();

            if (items == null)
            {
                return;
            }
            
            foreach (var item in items)
            {
                this._queue.Enqueue(item);
            }
        }

        public MarketDataUpdate Next()
        {
            try
            {
                return _queue.TryDequeue(out var result) ? result : null;
            }
            catch
            {
                return null;
            }
            
        }
    }
}