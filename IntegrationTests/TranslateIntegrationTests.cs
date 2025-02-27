namespace IntegrationTests
{
    using System.Text.Json;
    using Microsoft.Playwright;
    using Microsoft.Playwright.NUnit;
    using Models;
    using NUnit.Framework;

    /// <summary>
    /// Represents integration tests for the translation functionalities of the application.
    /// </summary>
    /// <remarks>
    /// These tests verify the translation features, ensuring the application behaves as expected,
    /// including displaying available languages and translating input text correctly.
    /// </remarks>
    /// <example>
    /// Integration tests include:
    /// - Testing if the homepage displays a list of available languages.
    /// - Verifying if the application translates input text correctly.
    /// </example>
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class TranslateIntegrationTests : PageTest
    {
        /// <summary>
        /// Represents the URL to the application being tested in integration tests.
        /// This variable is initialized with the application's URL from the settings file
        /// during the test setup phase and is used for navigation in various test methods.
        /// </summary>
        private string url;

        /// <summary>
        /// Sets up the test environment by loading configuration settings from a file.
        /// </summary>
        /// <exception cref="FileNotFoundException">
        /// Thrown when the test settings file could not be found in the expected directory.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the application URL is not defined in the settings file.
        /// </exception>
        [SetUp]
        public void Setup()
        {
            // Load settings from the testsettings.json file
            var settingsFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "testsettings.json");

            if (File.Exists(settingsFilePath))
            {
                var json = File.ReadAllText(settingsFilePath);
                var settings = JsonSerializer.Deserialize<TestSettings>(json);

                url = settings?.ApplicationUrl ?? throw new InvalidOperationException("Application URL is not defined in the settings file.");
            }
            else
            {
                throw new FileNotFoundException($"Settings file not found at path: {settingsFilePath}");
            }
        }

        /// <summary>
        /// Verifies that the homepage displays a list of available languages.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The test asserts that the
        /// list of languages is not empty and that specific languages, such as French, are included.
        /// </returns>
        [Test]
        public async Task HomePage_ShouldDisplay_LanguagesList()
        {
            // Navigate to the application URL
            await Page.GotoAsync(this.url);

            // Get all languages listed on the page
            var availableLanguages = await Page.Locator(".language").AllInnerTextsAsync();

            // Assert
            Assert.That(availableLanguages, Is.Not.Empty, "The language list should not be empty.");
            Assert.That(availableLanguages.ToList(), Does.Contain("fr"), "French should be included in the supported languages.");
        }

        /// <summary>
        /// Tests the translation functionality by providing an input text,
        /// selecting a target language, and verifying that the translated text
        /// matches the expected output for the selected language.
        /// </summary>
        /// <returns>
        /// Verifies that the translation result is not empty, differs from the input text,
        /// and matches the expected translated output.
        /// </returns>
        [Test]
        public async Task TranslateInput_ShouldReturn_TranslatedText()
        {
            // Navigate to the application URL
            await Page.GotoAsync(this.url);

            await Page.WaitForSelectorAsync(".input-text", new PageWaitForSelectorOptions
            {
                State = WaitForSelectorState.Visible
            });

            // Focus on the text area first
            await Page.Locator(".input-text").ClickAsync();

            // Then type the text manually
            await Page.Locator(".input-text").FillAsync("Hello, world!");

            // Locate and click the "fr" button in the language sidebar
            var frButton = Page.Locator("div.language:has-text('fr')");
            await frButton.ClickAsync();

            // Click the translate button
            await Page.Locator(".translate-button .button").EvaluateAsync("button => button.click()");

            // Wait for the translated text to appear
            await Page.WaitForFunctionAsync("document.querySelector('.output-text').innerText.trim() !== 'Translated text will appear here.'");

            Task.Delay(5000).Wait(); // Wait for 5 seconds ( 5000 ms = 5 seconds)
            
            // Get the translated text
            var translatedText = await Page.Locator(".output-text").InputValueAsync();

            // Assert
            Assert.That(translatedText, Is.Not.Empty, "Translated text should not be empty.");
            Assert.That(translatedText, Is.Not.EqualTo("Hello, how are you?"), "Translated text should differ from input.");
            Assert.That(translatedText, Is.EqualTo("Salut tout le monde!"), "The translated text should match expected French output.");
        }
    }
}
