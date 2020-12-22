using Autofac;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Power
{
    /// <summary>
    ///  依赖注入帮助类
    /// </summary>
    public class IoCHelper
    {
        private static ContainerBuilder _builder;
        private static IContainer _container;

        public delegate void BeforeInitEventHander(ContainerBuilder func);
        public static event BeforeInitEventHander BeforeInit;


        static IoCHelper()
        {
        }

        private static Assembly[] GetAllAssemblies()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string webPath = Path.Combine(path, "bin");
            if (Directory.Exists(webPath))
            {
                path = webPath;
            }

            return Directory.GetFiles(path, "Power*.dll").Select(Assembly.LoadFrom).Distinct().ToArray();
        }

        public static void Init(ContainerBuilder builder = null)
        {
            if (builder == null)
            {
                _builder = new ContainerBuilder();
            }
            if (BeforeInit != null)
            {
                BeforeInit(_builder);
            }

            var namedRegisterService = new Func<Type, string>(t =>
            {
                var name = string.Empty;
                var attribute = t.GetCustomAttributes(typeof(AliasNameAttribute), false).FirstOrDefault() as AliasNameAttribute;
                if (attribute != null)
                {
                    name = attribute.Name;
                }

                return name;
            });
            var assemblies = GetAllAssemblies();
            _builder.RegisterAssemblyTypes(assemblies)
                .Where(type => typeof(ITransientDependency).IsAssignableFrom(type) && !type.IsAbstract)
                .Named<ISingletonDependency>(namedRegisterService) 
                .AsImplementedInterfaces()
                .OnActivating(e =>
                {
                })
                .OnRegistered(e =>
                {
                })
                .OnPreparing(e =>
                {
                })
                .AsSelf()
                .PropertiesAutowired()
                .InstancePerDependency();

            _builder.RegisterAssemblyTypes(assemblies)
              .Where(type => typeof(ISingletonDependency).IsAssignableFrom(type) && !type.IsAbstract)
              .AsSelf()
              .Named<ISingletonDependency>(t =>
              {
                  var name = string.Empty;
                  if (t is ISingletonDependency)
                  {
                      var attribute = t.GetCustomAttributes(typeof(AliasNameAttribute), false).FirstOrDefault() as AliasNameAttribute;
                      if (attribute != null)
                      {
                          name = attribute.Name;
                      }
                      else
                      {
                          name = t.Name;
                      }
                  }
                  return name;
              })
              .AsImplementedInterfaces()
              .PropertiesAutowired()
              .SingleInstance();
        }

        public static T Resolve<T>()
        {
            if (_container == null)
            {
                _container = _builder.Build();
            }

            if (_container.IsRegistered<T>())
            {
                return _container.Resolve<T>();
            }
            else
            {
                return default(T);
            }
        }

        public static bool IsRegistered<T>()
        {
            if (_container == null)
            {
                _container = _builder.Build();
            }

            return _container.IsRegistered<T>();
        }

        public static T ResolveNamed<T>(string name)
        {
            if (_container == null)
            {
                _container = _builder.Build();
            }

            if (_container.IsRegisteredWithName<T>(name))
            {
                return _container.ResolveNamed<T>(name);
            }
            else
            {
                return default(T);
            }
        }

        public static bool IsRegisteredWithName<T>(string name)
        {
            if (_container == null)
            {
                _container = _builder.Build();
            }

            return _container.IsRegisteredWithName<T>(name);
        }
    }
}
