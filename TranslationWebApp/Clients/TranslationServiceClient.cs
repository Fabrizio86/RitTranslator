namespace TranslationWebApp.Clients
{
    using Common.DTO;
    using ITranslateService;

    /// <summary>
    /// A client for interacting with the translation service API. It provides methods to
    /// retrieve supported languages and perform text translations.
    /// </summary>
    public class TranslationServiceClient : ITranslationServiceClient
    {
        /// <summary>
        /// An instance of <c>HttpClient</c> used to send HTTP requests to the translation service API.
        /// It is utilized to perform communication with the API, such as sending requests for retrieving
        /// supported languages or translating text. The client should be properly configured for the
        /// intended API endpoints and authentication, if required.
        /// </summary>
        private readonly HttpClient httpClient;

        /// <summary>
        /// A client for interacting with a translation service, providing methods
        /// for retrieving supported languages and translating text.
        /// </summary>
        public TranslationServiceClient(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            var baseAddress = configuration["TranslationService:BaseAddress"];
            httpClient.BaseAddress = new Uri(baseAddress);
        }

        /// <summary>
        /// Asynchronously retrieves a list of supported languages for translation.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of supported language codes as strings.</returns>
        public async Task<List<string>> GetSupportedLanguagesAsync()
        {
            var response = await httpClient.GetFromJsonAsync<List<string>>("/languages");
            return response ?? [];
        }

        /// <summary>
        /// Sends a translation request to the translation service and retrieves the result.
        /// </summary>
        /// <param name="request">An instance of <see cref="TranslationRequest"/> containing the sentence to be translated and the target language.</param>
        /// <returns>A <see cref="TranslationResult"/> object containing the translated sentence, success status, and any error messages.</returns>
        public async Task<TranslationResult?> TranslateAsync(TranslationRequest request)
        {
            var response = await httpClient.PostAsJsonAsync("/translate", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TranslationResult>();
        }
    }
}
