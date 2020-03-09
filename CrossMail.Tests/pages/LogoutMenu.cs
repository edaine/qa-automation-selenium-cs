using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;

using OpenQA.Selenium.Interactions;

namespace CrossMail.Pages
{
    public class LogoutMenu
    {
        private readonly IWebDriver _browserDriver;
        Actions _actionsBuilder;

        [FindsBy (How = How.XPath, Using = "//a[@role='button' and contains(@href,'SignOutOptions')]")]
        private IWebElement _signOutLocator;

        [FindsBy (How = How.XPath, Using = "//a[text()='Sign out' and contains(@href,'Logout')]")]
        private IWebElement _signOutLink;

        public LogoutMenu(IWebDriver driver)
        {
            this._browserDriver = driver;
            this._actionsBuilder = new Actions(driver);
        }

        public void Logout()
        {
            if ( null != _signOutLocator && null != _signOutLink )
            {
                _actionsBuilder.MoveToElement(_signOutLocator)
                               .Click()
                               .Build()
                               .Perform();

                _signOutLink.Click();

            }
        }
    }
}