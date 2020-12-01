namespace MarketDataAggregator
{
    using System.Collections.Generic;
    using System.Linq;

    public class DefaultSliceAggregator
    {
        private readonly Dictionary<string, Dictionary<byte, long>> data;

        public DefaultSliceAggregator()
        {
            this.data = new Dictionary<string, Dictionary<byte, long>>();
        }

        public IDictionary<string, Dictionary<byte, long>> Data => this.data;

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
            this.data.Add(item.InstrumentId, item.Fields);
        }

        private bool HasInstrument(string instrumentId)
        {
            return this.data.ContainsKey(instrumentId);
        }

        private void Update(MarketDataUpdate item)
        {
            var existingItem = this.data[item.InstrumentId];

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