using MaterialDesignThemes.Wpf;
using System.Windows.Controls;

namespace TRPO_Project.WPFA.ViewModel
{
    public class SubItem
    {
        public SubItem(PackIconKind iconSub, string name, decimal check, string changes, UserControl screen = null)
        {
            IconSub = iconSub;
            Name = name;
            Check = check;
            Changes = changes;
            Screen = screen;
        }

        public SubItem(string name, decimal check, UserControl screen = null)
        {
            Name = name;
            Check = check;
            Screen = screen;
        }

        public PackIconKind IconSub { get; private set; }
        public string Name { get; private set; }
        public decimal Check { get; private set; }
        public string Changes { get; private set; }
        public UserControl Screen { get; private set; }
    }
}
