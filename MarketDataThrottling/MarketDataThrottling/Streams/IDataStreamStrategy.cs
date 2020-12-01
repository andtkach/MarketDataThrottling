namespace MarketDataAggregator
{
    public interface IDataStreamStrategy
    {
        MarketDataUpdate Next();
    }
}