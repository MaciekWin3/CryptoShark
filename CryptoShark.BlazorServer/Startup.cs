using CryptoShark.BlazorServer.Areas.Identity;
using CryptoShark.BlazorServer.Data;
using CryptoShark.DataAccessLibrary.CryptoDatabase;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Syncfusion.Blazor;
using Hangfire.SqlServer;
using Blazored.Modal;


namespace CryptoShark.BlazorServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            
            //Auth
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                   Configuration.GetConnectionString("DefaultConnection")));


            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddSingleton<WeatherForecastService>();

            //Crypto Api
            services.AddHttpClient("cryptonator", c =>
            {
                c.BaseAddress = new System.Uri(Configuration.GetValue<string>("CryptoAPI"));
            });


            //Data Access
            services.AddTransient<IHangfireJobs, HangfireJobs>();
            services.AddTransient<IDapperSqlDataAccess, DapperSqlDataAccess>();

            //Hangfire
            /*
            services.AddHangfire(config =>
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseDefaultTypeSerializer()
            );
            */

            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            services.AddHangfireServer();


            //services.AddHangfire(configuration =>
            //{
            //    configuration.UseSqlServerStorage(Configuration.GetConnectionString("Default"));
            //});

            //Syncfusion
            services.AddSyncfusionBlazor();
            services.AddSignalR(e => {
                e.MaximumReceiveMessageSize = 65536;
            });
            //For azure
            //services.AddSignalR(e => { e.MaximumReceiveMessageSize = 65536; }).AddAzureSignalR();

            //Modals
            services.AddBlazoredModal();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient backgroundJobs)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapHangfireDashboard();
            });

            //Hangfire

            

            app.UseHangfireDashboard();

            HangfireJobs Hangfire = new HangfireJobs();

            RecurringJob.AddOrUpdate(() => HangfireJobs.CallApiAndSave(Configuration.GetConnectionString("DefaultConnection")), Cron.Hourly);
            RecurringJob.AddOrUpdate(() => HangfireJobs.SaveUserData(Configuration.GetConnectionString("DefaultConnection")), Cron.Hourly);



            //Register syncfusion licence
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(Configuration.GetValue<string>("SyncfusionLicence"));
        }
    }
}
