using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Industries;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Domain.GameActions
{
    public class GameEffect
    {
        public GameEffectType Type { get; }
        public double Delta { get; }
        public bool NeedInputText { get; }
        public string Achievement { get; }

        public GameEffect(
            GameEffectType type,
            double? delta = null,
            string? achievement = null)
        {
            Type = type;
            Delta = delta ?? 0;
            Achievement = achievement ?? string.Empty;
            NeedInputText = delta == null && achievement == null;
        }

        internal void Apply(Colony colony, string? stringValue = null)
        {
            var colonyState = colony.State;
            switch (Type)
            {
                case GameEffectType.SetColonyName:
                    colony.DisplayInfo.SetName(stringValue);
                    break;
                case GameEffectType.AddSolars:
                    colonyState.Resources.Solars.Add(Delta);
                    break;
                case GameEffectType.SpendSolars:
                    colonyState.Resources.Solars.Add(-Delta);
                    break;
                case GameEffectType.AddPublicDebt:
                    colonyState.Reforms[ColonyReformType.PublicDebt].Add(Delta);
                    break;
                case GameEffectType.AddActionPoints:
                    colonyState.Resources.ActionPoints.Add((int)Delta);
                    break;
                case GameEffectType.SpendActionPoints:
                    colonyState.Resources.ActionPoints.Add(-(int)Delta);
                    break;
                case GameEffectType.AddMood:
                    colonyState.Resources.Mood.Add(Delta);
                    break;
                case GameEffectType.ReformTaxLevel:
                    colonyState.Reforms[ColonyReformType.TaxLevel].Set(Delta);
                    break;
                case GameEffectType.ReformSocialGuaranteesLevel:
                    colonyState.Reforms[ColonyReformType.SocialGuaranteesLevel].Set(Delta);
                    break;
                case GameEffectType.AddBuildingsAdministrativeState:
                    colonyState.Industries[ColonyIndustryType.Administrative].AddState((int)Delta);
                    break;
                case GameEffectType.AddBuildingsMiningState:
                    colonyState.Industries[ColonyIndustryType.Mining].AddState((int)Delta);
                    break;
                case GameEffectType.SetAchievement:
                    colonyState.Achievements.SetAchievement(Achievement);
                    break;
                default:
                    throw new YagoException($"Параметр {Type} недоступен для изменения.");
            }
        }
    }
}
