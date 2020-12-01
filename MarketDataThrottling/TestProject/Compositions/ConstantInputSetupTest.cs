using System.Collections.Generic;
using System.Threading;
using MarketDataAggregator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject.Compositions
{

    [TestClass]
    public class ConstantInputSetupTest
    {
        [TestMethod]
        public void Run()
        {
            // Arrange 
            var expectedInput1 = new List<MarketDataUpdate>();
            expectedInput1.Add(new MarketDataUpdate { InstrumentId = "AAPL_1", Fields = new Dictionary<byte, long>{ { 1, 10 }, { 4, 200 }, { 12, 187 } } });
            expectedInput1.Add(new MarketDataUpdate { InstrumentId = "AAPL_2", Fields = new Dictionary<byte, long>{ { 1, 12 }, { 4, 210 } } });
            expectedInput1.Add(new MarketDataUpdate { InstrumentId = "AAPL_1", Fields = new Dictionary<byte, long>{ { 12, 189 } } });
            expectedInput1.Add(new MarketDataUpdate { InstrumentId = "AAPL_1", Fields = new Dictionary<byte, long>{ { 2, 24 } } });
            expectedInput1.Add(new MarketDataUpdate { InstrumentId = "AAPL_2", Fields = new Dictionary<byte, long>{ { 5, 120 } } });
            
            string expected1 = "[1: 10, 4: 200, 12: 189, 2: 24]";
            string expected2 = "[1: 12, 4: 210, 5: 120]";

            var stream1 = new MarketDataStream(new ListDataStreamStrategy(expectedInput1));

            var aggregator = new ThrottledMarketDataStream(300);

            stream1.AddWatcher(aggregator);

            var logClient = new LogClientStrategy();
            var client = new Client(logClient);

            aggregator.AddWatcher(client);
            

            // Act
            aggregator.Start();

            stream1.Start();

            Thread.Sleep(2000);


            // Assert
            var result = logClient.Print();
            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result));

            Assert.IsTrue(result.Contains(expected1), $"Wrong result \n\tExpected to contain: \t{expected1}\n\tActual: \t{result}\n");

            Assert.IsTrue(result.Contains(expected2), $"Wrong result \n\tExpected to contain: \t{expected2}\n\tActual: \t{result}\n");
        }
    }
}
