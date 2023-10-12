using Microsoft.AspNetCore.Mvc;
using OhpenApi.Contracts.Responses;
using OhpenApi.Controllers;
using OhpenApi.Services;
using OhpenContracts.Services;
using OhpenApi.Contracts.Requests;
using OhpenApi.Entities.Models;

namespace OhpenApi.Tests
{
    /// <summary>
    /// Test suite to check good flow and error flow of the JobController class
    /// </summary>
    [TestFixture]
    public class JobControllerTests
    {

        private JobController? jobController;
        private IJobService jobService;


        // Setting up tests context
        public JobControllerTests()
        {
            this.jobService = new BaseJobService();
        }

        // Setting up controller before each test
        [SetUp]
        public void SetUp()
        {
            this.jobController = new JobController(this.jobService);
        }

        // Tearing down controller after each test to prevent problems that may occur form multiple tests using the same resource
        [TearDown] public void TearDown()
        {
            this.jobController = null;
        }

        /// <summary>
        /// Test is given a valid request body
        /// Test validates that the controller can successfully process the transactions
        /// </summary>
        [Test]
        public void PostTransactionsCorrectlyPostsData()
        {
            var customerTransactionModels = new List<CustomerTransactionModel> { new CustomerTransactionModel("0123456", "a123456", "b123456", 0) };
            StartJobRequest jobRequest = new StartJobRequest("bulk",customerTransactionModels);

            var result = this.jobController.PostTransactions(jobRequest);

            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());

        }

        /// <summary>
        /// Test is given a invalid request body
        /// Test validates that the controller fails to  process the transactions
        /// </summary>
        [Test]
        public void PostTransactionsInCorrectlyPostsData()
        {
            var customerTransactionModels = new List<CustomerTransactionModel> { new CustomerTransactionModel("01456", "a123456", "b123456", 0) };
            StartJobRequest invalidJobRequest = new StartJobRequest("string", customerTransactionModels);

            var result = this.jobController.PostTransactions(invalidJobRequest);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());

        }

        /// <summary>
        /// Test is given a valid JobId
        /// Test validates that the controller can get the job logs for that JobId
        /// </summary>
        [Test]
        public void GetJobLogsSuccessfullyRetrieves()
        {
            var jobId = Guid.NewGuid();
            var itemLogs = new List<ItemLog>();

            jobService.StoreResponseData(jobId, itemLogs, string.Empty);

            var result = jobController.GetJobLogs(jobId);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());

        }

        /// <summary>
        /// Test is given a invalid JobId
        /// Test validates that the controller fails to get job logs for that JobId
        /// </summary>
        [Test]
        public void GetJobLogsUnsuccessfullyRetrieves()
        {
            var result = jobController.GetJobLogs(Guid.NewGuid());

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());


        }

        /// <summary>
        /// Test is given a valid JobId
        /// Test validates that the controller can get the job response for that JobId
        /// </summary>
        [Test]
        public void GetJobSuccessfullyRetrieves()
        {
            var jobId = Guid.NewGuid();
            var itemLogs = new List<ItemLog> { new ItemLog("a123456", true, new List<string>()) };

            this.jobService.StoreResponseData(jobId, itemLogs, string.Empty);

            var result = jobController.GetJobResponse(jobId);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());

        }

        /// <summary>
        /// Test is given a invalid JobId
        /// Test validates that the controller fails to get job response for that JobId
        /// </summary>
        [Test]
        public void GetJobUnsuccessfullyRetrieves()
        {
            var result = jobController.GetJobResponse(Guid.NewGuid());

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());

        }
    }
}
