namespace MarketDataAggregator
{
    public class Client : IAggregatedMarketDataObserver
    {
        private readonly IClientStrategy clientStrategy;

        public Client()
        {
            this.clientStrategy = new DefaultClientStrategy();
        }

        public Client(IClientStrategy desiredClientStrategy)
        {
            this.clientStrategy = desiredClientStrategy;
        }

        public void OnUpdate(MarketDataUpdate[] marketDataUpdates)
        {
            if (this.clientStrategy == null)
            {
                return;
            }
            
            foreach (var marketDataUpdate in marketDataUpdates)
            {
                this.clientStrategy.Execute(marketDataUpdate);
            }
        }
    }
}