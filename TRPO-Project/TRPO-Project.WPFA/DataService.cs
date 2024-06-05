using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRPO_Project.WPFA
{
    public class DataService
    {
        private string connectionString = "Server=(localdb)\\ProjectModels;Database=TRPO-Project.Database;Integrated Security=True;";

        public decimal GetBalanceValue()
        {
            decimal balance = 0m;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
DECLARE 
@СуммаПокупки		decimal(10, 2), 
@СуммаПополнений	decimal(10, 2), 
@СуммаДивидендов	decimal(10, 2), 
@СуммаКупонов		decimal(10, 2), 
@СуммаПродажи		decimal(10, 2)

SELECT
@СуммаПокупки = (SELECT Sum(Сумма) FROM [dbo].[History] WHERE Операция = N'Покупка'),
@СуммаПополнений = (SELECT Sum(Сумма) FROM [dbo].[History] WHERE Операция = N'Пополнение счета'),
@СуммаДивидендов = (SELECT Sum(Сумма) FROM [dbo].[History] WHERE Операция = N'Зачисление дивидендов'),
@СуммаКупонов = (SELECT Sum(Сумма) FROM [dbo].[History] WHERE Операция = N'Зачисление купонов'),
@СуммаПродажи = (SELECT Sum(Сумма) FROM [dbo].[History] WHERE Операция = N'Продажа')

IF (@СуммаДивидендов IS NULL)
SET @СуммаДивидендов = 0

IF (@СуммаКупонов IS NULL)
SET @СуммаКупонов = 0

SELECT @СуммаПополнений + @СуммаДивидендов + @СуммаКупонов + @СуммаПродажи - @СуммаПокупки AS Баланс";

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                balance = (decimal)command.ExecuteScalar();
            }
            return balance;
        }

        public decimal GetBriefcaseValue()
        {
            decimal briefcase = 0m;

            string connectionString = "Server=(localdb)\\ProjectModels;Database=TRPO-Project.Database;Integrated Security=True;";
            string query = @"
            DECLARE @ПоследняяДата smalldatetime;

            SET @ПоследняяДата = (SELECT MAX(Дата) FROM [dbo].[History]);

            SELECT SUM(Сумма) FROM [dbo].[History]
            WHERE Дата = @ПоследняяДата AND Код != 'RUB' AND Операция = N'Учет';
        ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    briefcase = Convert.ToDecimal(result);
                }
                else
                {
                    briefcase = 0;
                }
            }

            return briefcase;
        }
    }

}
