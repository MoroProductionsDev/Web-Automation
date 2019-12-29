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

        private void withChrome(string path)
        {
            try
            {
                webdriver = new ChromeDriver(path);
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
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
    }
}
