using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;


namespace AutomacaoTests.SuiteManterEvento
{
    [TestFixture]
    public class CriarEventoValidoTest
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
        public void criarEventoValidoTest()
        {
            driver!.Navigate().GoToUrl("https://localhost:7131/");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            wait.Until(d => d.FindElement(By.LinkText("Entrar como Organização")));

            driver.FindElement(By.LinkText("Entrar como Organização")).Click();

            wait.Until(d => d.FindElement(By.Id("Input_Email")));

            driver.FindElement(By.Id("Input_Email")).SendKeys("lolo@gmail.com");

            driver.FindElement(By.Id("Input_Password")).SendKeys("123456");

            driver.FindElement(By.Id("login-submit")).Click();

            wait.Until(d => d.FindElement(By.CssSelector(".btn-text")));

            driver.FindElement(By.CssSelector(".btn-text")).Click();

            wait.Until(d => d.FindElement(By.Id("Nome")));

            driver.FindElement(By.Id("Nome")).SendKeys("Corrida Verão");

            js!.ExecuteScript(
                "document.getElementById('DataOnly').value='2025-05-30';");

            js.ExecuteScript(
                "document.getElementById('HoraOnly').value='06:00';");

            driver.FindElement(By.Id("NumeroParticipantes")).SendKeys("500");

            driver.FindElement(By.Id("Descricao"))
                .SendKeys("Prova para refrescar o corpo.");

            driver.FindElement(By.CssSelector(".check-tag:nth-child(1)")).Click();

            driver.FindElement(By.CssSelector(".check-tag:nth-child(2)")).Click();

            driver.FindElement(By.CssSelector(".check-tag:nth-child(3) > span")).Click();

            driver.FindElement(By.Id("Rua"))
                .SendKeys("Rua Jeremias da Silva Melo");

            driver.FindElement(By.Id("Bairro"))
                .SendKeys("São José");

            driver.FindElement(By.Id("Cidade"))
                .SendKeys("Aracaju");

            driver.FindElement(By.Id("Estado"))
                .SendKeys("SE");

            driver.FindElement(By.Id("InfoRetiradaKit"))
                .SendKeys("Camisa + Número do peito.");

            driver.FindElement(By.CssSelector(".btn-green")).Click();

            wait.Until(d =>
                d.FindElement(By.CssSelector(".kit-card:nth-child(2) .kit-title")));

            Assert.That(
                driver.FindElement(
                    By.CssSelector(".kit-card:nth-child(2) .kit-title")).Text,
                Is.EqualTo("Corrida Verão")
            );
        }
    }
}