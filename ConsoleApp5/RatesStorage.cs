using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    public class RatesStorage
    {
        private Dictionary<string, Rate> rates = new();

        static object _rateLock = new object();

       /// <summary>
        /// этим методом мы копируем котировки из нативного класса в базовый класс нашей программы, 
        /// обновляем котировки много и часто, будем считать, что у нас 1 млн вызовов этого метода в сутки
        /// </summary>
        /// <param name="newRate"></param>
        public void UpdateRate(NativeRate newRate)
        {
            lock (_rateLock)
            {
                if (rates.ContainsKey(newRate.Symbol) == false)
                {
                    rates.Add(newRate.Symbol, new Rate() { Symbol = newRate.Symbol });
                }

                var oldRate = rates[newRate.Symbol];
                oldRate.Time = newRate.Time;
                oldRate.Bid = newRate.Bid;
                oldRate.Ask = newRate.Ask;
            }
        }
        /// <summary>
        /// это тоже высоконагруженный метод, вызываем 1 млн раз в сутки
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public Rate? GetRate(string symbol)
        {
            lock (_rateLock)
            {
                if (rates.ContainsKey(symbol) == false)
                    return null;

                var rate = rates[symbol];

                //if (rate.Ask < rate.Bid)
                //{
                //    throw new Exception($"Incorrect rate: Ask < Bid; Ask={rate.Ask}, Bid={rate.Bid}");
                //}

                return rate;
            }     
        }

    }
}
