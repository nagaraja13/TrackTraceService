using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrackTraceService.Classes
{
    public class GetWIP_CompScan
    {
        public string ASSY_ITEM_CODE { get; set; }
        public string DESCRIPTION { get; set; }
        public decimal WO_QTY { get; set; }
        public string Result { get; set; }
    }
}