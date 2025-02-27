namespace Tests
{
    using Common.DTO;
    using ITranslateService;
    using NUnit.Framework;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Unit tests for verifying the behavior and functionality of the ITranslationServiceClient interface.
    /// Provides tests for retrieving supported languages and translating text, including handling various scenarios such as input validation, success, and failure cases.
    /// </summary>
    public class ITranslationServiceClientTest
    {
        /// <summary>
        /// A mock object for the ITranslationServiceClient interface.
        /// Used for unit testing to simulate the behavior of the translation service client,
        /// enabling testing of scenarios such as retrieving supported languages and text translation.
        /// </summary>
        private Mock<ITranslationServiceClient> translationServiceClientMock;

        /// <summary>
        /// Sets up the mock dependencies and any required preconditions for the unit tests
        /// in the ITranslationServiceClientTest class.
        /// This method ensures that a fresh Mock<ITranslationServiceClient> instance is created
        /// before each test to isolate test cases and avoid shared state between tests.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.translationServiceClientMock = new Mock<ITranslationServiceClient>();
        }

        /// <summary>
        /// Verifies that the GetSupportedLanguagesAsync method correctly returns the list of supported languages
        /// provided by the translation service.
        /// </summary>
        /// <returns>A task that represents the asynchronous test execution.</returns>
        [Test]
        public async Task GetSupportedLanguagesAsync_WhenCalled_ReturnsSupportedLanguages()
        {
            // Arrange
            var expectedLanguages = new List<string> { "en", "es", "fr" };
            
            this.translationServiceClientMock
                .Setup(client => client.GetSupportedLanguagesAsync())
                .ReturnsAsync(expectedLanguages);

            // Act
            var supportedLanguages = await this.translationServiceClientMock.Object.GetSupportedLanguagesAsync();

            // Assert
            Assert.That(supportedLanguages, Is.Not.Null);
            Assert.That(supportedLanguages, Is.Not.Empty);
            CollectionAssert.AreEquivalent(new[] { "en", "es", "fr" }, supportedLanguages);
        }

        /// <summary>
        /// Ensures that the GetSupportedLanguagesAsync method returns an empty list
        /// when no languages are available from the translation service.
        /// </summary>
        /// <returns>A task that represents the asynchronous test execution.</returns>
        [Test]
        public async Task GetSupportedLanguagesAsync_WhenNoLanguagesAreAvailable_ReturnsEmptyList()
        {
            // Arrange
            var expectedLanguages = new List<string>();
            
            this.translationServiceClientMock
                .Setup(client => client.GetSupportedLanguagesAsync())
                .ReturnsAsync(expectedLanguages);

            // Act
            var supportedLanguages = await this.translationServiceClientMock.Object.GetSupportedLanguagesAsync();

            // Assert
            Assert.That(supportedLanguages, Is.Not.Null);
            Assert.That(supportedLanguages, Is.Empty);
        }

        /// <summary>
        /// Ensures that the TranslateAsync method returns the expected translation result
        /// when provided with a valid translation request.
        /// </summary>
        /// <returns>A task that represents the asynchronous test execution.</returns>
        [Test]
        public async Task TranslateAsync_WhenValidRequest_ReturnsExpectedTranslation()
        {
            // Arrange
            var request = new TranslationRequest
            {
                InputSentence = "Hello",
                TargetLanguage = "fr"
            };

            var expectedResult = new TranslationResult
            {
                TranslatedSentence = "Bonjour",
                Success = true,
                ErrorMessage = null
            };

            this.translationServiceClientMock
                .Setup(client => client.TranslateAsync(request))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await this.translationServiceClientMock.Object.TranslateAsync(request);

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
        /// Verifies that the TranslateAsync method returns an error message
        /// when the request fails due to unsupported target language.
        /// </summary>
        /// <returns>A task that represents the asynchronous test execution.</returns>
        [Test]
        public async Task TranslateAsync_WhenRequestFails_ReturnsErrorMessage()
        {
            // Arrange
            var request = new TranslationRequest
            {
                InputSentence = "Hello",
                TargetLanguage = "unsupported"
            };

            var expectedResult = new TranslationResult
            {
                TranslatedSentence = string.Empty,
                Success = false,
                ErrorMessage = "Target language not supported"
            };

            this.translationServiceClientMock
                .Setup(client => client.TranslateAsync(request))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await this.translationServiceClientMock.Object.TranslateAsync(request);

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
        /// Verifies that the TranslateAsync method throws an ArgumentNullException
        /// when a null request is provided as input.
        /// </summary>
        /// <returns>A task that represents the asynchronous test execution.</returns>
        [Test]
        public async Task TranslateAsync_WhenRequestIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            this.translationServiceClientMock
                .Setup(client => client.TranslateAsync(null))
                .ThrowsAsync(new ArgumentNullException());

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await this.translationServiceClientMock.Object.TranslateAsync(null));
        }
    }
}