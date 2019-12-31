/*# @author: Raul Rivero Rubio
### @date:  12/27/2019
### @version: 1.0
### @brief: 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;

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
                BookTrip(driver);
            }
        }

        static void GoToHomePage(web_automation.WebDriver driver)
        {
            driver.NavigateTo(HOMEPAGE);
        }

        static void ClosePopUpWindow(web_automation.WebDriver driver)
        {
            IWebElement modalPopup;
            if ((modalPopup = driver.FindElement("ui-icon-closethick", Element_By.CLASSNAME)) != null)
            {
                modalPopup.Click();
            }
        }


        static void BookTrip(web_automation.WebDriver driver)
        {
            IWebElement form_input;
            IReadOnlyCollection<IWebElement> elemlist;
            const string DEPARTURE_CITY = "Las Vegas, NV (LAS)";
            const string DESTINATION_CITY = "Albuquerque, NM (ABQ)";
            const int ROUND_TRIP = 0;
            const int ONEWAY = 1;
            int choice = 1;

            Thread.Sleep(1000);
            if ((form_input = driver.FindElement("search_form[departure_city]", Element_By.NAME)) != null)
            {
                try
                {
                    form_input.Click();
                    form_input.SendKeys(DEPARTURE_CITY);
                }
                catch (ElementClickInterceptedException elmClkInt_Ex)
                {
                    Console.WriteLine(elmClkInt_Ex.Message);
                }
            }

            if ((form_input = driver.FindElement("search_form[destination_city]", Element_By.NAME)) != null)
            {
                form_input.Click();
                try
                {
                    if (DESTINATION_CITY != DEPARTURE_CITY)
                    {
                        form_input.SendKeys(DESTINATION_CITY);
                    }
                    else
                    {
                        throw new System.ArgumentException($"Departure city and destination city most be different. {DEPARTURE_CITY} : {DESTINATION_CITY}");
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            elemlist = driver.FindElements("search_form[trip_type]", Element_By.NAME);
            Console.WriteLine(elemlist);
            if (choice == ROUND_TRIP)
            {
                driver.FindElement("//input[@type='radio'][@value='return']", Element_By.XPATH).Click();

                // departure date
                NextAvailablesDate(driver, "search_form[departure_date]", "depart");

                // return date
                NextAvailablesDate(driver, "search_form[return_date]", "return");
            }
            else if (choice == ONEWAY)
            {
                driver.Wait(100);
                driver.FindElement("//input[@type='radio'][@value='return']", Element_By.XPATH).Click();
                driver.FindElement("//input[@type='radio'][@value='oneway']", Element_By.XPATH).Click();

                // departure date
                NextAvailablesDate(driver, "search_form[departure_date]", "depart");
            }
        }

        private static void NextAvailablesDate(web_automation.WebDriver driver, string value, string at)
        {
            bool foundAvailableDate = false;
            string attr;
            IWebElement datepicker_div = null;
            IWebElement table_head = null;
            IWebElement table_body = null;
            IWebElement form_input = null;
            IWebElement table = null;
            IWebElement nextmonth = null;
            IReadOnlyCollection<IWebElement> rows = null;
            IReadOnlyCollection<IWebElement> tds = null;
            WebDriverWait wait = new WebDriverWait(driver.CurrentDriver, TimeSpan.FromSeconds(20));

            Actions actions = new Actions(driver.CurrentDriver);
            if ((form_input = driver.FindElement(value, Element_By.NAME)) != null)
            {
                try
                {
                    form_input.Click();
                } catch (ElementClickInterceptedException elemClckIntEX)
                {
                    Console.WriteLine(elemClckIntEX);
                }
            }

            Thread.Sleep(1000);
            form_input.Click();
            try
            {
                // 
                datepicker_div = driver.FindElement(at, Element_By.CLASSNAME);
                // actions.DoubleClick(calender).Perform();
                //form_input.FindElement(By.ClassName("datepicker-toggle")).Click();
                //table = datepicker_div.FindElement(By.ClassName("table"));

                //table_body = datepicker_div.FindElement(By.TagName("tbody

                //rows = table_body.FindElements(By.TagName("tr"));

                //foreach (IWebElement row in rows)
                //{
                datepicker_div = driver.FindElement(at, Element_By.CLASSNAME);

                while (!foundAvailableDate)
                {
                    if (at == "depart")
                    {
                        table = driver.CurrentDriver.FindElement(By.XPath("/html/body/div[5]/div/div/div[1]/div[1]/div/div[2]/div[1]/div/div/div[1]/form/div/div[1]/div[2]/div[1]/div/div/div/div[2]/table"));
                    }
                    else
                    {
                        table = driver.CurrentDriver.FindElement(By.XPath("/html/body/div[5]/div/div/div[1]/div[1]/div/div[2]/div[1]/div/div/div[1]/form/div/div[1]/div[2]/div[2]/div/div/div/div[2]/table"));
                    }

                    //wait.Until(ExpectedConditions.ElementToBeClickable());
                    table_body = table.FindElement(By.TagName("tbody"));
                    tds = table_body.FindElements(By.TagName("td"));

                    foreach (IWebElement entry in tds)
                    {
                        attr = entry.GetAttribute("id");
                        if (attr != "")
                        {
                            driver.Wait(50);
                            entry.Click();
                            foundAvailableDate = true;
                            break;
                        }
                    }

                    if (!foundAvailableDate)
                    {
                        nextmonth = driver.CurrentDriver.FindElement(By.ClassName("ui-datepicker-next"));
                        nextmonth.Click();
                    }
            }
            }
            catch (NoSuchElementException nseEx)
            {
                Console.WriteLine(nseEx.Message);
            }
            catch (System.NullReferenceException nrfEx)
            {
                Console.WriteLine(nrfEx.Message);
            }/*
            catch (StaleElementReferenceException sterEx)
            {
                Console.WriteLine(sterEx.Message);
            }*/
        }
    }
}
