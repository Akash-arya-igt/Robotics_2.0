using IGT.Webjet.CommonUtil;
using IGT.Webjet.BusinessEntities.Enum;

namespace IGT.Webjet.ServiceConfigurationEngine
{
    public abstract class ServiceBaseConfig
    {
        /******** NEED TO DEFINE IN CONFIG MAPS/SECRET ********        
          
          1. (COMMON)
          DELAY_INTERVAL
          LOCAL_LOG_PATH
          SERVICE_NAME
            Possible values (Only for GAL Service):
            1. RemovePNRFromQ,
            2. ReadVendorRemarkQ,
            3. ReadBookingCaptureQ,
            4. ReadScheduleChangeQ,
            5. AddRemark
          CLOUD_LOGGING_Q_NAME
          CONNECTION_STRING
          CLOUD_PROVIDER
            Possible values
            1. Azure,
            2. AWS,
            3. Google,
            4. IBM,
            5. Oracle
          
          2. (GAL SERVICE)
          PCC
          GAL_Q_NUMBER
          DESTINATION_AZURE_Q_NAME
          +
          3. (SERVICE AUTH SECRET)
          PCC_SERVICE_END_POINT
          PCC_PROFILE
          PCC_USERID
          PCC_PASSWORD
          PCC_GDS_PROVIDER_SERIVCE
            Possible values:
            1. GAL_XMLSelect
            2. GAL_UAPI
          
          2. (NON GAL SERVICE)
          SOURCE_Q_NAME
          DESTINATION_Q_NAME

          2. (LOGGING)
          SOURCE_NAME
          SUMO_COLLECTION_URL
        
        */


        public int DelayInterval { get; set; }
        public string LocalLogPath { get; set; }
        public string ServiceName { get; set; }
        public string CloudLoggingQName { get; set; }
        public string ConnectionString { get; set; }
        public CloudProvidersEnum CloudProvider { get; set; }

        public ServiceBaseConfig()
        {
            DelayInterval = EnvironmentUtil.GetIntEnvironmentVarValue("DELAY_INTERVAL");
            LocalLogPath = EnvironmentUtil.GetStringEnvironmentVarValue("LOCAL_LOG_PATH");
            ServiceName = EnvironmentUtil.GetStringEnvironmentVarValue("SERVICE_NAME");
            CloudLoggingQName = EnvironmentUtil.GetStringEnvironmentVarValue("CLOUD_LOGGING_Q_NAME");
            ConnectionString = EnvironmentUtil.GetStringEnvironmentVarValue("CONNECTION_STRING");
            var strCloudPro = EnvironmentUtil.GetStringEnvironmentVarValue("CLOUD_PROVIDER");
            CloudProvider = strCloudPro.ToEnum<CloudProvidersEnum>();
        }
    }
}
