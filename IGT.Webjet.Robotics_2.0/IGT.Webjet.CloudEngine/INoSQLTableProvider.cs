using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace IGT.Webjet.CloudEngine
{
    public interface INoSQLTableProvider
    {
        void AddEntity<T>(T _pEntity) where T : TableEntity;
        void BatchInsert<T>(List<T> _pEntities) where T : TableEntity;
    }
}
