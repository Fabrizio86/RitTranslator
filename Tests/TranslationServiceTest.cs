namespace Tests
{
    using Common.DTO;
    using ITranslateService;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TranslationService;

    /// <summary>
    /// Represents a test class for the TranslationService, responsible for verifying the behavior
    /// of translation-related functionalities and ensuring the service provides correct results
    /// or handles errors as expected.
    /// </summary>
    public class TranslationServiceTest
    {
        /// <summary>
        /// An instance of the <see cref="TranslationService"/> class, used for performing translation operations during testing.
        /// </summary>
        private TranslationService translationService;

        /// <summary>
        /// A mocked instance of the <see cref="ITranslationProviderFactory"/> interface, used for testing purposes.
        /// </summary>
        private Mock<ITranslationProviderFactory> translationProviderFactoryMock;

        /// <summary>
        /// Represents a mock instance of the <see cref="ITranslationProvider"/> interface
        /// used in unit tests to simulate the behavior of a translation provider without
        /// relying on actual implementation or external dependencies.
        /// </summary>
        private Mock<ITranslationProvider> translationProviderMock;

        /// <summary>
        /// Initializes the necessary mocks and dependencies required for testing
        /// the TranslationService functionalities. This method sets up the
        /// TranslationService instance with a mock implementation of the
        /// ITranslationProviderFactory and ITranslationProvider interfaces to isolate
        /// the service's behavior during the test execution.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.translationProviderFactoryMock = new Mock<ITranslationProviderFactory>();
            this.translationProviderMock = new Mock<ITranslationProvider>();
            this.translationService = new TranslationService(this.translationProviderFactoryMock.Object);
        }

        /// <summary>
        /// Tests the functionality of the TranslateAsync method in the TranslationService when it is invoked with a valid translation request.
        /// Confirms that the method properly delegates to the appropriate translation provider and that the returned result matches
        /// the expected translation, including correctness of the translated sentence, success status, and any accompanying error details.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task succeeds if the translation result aligns with the expected output
        /// and all assertions pass without exceptions.
        /// </returns>
        [Test]
        public async Task TranslateAsync_WhenValidRequest_ReturnsExpectedTranslation()
        {
            // Arrange
            var request = new TranslationRequest
            {
                InputSentence = "Hello", TargetLanguage = "fr"
            };

            var expectedResult = new TranslationResult
            {
                TranslatedSentence = "Bonjour", Success = true, ErrorMessage = null
            };

            this.translationProviderFactoryMock
                .Setup(factory => factory.GetProvider(request))
                .Returns(this.translationProviderMock.Object);

            this.translationProviderMock
                .Setup(provider => provider.TranslateAsync(request))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await this.translationService.TranslateAsync(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.TranslatedSentence, Is.EqualTo("Bonjour"));
                Assert.That(result.ErrorMessage, Is.Null);
            });
        }

        /// <summary>
        /// Tests the behavior of the TranslationService when a translation request fails.
        /// This method verifies the service's ability to handle errors gracefully by
        /// ensuring an appropriate error message is returned while the translated sentence
        /// is empty and the success flag is set to false.
        /// </summary>
        /// <returns>
        /// A TranslationResult object with the Success property set to false, the
        /// TranslatedSentence property as an empty string, and an error message indicating
        /// the reason for the failure.
        /// </returns>
        [Test]
        public async Task TranslateAsync_WhenRequestFails_ReturnsErrorMessage()
        {
            // Arrange
            var request = new TranslationRequest
            {
                InputSentence = "Hello", TargetLanguage = "unsupported"
            };

            var expectedResult = new TranslationResult
            {
                TranslatedSentence = string.Empty, Success = false, ErrorMessage = "Target language not supported"
            };

            this.translationProviderFactoryMock
                .Setup(factory => factory.GetProvider(request))
                .Returns(this.translationProviderMock.Object);

            this.translationProviderMock
                .Setup(provider => provider.TranslateAsync(request))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await this.translationService.TranslateAsync(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.ErrorMessage, Is.EqualTo("Target language not supported"));
                Assert.That(result.TranslatedSentence, Is.Empty);
            });
        }

        /// <summary>
        /// Validates that the TranslateAsync method in the TranslationService
        /// throws an ArgumentNullException when a null translation request is provided.
        /// This test ensures proper handling of invalid input by verifying exception-raising behavior
        /// to enforce input validation.
        /// </summary>
        [Test]
        public void TranslateAsync_WhenRequestIsNull_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsAsync<NullReferenceException>(async () =>
                await this.translationService.TranslateAsync(null));
        }

        /// <summary>
        /// Verifies that the GetSupportedLanguagesAsync method retrieves the expected list
        /// of supported languages from the translation service when called. Ensures the service
        /// correctly interacts with the mocked ITranslationProviderFactory to return the
        /// predefined collection of supported languages.
        /// </summary>
        /// <returns>A task representing the asynchronous operation that validates the returned
        /// collection matches the predefined supported languages set up in the mock.
        /// Ensures the collection is not null, not empty, and contains the expected languages.
        /// </returns>
        [Test]
        public async Task GetSupportedLanguagesAsync_WhenCalled_ReturnsSupportedLanguages()
        {
            // Arrange
            var expectedLanguages = new List<string> { "en", "es", "fr" };

            this.translationProviderFactoryMock
                .SetupGet(factory => factory.SupportedLanguages)
                .Returns(expectedLanguages);

            // Act
            var supportedLanguages = await this.translationService.GetSupportedLanguagesAsync();

            // Assert
            var enumerable = supportedLanguages.ToList();
            Assert.That(enumerable, Is.Not.Null);
            Assert.That(enumerable, Is.Not.Empty);
            CollectionAssert.AreEquivalent(expectedLanguages, enumerable);
        }

        /// <summary>
        /// Tests the behavior of the GetSupportedLanguagesAsync method when there are
        /// no supported languages returned by the translation provider. Verifies that
        /// the method returns an empty collection in such scenarios, ensuring proper
        /// handling of edge cases with no data.
        /// </summary>
        /// <returns>
        /// An empty collection of supported languages, confirming that no languages
        /// are provided by the translation provider.
        /// </returns>
        [Test]
        public async Task GetSupportedLanguagesAsync_WhenNoSupportedLanguages_ReturnsEmptyCollection()
        {
            // Arrange
            var expectedLanguages = new List<string>();

            this.translationProviderFactoryMock
                .SetupGet(factory => factory.SupportedLanguages)
                .Returns(expectedLanguages);

            // Act
            var supportedLanguages = await this.translationService.GetSupportedLanguagesAsync();

            // Assert
            var enumerable = supportedLanguages.ToList();
            Assert.That(enumerable, Is.Not.Null);
            Assert.That(enumerable, Is.Empty);
        }
    }
}