using GraphQL;
using GraphQL.Server;
using GraphQLPizzaOrder.Core.Services;
using GraphQLPizzaOrder.GraphQLModels.Enums;
using GraphQLPizzaOrder.GraphQLModels.Queries;
using GraphQLPizzaOrder.GraphQLModels.Schema;
using GraphQLPizzaOrder.GraphQLModels.Types;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLPizzaOrder.API
{
    public static class ConfigureServicesExtension
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IPizzaDetailService, PizzaDetailService>();
            services.AddScoped<IOrderDetailService, OrderDetailService>();
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
            .AddSystemTextJson(deserializerSettings => { }, serializerSettings => { })
            .AddWebSockets()  // for GraphQL subscription
            .AddDataLoader()
            .AddGraphTypes(typeof(PizzaOrderSchema),ServiceLifetime.Scoped);
        }

        public static void AddGraphQLTypes(this IServiceCollection services)
        {
            services.AddScoped<OrderDetailType>();
            services.AddScoped<PizzaDetailType>();

            services.AddScoped<OrderStatusEnumType>();
            services.AddScoped<ToppingsEnumType>();

            services.AddScoped<PizzaOrderQuery>();
            services.AddScoped<PizzaOrderSchema>();
        }
    }
}
