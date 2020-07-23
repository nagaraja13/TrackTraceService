using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrackTraceService.Classes
{
    public class Rm_GetpickingData
    {
        public string PICK_SLIP_NO { get; set; }
        public string WORK_ORDER_NO { get; set; }
        public string ITEM_CODE { get; set; }
        public string DESCRIPTION { get; set; }
        public string LOCTAORs { get; set; }
        public decimal PICK_QTY { get; set; }
        public decimal PICKED_QTY { get; set; }
        public string UOM { get; set; }
        public string Result { get; set; }
       
    }
}