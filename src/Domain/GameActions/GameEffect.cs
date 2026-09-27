using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Councils;
using YAGO.World.Domain.Colonies.Industries;
using YAGO.World.Domain.Colonies.Reforms;

namespace YAGO.World.Domain.GameActions
{
    public class GameEffect
    {
        public GameEffectType Type { get; }
        public double Value { get; }
        public bool NeedInputText { get; }
        public string Code { get; }

        public GameEffect(
            GameEffectType type,
            double? value = null,
            string? code = null)
        {
            Type = type;
            Value = value ?? 0;
            Code = code ?? string.Empty;
            NeedInputText = value == null && code == null;
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
                    colonyState.Resources.Solars.Add(Value);
                    break;
                case GameEffectType.SpendSolars:
                    colonyState.Resources.Solars.Add(-Value);
                    break;
                case GameEffectType.AddPublicDebt:
                    colonyState.Policy.Reforms.AddPublicDebt(Value);
                    break;
                case GameEffectType.AddActionPoints:
                    colonyState.Resources.ActionPoints.Add((int)Value);
                    break;
                case GameEffectType.SpendActionPoints:
                    colonyState.Resources.ActionPoints.Add(-(int)Value);
                    break;
                case GameEffectType.AddMood:
                    colonyState.Mood.Add(Value);
                    break;
                case GameEffectType.SetCorporateTaxRate:
                    colonyState.Policy.Reforms.CorporateTaxRate.Set(Value);
                    break;
                case GameEffectType.SetMedicalInsuranceLevel:
                    colonyState.Policy.Reforms.MedicalInsurance.Set(Value);
                    break;
                case GameEffectType.SetAutomationIncentive:
                    colonyState.Policy.Reforms.AutomationIncentive.Set((AutomationIncentiveLevel)Value);
                    break;
                case GameEffectType.AddBuildingsAdministrativeState:
                    colonyState.Industries[ColonyIndustryType.Administrative].AddState((int)Value);
                    break;
                case GameEffectType.AddBuildingsMiningState:
                    colonyState.Industries[ColonyIndustryType.Mining].AddState((int)Value);
                    break;
                case GameEffectType.ChangeAdministratorLoyalty:
                    colonyState.Council.AddLoyalty(CouncilAdvisorRole.Administrator, (int)Value);
                    break;
                case GameEffectType.SetAchievement:
                    colonyState.Achievements.SetAchievement(Code);
                    break;
                case GameEffectType.SetAsteroid:
                    colonyState.SetAsteroid(Code);
                    break;
                case GameEffectType.UnlockWikiArticle:
                    colonyState.UnlockedWikiArticles.AddUnlocked(Code);
                    break;
                case GameEffectType.SetStation:
                    colonyState.SetStation(Code);
                    break;
                case GameEffectType.SetChoice:
                    colonyState.SetChoice(Code, (int)Value);
                    break;
            }
        }
    }
}
