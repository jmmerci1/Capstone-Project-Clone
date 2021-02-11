using NUnit.Framework;
using DirectorsPortalWPF.ValidateWebsite;

/// <summary>
/// 
/// 3 A's of Unit Testing:
/// 
///     Arrange: 
///     Arrange steps should setup the test case. Does the
///     test require any objects or special settings? Does it need to prep a database?
///     Does it need to log into a web app? Handle all of these operations at the start of the test.
///     
///     Act:
///     Act steps should cover the main thingto be tested. This could be calling a function or
///     method, calling a REST API, or interacting with a web page. Keep actions focused on the target
///     behavior.
///     
///     Assert:
///     Assert steps verify the goodness or badness of the Act response. Sometimes, assertions are as simple as checking
///     numeric or string values. Other times, they may require checking multiple facets of a system. Assertions
///     will ultimately determine if the test passes or fails. 
///         
/// 
///     See NUnit documentation to learn more:
///     https://docs.nunit.org/articles/nunit/intro.html
///     
///     Microsoft Documentation on NUnit:
///     https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-nunit
///     
/// </summary>
namespace DirectorsPortal_Unit_Tests
{
    [TestFixture]
    public class ValidateWebsiteTests
    {
        
        [SetUp]
        public void Setup()
        {
            //This code always runs before tests.
            //Anything that needs to be setup before a test should be done here. (i.e. instantiating objects, assigning variables, etc.)
            
        }

        [TearDown]
        public void Teardown()
        {
            //This code runs after tests.
            //Use this to perform any actions you need after a test.
        }

        /// <summary>
        /// Tests typically begin with the method name being tested, followed by a Explanation of what the
        /// test is testing for. Method name and explanation are usually separated, in this case with an underscore.
        /// </summary>
        [Test]
        public void GetTemplateLocation_RetrieveTheHTMLTemplateFilePath()
        {
            //Arrange
            HtmlPreviewGenerator c = new HtmlPreviewGenerator();

            //Act
            string filePath = c.GetTemplateLocation();

            //Assert
            Assert.AreEqual("C:\\Users\\nzwe45\\source\\repos\\DirectorPortal\\DirectorsPortal_Unit_Tests" +
                "\\bin\\Resources\\MembershipTemplate.html", filePath);
        }


    }
}