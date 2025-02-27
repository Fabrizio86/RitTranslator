namespace TranslationService
{
    using Common.DTO;
    using ITranslateService;

    /// <summary>
    /// The TranslationService class provides functionality for translating text between different languages.
    /// This service may include features like detecting the source language, translating phrases, and accessing a list of supported languages.
    /// </summary>
    public class TranslationService : ITranslateService
    {
        /// <summary>
        /// Represents a factory instance adhering to the <see cref="ITranslationProviderFactory"/> interface,
        /// used to create translation providers based on specific translation requests.
        /// It facilitates dynamic selection of translation provider implementations depending on the request attributes,
        /// such as target language, source language, or other request-specific properties.
        /// </summary>
        private readonly ITranslationProviderFactory translationProviderFactory;

        /// <summary>
        /// Provides functionality for translating text between different languages.
        /// Includes features such as detecting the source language, converting phrases,
        /// and retrieving supported language information. This service leverages
        /// translation providers determined by a factory implementation.
        /// </summary>
        public TranslationService(ITranslationProviderFactory translationProviderFactory)
        {
            this.translationProviderFactory = translationProviderFactory;
        }

        /// <summary>
        /// Translates the input sentence provided in the translation request into the target language,
        /// as specified by the implementation of the selected translation provider.
        /// </summary>
        /// <param name="request">An object containing the input sentence to be translated and other relevant parameters for translation.</param>
        /// <returns>A task representing the asynchronous operation. Upon completion, it contains the result of the translation,
        /// including the translated sentence, success status, and any error message.</returns>
        public async Task<TranslationResult> TranslateAsync(TranslationRequest request)
        {
            return await this.translationProviderFactory.GetProvider(request).TranslateAsync(request);
        }

        /// <summary>
        /// Retrieves a collection of languages supported by the translation service.
        /// This method can be used to obtain a list of all languages available for
        /// translation operations.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of supported language codes as strings.</returns>
        public Task<IEnumerable<string>> GetSupportedLanguagesAsync()
        {
            return Task.FromResult(this.translationProviderFactory.SupportedLanguages);
        }
    }
}
