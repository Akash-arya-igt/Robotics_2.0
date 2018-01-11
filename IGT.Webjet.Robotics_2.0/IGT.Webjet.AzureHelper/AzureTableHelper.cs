using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace IGT.Webjet.AzureHelper
{
    public class AzureTableHelper
    {
        private CloudTable _entityTable;

        public AzureTableHelper(string _pQName, string _pConnstionstring)
        {
            var storageAccount = CloudStorageAccount.Parse(_pConnstionstring);

            // Create the CloudQueueClient object for the storage account.
            CloudTableClient queueClient = storageAccount.CreateCloudTableClient();

            // Get a reference to the CloudQueue named "messageQueue"
            _entityTable = queueClient.GetTableReference(_pQName);

            Task createQTask = _entityTable.CreateIfNotExistsAsync();
            createQTask.Wait();
        }

        public async Task AddEntityAsync<T>(T _pEntity) where T : TableEntity
        {
            TableOperation insertOperation = TableOperation.Insert(_pEntity);
            await _entityTable.ExecuteAsync(insertOperation);
        }

        public void AddEntity<T>(T _pEntity) where T : TableEntity
        {
            Task tskAddEntity = AddEntityAsync(_pEntity);
            tskAddEntity.Wait();
        }

        public async Task BatchInsertAsync<T>(List<T> _pEntities) where T : TableEntity
        {
            TableBatchOperation batchOperation = new TableBatchOperation();
            foreach (var _pEntity in _pEntities)
            {
                batchOperation.Insert(_pEntity);
            }

            await _entityTable.ExecuteBatchAsync(batchOperation);
        }

        public void BatchInsert<T>(List<T> _pEntities) where T : TableEntity
        {
            Task tskBatchIns = BatchInsertAsync(_pEntities);
            tskBatchIns.Wait();
        }


        //public async Task GetPartitionRecords<T>(string _pPartitionKey) where T : TableEntity
        //{
        //    TableQuery<T> query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, _pPartitionKey));
        //    _entityTable.ExecuteBatchAsync()
        //}
    }
}
