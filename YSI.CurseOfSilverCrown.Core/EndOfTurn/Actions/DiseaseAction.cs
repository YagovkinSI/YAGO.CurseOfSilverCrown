using System;
using System.Collections.Generic;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.Parameters;
using YSI.CurseOfSilverCrown.Core.Utils;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.EndOfTurn.Helpers;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
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

        private (EventParametrChange, EventParametrChange) CalcWarrioirChange(double diseaseLevel)
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

            var werriorHereChange = EventParametrChangeHelper.Create(enActionParameter.WarriorInDomain,
                warrioirHereStart, warrioirHereStart - warrioirLost);
            var werriorAllChange = EventParametrChangeHelper.Create(enActionParameter.Warrior,
                warrioirAllStart, warrioirAllStart - warrioirLost);

            return (werriorHereChange, werriorAllChange);
        }

        private (bool, EventParametrChange) CalcInvestmentChange(double diseaseLevel)
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
            var investmentChange = EventParametrChangeHelper.Create(enActionParameter.Investments,
                startInvestments, endInvestments);
            return (true, investmentChange);
        }

        private EventStoryResult CreateEventStoryResult(EventParametrChange investmentChange,
            EventParametrChange werriorHereChange, EventParametrChange werriorAllChange)
        {
            var eventStoryResult = new EventStoryResult(enEventResultType.Disease);
            var temp = new List<EventParametrChange>
            {
                investmentChange, werriorHereChange, werriorAllChange
            };
            eventStoryResult.AddEventOrganization(Domain.Id, enEventOrganizationType.Main, temp);
            return eventStoryResult;
        }
    }
}
