using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRPO_Project.WPFA
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using MaterialDesignThemes.Wpf;

    public class CurrencyToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currency = value as string;
            switch (currency)
            {
                case "USD_RUB":
                    return PackIconKind.CurrencyUsd;
                case "EUR_RUB":
                    return PackIconKind.CurrencyEur;
                case "CNY_RUB":
                    return PackIconKind.CurrencyCny;
                default:
                    return PackIconKind.Help; // Значок по умолчанию
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
