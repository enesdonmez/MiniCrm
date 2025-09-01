using Carter;
using Microsoft.EntityFrameworkCore;
using MiniCrmApi;
using MiniCrmApi.Data;
using MiniCrmApi.Middlewares;
using Npgsql;
using NpgsqlTypes;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Sinks.PostgreSQL;
using System;
using System.Diagnostics;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenTelemetry().WithTracing(configure =>
        {
            configure
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MiniCrmApi"))
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddNpgsql()
                .AddOtlpExporter();
        })
       .WithMetrics(metricsProviderBuilder =>
       {
           metricsProviderBuilder
               .AddAspNetCoreInstrumentation()
               .AddHttpClientInstrumentation()
               .AddNpgsqlInstrumentation()
               .AddOtlpExporter();
       });
        builder.Services.AddProjectServices(builder.Configuration);
        builder.Services.AddCors();
        IDictionary<string, ColumnWriterBase> columnWriters = new Dictionary<string, ColumnWriterBase>
        {
            {"message", new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
            {"message_template", new MessageTemplateColumnWriter(NpgsqlDbType.Text) },
            {"level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
            {"raise_date", new TimestampColumnWriter(NpgsqlDbType.Timestamp) },
            {"exception", new ExceptionColumnWriter(NpgsqlDbType.Text) },
            {"properties", new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb) },
            {"props_test", new PropertiesColumnWriter(NpgsqlDbType.Jsonb) },
            {"machine_name", new SinglePropertyColumnWriter("MachineName", PropertyWriteMethod.ToString, NpgsqlDbType.Text, "l") }
        };
        Log.Logger = new LoggerConfiguration()
                   .MinimumLevel.Information()
                   .Enrich.FromLogContext()
                   .WriteTo.Console()
                   .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                   .WriteTo.PostgreSQL(
                    connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
                    tableName: "Logs",
                    needAutoCreateTable: true,
                    columnOptions: columnWriters)
                   .CreateLogger();

        builder.Host.UseSerilog();


        var app = builder.Build();
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<CrmDbContext>();
            db.Database.Migrate();
            if (!db.Customers.Any())
            {

                var fakeCustomers = FakeDataGenerator.GenerateCustomers(100);
                db.Customers.AddRange(fakeCustomers);
                db.SaveChanges();
            }

        }
        app.MapOpenApi();
        app.MapScalarApiReference(sc =>
        {
            sc.Theme = ScalarTheme.DeepSpace;
            sc.Title = "Mini Crm API";
            sc.DefaultOpenAllTags = true;
            sc.HideModels = true;
        });
        app.Use(async (context, next) =>
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                Activity.Current!.SetStatus(Status.Error);
                Activity.Current!.AddEvent(new ActivityEvent("error"));
                Activity.Current!.AddTag("error.message", ex.Message);
                Activity.Current.AddTag("error.stack.trace", ex.StackTrace);

                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(ex.Message);
            }
        });
        app.UseMiddleware<ReqAndResActivityBodyMiddleware>();
        app.UseCors(policy => policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin());
        app.UseHttpsRedirection();
        app.UseHsts();
        app.UseResponseCompression();
        app.UseRateLimiter();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapCarter();

        app.Run();
    }
}