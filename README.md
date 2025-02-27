# Introduction

This project is to build a web application that translates a user-inputted English sentence into French and displays the translated sentence in the UI. The application is designed with extendability in mind,
allowing future support for additional language translations.

# Getting Started

Follow these steps to set up and run the application on your local system:

1. **Installation Process**:
    - Clone the repository using the command:
      ```bash
      git clone https://github.com/Fabrizio86/RitTranslator
      ```
    - Navigate to the project directory.

2. **Software Dependencies**:
    - Install the .NET 8.0 SDK from [Microsoft .NET Download](https://dotnet.microsoft.com/download).
    - A modern IDE such as Visual Studio or JetBrains Rider is recommended for development.

3. **Latest Releases**:
    - Visit the [Releases Page](https://github.com/Fabrizio86/RitTranslator) to check for the latest updates or releases.

4. **API References**:
    - The application integrates with a third-party translation API for its functionality. Please refer to the API's official documentation for detailed usage information.

# Build and Test

1. To build the project, execute the following command:
   ```bash
   dotnet build
   ```
2. To run the tests, use:
   ```bash
   dotnet test
   ```
   Ensure that all tests pass successfully before running the application.

**Note**: Please note that running the application locally will fail to start unless the local system environment variables are configured with the appropriate access token and URL settings for the translation API. Make sure to configure these variables properly before attempting to execute the
application.