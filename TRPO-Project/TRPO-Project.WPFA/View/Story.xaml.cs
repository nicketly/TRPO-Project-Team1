using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TRPO_Project.WPFA.ViewModel;
using System.Collections.Generic;

namespace TRPO_Project.WPFA.View
{
    /// <summary>
    /// Логика взаимодействия для Story.xaml
    /// </summary>
    public partial class Story : UserControl
    {
        public Story()
        {
            InitializeComponent();


            var item10 = new ItemMenu(PackIconKind.History, "SBER", "1379.50 ₽");
            var item11 = new ItemMenu(PackIconKind.History, "SBER", "768 ₽");
            var item12 = new ItemMenu(PackIconKind.History, "SBER", "768 ₽");

            Menu5.Children.Add(new UserControlMenuItem(item10));
            Menu5.Children.Add(new UserControlMenuItem(item11));
            Menu5.Children.Add(new UserControlMenuItem(item12));
        }
    }
}
