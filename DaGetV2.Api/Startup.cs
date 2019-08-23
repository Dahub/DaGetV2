﻿using DaGetV2.Dal.EF;
using DaGetV2.Dal.Interface;
using DaGetV2.Service;
using DaGetV2.Service.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DaGetV2.Api
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
            string cs = Configuration.GetConnectionString("DaGetConnexionString");
            services.Configure<AppConfiguration>(Configuration.GetSection("AppConfiguration"));

            services.AddSingleton<IContextFactory>(cf => new EfContextFactory()
            {
                ConnexionString = cs
            });

            services.AddTransient<IBankAccountService>(bas => new BankAccountService());

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseMiddleware<DaOAuthIntrospectionMiddleware>();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
