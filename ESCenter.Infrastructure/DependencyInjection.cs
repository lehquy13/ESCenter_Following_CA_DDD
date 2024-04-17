using System.Security.Claims;
using System.Text;
using ESCenter.Application.Interfaces.Authentications;
using ESCenter.Application.Interfaces.Cloudinarys;
using ESCenter.Infrastructure.ServiceImpls.AppLogger;
using ESCenter.Infrastructure.ServiceImpls.Authentication;
using ESCenter.Infrastructure.ServiceImpls.Cloudinary;
using ESCenter.Infrastructure.ServiceImpls.EmailServices;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Emails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace ESCenter.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<SerilogFactory>();
            services.AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));

            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<ICurrentTenantService, CurrentTenantService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Authentication configuration using jwt bearer 
            services.AddAuth(configuration);
            IdentityModelEventSource.ShowPII = true; //Add this line

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(480);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // set configuration settings to emailSettingName and turn it into Singleton
            var emailSettingNames = new EmailSettingNames();
            configuration.Bind(EmailSettingNames._SectionName, emailSettingNames);
            services.AddSingleton(Options.Create(emailSettingNames));

            // set configuration settings to cloudinarySettings and turn it into Singleton
            var cloudinary = new CloudinarySetting();
            configuration.Bind(CloudinarySetting._SectionName, cloudinary);
            services.AddSingleton(Options.Create(cloudinary));
            services.AddScoped<ICloudinaryServices, CloudinaryServices>();

            services.AddScoped<IEmailSender, EmailSender>();

            //configure BackgroundService
            //services.AddHostedService<InfrastructureBackgroundService>();
            return services;
        }

        private static IServiceCollection AddAuth(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            // set configuration settings to jwtSettings and turn it into Singleton
            var jwtSettings = new JwtSettings();
            configuration.Bind(JwtSettings._SectionName, jwtSettings);

            services.AddSingleton(Options.Create(jwtSettings));
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IValidator, Validator>();

            services.AddAuthentication(scheme =>
                {
                    scheme.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    scheme.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    scheme.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            try
                            {
                                var token = context.HttpContext.Session.GetString("access_token");
                                if (token != null)
                                {
                                    context.Token = token;
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }

                            return Task.CompletedTask;
                        },
                    };
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                    };
                });

            services.AddAuthorizationBuilder()
                .AddPolicy("RequireAdministratorRole", policy =>
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(ClaimTypes.Role, "Admin") ||
                        context.User.HasClaim(ClaimTypes.Role, "SuperAdmin")))
                .AddPolicy("RequireSuperAdministratorRole", policy => { policy.RequireRole("SuperAdmin"); })
                .AddPolicy("RequireTutorRole", policy => { policy.RequireRole("Tutor"); });

            return services;
        }
    }
}