using System.Linq;
using System.Reflection;
using System.Text;
using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Remosys.Api.Core.AutoMapper;
using Remosys.Api.Core.Installer;
using Remosys.Api.Core.Interfaces;
using Remosys.Api.Core.Services;
using Remosys.Api.Middleware;
using Remosys.Common.Helper;
using Remosys.Common.Helper.Claims;
using Remosys.Common.Helper.Environment;
using Remosys.Common.Options;
using Remosys.Common.Sms;
using Remosys.Common.TemplateNotification;

namespace Remosys.Api.Installer
{
    public class ApiInstaller : IInstaller
    {
        public void InstallServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy",
                    builder =>
                    {
                        builder.AllowAnyHeader()
                            .AllowAnyOrigin()
                            .AllowAnyMethod();
                    });
            });

            services.AddControllers(opt => opt.Filters.Add<OnExceptionMiddleware>())
                .AddFluentValidation(mvcConfiguration =>
                    mvcConfiguration.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddSingleton<IRequestMeta, RequestMeta>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient<ITokenGenerator, TokenGenerator>();
            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.Configure<FileExtensions>(configuration.GetSection("FileExtensions"));
            services.Configure<GhasedakService>(configuration.GetSection("GhasedakService"));
            services.AddSingleton<IApplicationBootstrapper, ApplicationBootstrapper>();
            services.AddScoped<INotificationTemplateGenerator, NotificationTemplateGenerator>();
            services.AddTransient<IPayamakService, PayamakService>();

            #region AuthToken

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            var jwtSetting = new JwtSettings();
            configuration.Bind(nameof(JwtSettings), jwtSetting);
            services.AddSingleton(jwtSetting);

            var tokenValidationParameter = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSetting.Secret)),
                ValidateIssuer = false,
                ValidIssuer = jwtSetting.ValidIssuer,

                ValidAudience = jwtSetting.ValidAudience,
                ValidateLifetime = true,
                RequireExpirationTime = false
            };
            services.AddSingleton(tokenValidationParameter);

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.SaveToken = true;
                    x.TokenValidationParameters = tokenValidationParameter;
                });

            #endregion AuthToken

            #region Automapper

            var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            #endregion Automapper

            #region Api Behavior

            services.Configure<ApiBehaviorOptions>(options =>
            {
                //options.SuppressModelStateInvalidFilter = true;
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = new
                    {
                        message =
                            actionContext.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage.ToString())
                                .FirstOrDefault()
                    };
                    return new BadRequestObjectResult(errors);
                };
            });

            #endregion Api Behavior
        }
    }
}