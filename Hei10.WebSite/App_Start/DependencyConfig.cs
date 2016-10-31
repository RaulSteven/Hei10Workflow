using Autofac;
using Autofac.Integration.Mvc;
using Hei10.Core.Cache;
using Hei10.Domain.Entityframework;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Repositories;
using Hei10.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Hei10.WebSite
{
    public class DependencyConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetCallingAssembly())
                .PropertiesAutowired();
            builder.RegisterType<DbFactory>()
                .As<IDbFactory>()
                .InstancePerRequest()
                .PropertiesAutowired();
            builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>))
                .InstancePerRequest()
                .PropertiesAutowired();

            builder.RegisterAssemblyTypes(typeof(UsersRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerRequest()
                .PropertiesAutowired();

            builder.RegisterType<SysConfigRepository>()
                .As<ISysConfigRepository>()
                .InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(FormsAuthenticationSvc).Assembly)
                .Where(t => t.Name.EndsWith("Svc"))
                .AsImplementedInterfaces()
                .InstancePerRequest()
                .PropertiesAutowired();

            builder.RegisterType<MemoryCacheManager>()
                .As<ICacheManager>()
                .SingleInstance()
                .PropertiesAutowired();
            
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}