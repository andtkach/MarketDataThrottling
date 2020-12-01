namespace MarketDataAggregator
{
    public class Client : IAggregatedMarketDataObserver
    {
        private readonly IClientStrategy _clientStrategy;

        public Client()
        {
            this._clientStrategy = new DefaultClientStrategy();
        }

        public Client(IClientStrategy desiredClientStrategy)
        {
            this._clientStrategy = desiredClientStrategy;
        }

        public void OnUpdate(MarketDataUpdate[] marketDataUpdates)
        {
            if (this._clientStrategy == null)
            {
                return;
            }
            
            foreach (var marketDataUpdate in marketDataUpdates)
            {
                this._clientStrategy.Execute(marketDataUpdate);
            }
        }
    }
}