using Autofac;
using Microsoft.Extensions.Configuration;
using System.ServiceModel;
using Tianyi.Option;
using Tianyi.Service.Service;
using Topshelf;
using Topshelf.Autofac;

namespace Tianyi.Service
{
    class Program
    {
        static void Main(string[] args)
        {


            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", true, true);
            IConfiguration configuration = builder.Build();

            var powerOptions = configuration.GetSection("Tykd.Config").Get<TianyiOptions>();

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterInstance(powerOptions);
            containerBuilder.RegisterType<TianyiService>();
            containerBuilder.RegisterType<TianyikandianHostService>();
            var container = containerBuilder.Build();

            HostFactory.Run(cfg =>
           {
               cfg.UseAutofacContainer(container);
               cfg.Service<TianyikandianHostService>(s =>
               {
                   s.ConstructUsingAutofacContainer();
                   s.WhenStarted((service, hostControl) => service.Start(hostControl));
                   s.WhenStopped((service, hostControl) => service.Stop(hostControl));
               });
               cfg.RunAsLocalSystem();
               cfg.SetServiceName($"天翼看店-视频同步-相城");
               cfg.SetDisplayName($"天翼看店-视频同步-相城");
               cfg.SetDescription($"天翼看店-视频同步-相城");
           });
        }
    }
}
