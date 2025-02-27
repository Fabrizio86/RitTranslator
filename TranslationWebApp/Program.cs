namespace TranslationWebApp
{
    using Clients;
    using ITranslateService;

    /// <summary>
    /// Represents the main entry point of the TranslationWebApp application.
    /// </summary>
    /// <remarks>
    /// This class is responsible for configuring and running the web application.
    /// It sets up services, middleware, and routes.
    /// </remarks>
    public class Program
    {
        /// <summary>
        /// The main entry point for the TranslationWebApp application.
        /// </summary>
        /// <param name="args">An array of command-line arguments passed to the application.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHttpClient<ITranslationServiceClient, TranslationServiceClient>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<ITranslationServiceClient, TranslationServiceClient>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Translate}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
