using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YSI.CurseOfSilverCrown.Web.BL.EndOfTurn
{
    public static class DataScheduler
    {
        public static async void Start(IServiceProvider serviceProvider)
        {
            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            scheduler.JobFactory = serviceProvider.GetService(typeof(JobFactory)) as IJobFactory;
            await scheduler.Start();

            var jobDetail = JobBuilder.Create<DataJob>().Build();

            var now = DateTime.UtcNow;
            if (now.Hour > 2)
                now.AddDays(1);
            var firstDate = new DateTime(now.Year, now.Month, now.Day, 2, 0, 0);            
            var trigger = TriggerBuilder.Create()
                .WithIdentity("EndOfTurn", "default")
                .StartAt(firstDate)
                .WithSimpleSchedule(x => x
                    .WithIntervalInHours(24)
                    .RepeatForever())
                .Build();
            await scheduler.ScheduleJob(jobDetail, trigger);
        }
    }
}
