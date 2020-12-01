using System.Threading;
using MarketDataAggregator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject.Behavioral
{
    [TestClass]
    public class DefaultSetupTest
    {
        [TestMethod]
        public void Run()
        {
            // Arrange

            var stream1 = new MarketDataStream();
            var stream2 = new MarketDataStream();

            var aggregator = new ThrottledMarketDataStream();

            stream1.AddWatcher(aggregator);
            stream2.AddWatcher(aggregator);

            var client = new Client();
            aggregator.AddWatcher(client);

            
            // Act
            aggregator.Start();
            stream1.Start();
            stream2.Start();

            Thread.Sleep(3000);


            // Assert
        }
    }
}
