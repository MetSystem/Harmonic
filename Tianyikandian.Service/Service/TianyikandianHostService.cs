using Hangfire;
using Hangfire.MemoryStorage;
using System;
using Tianyi.Option;
using Topshelf;

namespace Tianyi.Service.Service
{
    public class TianyikandianHostService
    {
        private TianyiOptions Option { get; set; }
        private TianyiService Service { get; set; }

        private BackgroundJobServer JobServer { get; set; }

        public TianyikandianHostService(TianyiOptions option, TianyiService tianyikandianService)
        {
            Option = option;
            Service = tianyikandianService;
        }

        public bool Start(HostControl hostControl)
        {
            //Service.GetExtTerminalList();
            GlobalConfiguration.Configuration
                .UseColouredConsoleLogProvider()
                .UseMemoryStorage();

            var options = new BackgroundJobServerOptions
            {
                SchedulePollingInterval = TimeSpan.FromSeconds(3),
                WorkerCount = 1
            };

            RecurringJob.AddOrUpdate("SearchQysj", () => Service.GetExtTerminalList(), "*/5 * * * * *");

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
