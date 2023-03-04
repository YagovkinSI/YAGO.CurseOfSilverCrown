using System.Collections.Generic;
using YSI.CurseOfSilverCrown.Core.Database.Events;

namespace YSI.CurseOfSilverCrown.Core.APIModels
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

        private List<enEventType> _resultTypes;
        public List<enEventType> ResultTypes
        {
            get
            {
                if (_resultTypes == null)
                {
                    _resultTypes = new List<enEventType>();
                    if (AggressivePoliticalEvents)
                    {
                        _resultTypes.AddRange(new[] {
                            enEventType.FastWarSuccess,
                            enEventType.FastWarFail,
                            enEventType.FastRebelionSuccess,
                            enEventType.FastRebelionFail,
                            enEventType.DestroyedUnit,
                            enEventType.SiegeFail,
                            enEventType.UnitMove,
                            enEventType.UnitCantMove
                        });
                    }

                    if (PeacefullPoliticalEvents)
                    {
                        _resultTypes.AddRange(new[] {
                            enEventType.Liberation,
                            enEventType.ChangeSuzerain,
                            enEventType.VoluntaryOath,
                            enEventType.GoldTransfer
                        });
                    }

                    if (InvestmentEvents)
                    {
                        _resultTypes.AddRange(new[] {
                            enEventType.Growth,
                            enEventType.Investments,
                            enEventType.Fortifications,
                        });
                    }

                    if (BudgetEvents)
                    {
                        _resultTypes.AddRange(new[] {
                            enEventType.Idleness,
                            enEventType.TaxCollection,
                            enEventType.VasalTax,
                            enEventType.Maintenance,
                            enEventType.FortificationsMaintenance,
                            enEventType.Corruption
                        });
                    }

                    if (CataclysmEvents)
                    {
                        _resultTypes.AddRange(new[] {
                            enEventType.Mutiny,
                            enEventType.TownFire,
                            enEventType.CastleFire,
                            enEventType.Disease
                        });
                    }
                }
                return _resultTypes;
            }
        }
    }
}
