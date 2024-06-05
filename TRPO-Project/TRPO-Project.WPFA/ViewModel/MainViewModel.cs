using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRPO_Project.WPFA.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private decimal _balanceValue;
        private decimal _briefcaseValue;
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
