class TranslationManager {
    constructor(languageTranslation) {
        this.languageTranslation = languageTranslation;

        // Initialize DOM elements
        this.buttonWrapper = $(".button-wrapper");
        this.errorMessage = this.buttonWrapper.find(".error-message");
        this.button = $(".button-wrapper .button");
        this.spinner = $(".button-wrapper .spinner");
        this.textInput = $(".input-text");
        this.outputText = $(".output-text");

        // Event listeners
        this.initEventListeners();
    }

    // Show error messages and reset the UI
    displayError(message) {
        this.button.show();
        this.spinner.hide();
        this.errorMessage.text(message);
    }

    // Display the translation result
    displayTranslationResult(translatedSentence) {
        this.errorMessage.text("");
        this.outputText.val(translatedSentence);
    }

    // Clear messages and show loading spinner
    initTranslation() {
        this.errorMessage.text("");
        this.showSpinner();
        this.languageTranslation.inputSentence = this.textInput.val().trim();
    }

    // Handle empty input validation
    handleEmptyInputSentence() {
        this.displayError("Please enter a text to translate");
    }

    // Handle empty selected language validation
    handleEmptytargetLanguage() {
        this.displayError("Please select the language to use");
    }

    // Validate input text and selected language
    validateTranslation() {
        if (this.languageTranslation.inputSentence.length === 0) {
            this.handleEmptyInputSentence();
            return false;
        }

        if (this.languageTranslation.targetLanguage.length === 0) {
            this.handleEmptytargetLanguage();
            return false;
        }

        return true;
    }

    // Language selection logic
    handleLanguageSelection(event) {
        $(".language").removeClass("selected");
        $(event.currentTarget).addClass("selected");
        this.languageTranslation.targetLanguage = $(event.currentTarget).text();
    }

    // Button click logic
    handleTranslationRequest() {
        this.initTranslation();

        if (!this.validateTranslation()) return;

        this.sendTranslationRequest();
    }

    showSpinner() {
        this.button.hide();
        this.spinner.show();
    }

    showButton() {
        this.button.show();
        this.spinner.hide();
    }

    // Perform the AJAX call to translate text using modern jQuery
    sendTranslationRequest() {
        this.outputText.val("");

        $.ajax({
            url: "/api/translate", // Replace with your backend API endpoint
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify(this.languageTranslation), // Serialize the request object
            dataType: "json", // Expect JSON in response
        })
            .done((response) => {
                // Handle translation result
                if (response.success) {
                    this.displayTranslationResult(response.translatedSentence);
                } else {
                    this.displayError(response.errorMessage || "Translation failed");
                }

                this.showButton();
            })
            .fail((jqXHR) => {
                this.displayError(
                    jqXHR.responseJSON?.error || "Failed to connect to the server."
                );

                this.showButton();
            });
    }

    // Attach event listeners
    initEventListeners() {
        $(".language").on("click", (event) => this.handleLanguageSelection(event));
        this.buttonWrapper.on("click", () => this.handleTranslationRequest());
    }
}