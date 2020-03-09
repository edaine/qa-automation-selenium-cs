using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;
using SeleniumExtras.WaitHelpers;
using SeleniumExtras.PageObjects;

using CrossMail.Pages;

namespace CrossMail.Tests
{
    public class GMailTests: IClassFixture<GMailTestsFixture>
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly GMailTestsFixture _testFixture;

        public GMailTests(ITestOutputHelper output, GMailTestsFixture fixture)
        {
            this._outputHelper = output;
            this._testFixture = fixture;
            
        }

        [Fact]
        public void Should_Send_Email()
        {
            _testFixture.BrowserDriver.Navigate().GoToUrl("https://mail.google.com/");
            _testFixture.BrowserDriver.Manage().Window.Maximize();
            _testFixture.BrowserDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);

            LoginPage loginPage = PageFactory.InitElements<LoginPage>(_testFixture.BrowserDriver);
            loginPage.Login(_testFixture.Config["username"], _testFixture.Config["password"]);
            Assert.True(_testFixture.Wait.Until(ExpectedConditions.UrlContains("inbox")));

            // Create new file for attachment
            string filePath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string fileName = ($"{_testFixture.UniqueText}.txt");
            StreamWriter attachment = File.CreateText(filePath + $"\\{_testFixture.UniqueText}.txt");
            attachment.WriteLine("New file created: {0}", DateTime.Now.ToString());  
            attachment.Close();

            ComposeMessagePage composePage = PageFactory.InitElements<ComposeMessagePage>(_testFixture.BrowserDriver);
            composePage.ClickComposeButton();
            composePage.SendNewMessage(
                to: $"{_testFixture.Config["username"]}@gmail.com",
                subject: _testFixture.UniqueText,
                message: _testFixture.UniqueText,
                attachmentPath: ((FileStream)(attachment.BaseStream)).Name,
                attachmentFilename: fileName,
                label: "Social");

            Assert.True(composePage.IsAttachmentComplete());
            Assert.True(composePage.IsLabelCategoryChecked());
            Assert.True(composePage.IsMessageSent());

            InboxCategoryTab socialTab = PageFactory.InitElements<InboxCategoryTab>(_testFixture.BrowserDriver);
            socialTab.GoToInbox("Social");
            Assert.True(socialTab.IsCategoryTabSelected("Social"));

            ViewMessage email = PageFactory.InitElements<ViewMessage>(_testFixture.BrowserDriver);
            email.ClickOnViewMessageLink();
            Assert.True(email.IsSenderMatching($"{_testFixture.Config["username"]}@gmail.com"));
            Assert.True(email.IsSubjectMatching(_testFixture.UniqueText));
            Assert.True(email.IsBodyMatching(_testFixture.UniqueText));
            Assert.True(email.IsAttachmentFound(fileName));

            LogoutMenu logoutMenu = PageFactory.InitElements<LogoutMenu>(_testFixture.BrowserDriver);
            logoutMenu.Logout();
        }
    }
}
