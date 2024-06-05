using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TRPO_Project.WPFA.Model;

namespace TRPO_Project.WPFA.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly MoexService _moexService;
        private ObservableCollection<CurrencyRate> _currencyRates;

        public ObservableCollection<CurrencyRate> CurrencyRates
        {
            get { return _currencyRates; }
            set
            {
                _currencyRates = value;
                OnPropertyChanged(nameof(CurrencyRates));
            }
        }

        public ICommand FetchCurrencyRatesCommand { get; }

        public MainViewModel()
        {
            _moexService = new MoexService();
            CurrencyRates = new ObservableCollection<CurrencyRate>();
            FetchCurrencyRatesCommand = new RelayCommand(async (param) => await FetchCurrencyRates());
        }

        private async Task FetchCurrencyRates()
        {
            CurrencyRates.Clear();
            var usdRate = await _moexService.GetCurrencyRateAsync("USD_RUB");
            var eurRate = await _moexService.GetCurrencyRateAsync("EUR_RUB");
            var cnyRate = await _moexService.GetCurrencyRateAsync("CNY_RUB");

            // Добавьте логику для определения тренда (вверх или вниз)
            usdRate.TrendIcon = "Triangle"; // Или "TriangleDown"
            usdRate.TrendColor = "#18D78C"; // Или "#BA0E79"

            eurRate.TrendIcon = "Triangle"; // Или "TriangleDown"
            eurRate.TrendColor = "#18D78C"; // Или "#BA0E79"

            cnyRate.TrendIcon = "TriangleDown"; // Или "Triangle"
            cnyRate.TrendColor = "#BA0E79"; // Или "#18D78C"

            CurrencyRates.Add(usdRate);
            CurrencyRates.Add(eurRate);
            CurrencyRates.Add(cnyRate);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute) : this(execute, null) { }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
