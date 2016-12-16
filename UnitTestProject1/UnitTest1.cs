using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using WebPage.Areas.SaleManage.Controllers;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            CustomerController a = new CustomerController();
            var d = a.Index();
            int [] arr = {3,4,1,6,7,2 };
            IEnumerator en = arr.GetEnumerator();
            //en.Reset();
            while (en.MoveNext())
            {
                Console.WriteLine( en.Current);
            }
        }
    }
    
   
}
