namespace TranslationService.Implementations
{
    using Common.DTO;
    using ITranslateService;
    using Models;

    /// <summary>
    /// A factory class responsible for providing the appropriate translation provider
    /// based on the target language specified in the translation request.
    /// </summary>
    public class TranslationProviderFactory : ITranslationProviderFactory
    {
        /// <summary>
        /// A dictionary containing mappings between language codes and their respective translation providers.
        /// </summary>
        /// <remarks>
        /// The dictionary is case-insensitive and supports adding or retrieving providers by their keys,
        /// which represent language codes (e.g., "en" for English, "fr" for French).
        /// This variable is used to initialize translation providers supported by the factory.
        /// </remarks>
        private readonly Dictionary<string, ITranslationProvider> providers;

        /// <summary>
        /// A factory class responsible for selecting and providing the correct translation provider
        /// based on the target language defined in a translation request.
        /// </summary>
        public TranslationProviderFactory(TranslatorApi api)
        {
            // Dictionary to hold all discovered ITranslationProvider implementations.
            this.providers = new Dictionary<string, ITranslationProvider>(StringComparer.InvariantCultureIgnoreCase);

            // Get all types in the relevant assembly that implement ITranslationProvider.
            var providerTypes = AppDomain.CurrentDomain.GetAssemblies() // Load all assemblies
                .SelectMany(a => a.GetTypes()) // Get all types
                .Where(t => typeof(ITranslationProvider).IsAssignableFrom(t) && // Check if it implements the interface
                            !t.IsInterface && !t.IsAbstract);

            // Loop through each type and create an instance of it.
            foreach (var type in providerTypes)
            {
                ITranslationProvider? provider = null;

                try
                {
                    // Use Activator.CreateInstance and ensure it matches the constructor signature.
                    provider = (ITranslationProvider)Activator.CreateInstance(type, api)!;
                }
                catch (MissingMethodException)
                {
                    // Log or handle the error if a constructor signature doesn't match.
                    Console.WriteLine($"Skipping {type.Name}. Constructor does not match the required parameters.");
                }

                // Check if the provider was created successfully and get its LanguageCode.
                var languageCode = provider?.LanguageCode;
                if (!string.IsNullOrWhiteSpace(languageCode) && provider != null)
                {
                    this.providers.Add(languageCode, provider);
                }
            }
        }

        /// <summary>
        /// A collection of language codes supported by the translation providers.
        /// </summary>
        /// <remarks>
        /// This property retrieves a list of all configured language codes for which
        /// translation providers are available. Each entry corresponds to a language
        /// code (e.g., "en" for English, "fr" for French).
        /// </remarks>
        public IEnumerable<string> SupportedLanguages => this.providers.Keys;

        /// <summary>
        /// Retrieves the appropriate translation provider based on the language specified in the translation request.
        /// </summary>
        /// <param name="request">
        /// The translation request containing the target language for which the provider is to be retrieved.
        /// </param>
        /// <returns>
        /// The translation provider that supports the requested target language.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Thrown when the requested target language is not supported by any available translation provider.
        /// </exception>
        public ITranslationProvider GetProvider(TranslationRequest request)
        {
            var provider = this.providers.FirstOrDefault(p => p.Key == request.TargetLanguage).Value;
            if (provider == null) throw new NotSupportedException($"Translation to language '{request.TargetLanguage}' is not supported.");
            return provider;
        }
    }
}
