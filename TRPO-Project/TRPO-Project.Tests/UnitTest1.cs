using TRPO_Project.Lib;

namespace TRPO_Project.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ComputationalTest_ProfitExpected()     //Вычислительный тест
        {
            const double currentAvgPrice = 400;
            const double purchaseAvgPrice = 200;
            const double currentAmount = 200;
            const double expected = 40000;


            double result = new InvLib().ProfitExpected(currentAvgPrice, purchaseAvgPrice, currentAmount);
            Assert.AreEqual(expected, result);
        }
        [Test]

        public void ComputationalTest_ProfitFixed()     //Вычислительный тест
        {
            const double saleSum = 400;
            const double purchaseAvgPrice = 10;
            const double soldAmount = 10;
            const double dividendsSum = 200;
            const double couponsSum = 200;
            const double expected = 700;


            double result = new InvLib().ProfitFixed(saleSum, purchaseAvgPrice, soldAmount, dividendsSum, couponsSum);
            Assert.AreEqual(expected, result);
        }
        [Test]

        public void ComputationalTest_Income()     //Вычислительный тест
        {
            const double profitExpected = 510;
            const double profitFixed = 510;
            const double purchaseAvgPrice = 100;
            const double currentAmount = 100;
            const double saleSum = 200;
            const double expected = 10;


            double result = new InvLib().Income(profitExpected, profitFixed, purchaseAvgPrice, currentAmount, saleSum);
            Assert.AreEqual(expected, result);
        }

    }
}