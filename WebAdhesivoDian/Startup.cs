using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAdhesivoDian.Models;
using WebAdhesivoDian.Repositories;

namespace WebAdhesivoDian
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;


        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public async void ConfigureServices(IServiceCollection services)
        {

            services.AddSession(options =>
            {
                options.Cookie.Name = ".MyApp.Session";
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = ".MyApp.Cookie";
                options.Cookie.HttpOnly = false;
                options.LoginPath = "/Usuarios/Login";
                options.AccessDeniedPath = "/Usuarios/Login";
                options.ExpireTimeSpan = TimeSpan.FromDays(5);
            });


            services.AddControllersWithViews();
            services.AddAuthentication();
            services.AddTransient<RepositoryWeb>();

            services.AddDbContext<WebAdhesivoDianContext>(x =>
            {
                x.UseSqlServer(Configuration.GetConnectionString("DefaulConnection"));
            });

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<WebAdhesivoDianContext>();
            services.AddControllersWithViews();
            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, config =>
            //{
            //    config.AccessDeniedPath = "/Usuarios/Login";
            //});
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                 .AddCookie(options =>
                {
                    options.LoginPath = "/Usuarios/Login";
                    options.AccessDeniedPath = "/Usuarios/Login";
             });

            services.AddControllersWithViews(options => options.EnableEndpointRouting = false).AddSessionStateTempDataProvider();

            //List<Roles> ltsRoles = new List<Roles>();
            //ltsRoles = await _context.Roles.ToListAsync();

            services.AddAuthorization(options =>
            {


                //options.AddPolicy("CONTROLLER", policy => policy.RequireRole("Controller"));
                //options.AddPolicy("PRUEBA", policy => policy.RequireRole("Prueba"));
            });
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
                app.UseExceptionHandler("/Usuarios/Login");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseMvc(endpoints =>
            {
                endpoints.MapRoute(
                    name: "default",
                    template: "{controller=Usuarios}/{action=Login}/{id?}");
            });
            app.UseMvc(routes =>
            {
                // Resto del código de configuración de rutas

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Usuarios}/{action=Login}/{id?}");
            });
        }
    }
}
