using FullstackDevTS.Commands.Handler;
using FullstackDevTS.Db;
using FullstackDevTS.Models.Entities;
using FullstackDevTS.Repositories;
using FullstackDevTS.Repositories.TestRepository;
using FullstackDevTS.Services;
using FullstackDevTS.Services.Implementation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
namespace FullstackDevTS.Config;

public class ServiceRegistration
{
    public void OnLoad(IServiceCollection services, IConfiguration configuration)
    {
        AddLocalDatabase(services, configuration);
        ServiceOnLoad(services);
        SwaggerOnload(services);
    }
    
    private static void AddLocalDatabase(IServiceCollection services, IConfiguration configuration)
    {
        var localServer = Environment.GetEnvironmentVariable("FS_SERVER");
        var localDatabaseName = Environment.GetEnvironmentVariable("FS_DATABASENAME");
        var localUserId = Environment.GetEnvironmentVariable("FS_USERNAME");
        var localPassword = Environment.GetEnvironmentVariable("FS_PASSWORD");
        var localWindowsAuth = Environment.GetEnvironmentVariable("FS_WINDOWS_AUTH") == "1" ? "True" : "False";
    
        var localConnectionString g= configuration["connectionStrings:local_db"]
            .Replace("{FS_SERVER}", localServer)
            .Replace("{FS_DATABASENAME}", localDatabaseName)
            .Replace("{FS_USERNAME}", localUserId)
            .Replace("{FS_PASSWORD}",localPassword)
            .Replace("{FS_WINDOWS_AUTH}",localWindowsAuth);

        if (string.IsNullOrEmpty((localConnectionString)) || string.IsNullOrEmpty(localDatabaseName))
        {
            throw new InvalidOperationException("Invalid Database Server Connection");
        }
        
        services.AddDbContext<ApplicationDatabaseContext>(options => 
            options.UseSqlServer(localConnectionString,
                    provider => provider.EnableRetryOnFailure())
                .LogTo(Console.WriteLine, LogLevel.Information)
        );
    }

    private void ServiceOnLoad(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddScoped(typeof(IRepository<TestModel>),typeof(TestRepositoryData));
        services.AddScoped<ITestService, TestService>();
        
        services.AddScoped(typeof(ICategoryRepository<CategoryModel>),typeof(CategoryRepository));
        services.AddScoped(typeof(ICategoryService<CategoryModel>),typeof(CategoryService));
        
        services.AddMediatR(typeof(Program).Assembly);

        // services.AddScoped<TestDataCommandHandler>();
        // services.AddMediatR(typeof(TestDataCommandHandler).Assembly); 
        // services.AddMediatR(typeof(CreateCategoryCommandHandler).Assembly);
        // services.AddMediatR(typeof(GetAllCategoriesQueryHandler).Assembly);

    }

    private void SwaggerOnload(IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("ApiKey",new OpenApiSecurityScheme
            {
                Description = "The API Key to access the Backend Service",
                Type = SecuritySchemeType.ApiKey,
                Name = "x-api-key",
                In = ParameterLocation.Header,
                Scheme = "ApiKeyScheme"
            });
            
            var scheme = new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                },
                In = ParameterLocation.Header
            };

            var requirement = new OpenApiSecurityRequirement
            {
                { scheme, new List<string>() } 
            };
            
            c.AddSecurityRequirement((requirement));
            
        });

    }
}