namespace MarketDataAggregator
{
    public interface IMarketDataStream
    {
        void AddWatcher(IMarketDataObserver watcher);

        void RemoveWatcher(IMarketDataObserver watcher);

        void Start();

        void End();
    }
}