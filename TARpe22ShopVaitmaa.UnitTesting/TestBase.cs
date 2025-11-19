using Microsoft.AspNetCore.Hosting;
using TARpe22ShopVaitmaa.ApplicationServices.Services;
using TARpe22ShopVaitmaa.Core.ServiceInterface;
using TARpe22ShopVaitmaa.Data;
using TARpe22ShopVaitmaa.UnitTesting.Mock;
using TARpe22ShopVaitmaa.UnitTesting.Macros;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TARpe22ShopVaitmaa.SpaceshipTest.Mock;
using MockIHostEnvironment = TARpe22ShopVaitmaa.UnitTesting.Mock.MockIHostEnvironment;

namespace TARpe22ShopVaitmaa.UnitTesting
{
    public abstract class TestBase
    {
        protected IServiceProvider serviceProvider { get; set; }
        protected TestBase()
        {
            var services = new ServiceCollection();
            SetupServices(services);
            serviceProvider = services.BuildServiceProvider();
        }
        public virtual void SetupServices(IServiceCollection services)
        {
            services.AddScoped<IRealEstatesServices, RealEstatesServices>();
            services.AddScoped<IFilesServices, FilesServices>();
            services.AddScoped<IWebHost, MockIHostEnvironment>();

            services.AddDbContext<TARpe22ShopVaitmaaContext>(x =>
            {
                x.UseInMemoryDatabase("TEST");
                x.ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            }
            );

            RegisterMacros(services);
        }
        public void Dispose() { }

        protected T Svc<T>()
        {
            return serviceProvider.GetService<T>();
        }

        private void RegisterMacros(IServiceCollection services)
        {
            var macroBaseType = typeof(IMacros);
            var macros = macroBaseType.Assembly.GetTypes().Where(t => macroBaseType.IsAssignableFrom(t)
            && !t.IsInterface && !t.IsAbstract);

            foreach (var macro in macros) { services.AddSingleton(macro); }
        }
    }
}
