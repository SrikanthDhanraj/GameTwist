using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;
using System.Configuration;
using GameTwistSmoke.Common_Utilities;
using GameTwistSmoke.Application_Pages;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using OpenQA.Selenium.Interactions;


namespace GameTwistSmoke
{
    
    public class SmokeTest
    {
        IWebDriver driver;
        String Browsertype, EnvironmentURL, UserName, Password, ToBeSelectedGameRow, SlotSearchGamesCount, SearchString;
        int InternalWaitperiod,ExternalWaitPeriod;
        CommonFunctions commonfunctions;
        HomePage homepage;
        WebDriverWait InternalWait;


        /*
         * Created By : Srikanth D
         * Description: To Initialize the  Set up
         * Date Modified:
         * Modification Details : 
         */
        [SetUp]
        public void TestInitialize()
        {
            //Initializing the variables for Config file
            Browsertype = ConfigurationManager.AppSettings["BrowserType"];
            EnvironmentURL = ConfigurationManager.AppSettings["EnvironmentURL"];
            InternalWaitperiod = Convert.ToInt32( ConfigurationManager.AppSettings["internalWait"]);
            ExternalWaitPeriod = Convert.ToInt32(ConfigurationManager.AppSettings["explicitwait"]);
            SearchString = ConfigurationManager.AppSettings["SearchString"];
            UserName = ConfigurationManager.AppSettings["UserName"];
            Password = ConfigurationManager.AppSettings["Password"];
            ToBeSelectedGameRow = ConfigurationManager.AppSettings["ToBeSelectedGamerow"];
            SlotSearchGamesCount = ConfigurationManager.AppSettings["SlotSearchGamesCount"];

            commonfunctions = new CommonFunctions();
            driver = commonfunctions.LaunchApplication(Browsertype, EnvironmentURL);

            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(ExternalWaitPeriod));
            InternalWait = new WebDriverWait(driver, TimeSpan.FromSeconds(InternalWaitperiod));
            
            //Initializing the Home Page class with Driver            
            homepage = new HomePage(driver);

            //Sign In
            homepage.Login(UserName, Password);

            //Closing the External popups
            commonfunctions.CloseExternalPopups(driver);

            //Changing the language to English
            homepage.ChangeLanguage(HomePage.ApplicationLanguage.English, Browsertype);

        }

        /*
         * Created By : Srikanth D
         * Description: To Perform the Task specified
         * Date Modified:
         * Modification Details : 
         */
        [Test]
        public void TestSmokeGameTwist()
        {
            //Navigate and Check the Navigation Bar
            commonfunctions.ClickElement(driver, homepage.lnk_Slots);
            Assert.AreEqual("Slots", homepage.GetNavigationText());

            commonfunctions.ClickElement(driver, homepage.lnk_Bingo);
            Assert.AreEqual("Bingo", homepage.GetNavigationText());

            commonfunctions.ClickElement(driver, homepage.lnk_Casino);
            Assert.AreEqual("Casino", homepage.GetNavigationText());

            commonfunctions.ClickElement(driver, homepage.lnk_Poker);
            Assert.AreEqual("Poker", homepage.GetNavigationText());

            //Perform Search
            homepage.SearchGames(SearchString);

            InternalWait.Until(ExpectedConditions.ElementExists(By.XPath(homepage.lst_ResultGames)));

            //Validating the Results Count
            String ActualResultCount = homepage.GetSearchedGamesResultCount();
            Assert.AreEqual(SlotSearchGamesCount, ActualResultCount);

            //Selecting the particular Game as specified in Config file
            String TobeSelectedGame = homepage.lst_ResultGames.ToString().Replace("li[1]", "li[" + ToBeSelectedGameRow + "]");

            String SlotName = driver.FindElement(By.XPath(TobeSelectedGame)).GetAttribute("data-name");
            driver.FindElement(By.XPath(TobeSelectedGame)).Click();

            //Validating the Navigation Bar as per the seelcted game
            InternalWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(homepage.txt_NavigatorBar));
            Assert.AreEqual(SlotName, homepage.GetNavigationText());

            //Changing the Languague
            homepage.ChangeLanguage(HomePage.ApplicationLanguage.Deutsch, Browsertype);
            
            

        }

        /*
         * Created By : Srikanth D
         * Description: To perform Clean up Activities
         * Date Modified:
         * Modification Details : 
         */

        [TearDown]
        public void CloseTest()
        {
            homepage.Signout(Browsertype);
            driver.Quit();
        }

    }
}
