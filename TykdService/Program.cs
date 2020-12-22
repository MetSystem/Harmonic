using System;
using Topshelf;
using TykdService.Service;

namespace TykdService
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.UseAssemblyInfoForServiceInfo();

                bool throwOnStart = false;
                bool throwOnStop = false;
                bool throwUnhandled = false;

                x.Service(settings => new TianyikandianHostService(), s =>
                {
                    s.BeforeStartingService(_ => Console.WriteLine("BeforeStart"));
                    s.BeforeStoppingService(_ => Console.WriteLine("BeforeStop"));
                });
                x.RunAsLocalSystem();
                x.SetServiceName("天翼看店-视频同步");
                x.SetDisplayName("天翼看店-视频同步");

                x.SetStartTimeout(TimeSpan.FromSeconds(10));
                x.SetStopTimeout(TimeSpan.FromSeconds(10));

                x.EnableServiceRecovery(r =>
                {
                    r.RestartService(3);
                    r.RunProgram(7, "ping google.com");
                    r.RestartComputer(5, "message");

                    r.OnCrashOnly();
                    r.SetResetPeriod(2);
                });

                x.AddCommandLineSwitch("throwonstart", v => throwOnStart = v);
                x.AddCommandLineSwitch("throwonstop", v => throwOnStop = v);
                x.AddCommandLineSwitch("throwunhandled", v => throwUnhandled = v);

                x.OnException((exception) =>
                {
                    Console.WriteLine("Exception thrown - " + exception.Message);
                });
            });
        }
    }
}
