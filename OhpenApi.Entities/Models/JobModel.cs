using System;

namespace OhpenApi.Entities.Models
{
    /// <summary>
    /// This class models the Job request received from the Request body
    /// </summary>
    public class JobModel
    {
        public string JobType { get; }

        public List<CustomerTransactionModel> CustomerTransactionModels { get; }

        public JobModel(string jobType, List<CustomerTransactionModel> customerTransactionModels)
        {
            JobType = jobType;
            CustomerTransactionModels = customerTransactionModels;
        }
    }
}

