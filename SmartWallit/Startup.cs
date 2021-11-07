using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartWallit.Core.Interfaces;
using SmartWallit.Core.Models;
using SmartWallit.Extensions;
using SmartWallit.Helpers;
using SmartWallit.Infrastructure.Data;
using SmartWallit.Infrastructure.Data.Repositories;
using SmartWallit.Infrastructure.Identity;
using SmartWallit.Infrastructure.Services;

namespace SmartWallit
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
            services.AddDbContext<WalletContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("SmartWallit"))
            );

            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Identity"))
            );

            services.AddTransient<IWalletRepository, WalletRepository>();
            services.AddTransient<ICardRepository, CardRepository>();
            services.AddTransient<ILogRepository, LogRepository>();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEncryptionService, EncryptionService>();

            services.AddAutoMapper(typeof(MappingProfiles));

            services.AddIdentityServices(Configuration);

            services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase);

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context => new ValidationFailedResult(context.ModelState);
            }); // Transform Entity validation errors into your own model

            services.AddSwaggerDocumentation();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(x => x
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());

                app.UseSwaggerDocumentation();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.ConfigureExceptionMiddleware();

            app.UseAuthentication(); // Must come before Authorization

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
