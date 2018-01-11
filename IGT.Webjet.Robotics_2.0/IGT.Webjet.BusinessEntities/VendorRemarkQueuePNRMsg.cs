using System;
using IGT.Webjet.CommonUtil;
using System.Collections.Generic;

namespace IGT.Webjet.BusinessEntities
{
    [Serializable]
    public class VendorRemarkQueuePNRMsg : ReadQueuePNRMsg
    {
        public List<string> GeneralRemark { get; set; }
        public List<string> VendorRemark { get; set; }

        public override string ToString()
        {
            return this.GetJsonString();
        }
    }
}
