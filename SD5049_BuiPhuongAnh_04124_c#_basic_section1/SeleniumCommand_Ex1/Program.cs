using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumAutomationExercise
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Launch browser
            IWebDriver driver = new ChromeDriver();

            try
            {
                // Maximize window for better visibility
                driver.Manage().Window.Maximize();

                // 2. Navigate to url 'http://automationexercise.com'
                driver.Navigate().GoToUrl("http://automationexercise.com");

                // 3. Verify that home page is visible successfully
                string homeTitle = driver.Title;
                if (homeTitle.Contains("Automation Exercise"))
                {
                    Console.WriteLine("Home page is visible successfully.");
                }
                else
                {
                    Console.WriteLine("Home page NOT visible.");
                }

                // 4. Click on 'Signup / Login' button
                driver.FindElement(By.XPath("//a[contains(text(),'Signup / Login')]")).Click(); 

                // 5. Verify 'Login to your account' is visible
                try
                {
                    IWebElement loginHeader = driver.FindElement(By.XPath("//h2[contains(text(),'Login to your account')]"));
                    if (loginHeader.Displayed)
                    {
                        Console.WriteLine("'" + loginHeader.Text + "'" + " is visible.");
                    }                    
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine("Login header not found on the page.");
                    driver.Quit();
                    return;
                }

                // 6. Enter incorrect email address and password
                driver.FindElement(By.XPath("//input[@data-qa='login-email']")).SendKeys("wrongemail@test.com");
                driver.FindElement(By.XPath("//input[@data-qa='login-password']")).SendKeys("wrongpassword");

                // 7. Click 'login' button
                driver.FindElement(By.XPath("//button[@data-qa='login-button']")).Click();

                // 8. Verify error 'Your email or password is incorrect!' is visible
                try
                {
                    IWebElement errorMsg = driver.FindElement(By.XPath("//p[contains(text(),'Your email or password is incorrect!')]"));
                    if (errorMsg.Displayed)
                    {
                        Console.WriteLine("Error message is visible: " + errorMsg.Text);
                    }
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine("Error message element not found on the page.");
                    driver.Quit();
                    return;
                }               
            }
            catch (Exception ex)
            {
                Console.WriteLine("Test failed with exception: " + ex.Message);
            }
            finally
            {
                if (driver != null)
                {
                    // Close browser
                    driver.Quit();
                    Console.WriteLine("Browser closed. Test finished.");
                }
            }
        }
    }
}
