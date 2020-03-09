using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

using CrossMail.Utils;

namespace CrossMail.Tests
{
    public class GMailTestsFixture: IDisposable
    {
        public GMailTestsFixture()
        {
            ChromeOptions options = new ChromeOptions();
            options.AcceptInsecureCertificates = true;
            options.AddArgument("--enable-automation");
            
            BrowserDriver = new ChromeDriver("./", options);
            Wait = new WebDriverWait(BrowserDriver, TimeSpan.FromSeconds(20));

            Config = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .Build();
            
            UniqueText = ProcessUtils.GenerateRandomText(8);
        }

        public void Dispose()
        {
            BrowserDriver.Quit();
        }

        public IWebDriver BrowserDriver { get; private set; }
        public WebDriverWait Wait { get; private set; }
        public IConfiguration Config { get; private set; }
        public String UniqueText { get; private set; }
    }
}