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
        /***
        Parameterized Constructor
        @brief  Attempts to connect the driver from the console to
                    the web browser.
        @param      path       the directory and file name (+extension) where
                    the driver is located.
        @param      browser     a enum with the desire browser to be connected.
        */
        public WebDriver(string path, IBrowser browser)
        {
            Establish(path, browser);
        }

        /***
        @brief  Attempts to connect the driver from the console to
                    the web browser.
        @param      path       the directory and file name (+extension) where
                    the driver is located.
        @param      browser     a enum with the desire browser to be connected.
        */    
        public void Establish(string path, IBrowser browser)
        {
            switch(browser)
            {
                case IBrowser.CHROME: withChrome(path); break;
                case IBrowser.FIREFOX: ; break;
                case IBrowser.EDGE:; break;
            }
        }
        /// <summary>
        /// @brief    Close the web driver connection.
        /// /// </summary>
        public void Close()
        {
            webdriver.Close();
            webdriver = null;
        }

        /// <summary>
        /// @brief    Checks if the driver connection was establish.
        /// </summary>
        /// <returns>If null (Con fail) | If not null (con success).</returns>
        public bool isConEstablished()
        {
            return webdriver != null;
        }

        /// <summary>
        ///  @brief    Checks whether a connection is established or not.
        /// </summary>
        public IWebDriver CurrentDriver {
            get {
                return webdriver;
            }
        }

        /// <summary>
        /// Creates and return a web element found by the driver base on the value 
        ///         and the method specify.
        /// </summary>
        /// <param name="value"> The value use to locate the element identifier</param>
        /// <param name="elem_by"> The method use to find the element.</param>
        /// <returns>A web element</returns>
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

        /// <summary>
        /// Creates and return a readonly collection of web elements found by the driver base on the value 
        ///         and the method specify.
        /// </summary>
        /// <param name="value"> The value use to locate the element identifier</param>
        /// <param name="elem_by"> The method use to find the element.</param>
        /// <returns>A read only collection of web elements</returns>
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

        /// <summary>
        /// @brief      It delay the driver action depending on the time unit.
        /// 
        /// @param      sec     Time to be delay in seconds.
        /// </summary>
        /// <param name="sec"></param>

        public void Wait(uint sec)
        {
            webdriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(sec);
        }

        /// <summary>
        /// @brief      Navigate to specific url address.
        /// 
        /// @param      url     The Uniform Resource Locator.
        /// </summary>
        /// <param name="url"></param>
        public void NavigateTo(string url)
        {
            webdriver.Navigate().GoToUrl(url);
        }


        /// <summary>
        /// @brief    Attempts to connect with the Chrome browser.
        /// @param    path       the directory and file name (+extension) where
        /// the driver is located.
        /// @throw    Exception if any error occurred establishing the connection.
        /// </summary>
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
