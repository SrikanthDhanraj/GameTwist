using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Configuration;
using System.Threading;
using OpenQA.Selenium.Interactions;

namespace GameTwistSmoke.Application_Pages
{
    class HomePage
    {
        //Object Repository of Home Page
        public By txtbox_UserName = By.Name("login-nickname");
        public By txtbox_Password = By.Id("login-password");
        public By btn_Login = By.XPath("//span[contains(text(), 'LOG IN')]");
        public By btn_CookiesOK = By.XPath("//button[contains(text(), 'OK')]");
        public By lnk_InternalPopupClose = By.Id("wof_close_x");
        public By lnk_Slots = By.XPath("//nav[@id = 'navigation-main']//li[1]");
        public By lnk_Bingo = By.XPath("//nav[@id = 'navigation-main']//li[2]");
        public By lnk_Casino = By.XPath("//nav[@id = 'navigation-main']//li[3]");
        public By lnk_Poker = By.XPath("//nav[@id = 'navigation-main']//li[4]");
        public By txt_NavigatorBar = By.XPath("//ol[contains(@class,'nav breadcrumb')]//li[last()]");
        public By btn_AuthenticatedLanguage = By.XPath("//div[@class ='branding__content float--right authenticated']//span[@class = 'select-language__button']");        
        public By txtbox_SearchGames = By.Id("ctl00_cphNavAndSearch_ctl01_gameSearch");
        public By txt_RawGamescount = By.XPath("//li[contains(@class , 'game-search__results-sum grid__item one-whole results--sum')]");
        public String lst_ResultGames = "//ul[contains(@class, 'game-search__list grid flyout__content js-game-search-list')]//li[1]";
        public By btn_UserOptions = By.XPath("//div[contains(@class , 'branding__content float--right authenticated')]//span[@class = 'nickname']");
        public By btn_signout = By.XPath("//div[contains(@class , 'branding__content float--right authenticated')]//div[@class = 'flyout my-gt-menu__flyout']//li[last()]");

        //Language Selection Enum
        public enum ApplicationLanguage {English, Deutsch, Español, Français };

        int InternalWaitperiod;
        WebDriverWait InternalWait;        
        IWebDriver driver;
        Common_Utilities.CommonFunctions commonfunctions;

        /*
         * Created By : Srikanth D
         * Description: To Initialize the Home Page variables
         * Date Modified:
         * Modification Details : 
         */
        public HomePage(IWebDriver driver)
        {
            this.driver = driver;
            InternalWaitperiod = Convert.ToInt32(ConfigurationManager.AppSettings["internalWait"]);
            InternalWait = new WebDriverWait(driver, TimeSpan.FromSeconds(InternalWaitperiod));
            commonfunctions = new Common_Utilities.CommonFunctions();
        }

        /*
         * Created By : Srikanth D
         * Description: To Get the Navigation Text
         * Date Modified:
         * Modification Details : 
         */
        public String GetNavigationText()
        {
            
            InternalWait.Until(ExpectedConditions.ElementExists(txt_NavigatorBar));
            return driver.FindElement(txt_NavigatorBar).Text.Trim();

        }

        /*
         * Created By : Srikanth D
         * Description: To Perform Signin
         * Date Modified:
         * Modification Details : 
         */
        public void Login(String UserName, String Password)
        {
            //Click on Cookies OK button if available
            if (driver.FindElements(btn_CookiesOK).Count > 0)
            {
                InternalWait.Until(ExpectedConditions.ElementToBeClickable(btn_CookiesOK));
                driver.FindElement(btn_CookiesOK).Click();
            }
                

            driver.FindElement(txtbox_UserName).SendKeys(UserName);
            driver.FindElement(txtbox_Password).SendKeys(Password);

            driver.FindElement(btn_Login).Click();

            //To Close Internal Pop up
            if (driver.FindElements(lnk_InternalPopupClose).Count > 0)
            {
                InternalWait.Until(ExpectedConditions.ElementToBeClickable(lnk_InternalPopupClose));
                driver.FindElement(lnk_InternalPopupClose).Click();
            }                       
                
        }

