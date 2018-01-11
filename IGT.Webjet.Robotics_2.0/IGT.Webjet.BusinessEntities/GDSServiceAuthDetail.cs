using IGT.Webjet.BusinessEntities.Enum;

namespace IGT.Webjet.BusinessEntities
{
    public class GDSServiceAuthDetail
    {
        public string PCC { get; set; }
        public GDSProviderServiceEnum GDSProviderService { get; set; }
        public string Profile { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
        public string GWSConnURL { get; set; }
    }
}
