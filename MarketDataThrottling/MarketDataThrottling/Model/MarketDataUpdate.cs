using System.Collections.Generic;
using System.Text;

namespace MarketDataAggregator
{
    public sealed class MarketDataUpdate
    {
        public string InstrumentId { get; set; }

        public Dictionary<byte, long> Fields { get; set; }

        public MarketDataUpdate()
        {
        }

        public MarketDataUpdate(string instrumentId, IDictionary<byte, long> fields)
        {
            this.InstrumentId = instrumentId;
            this.Fields = new Dictionary<byte, long>(fields);

        }

        public override string ToString()
        {
            bool needCleanEmptyComma = false;

            StringBuilder builder = new StringBuilder();

            builder.Append($"Instrument: {InstrumentId}, FieldsNo: {Fields.Count}, Fields: [");
            
            foreach (var field in Fields)
            {
                needCleanEmptyComma = true;
                builder.Append($"{field.Key}: {field.Value}, ");
            }

            if (needCleanEmptyComma)
            {
                builder.Remove(builder.Length - 2, 2);
            }

            builder.Append($"]");
            
            return builder.ToString();
        }
    }
}