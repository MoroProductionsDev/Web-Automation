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

            if (driver.isConEstablished())
            {
                GoToHomePage(driver);
                ClosePopUpWindow(driver);
                BookTrip(driver);
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

            Thread.Sleep(700);
            // DEPARTURE_CITY
            ActionOnInput(driver, Element_By.NAME, InputAcition.CLICK, "search_form[departure_city]", "");
            ActionOnInput(driver, Element_By.NAME, InputAcition.SENDKEYS, "search_form[departure_city]", DEPARTURE_CITY);


            Thread.Sleep(700);
            // DESTINATION_CITY
            try
            {
                // Same distination not allowed.
                if (DESTINATION_CITY != DEPARTURE_CITY)
                {
                    ActionOnInput(driver, Element_By.NAME, InputAcition.CLICK, "search_form[destination_city]", "");
                    ActionOnInput(driver, Element_By.NAME, InputAcition.SENDKEYS, "search_form[destination_city]", DESTINATION_CITY);
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

            Thread.Sleep(1000);
            // Trip type (return) || (oneway)
            if (trip_choice == ROUND_TRIP)
            {
                // NEEDS WORK
                //driver.FindElement("//input[@type='radio'][@value='return']", Element_By.XPATH).Click();

                // departure date
                NextAvailablesDate(driver, "search_form[departure_date]", "depart", 0);

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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver">The current drive use in the this section.</param>
        /// <param name="by">The element method how the its going to be found.</param>
        /// <param name="action">The action about to be perform on the element (clickable, typeable...)</param>
        /// <param name="attribute_val">The attribute value that is use to find the element</param>
        /// <param name="key_val">The text stream to be inserted in the input element.</param>
        private static void ActionOnInput(web_automation.WebDriver driver, Element_By by, InputAcition action, string attribute_val, string keyboard_text)
        {
            IWebElement form_input;
            if ((form_input = driver.FindElement(attribute_val, by)) != null)
            {
                try
                {
                    switch (action)
                    {
                        case InputAcition.CLICK: form_input.Click(); break;
                        case InputAcition.SENDKEYS: form_input.SendKeys(keyboard_text); break;
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
            IWebElement nextmonth = null;               // ancher to table for the next month
            IReadOnlyCollection<IWebElement> tds = null;    // all the cell in the table body


            // Click
            ActionOnInput(driver, Element_By.NAME, InputAcition.CLICK, attribute_val, "");

            Thread.Sleep(1000);

            // Click
            ActionOnInput(driver, Element_By.NAME, InputAcition.CLICK, attribute_val, "");
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
                        ActionOnInput(driver, Element_By.CLASSNAME, InputAcition.CLICK, "ui-datepicker-next", "");
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
