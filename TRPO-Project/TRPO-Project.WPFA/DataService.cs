using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TRPO_Project.Lib;

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
@СуммаПродажи		decimal(10, 2),
@СуммаВывода		decimal(10, 2)

SELECT
@СуммаПокупки = (SELECT Sum(Сумма) FROM [dbo].[History] WHERE Операция = N'Покупка'),
@СуммаПополнений = (SELECT Sum(Сумма) FROM [dbo].[History] WHERE Операция = N'Пополнение счета'),
@СуммаДивидендов = (SELECT Sum(Сумма) FROM [dbo].[History] WHERE Операция = N'Зачисление дивидендов'),
@СуммаКупонов = (SELECT Sum(Сумма) FROM [dbo].[History] WHERE Операция = N'Зачисление купонов'),
@СуммаПродажи = (SELECT Sum(Сумма) FROM [dbo].[History] WHERE Операция = N'Продажа'),
@СуммаВывода = (SELECT Sum(Сумма) FROM [dbo].[History] WHERE Операция = N'Вывод средств')

IF (@СуммаДивидендов IS NULL)
SET @СуммаДивидендов = 0

IF (@СуммаКупонов IS NULL)
SET @СуммаКупонов = 0

IF(@СуммаВывода IS NULL)
SET @СуммаВывода = 0

IF(@СуммаПокупки IS NULL)
SET @СуммаПокупки = 0

IF(@СуммаПополнений IS NULL)
SET @СуммаПополнений = 0

IF(@СуммаПродажи IS NULL)
SET @СуммаПродажи = 0

