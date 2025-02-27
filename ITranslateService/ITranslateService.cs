namespace ITranslateService
{
    using Common.DTO;

    /// <summary>
    /// Represents a service for translating text from one language to another.
    /// </summary>
    public interface ITranslateService
    {
        /// <summary>
        /// Translates the input text to the target language.
        /// </summary>
        /// <param name="request">The translation request containing the input text and target language.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a TranslationResult object,
        /// which includes the translated text, a success status, and any error messages.</returns>
        Task<TranslationResult> TranslateAsync(TranslationRequest request);

        /// <summary>
        /// Retrieves a list of supported languages for translation.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IEnumerable of strings,
        /// where each string represents a language code supported by the translation service.</returns>
        Task<IEnumerable<string>> GetSupportedLanguagesAsync();
    }
}
