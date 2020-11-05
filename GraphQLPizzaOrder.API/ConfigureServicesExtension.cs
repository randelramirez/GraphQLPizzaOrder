using GraphQL;
using GraphQL.Authorization;
using GraphQL.Server;
using GraphQL.Validation;
using GraphQLPizzaOrder.Core.Constants;
using GraphQLPizzaOrder.Core.Services;
using GraphQLPizzaOrder.Data;
using GraphQLPizzaOrder.GraphQLModels;
using GraphQLPizzaOrder.GraphQLModels.Enums;
using GraphQLPizzaOrder.GraphQLModels.InputTypes;
using GraphQLPizzaOrder.GraphQLModels.Mutations;
using GraphQLPizzaOrder.GraphQLModels.Queries;
using GraphQLPizzaOrder.GraphQLModels.Schema;
using GraphQLPizzaOrder.GraphQLModels.Subscriptions;
using GraphQLPizzaOrder.GraphQLModels.Types;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GraphQLPizzaOrder.API
{
    public static class ConfigureServicesExtension
    {
        public static void AddApplicationIdentity(this IServiceCollection services)
        {
            // Added Identity
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<PizzaOrderContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password configs
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;

                // ApplicationUser settings
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@.-_";
            });
        }

        public static void AddApplicationJWT(this IServiceCollection services, IConfiguration configuration)
        {
            // Added JWT Authentication
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(configuration.GetSection("JwtIssuerOptions:SecretKey").Value));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(configureOptions =>
            {
                configureOptions.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration.GetSection("JwtIssuerOptions:Issuer").Value,
                    ValidAudience = configuration.GetSection("JwtIssuerOptions:Audience").Value,
                    ValidateLifetime = true,
                    IssuerSigningKey = signingKey,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }

        public static void AddCustomServices(this IServiceCollection services)
        {
            // Why do we need the services to be transient
            services.AddTransient<IPizzaDetailService, PizzaDetailService>();
            services.AddTransient<IOrderDetailService, OrderDetailService>();
            services.AddTransient<IEventService, EventService>();
        }

        public static void AddGraphQLServices(this IServiceCollection services)
        {
            services.AddScoped<IServiceProvider>(c => new FuncServiceProvider(type => c.GetRequiredService(type)));
            services.AddGraphQL(options =>
            {
                options.EnableMetrics = true;
                options.UnhandledExceptionDelegate = context =>
                {
                    Console.WriteLine($"Error: {context.OriginalException.Message}");
                };
            })
            .AddSystemTextJson(deserializerSettings => {  }, serializerSettings => { })
            .AddUserContextBuilder(httpContext => new GraphQLUserContext { User = httpContext.User })
            .AddWebSockets()  // for GraphQL subscription
            .AddDataLoader()
            .AddGraphTypes(typeof(PizzaOrderSchema),ServiceLifetime.Singleton);
        }

        public static void AddGraphQLTypes(this IServiceCollection services)
        {
            // Enum
            services.AddSingleton<OrderStatusEnumType>();
            services.AddSingleton<ToppingsEnumType>();
            services.AddSingleton<CompletedOrdersSortingFieldsEnumType>();
            services.AddSingleton<SortingDirectionEnumType>();

            // Model
            services.AddSingleton<EventDataType>();

            // InputType
            services.AddSingleton<OrderDetailInputType>();
            services.AddSingleton<PizzaDetailInputType>();
            services.AddSingleton<CompletedOrderOrderByInputType>();

            // ObjectGraph Type
            services.AddSingleton<OrderDetailType>();
            services.AddSingleton<PizzaDetailType>();
            services.AddSingleton<PizzaOrderQuery>();
            services.AddSingleton<PizzaOrderMutation>();
            services.AddSingleton<PizzaOrderSubscription>();
            services.AddSingleton<PizzaOrderSchema>();
        }

        public static void AddApplicationGraphQLAuthorization(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            //services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IAuthorizationEvaluator, AuthorizationEvaluator>();
            services.TryAddSingleton<IValidationRule, AuthorizationValidationRule>();

            services.TryAddSingleton(_ =>
            {
                var authSettings = new AuthorizationSettings();

                authSettings.AddPolicy(AuthPolicy.CustomerPolicy, 
                    policy => policy.RequireClaim(ClaimTypes.Role, Roles.Customer));

                authSettings.AddPolicy(AuthPolicy.RestaurantPolicy,
                   policy => policy.RequireClaim(ClaimTypes.Role, Roles.Restaurant));

                authSettings.AddPolicy(AuthPolicy.AdminPolicy,
                  policy => policy.RequireClaim(ClaimTypes.Role, Roles.Admin, Roles.Customer, Roles.Restaurant));

                return authSettings;

            });
        }
    }
}
