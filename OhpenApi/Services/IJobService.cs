using System;
using ErrorOr;
using OhpenApi.Contracts.Responses;
using OhpenApi.Entities.Models;
using OhpenContracts.Services;

namespace OhpenApi.Services
{
    public interface IJobService
	{
        ErrorOr<StartJobResponse> ProcessCustomerTransactions(Guid jobId, JobModel jobModel);

        ErrorOr<StartJobResponse> GetJobResponse(Guid jobId);

        ErrorOr<JobLogResponse> GetLogResponse(Guid jobId);

        ErrorOr<List<ItemLog>> DetermineJobSuccess(List<CustomerTransactionModel> customerTransactionModels, out string description);


        void StoreResponseData(Guid jobId, List<ItemLog> itemLogs, string? description);


    }
}

