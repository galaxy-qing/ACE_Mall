using ACE_Mall.BLL;
using ACE_Mall.Model;
using Quartz;
using Quartz.Impl;
using System;
using System.Linq;

namespace ACE_Behind_Mall.WebApi
{
    public class TimeJob : IJob
    {
        private OrderBLL orderbll = new OrderBLL();
        //要执行的任务  不管多久执行 只管执行要做什么
        public void Execute(IJobExecutionContext context)
        {
            //订单超过20分钟未付款则取消订单
            var NowTime = DateTime.Now;
            var ordermodel = orderbll.GetList(x => x.IsDelete == 0 && x.CreateTime < (NowTime.AddMinutes(-20))).ToList();
            foreach (var item in ordermodel)
            {
                item.OrderState = 3;
                My_Order m = orderbll.GetUpdateModel<My_Order>(item, "ID");
                orderbll.Update(m);
            }
        }
        //Task IJob.Execute(IJobExecutionContext context)
        //{
        //    throw new NotImplementedException();
        //}
        //public static void Start()
        //{
        //    //调度器工厂
        //    ISchedulerFactory factory = new StdSchedulerFactory();
        //    //调度器
        //    IScheduler scheduler = factory.GetScheduler();
        //    scheduler.GetJobGroupNames();

        //    /*-------------计划任务代码实现------------------*/
        //    //创建任务
        //    IJobDetail job = JobBuilder.Create<TimeJob>().Build();
        //    //创建触发器
        //    ITrigger trigger = TriggerBuilder.Create().WithIdentity("TimeTrigger", "TimeGroup").WithSimpleSchedule(t => t.WithIntervalInMinutes(30).RepeatForever()).Build();
        //    //添加任务及触发器至调度器中
        //    scheduler.ScheduleJob(job, trigger);
        //    /*-------------计划任务代码实现------------------*/

        //    //启动
        //    scheduler.Start();
        //}
        //public static async Task RunProgramRunExample()
        //{
        //    try
        //    {
        //        // Grab the Scheduler instance from the Factory
        //        NameValueCollection props = new NameValueCollection
        //        {
        //            { "quartz.serializer.type", "binary" }
        //        };
        //        StdSchedulerFactory factory = new StdSchedulerFactory(props);
        //        IScheduler scheduler = await factory.GetScheduler();

        //        // and start it off
        //        await scheduler.Start();

        //        // define the job and tie it to our HelloJob class
        //        IJobDetail job = JobBuilder.Create<TimeJob>()
        //            .WithIdentity("job1", "group1")
        //            .Build();

        //        // Trigger the job to run now, and then repeat every 10 seconds
        //        ITrigger trigger = TriggerBuilder.Create()
        //            .WithIdentity("trigger1", "group1")
        //            .StartNow()
        //            .WithSimpleSchedule(x => x
        //                .WithIntervalInSeconds(1)            //在这里配置执行延时
        //                .RepeatForever())
        //            .Build();

        //        // Tell quartz to schedule the job using our trigger
        //        await scheduler.ScheduleJob(job, trigger);

        //        // some sleep to show what‘s happening
        //        //                await Task.Delay(TimeSpan.FromSeconds(5));
        //        // and last shut down the scheduler when you are ready to close your program
        //        //                await scheduler.Shutdown();           

        //        //如果解除await Task.Delay(TimeSpan.FromSeconds(5))和await scheduler.Shutdown()的注释，
        //        //5秒后输出"Press any key to close the application"，
        //        //scheduler里注册的任务也会停止。


        //    }
        //    catch (SchedulerException se)
        //    {
        //        Console.WriteLine(se);
        //    }
        //}
    }
}