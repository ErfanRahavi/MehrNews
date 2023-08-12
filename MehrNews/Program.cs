using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MehrNews
{
    public class Program
    {

        static void Main(string[] args)
        {
            
            IWebDriver driver = new ChromeDriver("D:\\chromedriver");
            driver.Navigate().GoToUrl("https://www.mehrnews.com/");
            var report = driver.FindElements(By.ClassName("news"));
            bool sElement = true;
            while (sElement)
            {

                try
                {

                    for (int i = 0; i < report.Count; i++)
                    {
                        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
                        report[i].Click();
                        driver.SwitchTo().Window(driver.WindowHandles[1]);

                        var atcBody = driver.FindElement(By.ClassName("item-text"));

                        var title = driver.FindElement(By.ClassName("title"));

                        var id = driver.FindElement(By.ClassName("item-code"));

                        var date = driver.FindElement(By.XPath("/html/body/main/div/div/div/div/div[1]/article/div[2]/div[1]/div[2]/span"));
                        
                        var body = atcBody.Text;

                        var header = title.Text;
                        
                        var name = id.Text + date.Text;
                        name = new string(name.Where(c => !char.IsPunctuation(c)).ToArray());
                        var directory = Environment.CurrentDirectory;
                        var filePath = Path.GetFullPath(directory);
                        
                        string fileName = $"name{name}.txt";
                        string fullPath = Path.Combine(filePath, fileName);
                        File.WriteAllText(fullPath, header + body);

                        driver.Close();
                        driver.SwitchTo().Window(driver.WindowHandles[0]);
                    }

                    sElement = false;
                }
                catch (StaleElementReferenceException e)
                {
                   sElement = true;
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    driver.Quit();
                    break;
                }

            }
        }    
    }
}
