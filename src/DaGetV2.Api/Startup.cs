namespace DaGetV2.Api
{
    using System;
    using System.IO;
    using ApplicationCore.Domain;
    using ApplicationCore.Interfaces;
    using ApplicationCore.Services;
    using ApplicationCore.Tools;
    using Filters;
    using Infrastructure.Data;
    using Infrastructure.Interfaces;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using NLog.Extensions.Logging;
    using Shared.Constant;
    using Swashbuckle.AspNetCore.Swagger;

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
            services.Configure<AppConfiguration>(Configuration.GetSection("AppConfiguration"));
            var conf = Configuration.GetSection("AppConfiguration").Get<AppConfiguration>();

            var serviceProvider = services.BuildServiceProvider();
            var loggerServiceFactory = serviceProvider.GetService<ILoggerFactory>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            if (conf.DataBaseType.Equals(DataBaseType.CosmosDb))
            {
                services.AddSingleton(cf => BuildCosmosDbEfContextFactory());
            }
            else
            {
                services.AddSingleton(cf => BuildSqlServerDbEfContextFactory());
            }     

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
            services.AddTransient<IOperationService>(ots => new OperationService()
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

        private IContextFactory BuildCosmosDbEfContextFactory()
        {
            var cosmosSettings = Configuration.GetSection("CosmosSettings").Get<CosmosSettings>();
            var contextFactory = new CosmosDbEfContextFactory(
                 cosmosSettings.ServiceEndpoint,
                 cosmosSettings.PrimaryKey,
                 cosmosSettings.DatabaseName);

            using (var context = (DaGetContext)contextFactory.CreateContext())
            {
                if (context.Database.EnsureCreated())
                {
                    context.BankAccountTypes.Add(new BankAccountType()
                    {
                        Id = BankAccountTypeIds.Current,
                        Wording = "Courant",
                        CreationDate = DateTime.Now,
                        ModificationDate = DateTime.Now
                    });
                    context.BankAccountTypes.Add(new BankAccountType()
                    {
                        Id = BankAccountTypeIds.Saving,
                        Wording = "Epargne",
                        CreationDate = DateTime.Now,
                        ModificationDate = DateTime.Now
                    });
                    context.Commit();
                }
            }

            return contextFactory;
        }

        private IContextFactory BuildSqlServerDbEfContextFactory()
        {
            var cs = Configuration.GetConnectionString("DaGetConnexionString");
            return new SqlServerEfContextFactory(cs);
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
