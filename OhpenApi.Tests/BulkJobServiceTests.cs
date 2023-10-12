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
    [TestFixture]
    public class BulkJobServiceTests: BaseJobServiceTests
    {
        private BulkJobService bulkJobService;
        [SetUp]
        public void SetUp()
        {
            this.bulkJobService = new BulkJobService();

        }
        //Taking away any additions to in memory storage after a test is complete
        [TearDown]
        public void TearDown()
        {
            this.bulkJobService = null;

        }
        /// <summary>
        /// Test validates that the Process Customer Transactions method returns a response with an empty description 
        /// if a valid jobModel is submitted
        /// </summary>
        [Test]
        public void ProcessCustomerTransactionsBulkJobCompletesWithNoErrors()
        {
            // Arrange
            this.bulkJobService = new BulkJobService();

            var customerTransactions = new List<CustomerTransactionModel> { new CustomerTransactionModel("012345", "a123456", "b123456", 0), new CustomerTransactionModel("123456", "a123456", "b123456", 0) };

            var validJobModel = new JobModel("bulk", customerTransactions);

            // Act
            var response = this.bulkJobService.ProcessCustomerTransactions(this.testJobId, validJobModel);

            // message will be empty in the no errors case
            var message = response.Value.message;

            // Assert
            Assert.IsEmpty(message);

        }

        /// <summary>
        /// This method validates that the batch job service will return a message in the 
        /// Start Job response detailing that errors where found in the records
        /// </summary>
        [Test]
        public void ProcessCustomerTransactionsBulkCompletesWithErrors()
        {
            // Arrange
            this.bulkJobService = new BulkJobService();

            // Transactions with errors
            var customerTransactions = new List<CustomerTransactionModel> { new CustomerTransactionModel("--", "a12346", "123456", -2), new CustomerTransactionModel("123456", "a123456", "b123456", 0) };

            var validJobModel = new JobModel("bulk", customerTransactions);

            // Act
            var response = this.bulkJobService.ProcessCustomerTransactions(this.testJobId, validJobModel);

            // message will be empty in the no errors case
            var message = response.Value.message;

            var jobLogs = this.baseJobService.GetLogResponse(testJobId);

            // Assert
            Assert.That(message, Is.EquivalentTo("Some records contain errors, check the job log. Only bulk jobs will have completed"));
        }
    }
}
