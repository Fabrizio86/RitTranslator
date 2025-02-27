namespace ITranslateService
{
    using Common.DTO;

    /// <summary>
    /// Provides functionality to translate text into the specified target language.
    /// </summary>
    public interface ITranslationProvider
    {
        /// <summary>
        /// Translates the given input sentence into the specified target language asynchronously.
        /// </summary>
        /// <param name="request">The translation request containing the input sentence and the target language.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the translation result, including success state, the translated sentence, and any error message.</returns>
        Task<TranslationResult> TranslateAsync(TranslationRequest request);

        /// <summary>
        /// Gets the language code that represents the target language for translation.
        /// This property indicates the ISO 639-1 standard language code used during the translation process.
        /// </summary>
        string LanguageCode { get; }
    }
}
