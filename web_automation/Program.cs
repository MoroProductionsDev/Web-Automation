/*# @author: Raul Rivero Rubio
### @date:  12/27/2019
### @version: 1.0
### @brief: This application automate the process of filling a flighting form for Allegiant Airlines.
###         Base on the options provide it will calculate the total mount of the flight.
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
        enum InputAcition {CLICK, SENDKEYS };

        // Driver Local Location. Absolute Path Not a Resource
        const string ABSOLUTE_PATH = "C:\\Users\\Jesus\\Documents\\C#\\alliante_automation\\web_automation\\web_automation\\drivers\\chromedriver_win32";
        const string HOMEPAGE = "https://www.allegiantair.com";

        static void Main(string[] args)
        {
            // Initiate Web Driver
            web_automation.WebDriver driver = new WebDriver(ABSOLUTE_PATH, IBrowser.CHROME);
            driver.CurrentDriver.Manage().Window.Maximize();    // Maximize window

            if (driver.isConEstablished())
            {
                GoToHomePage(driver);
                ClosePopUpWindow(driver);
                BookTrip(driver);
                Verify(driver);
                bundle(driver);
            }
        }

        /// <summary>
        /// Navigates to website homepage.
        /// </summary>
        /// <param name="driver">The current drive use in the this section.</param>
        static void GoToHomePage(web_automation.WebDriver driver)
        {
            driver.NavigateTo(HOMEPAGE);
        }

        /// <summary>
        /// Closes a popup modal window.
        /// </summary>
        /// <param name="driver">The current drive use in the this section.</param>
        static void ClosePopUpWindow(web_automation.WebDriver driver)
        {
            IWebElement modalPopup;
            if ((modalPopup = driver.FindElement("ui-icon-closethick", Element_By.CLASSNAME)) != null)
            {
                Thread.Sleep(1000);
                modalPopup.Click();
            }
        }

        /// <summary>
        /// Proceeds with the automation on the landing page to book the trip.
        /// </summary>
        /// <param name="driver">The current drive use in the this section.</param>
        static void BookTrip(web_automation.WebDriver driver)
        {
            // Book Trip Options
            const string DEPARTURE_CITY = "Las Vegas, NV (LAS)";
            const string DESTINATION_CITY = "Albuquerque, NM (ABQ)";
            // Const int (Enums)
            const int ROUND_TRIP = 0;
            const int ONEWAY = 1;
            int trip_choice = 0;
            IWebElement form_input = null;

            Thread.Sleep(1000);
            // DEPARTURE_CITY
            form_input = GetElement(driver, Element_By.NAME, "search_form[departure_city]");
            ActionOnInput(form_input, InputAcition.CLICK, "");
            ActionOnInput(form_input, InputAcition.SENDKEYS, DEPARTURE_CITY);

            Thread.Sleep(1000);
            // DESTINATION_CITY
            try
            {
                // Same distination not allowed.
                if (DESTINATION_CITY != DEPARTURE_CITY)
                {

                    form_input = GetElement(driver, Element_By.NAME, "search_form[destination_city]");
                    ActionOnInput(form_input, InputAcition.CLICK, "");
                    ActionOnInput(form_input, InputAcition.SENDKEYS, DESTINATION_CITY);
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

            Thread.Sleep(1500);

            // Trip type (return) || (oneway)
            if (trip_choice == ROUND_TRIP)
            {
                // NEEDS WORK
                //driver.FindElement("//input[@type='radio'][@value='return']", Element_By.XPATH).Click();

                // departure date
                NextAvailablesDate(driver, "search_form[departure_date]", "depart", 0);

                Thread.Sleep(1500);

                // return date
                NextAvailablesDate(driver, "search_form[return_date]", "return", 0);
            }
            else if (trip_choice == ONEWAY)
            {
                driver.Wait(100);
                // NEEDS WORK
                //driver.FindElement("//input[@type='radio'][@value='oneway']", Element_By.XPATH).Click();

                // departure date
                NextAvailablesDate(driver, "search_form[departure_date]", "depart", 0);
            }

            Thread.Sleep(1000);

            uint adultsCnt = 3;         // adults count
            uint childrenCnt = 1;       // children count
            form_input = GetElement(driver, Element_By.NAME, "search_form[adults]");
            // Count Adult Passengers
            ActionOnInput(form_input, InputAcition.CLICK, "");
            ActionOnInput(form_input, InputAcition.SENDKEYS, adultsCnt.ToString());

            // Count Children Passengers
            form_input = GetElement(driver, Element_By.NAME, "search_form[children]");
            ActionOnInput(form_input, InputAcition.CLICK, "");
            ActionOnInput(form_input, InputAcition.SENDKEYS, childrenCnt.ToString());
            ActionOnInput(form_input, InputAcition.CLICK, "");

            Thread.Sleep(1500);

            if (form_input.Text != "0")
            {
                // array containers of name attribute of the inputs.
                string[] months =  {"search_form[search_child_1_month]", "search_form[search_child_2_month]", "search_form[search_child_3_month]" };
                string[] days = { "search_form[search_child_1_day]", "search_form[search_child_2_day]", "search_form[search_child_3_day]" };
                string[] years = { "search_form[search_child_1_year]", "search_form[search_child_2_year]", "search_form[search_child_3_year]" };

                Thread.Sleep(1000);

                for (uint i = 0; i < childrenCnt; ++i) {
                    form_input = GetElement(driver, Element_By.NAME, months[i]);
                    ActionOnInput(form_input, InputAcition.CLICK, "");
                    ActionOnInput(form_input, InputAcition.SENDKEYS, "JUL");

                    Thread.Sleep(1000);

                    form_input = GetElement(driver, Element_By.NAME, days[i]);
                    ActionOnInput(form_input, InputAcition.CLICK, "");
                    ActionOnInput(form_input, InputAcition.SENDKEYS, "19");

                    Thread.Sleep(1000);

                    form_input = GetElement(driver, Element_By.NAME, years[i]);
                    ActionOnInput(form_input, InputAcition.CLICK, "");
                    ActionOnInput(form_input, InputAcition.SENDKEYS, "2015");
                    ActionOnInput(form_input, InputAcition.CLICK, "");
                }

                //Thread.Sleep(5000);
                //GetElement(driver, Element_By.NAME, "search_form[search_child_1_lap]").Click();

                //Thread.Sleep(3000);
                //GetElement(driver, Element_By.CLASSNAME, "close").Click();
            }

            Thread.Sleep(1500);
            // Next Page
            GetElement(driver, Element_By.ID, "submit-search").Click();
        }

        static void Verify(web_automation.WebDriver driver)
        {
            Thread.Sleep(1500);

            // Scroll Down
            IWebElement element = GetElement(driver, Element_By.XPATH, "//*[@id=\"flights\"]/div[6]/div[3]/button");
            Actions actions = new Actions(driver.CurrentDriver);
            actions.MoveToElement(element);
            actions.Perform();

            Thread.Sleep(1500);
            // Next Page
            element.Click();
        }

        static void bundle(web_automation.WebDriver driver)
        {
            IWebElement element;
            Thread.Sleep(1500);
            string[] bundle = { "//*[@id=\"package\"]/fieldset/div/div[3]/div/div[2]/div[2]/button",
                                "//*[@id=\"package\"]/fieldset/div/div[5]/div/div[2]/div[2]/button" };

            // Get Bundle
            GetElement(driver, Element_By.XPATH, bundle[0]).Click();

            Thread.Sleep(1500);
            // Next Page
            element = GetElement(driver, Element_By.CLASSNAME, "continue");
            element.Click();
        }

        /// <summary>
        /// Finds an web eleemnt base on the value of the attribute and the method which is found (id, class, name, tagname...)
        /// </summary>
        /// <param name="driver">The current drive use in the this section.</param>
        /// <param name="by">The element method how the its going to be found.</param>
        /// <param name="attribute_val">The attribute value that is use to find the element</param>
        /// <returns>The foud web element || Null if not found</returns>
        private static IWebElement GetElement(web_automation.WebDriver driver, Element_By by, string attribute_val)
        {
            return (driver.FindElement(attribute_val, by));
        }

        /// <summary>
        /// Perform an action in an web element.
        /// </summary>
        /// <param name="input">The input element where the action is going to execute.</param>
        /// <param name="action">The action about to be perform on the element (clickable, typeable...)</param>
        /// <param name="key_val">The text stream to be inserted in the input element.</param>
        private static void ActionOnInput(IWebElement input, InputAcition action, string keyboard_text)
        {
            if (input != null)
            {
                try
                {
                    switch (action)
                    {
                        case InputAcition.CLICK: input.Click(); break;
                        case InputAcition.SENDKEYS: input.SendKeys(keyboard_text); break;
                    }
                }
                catch (ElementClickInterceptedException elmClkInt_Ex)
                {
                    Console.WriteLine(elmClkInt_Ex.Message);
                } 
                catch (ElementNotVisibleException elemNotVis_EX)
                {
                    Console.WriteLine(elemNotVis_EX.Message);
                }
                catch (InvalidElementStateException invElemState_Ex)
                {
                    Console.WriteLine(invElemState_Ex.Message);
                }
                catch (StaleElementReferenceException staElemRef_Ex)
                {
                    Console.WriteLine(staElemRef_Ex.Message);
                }
            }
        }

        /// <summary>
        /// Find the next Available date for departure or return.
        /// </summary>
        /// <param name="driver">The current drive use in the this section.</param>
        /// <param name="attribute_val">The attribute value that is use to find the element.</param>
        /// <param name="flightType">The type of flight either (oneway) || roundtrip (return).</param>
        /// <param name="skip_days">The counter of how many available dates should be skip.</param>
        private static void NextAvailablesDate(web_automation.WebDriver driver, string attribute_val, string flightType, uint skip_days)
        {
            bool foundAvailableDate = false;            // flag for available date
            string attr;                                // value of a attribute
            IWebElement table_body = null;
            IWebElement table = null;
            IWebElement form_input = null;               
            IReadOnlyCollection<IWebElement> tds = null;    // all the cell in the table body


            // Click
            form_input = GetElement(driver, Element_By.NAME, attribute_val);
            ActionOnInput(form_input, InputAcition.CLICK, "");

            Thread.Sleep(1000);

            // Click
            ActionOnInput(form_input, InputAcition.CLICK, "");
            try
            {
                // Keep Iterating until you find an available date
                while (!foundAvailableDate)
                {
                    if (flightType == "depart")
                    {
                        // full (absolute) xpath
                        table = driver.FindElement("/html/body/div[5]/div/div/div[1]/div[1]/div/div[2]/div[1]/div/div/div[1]/form/div/div[1]/div[2]/div[1]/div/div/div/div[2]/table", Element_By.XPATH);
                    }
                    else
                    {
                        table = driver.FindElement("/html/body/div[5]/div/div/div[1]/div[1]/div/div[2]/div[1]/div/div/div[1]/form/div/div[1]/div[2]/div[2]/div/div/div/div[2]/table", Element_By.XPATH);
                    }

                    table_body = table.FindElement(By.TagName("tbody"));
                    tds = table_body.FindElements(By.TagName("td"));

                    // Iterate through all the table cell
                    foreach (IWebElement entry in tds)
                    {
                        // Only the available dates have an id.
                        attr = entry.GetAttribute("id");

                        // if it is not empty
                        if (attr != "")
                        {
                            // Check if their days needed to be skipped
                            if (skip_days == 0)
                            {
                                driver.Wait(50);
                                entry.Click();
                                foundAvailableDate = true;
                            } else
                            {
                                // is (Unsigned int). Only decrement while is not 0.
                                --skip_days;
                            }
                            break;
                        }
                    }

                    // If no available date found in the month. Proceed to next month
                    if (!foundAvailableDate)
                    {
                        // CLICK (Next Month)
                        form_input = GetElement(driver, Element_By.CLASSNAME, "ui-datepicker-next");
                        ActionOnInput(form_input, InputAcition.CLICK, "");
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
            }
            catch (ElementNotVisibleException elemNotVis_EX)
            {
                Console.WriteLine(elemNotVis_EX.Message);
            }
            catch (StaleElementReferenceException staElemRef_Ex)
            {
                Console.WriteLine(staElemRef_Ex.Message);
            }
        }
    }
}
