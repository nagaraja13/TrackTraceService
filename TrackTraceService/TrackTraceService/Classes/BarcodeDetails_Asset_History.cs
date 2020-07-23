using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrackTraceService.Classes
{
    public class BarcodeDetails_Asset_History
    {
        public string Barcode { get; set; }
        public string I_Number { get; set; }
        public string Sub_Category { get; set; }
        public string Description { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string SLNO { get; set; }
        public string Range { get; set; }
        public string Calibration_Interval { get; set; }
        public DateTime? Calibrated_On { get; set; }
        public DateTime? Calibrated_Due_On { get; set; }
        public string Calibrate_Rep_On { get; set; }
        public string Calibration_Type { get; set; }
        public string Calibration_Agency { get; set; }
        public string Applicable_To { get; set; }
        public string Borrowers_Name { get; set; }
        public string Borrowers_Dept { get; set; }
        public DateTime? Issue_Date { get; set; }
        public string Acceptance_Criteria { get; set; }
        public string Acceptance_Decision { get; set; }
        public string Remark { get; set; }
        public string Last_Modified_By { get; set; }
        public DateTime? Last_Modified_Date { get; set; }
        public string Result { get; set; }

        
    }
}