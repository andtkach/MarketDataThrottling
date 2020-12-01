using System.Collections.Generic;
using System.Text;

namespace MarketDataAggregator
{
    public sealed class MarketDataUpdate
    {
        public string InstrumentId { get; set; }

        public Dictionary<byte, long> Fields { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append($"Instrument: {InstrumentId}, FieldsNo: {Fields.Count}, Fields: [");
            
            foreach (var field in Fields)
            {
                builder.Append($"{field.Key}: {field.Value}, ");
            }
            
            builder.Append($"]");
            
            return builder.ToString();
        }
    }
}