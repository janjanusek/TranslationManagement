using System.Reflection;
using External.ThirdParty.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using TranslationManagement.Api.Filters;
using TranslationManagement.Dal;

namespace TranslationManagement.Api
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
            services.AddSingleton(_ => Configuration
                .GetSection(TranslationManagementApiConfiguration.ConfigurationSection)
                .Get<TranslationManagementApiConfiguration>()
            );
            
            services.AddControllers(options =>
            {
                options.Filters.Add<ExceptionsActionFilter>(1024);
            });
            services.AddDbContext<AppDbContext>(options => 
                options.UseSqlite("Data Source=TranslationAppDatabase.db"));
            services.AddTransient<INotificationService, UnreliableNotificationService>();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TranslationManagement.Api", Version = "v1" });
                c.CustomSchemaIds(x => x.FullName);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TranslationManagement.Api v1"));

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
