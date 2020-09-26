using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Remosys.Api.Core.Installer;
using Remosys.Api.Core.Models;
using Remosys.Api.Middleware;
using Remosys.Common.Helper.Environment;
using Remosys.Common.Mongo;
using Remosys.Common.Swagger;
using Serilog;

namespace Remosys.Api
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
            services.InstallServicesAssembly(Configuration);
            services.AddSwaggerDocs(Configuration);

            services.AddMongo(Configuration);
            services.AddMongoRepository<User>("Users");
            services.AddMongoRepository<Role>("Roles");
            services.AddMongoRepository<UserToken>("UserTokens");
            services.AddMongoRepository<Permission>("Permissions");
            services.AddMongoRepository<Plan>("Plans");
            services.AddMongoRepository<Contract>("Contracts");

            services.AddMongoRepository<UserFile>("UserFiles");
            services.AddMongoRepository<UserActivity>("UserActivities");

            services.AddMongoRepository<Organization>("organizations");
            services.AddMongoRepository<Tool>("Tools");
            services.AddMongoRepository<ToolsCategory>("ToolCategories");
            services.AddMongoRepository<User>("Users");
            services.AddMongoRepository<Role>("Roles");
            services.AddMongoRepository<Department>("Department");
            services.AddMongoRepository<Employee>("Employees");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApplicationBootstrapper applicationBootstrapper)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            applicationBootstrapper.Initial();

            #region Static files Setting

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, ApplicationStaticPath.Avatars)),
                RequestPath = ApplicationStaticPath.Clients.Avatar
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, ApplicationStaticPath.Images)),
                RequestPath = ApplicationStaticPath.Clients.Image
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, ApplicationStaticPath.Videos)),
                RequestPath = ApplicationStaticPath.Clients.Video
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, ApplicationStaticPath.Musics)),
                RequestPath = ApplicationStaticPath.Clients.Music
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, ApplicationStaticPath.Documents)),
                RequestPath = ApplicationStaticPath.Clients.Document
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, ApplicationStaticPath.Others)),
                RequestPath = ApplicationStaticPath.Clients.Other
            });

            #endregion Static files Setting

            app.UseSerilogRequestLogging();


            app.UseRouting();
            app.UseCors("MyPolicy");
            app.UseHttpsRedirection();
            app.UseAccessControlAllowOriginAlways();
            app.UseMiddleware<ApplicationMetaMiddleware>();
            app.UseMiddleware<MembershipMiddleware>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwaggerDocs();
        }
    }
}
