using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Remosys.Api.Core.Application.System;
using Remosys.Common.Logging;
using Serilog;

namespace Remosys.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                #region SeriLog

                var seriLogSetting = new SeriLogSetting();
                config.GetSection("SeriLogSetting").Bind(seriLogSetting);

                Log.Logger = new LoggerConfiguration()
                    .WriteTo.Console()
                    .MinimumLevel.Information()
                    .WriteTo.Seq(seriLogSetting.Address)
                    .CreateLogger();
                Log.Information(" SeriLog Initialized on {Address} ... ", seriLogSetting.Address);

                #endregion SeriLog

                #region SeedData

                var mediator = services.GetRequiredService<IMediator>();
                await mediator.Send(new SampleSeedDataCommand(), CancellationToken.None);

                #endregion

                await host.RunAsync(CancellationToken.None);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while migrating or initializing the database.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseSerilog();
    }
}
