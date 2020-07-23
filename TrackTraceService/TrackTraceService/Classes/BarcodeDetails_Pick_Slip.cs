using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrackTraceService.Classes
{
    public class BarcodeDetails_Pick_Slip
    {
        public string SERIALNO { get; set; }
        public string PICK_SLIP_NO { get; set; }
        public string WORK_ORDER_NO { get; set; }
        public string ITEM_CODE { get; set; }
        public string DESCRIPTION { get; set; }
        public string SUB_INVENTORY { get; set; }
        public string LOCATOR { get; set; }
        public string PROJECT { get; set; }
        public string TASK { get; set; }
        public decimal ISSUED_QTY { get; set; }
        public decimal PICKED_QTY { get; set; }
        public string UOM { get; set; }
        public string LOTNO { get; set; }
        public string Result { get; set; }
    }
}