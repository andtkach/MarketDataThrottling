using System;

namespace MarketDataAggregator
{
    public class DefaultClientStrategy : IClientStrategy
    {
        public void Execute(MarketDataUpdate marketDataUpdate)
        {
            Console.WriteLine(marketDataUpdate);
        }
    }
}