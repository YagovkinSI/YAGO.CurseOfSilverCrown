using System;
using YSI.CurseOfSilverCrown.Web.Helpers;
using YSI.CurseOfSilverCrown.Web.Parameters;

namespace YSI.CurseOfSilverCrown.Web.Helpers.StartingDatas
{
    internal class StartingDataMapModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public int[] BorderingDomainModelIds { get; set; }
        public int? SuzerainId { get; set; }
        public int Investments { get; set; }
        public int Fortifications { get; set; }
        public int VassalCount { get; set; }
        public int Warrioirs { get; set; }

        public StartingDataMapModel(int id, int? suzerainId, int investments, int fortifications, int power, int size, string name, int[] borderingDomainModelIds)
        {
            Id = id;
            Name = name;
            Size = size;
            BorderingDomainModelIds = borderingDomainModelIds;
            SuzerainId = suzerainId;
            Investments = investments == 1
                ? InvestmentsHelper.StartInvestment
                : (investments - 1) * 3 * InvestmentsHelper.StartInvestment;

            VassalCount = (power - 1) * 3;

            var tax = InvestmentsHelper.GetInvestmentTax(Investments) * (1 - (SuzerainId == null ? 0 : Constants.BaseVassalTax));
            var vasalInvestment = 3 * InvestmentsHelper.StartInvestment;
            var vassals = VassalCount * InvestmentsHelper.GetInvestmentTax(vasalInvestment) * Constants.BaseVassalTax;
            var summ = (int)(tax + vassals);

            Fortifications = FortificationsParameters.StartCount + (fortifications * fortifications - 1) * 5000;
            summ -= (int)(Fortifications * FortificationsParameters.MaintenancePercent);

            var forWarrioirs = summ - 2000;
            Warrioirs = Math.Max(forWarrioirs / WarriorParameters.Maintenance, 0);
        }
    }
}
