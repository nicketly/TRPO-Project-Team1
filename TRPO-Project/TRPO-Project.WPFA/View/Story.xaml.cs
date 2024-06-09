using MaterialDesignColors;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using UserControl = System.Windows.Controls.UserControl;
using OfficeOpenXml;
using System.IO;
using System.Reflection.Metadata;
using System.Windows.Documents;
using System.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Drawing;



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

        

            private void ButtonSearch_Click(object sender, RoutedEventArgs e)

        {
            var connectionString = "Server=(localdb)\\ProjectModels;Database=TRPO-Project.Database;Integrated Security=True;";
            
            

            try
            {
                string m_c = "";
                string search = TextBoxSearch.Text;
                try { var test = decimal.Parse(search); m_c = ""; }
                catch { m_c = "'"; }

                DateTime now = DateTime.Now;


                using (var connection = new SqlConnection(connectionString))
                {
                    var sqlQuerySearch = @"
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
JOIN Assets ON History.Код = Assets.Код ";

                    if (m_c == "") { sqlQuerySearch += $"WHERE (History.ID_Ист = {m_c}{search}{m_c} OR History.Цена =  {m_c}{search}{m_c} OR History.Количество =  {m_c}{search}{m_c} OR History.Сумма =  {m_c}{search}{m_c})"; }
                    else { sqlQuerySearch += $"WHERE ( History.Код LIKE N{m_c}%{search}%{m_c} OR Assets.Наименование LIKE N{m_c}%{search}%{m_c} OR Assets.Тип LIKE N{m_c}%{search}%{m_c} OR History.Операция LIKE  N{m_c}%{search}%{m_c});"; }
                        
                    //using (var command = new SqlCommand(sqlQuerySearch, connection))
                    //    {
                    //connection.Open();
                    //command.Parameters.AddWithValue("@search", search);
                    SqlCommand command = new SqlCommand(sqlQuerySearch, connection);
                    connection.Open();
                    //command.ExecuteScalar();
                    DataTable dataTable = new DataTable();
                            var adapter = new SqlDataAdapter(command);
                            adapter.Fill(dataTable);
                            if (dataGrid.ItemsSource != null)
                            {
                                dataGrid.ItemsSource = null;
                            }
                            dataGrid.ItemsSource = dataTable.DefaultView;
                        }

                    
                

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Введите правильные данные");
            }
        }
        private void ButtonExel_Click(object sender, RoutedEventArgs e)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // Создаем новый Excel пакет
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                // Добавляем новый лист
                ExcelWorksheet ws = excelPackage.Workbook.Worksheets.Add("Sheet1");

                // Получаем источник данных DataGrid
                var dataTable = (dataGrid.ItemsSource as DataView).ToTable();

                // Заполняем Excel лист данными из источника данных
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        ws.Cells[i + 1, j + 1].Value = dataTable.Rows[i][j];
                    }
                }

                // Показываем диалоговое окно для выбора места сохранения файла
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    FileInfo excelFile = new FileInfo(filePath);
                    excelPackage.SaveAs(excelFile);
                }
            }
        }

        private void ButtonPdf_Click(object sender, RoutedEventArgs e)
        {
            // Создаем новый PDF документ
            PdfDocument pdf = new PdfDocument();
            PdfPage page = pdf.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Verdana", 12);

            // Получаем источник данных DataGrid
            var dataTable = (dataGrid.ItemsSource as DataView).ToTable();

            // Определяем начальные координаты для вывода данных
            double x = 40, y = 40;

            // Заполняем PDF документ данными из источника данных
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    gfx.DrawString(dataTable.Rows[i][j].ToString(), font, XBrushes.Black, x, y);
                    x += 100; // увеличиваем координату x для следующей ячейки
                }
                y += 20; // увеличиваем координату y для следующей строки
                x = 40; // сбрасываем координату x в начало строки
            }

            // Показываем диалоговое окно для выбора места сохранения файла
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                pdf.Save(filePath);
            }
        }
    }
}
