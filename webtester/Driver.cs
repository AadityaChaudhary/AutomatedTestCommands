using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

public class Driver
{
    
    IWebDriver driver;
   

    public Driver()
	{
        ChromeOptions options = new ChromeOptions();
        //options.AddArguments("headless");
        driver = new ChromeDriver("C:\\Users\\intern2.INOVEX-HQ\\Documents", options);
    }

    public void access(String id)
    {
        IWebElement e;
        Console.WriteLine("accessing >>>" + id);
        if (id.StartsWith("link="))
        {
            Console.WriteLine(id.Substring(5));
            e = driver.FindElement(By.LinkText(id.Substring(5)));
            Console.WriteLine(id.Substring(5));

        }
        else if (id.StartsWith("id="))
        {
            Console.WriteLine(id.Substring(3));
            e = driver.FindElement(By.Id(id.Substring(3)));
            


        }
        else if (id.StartsWith("xpath="))
        {
            Console.WriteLine(id.Substring(6));
            e = driver.FindElement(By.XPath(id.Substring(6)));


        }
        else
        {
            Console.WriteLine("failed to type into.." + id);
            return;
        }
        return;

    }

    public void openLink(string link)
    {
        driver.Url = link;
    }

    public string click(string id)
    {
        By searchBy;
        if (id.StartsWith("link=")) { searchBy = By.LinkText(id.Substring(5)); }
        else if (id.StartsWith("id=")) { searchBy = By.Id(id.Substring(3)); }
        else if (id.StartsWith("xpath=")) { searchBy = By.XPath(id.Substring(6)); Console.WriteLine(id.Substring(6)); }
        else { return "FAILED invalid id"; }

        try
        {
            IWebElement e = driver.FindElement(searchBy);

            e.Click();
            return "PASS - click";
        }
        catch (NoSuchElementException e)
        {
            return "FAILED - ELEMENT NOT VISIBLE";
        }

        catch (ElementNotInteractableException e)
        {
            return "FAILED - ELEMENT NOT INTERACTABLE";
        }

    }

    public string type(String id, String msg)
    {
        By searchBy;
        if (id.StartsWith("link=")) searchBy = By.LinkText(id.Substring(5));
        else if (id.StartsWith("id=")) searchBy = By.Id(id.Substring(3));
        else if (id.StartsWith("xpath=")) searchBy = By.XPath(id.Substring(6));
        else return "FAILED invalid id";
    
        try
        {
            IWebElement e = driver.FindElement(searchBy);

            e.SendKeys(msg);
            return "PASS - type";
        }
        catch(NoSuchElementException e) 
        {
            return "FAILED - ELEMENT NOT VISIBLE";
        }
    }

    public void closeBrowser()
    {
        driver.Close();
    }

    public Boolean checkEntry(String id, String msg)
    {
        IWebElement e;
        Console.WriteLine("checking >>>" + id);
        if (id.Contains("link="))
        {
            Console.WriteLine(id.Substring(5));
             e = driver.FindElement(By.LinkText(id.Substring(5)));

            
        }
        else if (id.Contains("id="))
        {
            Console.WriteLine(id.Substring(3));
             e = driver.FindElement(By.Id(id.Substring(3)));

            
        }
        else if (id.Contains("xpath="))
        {
            Console.WriteLine(id.Substring(6));
             e = driver.FindElement(By.XPath(id.Substring(6)));

            
        }
        else
        {
            Console.WriteLine("failed to type into.." + id);
            return false;
        }
        return e.GetAttribute("value").Equals(msg);
        
    }

    public bool isElementInteractable(string id)
    {
        By searchBy;
        if (id.Contains("link=")) searchBy = By.LinkText(id.Substring(5));
        else if (id.Contains("id=")) searchBy = By.Id(id.Substring(3));
        else if (id.Contains("xpath=")) searchBy = By.XPath(id.Substring(6));
        else return false;

        IWebElement e = driver.FindElement(searchBy);
        return e.Enabled;
        return driver.FindElements(searchBy).Count > 0;
    }

    public void waitForElement(String id, int timeout)
    {
        bool checkTime = true;
        if (timeout == -1)
        {
            checkTime = false;
        }

        int x = 0;
        Console.WriteLine("waiting... " + x + " " + id);
        while (!doesElementExist(id))
        {
            
            Thread.Sleep(TimeSpan.FromMilliseconds(5));
            x++;

            if (checkTime && x > timeout)
            {
                return;
            }

        }


    }

    public bool doesElementExist(String id)
    {
        By searchBy;
        if (id.Contains("link=")) searchBy = By.LinkText(id.Substring(5));
        else if (id.Contains("id=")) searchBy = By.Id(id.Substring(3));
        else if (id.Contains("xpath=")) searchBy = By.XPath(id.Substring(6));
        else return false;

        if(driver.FindElements(searchBy).Count > 0)
        {
           return driver.FindElement(searchBy).Displayed && driver.FindElement(searchBy).Enabled;
        }
        else
        {
            return false;
        }
 

    }




}
