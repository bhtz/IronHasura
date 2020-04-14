﻿using IronHasura.Configurations;
using IronHasura.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Westwind.AspNetCore.Markdown;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;

namespace IronHasura
{
    public class Startup
    {
        private string ConnexionString { get; set; }
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            this.ConnexionString = Configuration.GetConnectionString("IRONHASURA_DATA_CONNECTION_STRING");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkNpgsql()
                .AddDbContext<IronHasuraDbContext>(opt => opt.UseNpgsql(this.ConnexionString));

            services.AddCors();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                    );
            });

            services.AddIdentityConfiguration();
            services.AddIdentityServerConfiguration();
            services.AddAuthConfiguration(this.Configuration);
            // services.AddAuthorization(o => {
            //     o.AddPolicy("Administrator", policy => policy.RequireRole("Admin"));
            // });

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddSwaggerConfiguration();
            services.AddStorageConfiguration(this.Configuration);
            services.AddMarkdownConfiguration();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseStaticFiles();

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseCors("AllowSpecificOrigin");
            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMarkdown();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }
    }
}
