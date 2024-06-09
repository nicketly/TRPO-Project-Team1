using System.Collections.Generic;
using System.ComponentModel;

namespace TRPO_Project.WPFA.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private decimal _balanceValue;
        private decimal _briefcaseValue;
        private decimal _profitExpectedValue;
        private decimal _profitFixedValue;
        private decimal _incomeValue;
        public decimal BalanceValue
        {
            get { return _balanceValue; }
            set
            {
                if (_balanceValue != value)
                {
                    _balanceValue = value;
                    OnPropertyChanged(nameof(BalanceValue));
                }
            }
        }

        public decimal BriefcaseValue
        {
            get { return _briefcaseValue; }
            set
            {
                if (_briefcaseValue != value)
                {
                    _briefcaseValue = value;
                    OnPropertyChanged(nameof(BriefcaseValue));
                }
            }
        }

        public decimal ProfitExpectedValue
        {
            get { return _profitExpectedValue; }
            set
            {
                if (_profitExpectedValue != value)
                {
                    _profitExpectedValue = value;
                    OnPropertyChanged(nameof(ProfitExpectedValue));
                }
            }
        }

        public decimal ProfitFixedValue
        {
            get { return _profitFixedValue; }
            set
            {
                if (_profitFixedValue != value)
                {
                    _profitFixedValue = value;
                    OnPropertyChanged(nameof(ProfitFixedValue));
                }
            }
        }

        public decimal IncomeValue
        {
            get { return _incomeValue; }
            set
            {
                if (_incomeValue != value)
                {
                    _incomeValue = value;
                    OnPropertyChanged(nameof(IncomeValue));
                }
            }
        }

        public void AddData(Dictionary<string, List<double>> dict, string key, double value)
        {
            if (!dict.ContainsKey(key))
            {
                dict.Add(key, new List<double> { value });
            }
            else
            {
                dict[key].Add(value);
            }
        }

        

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
