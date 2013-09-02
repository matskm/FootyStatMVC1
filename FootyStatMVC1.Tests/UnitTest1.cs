using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using WatiN.Core;

namespace FootyStatMVC1.Tests
{
    [TestFixture]
    [RequiresSTA]
    public class UnitTest1
    {
        [Test]
        public void TestMethod1()
        {
            //var view = new PlayerStatController.
            // Open a new Internet Explorer window and
            // goto the google website.
            IE ie = new IE("http://www.google.com");

            // Find the search text field and type Watin in it.
            ie.TextField(Find.ByName("q")).TypeText("WatiN");

            // Click the Google search button.
            ie.Button(Find.ByValue("Google Search")).Click();

            // Uncomment the following line if you want to close
            // Internet Explorer and the console window immediately.
            //ie.Close();

        }
    }
}
