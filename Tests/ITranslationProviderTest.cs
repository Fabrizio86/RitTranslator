namespace Tests
{
    using Common.DTO;
    using ITranslateService;
    using NUnit.Framework;
    using Moq;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a test suite for verifying the functionality of the ITranslationProvider interface.
    /// This test class contains various test methods to validate translation operations
    /// and ensure proper behavior for edge cases and error scenarios.
    /// </summary>
    public class ITranslationProviderTest
    {
        /// <summary>
        /// Mock object of the <see cref="ITranslationProvider"/> interface used for testing translation functionality.
        /// This mock instance allows the simulation of translation operations and configuration of expected behavior
        /// for tests within the <c>ITranslationProviderTest</c>.
        /// </summary>
        private Mock<ITranslationProvider> translationProviderMock;
        /// <summary>
        /// 
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.translationProviderMock = new Mock<ITranslationProvider>();
        }

        /// <summary>
        /// Validates that the TranslateAsync method of the ITranslationProvider interface,
        /// when provided with a valid translation request, successfully returns the expected
        /// translated result matching the input parameters.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation, where the result is the expected
        /// successful translation output.
        /// </returns>
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

            this.translationProviderMock
                .Setup(provider => provider.TranslateAsync(request))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await this.translationProviderMock.Object.TranslateAsync(request);

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
        /// Ensures that the TranslateAsync method of the ITranslationProvider interface,
        /// when invoked with a request that fails due to an unsupported language or invalid input,
        /// returns an appropriate error message indicating the reason for the failure.
        /// </summary>
        [Test]
        public async Task TranslateAsync_WhenRequestFails_ReturnsErrorMessage()
        {
            // Arrange
            var request = new TranslationRequest
            {
                InputSentence = "Hello",
                TargetLanguage = "invalid-lang"
            };

            var expectedResult = new TranslationResult
            {
                TranslatedSentence = string.Empty,
                Success = false,
                ErrorMessage = "Unsupported language"
            };

            this.translationProviderMock
                .Setup(provider => provider.TranslateAsync(request))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await this.translationProviderMock.Object.TranslateAsync(request);

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
        /// Ensures that the TranslateAsync method of the ITranslationProvider interface,
        /// when invoked with an input sentence that is empty,
        /// returns an appropriate error message indicating that input cannot be empty.
        /// </summary>
        [Test]
        public async Task TranslateAsync_WhenInputSentenceIsEmpty_ReturnsErrorMessage()
        {
            // Arrange
            var request = new TranslationRequest
            {
                InputSentence = string.Empty,
                TargetLanguage = "es"
            };

            var expectedResult = new TranslationResult
            {
                TranslatedSentence = string.Empty,
                Success = false,
                ErrorMessage = "Input sentence cannot be empty"
            };

            this.translationProviderMock
                .Setup(provider => provider.TranslateAsync(request))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await this.translationProviderMock.Object.TranslateAsync(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.ErrorMessage, Is.EqualTo("Input sentence cannot be empty"));
                Assert.That(result.TranslatedSentence, Is.Empty);
            });
        }

        /// <summary>
        /// Validates that the LanguageCode property of the ITranslationProvider interface, when accessed,
        /// returns the expected language code as configured in the mock translation provider instance.
        /// </summary>
        [Test]
        public void LanguageCode_WhenCalled_ReturnsExpectedLanguageCode()
        {
            // Arrange
            var expectedLanguageCode = "fr";
            this.translationProviderMock
                .SetupGet(provider => provider.LanguageCode)
                .Returns(expectedLanguageCode);

            // Act
            var languageCode = this.translationProviderMock.Object.LanguageCode;

            // Assert
            Assert.That(languageCode, Is.EqualTo(expectedLanguageCode));
        }

        /// <summary>
        /// Tests the TranslateAsync method to verify that it returns an error message when the target language
        /// specified in the request is unsupported.
        /// </summary>
        /// <returns>
        /// A test result that ensures the TranslateAsync method returns a TranslationResult object with its
        /// Success property set to false, the TranslatedSentence as an empty string, and an appropriate error
        /// message indicating that the target language is not supported.
        /// </returns>
        [Test]
        public async Task TranslateAsync_WhenTargetLanguageIsUnsupported_ReturnsError()
        {
            // Arrange
            var request = new TranslationRequest
            {
                InputSentence = "Hello",
                TargetLanguage = "xx" // Assume "xx" is an unsupported language code
            };

            var expectedResult = new TranslationResult
            {
                TranslatedSentence = string.Empty,
                Success = false,
                ErrorMessage = "Target language not supported"
            };

            this.translationProviderMock
                .Setup(provider => provider.TranslateAsync(request))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await this.translationProviderMock.Object.TranslateAsync(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.TranslatedSentence, Is.Empty);
                Assert.That(result.ErrorMessage, Is.EqualTo("Target language not supported"));
            });
        }
    }
}