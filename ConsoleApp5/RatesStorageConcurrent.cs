using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    public class RatesStorageConcurrent
    {
        private readonly ConcurrentDictionary<string, Rate> rates = new();

       /// <summary>
        /// этим методом мы копируем котировки из нативного класса в базовый класс нашей программы, 
        /// обновляем котировки много и часто, будем считать, что у нас 1 млн вызовов этого метода в сутки
        /// </summary>
        /// <param name="newRate"></param>
        public void UpdateRate(NativeRate newRate)
        {
            var newRateCopy = new Rate
            {
                Time = newRate.Time,
                Symbol = newRate.Symbol,
                Bid = newRate.Bid,
                Ask = newRate.Ask
            };

            rates.AddOrUpdate(newRate.Symbol,
                newRateCopy,
                (key, existingRate) => newRateCopy);
        }
        /// <summary>
        /// это тоже высоконагруженный метод, вызываем 1 млн раз в сутки
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public Rate? GetRate(string symbol)
        {
            rates.TryGetValue(symbol, out var rate);
            return rate;
        }

    }
}
