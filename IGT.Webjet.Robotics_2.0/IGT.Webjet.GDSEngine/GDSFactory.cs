using IGT.Webjet.BusinessEntities;
using IGT.Webjet.BusinessEntities.Enum;

namespace IGT.Webjet.GDSEngine
{
    public class GDSFactory
    {
        static public IGDSProvider GetGDSProvider(GDSServiceAuthDetail _gdsServiceAuthDetail)
        {
            IGDSProvider objSelector = null;

            switch (_gdsServiceAuthDetail.GDSProviderService)
            {
                case GDSProviderServiceEnum.GAL_XMLSelect:
                    objSelector = new GALProvider(_gdsServiceAuthDetail);
                    break;
                case GDSProviderServiceEnum.GAL_UAPI:
                    objSelector = new GALProvider(_gdsServiceAuthDetail);
                    break;
                default:
                    objSelector = new GALProvider(_gdsServiceAuthDetail);
                    break;
            }

            return objSelector;
        }
    }
}
