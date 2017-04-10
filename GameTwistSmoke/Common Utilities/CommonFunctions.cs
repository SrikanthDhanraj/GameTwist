using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Configuration;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;

namespace GameTwistSmoke.Common_Utilities
{
    class CommonFunctions
    {
        IWebDriver driver;
        WebDriverWait InternalWait;
        int InternalWaitperiod = Convert.ToInt32( ConfigurationManager.AppSettings["internalWait"]);
        String ChromeDriverlocation = ConfigurationManager.AppSettings["ChromeDriverLocation"];

        /*
         * Created By : Srikanth D
         * Description: To Launch the Browser and Navigate to the URL provided
         * Date Modified:
         * Modification Details : 
         */
        public IWebDriver LaunchApplication(String BrowserType, String ApplicationURL)
        {
            if(BrowserType.Trim().ToLower() == "firefox")
            {
                driver = new FirefoxDriver(new FirefoxBinary(), new FirefoxProfile(), TimeSpan.FromSeconds(180));
                driver.Url = ApplicationURL;                
            }

            if (BrowserType.Trim().ToLower() == "chrome")
            {
                var options = new ChromeOptions();
                options.AddArgument("start-maximized");
                driver = new ChromeDriver(ChromeDriverlocation, options);                
                driver.Url = ApplicationURL;                
            }

            return driver;           
        }
        /*
         * Created By : Srikanth D
         * Description: To Click on the Web Element
         * Date Modified:
         * Modification Details :  
         */

        public void ClickElement(IWebDriver driver, By Linklocator) 
        {
            InternalWait = new WebDriverWait(driver, TimeSpan.FromSeconds(InternalWaitperiod));
            InternalWait.Until(ExpectedConditions.ElementExists(Linklocator));
            InternalWait.Until(ExpectedConditions.ElementIsVisible(Linklocator));
            //InternalWait.Until(ExpectedConditions.StalenessOf(driver.FindElement(Linklocator)));
            InternalWait.Until(ExpectedConditions.ElementToBeClickable(Linklocator));
            driver.FindElement(Linklocator).Click();
        }

        /*
         * Created By : Srikanth D
         * Description: To Close the External Popups
         * Date Modified:
         * Modification Details : 
         */

        public void CloseExternalPopups(IWebDriver driver)
        {
            String ActualWindowHandle = driver.CurrentWindowHandle;

             //Switch to each webdriver browser window and closing if the Title does not contains gametwist text
            foreach(String Currenthandle in driver.WindowHandles)
            {
                driver.SwitchTo().Window(Currenthandle);

                if(driver.Title.ToLower().Contains("gametwist"))
                {
                   
                    ActualWindowHandle = Currenthandle;
                    continue;
                }
                driver.Close();
            }
            driver.SwitchTo().Window(ActualWindowHandle);           

        }
        
    }
}
