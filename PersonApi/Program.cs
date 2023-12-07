using Microsoft.EntityFrameworkCore;
using PersonApi.Repositories;
using PersonApi.Models;
using FluentValidation;
using PersonApi.Validators;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace PersonApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(
                        formatter: new CompactJsonFormatter(),
                        path: "logs/log-.txt",
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 10
                    )
                .CreateLogger();

            // Add services to the container.
            builder.Services.AddDbContext<PeopleContext>(options => options.UseInMemoryDatabase("PeopleDb"));

            builder.Services.AddScoped<IPeopleRepository, PeopleRepository>();
            builder.Services.AddScoped<IValidator<Person>, PersonValidator>();

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}