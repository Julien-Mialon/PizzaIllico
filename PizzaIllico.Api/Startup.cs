using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PizzaIllico.Api.Domains.Accounts;
using PizzaIllico.Api.Domains.Authentications;
using Storm.Api.Launchers;

namespace PizzaIllico.Api
{
    public class Startup : BaseStartup
    {
        protected override string LogsProjectName { get; } = "pizza";
        
        public Startup(IConfiguration configuration, IWebHostEnvironment environment) : base(configuration, environment)
        {
            ForceHttps = false;
        }
        
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddNewtonsoftJson((options =>
            {
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                options.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new DefaultNamingStrategy()
                };
            }));

            services.AddCors(x => x
                .AddDefaultPolicy(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("Date")));

            base.ConfigureServices(services);



            services.AddSwaggerGenNewtonsoftSupport();

            RegisterConsoleLogger(services);

            services.UseAccountModule()
                .UseAuthenticationModule()
                ;

            services.AddControllers();
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("Date"));

            base.Configure(app, env);

            app.UseDeveloperExceptionPage();

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("Date"));
        }
    }
}