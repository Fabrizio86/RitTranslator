namespace ITranslateService
{
    using Common.DTO;

    /// <summary>
    /// Defines the contract for a translation service client capable of providing
    /// language translation functionality and retrieving supported languages.
    /// </summary>
    public interface ITranslationServiceClient
    {
        /// <summary>
        /// Asynchronously retrieves a list of languages supported by the translation service.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of supported language codes as strings.</returns>
        Task<List<string>> GetSupportedLanguagesAsync();

        /// <summary>
        /// Translates the given input sentence into the specified target language.
        /// </summary>
        /// <param name="request">An object containing the input sentence to be translated and the target language for translation.</param>
        /// <returns>A <see cref="TranslationResult"/> object containing the success state of the translation, the translated sentence, or any error messages if the translation failed.</returns>
        Task<TranslationResult?> TranslateAsync(TranslationRequest request);
    }
}
