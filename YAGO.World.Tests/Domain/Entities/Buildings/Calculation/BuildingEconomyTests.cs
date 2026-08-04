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
            var reportStr = report.ToString();
        }
    }
}
