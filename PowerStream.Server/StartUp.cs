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
            var services = IoCHelper.GetAssignableFrom<IModuleService>();
            services?.ForEach(t => t.Init());
            IoCHelper.Init(builder);

            var powerOptions = PowerManager.Configuration.GetSection("Server").Get<PowerOptions>();
            builder.RegisterType<PowerSmartController>().AsSelf();
            builder.RegisterType<StreamService>().As<IStreamService>();
            builder.RegisterInstance<PowerOptions>(powerOptions);
        }
    }
}