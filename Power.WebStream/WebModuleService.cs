using Autofac;
using Microsoft.Extensions.Configuration;
using PowerStream.Core;
using System;
using System.Net;

namespace Power.WebStream
{
    public class WebModuleService : IModuleService
    {
        public void Init()
        {
            Console.WriteLine("【模块】WebModuleService 初始化");
            IoCHelper.BeforeInit += IoCHelper_BeforeInit;
        }

        private void IoCHelper_BeforeInit(Autofac.ContainerBuilder func)
        {
            var tianyiOptions = WebManager.Configuration.GetSection("Web.Config").Get<WebOptions>();
            var webClient = new WebClient();
            func.RegisterInstance<WebClient>(webClient);
        }
    }
}
