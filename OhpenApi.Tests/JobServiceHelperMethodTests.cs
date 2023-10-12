using OhpenApi.Entities.Models;
using OhpenApi.Services;
using OhpenContracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhpenApi.Tests
{
    /// <summary>
    /// Test suite to check good flow and error flow of the JobServiceHelperMethods class
    /// </summary>
    [TestFixture]
    public class JobServiceHelperMethodTests
    {
        /// <summary>
        /// Checking the logic in the Add errors method to make sure that it can correctly distinguish errors
        /// No errors case
        /// </summary>
        [Test]
        public void AddErrorDescriptionShouldAddNoErrorDescriptions()
        {
            // Arrange
            var expected = new List<string>();

            var testTransaction = new CustomerTransactionModel("a1234", "a123456", "b123456", 0);

            // Act
            var result = JobServiceHelperMethods.AddErrorDescription(testTransaction);

            // Assert
            Assert.That(result, Is.EquivalentTo(expected));
        }

        /// <summary>
        /// Checking the logic in the Add errors method to make sure that it can correctly distinguish errors
        /// Errors case
        /// </summary>
        [Test]
        public void AddErrorDescriptionShouldAddCorrectErrorDescriptions()
        {
            // Arrange
            var fullErrorList = new List<string> { "Transaction ID contains invalid Character", "Sending Account ID should be of length 7", "Destination Account ID should be of length 7", "Invalid amount, amount of a transaction should be above 0" };

            var testTransaction = new CustomerTransactionModel("--", "a12346", "123456", -2);

            // Act
            var result = JobServiceHelperMethods.AddErrorDescription(testTransaction);

            // Assert
            Assert.That(result, Is.EquivalentTo(fullErrorList));
        }

        /// <summary>
        /// Checking that switch will return the correct type when jobType is bulk
        /// </summary>
        [Test]
        public void ServiceSwitchReturnsBulkJob()
        {
            //Arrange 
            string jobType = "bulk";

            // Act
            var result = JobServiceHelperMethods.JobServicesSwitch(jobType);

            // Assert 
            Assert.IsTrue(result is BulkJobService);
        }

        /// <summary>
        /// Checking that switch will return the correct type when jobType is batch
        /// </summary>
        [Test]
        public void ServiceSwitchReturnsBatchJob()
        {
            //Arrange 
            string jobType = "batch";

            // Act
            var result = JobServiceHelperMethods.JobServicesSwitch(jobType);

            // Assert 
            Assert.IsTrue(result is BatchJobService);
        }

        /// <summary>
        /// Checking that switch will return the correct type when jobType is not bulk or batch
        /// </summary>
        [Test]
        public void ServiceSwitchReturnsBaseJob()
        {
            //Arrange 
            string jobType = "string";

            // Act
            var result = JobServiceHelperMethods.JobServicesSwitch(jobType);

            // Assert 
            Assert.IsTrue(result is BaseJobService);
        }
    }
}
