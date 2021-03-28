using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YSI.CurseOfSilverCrown.Web.BL.EndOfTurn
{
    public class DataJob : IJob
    {
        protected readonly EndOfTurnService _endOfTurnService;

        public DataJob(EndOfTurnService endOfTurnService)
        {
            _endOfTurnService = endOfTurnService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var success = await _endOfTurnService.Execute();
        }
    }
}
