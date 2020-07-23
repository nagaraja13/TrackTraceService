using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrackTraceService.Classes
{
    public class BarcodeDetails_Asset
    {
        public string BARCODE { get; set; }
        public string CATEGORY { get; set; }
        public string SUBCATEGORY { get; set; }
        public string ERPASSETCODE { get; set; }
        public string DESCRIPTION { get; set; }
        public string MAKE { get; set; }
        public string MODEL { get; set; }
        public string SLNO { get; set; }
        public string I_NUMBER { get; set; }
        public string VALUE { get; set; }
        public string VENDOR { get; set; }
        public string PARRENT_ASSET { get; set; }
        public string RANGE { get; set; }
        public string LEAST_COUNT { get; set; }
        public string CRITICAL_INST { get; set; }
        public string COMS_ON { get; set; }
        public DateTime? INSTALLED_ON { get; set; }
        public DateTime? WARRANTY_DUE_ON { get; set; }
        public DateTime? AMC_DUE { get; set; }
        public string PR_MAINT_INTER { get; set; }
        public string CALIBRATE_INTER { get; set; }
        public DateTime? CALIBRATED_ON { get; set; }
        public DateTime? CALIBRATED_DUE { get; set; }
        public string CALIBRATE_REP_NO { get; set; }
        public string VAL_INTER { get; set; }
        public DateTime? VALIDATED_ON { get; set; }
        public DateTime? VALIDATED_DUE { get; set; }
        public string TOBEUSED { get; set; }
        public string P_M_MANUAL { get; set; }
        public string CHECK_METHOD { get; set; }
        public string APP_TO { get; set; }
        public string BORROWER { get; set; }
        public DateTime? ISSUE_DT { get; set; }
        public string REMARK { get; set; }
        public string CRUSER { get; set; }
        public string CRDATE { get; set; }
        public string MDUSER { get; set; }
        public string MDDATE { get; set; }
        public string PLANT { get; set; }
        public decimal ST { get; set; }
        public string Result { get; set; }

        

    }
}