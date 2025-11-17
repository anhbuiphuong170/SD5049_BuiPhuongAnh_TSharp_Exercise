

# C# Selenium Automation Solution

This repository demonstrates browser automation using Selenium WebDriver, NUnit, and the Page Object Model (POM) in C#. It includes multiple projects for learning and practicing automation techniques.

## Projects Overview

- **DemoQATests**: Automated UI tests for [demoqa.com](https://demoqa.com) using Selenium WebDriver and NUnit.
- **SeleniumCommand**: Modular console automation project with reusable Selenium commands.
- **SeleniumCommand_Ex1/Ex2**: Console automation scripts for login scenarios.
- **SeleniumNUnitPOM**: Recommended structure using NUnit and POM for scalable test automation.

## Setup Instructions
1. Install .NET SDK 7.0 or later
2. Open the project folder in Visual Studio or VS Code
3. Restore NuGet packages for each project:
   ```cmd
   dotnet restore SeleniumCommand/SeleniumCommand.csproj
   dotnet restore SeleniumNUnitPOM/SeleniumNUnitPOM.csproj
   dotnet restore DemoQATests/DemoQATests.csproj
   ```

## How to Run
1. Open a terminal in the project root
2. For console projects:
   ```cmd
   dotnet run --project SeleniumCommand/SeleniumCommand.csproj
   ```
3. For NUnit tests:
   ```cmd
   dotnet test DemoQATests/DemoQATests.csproj
   dotnet test SeleniumNUnitPOM/SeleniumNUnitPOM.csproj
   ```
4. If you encounter build/test issues, clean the output folders first:
   ```cmd
   rd /s /q bin
   rd /s /q obj
   dotnet build
   dotnet test
   or dotnet test --logger:"console;verbosity=detailed"
   ```

## Selenium Packages Used
- Selenium.WebDriver
- Selenium.Support
- Selenium.WebDriver.ChromeDriver
- DotNetSeleniumExtras.WaitHelpers

## DemoQATests: Automated Scenarios

The `DemoQATests` project covers the following scenarios:

### 1. Login Automation
- Automate login to demoqa.com with valid credentials
- Verify successful login and user profile display

### 2. Profile Page Validation
- Navigate to Profile page
- Validate displayed user information (username, book list)

### 3. Book Collection Management
- Add all available books to the user's collection via API
- Search for a specific book in the profile
- Delete a book from the collection and verify removal

### 4. Alert and Popup Handling
- Interact with browser alerts and popups during book deletion
- Validate alert messages and proper handling

### 5. Retry and API Helpers
- Use helper classes for retry logic and API interaction to improve test reliability

## Example Credentials
- Username: buiphuonganh
- Password: @Nh17102025

## Folder Structure
```
DemoQATests/
  Helpers/      # Utility classes for alerts, API, retries
  Pages/        # Page Object Model classes (LoginPage, ProfilePage, etc.)
  Tests/        # NUnit test classes (LoginTests, BooksTests, etc.)
```