SELECT @СуммаПополнений + @СуммаДивидендов + @СуммаКупонов + @СуммаПродажи - @СуммаПокупки - @СуммаВывода AS Баланс";

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

        public List<string> GetAllAssetCodes()
        {
            List<string> assetCodes = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Код FROM Assets WHERE Код != 'RUB'";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    assetCodes.Add(reader["Код"].ToString());
                }
            }
            return assetCodes;
        }
        public decimal GetPurchaseAvgPriceAsset(string AssetCode)
        {
            decimal purchaseAvgPrice = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //                string query = @"
                //DECLARE 
                //@ПоследняяДата smalldatetime

                //SET @ПоследняяДата = (SELECT MAX(Дата) FROM [dbo].[History])

                //SELECT AVG(СредняяЦена)
                //FROM (
                //	SELECT
                //	AVG(Цена) AS СредняяЦена
                //	FROM History

                //	WHERE Операция = N'Покупка' AND Код in (SELECT DISTINCT Код FROM History WHERE Дата = @ПоследняяДата AND Операция = N'Учет' AND Код != 'RUB')
                //	GROUP BY Код
                //) AS Подзапрос";

                string query = "SELECT AVG(Цена) FROM History WHERE Операция = N'Покупка' AND Код = @AssetCode";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@AssetCode", AssetCode);
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    purchaseAvgPrice = Convert.ToDecimal(result);
                }
            }
            return purchaseAvgPrice;
        }

        public decimal GetCurrentAvgPriceAsset(string AssetCode)
        {
            decimal currentAvgPrice = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //                string query = @"
                //DECLARE 
                //@ПоследняяДата smalldatetime

                //SET @ПоследняяДата = (SELECT MAX(Дата) FROM [dbo].[History])

                //SELECT 
                //AVG(Цена)
                //FROM History

                //WHERE Дата = @ПоследняяДата AND Операция = N'Учет' AND Код != 'RUB'";

                string query = @"
DECLARE 
@ПоследняяДата smalldatetime

SET @ПоследняяДата = (SELECT MAX(Дата) FROM [dbo].[History])

SELECT Цена FROM History WHERE Дата = @ПоследняяДата AND Операция = N'Учет' AND Код = @AssetCode";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@AssetCode", AssetCode);
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    currentAvgPrice = Convert.ToDecimal(result);
                }
            }
            return currentAvgPrice;
        }

        public decimal GetCurrentAmountAsset(string AssetCode)
        {
            decimal currentAmount = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //                string query = @"
                //DECLARE 
                //@ПоследняяДата smalldatetime

                //SET @ПоследняяДата = (SELECT MAX(Дата) FROM [dbo].[History])

                //SELECT 
                //SUM(Количество)
                //FROM History

                //WHERE Дата = @ПоследняяДата AND Операция = N'Учет' AND Код != 'RUB'";

                string query = @"
DECLARE 
@ПоследняяДата smalldatetime

SET @ПоследняяДата = (SELECT MAX(Дата) FROM [dbo].[History])

SELECT 
Количество
FROM History

WHERE Дата = @ПоследняяДата AND Операция = N'Учет' AND Код = @AssetCode";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@AssetCode", AssetCode);
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    currentAmount = Convert.ToDecimal(result);
                }
            }
            return currentAmount;
        }

        public decimal GetSaleSumAsset(string AssetCode)
        {
            decimal saleSum = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT SUM(Сумма) FROM History WHERE Операция = N'Продажа' AND Код = @AssetCode";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@AssetCode", AssetCode);
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    saleSum = Convert.ToDecimal(result);
                }
            }
            return saleSum;
        }

        public decimal GetSoldAmountAsset(string AssetCode)
        {
            decimal soldAmount = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT SUM(Количество) FROM History WHERE Операция = N'Продажа' AND Код = @AssetCode";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@AssetCode", AssetCode);
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    soldAmount = Convert.ToDecimal(result);
                }
            }
            return soldAmount;
        }

        public decimal GetDividendsSumAsset(string AssetCode)
        {
            decimal dividendsSum = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
DECLARE 
@СуммаДивидендов decimal(10,2)

SET @СуммаДивидендов = (SELECT SUM(Сумма) FROM History WHERE Операция = N'Зачисление дивидендов' AND Код = @AssetCode)

IF @СуммаДивидендов IS NULL
SET @СуммаДивидендов = 0

SELECT @СуммаДивидендов
";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@AssetCode", AssetCode);
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    dividendsSum = Convert.ToDecimal(result);
                }
            }
            return dividendsSum;
        }

        public decimal GetCouponsSumAsset(string AssetCode)
        {
            decimal couponsSum = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
DECLARE 
@СуммаКупонов decimal(10,2)

SET @СуммаКупонов = (SELECT SUM(Сумма) FROM History WHERE Операция = N'Зачисление купонов' AND Код = @AssetCode)

IF @СуммаКупонов IS NULL
SET @СуммаКупонов = 0

SELECT @СуммаКупонов
";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@AssetCode", AssetCode);
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    couponsSum = Convert.ToDecimal(result);
                }
            }
            return couponsSum;
        }

        public decimal GetPurchaseSum()
        {
            decimal purchaseSum = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT SUM(Сумма) FROM History WHERE Операция = N'Покупка'";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    purchaseSum = Convert.ToDecimal(result);
                }
            }
            return purchaseSum;
        }
        public double GetProfitExpectedAsset(string AssetCode)
        {
            double currentAvgPrice = (double)GetCurrentAvgPriceAsset(AssetCode);
            double purchaseAvgPrice = (double)GetPurchaseAvgPriceAsset(AssetCode);
            double currentAmount = (double)GetCurrentAmountAsset(AssetCode);

            double profitExpected = InvLib.ProfitExpected(currentAvgPrice, purchaseAvgPrice, currentAmount);
            return profitExpected;
        }

        public double GetProfitFixedAsset(string AssetCode)
        {
            double purchaseAvgPrice = (double)GetPurchaseAvgPriceAsset(AssetCode);
            double saleSum = (double)GetSaleSumAsset(AssetCode);
            double soldAmount = (double)GetSoldAmountAsset(AssetCode);
            double dividendsSum = (double)GetDividendsSumAsset(AssetCode);
            double couponsSum = (double)GetCouponsSumAsset(AssetCode);

            return InvLib.ProfitFixed(saleSum, purchaseAvgPrice, soldAmount, dividendsSum, couponsSum);
        }

        public double GetIncomeAsset(string AssetCode)
        {
            double profitExpected = (double)GetProfitExpectedAsset(AssetCode);
            double profitFixed = (double)GetProfitFixedAsset(AssetCode);
            double purchaseAvgPrice = (double)GetPurchaseAvgPriceAsset(AssetCode);
            double currentAmount = (double)GetCurrentAmountAsset(AssetCode);
            double saleSum = (double)GetSaleSumAsset(AssetCode);

            return InvLib.Income(profitExpected, profitFixed, purchaseAvgPrice, currentAmount, saleSum);
        }

        public double GetProfitExpected()
        {
            List<string> assetCodes = GetAllAssetCodes();
            double totalProfitExpected = 0;

            foreach (string assetCode in assetCodes)
            {
                totalProfitExpected += GetProfitExpectedAsset(assetCode);
            }

            return Math.Round(totalProfitExpected, 2);
        }

        public double GetProfitFixed()
        {
            List<string> assetCodes = GetAllAssetCodes();
            double totalProfitFixed = 0;

            foreach (string assetCode in assetCodes)
            {
                totalProfitFixed += GetProfitFixedAsset(assetCode);
            }

            return Math.Round(totalProfitFixed, 2);
        }

        public double GetIncome()
        {
            return Math.Round(((double)GetProfitExpected() / (double)GetPurchaseSum()) * 100, 2);
        }
    }

}
