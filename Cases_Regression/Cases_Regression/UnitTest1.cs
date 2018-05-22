using System;
using NUnit.Framework;

namespace Cases_Regression
{
    [TestFixture]
    public class UnitTest1
    {
        BDDlib bdd = new BDDlib("https://pmdemo2-dev.outsystemsenterprise.com/BDDFramework/rest/v1/BDDTestRunner/");

        [Test]
        public void Test1()
        {
            bdd.AssertTestSuite("Cases_UnitTests", "Case_CRUD");
        }
    }
}