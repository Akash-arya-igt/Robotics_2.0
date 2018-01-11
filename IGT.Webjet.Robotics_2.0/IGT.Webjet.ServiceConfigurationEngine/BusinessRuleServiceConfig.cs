using IGT.Webjet.CommonUtil;

namespace IGT.Webjet.ServiceConfigurationEngine
{
    public class BusinessRuleServiceConfig : ServiceBaseConfig
    {
        public string SourceQName { get; set; }
        public string DestinationQName { get; set; }

        public BusinessRuleServiceConfig()
            :base()
        {
            SourceQName = EnvironmentUtil.GetStringEnvironmentVarValue("SOURCE_Q_NAME");
            DestinationQName = EnvironmentUtil.GetStringEnvironmentVarValue("DESTINATION_Q_NAME");
        }
    }
}
