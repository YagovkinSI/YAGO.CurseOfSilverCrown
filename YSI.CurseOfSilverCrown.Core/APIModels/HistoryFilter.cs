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

        private List<EventType> _resultTypes;
        public List<EventType> ResultTypes
        {
            get
            {
                if (_resultTypes == null)
                {
                    _resultTypes = new List<EventType>();
                    if (AggressivePoliticalEvents)
                    {
                        _resultTypes.AddRange(new[] {
                            EventType.FastWarSuccess,
                            EventType.FastWarFail,
                            EventType.FastRebelionSuccess,
                            EventType.FastRebelionFail,
                            EventType.DestroyedUnit,
                            EventType.SiegeFail,
                            EventType.UnitMove,
                            EventType.UnitCantMove
                        });
                    }

                    if (PeacefullPoliticalEvents)
                    {
                        _resultTypes.AddRange(new[] {
                            EventType.Liberation,
                            EventType.ChangeSuzerain,
                            EventType.VoluntaryOath,
                            EventType.GoldTransfer
                        });
                    }

                    if (InvestmentEvents)
                    {
                        _resultTypes.AddRange(new[] {
                            EventType.Growth,
                            EventType.Investments,
                            EventType.Fortifications,
                        });
                    }

                    if (BudgetEvents)
                    {
                        _resultTypes.AddRange(new[] {
                            EventType.Idleness,
                            EventType.TaxCollection,
                            EventType.VasalTax,
                            EventType.Maintenance,
                            EventType.FortificationsMaintenance,
                            EventType.Corruption
                        });
                    }

                    if (CataclysmEvents)
                    {
                        _resultTypes.AddRange(new[] {
                            EventType.Mutiny,
                            EventType.TownFire,
                            EventType.CastleFire,
                            EventType.Disease
                        });
                    }
                }
                return _resultTypes;
            }
        }
    }
}
