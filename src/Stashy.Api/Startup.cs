using Autofac;
using AutoMapper;
using Flurl.Http.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Stashy.Api.Infrastructure;
using Stashy.Api.Infrastructure.Clients;
using Stashy.Api.Infrastructure.Clients.CoinGecko;
using Stashy.Api.Infrastructure.Hosting;
using Stashy.Api.Infrastructure.Services;

namespace Stashy.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<StashyBackgroundService>();
            services.AddAutoMapper();
            services.AddCors(options =>
            {
                options.AddPolicy("DefaultCorsPolicy",
                    builder => builder.WithOrigins(Configuration.GetSection("Cors:Origins").Get<string[]>())
                        .AllowAnyMethod().AllowAnyHeader());
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(jo =>
            {
                jo.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                };
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<PerBaseUrlFlurlClientFactory>().As<IFlurlClientFactory>().SingleInstance();
            builder.RegisterType<CoinService>().As<ICoinService>().SingleInstance();
            builder.RegisterType<CoinGeckoClient>().As<IClient>().InstancePerLifetimeScope();
            builder.RegisterType<CoinManager>().As<ICoinManager>().InstancePerLifetimeScope();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMapper autoMapper)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            autoMapper.ConfigurationProvider.AssertConfigurationIsValid();

            app.UseCors("DefaultCorsPolicy");
            app.UseMvc();
        }
    }
}
