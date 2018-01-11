using IGT.Webjet.BusinessEntities.Enum;

namespace IGT.Webjet.CloudEngine
{
    public static class CloudObjectFactory
    {
        public static IMsgQProvider GetMsgQProvider(CloudProvidersEnum _pCloudPro, string _pQName, string _pConnstionstring)
        {
            IMsgQProvider objMsgQ = null;

            switch(_pCloudPro)
            {
                case CloudProvidersEnum.Azure:
                    objMsgQ = new AzureMsgQProvider(_pQName, _pConnstionstring);
                    break;
                default:
                    objMsgQ = new AzureMsgQProvider(_pQName, _pConnstionstring);
                    break;
            }

            return objMsgQ;
        }

        public static INoSQLTableProvider GetNoSQLTableProvider(CloudProvidersEnum _pCloudPro, string _pQName, string _pConnstionstring)
        {
            INoSQLTableProvider objNoSQLTbl = null;

            switch (_pCloudPro)
            {
                case CloudProvidersEnum.Azure:
                    objNoSQLTbl = new AzureNoSQLTableProvider(_pQName, _pConnstionstring);
                    break;
                default:
                    objNoSQLTbl = new AzureNoSQLTableProvider(_pQName, _pConnstionstring);
                    break;
            }

            return objNoSQLTbl;
        }
    }
}
