namespace TestProject.Compositions
{
    using System.Collections.Generic;
    using System.Threading;
    using MarketDataAggregator;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LoadSetupTest
    {
        [TestMethod]
        public void Run()
        {
            //// Arrange

            var aggregator = new ThrottledMarketDataStream();
            var streams = new List<MarketDataStream>();
            var client = new Client();

            for (var i = 0; i < 1000; i++)
            {
                var stream = new MarketDataStream();
                stream.AddWatcher(aggregator);
                streams.Add(stream);
            }
            
            aggregator.AddWatcher(client);
            
            //// Act
            aggregator.Start();

            foreach (var stream in streams)
            {
                stream.Start();
            }

            Thread.Sleep(10000);

            foreach (var stream in streams)
            {
                stream.End();
            }

            aggregator.End();

            //// Assert
        }
    }
}
