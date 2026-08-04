using YAGO.World.Domain.Entities.Buildings.Calculation;

namespace YAGO.World.Tests.Domain.Entities.Buildings.Calculation
{
    public class BuildingEconomyTests
    {
        [Fact]
        public void GetReport_Default()
        {
            //Arrange
            var laws = new EconomicLaws();
            var type = new BuildingTypeSettings();
            var economy = new BuildingEconomy(10000, laws, type);

            //Act
            var report = economy.GetReport();

            //Assert
            Assert.True(report.LaborCosts > 750 && report.LaborCosts < 1250);
            Assert.True(report.AverageSalary > 7 && report.AverageSalary < 13);
            Assert.True(report.GrossProfit > 1000 && report.LaborCosts < 2000);
            Assert.True(report.NetProfit > 750 && report.NetProfit < 1250);
            Assert.True(report.TaxAmount > 400 && report.TaxAmount < 650);
        }


        [Fact]
        public void GetReport_Tax30()
        {
            //Arrange
            var laws = new EconomicLaws()
            {
                CorporateTaxRate = 30
            };
            var type = new BuildingTypeSettings();
            var economy = new BuildingEconomy(10000, laws, type);

            //Act
            var report = economy.GetReport();

            //Assert
            Assert.True(report.LaborCosts > 750 && report.LaborCosts < 1250);
            Assert.True(report.AverageSalary > 7 && report.AverageSalary < 13);
            Assert.True(report.GrossProfit > 1000 && report.LaborCosts < 2000);
            Assert.True(report.NetProfit > 650 && report.NetProfit < 1100);
            Assert.True(report.TaxAmount > 550 && report.TaxAmount < 800);
        }
    }
}
