using System;
using ErrorOr;
using OhpenApi.Contracts.Responses;
using OhpenApi.Entities.Models;
using OhpenContracts.Services;

namespace OhpenApi.Services
{
    public class BatchJobService: BaseJobService
	{
        /// <summary>
        /// This method Error Checks the list of transactions in the batch instance, behaviour differs here as if an item contains any errors the job will stop
        /// thus a while loop is more appropriate as it takes into account the abortErrorCheck condition which activates when an item contains errors
        /// </summary>
        /// <param name="customerTransactionModels"></param>
        /// <param name="jobSuccess"></param>
        /// <returns>ErrorOr<List<ItemLog>></returns>
        public override ErrorOr<List<ItemLog>> ErrorCheckCustomerTransactions(List<CustomerTransactionModel> customerTransactionModels, ref bool jobSuccess)
        {
            bool abortErrorCheck = false;
            int loopCount = 0;

            List<ItemLog> itemLogs = new List<ItemLog>();
            while (!abortErrorCheck & loopCount < customerTransactionModels.Count)
            {
                CustomerTransactionModel customerTransactionUnderTest = customerTransactionModels[loopCount];
                var errorDescriptions = JobServiceHelperMethods.AddErrorDescription(customerTransactionUnderTest);
                var success = errorDescriptions.Count == 0 ? true : false;

                if (!success)
                {
                    jobSuccess = false;
                    abortErrorCheck = true;
                }

                var itemLog = new ItemLog(customerTransactionUnderTest.Id, success, errorDescriptions);
                itemLogs.Add(itemLog);
                loopCount++;
            }

            return itemLogs;
        }
    }
}

