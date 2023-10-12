using OhpenApi.Contracts.Responses;
using OhpenApi.Entities.Models;
using OhpenApi.Services;
using OhpenContracts.Services;

namespace OhpenApi.Tests
{
    /// <summary>
    /// Test suite to check good flow and error flow of the BaseJobService class
    /// </summary>
    [TestFixture]
    public class BaseJobServiceTests
    {
        protected BaseJobService? baseJobService;
        protected Guid testJobId;
        protected Dictionary<Guid,StartJobResponse> startJobResponses;
        protected Dictionary<Guid,JobLogResponse> jobLogResponses;

        // Setting up test context
        public BaseJobServiceTests() {
            this.testJobId = Guid.NewGuid();

        }

        [SetUp]
        public void SetUp()
        {
            this.startJobResponses = BaseJobService.StartJobResponses;
            this.jobLogResponses = BaseJobService.JobLogResponses;
            this.baseJobService = new BaseJobService();

        }
        //Taking away any additions to in memory storage after a test is complete
        [TearDown]
        public void TearDown()
        {
            if (this.startJobResponses.ContainsKey(this.testJobId))
            {
                this.startJobResponses.Remove(this.testJobId);
            }

            if (this.jobLogResponses.ContainsKey(this.testJobId))
            {
                this.jobLogResponses.Remove(this.testJobId);
            }
            this.baseJobService = null;

        }

        /// <summary>
        /// Test validates that the GetJobResponse method returns a Start Job Response 
        /// if a valid jobId is submitted
        /// </summary>
        [Test]
        public void GetJobResponseReturnsJobResponse()
        {
            // Arrange
            this.startJobResponses.Add(testJobId, new StartJobResponse(testJobId, string.Empty));

            // Assert
            var actual = this.baseJobService.GetJobResponse(testJobId);

            // Assert
            Assert.That(!actual.IsError);
        }

        /// <summary>
        /// Test validates that the GetJobResponse method returns a not found error
        /// if an invalid jobId is submitted
        /// </summary>
        [Test]
        public void GetJobResponseReturnsError()
        {
            // Act
            var actual = this.baseJobService.GetJobResponse(Guid.Empty);

            // Assert
            Assert.That(actual.IsError);
        }

        /// <summary>
        /// Test validates that the GetJobLogResponse method returns a Job Log Response 
        /// if a valid jobId is submitted
        /// </summary>
        [Test]
        public void GetJobLogseReturnsLogResponse()
        {
            // Arrange 
            this.jobLogResponses.Add(testJobId, new JobLogResponse(testJobId, new List<ItemLog>() ));

            // Act
            var actual = this.baseJobService.GetLogResponse(testJobId);

            // Assert
            Assert.That(!actual.IsError);
        }

        /// <summary>
        /// Test validates that the GetJobLogResponse method returns an not found error
        /// if an invalid jobId is submitted
        /// </summary>
        [Test]
        public void GetJobLogseReturnsError()
        {
            // Act
            var actual = this.baseJobService.GetLogResponse(Guid.Empty);

            // Assert
            Assert.That(actual.IsError);
        }
    }
}
