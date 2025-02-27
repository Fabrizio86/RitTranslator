namespace TranslationService.Implementations
{
    using Common.DTO;
    using ITranslateService;
    using Models;

    /// <summary>
    /// Provides English language translation functionality by implementing the ITranslationProvider interface.
    /// </summary>
    /// <remarks>
    /// This class is part of the TranslationService.Implementations namespace and is designed to handle
    /// translation requests for the English language. The TranslateAsync method needs to be implemented
    /// to perform the actual translation logic.
    /// </remarks>
    public class ItalianTranslationProvider : TranslatorProviderBase, ITranslationProvider
    {
        /// <summary>
        /// Gets the language code associated with the translation provider.
        /// This property is intended to represent the target language code
        /// supported by the specific implementation of the translation provider
        /// (e.g., "it" for Italian, "en" for English).
        /// </summary>
        public string LanguageCode => "it";

        /// <summary>
        /// Implements English language translation functionalities by extending TranslatorProviderBase
        /// and adhering to the ITranslationProvider interface.
        /// </summary>
        /// <remarks>
        /// This class is responsible for processing translation requests for English, utilizing the
        /// TranslatorApi to perform necessary operations. It serves as a concrete implementation
        /// of the translation provider for English within the translation service framework.
        /// </remarks>
        public ItalianTranslationProvider(TranslatorApi api) : base(api)
        {
        }

        /// <summary>
        /// Asynchronously translates a given text input into English based on the provided translation request.
        /// </summary>
        /// <param name="request">
        /// The translation request containing the input text and relevant details for the translation process.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. On completion, contains the translation result,
        /// including the success status, translated sentence, and any error message if applicable.
        /// </returns>
        public async Task<TranslationResult> TranslateAsync(TranslationRequest request)
        {
            request.TargetLanguage = LanguageCode;
            return await this.TranslateRequestAsync(request);
        }
    }
}
