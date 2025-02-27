namespace TranslationService.DTO
{
    /// <summary>
    /// Represents the response received from the LibreTranslate API.
    /// </summary>
    public class LibreTranslateResponse
    {
        /// <summary>
        /// The translated text as returned by the API.
        /// </summary>
        public string translatedText { get; set; }
    }
}
