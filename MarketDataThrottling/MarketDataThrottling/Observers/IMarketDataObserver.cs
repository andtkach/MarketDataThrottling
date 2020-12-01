namespace MarketDataAggregator
{
    public interface IMarketDataObserver
    {
        void OnUpdate(MarketDataUpdate marketDataUpdate);
    }
}