using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace AutomacaoTests.SuiteAutenticacao
{
    [TestFixture]
    public class EntradaDeDadosCorredorValidoTest
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
        public void entradaDeDadosCorredorValido()
        {
            driver.Navigate().GoToUrl("https://localhost:7131/");
            driver.Manage().Window.Size = new System.Drawing.Size(1552, 832);
            driver.FindElement(By.LinkText("Entrar como Corredor")).Click();
            driver.FindElement(By.Id("email")).Click();
            driver.FindElement(By.Id("email")).SendKeys("lorelore@gmail.com");
            driver.FindElement(By.Id("cpf")).Click();
            driver.FindElement(By.Id("cpf")).SendKeys("08607884581");
            driver.FindElement(By.Id("senha")).Click();
            driver.FindElement(By.Id("senha")).SendKeys("123456");
            driver.FindElement(By.CssSelector(".evenpace-btn-yellow")).Click();
            Assert.That(driver.FindElement(By.CssSelector("a:nth-child(2) .event-title")).Text, Is.EqualTo("Corrida Ibirapuera"));
        }
    }
}
