using AutoMapper;
using FindRoommate.Infrastructure;
using FindRoommate.Infrastructure.Validators;
using FindRoommate.Models;
using FindRoommate.Models.Advert;
using FindRoommate.Models.AdvertImage;
using FindRoommate.Models.UserProfile;
using FindRoommate.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vereyon.Web;

namespace FindRoommate
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration["Data:ConnectionString"]));
            services.AddIdentity<AppUser, IdentityRole>(opts =>
                {
                    opts.Password.RequireLowercase = false;
                    opts.Password.RequiredLength = 0;
                    opts.Password.RequireUppercase = false;
                    opts.Password.RequireDigit = false;
                    opts.Password.RequireNonAlphanumeric = false;
                    opts.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddTransient<IAdvertRepository, EFAdvertRepository>();
            services.AddTransient<IUserProfileRepository, EFUserProfileRepository>();
            services.AddTransient<IAdvertImageRepository, EFAdvertImageRepository>();
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<IPasswordValidator<AppUser>, CustomPasswordValidator>();
            services.AddFlashMessage();
            services.AddAutoMapper(typeof(Startup));
            services.ConfigureApplicationCookie(options => options.LoginPath = "/Account/Login");
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStatusCodePages();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: null,
                    template: "{category}/Strona{productPage:int}",
                    defaults: new { controller = "Advert", action = "List" }
                    );

                routes.MapRoute(
                    name: null,
                    template: "Strona{productPage:int}",
                    defaults: new { controller = "Advert", action = "List", productPage = 1 }
                    );

                routes.MapRoute(
                    name: null,
                    template: "{category}",
                    defaults: new { controller = "Advert", action = "List", productPage = 1 }
                    );

                routes.MapRoute(
                    name: null,
                    template: "",
                    defaults: new { controller = "Advert", action = "List", productPage = 1}
                    );

                routes.MapRoute(
                    name: null, 
                    template: "{controller}/{action}/{id?}"
                    );
            });

            
        }
    }
}
