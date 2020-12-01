namespace MarketDataAggregator
{
    public interface IClientStrategy
    {
        void Execute(MarketDataUpdate marketDataUpdate);
    }
}