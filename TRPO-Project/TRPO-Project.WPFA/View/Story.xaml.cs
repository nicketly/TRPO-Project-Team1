using System.Data;
using System.Data.SqlClient;
using UserControl = System.Windows.Controls.UserControl;

namespace TRPO_Project.WPFA.View
{
    /// <summary>
    /// Логика взаимодействия для Story.xaml
    /// </summary>
    public partial class Story : UserControl
    {
        private string connectionString = "Server=(localdb)\\ProjectModels;Database=TRPO-Project.Database;Integrated Security=True;";
        public Story()
        {
            InitializeComponent();
            LoadHistoryData();

            //var item10 = new ItemMenu(PackIconKind.History, "SBER", "1379.50 ₽");
            //var item11 = new ItemMenu(PackIconKind.History, "SBER", "768 ₽");
            //var item12 = new ItemMenu(PackIconKind.History, "SBER", "768 ₽");

            //Menu5.Children.Add(new UserControlMenuItem(item10));
            //Menu5.Children.Add(new UserControlMenuItem(item11));
            //Menu5.Children.Add(new UserControlMenuItem(item12));
        }

        private void LoadHistoryData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
SELECT 
History.ID_Ист,
History.Код,
Assets.Наименование,
Assets.Тип,
History.Дата,
History.Цена,
History.Количество,
History.Сумма,
History.Операция
FROM History
JOIN Assets ON History.Код = Assets.Код;";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGrid.ItemsSource = dataTable.DefaultView;
            }
        }
    }
}
