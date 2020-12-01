using System.Security;
using System.Threading;
using MarketDataAggregator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject.Behavioral
{
    [TestClass]
    public class ConstantInputSetupTest
    {
        [TestMethod]
        public void Run()
        {
            // Arrange 

            string expected1 = "AAPL_1 [1: 10, 4: 200, 12: 189, 2: 24]";
            string expected2 = "AAPL_2 [1: 12, 4: 210, 5: 120]";

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

            Assert.IsTrue(result.Contains(expected1));
            Assert.IsTrue(result.Contains(expected2));
        }
    }
}
