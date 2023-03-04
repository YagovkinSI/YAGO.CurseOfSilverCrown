using System;
using System.Collections.Generic;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.MainModels;
using YSI.CurseOfSilverCrown.Core.MainModels.Domains;
using YSI.CurseOfSilverCrown.Core.MainModels.EventDomains;
using YSI.CurseOfSilverCrown.Core.MainModels.Events;
using YSI.CurseOfSilverCrown.Core.MainModels.Turns;
using YSI.CurseOfSilverCrown.Core.Parameters;
using YSI.CurseOfSilverCrown.Core.Utils;

namespace YSI.CurseOfSilverCrown.Core.Helpers.Actions
{
    internal class DiseaseAction : DomainActionBase
    {
        public DiseaseAction(ApplicationDbContext context, Turn currentTurn, Domain domain)
            : base(context, currentTurn, domain)
        {
        }

        public override bool CheckValidAction()
        {
            return !Domain.UnitsHere.Any(u => u.DomainId != Domain.Id && u.Warriors > 50);
        }

        protected override bool Execute()
        {
            var diseaseLevel = 0.1 + Random.NextDouble() / 4;

            var (success, investmentChange) = CalcInvestmentChange(diseaseLevel);
            if (!success)
                return false;

            var (werriorHereChange, werriorAllChange) = CalcWarrioirChange(diseaseLevel);

            var eventStoryResult = CreateEventStoryResult(investmentChange, werriorHereChange, werriorAllChange);
            var dommainEventStories = new Dictionary<int, int>
            {
                {
                    Domain.Id,
                        - (investmentChange.After - investmentChange.Before) * 2
                        - (werriorHereChange.After - werriorHereChange.Before) * WarriorParameters.Price * 2
                }
            };
            CreateEventStory(eventStoryResult, dommainEventStories);

            return true;
        }

        private (EventJsonParametrChange, EventJsonParametrChange) CalcWarrioirChange(double diseaseLevel)
        {
            var warriorPercentEnd = 1.0 - diseaseLevel;
            var warrioirAllStart = 0;
            var warrioirHereStart = 0;
            var warrioirLost = 0;
            foreach (var unit in Domain.Units)
            {
                warrioirAllStart += unit.Warriors;
                if (unit.PositionDomainId != Domain.Id)
                    continue;

                var unitOnStart = unit.Warriors;
                warrioirHereStart += unitOnStart;
                unit.Warriors = (int)Math.Round(unitOnStart * RandomHelper.AddRandom(warriorPercentEnd));
                warrioirLost += unitOnStart - unit.Warriors;
            }

            var werriorHereChange = EventJsonParametrChangeHelper.Create(enEventParameterType.WarriorInDomain,
                warrioirHereStart, warrioirHereStart - warrioirLost);
            var werriorAllChange = EventJsonParametrChangeHelper.Create(enEventParameterType.Warrior,
                warrioirAllStart, warrioirAllStart - warrioirLost);

            return (werriorHereChange, werriorAllChange);
        }

        private (bool, EventJsonParametrChange) CalcInvestmentChange(double diseaseLevel)
        {
            var investmentPercentEnd = 1.0 - diseaseLevel / 2;
            var startInvestments = Domain.Investments;
            var endInvestments = (int)(Domain.Investments * RandomHelper.AddRandom(investmentPercentEnd));
            if (endInvestments < InvestmentsHelper.StartInvestment * 0.9)
                endInvestments = RandomHelper.AddRandom(InvestmentsHelper.StartInvestment);
            if (endInvestments > startInvestments)
                endInvestments = (int)RandomHelper.AddRandom(startInvestments * 0.98, 2);
            var deltaInvestments = endInvestments - startInvestments;
            if (deltaInvestments > 1)
                return (false, null);

            Domain.Investments = endInvestments;
            var investmentChange = EventJsonParametrChangeHelper.Create(enEventParameterType.Investments,
                startInvestments, endInvestments);
            return (true, investmentChange);
        }

        private EventJson CreateEventStoryResult(EventJsonParametrChange investmentChange,
            EventJsonParametrChange werriorHereChange, EventJsonParametrChange werriorAllChange)
        {
            var eventStoryResult = new EventJson(enEventType.Disease);
            var temp = new List<EventJsonParametrChange>
            {
                investmentChange, werriorHereChange, werriorAllChange
            };
            eventStoryResult.AddEventOrganization(Domain.Id, enEventDomainType.Main, temp);
            return eventStoryResult;
        }
    }
}
