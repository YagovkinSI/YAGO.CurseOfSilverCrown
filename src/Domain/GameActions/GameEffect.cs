using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Councils;
using YAGO.World.Domain.Colonies.Industries;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.Stations;

namespace YAGO.World.Domain.GameActions
{
    public class GameEffect
    {
        public GameEffectType Type { get; }
        public double Delta { get; }
        public bool NeedInputText { get; }
        public string Code { get; }

        public GameEffect(
            GameEffectType type,
            double? delta = null,
            string? code = null)
        {
            Type = type;
            Delta = delta ?? 0;
            Code = code ?? string.Empty;
            NeedInputText = delta == null && code == null;
        }

        internal void Apply(Colony colony, string? stringValue = null)
        {
            var colonyState = colony.State;
            switch (Type)
            {
                case GameEffectType.SetColonyName:
                    colony.SetName(stringValue);
                    break;
                case GameEffectType.AddSolars:
                    colonyState.Resources.Solars.Add(Delta);
                    break;
                case GameEffectType.SpendSolars:
                    colonyState.Resources.Solars.Add(-Delta);
                    break;
                case GameEffectType.AddPublicDebt:
                    colonyState.Reforms.AddPublicDebt(Delta);
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
                case GameEffectType.SetCorporateTaxRate:
                    colonyState.Reforms.CorporateTaxRate.Set(Delta);
                    break;
                case GameEffectType.ReformSocialGuaranteesLevel:
                    colonyState.Reforms.SetSocialGuaranteesLevel(Delta);
                    break;
                case GameEffectType.AddBuildingsAdministrativeState:
                    colonyState.Industries[ColonyIndustryType.Administrative].AddState((int)Delta);
                    break;
                case GameEffectType.AddBuildingsMiningState:
                    colonyState.Industries[ColonyIndustryType.Mining].AddState((int)Delta);
                    break;
                case GameEffectType.ChangeAdministratorLoyalty:
                    colonyState.Council.AddLoyalty(CouncilAdvisorRole.Administrator, (int)Delta);
                    break;
                case GameEffectType.SetAchievement:
                    colonyState.Achievements.SetAchievement(Code);
                    break;
                case GameEffectType.SetAsteroid:
                    colonyState.SetAsteroid(AsteroidDataset.GetRequired(Code).Id);
                    break;
                case GameEffectType.UnlockWikiArticle:
                    colonyState.UnlockedWikiArticles.AddUnlocked(Code);
                    break;
                default:
                    throw new YagoException($"Параметр {Type} недоступен для изменения.");
            }
        }
    }
}
