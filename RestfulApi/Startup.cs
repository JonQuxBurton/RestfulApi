using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AspNetCoreRateLimit;

namespace RestfulApi
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
            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>((options) => {
                options.GeneralRules = new List<RateLimitRule> {
                    new RateLimitRule{
                        Endpoint = "*",
                        Limit = 100,
                        Period = "5m"
                    }
                };
            });

            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();

            services.AddHttpCacheHeaders(
                (expirationModelOptions) =>
                {
                    expirationModelOptions.MaxAge = 600;
                },
                (validationModelOptions) =>
                {
                    validationModelOptions.AddMustRevalidate = true;
                });

            services.AddMvc(setupAction => {
                setupAction.ReturnHttpNotAcceptable = true;
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseIpRateLimiting();
            app.UseHttpCacheHeaders();

            app.UseMvc();
        }
    }
}
