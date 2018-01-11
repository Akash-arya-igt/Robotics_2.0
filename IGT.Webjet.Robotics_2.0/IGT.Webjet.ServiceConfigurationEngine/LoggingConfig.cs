using IGT.Webjet.CommonUtil;

namespace IGT.Webjet.ServiceConfigurationEngine
{
    public class LoggingConfig : ServiceBaseConfig
    {
        public string SourceName { get; set; }
        public string SumoCollectionURL { get; set; }

        public LoggingConfig()
            :base()
        {
            SourceName = EnvironmentUtil.GetStringEnvironmentVarValue("SOURCE_NAME");
            SumoCollectionURL = EnvironmentUtil.GetStringEnvironmentVarValue("SUMO_COLLECTION_URL");
        }
    }
}
