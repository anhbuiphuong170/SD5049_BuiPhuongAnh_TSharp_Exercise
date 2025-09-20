using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumAutomationExercise
{
    class Program
    {
        static void Main(string[] args)
        {
            IWebDriver driver = new ChromeDriver();
            try
            {
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl("http://automationexercise.com");

                string homeTitle = driver.Title;
                if (homeTitle.Contains("Automation Exercise"))
                {
                    Console.WriteLine("Home page is visible successfully.");
                }
                else
                {
                    Console.WriteLine("Home page NOT visible.");
                }

                driver.FindElement(By.XPath("//a[contains(text(),'Signup / Login')]"))?.Click();

                try
                {
                    IWebElement loginHeader = driver.FindElement(By.XPath("//h2[contains(text(),'Login to your account')]"));
                    if (loginHeader.Displayed)
                    {
                        Console.WriteLine("'" + loginHeader.Text + "'" + " is visible.");
                    }
                    else
                    {
                        Console.WriteLine("'" + loginHeader.Text + "'" + " is not visible.");
                        return;
                    }
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine("Login header not found on the page.");
                    return;
                }

                // Enter correct email and password
                string correctEmail = "anh.bp@test.com"; // Replace with a valid email
                string correctPassword = "anh.bp@test.com";    // Replace with the correct password

                driver.FindElement(By.XPath("//input[@data-qa='login-email']")).SendKeys(correctEmail);
                driver.FindElement(By.XPath("//input[@data-qa='login-password']")).SendKeys(correctPassword);
                driver.FindElement(By.XPath("//button[@data-qa='login-button']")).Click();

                // Verify 'Logged in as username' is visible
                try
                {
                    IWebElement loggedInMsg = driver.FindElement(By.XPath("//a[contains(text(),'Logged in as')]"));
                    if (loggedInMsg.Displayed)
                    {
                        Console.WriteLine("Logged in as message is visible: " + loggedInMsg.Text);
                    }
                    else
                    {
                        Console.WriteLine("Logged in as message is not visible.");
                    }
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine("'Logged in as username' element not found on the page.");
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
                    driver.Quit();
                    Console.WriteLine("Browser closed. Test finished.");
                }
            }
        }
    }
}
