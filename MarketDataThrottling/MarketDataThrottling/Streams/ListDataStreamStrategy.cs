namespace MarketDataAggregator
{
    using System.Collections.Generic;

    public class ListDataStreamStrategy : IDataStreamStrategy
    {
        private readonly Queue<MarketDataUpdate> queue;

        public ListDataStreamStrategy(IEnumerable<MarketDataUpdate> items)
        {
            this.queue = new Queue<MarketDataUpdate>();

            if (items == null)
            {
                return;
            }
            
            foreach (var item in items)
            {
                this.queue.Enqueue(item);
            }
        }

        public MarketDataUpdate Next()
        {
            try
            {
                return this.queue.TryDequeue(out var result) ? result : null;
            }
            catch
            {
                return null;
            }
        }
    }
}