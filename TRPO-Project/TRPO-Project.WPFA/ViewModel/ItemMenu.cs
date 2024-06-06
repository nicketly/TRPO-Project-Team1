using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Windows.Controls;

namespace TRPO_Project.WPFA.ViewModel
{
    public class ItemMenu
    {
        public ItemMenu(string header, List<SubItem> subItems, PackIconKind icon)
        {
            Header = header;
            SubItems = subItems;
            Icon = icon;
        }

        public ItemMenu(string header, UserControl screen, PackIconKind icon)
        {
            Header = header;
            Screen = screen;
            Icon = icon;
        }

        public ItemMenu(PackIconKind icon, string header, string sum)
        {
            Header = header;
            Icon = icon;
            Sum = sum;

        }

        public string Header { get; private set; }
        public string Sum { get; private set; }
        public PackIconKind Icon { get; private set; }
        public List<SubItem> SubItems { get; private set; }
        public UserControl Screen { get; private set; }
    }
}
