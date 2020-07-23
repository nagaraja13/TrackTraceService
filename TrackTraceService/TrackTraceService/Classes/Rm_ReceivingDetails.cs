using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrackTraceService.Classes
{
    public class Rm_ReceivingDetails
    {
        public string LOTNO { get; set; }
        public string RECEIPT_NUM { get; set; }
        public string ITEM_CODE { get; set; }
        public string PROJECT_NUMBER { get; set; }
        public string TXN_QTY { get; set; }
        public decimal STATUS { get; set; }
        public string ITEM_DESCRIPTION { get; set; }
        public string Result { get; set; }
    }
}