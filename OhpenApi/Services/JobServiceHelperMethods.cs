using ErrorOr;
using OhpenApi.Contracts.Responses;
using OhpenApi.Entities.Models;
using OhpenContracts.Services;

namespace OhpenApi.Services
{
    public class JobServiceHelperMethods
    {
        /// <summary>
        /// This method is used internally by the Job Service classes
        /// it checks an individual customer transaction for errors
        /// any errors that are found are populated as part of the Item Log description
        /// </summary>
        /// <param name="customerTransactionUnderTest"></param>
        /// <returns></returns>
        public static List<string> AddErrorDescription(CustomerTransactionModel customerTransactionUnderTest)
        {
            var errorDescriptions = new List<string>();

            // Checking ID contains only alphanumeric characters
            if (!customerTransactionUnderTest.Id.All(x => char.IsLetterOrDigit(x)))
            {
                errorDescriptions.Add("Transaction ID contains invalid Character");
            }
            // Length Check
            if (customerTransactionUnderTest.AccountId.Length != 7)
            {
                errorDescriptions.Add("Sending Account ID should be of length 7");
            }

            // Checking Account ID contains only alphanumeric characters
            if (!customerTransactionUnderTest.AccountId.All(x => char.IsLetterOrDigit(x)))
            {
                errorDescriptions.Add("Account ID contains invalid Character");
            }
            // Length Check
            if (customerTransactionUnderTest.DestinationAccountId.Length != 7)
            {
                errorDescriptions.Add("Destination Account ID should be of length 7");
            }

            // Checking Destination Account ID contains only alphanumeric characters
            if (!customerTransactionUnderTest.DestinationAccountId.All(x => char.IsLetterOrDigit(x)))
            {
                errorDescriptions.Add("Destination Account ID contains invalid character");
            }

            // Checking Amount is not negative
            if (customerTransactionUnderTest.Amount < 0)
            {
                errorDescriptions.Add("Invalid amount, amount of a transaction should be above 0");
            }

            return errorDescriptions;
        }

        /// <summary>
        /// This method verifies what type of service to use when a job type has been specified
        /// </summary>
        /// <param name="jobType"></param>
        /// <returns></returns>
        public static BaseJobService JobServicesSwitch(string jobType)
        {
            switch (jobType.ToLower())
            {
                case "bulk":
                    return new BulkJobService();
                case "batch":
                    return new BatchJobService();
                default:
                    return new BaseJobService();
            }
        }
    }
}
