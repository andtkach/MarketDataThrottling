using System;
using System.Collections.Generic;

namespace MarketDataAggregator
{
    public class RandomDataStreamStrategy : IDataStreamStrategy
    {
        private readonly Random _randomizer;

        public RandomDataStreamStrategy()
        {
            _randomizer = new Random();
        }

        public MarketDataUpdate Next()
        {
            var update = new MarketDataUpdate();
            update.InstrumentId = "AAPL_" + _randomizer.Next(1, 100);
            update.Fields = new Dictionary<byte, long>();

            for (int i = 0; i < _randomizer.Next(1, 5); i++)
            {
                update.Fields[(byte)_randomizer.Next(1, 20)] = _randomizer.Next(1, 10000);
            }

            return update;
        }
    }
}