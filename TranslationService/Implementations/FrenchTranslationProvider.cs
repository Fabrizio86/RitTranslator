namespace TranslationService.Implementations
{
    using Common.DTO;
    using ITranslateService;
    using Models;

    /// <summary>
    /// Represents a French translation provider that implements the ITranslationProvider interface.
    /// Provides functionality to process translation requests for the French language.
    /// </summary>
    public class FrenchTranslationProvider : TranslatorProviderBase, ITranslationProvider
    {
        /// <summary>
        /// Gets the language code associated with the translation provider.
        /// This property is intended to represent the target language code
        /// supported by the specific implementation of the translation provider
        /// (e.g., "it" for Italian, "en" for English).
        /// </summary>
        public string LanguageCode => "fr";
        
        /// <summary>
        /// Provides a French-specific implementation of the <see cref="ITranslationProvider"/> interface.
        /// Enables the handling of translation requests targeting the French language.
        /// </summary>
        public FrenchTranslationProvider(TranslatorApi api) : base(api)
        {
        }

        /// <summary>
        /// Translates the input request to the French language asynchronously.
        /// </summary>
        /// <param name="request">
        /// The <see cref="TranslationRequest"/> containing the details of the text and language to be translated.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation. The result contains a <see cref="TranslationResult"/>
        /// which indicates the success or failure of the translation along with the translated output.
        /// </returns>
        public async Task<TranslationResult> TranslateAsync(TranslationRequest request)
        {
            request.TargetLanguage = LanguageCode;
            return await this.TranslateRequestAsync(request);
        }
    }
}
