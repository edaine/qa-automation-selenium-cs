using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using System;

using CrossMail.Utils;

namespace CrossMail.Pages
{
    public class ComposeMessagePage
    {
        private readonly IWebDriver _browserDriver;
        private readonly WebDriverWait _wait;
        private readonly Actions _actionsBuilder;

        [FindsBy (How = How.XPath, Using = "//*[@role='button' and text()='Compose']")]
        private IWebElement _composeButton;
        
        [FindsBy (How = How.XPath, Using = "//*[@role='dialog']//*[text()='New Message']")]
        private IWebElement _newMessageDialog;
        
        [FindsBy (How = How.Name, Using = "to")]           
        private IWebElement _receiverField;
        
        [FindsBy (How = How.XPath, Using = "//*[@aria-label='Subject']")]
        private IWebElement _subjectField;
        
        [FindsBy (How = How.XPath, Using = "//*[@role='textbox' and @aria-label='Message Body']")]
        private IWebElement _messageBody;
        
        [FindsBy (How = How.XPath, Using = "//input[@type='file' and @name='Filedata']")]
        private IWebElement _attachFile;
        private bool _isAttachmentComplete;

        [FindsBy (How = How.XPath, Using = "//*[@role='button' and text()='Send']")]
        private IWebElement _sendButtonElement;

        [FindsBy (How = How.XPath, Using = "//*[@role='button' and @aria-label='More options']")]
        private IWebElement _moreOptions;
        
        private bool _isMessageSent;

        private LabelMenu _labelMenu;
        private bool _isLabelApplied;

        public ComposeMessagePage(IWebDriver driver)
        {
            this._browserDriver = driver;
            this._wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            this._actionsBuilder = new Actions(driver);

            _isAttachmentComplete = false;
            _isMessageSent = false;
            _isLabelApplied = false;
        }

        public void ClickComposeButton()
        {
            try
            {
                if ( null != _wait.Until(ExpectedConditions.ElementToBeClickable(_composeButton)) )
                {
                    _composeButton.Click();
                    _wait.Until(ExpectedConditions.TextToBePresentInElement(_newMessageDialog, "New Message"));
                }
                else
                {
                    ProcessUtils.ConsoleLog("ERROR: COMPOSE Button Not Available");
                }
            }
            catch (WebDriverTimeoutException timeout)
            {
                ProcessUtils.ConsoleLog(timeout.Message);
            }
        }

        public void SendNewMessage(string to,
                                   string subject,
                                   string message,
                                   string attachmentPath = "",
                                   string attachmentFilename = "",
                                   string label = "")
        {
            _isMessageSent = false;
            _isAttachmentComplete = false;
            _isLabelApplied = false;

            this.SetReceiverField(to);
            this.SetSubjectField(subject);
            this.SetMessageBody(message);
            this.AddAttachment(strFullFilePath: attachmentPath, strFilename: attachmentFilename);
            this.LabelWithCategory(label);
            this.ClickSendButton();
        }

        public bool IsMessageSent()
        {
            return _isMessageSent;
        }

        public bool IsAttachmentComplete()
        {
            return _isAttachmentComplete;
        }

        public bool IsLabelCategoryChecked()
        {
            return _isLabelApplied;
        }

        private void SetReceiverField(string strReceiver)
        {
            try
            { 
                if ( null != _wait.Until(ExpectedConditions.ElementToBeClickable(_receiverField)) )
                {
                    _receiverField.Clear();
                    _receiverField.SendKeys(strReceiver);
                }
                else
                {
                    ProcessUtils.ConsoleLog("ERROR: New Message Receiver Field Not Found");
                }
            }
            catch (WebDriverTimeoutException timeout)
            {
                ProcessUtils.ConsoleLog(timeout.Message);
            }
        }

        public void SetSubjectField(string strSubject)
        {
            try
            { 
                if ( null != _wait.Until(ExpectedConditions.ElementToBeClickable(_subjectField)) )
                {
                    _subjectField.Clear();
                    _subjectField.SendKeys(strSubject);
                }
                else
                {
                    ProcessUtils.ConsoleLog("ERROR: New Message Subject Field Not Found");
                }
            }
            catch (WebDriverTimeoutException timeout)
            {
                ProcessUtils.ConsoleLog(timeout.Message);
            }
        }

        public void SetMessageBody(string strMessageBody)
        {
            try
            { 
                if ( null != _wait.Until(ExpectedConditions.ElementToBeClickable(_messageBody)) )
                {
                    _messageBody.Clear();
                    _messageBody.SendKeys(strMessageBody);
                }
                else
                {
                    ProcessUtils.ConsoleLog("ERROR: New Message Body Field Not Found");
                }
            }
            catch (WebDriverTimeoutException timeout)
            {
                ProcessUtils.ConsoleLog(timeout.Message);
            }
        }

        public void AddAttachment(string strFullFilePath, string strFilename)
        {
            if ( !strFullFilePath.Equals("") && !strFilename.Equals("") )
            {
                try
                { 
                    if ( null != _attachFile )
                    {
                        _attachFile.SendKeys(strFullFilePath);
                        _actionsBuilder.SendKeys(Keys.Enter);

                        By attachmentInfo = By.XPath($"//input[contains(@value,'{strFilename}')]");
                        _isAttachmentComplete = _wait.Until(ExpectedConditions.TextToBePresentInElementValue(attachmentInfo, strFilename));

                        ProcessUtils.ConsoleLog( $"Attach File Complete: {_isAttachmentComplete}");
                    }
                    else
                    {
                        ProcessUtils.ConsoleLog("ERROR: Input Attach File Path Not Found");
                    }
                }
                catch (WebDriverTimeoutException timeout)
                {
                    ProcessUtils.ConsoleLog(timeout.Message);
                }
            }
            else
            {
                ProcessUtils.ConsoleLog("No ATTACHMENT provided");
            }
        }

        private void LabelWithCategory(string strCategory)
        {
            if ( !strCategory.Equals("") )
            {
                if ( null != _moreOptions )
                {
                    _moreOptions.Click();
                    _labelMenu = PageFactory.InitElements<LabelMenu>(_browserDriver);
                    _labelMenu.LabelWithCategory(strCategory);
                    _isLabelApplied = _labelMenu.IsLabelCategoryChecked(strCategory);
                }
                else
                {
                    ProcessUtils.ConsoleLog("ERROR: More Options button not available");
                }
            }
            else
            {
                ProcessUtils.ConsoleLog("No CATEGORY LABEL provided.");
            }
        }

        private void ClickSendButton()
        {
            if ( null != _wait.Until(ExpectedConditions.ElementToBeClickable(_sendButtonElement)))
            {
                _actionsBuilder.MoveToElement(_sendButtonElement);
                _actionsBuilder.Click()
                               .Perform();
            }
            else
            {
                ProcessUtils.ConsoleLog("ERROR: Send Button Not Available");
            }

            IWebElement messageSentInfo = _wait.Until(ExpectedConditions.ElementExists(By.XPath("//span[text()='Message sent.']")));
            _isMessageSent = _wait.Until(ExpectedConditions.TextToBePresentInElement(messageSentInfo, "Message sent."));

            ProcessUtils.ConsoleLog( $"Message Sent: {_isMessageSent}");
        }        

    }
}