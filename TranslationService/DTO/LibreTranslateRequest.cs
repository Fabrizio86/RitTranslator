namespace TranslationService.DTO
{
    /// <summary>
    /// Represents a request for translation using the LibreTranslate API.
    /// </summary>
    public class LibreTranslateRequest
    {
        /// <summary>
        /// The text that needs to be translated.
        /// </summary>
        /// <remarks>
        /// This is the source sentence to translate.
        /// </remarks>
        public string q { get; set; }

        /// <summary>
        /// Source language code (e.g., "en" for English, "fr" for French).
        /// If set to "auto", the API will attempt to detect the source language.
        /// </summary>
        public string source { get; set; } = "auto"; // Default is to auto-detect

        /// <summary>
        /// Target language code (e.g., "es" for Spanish).
        /// </summary>
        public string target { get; set; }

        /// <summary>
        /// API field to indicate if a formal tone should be applied (optional, defaults to null).
        /// </summary>
        public string? format { get; set; } = "text"; // LibreTranslate supports "text" or "html".
    }
}

