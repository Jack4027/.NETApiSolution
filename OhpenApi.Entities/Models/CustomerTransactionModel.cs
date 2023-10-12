namespace OhpenApi.Entities.Models
{
    /// <summary>
    /// This class provides a model for each customer transaction
    /// received from the request body
    /// </summary>
    public class CustomerTransactionModel
    {
        public string Id { get; }

        public string AccountId { get; }

        public string DestinationAccountId { get; }

        public int Amount { get; }

        public CustomerTransactionModel(string id, string accountId, string destinationAccountId, int amount)
        {
            Id = id;
            AccountId = accountId;
            DestinationAccountId = destinationAccountId;
            Amount = amount;

        }
    }
}

