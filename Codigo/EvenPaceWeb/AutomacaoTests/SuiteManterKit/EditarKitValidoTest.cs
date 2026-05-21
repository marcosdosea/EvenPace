using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace AutomacaoTests.SuiteKit
{
    [TestFixture]
    public class EditarKitValidoTest
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
        public void editarKitValido()
        {
            driver.Navigate().GoToUrl("https://localhost:7131/");
            driver.Manage().Window.Size = new System.Drawing.Size(1552, 832);
            driver.FindElement(By.LinkText("Entrar como Organização")).Click();
            driver.FindElement(By.Id("Input_Email")).Click();
            driver.FindElement(By.Id("Input_Email")).SendKeys("lolo@gmail.com");
            driver.FindElement(By.Id("Input_Password")).Click();
            driver.FindElement(By.Id("Input_Password")).SendKeys("123456");
            driver.FindElement(By.Id("login-submit")).Click();
            driver.FindElement(By.LinkText("📂 Abrir")).Click();
            driver.FindElement(By.CssSelector(".page-header")).Click();
            Assert.That(driver.FindElement(By.CssSelector(".subtitle")).Text, Is.EqualTo("Corrida Beira Mar"));
            driver.FindElement(By.CssSelector(".menu-item:nth-child(7)")).Click();
            driver.FindElement(By.CssSelector(".page-header")).Click();
            Assert.That(driver.FindElement(By.CssSelector("h1")).Text, Is.EqualTo("KITS"));
            driver.FindElement(By.CssSelector(".kit-card:nth-child(1) .kit-title")).Click();
            Assert.That(driver.FindElement(By.CssSelector(".kit-card:nth-child(1) .kit-title")).Text, Is.EqualTo("Kit Básico Beira Mar"));
            driver.FindElement(By.LinkText("✏️ Editar")).Click();
            driver.FindElement(By.Id("Descricao")).Click();
            driver.FindElement(By.Id("Descricao")).SendKeys("");
            driver.FindElement(By.Id("Descricao")).SendKeys("Camiseta + número do peito.");
            driver.FindElement(By.CssSelector(".btn-gray:nth-child(1)")).Click();
        }
    }
}
