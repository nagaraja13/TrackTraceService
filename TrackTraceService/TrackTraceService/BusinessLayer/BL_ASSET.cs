using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace BusinessLayer
{
    public class BL_ASSET
    {
        public string Asset_Calibration(string strBarcode, string strLastCaliDate, string strUsername)
        {
            SqlDataLayer objSql = new SqlDataLayer();
            SqlParameter[] objParameters = new SqlParameter[5];
            try
            {
                objParameters[0] = new SqlParameter("@ASSETCODE", SqlDbType.VarChar);
                objParameters[1] = new SqlParameter("@CALDATE", SqlDbType.DateTime);
                objParameters[2] = new SqlParameter("@MODE", SqlDbType.VarChar);
                objParameters[3] = new SqlParameter("@USERNAME", SqlDbType.VarChar);
                objParameters[4] = new SqlParameter("@RESULT", SqlDbType.VarChar, 600);

                objParameters[0].Value = strBarcode.Trim();
                if (strLastCaliDate == string.Empty) { objParameters[1].Value = DateTime.Now; } else { objParameters[1].Value = Convert.ToDateTime(strLastCaliDate.Trim()); }
                if (strLastCaliDate == "") { objParameters[2].Value = "GET"; } else { objParameters[2].Value = "SET"; }
                objParameters[3].Value = strUsername.Trim();
                objParameters[4].Direction = ParameterDirection.Output;

                if (objSql.ExecuteProcedureParam(SqlDataLayer.strLocal, "sp_ScanningCalibration", objParameters, "@RESULT", "@RESULT") != "")
                {
                    BL_Common.BL_Write(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, 2, "Executed Successfully.", strUsername);
                    return  objParameters[4].Value.ToString();
                }
                else
                {
                    BL_Common.BL_Write(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, 0, "Execution Failed.", strUsername);
                    return "1~Invalid assetcode scanned.";
                }
            }
            catch (Exception ex)
            {
                BL_Common.BL_Write(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, 0, ex.ToString(), strUsername);
                return "1~" + objSql.getErrorMsg(ex.ToString());
            }
            finally
            {
                objParameters = null;
                objSql = null;
            }
        }
    }
}

