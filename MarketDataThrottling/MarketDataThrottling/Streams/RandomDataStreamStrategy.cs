namespace MarketDataAggregator
{
    using System;
    using System.Collections.Generic;

    public class RandomDataStreamStrategy : IDataStreamStrategy
    {
        private readonly Random randomizer;

        public RandomDataStreamStrategy()
        {
            this.randomizer = new Random();
        }

        public MarketDataUpdate Next()
        {
            var update = new MarketDataUpdate();
            update.InstrumentId = "AAPL_" + this.randomizer.Next(1, 100);
            update.Fields = new Dictionary<byte, long>();

            for (int i = 0; i < this.randomizer.Next(1, 5); i++)
            {
                update.Fields[(byte)this.randomizer.Next(1, 20)] = this.randomizer.Next(1, 10000);
            }

            return update;
        }
    }
}