using System;
using Autofac;
using AutoMapper;
using Flurl.Http.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Stashy.Api.Infrastructure.Clients;
using Stashy.Api.Infrastructure.Clients.CoinGecko;
using Stashy.Api.Infrastructure.Hosting;
using Stashy.Api.Infrastructure.Services;
using Stashy.Extensions;

namespace Stashy.Api.Infrastructure
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
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddCors(options =>
            {
                string[] origins = Configuration.GetSection("Cors:Origins")
                    .Get<string[]>();
                options.AddPolicy("DefaultCorsPolicy", builder => builder.WithOrigins(origins)
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
            services.AddMemoryCache();
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(jo =>
                {
                    jo.SerializerSettings.ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    };
                });
            services.AddApiDocumentation();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<PerBaseUrlFlurlClientFactory>()
                .As<IFlurlClientFactory>()
                .SingleInstance();
            builder.RegisterType<CoinService>()
                .As<ICoinService>()
                .SingleInstance();
            builder.RegisterType<CoinGeckoClient>()
                .As<IClient>()
                .InstancePerLifetimeScope();
            builder.RegisterType<CoinManager>()
                .As<ICoinManager>()
                .InstancePerLifetimeScope();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMapper autoMapper, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // validate mapping configuration
            autoMapper.ConfigurationProvider.AssertConfigurationIsValid();

            app.UseCors("DefaultCorsPolicy");
            app.UseStaticFiles();
            app.UseMvc();
            app.UseMvc();
            app.UseApiDocumentation(provider);
        }
    }
}
