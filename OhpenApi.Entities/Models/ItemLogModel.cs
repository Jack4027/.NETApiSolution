namespace OhpenApi.Entities.Models
{
    /// <summary>
    /// This class provides a model for each Item value 
    /// in the Log response
    /// </summary>
    public class ItemLogModel
    {
        private string ItemId { get; set; }
        private bool Success { get; set; }
        private string description { get; set; }

        public ItemLogModel(string itemId, bool success, string description)
        {
            ItemId = itemId;
            Success = success;
            this.description = description;
        }
    }
}



