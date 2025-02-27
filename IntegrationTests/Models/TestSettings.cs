namespace IntegrationTests.Models
{
    /// <summary>
    /// Represents the configuration settings for integration tests.
    /// </summary>
    public class TestSettings
    {
        /// <summary>
        /// Gets or sets the URL of the application under test.
        /// </summary>
        /// <remarks>
        /// This property is used to specify the base URL for the application being tested
        /// during integration tests. The value can be loaded from a configuration file,
        /// such as "testsettings.json".
        /// </remarks>
        public string? ApplicationUrl { get; set; }
    }
}
