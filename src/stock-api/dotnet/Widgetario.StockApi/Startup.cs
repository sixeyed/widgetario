using EasyCaching.Core;
using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders.Thrift;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Util;
using Prometheus;
using Widgetario.StockApi.Caching;
using Widgetario.StockApi.Entities;

namespace Widgetario.StockApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<StockContext>(options =>
                     options.UseNpgsql(Configuration.GetConnectionString("StockDb"), postgresOptions => postgresOptions.EnableRetryOnFailure())
                            .UseSnakeCaseNamingConvention(),
                     ServiceLifetime.Scoped);

            if (Configuration.GetValue<bool>("Tracing:Enabled"))
            {
                services.AddSingleton<ITracer>(serviceProvider =>
                {
                    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                    var sampler = new ConstSampler(sample: true);

                    var reporter = new RemoteReporter.Builder()
                        .WithLoggerFactory(loggerFactory)
                        .WithSender(new UdpSender(Configuration["Tracing:Target"], 6831, 0))
                        .Build();

                    var tracer = new Tracer.Builder("Widgetario.StockApi")
                        .WithLoggerFactory(loggerFactory)
                        .WithSampler(sampler)
                        .WithReporter(reporter)
                        .Build();

                    GlobalTracer.Register(tracer);
                    return tracer;
                });
                services.AddOpenTracing();
            }
            else
            {
                services.AddSingleton(GlobalTracer.Instance);
            }

            if (Configuration.GetValue<bool>("Caching:Enabled"))
            {
                services.AddEasyCaching(options =>
                {
                    options.UseInMemory("default");
                });
            }
            else
            {
                services.AddSingleton<IEasyCachingProvider>(new NoopCachingProvider());
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseMetricServer();
            app.UseHttpMetrics();  

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
