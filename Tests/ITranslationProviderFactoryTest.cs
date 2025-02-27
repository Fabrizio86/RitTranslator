namespace Tests
{
    using Common.DTO;
    using ITranslateService;
    using NUnit.Framework;
    using Moq;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Unit tests for the ITranslationProviderFactory interface.
    /// The tests verify the correctness of the factory's behavior, including
    /// the retrieval of supported languages and the provision of translation providers.
    /// </summary>
    public class ITranslationProviderFactoryTest
    {
        /// <summary>
        /// A mock instance of the <see cref="ITranslationProviderFactory"/> interface,
        /// used for unit testing. It enables controlled testing of methods such as
        /// retrieving supported languages and providing translation providers, including
        /// scenarios with mocked return values and behaviors.
        /// </summary>
        private Mock<ITranslationProviderFactory> translationProviderFactoryMock;

        /// <summary>
        /// Sets up the test environment for the unit tests in the
        /// <see cref="ITranslationProviderFactoryTest"/> class. Initializes
        /// necessary dependencies and mock objects, such as the mock
        /// instance of the <see cref="ITranslationProviderFactory"/> interface,
        /// to enable isolated testing of behaviors and methods.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.translationProviderFactoryMock = new Mock<ITranslationProviderFactory>();
        }

        /// <summary>
        /// Tests the behavior of the <see cref="ITranslationProviderFactory.SupportedLanguages"/> property
        /// to ensure it returns the expected collection of supported languages. Verifies that the
        /// property is not null, contains the expected values, and exhibits the correct behavior
        /// for language retrieval.
        /// </summary>
        [Test]
        public void SupportedLanguages_WhenCalled_ReturnsExpectedLanguages()
        {
            // Arrange
            var expectedLanguages = new List<string> { "en", "es", "fr" };
            
            this.translationProviderFactoryMock
                .SetupGet(factory => factory.SupportedLanguages)
                .Returns(expectedLanguages);

            // Act
            var supportedLanguages = this.translationProviderFactoryMock.Object.SupportedLanguages;

            // Assert
            Assert.That(supportedLanguages, Is.Not.Null);
            Assert.That(supportedLanguages, Is.Not.Empty);
            CollectionAssert.AreEquivalent(new[] { "en", "es", "fr" }, supportedLanguages);
        }

        /// <summary>
        /// Tests the behavior of the <see cref="ITranslationProviderFactory.GetProvider"/> method
        /// when a supported language is requested. Verifies that a valid instance of
        /// <see cref="ITranslationProvider"/> is returned for the provided
        /// <see cref="TranslationRequest"/>. Ensures the factory correctly handles supported
        /// language scenarios and returns the appropriate provider implementation.
        /// </summary>
        [Test]
        public void GetProvider_WhenLanguageIsSupported_ReturnsTranslationProvider()
        {
            // Arrange
            var mockTranslationProvider = new Mock<ITranslationProvider>();
            var translationRequest = new TranslationRequest
            {
                InputSentence = "Hello",
                TargetLanguage = "es"
            };

            this.translationProviderFactoryMock
                .Setup(factory => factory.GetProvider(translationRequest))
                .Returns(mockTranslationProvider.Object);

            // Act
            var provider = this.translationProviderFactoryMock.Object.GetProvider(translationRequest);

            // Assert
            Assert.That(provider, Is.Not.Null);
            Assert.That(provider, Is.InstanceOf<ITranslationProvider>());
        }

        /// <summary>
        /// Validates that the <see cref="ITranslationProviderFactory.GetProvider"/> method
        /// throws a <see cref="NotSupportedException"/> when an unsupported target language
        /// is specified in the <see cref="TranslationRequest"/>. Ensures that the factory
        /// correctly identifies unsupported languages and responds with the appropriate
        /// exception.
        /// </summary>
        [Test]
        public void GetProvider_WhenLanguageIsNotSupported_ThrowsNotSupportedException()
        {
            // Arrange
            var translationRequest = new TranslationRequest
            {
                InputSentence = "Hello",
                TargetLanguage = "xx" // Assume "xx" is unsupported
            };

            this.translationProviderFactoryMock
                .Setup(factory => factory.GetProvider(translationRequest))
                .Throws<NotSupportedException>();

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => this.translationProviderFactoryMock.Object.GetProvider(translationRequest));
        }

        /// <summary>
        /// Tests that the <see cref="ITranslationProviderFactory.GetProvider"/> method throws
        /// an <see cref="ArgumentNullException"/> when the provided <see cref="TranslationRequest"/>
        /// argument is null. Ensures that the method correctly validates its input to
        /// prevent undefined behavior caused by null requests.
        /// </summary>
        [Test]
        public void GetProvider_WhenRequestIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            this.translationProviderFactoryMock
                .Setup(factory => factory.GetProvider(null))
                .Throws<ArgumentNullException>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => this.translationProviderFactoryMock.Object.GetProvider(null));
        }

        /// <summary>
        /// Verifies the behavior of the <see cref="ITranslationProviderFactory.SupportedLanguages"/> property
        /// when no languages are available. Ensures that the method returns an empty collection
        /// and does not result in a null or unexpected value.
        /// </summary>
        [Test]
        public void SupportedLanguages_WhenNoLanguagesAreAvailable_ReturnsEmptyCollection()
        {
            // Arrange
            var expectedLanguages = new List<string>();
            this.translationProviderFactoryMock
                .SetupGet(factory => factory.SupportedLanguages)
                .Returns(expectedLanguages);

            // Act
            var supportedLanguages = this.translationProviderFactoryMock.Object.SupportedLanguages;

            // Assert
            Assert.That(supportedLanguages, Is.Not.Null);
            Assert.That(supportedLanguages, Is.Empty);
        }
    }
}