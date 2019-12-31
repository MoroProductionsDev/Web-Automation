/*# @author: Raul Rivero Rubio
### @date:  12/27/2019
### @version: 1.0
### @brief: webdriver class that creates a connect with a
###         specific web browser. If successful it it will
###         a browser object ready for automation.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace web_automation
{
    public enum IBrowser { CHROME, FIREFOX, EDGE}
    public enum Element_By {ID, NAME, CLASSNAME,TAGNAME , XPATH}


    public class WebDriver
    {
        private IWebDriver webdriver = null;

        public WebDriver(string path, IBrowser browser)
        {
            Establish(path, browser);
        }

        public void Establish(string path, IBrowser browser)
        {
            switch(browser)
            {
                case IBrowser.CHROME: withChrome(path); break;
                case IBrowser.FIREFOX: ; break;
                case IBrowser.EDGE:; break;
            }
        }

        public void Close()
        {
            webdriver.Close();
            webdriver = null;
        }

        public bool isConEstablished()
        {
            return webdriver != null;
        }

        public IWebDriver CurrentDriver {
            get {
                return webdriver;
            }
        }
        
        public IWebElement FindElement(string value, Element_By elem_by)
        {
            IWebElement element = null;
            try
            {
                switch (elem_by)
                {
                    case Element_By.ID:
                        element = webdriver.FindElement(By.Id(value));
                        break;
                    case Element_By.NAME:
                        element = webdriver.FindElement(By.Name(value));
                        break;
                    case Element_By.CLASSNAME:
                        element = webdriver.FindElement(By.ClassName(value));
                        break;
                    case Element_By.TAGNAME:
                        element = webdriver.FindElement(By.TagName(value));
                        break;
                    case Element_By.XPATH:
                        element = webdriver.FindElement(By.XPath(value));
                        break;
                }
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return element;
        }

        public IReadOnlyCollection<IWebElement> FindElements(string value, Element_By elem_by)
        {
            IReadOnlyCollection<IWebElement> elementsList = null;
            try
            {
                switch (elem_by)
                {
                    case Element_By.ID:
                        elementsList = webdriver.FindElements(By.Id(value));
                        break;
                    case Element_By.NAME:
                        elementsList = webdriver.FindElements(By.Name(value));
                        break;
                    case Element_By.CLASSNAME:
                        elementsList = webdriver.FindElements(By.ClassName(value));
                        break;
                    case Element_By.TAGNAME:
                        elementsList = webdriver.FindElements(By.TagName(value));
                        break;
                    case Element_By.XPATH:
                        elementsList = webdriver.FindElements(By.XPath(value));
                        break;
                }
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return elementsList;
        }

        public void Wait(uint sec)
        {
            webdriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(sec);
        }

        public void NavigateTo(string url)
        {
            webdriver.Navigate().GoToUrl(url);
        }

        private void withChrome(string path)
        {
            try
            {
                webdriver = new ChromeDriver(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
