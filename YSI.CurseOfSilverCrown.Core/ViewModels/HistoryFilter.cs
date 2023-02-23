using System.Collections.Generic;
using YSI.CurseOfSilverCrown.Core.Database.Enums;

namespace YSI.CurseOfSilverCrown.Core.ViewModels
{
    public class HistoryFilter
    {
        public int Important { get; set; }
        public int Region { get; set; } //0 - все, 1 - королевство, 2 - все вассалы, 3 - прямые вассалы, 4 - только свои
        public int Turns { get; set; }
        public bool AggressivePoliticalEvents { get; set; }
        public bool PeacefullPoliticalEvents { get; set; }
        public bool InvestmentEvents { get; set; }
        public bool BudgetEvents { get; set; }
        public bool CataclysmEvents { get; set; }

        public HistoryFilter()
        {
            Important = 0;
            Region = 0;
            Turns = int.MaxValue;
            AggressivePoliticalEvents = true;
            PeacefullPoliticalEvents = true;
            InvestmentEvents = true;
            BudgetEvents = true;
            CataclysmEvents = true;
        }

        private List<enEventResultType> _resultTypes;
        public List<enEventResultType> ResultTypes
        {
            get
            {
                if (_resultTypes == null)
                {
                    _resultTypes = new List<enEventResultType>();
                    if (AggressivePoliticalEvents)
                    {
                        _resultTypes.AddRange(new[] {
                            enEventResultType.FastWarSuccess,
                            enEventResultType.FastWarFail,
                            enEventResultType.FastRebelionSuccess,
                            enEventResultType.FastRebelionFail,
                            enEventResultType.DestroyedUnit,
                            enEventResultType.SiegeFail,
                            enEventResultType.UnitMove,
                            enEventResultType.UnitCantMove
                        });
                    }

                    if (PeacefullPoliticalEvents)
                    {
                        _resultTypes.AddRange(new[] {
                            enEventResultType.Liberation,
                            enEventResultType.ChangeSuzerain,
                            enEventResultType.VoluntaryOath,
                            enEventResultType.GoldTransfer
                        });
                    }

                    if (InvestmentEvents)
                    {
                        _resultTypes.AddRange(new[] {
                            enEventResultType.Growth,
                            enEventResultType.Investments,
                            enEventResultType.Fortifications,
                        });
                    }

                    if (BudgetEvents)
                    {
                        _resultTypes.AddRange(new[] {
                            enEventResultType.Idleness,
                            enEventResultType.TaxCollection,
                            enEventResultType.VasalTax,
                            enEventResultType.Maintenance,
                            enEventResultType.FortificationsMaintenance,
                            enEventResultType.Corruption
                        });
                    }

                    if (CataclysmEvents)
                    {
                        _resultTypes.AddRange(new[] {
                            enEventResultType.Mutiny,
                            enEventResultType.TownFire,
                            enEventResultType.CastleFire,
                            enEventResultType.Disease
                        });
                    }
                }
                return _resultTypes;
            }
        }
    }
}
