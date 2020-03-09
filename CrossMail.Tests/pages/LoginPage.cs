using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using System;

using CrossMail.Utils;

namespace CrossMail.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _browserDriver;
        private readonly WebDriverWait _wait;

        [FindsBy (How = How.Id, Using = "identifierId")]
        private IWebElement _usernameInput;
        
        [FindsBy (How = How.Id, Using = "identifierNext")]
        private IWebElement _nextButtonPassword;
        
        [FindsBy (How = How.XPath, Using = "//input[@type='password' and @name='password']")]
        private IWebElement _passwordInput;

        [FindsBy (How = How.Id, Using = "passwordNext")]
        private IWebElement _nextButtonLogin;

        public LoginPage(IWebDriver driver)
        {
            this._browserDriver = driver;
            this._wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }

        public void Login(string username, string password)
        {
            this.TypeUsername(username);
            this.ClickNextForPassword();
            this.TypePassword(password);
            this.ClickNextForLogin();
        }

        private void TypeUsername(string username)
        {
            try
            {
                _wait.Until(ExpectedConditions.ElementToBeClickable(_usernameInput)).SendKeys(username);
            }
            catch (WebDriverTimeoutException timeout)
            {
                ProcessUtils.ConsoleLog(timeout.Message);
            }
        }        

        private void ClickNextForPassword()
        {
            try
            {
                _wait.Until(ExpectedConditions.ElementToBeClickable(_nextButtonPassword)).Click();
            }
            catch (WebDriverTimeoutException timeout)
            {
                ProcessUtils.ConsoleLog(timeout.Message);
            }
        }

        private void TypePassword(string password)
        {
            try
            {
                _wait.Until(ExpectedConditions.ElementToBeClickable(_passwordInput)).SendKeys(password);
            }
            catch (WebDriverTimeoutException timeout)
            {
                ProcessUtils.ConsoleLog(timeout.Message);
            }
        }

        private void ClickNextForLogin()
        {
            try
            {
                _wait.Until(ExpectedConditions.ElementToBeClickable(_nextButtonLogin)).Click();
            }
            catch (WebDriverTimeoutException timeout)
            {
                ProcessUtils.ConsoleLog(timeout.Message);
            }
        }

    }
}