using ConsoleApp5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class RatesStorageConcurrentTest
    {
        [Fact]
        public void GetRate_Symbol_ShouldReturnSymbolRate()
        {
            //Arrange
            var rateStorage = new RatesStorageConcurrent();
            rateStorage
                .UpdateRate(new NativeRate
                {
                    Symbol = "EURUSD",
                    Bid = 1.42,
                    Ask = 1.45,
                    Time = DateTime.Now
                });

            //Act
            var rate = rateStorage.GetRate("EURUSD");

            //Assert
            var expected = new Rate { Symbol = "EURUSD" }.Symbol;
            var actual = rate?.Symbol;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetRate_SymbolNotExisted_ShouldReturnNull()
        {
            //Arrange
            var rateStorage = new RatesStorageConcurrent();

            //Act
            var rate = rateStorage.GetRate("EURUSD");

            //Assert
            Assert.Null(rate);
        }

        [Fact]
        public void GetRateUpdateRate_ConcurrentAccess_BidShouldLessOrEqualThenAsk()
        {
            var storage = new RatesStorageConcurrent();
            var symbols = new[] { "EURUSD", "USDJPY", "GBPUSD" };

            // Запускаем задачи для обновления и чтения котировок
            var tasks = new List<Task>();

            foreach (var symbol in symbols)
            {
                tasks.Add(Task.Run(() =>
                {
                    for (int i = 0; i < 10000; i++)
                    {
                        storage.UpdateRate(new NativeRate
                        {
                            Time = DateTime.UtcNow,
                            Symbol = symbol,
                            Bid = i,
                            Ask = i + 0.1
                        });
                    }
                }));

                tasks.Add(Task.Run(() =>
                {
                    for (int i = 0; i < 10000; i++)
                    {
                        var rate = storage.GetRate(symbol);
                        if (rate != null)
                        {
                            Assert.True(rate.Bid <= rate.Ask);
                        }
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());
        }
    }
}
