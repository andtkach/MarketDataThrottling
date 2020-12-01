namespace TestProject
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using MarketDataAggregator;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MarketDataUpdateTests
    {
        [TestMethod]
        public void CheckToString_ValidOutput()
        {
            //// Arrange
            string expected = "Instrument: AAA, FieldsNo: 2, Fields: [1: 10, 2: 12]";
            var item = new MarketDataUpdate()
            {
                InstrumentId = "AAA", Fields = new Dictionary<byte, long>
                {
                    { 1, 10 },
                    { 2, 12 }
                }
            };

            //// Act
            var result = item.ToString();

            //// Assert
            Assert.IsTrue(result.Equals(expected), $"Wrong ToString result. \n\tExpected: \t{expected}, \n\tActual: \t{result}\n");
            Console.WriteLine(expected);
            Console.WriteLine(result);
        }

        [TestMethod]
        public void CheckToString_NoEmptyComma()
        {
            //// Arrange
            var item = new MarketDataUpdate()
            {
                InstrumentId = "AAA",
                Fields = new Dictionary<byte, long>
                {
                    { 1, 10 },
                    { 2, 12 }
                }
            };

            //// Act
            var result = item.ToString();

            //// Assert
            Assert.IsFalse(result.Contains(", ]"));
        }
    }
}
