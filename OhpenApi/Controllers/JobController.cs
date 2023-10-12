using Microsoft.AspNetCore.Mvc;
using OhpenApi.Entities.Models;
using OhpenApi.Services;
using ErrorOr;
using OhpenApi.ServiceErrors;
using OhpenApi.Contracts.Requests;
using OhpenApi.Contracts.Responses;

namespace OhpenApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobController: ControllerBase
    {

        private IJobService jobService;

        //Injecting different types of service via dependency injection
        public JobController(IJobService jobService)
        {
            this.jobService = jobService;
        }

        /// <summary>
        /// This endpoint will be where the user will start the job request
        /// It will receive the user's data and the transfer this to the service class to be processed 
        /// Bulk jobs will process all transactions to completion even if some have errors 
        /// Batch jobs will stop at the first job that contains any errors, any further jobs will not be processed
        /// It will then either recieve back an JobTypeValidation Error or the Start Job Response Object from the service class
        /// If it receives back the error it returns a Bad Request Object to the user telling them to enter 
        /// either bulk or batch as the job type
        /// If it receives the start job response then it returns this as a Created At Action which will return the response
        /// and where it can be accessed - the Get Job endpoint
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult PostTransactions([FromBody] StartJobRequest request )
        {
            var jobType = request.JobType;

            //Implementing correct abstraction of the Job Service Interface
            this.jobService = JobServiceHelperMethods.JobServicesSwitch(jobType);

            //Taking request data and storing it as C# object
            var jobmodel = new JobModel(jobType, request.JobItems);

            // Generating Unique JobID
            var jobId = Guid.NewGuid();

            //Using the Service class to generate the response Batch response object
            var result = this.jobService.ProcessCustomerTransactions(jobId, jobmodel);

            //Error Check
            if (result.IsError && result.FirstError == Errors.JobTypeValidation())
            {
                return BadRequest(result.FirstError);
            }
            //Returning response object to the user
            return CreatedAtAction(nameof(GetJobResponse), new { jobId = jobId }, result.Value);
        }

        /// <summary>
        /// This endpoint will be the location at which the user can retrieve the Start Job Response
        /// that is returned when the user launches a start job request.
        /// The response consists of the Job ID and a message that details whether or not anything went wrong
        /// with the request.
        /// The Url in this instances will be Job/{jobId}.
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpGet("{jobId:guid}")]
        public IActionResult GetJobResponse(Guid jobId)
        {
            // Calling Service to retrieve from in memory storage 
            ErrorOr<StartJobResponse> result = jobService.GetJobResponse(jobId);

            //Error Check
            if (result.IsError && result.FirstError == Errors.JobNotFound(jobId))
            {
                return NotFound(result.FirstError);
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// This endpoint will be the location at which the customer can view the log response
        /// for a specified jobid (passed via the URL).
        /// This response consists of the Job ID and a list of responses from each individual
        /// item from the job.
        /// Each Job Response consists of an Item ID, a Success Boolean and a Description as
        /// a string list which details any errors that occured.
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpGet("jobLogs/{jobId:guid}")]
        public IActionResult GetJobLogs(Guid jobId)
        {
            // Calling Service to retrieve from in memory storage 
            ErrorOr<JobLogResponse> result = jobService.GetLogResponse(jobId);

            //Error Check
            if (result.IsError && result.FirstError == Errors.JobNotFound(jobId))
            {
                return NotFound(result.FirstError);
            }
            return Ok(result.Value);
        }
    }
}

