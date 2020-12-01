namespace TestProject
{
    using System.Collections.Generic;
    using System.Linq;
    using MarketDataAggregator;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AggregatorTests
    {
        [TestMethod]
        public void CheckToString_ValidOutput()
        {
            //// Arrange
            var input = new List<MarketDataUpdate>
            {
                new MarketDataUpdate
                {
                    InstrumentId = "AAPL_1", Fields = new Dictionary<byte, long> { { 1, 10 }, { 4, 200 }, { 12, 187 } }
                },
                new MarketDataUpdate
                {
                    InstrumentId = "AAPL_2", Fields = new Dictionary<byte, long> { { 1, 12 }, { 4, 210 } }
                },
                new MarketDataUpdate
                {
                    InstrumentId = "AAPL_1", Fields = new Dictionary<byte, long> { { 12, 189 } }
                },
                new MarketDataUpdate
                {
                    InstrumentId = "AAPL_1", Fields = new Dictionary<byte, long> { { 2, 24 } }
                },
                new MarketDataUpdate
                {
                    InstrumentId = "AAPL_2", Fields = new Dictionary<byte, long> { { 5, 120 } }
                }
            };

            var slice = new DefaultSliceAggregator();
            
            //// Act

            foreach (var item in input)
            {
                slice.AddItem(item);   
            }

            //// Assert
            var result = slice.Data;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.Count == 2);

            var r1 = result["AAPL_1"];
            Assert.IsTrue(r1.Count == 4);

            Assert.IsTrue(r1[1] == 10);
            Assert.IsTrue(r1[4] == 200);
            Assert.IsTrue(r1[12] == 189);
            Assert.IsTrue(r1[2] == 24);

            var r2 = result["AAPL_2"];
            Assert.IsTrue(r2.Count == 3);
            Assert.IsTrue(r2[1] == 12);
            Assert.IsTrue(r2[4] == 210);
            Assert.IsTrue(r2[5] == 120);
        }
    }
}
