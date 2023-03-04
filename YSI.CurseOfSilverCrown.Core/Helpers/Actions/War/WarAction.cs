using YSI.CurseOfSilverCrown.Core.Database;
using YSI.CurseOfSilverCrown.Core.Database.Commands;
using YSI.CurseOfSilverCrown.Core.Database.Commands.UnitCommands;
using YSI.CurseOfSilverCrown.Core.Database.Turns;
using YSI.CurseOfSilverCrown.Core.Helpers.Map.Routes;

namespace YSI.CurseOfSilverCrown.Core.Helpers.Actions.War
{
    internal partial class WarAction : UnitActionBase
    {
        private WarActionParameters _warActionParameters;

        public WarAction(ApplicationDbContext context, Turn currentTurn, int unitId)
                : base(context, currentTurn, unitId)
        {
        }

        public override bool CheckValidAction()
        {
            return Unit.Type == enUnitCommandType.War &&
                Unit.TargetDomainId != null &&
                Unit.Status == enCommandStatus.ReadyToMove &&
                RouteHelper.IsNeighbors(Context, Unit.PositionDomainId.Value, Unit.TargetDomainId.Value) &&
                !Context.Domains.IsSameKingdoms(Unit.Domain, Unit.Target);
        }

        protected override bool Execute()
        {
            _warActionParameters = new WarActionParameters(Context, Unit);

            while (!_warActionParameters.WarIsOver)
            {
                RetreatCheck();
                CalcWarActionStage();
            }

            CalsWarResult();

            CreateEvent();

            return true;
        }

        private void RetreatCheck()
        {
            var task = new WarActionRetreatCheckTask(_warActionParameters);
            task.Execute();
        }

        private void CalcWarActionStage()
        {
            var task = new WarActionStageCalcTask(_warActionParameters);
            task.Execute();
        }

        private void CalsWarResult()
        {
            var resultCalcTask = new WarActionResultCalcTask(Context, _warActionParameters, CurrentTurn);
            resultCalcTask.Execute();
        }

        private void CreateEvent()
        {
            var task = new WarEventCreateTask(Context, _warActionParameters);
            task.Execute();
            CreateEventStory(task.EventStoryResult, task.DommainEventStories);
        }
    }
}
