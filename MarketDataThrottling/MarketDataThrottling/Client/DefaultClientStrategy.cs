namespace MarketDataAggregator
{
    using System;

    public class DefaultClientStrategy : IClientStrategy
    {
        public void Execute(MarketDataUpdate marketDataUpdate)
        {
            Console.WriteLine(marketDataUpdate);
        }
    }
}