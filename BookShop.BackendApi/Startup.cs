using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Hangfire;
using BookShop.BackendApi.Provider;
using Hangfire.SqlServer;
using System;
using Microsoft.AspNetCore.Identity;
using BookShop.Data.Entities;
using BookShop.Data.EF;
using Microsoft.EntityFrameworkCore;
using BookShop.BackendApi.Utils;
using HangfireBasicAuthenticationFilter;
using FluentValidation.AspNetCore;
using FluentValidation;
using BookShop.BackendApi.Models;
using BookShop.BackendApi.Validate;

namespace BookShop.BackendApi 
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
            services.AddDbContext<BookShopDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString(SystemConstant.MainConnectionString)));
            services.AddIdentity<User, Role>().AddEntityFrameworkStores<BookShopDbContext>().AddDefaultTokenProviders();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "BookShop API",
                    Version = "v2",
                    Description = "Test Api for BookShop website",
                });
            });

            services.AddTransient<UserManager<User>, UserManager<User>>();
            services.AddTransient<IUserApiService, UserApiService>();
            services.AddTransient<IValidator<LoginRequest>, LoginValidate>();
            services.AddTransient<IValidator<RegisterRequest>, RegisterValidate>();
            services.AddTransient<ICronjob, Cronjob>();
            services.AddControllersWithViews();
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("HangFireDb")));

            services.AddHangfireServer();
            services.AddControllers().AddFluentValidation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v2/swagger.json", "BookShop API "));
            app.UseAuthorization();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                AppPath = null,
                DashboardTitle = "HangFire DashBoard",
                Authorization = new[]
                {
                    new HangfireCustomBasicAuthenticationFilter
                    {
                        User = Configuration.GetSection("HangfireCredentials:UserName").Value,
                        Pass = Configuration.GetSection("HangfireCredentials:Password").Value
                    }
                }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
