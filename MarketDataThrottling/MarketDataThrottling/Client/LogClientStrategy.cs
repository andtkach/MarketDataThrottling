namespace MarketDataAggregator
{
    using System.Text;

    public class LogClientStrategy : IClientStrategy
    {
        private readonly StringBuilder log = new StringBuilder();

        public void Execute(MarketDataUpdate marketDataUpdate)
        {
            this.log.AppendLine($"{marketDataUpdate}");
        }

        public string Print()
        {
            return this.log.ToString();
        }
    }
}