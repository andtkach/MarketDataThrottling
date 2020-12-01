using System.Text;

namespace MarketDataAggregator
{
    public class LogClientStrategy : IClientStrategy
    {
        private readonly StringBuilder _log = new StringBuilder();

        public void Execute(MarketDataUpdate marketDataUpdate)
        {
            this._log.AppendLine($"{marketDataUpdate}");
        }

        public string Print()
        {
            return this._log.ToString();
        }
    }
}