        /*
         * Created By : Srikanth D
         * Description: To get the total number of Games count
         * Date Modified:
         * Modification Details : 
         */
        public String GetSearchedGamesResultCount()
        {
            String ResultText = driver.FindElement(txt_RawGamescount).Text;
            String[] ResultArray = ResultText.Trim().Split('/');
            ResultText = ResultArray[ResultArray.Length - 1].Trim();           
            return ResultText;
        }


        /*
         * Created By : Srikanth D
         * Description: To Perform Game Search in Home Page
         * Date Modified:
         * Modification Details : 
         */
        public void SearchGames(String SearchText)
        {
            driver.FindElement(txtbox_SearchGames).SendKeys(SearchText);
            Thread.Sleep(3000);
        }

        /*
         * Created By : Srikanth D
         * Description: To Change the Language in the application
         * Date Modified:
         * Modification Details : 
         */
        public void ChangeLanguage(ApplicationLanguage Language, String BrowserType)
        {

            //Due to issues in the Web Driver Move to Element is not working consistently
            //driver.FindElement(btn_AuthenticatedLanguage).Click();
            //Actions act = new Actions(driver);
            //act.MoveToElement(driver.FindElement(btn_AuthenticatedLanguage)).ContextClick().Build().Perform();

            Actions act = new Actions(driver);

            InternalWait.Until(ExpectedConditions.ElementExists(txtbox_SearchGames));
            InternalWait.Until(ExpectedConditions.ElementIsVisible(txtbox_SearchGames));
            //Work Arrounds to hover over Language selection button. It is different for firefox and Chrome
            if (BrowserType.Trim().ToLower() == "firefox")
            {
                try
                {
                    commonfunctions.ClickElement(driver, btn_AuthenticatedLanguage);
                }
                
                catch(Exception Ex)
                {

                }
                
            }


            if (BrowserType.Trim().ToLower() == "chrome")
            {
                InternalWait.Until(ExpectedConditions.ElementExists(btn_AuthenticatedLanguage));
                InternalWait.Until(ExpectedConditions.ElementIsVisible(btn_AuthenticatedLanguage));
                
                act.MoveToElement(driver.FindElement(btn_AuthenticatedLanguage)).ContextClick(driver.FindElement(btn_AuthenticatedLanguage)).Build().Perform();
                
            }



            if (Language == ApplicationLanguage.English)
                driver.FindElement(By.LinkText("English")).Click();

            else if (Language == ApplicationLanguage.Deutsch)
                driver.FindElement(By.LinkText("Deutsch")).Click();

            else if (Language == ApplicationLanguage.Español)
                driver.FindElement(By.LinkText("Español")).Click();            

            else if (Language == ApplicationLanguage.Français)
                driver.FindElement(By.LinkText("Français")).Click();

            InternalWait.Until(ExpectedConditions.ElementExists(btn_UserOptions));
            //InternalWait.Until(ExpectedConditions.ElementToBeClickable(btn_UserOptions));

        }

        /*
         * Created By : Srikanth D
         * Description: To Perform Signout
         * Date Modified:
         * Modification Details : 
         */
        public void Signout(String BrowserType)
        {
            //Due to issues in the Web Driver Move to Element is not working consistently

            InternalWait.Until(ExpectedConditions.ElementExists(txtbox_SearchGames));
            InternalWait.Until(ExpectedConditions.ElementIsVisible(txtbox_SearchGames));

            //Work Arrounds to hover over user options selection button. It is different for firefox and Chrome
            if (BrowserType.Trim().ToLower() == "firefox")
            {
                commonfunctions.ClickElement(driver, btn_UserOptions);                
            }

            if (BrowserType.Trim().ToLower() == "chrome")
            {
                
                InternalWait.Until(ExpectedConditions.ElementExists(btn_UserOptions));
                InternalWait.Until(ExpectedConditions.StalenessOf(driver.FindElement(btn_UserOptions)));
                InternalWait.Until(ExpectedConditions.ElementIsVisible(btn_UserOptions));
                InternalWait.Until(ExpectedConditions.ElementToBeClickable(btn_UserOptions));

                Actions act = new Actions(driver);
                act.MoveToElement(driver.FindElement(btn_UserOptions)).ContextClick(driver.FindElement(btn_UserOptions)).Build().Perform();
                
            }

            commonfunctions.ClickElement(driver, btn_signout);
            
        }

    }
}
 