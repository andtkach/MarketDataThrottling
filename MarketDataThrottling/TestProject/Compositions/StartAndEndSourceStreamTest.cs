namespace TestProject.Compositions
{
    using System.Threading;
    using MarketDataAggregator;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StartAndEndSourceStreamTest
    {
        [TestMethod]
        public void Run()
        {
            //// Arrange

            var stream1 = new MarketDataStream();

            var aggregator = new ThrottledMarketDataStream();

            stream1.AddWatcher(aggregator);

            var client = new Client();
            aggregator.AddWatcher(client);

            //// Act
            aggregator.Start();
            stream1.Start();

            Thread.Sleep(3000);

            stream1.End();

            Thread.Sleep(3000);
            
            //// Assert
        }
    }
}
