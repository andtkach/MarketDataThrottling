namespace MarketDataAggregator
{
    internal interface IThrottledMarketDataStream
    {
        void AddWatcher(IAggregatedMarketDataObserver watcher);

        void RemoveWatcher(IAggregatedMarketDataObserver watcher);

        void Start();

        void End();
    }
}