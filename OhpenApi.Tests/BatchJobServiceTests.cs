using OhpenApi.Entities.Models;
using OhpenApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhpenApi.Tests
{
    /// <summary>
    /// Checking Error flow in the Batch Job Service class
    /// </summary>
    [TestFixture]
    public class BatchJobServiceTests : BaseJobServiceTests
    {
        private BatchJobService batchJobService;
        [SetUp]
        public void SetUp()
        {
            this.batchJobService = new BatchJobService();

        }

        [TearDown]
        public void TearDown()
        {
            this.batchJobService = null;

        }

        /// <summary>
        /// Test validates that the Process Customer Transactions method returns a response with an empty description 
        /// if a valid jobModel is submitted
        /// </summary>
        [Test]
        public void ProcessCustomerTransactionsBatchJobCompletesWithNoErrors()
        {
            // Arrange
            this.batchJobService = new BatchJobService();

            var customerTransactions = new List<CustomerTransactionModel> { new CustomerTransactionModel("012345", "a123456", "b123456", 0), new CustomerTransactionModel("123456", "a123456", "b123456", 0) };

            var validJobModel = new JobModel("bulk", customerTransactions);

            // Act
            var response = this.batchJobService.ProcessCustomerTransactions(this.testJobId, validJobModel);

            // message will be empty in the no errors case
            var message = response.Value.message;

            // Assert
            Assert.IsEmpty(message);

        }

        /// <summary>
        /// This method validates that the batch job service will not complete processing the data set
        /// and will also return a message in the Start Job response detailing that errors where found in the records
        /// </summary>
        [Test]
        public void ProcessCustomerTransactionsBatchWithErrors()
        {

            // Arrange 
            this.batchJobService = new BatchJobService();

            // Transactions with errors
            var customerTransactions = new List<CustomerTransactionModel> { new CustomerTransactionModel("--", "a12346", "123456", -2), new CustomerTransactionModel("123456", "a123456", "b123456", 0) };

            var validJobModel = new JobModel("batch", customerTransactions);

            // Act 
            var response = this.batchJobService.ProcessCustomerTransactions(this.testJobId, validJobModel);

            var itemLogsCount = jobLogResponses[testJobId].Items.Count;

            // message will be empty in the no errors case
            var message = response.Value.message;

            // Assert 
            Assert.That(itemLogsCount, Is.EqualTo(1));

            Assert.That(message, Is.EquivalentTo("Some records contain errors, check the job log. Only bulk jobs will have completed"));
        }

    }
}
