using System;
using IGT.Webjet.CommonUtil;

namespace IGT.Webjet.BusinessEntities
{
    [Serializable]
    public class ReadQueuePNRMsg
    {
        public string Recloc { get; set; }
        public string PCC { get; set; }
        public int FromQueue { get; set; }
        public DateTime StartTime { get; set; }

        public override string ToString()
        {
            return this.GetJsonString();
        }
    }
}
