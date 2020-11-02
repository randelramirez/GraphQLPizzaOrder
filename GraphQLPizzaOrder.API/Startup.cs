using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Server;
using GraphQLPizzaOrder.Data;
using GraphQLPizzaOrder.GraphQLModels.Schema;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GraphQLPizzaOrder.API
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
            // For ASP.NET Core 3.x
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddControllers();

            var migrationAssembly = typeof(PizzaOrderContext).GetTypeInfo().Assembly.GetName().Name;

            //services.AddDbContextPool<PizzaOrderContext>(options =>
            //{
            //    options.UseSqlServer(Configuration.GetConnectionString(nameof(PizzaOrderContext)), sql =>
            //        sql.MigrationsAssembly(migrationAssembly));

            //});

            services.AddDbContext<PizzaOrderContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString(nameof(PizzaOrderContext)), sql =>
                   sql.MigrationsAssembly(migrationAssembly));
            }, ServiceLifetime.Singleton);

            
            // Workaround for multiple queries
            services.AddScoped<IDocumentExecuter, EFDocumentExecuter>();
            //services.AddSingleton<IDocumentWriter, DocumentWriter>();


            services.AddCustomServices();
            services.AddGraphQLServices();
            services.AddGraphQLTypes();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, PizzaOrderContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            context.EnsureDataSeeding();
            app.UseWebSockets();

            app.UseGraphQL<PizzaOrderSchema>();
            app.UseGraphQLWebSockets<PizzaOrderSchema>();  // for GraphQL subscription


            app.UseGraphQLPlayground();
        }
    }
}
