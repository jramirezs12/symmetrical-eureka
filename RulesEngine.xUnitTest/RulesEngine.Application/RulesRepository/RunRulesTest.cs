using Autofac.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using RulesEngineAPI.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RulesEngine.Persistence;
using RulesEngine.Application;
using Microsoft.Extensions.Configuration;

namespace RulesEngine.xUnitTest.RulesEngine.Application.RulesRepository
{
    public class RunRulesTest
    {
    }

    public class Startup
    {
        private readonly IConfiguration _Configuration;

        public Startup(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        public void Configure(IApplicationBuilder app)
        {
            throw new NotImplementedException();
        }


        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddAPI();
            services.AddPersistence(configuration);
            services.AddApplication();
        }
    }

    //public abstract class BaseTest : IClassFixture<Startup>
    //{
    //    protected IMyService MyService { get; } // Replace with your actual service interface
    //    protected IDbContext DbContext { get; } // Replace with your actual DbContext interface (if applicable)

    //    protected BaseTest(IMyService myService, IDbContext dbContext) // Inject dependencies
    //    {
    //        MyService = myService;
    //        DbContext = dbContext;
    //    }

    //    protected virtual void SetupDependencies()
    //    {
    //        var factory = new WebApplicationFactory<Startup>();

    //        // You can access services from the factory.Services property here if needed
    //        // for more complex setup (e.g., mocking specific services)
    //    }
    //}
}
