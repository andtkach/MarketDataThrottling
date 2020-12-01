using System.Security;
using System.Threading;
using MarketDataAggregator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject.Behavioral
{
    [TestClass]
    public class TextOutputSetupTest
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

            var logClient = new LogClientStrategy();
            var client = new Client(logClient);

            aggregator.AddWatcher(client);
            

            // Act
            aggregator.Start();

            stream1.Start();
            stream2.Start();

            Thread.Sleep(3000);


            // Assert
            var result = logClient.Print();
            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }
    }
}
