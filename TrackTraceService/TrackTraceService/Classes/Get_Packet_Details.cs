﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrackTraceService.Classes
{
    public class Get_Packet_Details
    {
       public string ITEM_CODE { get; set; }
       public string DESCRIPTION { get; set; }
       public decimal ISSUED_QTY { get; set; }
       public decimal PICK_QTY { get; set; }
       public string UOM { get; set; }
       public string Result { get; set; }
    }
}