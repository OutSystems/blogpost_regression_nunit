using System;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using NUnit.Framework;

namespace Cases_Regression
{
    class BDDlib
    {
        string targetEnv = "";

        public BDDlib(string targetEnvironment)
        {
            targetEnv = targetEnvironment;
            if (!targetEnv.EndsWith("/"))
                targetEnv = targetEnv + "/";
        }

        public void AssertTestSuite(string EspaceName, string suiteName)
        {
            // GetRequest performs a simple HTTP get to the BDD Framework REST API
            WebRequest request = WebRequest.Create(targetEnv + EspaceName + "/" + suiteName);
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
            string result = reader.ReadToEnd();

            // the returned output is in JSON format so we can convert it into a BDDTestSuiteResult object
            JavaScriptSerializer deserializedResponse = new JavaScriptSerializer();
            BDDTestSuiteResult testResult = (BDDTestSuiteResult)deserializedResponse.Deserialize(result, typeof(BDDTestSuiteResult));

            // Perform the NUnit Assert
            Assert.IsTrue(testResult.SuiteSuccess, GetOutputMessage(testResult));
        }

        private class BDDTestSuiteResult
        {
            public bool SuiteSuccess { get; set; }
            public int SuccessfulScenarios { get; set; }
            public int FailedScenarios { get; set; }
            public string ErrorMessage { get; set; }
            public List<string> FailureReports { get; set; }
        }

        // Gets the BDD Test failure reports to be outputted by NUnit when the test fails (in text format)
        private static string GetOutputMessage(BDDTestSuiteResult testResult)
        {
            if (testResult.ErrorMessage != "")
            {
                return "Failed to obtain BDD Test Suite. " + testResult.ErrorMessage;
            }
            else
            {
                string finalReport = "";
                foreach (string report in testResult.FailureReports)
                {
                    finalReport += report;
                }

                return "BDD Test Suite failed " + testResult.FailedScenarios + " scenarios (in " +
                    (testResult.SuccessfulScenarios + testResult.FailedScenarios) + ") \n" + finalReport;
            }
        }
    }
}
