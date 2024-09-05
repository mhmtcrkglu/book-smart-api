using System.Data;
using BookSmart.Api.Exceptions;
using BookSmart.Api.Repositories;
using BookSmart.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace BookSmart.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors();

        services.AddHealthChecks();

        services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory =
                    context => throw new ModelValidationException(context.ModelState);
            });

        services.AddControllers(options => { options.Filters.Add<HttpGlobalExceptionFilter>(); });

        services.AddSingleton<IDbConnection>(_ =>
            new NpgsqlConnection(Configuration["ConnectionStrings:DefaultConnection"]));
        services.AddScoped<ICalendarService, CalendarService>();
        services.AddScoped<ISalesManagerRepository, SalesManagerRepository>();
        services.AddScoped<ISlotRepository, SlotRepository>();
    }

    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

        app.UseHealthChecks("/healthcheck");

        app.UseRouting();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}