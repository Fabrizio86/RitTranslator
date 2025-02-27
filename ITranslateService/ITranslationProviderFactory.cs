namespace ITranslateService
{
    using Common.DTO;

    /// <summary>
    /// Defines a factory interface responsible for creating instances of <see cref="ITranslationProvider"/>
    /// based on the specified translation request.
    /// </summary>
    public interface ITranslationProviderFactory
    {
        /// <summary>
        /// Gets the collection of languages supported by the translation providers.
        /// </summary>
        /// <value>
        /// A collection of language codes (e.g., "en", "fr") that represent the languages
        /// for which translation providers are available.
        /// </value>
        IEnumerable<string> SupportedLanguages { get; }

        /// <summary>
        /// Retrieves the appropriate translation provider based on the language specified in the translation request.
        /// </summary>
        /// <param name="request">
        /// The translation request containing details of the target language and input sentence.
        /// </param>
        /// <returns>
        /// The <see cref="ITranslationProvider"/> that handles translations for the specified target language.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Thrown when no provider is available for the requested target language.
        /// </exception>
        ITranslationProvider GetProvider(TranslationRequest request);
    }
}
