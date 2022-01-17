using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

using TeaBar.Data;
using TeaBar.EmailService;
using TeaBar.Models;

namespace TeaBar
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<ApplicationUsers>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            //services.AddIdentity<ApplicationUsers, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;               //�K�X�n���Ʀr
                options.Password.RequireLowercase = true;           //�n���p�g�^��r��
                options.Password.RequireNonAlphanumeric = false;    //���ݭn�Ÿ��r��
                options.Password.RequireUppercase = true;           //�n���j�g�^��r��
                options.Password.RequiredLength = 6;                //�K�X�ܤ֭n6�Ӧr����
                options.Password.RequiredUniqueChars = 1;           //�ܤ֭n��1�Ӧr�����@��

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); //5�����S�����R�N�۰����w����A�w�]5����
                options.Lockout.MaxFailedAccessAttempts = 5; //�T���K�X�~�N��w���, �w�]5��
                options.Lockout.AllowedForNewUsers = true; //�s�W���ϥΪ̤]�|�Q��w�A�N�O�ǳW�S���s�H�u��

                // User settings.
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true; //�l�c���୫�Шϥ�
                
            });
            services.AddTransient<IEmailSender, EmailSender>();

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                //options.Cookie.SameSite = SameSiteMode.None;
                options.LoginPath = "/Identity/Account/Login"; //�n�J��
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";//�n�XAction
                options.SlidingExpiration = true;
            });

            services.AddControllersWithViews();
            services.AddAuthentication().AddFacebook(opt =>
            {
                opt.AppId = Configuration["Facebook:AppId"];
                opt.AppSecret = Configuration["Facebook:AppSecret"];
            });
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromDays(5);
              
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                //app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
