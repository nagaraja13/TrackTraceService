using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrackTraceService.Classes
{
    public class BarcodeDetails_Assembly
    {
        public string TRACKING_ID { get; set; }
        public string WO_NO { get; set; }
        public string ASSY_ITEM_CODE { get; set; }
        public string DESCRIPTION { get; set; }
        public decimal WO_QTY { get; set; }
        public string SER { get; set; }
        public string ITEM_TYPE { get; set; }
        public string PROJECT_NUMBER { get; set; }
        public string TASK_NUMBER { get; set; }
        public string WO_STATUS { get; set; }   
        public string Result { get; set; }
            
    }
}