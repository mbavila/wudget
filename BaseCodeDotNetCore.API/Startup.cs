// <copyright file="Startup.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.API
{
    using System;
    using System.Diagnostics;
    using System.Text;
    using System.Threading.Tasks;
    using BaseCodeDotNetCore.API.Authentication;
    using BaseCodeDotNetCore.API.Middlewares;
    using BaseCodeDotNetCore.Data;
    using BaseCodeDotNetCore.Data.DependencyInjection;
    using BaseCodeDotNetCore.Domain;
    using BaseCodeDotNetCore.Domain.Budgets;
    using BaseCodeDotNetCore.Domain.Clients;
    using BaseCodeDotNetCore.Domain.Employees;
    using BaseCodeDotNetCore.Domain.SubCategories;
    using BaseCodeDotNetCore.Domain.Transactions;
    using BaseCodeDotNetCore.Domain.Users;
    using BaseCodeDotNetCore.Utils.DtoMapper;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.AspNetCore.Server.Kestrel.Core;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;

    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">Project configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton(MapConfigurationFactory.Scan<Startup>());
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod()
                .AllowAnyHeader());
            });

            services.AddMvc();
            var connectionString = Configuration.GetConnectionString("myconn");
            services.AddDbContext<BaseCodeDbContext>(options => options.UseMySql(connectionString, 
                new MySqlServerVersion(new Version(10, 1, 40))));
            services.AddDbContextPool<IdentityDbContext<IdentityUser>>(options =>
            {
                options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString),
                    options => options.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: System.TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null)
                    );
            });

            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<BaseCodeDbContext>()
                    .AddRoles<IdentityRole>()
                    .AddDefaultTokenProviders();

            ConfigureDependencies(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(Constants.Swagger.Version, new OpenApiInfo { Title = Constants.Swagger.Title, Version = Constants.Swagger.Version });

                c.AddSecurityDefinition(Constants.Swagger.SecurityDefinitionScheme, new OpenApiSecurityScheme()
                {
                    Description = Constants.Swagger.SecurityDefinitionDescription,
                    Name = Constants.Swagger.SecurityDefinitionName,
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = Constants.Swagger.SecurityDefinitionBearerFormat,
                    Scheme = Constants.Swagger.SecurityDefinitionScheme,
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = Constants.Swagger.SecurityDefinitionScheme,
                            },
                        },
                        Array.Empty<string>()
                    },
                });
            });

            services.AddScoped<ModelValidationAttribute>();

            var authSecretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration[Constants.Config.SecretKey]));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = authSecretKey,

                    RequireSignedTokens = true,
                    RequireExpirationTime = true,

                    ValidIssuer = Configuration[Constants.Config.Issuer],
                    ValidAudience = Configuration[Constants.Config.Audience],

                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add(Constants.Common.TokenExpired, Constants.Generic.TrueString);
                        }
                        return Task.CompletedTask;
                    },
                };
            });

            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            //services.AddHangfire(configuration => configuration
            //        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            //        .UseSimpleAssemblyNameTypeSerializer()
            //        .UseRecommendedSerializerSettings()
            //        .UseStorage(new MySqlStorage(
            //            Configuration.GetConnectionString("myconn"),
            //            new MySqlStorageOptions
            //            {
            //                TablesPrefix = "Hangfire"
            //            }
            //        )));

            //services.AddHangfireServer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(Constants.Swagger.URL, Constants.Swagger.Name);
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<TokenProviderMiddleware>();

            app.UseMiddleware<RefreshTokenProviderMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //GlobalConfiguration.Configuration.UseStorage(new MySqlStorage(
            //            Configuration.GetConnectionString("myconn"),
            //            new MySqlStorageOptions
            //            {
            //                TablesPrefix = "Hangfire"
            //            }
            //));

            //// Uncomment this to test Hangfire
            //// RecurringJob.AddOrUpdate<IJobService>(job => job.SendDailyEmailBlast(), Cron.Daily());
            //app.UseHangfireDashboard();

            //app.UseHangfireServer();
        }

        private void ConfigureDependencies(IServiceCollection services)
        {
            services.AddScoped<ClaimsProvider, ClaimsProvider>();
            services.AddScoped<ClaimsProvider>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISubCategoryService, SubCategoryService>();
            services.AddScoped<IBudgetService, BudgetService>();
            services.AddScoped<ITransactionService, TransactionService>();
            UnitOfWorkServiceCollectionExtentions.AddUnitOfWork<BaseCodeDbContext>(services);
        }
    }
}
