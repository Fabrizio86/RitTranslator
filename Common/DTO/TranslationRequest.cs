namespace Common.DTO
{
    /// <summary>
    /// Represents a request for translating a given input sentence into a specified target language.
    /// </summary>
    public class TranslationRequest
    {
        /// <summary>
        /// Gets or sets the input sentence to be translated.
        /// </summary>
        /// <remarks>
        /// This property represents the text that will be used as the source for translation.
        /// The input should be provided in the source language prior to initiating the translation process.
        /// </remarks>
        public string InputSentence { get; set; }

        /// <summary>
        /// Gets or sets the target language for the translation.
        /// </summary>
        /// <remarks>
        /// This property specifies the language into which the input sentence will be translated.
        /// The value should be a valid language code (e.g., "fr", "en") recognized by the translation service.
        /// </remarks>
        public string TargetLanguage { get; set; }
    }
}
