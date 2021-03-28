using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.Data;

namespace YSI.CurseOfSilverCrown.Web.BL.EndOfTurn
{
    public class JobFactory : IJobFactory
    {
        protected readonly IServiceScopeFactory serviceScopeFactory;
        protected readonly EndOfTurnService _endOfTurnService;

        public JobFactory(IServiceScopeFactory serviceScopeFactory, EndOfTurnService endOfTurnService)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            _endOfTurnService = endOfTurnService;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var scope = serviceScopeFactory.CreateScope();
            var job = scope.ServiceProvider.GetService(bundle.JobDetail.JobType) as IJob;
            return job;
        }

        public void ReturnJob(IJob job)
        {
            //Do something if need
        }
    }
}
