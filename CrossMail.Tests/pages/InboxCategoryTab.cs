using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;

using CrossMail.Utils;
using System;

namespace CrossMail.Pages
{
    public class InboxCategoryTab
    {
        private IWebDriver _browserDriver;
        private WebDriverWait _wait;
        private Actions _actionsBuilder;
        
        [FindsBy (How = How.XPath, Using = "//*[@role='tab' and @aria-label='Social']")]
        private IWebElement _socialCategoryTabElement;

        public InboxCategoryTab(IWebDriver driver)
        {
            this._browserDriver = driver;
            this._wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            this._actionsBuilder = new Actions(driver);
        }

        public void GoToInbox(string strCategory)
        {
            if ( !strCategory.Equals("") )
            {
                try
                {
                    switch (strCategory)
                    {
                        case "Social" :
                            if ( null != _socialCategoryTabElement )
                            {
                                _actionsBuilder.MoveToElement(_socialCategoryTabElement)
                                               .Click()
                                               .Build()
                                               .Perform();
                            }
                            break;
                        default:
                            break;
                    }
                }
                catch (WebDriverTimeoutException ex)
                {
                    ProcessUtils.ConsoleLog( ex.Message );
                }
            }
            else
            {
                ProcessUtils.ConsoleLog("No CATEGORY provided.");
            }
        }

        public bool IsCategoryTabSelected(string strCategory)
        {
            switch (strCategory)
            {
                case "Social" :
                    return _socialCategoryTabElement.GetAttribute("aria-selected").Equals("true");
                default:
                    return false;
            }
            
        }

    }
}