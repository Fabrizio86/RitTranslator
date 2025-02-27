namespace TranslationService.Models
{
    /// <summary>
    /// Represents the API interface for handling translation services.
    /// </summary>
    public class TranslatorApi
    {
        /// <summary>
        /// A property to store the API key used for authentication with the translation service.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// A property to store the URL of the API endpoint used for the translation service.
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        /// A property to specify the region associated with the translation service endpoint.
        /// </summary>
        public string Region { get; set; }
    }
}
