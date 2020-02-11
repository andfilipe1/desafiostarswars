using StarWars.Infra.Connection;
using StarWars.Infra.Interfaces;
using StarWars.Infra.Repositories;
using StarWarsApi.Services;
using StarWarsApi.Util;
using StarWarsApi.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace StarWarsApi
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

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "Star Wars",
                        Version = "v1",
                        Description = "API informações Star Wars",
                        Contact = new Contact
                        {
                            Name = "Luiz Filipe",
                            Url = "https://github.com/andfilipe1",
                            Email = "luiz.brandao@live.com.br"
                        }
                    });
            });


            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("ConexaoRedis");
                options.InstanceName = "SWRedisCache";
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSingleton<IConfiguration>(Configuration);
            services.AddScoped<IManageCache, ManageCache>();
            services.AddScoped<IConfigurationMongo, ConfigurationMongo>();
            services.AddScoped<IConnectMongo, ConnectionMongo>();
            services.AddScoped<IPlanetService, PlanetService>();
            services.AddScoped<IPlanetaRepository, PlanetaRepository>();
            services.AddScoped<IUtilHttpClient, UtilHttpClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Ativando middlewares para uso do Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Agenda API V1");
            });

            app.UseHttpsRedirection();

            app.UseMvc();
        }
    }
}
