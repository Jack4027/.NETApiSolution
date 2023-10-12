using ErrorOr;
using OhpenApi.Contracts.Responses;
using OhpenApi.Entities.Models;
using OhpenContracts.Services;

namespace OhpenApi.Services
{
    public class BulkJobService: BaseJobService
    {
        /// <summary>
        /// This method Error checks the customer transactions in the Bulk Job instance 
        /// This instance will allow the job to finish even if there is errors on it so 
        /// a for each loop is used to simplify code and iterate through each item in the list
        /// </summary>
        /// <param name="customerTransactionModels"></param>
        /// <param name="jobSuccess"></param>
        /// <returns>ErrorOr<List<ItemLog>></returns>
        public override ErrorOr<List<ItemLog>> ErrorCheckCustomerTransactions(List<CustomerTransactionModel> customerTransactionModels, ref bool jobSuccess)
        {
            List<ItemLog> itemLogs = new List<ItemLog>();
            foreach(var model in  customerTransactionModels)
            {
                var errorDescriptions = JobServiceHelperMethods.AddErrorDescription(model);
                var itemSuccess = errorDescriptions.Count == 0 ? true : false;

                if (!itemSuccess)
                {
                    jobSuccess = false;
                }

                var itemLog = new ItemLog(model.Id, itemSuccess, errorDescriptions);
                itemLogs.Add(itemLog);
            }

            return itemLogs;
        }
    }
}
