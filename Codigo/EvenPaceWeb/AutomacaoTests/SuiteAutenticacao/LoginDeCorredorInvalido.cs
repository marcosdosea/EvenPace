using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace AutomacaoTests.SuiteAutenticacao
{
    [TestFixture]
    public class LoginDeCorredorInvalido
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
        public void entrarComCPFInvalido()
        {
            driver.Navigate().GoToUrl("https://localhost:7131/");

            driver.Manage().Window.Size =
                new System.Drawing.Size(790, 816);

            // Entrar como corredor
            driver.FindElement(
                By.LinkText("Entrar como Corredor")
            ).Click();

            // Preencher dados inválidos
            driver.FindElement(By.Id("email")).Click();
            driver.FindElement(By.Id("email"))
                .SendKeys("usuarioteste@gmail");

            driver.FindElement(By.Id("cpf")).Click();
            driver.FindElement(By.Id("cpf"))
                .SendKeys("456789");

            driver.FindElement(By.Id("senha")).Click();
            driver.FindElement(By.Id("senha"))
                .SendKeys("123456");

            // Clicar em entrar
            driver.FindElement(
                By.CssSelector(".evenpace-btn-yellow")
            ).Click();

            // Esperar mensagem aparecer
            System.Threading.Thread.Sleep(2000);

            // Validar mensagem de erro
            Assert.That(
                driver.PageSource.Contains("CPF inválido"),
                Is.True
            );
        }
    }
}