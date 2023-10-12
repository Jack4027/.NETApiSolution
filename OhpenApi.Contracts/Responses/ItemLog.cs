using System;
namespace OhpenApi.Contracts.Responses
{
    // Making this a record type makes it immutable which is desired
    public record ItemLog(string ItemId, bool Success, List<string> Description);
}

