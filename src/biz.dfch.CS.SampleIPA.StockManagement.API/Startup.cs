using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using biz.dfch.CS.SampleIPA.StockManagement.API.Data;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Builder;
using biz.dfch.CS.SampleIPA.StockManagement.API.Models;
using Microsoft.OData.Edm;

namespace biz.dfch.CS.SampleIPA.StockManagement.API
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
            services.AddControllers(mvcOptions =>
                mvcOptions.EnableEndpointRouting = false);

            services.AddOData();

            services.AddDbContext<StockManagementContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString(nameof(StockManagementContext))));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseMvc(routeBuilder =>
            {
                routeBuilder.Select().Expand().Count().Filter().OrderBy().MaxTop(100).SkipToken().Build();
                routeBuilder.MapODataServiceRoute("odata", "odata", GetEdmModel());
            });
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Product>(nameof(Product));
            builder.EntitySet<Booking>(nameof(Booking));
            builder.EntitySet<Category>(nameof(Category));
            return builder.GetEdmModel();
        }
    }
}
