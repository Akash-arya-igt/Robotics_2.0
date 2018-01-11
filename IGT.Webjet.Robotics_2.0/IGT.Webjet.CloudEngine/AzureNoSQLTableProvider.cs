using IGT.Webjet.AzureHelper;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace IGT.Webjet.CloudEngine
{
    class AzureNoSQLTableProvider : INoSQLTableProvider
    {
        private AzureTableHelper _objAzureTblHelper;

        public AzureNoSQLTableProvider(string _pQName, string _pConnstionstring)
        {
            _objAzureTblHelper = new AzureTableHelper(_pQName, _pConnstionstring);
        }

        public void AddEntity<T>(T _pEntity) where T : TableEntity
        {
            _objAzureTblHelper.AddEntity(_pEntity);
        }

        public void BatchInsert<T>(List<T> _pEntities) where T : TableEntity
        {
            _objAzureTblHelper.BatchInsert(_pEntities);
        }
    }
}
