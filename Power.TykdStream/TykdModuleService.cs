using Autofac;
using Microsoft.Extensions.Configuration;
using PowerStream.Core;
using System;
using Tianyi;
using Tianyi.Option;

namespace Power
{
    public class TykdModuleService : IModuleService
    {
        public void Init()
        {
            Console.WriteLine("【模块】TykdModuleService 初始化");
            IoCHelper.BeforeInit += IoCHelper_BeforeInit;
        }

        private void IoCHelper_BeforeInit(Autofac.ContainerBuilder func)
        {
            var tianyiOptions = Tykd.Configuration.GetSection("Tykd.Config").Get<TianyiOptions>();
            func.RegisterInstance<TianyiService>(new TianyiService(tianyiOptions.AppKey, tianyiOptions.AppSecret, tianyiOptions.Account));
        }
    }
}
