using System.Windows;
using System.Windows.Controls;
using TRPO_Project.WPFA.ViewModel;

namespace TRPO_Project.WPFA.View
{
    /// <summary>
    /// Логика взаимодействия для UserControlMenuItem.xaml
    /// </summary>
    public partial class UserControlMenuItem : UserControl
    {
        public UserControlMenuItem(ItemMenu itemMenu)
        {
            InitializeComponent();

            ExpanderMenu.Visibility = itemMenu.SubItems == null ? Visibility.Collapsed : Visibility.Visible;
            ListViewItemMenu.Visibility = itemMenu.SubItems == null ? Visibility.Visible : Visibility.Collapsed;

            this.DataContext = itemMenu;
        }
    }
}
