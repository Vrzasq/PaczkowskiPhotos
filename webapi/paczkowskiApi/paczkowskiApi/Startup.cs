﻿using Commons;
using DbContract.Repository;
using DbContract.RepositoryContract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using paczkowskiApi.security;

namespace paczkowskiApi
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
            services.AddCors(options =>
                options.AddPolicy("default", builder =>
                            builder
                            .AllowAnyOrigin()
                            .AllowCredentials()
                            .AllowAnyHeader()
                            .AllowAnyMethod()));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = AuthScheme.Cookies;
            }).AddScheme<AuthenticationSchemeOptions, AuthCookieHandler>(AuthScheme.Cookies, o => { });
            services.AddOptions();
            services.Configure<AesEncryption>(Configuration.GetSection("AesEncryption"));
            services.AddScoped<IRepository, DbRepository>();
            services.AddWebEncoders();
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

            app.UseCors("default");
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
