using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrackTraceService.Classes
{
    public class BarcodeDetails_Accepted_Rejected
    {
        public string LOTNO { get; set; }
        public string RECEIPT_NUM { get; set; }
        public string WO_NUMBER { get; set; }
        public string SUPPLIER_NAME { get; set; }
        public string ITEM_CODE { get; set; }
        public string ITEM_DESCRIPTION { get; set; }
        public string PROJECT_NUMBER { get; set; }
        public string TASK_NUMBER { get; set; }
        public decimal QTY { get; set; }
        public string UOM { get; set; }
        public string DWG_NUMBER { get; set; }
        public string WCH_ISSUE_NUMBER { get; set; }
        public string SHELF_EXP_DATE { get; set; }
        public string BATCH_NUMBER { get; set; }
        public string MS_LEVEL { get; set; }
        public string TYPE { get; set; }
        public string CRUSER { get; set; }
        public DateTime CRDATE { get; set; }
        public string PART_REV_NO_INSPN { get; set; }
        public string MFGSERIAL { get; set; }
        public string Result { get; set; }
        
    }
}