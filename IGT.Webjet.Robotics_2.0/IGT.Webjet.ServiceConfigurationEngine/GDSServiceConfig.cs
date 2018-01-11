using IGT.Webjet.CommonUtil;
using IGT.Webjet.BusinessEntities;
using IGT.Webjet.BusinessEntities.Enum;

namespace IGT.Webjet.ServiceConfigurationEngine
{
    public class GDSServiceConfig : ServiceBaseConfig
    {
        public string PCC { get; set; }
        public GDSServiceAuthDetail GDSAuthDetail { get; set; }
        public string Profile { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public int GalQNumber { get; set; }
        public string DestinationCloudQName { get; set; }
        public PNRMsgTemplateEnum MsgTemplate { get; set; }
        public RoboticsServiceNameEnum StorngTypeServiceName { get; set; }

        public GDSServiceConfig()
            : base()
        {
            PCC = EnvironmentUtil.GetStringEnvironmentVarValue("PCC");
            GalQNumber = EnvironmentUtil.GetIntEnvironmentVarValue("GAL_Q_NUMBER");
            DestinationCloudQName = EnvironmentUtil.GetStringEnvironmentVarValue("DESTINATION_AZURE_Q_NAME");
            StorngTypeServiceName = ServiceName.ToEnum<RoboticsServiceNameEnum>();
            var gdsProviderSerivce = EnvironmentUtil.GetStringEnvironmentVarValue(PCC + "_GDS_PROVIDER_SERIVCE");

            GDSServiceAuthDetail objHAP = new GDSServiceAuthDetail()
            {
                PCC = PCC,
                GWSConnURL = EnvironmentUtil.GetStringEnvironmentVarValue(PCC + "_SERVICE_END_POINT"),
                Profile = EnvironmentUtil.GetStringEnvironmentVarValue(PCC + "_PROFILE"),
                UserID = EnvironmentUtil.GetStringEnvironmentVarValue(PCC + "_USERID"),
                Password = EnvironmentUtil.GetStringEnvironmentVarValue(PCC + "_PASSWORD"),
                GDSProviderService = gdsProviderSerivce.ToEnum<GDSProviderServiceEnum>()
            };

            GDSAuthDetail = objHAP;

            // DEFINING MESSAGE TEMPALTE FOR THE FLOW
            switch (StorngTypeServiceName)
            {
                case RoboticsServiceNameEnum.RemovePNRFromQ:
                    MsgTemplate = PNRMsgTemplateEnum.PNRMsg;
                    break;

                case RoboticsServiceNameEnum.ReadVendorRemarkQ:
                    MsgTemplate = PNRMsgTemplateEnum.VendorRemarkPNRMsg;
                    break;

                default:
                    MsgTemplate = PNRMsgTemplateEnum.PNRMsg;
                    break;
            }
        }
    }
}
