using Autofac;
using Harmonic.Hosting;
using Microsoft.Extensions.Configuration;
using PowerStream.Core;

namespace PowerStream.Server
{
    internal class Startup : IStartup
    {
        public void ConfigureServices(ContainerBuilder builder)
        {
            IoCHelper.Init(builder);
            var services = IoCHelper.GetAssignableFrom<IModuleService>();
            if (services != null)
            {
                foreach (var item in services)
                {
                    item.Init();
                }
            }
            var powerOptions = PowerManager.Configuration.GetSection("Server").Get<PowerOptions>();
            builder.RegisterType<PowerSmartController>().AsSelf();
            builder.RegisterType<StreamService>().As<IStreamService>();
            builder.RegisterInstance<PowerOptions>(powerOptions);
            IoCHelper.BeforeInit += IoCHelper_BeforeInit;
        }

        private void IoCHelper_BeforeInit(ContainerBuilder func)
        {

        }
    }
}