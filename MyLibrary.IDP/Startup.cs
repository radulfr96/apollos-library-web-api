// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyLibrary.IDP.Model;
using MyLibrary.IDP.Services;
using MyLibrary.IDP.UnitOfWork;

namespace MyLibrary.IDP
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // uncomment, if you want to add an MVC-based UI
            services.AddControllersWithViews();

            var optionsBuilder = new DbContextOptionsBuilder<MyLibraryContext>();
            services.AddDbContext<MyLibraryContext>(options => options.UseSqlServer(Configuration.GetSection("ConnectionString").Value));
            var context = new MyLibraryContext(optionsBuilder.Options);

            services.AddScoped<IUserService>(provider =>
            {
                return new UserService(new UserUnitOfWork(provider.GetRequiredService<MyLibraryContext>()), new PasswordHasher<User>());
            });

            var builder = services.AddIdentityServer(options =>
            {
                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                options.EmitStaticAudienceClaim = true;
            });

            builder.AddProfileService<ProfileService>();

            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();

            builder.AddConfigurationStore(opt =>
            {
                opt.ConfigureDbContext = builder =>
                builder.UseSqlServer(Configuration.GetSection("ConnectionString").Value);
            });

            builder.AddOperationalStore(opt =>
            {
                opt.ConfigureDbContext = builder =>
                builder.UseSqlServer(Configuration.GetSection("ConnectionString").Value);
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // uncomment if you want to add MVC
            app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();

            // uncomment, if you want to add MVC
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
