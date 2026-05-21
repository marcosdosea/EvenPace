using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace AutomacaoTests.SuiteInscricao
{
    [TestFixture]
    public class InscrevernaCorridaValida
    {
        private IWebDriver? driver;
        private IJavaScriptExecutor? js;

        [SetUp]
        public void SetUp()
        {
            EdgeOptions options = new EdgeOptions();

            options.AcceptInsecureCertificates = true;

            driver = new EdgeDriver(options);

            driver.Manage().Window.Maximize();

            js = (IJavaScriptExecutor)driver;
        }

        [TearDown]
        public void TearDown()
        {
            driver?.Quit();
            driver?.Dispose();
        }

        [Test]
        public void inscreverNaCorridaValida()
        {
            driver!.Navigate().GoToUrl("https://localhost:7131/");

            driver.Manage().Window.Size =
                new System.Drawing.Size(1552, 832);

            driver.FindElement(
                By.LinkText("Entrar como Corredor")
            ).Click();

          
            driver.FindElement(By.Id("email"))
                .SendKeys("teste@gmail.com");

            driver.FindElement(By.Id("cpf"))
                .SendKeys("12345678912");

            driver.FindElement(By.Id("senha"))
                .SendKeys("123456");

            driver.FindElement(
                By.CssSelector(".evenpace-btn-yellow")
            ).Click();

            Thread.Sleep(2000);

        
            driver.FindElement(
                By.CssSelector("a:nth-child(1) .btn-inscrever")
            ).Click();

            Thread.Sleep(2000);

      
            driver.FindElement(
                By.CssSelector(".kit-card:nth-child(10) > .btn-inscrever")
            ).Click();

            Thread.Sleep(2000);

            driver.FindElement(
                By.CssSelector(".opcoes:nth-child(8) > .opcao-label:nth-child(1)")
            ).Click();

            driver.FindElement(
                By.CssSelector(".opcoes:nth-child(10) > .opcao-label:nth-child(1) > .checkmark")
            ).Click();

            driver.FindElement(
                By.CssSelector(".btn-confirmar")
            ).Click();

            Thread.Sleep(2000);

       
            Assert.That(
                driver.PageSource.Contains("Inscrição"),
                Is.True
            );
        }
    }
}