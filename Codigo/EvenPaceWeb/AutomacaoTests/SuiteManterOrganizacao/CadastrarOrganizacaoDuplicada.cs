using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace AutomacaoTests.SuiteAutenticacao
{
    [TestFixture]
    public class CadastrandoOrganizacaoDuplicada
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
        public void cadastrarOrganizacaoComCpfJaExistente()
        {
            driver.Navigate().GoToUrl("https://localhost:7131/");

            driver.Manage().Window.Size =
                new System.Drawing.Size(1552, 832);

            // Entrar como organização
            driver.FindElement(
                By.LinkText("Entrar como Organização")
            ).Click();

            // Ir para cadastro
            driver.FindElement(
                By.LinkText("Cadastre-se aqui")
            ).Click();

            // CPF já cadastrado
            driver.FindElement(By.Id("Input_Cpf")).Click();
            driver.FindElement(By.Id("Input_Cpf"))
                .SendKeys("12345678923");

            // Email inválido
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email"))
                .SendKeys("organizacao@gmail");

            // Senha
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password"))
                .SendKeys("123456");

            // Confirmar senha
            driver.FindElement(By.Id("Input_ConfirmPassword")).Click();
            driver.FindElement(By.Id("Input_ConfirmPassword"))
                .SendKeys("123456");

            // Avançar cadastro
            driver.FindElement(By.Id("registerSubmit")).Click();

            // Esperar mensagem aparecer
            System.Threading.Thread.Sleep(2000);

            // Validar mensagem
            Assert.That(
                driver.PageSource.Contains("CPF já cadastrado."),
                Is.True
            );
        }
    }
}