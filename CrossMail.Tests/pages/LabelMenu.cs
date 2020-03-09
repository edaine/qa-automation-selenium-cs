using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using SeleniumExtras.WaitHelpers;
using System;
using Xunit.Abstractions;
using SeleniumExtras.PageObjects;

namespace CrossMail.Pages
{
    public class LabelMenu
    {
        private ITestOutputHelper _output;
        private IWebDriver _browserDriver;
        private WebDriverWait _wait;
        Actions _actionsBuilder;

        [FindsBy (How = How.XPath, Using = "//*[@role='menuitem']//*[text()='Label']")]
        private IWebElement _labelAs;
        
        [FindsBy (How = How.XPath, Using = "//input[@type='text' and @aria-label='Label-as menu open']")]
        private IWebElement _labelAsInput;

        [FindsBy (How = How.XPath, Using = "//*[@role='menuitemcheckbox' and @title='Social']")]
        private IWebElement _socialCategoryCheckbox;

        [FindsBy (How = How.XPath, Using = "//*[@role='menuitemcheckbox' and @title='Social']//div//div")]
        private IWebElement _socialCategoryText;

        [FindsBy (How = How.XPath, Using = "//*[@role='menuitem']//*[text()='Apply']//ancestor::div[1]")]
        private IWebElement _applyLabelMenuItem;

        [FindsBy (How = How.XPath, Using = "//*[@role='menuitem']//*[text()='Apply']")]
        private IWebElement _applyLabelText;

        public LabelMenu(IWebDriver driver)
        {
            this._browserDriver = driver;
            this._wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            this._actionsBuilder = new Actions(driver);
        }

        public void LabelWithCategory(string strCategory)
        {
            _actionsBuilder.MoveToElement(_labelAs)
                           .Perform();

            try
            {
                if ( null != _labelAsInput )
                {
                    switch (strCategory)
                    {
                        case "Social" :
                            if ( null != _socialCategoryText )
                            {    _actionsBuilder.MoveToElement(_socialCategoryText)
                                                .Click()
                                                .Build()
                                                .Perform();
                            }
                            break;

                        default:
                            break;
                    }

                    if ( null != _wait.Until(ExpectedConditions.ElementToBeClickable(_applyLabelMenuItem)) )
                    {
                        _actionsBuilder.MoveToElement(_applyLabelText)
                                       .Perform();
                        _actionsBuilder.Click()
                                       .Perform();
                    }

                }
            }
            catch (WebDriverTimeoutException timeout)
            {
                _output.WriteLine(timeout.Message);
            }

        }

        public bool IsLabelCategoryChecked(string strCategory)
        {
            switch (strCategory)
            {
                case "Social":
                    return (_socialCategoryCheckbox.GetAttribute("aria-checked").Equals("true"));

                default:
                    return false;

            }
        }

    }
}