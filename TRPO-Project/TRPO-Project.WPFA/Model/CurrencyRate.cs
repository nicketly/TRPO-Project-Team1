using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRPO_Project.WPFA.Model
{
    public class CurrencyRate
    {
        public string Currency { get; set; }
        public decimal Rate { get; set; }
        public string TrendIcon { get; set; } // Можно использовать для отображения иконки тренда
        public string TrendColor { get; set; } // Можно использовать для цвета тренда
    }
}
