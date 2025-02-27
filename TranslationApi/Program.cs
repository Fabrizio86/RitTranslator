namespace TranslationApi
{
    using Azure.Identity;
    using Common.DTO;
    using ITranslateService;
    using TranslationService;
    using TranslationService.Implementations;
    using TranslationService.Models;

    /// <summary>
    /// The Program class serves as the entry point for the Translation API application,
    /// configuring and starting the web application. It sets up the service container,
    /// middleware pipeline, and HTTP API endpoints for translation-related operations.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The entry point for the application.
        /// </summary>
        /// <param name="args">An array of command-line arguments passed to the application.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
        
            var keyVaultName = builder.Configuration["KeyVaultName"];
            if (!string.IsNullOrWhiteSpace(keyVaultName))
            {
                var keyVaultUri = new Uri($"https://{keyVaultName}.vault.azure.net/");
        
                builder.Configuration.AddAzureKeyVault(
                    keyVaultUri,
                    new DefaultAzureCredential()
                );
            }

            builder.Services.AddScoped(sp =>
                {
                    // Fetch values directly from Key Vault using builder.Configuration
                    var config = sp.GetRequiredService<IConfiguration>();
        
                    return new TranslatorApi
                    {
                        ApiKey = config["TranslatorApi-ApiKey"]!,
                        ApiUrl = config["TranslatorApi-ApiUrl"]!,
                        Region = config["TranslatorApi-Region"]!
                    };
                });

            // Add CORS
            builder.Services.AddCors(options =>
                {
                    options.AddDefaultPolicy(policy =>
                        {
                            policy.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader();
                        });
                });
        
            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add scoped translation service
            builder.Services.AddScoped<ITranslationProviderFactory, TranslationProviderFactory>();
            builder.Services.AddScoped<ITranslateService, TranslationService>();

            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors();
            app.UseHttpsRedirection();

            app.UseAuthorization();

            // Map the routes
            app.MapPost("/translate", async (ITranslateService translateService, TranslationRequest request) =>
                    {
                        var result = await translateService.TranslateAsync(request);
                        return Results.Ok(result);
                    })
                .WithName("Translate")
                .WithOpenApi();

            app.MapGet("/languages", (ITranslationProviderFactory providerFactory) =>
                    {
                        var languages = providerFactory.SupportedLanguages;
                        return Results.Ok(languages);
                    })
                .WithName("GetSupportedLanguages")
                .WithOpenApi();

            app.Run();
        }
    }
}
