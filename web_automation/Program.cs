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
        enum InputAcition { CLICK, SENDKEYS };
        private enum ThreadTimeSpan{HALF_SEC, ONE_SEC, ONE_HALF_SEC, THREE_SEC }
        private enum DriverTimeSpan {THREE_SEC, FIVE_SEC, TEN_SEC};

        // Driver Local Location. Absolute Path Not a Resource
        const string ABSOLUTE_PATH = "C:\\Users\\Jesus\\Documents\\C#\\alliante_automation\\web_automation\\web_automation\\drivers\\chromedriver_win32";
        const string HOMEPAGE = "https://www.allegiantair.com/";
        static readonly uint[] DRIVER_WAIT = {3,  5, 10 };
        static readonly int[] THREAD_SLEEP = { 500, 1000, 1500, 3000 };

        static void Main(string[] args)
        {
            // Initiate Web Driver
            web_automation.WebDriver driver;

            // Driver initialization
            // Absolute Path
            driver = new WebDriver(ABSOLUTE_PATH, IBrowser.CHROME);

            driver.CurrentDriver.Manage().Window.Maximize();    // Maximize window

            if (driver.isConEstablished())
            {
                GoToHomePage(driver);
                
                // Proceed if the navigated to the inteded web site
                Assert.AreEqual(HOMEPAGE, driver.CurrentDriver.Url);

                ClosePopUpWindow(driver);
                BookTrip(driver);

                Thread.Sleep(THREAD_SLEEP[(int)ThreadTimeSpan.THREE_SEC]);
                Verify(driver);

                Thread.Sleep(THREAD_SLEEP[(int)ThreadTimeSpan.THREE_SEC]);
                bundle(driver);

                Thread.Sleep(THREAD_SLEEP[(int)ThreadTimeSpan.ONE_SEC]);
                CarRental(driver);

                Thread.Sleep(THREAD_SLEEP[(int)ThreadTimeSpan.HALF_SEC]);
                CheckTotal(driver);
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
                Thread.Sleep(THREAD_SLEEP[(int)ThreadTimeSpan.ONE_HALF_SEC]);
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
            // User Data
            const string DEPARTURE_CITY = "Las Vegas, NV (LAS)";
            const string DESTINATION_CITY = "Albuquerque, NM (ABQ)";
            string[] usrMonth = { "JUL" , "MAY", "NOV"};
            uint[] usrDay = { 19, 7, 30};
            uint[] usrYear = { 2015, 2013, 1995};       
            int trip_choice = 0;
            bool wantToddlerChair = false;
            uint[] skip_days = {2 , 3}; // departure, return
            // Const int (Enums)
            const int ROUND_TRIP = 0;
            const int ONEWAY = 1;
            IWebElement form_input = null;

            driver.Wait(DRIVER_WAIT[(int) DriverTimeSpan.THREE_SEC]);
            Thread.Sleep(THREAD_SLEEP[(int)ThreadTimeSpan.ONE_SEC]);

            // DEPARTURE_CITY
            form_input = GetElement(driver, Element_By.NAME, "search_form[departure_city]");
            ActionOnInput(form_input, InputAcition.CLICK, null);
            ActionOnInput(form_input, InputAcition.SENDKEYS, DEPARTURE_CITY);

            Thread.Sleep(1000);
            // DESTINATION_CITY
            try
            {
                // Same distination not allowed.
                if (DESTINATION_CITY != DEPARTURE_CITY)
                {

                    form_input = GetElement(driver, Element_By.NAME, "search_form[destination_city]");
                    ActionOnInput(form_input, InputAcition.CLICK, null);
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

            Thread.Sleep(THREAD_SLEEP[(int)ThreadTimeSpan.ONE_HALF_SEC]);

            // Trip type (return) || (oneway)
            if (trip_choice == ROUND_TRIP)
            {
                IWebElement radioBtn;

                // [[NEEDS TO BE FIX]]
                radioBtn = driver.FindElement("//input[@type='radio'][@value='return']", Element_By.XPATH);
                if (!radioBtn.Selected)
                {
                    ActionOnInput(radioBtn, InputAcition.CLICK, null);
                }
                // [[end]]

                // departure date
                NextAvailablesDate(driver, "search_form[departure_date]", "depart", skip_days[0], false);

                Thread.Sleep(THREAD_SLEEP[(int)ThreadTimeSpan.ONE_HALF_SEC]);

                // return date
                NextAvailablesDate(driver, "search_form[return_date]", "return", skip_days[1], true);
            }
            else if (trip_choice == ONEWAY)
            {
                IWebElement radioBtn;

                radioBtn = driver.FindElement("//input[@type='radio'][@value='oneway']", Element_By.XPATH);

                // [[NEEDS TO BE FIX]]. Radio button
                if (!radioBtn.Selected)
                {
                    ActionOnInput(radioBtn, InputAcition.CLICK, null);
                }
                // [[end]]

                // departure date
                NextAvailablesDate(driver, "search_form[departure_date]", "depart", skip_days[0], false);
            }

            Thread.Sleep(1000);

            uint adultsCnt = 3;         // adults count
            uint childrenCnt = 2;       // children count
            form_input = GetElement(driver, Element_By.NAME, "search_form[adults]");
            // Count Adult Passengers
            ActionOnInput(form_input, InputAcition.CLICK, null);
            ActionOnInput(form_input, InputAcition.SENDKEYS, adultsCnt.ToString());

            // Count Children Passengers
            form_input = GetElement(driver, Element_By.NAME, "search_form[children]");
            ActionOnInput(form_input, InputAcition.CLICK, null);
            ActionOnInput(form_input, InputAcition.SENDKEYS, childrenCnt.ToString());
            ActionOnInput(form_input, InputAcition.CLICK, null);

            if (form_input.Text != "0")
            {
                // array containers of name attribute of the inputs.
                string[] months =  {"search_form[search_child_1_month]", "search_form[search_child_2_month]", "search_form[search_child_3_month]" };
                string[] days = { "search_form[search_child_1_day]", "search_form[search_child_2_day]", "search_form[search_child_3_day]" };
                string[] years = { "search_form[search_child_1_year]", "search_form[search_child_2_year]", "search_form[search_child_3_year]" };

                Thread.Sleep(THREAD_SLEEP[(int)ThreadTimeSpan.ONE_HALF_SEC]);

                for (uint i = 0; i < childrenCnt; ++i) {
                    form_input = GetElement(driver, Element_By.NAME, months[i]);
                    ActionOnInput(form_input, InputAcition.CLICK, null);
                    ActionOnInput(form_input, InputAcition.SENDKEYS, usrMonth[i]);

                    Thread.Sleep(THREAD_SLEEP[(int)ThreadTimeSpan.ONE_SEC]);

                    form_input = GetElement(driver, Element_By.NAME, days[i]);
                    ActionOnInput(form_input, InputAcition.CLICK, null);
                    ActionOnInput(form_input, InputAcition.SENDKEYS, usrDay[i].ToString());

                    Thread.Sleep(THREAD_SLEEP[(int)ThreadTimeSpan.ONE_SEC]);

                    form_input = GetElement(driver, Element_By.NAME, years[i]);
                    ActionOnInput(form_input, InputAcition.CLICK, null);
                    ActionOnInput(form_input, InputAcition.SENDKEYS, usrYear[i].ToString());
                    ActionOnInput(form_input, InputAcition.CLICK, null);

                    // [[NEEDS TO BE FIX]]. Radio button
                    if (DateTime.Now.Year - usrYear[i] <= 2)
                    {
                        Thread.Sleep(2000);
                        if (wantToddlerChair)
                        {
                            form_input = GetElement(driver, Element_By.XPATH,
                                    "//*[@id=\"searchform_models_search_form_3a6b2022-609c-acca-99fc-2fa171a1c5ad_search_child_2_lap_uid34\"]/label[1]/input");
                            ActionOnInput(form_input, InputAcition.CLICK, null);
                        } else
                        {
                            form_input = GetElement(driver, Element_By.XPATH,
                                    "//*[@id=\"searchform_models_search_form_3a6b2022-609c-acca-99fc-2fa171a1c5ad_search_child_2_lap_uid34\"]/label[2]/input");
                            ActionOnInput(form_input, InputAcition.CLICK, null);
                        }
                    }
                    // [[end]]

                    Thread.Sleep(THREAD_SLEEP[(int)ThreadTimeSpan.HALF_SEC]);
                }

                //Thread.Sleep(5000);
                //GetElement(driver, Element_By.NAME, "search_form[search_child_1_lap]").Click();

                //Thread.Sleep(3000);
                //GetElement(driver, Element_By.CLASSNAME, "close").Click();
            }

            // Submit form and go Next Page
            ActionOnInput(GetElement(driver, Element_By.ID, "submit-search"), InputAcition.CLICK, null);
        }

        /// <summary>
        /// Proceeds with the automation to next page.
        /// </summary>
        /// <param name="driver">The current drive use in the this section.</param>
        static void Verify(web_automation.WebDriver driver)
        {
            Actions actions;        
            IWebElement nextPage;

            driver.Wait(DRIVER_WAIT[(int)DriverTimeSpan.THREE_SEC]);
            nextPage = GetElement(driver, Element_By.XPATH, "//*[@id=\"flights\"]/div[6]/div[3]/button");

            // Scroll Down
            actions = new Actions(driver.CurrentDriver);
            actions.MoveToElement(nextPage);
            actions.Perform();

            Thread.Sleep(THREAD_SLEEP[(int)ThreadTimeSpan.ONE_HALF_SEC]);

            // Next Page
            ActionOnInput(nextPage, InputAcition.CLICK, null);
        }

        /// <summary>
        /// Proceeds with the automation to next page.
        /// </summary>
        /// <param name="driver">The current drive use in the this section.</param>
        static void bundle(web_automation.WebDriver driver)
        {
            driver.Wait(DRIVER_WAIT[(int)DriverTimeSpan.THREE_SEC]);
            string[] bundle = { "//*[@id=\"package\"]/fieldset/div/div[3]/div/div[2]/div[2]/button",
                                "//*[@id=\"package\"]/fieldset/div/div[5]/div/div[2]/div[2]/button" };

            // Get Bundle
            GetElement(driver, Element_By.XPATH, bundle[0]).Click();

            Thread.Sleep(1500);
            // Next Page
            ActionOnInput(GetElement(driver, Element_By.CLASSNAME, "continue"), InputAcition.CLICK, null);
        }

        /// <summary>
        /// Proceeds with the automation to next page.
        /// </summary>
        /// <param name="driver">The current drive use in the this section.</param>
        private static void CarRental(web_automation.WebDriver driver)
        {
            IWebElement table;
            IWebElement nextPage;
            Actions actions;
            IReadOnlyCollection<IWebElement> links;

            // User Data
            string[] carAgencies = {"Alamo", "Enterprise", "National"};
            string[] carTypes = {"Economy 2/4 Door", "Compact 2/4 Doorr", "Intermediate 2/4 Door", "Standard 2/4 Door", "Fullsize 2/4 Door",
                                "Premium 2/4 Door"};

            Thread.Sleep(THREAD_SLEEP[(int)ThreadTimeSpan.ONE_HALF_SEC]);

            table = driver.FindElement("//*[@id=\"vendors\"]/div[2]/table", Element_By.XPATH);

            try
            {
                links = table.FindElements(By.TagName("a"));
                foreach (IWebElement elem in links)
                {
                    // Check for specific agency
                    try
                    {
                        Thread.Sleep(THREAD_SLEEP[(int)ThreadTimeSpan.ONE_HALF_SEC]);

                        if (elem.GetAttribute("aria-label").Contains(carAgencies[0]))
                        {
                            ActionOnInput(elem, InputAcition.CLICK, null);
                            break;
                        }
                    }
                    catch (ArgumentNullException argNull_Ex)
                    {
                        Console.WriteLine(argNull_Ex);
                    }
                }
            } catch (NoSuchElementException nse_Ex)
            {
                Console.WriteLine(nse_Ex.Message);
            }


            Thread.Sleep(THREAD_SLEEP[(int)ThreadTimeSpan.ONE_SEC]);

            nextPage = GetElement(driver, Element_By.CLASSNAME, "continue");

            if (nextPage != null)
            {
                // Scroll Down
                actions = new Actions(driver.CurrentDriver);
                actions.MoveToElement(nextPage);
                actions.Perform();
            }

            Thread.Sleep(THREAD_SLEEP[(int)ThreadTimeSpan.ONE_HALF_SEC]);

            // Next Page
            ActionOnInput(nextPage, InputAcition.CLICK, null);
        }

        static void CheckTotal(web_automation.WebDriver driver)
        {
            IWebElement td;
            decimal flight_car_price;
            decimal roundtrip_discount;
            decimal taxes_carriers_fees_prices;
            decimal allegiant_bonus;
            decimal total_USD;
            decimal calculatedBalance = 0;

            driver.Wait(DRIVER_WAIT[(int)DriverTimeSpan.THREE_SEC]);
            Thread.Sleep(THREAD_SLEEP[(int)ThreadTimeSpan.ONE_HALF_SEC]);

            td = driver.FindElement("//*[@id=\"pricing\"]/div/table/tbody[1]/tr/td", Element_By.XPATH);
            Decimal.TryParse(td.Text.Substring(1), out flight_car_price);
            calculatedBalance += flight_car_price;

            td = driver.FindElement("//*[@id=\"pricing\"]/div/table/tbody[2]/tr/td", Element_By.XPATH);
            Decimal.TryParse(td.Text.Substring(2), out roundtrip_discount);
            calculatedBalance -= roundtrip_discount;

            td = driver.FindElement("//*[@id=\"pricing\"]/div/table/tbody[3]/tr/td", Element_By.XPATH);
            Decimal.TryParse(td.Text.Substring(1), out taxes_carriers_fees_prices);
            calculatedBalance += taxes_carriers_fees_prices;

            td = driver.FindElement("//*[@id=\"pricing\"]/div/table/tbody[4]/tr/td", Element_By.XPATH);
            Decimal.TryParse(td.Text.Substring(1), out allegiant_bonus);
            calculatedBalance += allegiant_bonus;

            td = driver.FindElement("//*[@id=\"pricing\"]/div/table/tbody[7]/tr/td", Element_By.XPATH);
            Decimal.TryParse(td.Text.Substring(1), out total_USD);

            // Console.WriteLine($"flight_car_price {flight_car_price}, roundtrip_discount {roundtrip_discount}, taxes_carriers_fees_prices {taxes_carriers_fees_prices}," +
            //            $"allegiant_bonus {allegiant_bonus}, total_USD {total_USD}");

            // VALIDATION
            Assert.AreEqual(calculatedBalance, total_USD);
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
        /// /// <param name="isReturnDate">Bool flag to ignore an extract click in the return input.</param>
        private static void NextAvailablesDate(web_automation.WebDriver driver, string attribute_val, string flightType, uint skip_days, bool isReturnDate)
        {
            bool foundAvailableDate = false;            // flag for available date
            string attr;                                // value of a attribute
            IWebElement table_body = null;
            IWebElement table = null;
            IWebElement form_input = null;               
            IReadOnlyCollection<IWebElement> tds = null;    // all the cell in the table body


            // Click
            form_input = GetElement(driver, Element_By.NAME, attribute_val);
            ActionOnInput(form_input, InputAcition.CLICK, null);

            Thread.Sleep(THREAD_SLEEP[(int)ThreadTimeSpan.ONE_SEC]);

            if (!isReturnDate)
            {
                // Click
                ActionOnInput(form_input, InputAcition.CLICK, null);
            }

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
                                Thread.Sleep(THREAD_SLEEP[(int)ThreadTimeSpan.ONE_SEC]);

                                ActionOnInput(entry, InputAcition.CLICK, null);
                                foundAvailableDate = true;
                                break;
                            } else
                            {
                                // is (Unsigned int). Only decrement while is not 0.
                                --skip_days;
                            }
                        }
                    }

                    // If no available date found in the month. Proceed to next month
                    if (!foundAvailableDate)
                    {
                        // CLICK (Next Month)
                        form_input = GetElement(driver, Element_By.CLASSNAME, "ui-datepicker-next");
                        ActionOnInput(form_input, InputAcition.CLICK, null);
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
