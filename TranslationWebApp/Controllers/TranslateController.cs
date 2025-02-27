namespace TranslationWebApp.Controllers
{
    using Common.DTO;
    using ITranslateService;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Controller responsible for handling requests related to the home page and other static pages of the application.
    /// </summary>
    public class TranslateController : Controller
    {
        private readonly ILogger<TranslateController> logger;
        private readonly ITranslationServiceClient client;
        private const string ErrorWhileFetchingSupportedLanguages = "Error while fetching supported languages, please try again later.";

        /// <summary>
        /// Controller responsible for handling translation-related requests
        /// within the application.
        /// </summary>
        public TranslateController(ILogger<TranslateController> logger, ITranslationServiceClient client)
        {
            this.logger = logger;
            this.client = client;
        }

        /// <summary>
        /// Handles requests for the main page or other related entry points of the application.
        /// </summary>
        /// <returns>Returns an IActionResult that represents the view for the main page.</returns>
        public async Task<IActionResult> Index()
        {
            var languages = new List<string>();
            
            try
            {
                languages = await this.client.GetSupportedLanguagesAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ErrorWhileFetchingSupportedLanguages);
                ViewBag.ErrorMessage = ErrorWhileFetchingSupportedLanguages;
            }

            ViewBag.Languages = languages;
            return View();
        }

        /// <summary>
        /// Translates a given input sentence into the specified target language.
        /// </summary>
        /// <param name="request">An object containing the input sentence to be translated and the target language for the translation.</param>
        /// <returns>A <see cref="TranslationResult"/> object containing the success status, the translated sentence, or any error messages if applicable.</returns>
        [HttpPost("api/translate")]
        public async Task<TranslationResult> Translate([FromBody] TranslationRequest request)
        {
            try
            {
                return (await this.client.TranslateAsync(request))!;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occurred while translating the sentence");
                return new TranslationResult
                {
                    Success = false, TranslatedSentence = string.Empty, ErrorMessage = "An unexpected error occurred. Please try again later."
                };
            }
        }
    }
}
