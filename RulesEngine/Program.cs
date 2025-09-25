using RulesEngine.Application;
using RulesEngine.Infrastructure;
using RulesEngineAPI.Extensions;
using RulesEngineAPI.Infrastructure;
using Serilog;

namespace RulesEngineAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((context, loggerContext) =>
                loggerContext.ReadFrom.Configuration(context.Configuration));

            // Add services to the container.
            builder.Services.AddAPI()
                .AddApplication()
                .AddInfrastructure(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseMiddleware<RequestLogContextMiddleware>();
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
            app.UseMiddleware<TenantResolver>();
            app.MapControllers();
            app.Run();
        }
    }
}