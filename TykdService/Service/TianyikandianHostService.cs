using Hangfire;
using Hangfire.MemoryStorage;
using System;
using Topshelf;

namespace TykdService.Service
{
    public class TianyikandianHostService : ServiceControl
    {
        private BackgroundJobServer JobServer { get; set; }


        public bool Start(HostControl hostControl)
        {
            new TianyikandianService().GetExtTerminalList();
            return false;
            GlobalConfiguration.Configuration
                .UseColouredConsoleLogProvider()
                .UseMemoryStorage();

            var options = new BackgroundJobServerOptions
            {
                SchedulePollingInterval = TimeSpan.FromSeconds(3),
                WorkerCount = 1
            };

            RecurringJob.AddOrUpdate("SearchQysj", () => new TianyikandianService().GetExtTerminalList(), "*/5 * * * * *");

            JobServer = new BackgroundJobServer(options);
            JobServer.Start();
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            return JobServer.WaitForShutdown(TimeSpan.FromSeconds(3));
        }
    }
}
