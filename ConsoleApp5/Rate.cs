using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    public class Rate
    {
        public DateTime Time { get; set; }
        /// <summary>
        /// название инструмента, например EURUSD
        /// </summary>
        public string Symbol { get; set; }  // 
        /// <summary>
        /// цена для покупок 
        /// </summary>
        public double Bid { get; set; } // я бы тут использовал decimal
        /// <summary>
        /// цена для продаж Ask >= Bid всегда!
        /// </summary>
        public double Ask { get; set; } // я бы тут использовал decimal
    }
}
