
# C# Selenium Automation Project

This project demonstrates browser automation using Selenium WebDriver, NUnit, and the Page Object Model (POM) in C#.

## Project Structure
- **SeleniumCommand_Ex1**: Basic console automation script (no POM, no NUnit)
- **SeleniumCommand_Ex2**: Console automation script for valid login
- **SeleniumCommand**: Console automation project (improved, modular, and maintainable)
- **SeleniumNUnitPOM**: Recommended structure using NUnit and POM
   - `Pages/`: Page Object classes (e.g., LoginPage.cs)
   - `Tests/`: NUnit test classes (e.g., LoginTests.cs)


## Setup
1. Install .NET SDK
2. Open the project folder in Visual Studio Code or Visual Studio
3. Restore NuGet packages for each project:
   ```
   dotnet restore SeleniumCommand/SeleniumCommand.csproj
   dotnet restore SeleniumNUnitPOM/SeleniumNUnitPOM.csproj
   ```


## How to Run
1. Open a terminal in the project root
2. For console projects:
   ```
   dotnet run --project SeleniumCommand/SeleniumCommand.csproj
   ```
3. For NUnit tests:
   ```
   dotnet test SeleniumNUnitPOM/SeleniumNUnitPOM.csproj
   ```

**Note:** If you encounter build/test issues, clean the output folders first:
```
rd /s /q bin
rd /s /q obj
```
Then run:
```
dotnet build
dotnet test
```


## Selenium Packages Used
- Selenium.WebDriver
- Selenium.Support
- Selenium.WebDriver.ChromeDriver
- DotNetSeleniumExtras.WaitHelpers


## Practice Scenarios
// Add your automation scenarios and exercises here

### Exercise 1: Invalid Login
1. Launch browser
2. Navigate to 'http://automationexercise.com'
3. Verify home page is visible
4. Click 'Signup / Login'
5. Verify 'Login to your account' is visible
6. Enter incorrect email and password
7. Click 'login'
8. Verify error message is visible

### Exercise 2: Valid Login
1. Launch browser
2. Navigate to 'http://automationexercise.com'
3. Verify home page is visible
4. Click 'Signup / Login'
5. Verify 'Login to your account' is visible
6. Enter correct email and password
7. Click 'login'
8. Verify 'Logged in as username' is visible
