namespace Common.DTO
{
    /// <summary>
    /// Represents the result of a translation request, containing the success state,
    /// the translated sentence, and any error messages that occurred during the process.
    /// </summary>
    public class TranslationResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the operation completed successfully.
        /// </summary>
        /// <remarks>
        /// This property reflects the success status of the operation. A value of <c>true</c>
        /// indicates that the process executed without errors, while a value of <c>false</c>
        /// signifies a failure, in which case additional details may be available in the <c>ErrorMessage</c> property.
        /// </remarks>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the translated sentence resulting from a translation process.
        /// </summary>
        /// <remarks>
        /// This property contains the output of a successful translation operation.
        /// If the translation fails, this property may remain empty or null, and
        /// detailed error information would be available in the <c>ErrorMessage</c> property.
        /// </remarks>
        public string TranslatedSentence { get; set; }

        /// <summary>
        /// Gets or sets the error message associated with the operation.
        /// </summary>
        /// <remarks>
        /// This property contains detailed information about the error that occurred during the process.
        /// It is expected to have a value when the operation fails.
        /// </remarks>
        public string ErrorMessage { get; set; }
    }
}
