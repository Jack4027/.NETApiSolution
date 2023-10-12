namespace OhpenApi.Contracts.Responses
{
    // record type chosen as records are immutable meaning the response object can not be changed.
    public record StartJobResponse(Guid Id, string message);

}

