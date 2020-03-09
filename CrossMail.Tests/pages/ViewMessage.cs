using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;

using CrossMail.Utils;

namespace CrossMail.Pages
{
    public class ViewMessage
    {
        private IWebDriver _browserDriver;
        private WebDriverWait _wait;
        Actions _actionsBuilder;
        
        [FindsBy (How = How.XPath, Using = "//table//*[@role='row']")]
        private IList<IWebElement> _inboxMessageList;
        [FindsBy (How = How.Id, Using = "link_vsm")]
        private IWebElement _viewMessageLink;
        [FindsBy (How = How.XPath, Using = "//*[@role='button' and @aria-label='Labels']")]
        private IWebElement _labelsMenuButton;
        private IWebElement _email;
        

        public ViewMessage(IWebDriver driver)
        {
            this._browserDriver = driver;
            this._wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            this._actionsBuilder = new Actions(driver);

            _email = null;
        }

        public IWebElement SearchMailBySubject(string strSubject)
        {
            for ( int i = 0; i < _inboxMessageList.Count; i++ )
            {
                if ( _inboxMessageList[i].Text.Equals(strSubject) )
                {
                    return _inboxMessageList[i];
                }
            }
            
            return null;
            
        }

        public void ClickOnEmailBasedOnSubject(string strSubject)
        {
            _email = SearchMailBySubject(strSubject);

            if ( null != _email )
            {
                _actionsBuilder.MoveToElement(_email)
                               .Click()
                               .Build()
                               .Perform();

                _browserDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);

            }
            else
            {
                ProcessUtils.ConsoleLog("ERROR: No messages matched your search.");
            }
        }

        public void ClickOnViewMessageLink()
        {
            try
            {
                if ( null != _wait.Until(ExpectedConditions.ElementToBeClickable(_viewMessageLink)) )
                {
                    _actionsBuilder.MoveToElement(_viewMessageLink)
                                   .Click()
                                   .Build()
                                   .Perform();

                    _browserDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                }    
            }
            catch (WebDriverTimeoutException timeout)
            {
                ProcessUtils.ConsoleLog(timeout.Message);
            }

        }

        public bool IsSenderMatching(string strSender)
        {
            try
            {
                IWebElement senderElement = _wait.Until(ExpectedConditions.ElementExists(By.XPath($"//h3//span[@email='{strSender}']")));

                if ( null != senderElement )
                {
                    Console.WriteLine($"senderElement.GetAttribute('email'): {senderElement.GetAttribute("email")}, strSender: {strSender}");
                    return senderElement.GetAttribute("email").Equals(strSender);
                }    
            }
            catch (WebDriverTimeoutException timeout)
            {
                ProcessUtils.ConsoleLog(timeout.Message);
            }

            return false;
            
        }

        public bool IsSubjectMatching(string strSubject)
        {
            Console.WriteLine(_browserDriver.Title);
            return (_browserDriver.Title.StartsWith(strSubject));
        }

        public bool IsBodyMatching(string strMsgBody)
        {
            try
            {
                IWebElement messageBody = _wait.Until(ExpectedConditions.ElementExists(By.XPath($"//div[text()='{strMsgBody}']")));
                if ( null != messageBody )
                {
                    Console.WriteLine($"messageBody.Text: {messageBody.Text}, strSender: {strMsgBody}");
                    return messageBody.Text.Equals(strMsgBody);
                }    
            }
            catch (WebDriverTimeoutException timeout)
            {
                ProcessUtils.ConsoleLog(timeout.Message);
            }

            return false;            
        }

        public bool IsAttachmentFound(string strFilename)
        {
            try
            {
                IWebElement attachment = _wait.Until(ExpectedConditions.ElementExists(By.XPath($"//span[text()='{strFilename}']")));
                if ( null != attachment )
                {
                    Console.WriteLine($"attachment.Text: {attachment.Text}, strSender: {strFilename}");
                    return attachment.Text.Equals(strFilename);
                }    
            }
            catch (WebDriverTimeoutException timeout)
            {
                ProcessUtils.ConsoleLog(timeout.Message);
            }

            return false;            
        }

        public bool IsLabelApplied(string strLabel)
        {
            try
            {
                foreach (IWebElement labelCheckbox in _browserDriver.FindElements(By.XPath($"//*[@role='menuitemcheckbox' and @title='{strLabel}']")))
                {
                    Console.WriteLine($"labelCheckbox.GetAttribute('aria-checked'): {labelCheckbox.GetAttribute("aria-checked")}");
                    if ( labelCheckbox.GetAttribute("aria-checked").Equals("true") )
                    {
                        return true;
                    }
                }

            }
            catch (WebDriverTimeoutException timeout)
            {
                ProcessUtils.ConsoleLog(timeout.Message);
            }

            return false;
        }

    }
}