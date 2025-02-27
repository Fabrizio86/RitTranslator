namespace TranslationService.Implementations
{
    using Azure;
    using Azure.AI.Translation.Text;
    using Common.DTO;
    using Models;

    /// <summary>
    /// Serves as a base class for translation providers in the system.
    /// Provides a foundational structure for implementing specific translation provider functionalities.
    /// </summary>
    public class TranslatorProviderBase
    {
        /// <summary>
        /// Provides a client instance for interacting with a text translation service,
        /// facilitating the processing and management of translation tasks.
        /// </summary>
        protected TextTranslationClient TranslationClient;

        /// <summary>
        /// Provides a base class for translation providers, allowing for the implementation of
        /// specific translation functionalities through the integration with a TranslatorApi instance.
        /// </summary>
        public TranslatorProviderBase(TranslatorApi api)
        {
            this.TranslationClient = new TextTranslationClient(new AzureKeyCredential(api.ApiKey),
                new Uri(api.ApiUrl),
                api.Region,
                new TextTranslationClientOptions()
            );
        }

        /// <summary>
        /// Asynchronously processes a translation request and returns the translated text
        /// based on the input sentence and target language specified in the request.
        /// </summary>
        /// <param name="request">The translation request containing the input sentence and target language.</param>
        /// <returns>A task that represents the asynchronous operation, with the translated text as the result.</returns>
        protected async Task<TranslationResult> TranslateRequestAsync(TranslationRequest request)
        {
            var response = await this.TranslationClient.TranslateAsync(
                content: [request.InputSentence],
                targetLanguage: request.TargetLanguage
            );

            var result = new TranslationResult();

            if (string.IsNullOrWhiteSpace(request.InputSentence))
            {
                result.Success = false;
                result.TranslatedSentence = string.Empty;
                result.ErrorMessage = "Input sentence cannot be null or empty.";
                return result;
            }

            try
            {
                request.TargetLanguage = request.TargetLanguage;
                result.Success = true;
                result.TranslatedSentence = response.Value.First().Translations.First().Text;
                result.ErrorMessage = string.Empty;
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.TranslatedSentence = string.Empty;
                result.ErrorMessage = ex.Message;
                return result;
            }
        }
    }
}
