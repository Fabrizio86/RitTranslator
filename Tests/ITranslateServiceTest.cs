namespace Tests
{
    using Common.DTO;
    using ITranslateService;
    using NUnit.Framework;
    using Moq;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// A test class for validating the functionality of the TranslateService implementation.
    /// </summary>
    /// <remarks>
    /// This class includes unit tests for methods of the TranslateService,
    /// which is responsible for translating text to specified target languages
    /// and retrieving a list of supported languages.
    /// </remarks>
    /// <example>
    /// This is part of a test suite utilizing NUnit for performing asynchronous
    /// unit tests to ensure reliability and correctness of TranslateService methods.
    /// </example>
    /// <seealso cref="ITranslateService"/>
    public class TranslateServiceTest
    {
        /// <summary>
        /// A mock instance of the <see cref="ITranslateService"/> interface used for unit testing purposes.
        /// This mock is configured to simulate the behavior of the translation service by allowing method setups
        /// and verifications in test cases.
        /// </summary>
        private Mock<ITranslateService> translateServiceMock;

        /// <summary>
        /// Sets up prerequisites for unit tests in the TranslateServiceTest class.
        /// Initializes a mock instance of the ITranslateService interface to simulate the behavior of the translation service,
        /// which facilitates the testing of service methods without relying on the actual implementation.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.translateServiceMock = new Mock<ITranslateService>();
        }

        /// <summary>
        /// Tests the TranslateAsync method of the translation service to ensure that,
        /// given a valid translation request, it successfully returns the expected translation result.
        /// The test verifies that the translation is accurate, successful, and contains no error messages.
        /// </summary>
        [Test]
        public async Task TranslateAsync_WhenValidRequest_ReturnsExpectedTranslation()
        {
            // Arrange
            var translationRequest = new TranslationRequest
            {
                InputSentence = "Hello", TargetLanguage = "es"
            };

            var expectedResult = new TranslationResult
            {
                TranslatedSentence = "Hola", Success = true, ErrorMessage = string.Empty
            };

            this.translateServiceMock
                .Setup(service => service.TranslateAsync(translationRequest))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await this.translateServiceMock.Object.TranslateAsync(translationRequest);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.TranslatedSentence, Is.EqualTo("Hola"));
            });
        }

        /// <summary>
        /// Tests the TranslateAsync method of the translation service when the input request fails to process.
        /// Verifies that the method returns an appropriate error message, an empty translated sentence, and a success flag set to false.
        /// </summary>
        [Test]
        public async Task TranslateAsync_WhenRequestFails_ReturnsErrorMessage()
        {
            // Arrange
            var translationRequest = new TranslationRequest
            {
                InputSentence = "Hello", TargetLanguage = "invalid-lang"
            };

            var expectedResult = new TranslationResult
            {
                TranslatedSentence = string.Empty, Success = false, ErrorMessage = "Unsupported language"
            };

            this.translateServiceMock
                .Setup(service => service.TranslateAsync(translationRequest))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await this.translateServiceMock.Object.TranslateAsync(translationRequest);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
                {
                    Assert.That(result.Success, Is.False);
                    Assert.That(result.ErrorMessage, Is.EqualTo("Unsupported language"));
                    Assert.That(result.TranslatedSentence, Is.Empty);
                });
        }

        /// <summary>
        /// Tests the TranslateAsync method of ITranslateService to verify that
        /// it returns an error response when the target language in the
        /// translation request is not supported.
        /// Ensures the appropriate error message and failure status are set
        /// in the translation result in this scenario.
        /// </summary>
        /// <returns>A completed task representing the asynchronous test operation.</returns>
        [Test]
        public async Task TranslateAsync_WhenTargetLanguageIsNotSupported_ReturnsError()
        {
            // Arrange
            var translationRequest = new TranslationRequest
            {
                InputSentence = "Hello", TargetLanguage = "xx"
            };

            var expectedResult = new TranslationResult
            {
                TranslatedSentence = string.Empty, Success = false, ErrorMessage = "Language not supported"
            };

            this.translateServiceMock
                .Setup(service => service.TranslateAsync(translationRequest))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await this.translateServiceMock.Object.TranslateAsync(translationRequest);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
                {
                    Assert.That(result.Success, Is.False);
                    Assert.That(result.ErrorMessage, Is.EqualTo("Language not supported"));
                });
        }

        /// <summary>
        /// Verifies that GetSupportedLanguagesAsync returns a non-empty list of supported languages.
        /// Sets up a mock implementation of the translation service to return a predefined list of languages
        /// and checks that the returned list matches the expected values, ensuring the method functions correctly.
        /// </summary>
        [Test]
        public async Task GetSupportedLanguagesAsync_ReturnsNonEmptyLanguageList()
        {
            // Arrange
            var supportedLanguages = new List<string>
            {
                "en", "es", "fr"
            };

            this.translateServiceMock
                .Setup(service => service.GetSupportedLanguagesAsync())
                .ReturnsAsync(supportedLanguages);

            // Act
            var result = await this.translateServiceMock.Object.GetSupportedLanguagesAsync();

            // Assert
            var enumerable = result.ToList();
            Assert.That(enumerable, Is.Not.Null);
            Assert.That(enumerable, Is.Not.Empty);
            CollectionAssert.AreEquivalent(new[]
            {
                "en", "es", "fr"
            }, enumerable);
        }

        /// <summary>
        /// Tests the behavior of the GetSupportedLanguagesAsync method when no supported languages are available.
        /// Asserts that the returned result is an empty list, ensuring the method correctly handles cases
        /// where no languages are provided by the underlying service.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous test operation, ensuring the method returns an empty list
        /// when no supported languages are available.
        /// </returns>
        [Test]
        public async Task GetSupportedLanguagesAsync_WhenNoLanguagesAreAvailable_ReturnsEmptyList()
        {
            // Arrange
            var supportedLanguages = new List<string>();
            this.translateServiceMock
                .Setup(service => service.GetSupportedLanguagesAsync())
                .ReturnsAsync(supportedLanguages);

            // Act
            var result = await this.translateServiceMock.Object.GetSupportedLanguagesAsync();

            // Assert
            var enumerable = result.ToList();
            Assert.That(enumerable, Is.Not.Null);
            Assert.That(enumerable, Is.Empty);
        }
    }
}
