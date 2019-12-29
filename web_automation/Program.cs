/*# @author: Raul Rivero Rubio
### @date:  12/27/2019
### @version: 1.0
### @brief: 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace web_automation
{
    class Program
    {
        const string RELATIVE_PATH = "C:\\Users\\Jesus\\Documents\\C#\\alliante_automation\\web_automation\\web_automation\\drivers\\chromedriver_win32";
        const string HOMEPAGE = "https://www.allegiantair.com";


        static void Main(string[] args)
        {
            web_automation.WebDriver driver = new WebDriver(RELATIVE_PATH, IBrowser.CHROME);

            if (driver.CurrentDriver != null)
            {
                GoToHomePage(driver);
                ClosePopUpWindow(driver);
            }
        }

        static void GoToHomePage(web_automation.WebDriver driver)
        {
            try {
                driver.CurrentDriver.Navigate().GoToUrl(HOMEPAGE);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void ClosePopUpWindow(web_automation.WebDriver driver)
        {
            try
            {
                //driver.CurrentDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                driver.CurrentDriver.FindElement(By.ClassName("ui-icon-closethick")).Click();
                Console.WriteLine("close pop");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /*
        static void BookTrip(web_automation.WebDriver driver)
        {
            try
            {
                IWebElement input = driver.CurrentDriver.FindElement(By.Name("search_form[departure_city]"));
                SelectElement selectElement = new SelectElement(input);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        */
    }
}
