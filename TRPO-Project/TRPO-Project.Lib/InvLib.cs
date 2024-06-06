namespace TRPO_Project.Lib
{
    public class InvLib
    {
        public static double ProfitExpected(double CurrentAvgPrice, double PurchaseAvgPrice, double CurrentAmount)
        {
            if (CurrentAvgPrice < 0 || PurchaseAvgPrice < 0 || CurrentAmount < 0)
            {
                throw new Exception();
            }

            return (CurrentAvgPrice - PurchaseAvgPrice) * CurrentAmount;
        }

        public static double ProfitFixed(double SaleSum, double PurchaseAvgPrice, double SoldAmount, double DividendsSum, double CouponsSum)
        {
            if (SaleSum < 0 || PurchaseAvgPrice < 0 || SoldAmount < 0 || DividendsSum < 0 || CouponsSum < 0)
            {
                throw new Exception();
            }
            return (SaleSum - PurchaseAvgPrice * SoldAmount + DividendsSum + CouponsSum);
        }

        public static double Income(double ProfitExpected, double ProfitFixed, double PurchaseAvgPrice, double CurrentAmount, double SaleSum)
        {
            if (PurchaseAvgPrice < 0 || CurrentAmount < 0 || SaleSum < 0)
            {
                throw new Exception();
            }

            return ((ProfitExpected + ProfitFixed) / (PurchaseAvgPrice * CurrentAmount + SaleSum)) * 100;
        }
    }
}