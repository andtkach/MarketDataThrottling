namespace MarketDataAggregator
{
    public interface IAggregatedMarketDataObserver
    {
        void OnUpdate(MarketDataUpdate[] marketDataUpdate);
    }
}