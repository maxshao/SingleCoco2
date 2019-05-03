using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SingleCoco.Repository;
using SingleCoco.Repository.System;
using SingleCoco.Api.Controllers;
using SingleCoco.Entities;
using SingleCoco.Infrastructure.Dapper;
using FluentValidation.AspNetCore;
using SingleCoco.Entities.Systerm;
using FluentValidation;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using NLog.Web;

namespace SingleCoco.Api
{
    public class Startup
    {
        public IContainer ApplicationContainer { get; private set; }
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // change return type - void as IServiceProvider for Autofac
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            //Use Jwt bearer authentication
            string issuer = Configuration["Jwt:Issuer"];
            string audience = Configuration["Jwt:Audience"];
            string expire = Configuration["Jwt:ExpireMinutes"];
            TimeSpan expiration = TimeSpan.FromMinutes(Convert.ToDouble(expire));
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecurityKey"]));

            services.AddAuthorization(options =>
            {
                //1、Definition authorization policy
                options.AddPolicy("Permission",
                   policy => policy.Requirements.Add(new PolicyRequirement()));
            }).AddAuthentication(s =>
            {
                //2、Authentication
                s.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(s =>
            {
                //3、Use Jwt bearer 
                s.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = key,
                    ClockSkew = expiration,
                    ValidateLifetime = true
                };
                s.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        //Token expired
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });




            // FulentValidation
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddFluentValidation(fv =>
            {
                //var types = Assembly.GetExecutingAssembly().GetTypes().Where(p => p.BaseType.GetInterfaces().Any(x => x == typeof(IValidator)));
                //foreach (var type in types)
                //{
                //    var genericType = typeof(IValidator<>).MakeGenericType(type.BaseType.GenericTypeArguments[0]);
                //    services.AddSingleton(genericType, type);
                //}

                //fv.RegisterValidatorsFromAssemblyContaining<Accounts>();
                //fv.RegisterValidatorsFromAssemblyContaining<Players>();
                string path = $"{ AppContext.BaseDirectory}SingleCoco.Entities.dll";
                fv.RegisterValidatorsFromAssembly(Assembly.LoadFrom(path));
            });


            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "Test API", Version = "v1" });

                //Add Jwt Authorize to http header
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",//Jwt default param name
                    In = "header",//Jwt store address
                    Type = "apiKey"//Security scheme type
                });
                //Add authentication type
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });

            });

            // init database connection
            IConfigurationBuilder configbuilder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json");
            IConfigurationRoot ConfigurationRoot = configbuilder.Build();
            AppSettings.SetAppSettings(ConfigurationRoot, typeof(Entities.Utility.ConnectionStrings).GetEnumNames());

            //  Autofac
            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterAssemblyTypes(typeof(AccountsRepository).Assembly).AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(AccountsRepository).Assembly)
               .Where(t => t.Name.EndsWith("Repository"))
               .AsImplementedInterfaces().InstancePerDependency();

            builder.RegisterAssemblyTypes(typeof(IDbConnectionFactory).Assembly)
               .AsImplementedInterfaces().InstancePerDependency();


            // 注册权限的处理方法
            builder.RegisterType<PermissionsPolicyHandler>().As<IAuthorizationHandler>();


            ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(ApplicationContainer);
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

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui(HTML, JS, CSS,etc.)
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SingleCoco");
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
