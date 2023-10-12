namespace OhpenApi.Contracts.Requests
{
    using OhpenApi.Entities.Models;

    // record type chosen as records are immutable meaning the request object can not be changed once created.
    public record StartJobRequest(string JobType, List<CustomerTransactionModel> JobItems);
}