using System.IO;
using DaGetV2.Api.Filters;
using DaGetV2.Dal.EF;
using DaGetV2.Dal.Interface;
using DaGetV2.Service;
using DaGetV2.Service.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;

namespace DaGetV2.Api
{
    public class Startup
    { 
        private IHostingEnvironment CurrentEnvironment { get; set; }

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }        

        public void ConfigureServices(IServiceCollection services)
        {
            var cs = Configuration.GetConnectionString("DaGetConnexionString");
            services.Configure<AppConfiguration>(Configuration.GetSection("AppConfiguration"));
            var conf = Configuration.GetSection("AppConfiguration").Get<AppConfiguration>();

            var serviceProvider = services.BuildServiceProvider();
            var loggerServiceFactory = serviceProvider.GetService<ILoggerFactory>();

            services.AddSingleton<IContextFactory>(cf => new EfContextFactory(cs));

            services.AddTransient<IBankAccountService>(bas => new BankAccountService()
            {
                Configuration = conf
            });
            services.AddTransient<IBankAccountTypeService>(bats => new BankAccountTypeService()
            {
                Configuration = conf
            });
            services.AddTransient<IOperationTypeService>(ots => new OperationTypeService()
            {
                Configuration = conf
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "DaGet Gui API", Version = "v1" });
                var xmlPath = Path.ChangeExtension(typeof(Startup).Assembly.Location, ".xml");
                c.IncludeXmlComments(xmlPath);
            });

            services
                .AddMvc(options => options.Filters.Add(new DaGetExceptionFilter(CurrentEnvironment, loggerServiceFactory)))
                .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            loggerFactory.AddNLog();
            NLog.LogManager.LoadConfiguration("nlog.config");

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "DaOAuth Gui API");
            });

            app.UseMiddleware<DaOAuthIntrospectionMiddleware>();            
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
