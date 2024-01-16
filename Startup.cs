using System;
using System.IO;
using AspNetCoreRateLimit;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Meb.Api.Framework.Infrastructure.Extensions;
using Meb.Api.Framework.Infrastructure.Wrapper;
using Meb.Core.Infrastructure;
using Meb.Services.Logging;
using Meb.Services.Services.Ldap;
using Intra.Api.Services.Jwt;
using Newtonsoft;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Intra.Api.Models;
using Intra.Api.Services.DeepCell;
using Intra.Api.Services.PasswordHash;
using Microsoft.AspNetCore.Http;
using Intra.Api.Infrastructure;
using Microsoft.OpenApi.Models;


namespace Intra.Api
{
    public class Startup
    {

        #region Properties

        /// <summary>
        /// Get configuration root of the application
        /// </summary>
        public IConfigurationRoot Configuration { get; }
        private readonly IHostingEnvironment _env;

        public static IConfiguration ConfigurationPublic { get; set; }
        #endregion

        #region Ctor

        public Startup(IHostingEnvironment environment)
        {
            _env = environment;

            Configuration = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();



            ConfigurationPublic = Configuration;
        }

        #endregion

        /// <summary>
        /// Add services to the application and configure service provider
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            //Mongo Db Degister
            //services.AddScoped<IMongoDbDataService>(i => new MongoDbDataService(Configuration.GetConnectionString("DeepCellMongoDb")));


            // configure strongly typed settings objects
            // var appSettingsSection = Configuration.GetSection("AppSettings");
            // services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            //var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(Configuration["Token:SecurityKey"]);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddScoped<IPasswordHashService, PasswordHashService>();
            services.AddScoped<ILdapAuthenticationService, LdapAuthenticationService>();
            services.AddScoped<IJwtTokenHandlerService, JwtTokenHandlerService>();
            services.AddScoped<IEmailService, EmailService>();


            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = Configuration.GetSection("Swagger:Title").Value,
                    Description = Configuration.GetSection("Swagger:Description").Value,
                    Version = Configuration.GetSection("Swagger:Version").Value
                });
            });

            services.AddMebMvc();

            return services.ConfigureApplicationServices(Configuration);
        }

        /// <summary>
        /// Configure the application HTTP request pipeline
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application, IHostingEnvironment env)
        {
            // global cors policy
            application.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            application.UseCors("CorsPolicy");
            application.UseAuthentication();
            application.UseMvc();
        }

    }
}
