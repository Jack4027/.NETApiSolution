using ErrorOr;
using OhpenApi.Contracts.Responses;
using OhpenApi.Entities.Models;
using OhpenApi.ServiceErrors;
using OhpenApi.Services;

namespace OhpenContracts.Services
{
    public class BaseJobService: IJobService
    {
        // Using dictionary as in memory data storage for Job log Responses
        // Job ID as Key and List of Job Log Responses as Value
        public static Dictionary<Guid, JobLogResponse>? JobLogResponses = new Dictionary<Guid, JobLogResponse>();

        // Using dictionary as in memory data storage
        // Job ID as Key and List of Start Job Responses as Value
        public static Dictionary<Guid,StartJobResponse>? StartJobResponses = new Dictionary<Guid,StartJobResponse>();

        /// <summary>
        /// This method starts the processesing of the users data request 
        /// If the processing is successful it will return the response object which is stored in memory 
        /// in the StartJobResponses Dictionary Object
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="jobModel"></param>
        /// <returns></returns>
        public ErrorOr<StartJobResponse> ProcessCustomerTransactions(Guid jobId, JobModel jobModel)
        {
        
            var itemLogs = DetermineJobSuccess(jobModel.CustomerTransactionModels, out string description);

            //Error Check
            if (itemLogs.IsError)
            {
                return itemLogs.FirstError;
            }

            StoreResponseData(jobId, itemLogs.Value, description);
           
            return StartJobResponses[jobId];
        }

        /// <summary>
        /// This method performs a simple get which first of all checks that the
        /// provided JobId the user is searching for exists
        /// if not then the method returns a not found 
        /// if it does the method returns the StartJobResponse
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public ErrorOr<StartJobResponse> GetJobResponse(Guid jobId)
        {
            if (StartJobResponses.ContainsKey(jobId))
            {
             return StartJobResponses[jobId];
            }
            return Errors.JobNotFound(jobId);
        }

        /// <summary>
        /// This method performs a simple get which first of all checks that the
        /// provided JobId the user is searching for exists
        /// if not then the method returns a not found 
        /// if it does the method returns the JobLogResponse
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public ErrorOr<JobLogResponse> GetLogResponse(Guid jobId)
        {
            if (JobLogResponses.ContainsKey(jobId))
            {
             return JobLogResponses[jobId];
            }
            return Errors.JobNotFound(jobId);
        }

        /// <summary>
        /// This method determines whether or not the job has been successful
        /// The job is deemed unsuccessful if any transactions have errors, the only jobs that will complete if there are errors on the transactions
        /// are bulk jobs
        /// It calls the ErrorCheckCustomer transactions method which iterates through each customer transaction and performs the validation checks
        /// </summary>
        /// <param name="customerTransactionModels"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        
        public ErrorOr<List<ItemLog>> DetermineJobSuccess(List<CustomerTransactionModel> customerTransactionModels, out string description)
        {
            var jobSuccess = true;
            var itemLogs = ErrorCheckCustomerTransactions(customerTransactionModels, ref jobSuccess);
            description = jobSuccess ? string.Empty : "Some records contain errors, check the job log. Only bulk jobs will have completed";

            if (itemLogs.IsError)
            {
                return itemLogs.FirstError;
            }

            return itemLogs;
        }

        /// <summary>
        /// This method will store response data in the appropriate in memory objects
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="itemLogs"></param>
        /// <param name="description"></param>
        public void StoreResponseData(Guid jobId, List<ItemLog>itemLogs, string? description)
        {
            var jobLogs = new JobLogResponse(jobId, itemLogs);

            // Adding the Job Log responses to our in memory data storage
            JobLogResponses.Add(jobId, jobLogs);

            // Creating response object
            var response = new StartJobResponse(jobId, description);

            // Adding response to in memory storage
            StartJobResponses.Add(jobId, response);
        }

        /// <summary>
        /// Because the application is only taking in Bulk and Batch Job requests failure 
        /// to specify either of these types as the job type results in an error being returned 
        /// asking the user to specify either bulk or batch job
        /// </summary>
        /// <param name="customerTransactionModels"></param>
        /// <param name="jobSuccess"></param>
        /// <returns></returns>
        public virtual ErrorOr<List<ItemLog>> ErrorCheckCustomerTransactions(List<CustomerTransactionModel> customerTransactionModels, ref bool jobSuccess)
        {
            // Returns the Job Type Validation Error from ServicesErrors.Errors file
            return Errors.JobTypeValidation();
        }

    }
}

