using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Linq;
using System.Threading;
using WebDriverManager.DriverConfigs.Impl;

namespace STS_automated_tests
{
    public class ExamplaryTests
    {
        ChromeDriver driver;
        private const string invalidEmail = "I_want_to_work_in@sts.pl";
        private const string invalidPassword = "RekrutacjaSTS*";
        private IWebElement ResponsibleBarButton => driver.FindElement(By.Id("responsible-bar-confirm"));
        private IWebElement CookieConfirmButton => driver.FindElement(By.Id("cookie-bar-confirm"));
        private IWebElement ZalogujButton => driver.FindElement(By.Id("sign-in-button"));
        private IWebElement EmailTextarea => driver.FindElement(By.Id("txtLogin"));
        private IWebElement PasswordTextarea => driver.FindElement(By.Id("txtPasswd-password"));
        private IWebElement ZalogujConfirmationButton => driver.FindElement(By.XPath("//input[@value='Zaloguj']"));
        private IWebElement StatystykiTab => driver.FindElement(By.XPath("//li[@data-page='stats']"));
        private IWebElement CentrumKlientaTab => driver.FindElement(By.XPath("//li[@data-page='customer-service']"));
        private IWebElement AbcZakladowTab => driver.FindElement(By.LinkText("ABC Zakładów"));


        private IWebElement SettingsButton => driver.FindElement(By.XPath("//a[@href='/pl/settings']"));
        private IWebElement CeskyVersionOfWebsiteButton => driver.FindElement(By.XPath("//a[@href='/cs']"));

        private IWebElement TypZakladowDropdownList => driver.FindElement(By.Id("types"));
        private IWebElement DyscyplinaDropdownList => driver.FindElement(By.Id("sports"));
        private IWebElement PodgraDropdownList => driver.FindElement(By.Id("games"));
        private IWebElement LiveTab => driver.FindElement(By.XPath("//li[@data-value='live']"));
        private IWebElement SiatkowkaPlazowaTab => driver.FindElement(By.XPath("//li[@data-value='SIATKÓWKA PLAŻOWA']"));

        private void CloseConsents()
        {
            ZalogujButton.Click();
            ResponsibleBarButton.Click();
            CookieConfirmButton.Click();
        }

        private void Login(string email, string password)
        {
            ZalogujButton.Click();
            EmailTextarea.Click();
            EmailTextarea.SendKeys(email);
            PasswordTextarea.Click();
            PasswordTextarea.SendKeys(password);
            ZalogujConfirmationButton.Click();
        }

        [SetUp]
        public void Setup()
        {
            new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);
            driver.Navigate().GoToUrl("https://www.sts.pl/");
            driver.Manage().Window.Maximize();

        }

        [Test]
        public void IsIloscPunktowWSecieVisible()
        {
            CloseConsents();
            CentrumKlientaTab.Click();
            AbcZakladowTab.Click();
            TypZakladowDropdownList.Click();
            LiveTab.Click();
            DyscyplinaDropdownList.Click();
            SiatkowkaPlazowaTab.Click();
            Assert.DoesNotThrow(() => driver.FindElement(By.ClassName("two-set-ilo-punktw-w-secie")), "Element not found");
        }

        [Test]
        public void LoginWithInvalidData()
        {
            CloseConsents();
            Login(invalidEmail, invalidPassword);
            Assert.DoesNotThrow(()=>driver.FindElement(By.ClassName("modal–login")), "The \"Wrong password\" window is not visible");
            // Use this assertion if you see the reCAPTCHA window instead of the window about wrong password. If so, remember to comment assertion in the line 83 and uncomment the one in line 85
            // Assert.DoesNotThrow(() => driver.FindElement(By.Id("_hjRemoteVarsFrame")), "There's no reCAPTCHA visible frame");
        }

        [Test]
        public void CheckCzechTranslationOfStatsPage()
        {
            CloseConsents();
            StatystykiTab.Click();
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            SettingsButton.Click();
            CeskyVersionOfWebsiteButton.Click();
            Assert.DoesNotThrow(() => driver.FindElement(By.ClassName("banner-image")), "404 – Not Found");
        }

        [TearDown]
        public void QuitDriver()
        {
            driver.Quit();
        }
    }
}