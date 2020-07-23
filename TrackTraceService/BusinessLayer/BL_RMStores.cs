using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace BusinessLayer
{
    
    public class BL_RMStores
    {
        public string UpdateRecevingDL(string strBarcode, string strStatus, string strUserID)
        {
            SqlDataLayer objSql = new SqlDataLayer();
            SqlParameter[] objParameters = new SqlParameter[4];
            try
            {
                objParameters[0] = new SqlParameter("@BARCODE", SqlDbType.VarChar);
                objParameters[1] = new SqlParameter("@USERNAME", SqlDbType.VarChar);
                objParameters[2] = new SqlParameter("@STATUS", SqlDbType.VarChar);
                objParameters[3] = new SqlParameter("@RESULT", SqlDbType.VarChar, 100);

                objParameters[0].Value = strBarcode.Trim();
                objParameters[1].Value = strUserID.Trim();
                objParameters[2].Value = strStatus.Trim();
                objParameters[3].Direction = ParameterDirection.Output;

                BL_Common.BL_Write(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, 2, "Barcode receiving successfully.", strUserID);

                return "0~" + BL_Common.dataTableToString(objSql.ExecuteProcedure_Table(SqlDataLayer.strLocal, "sp_ScanningReceiving", objParameters));
            }
            catch (Exception ex)
            {
                BL_Common.BL_Write(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, 0, ex.ToString(), strUserID);
                return "1~" + objSql.getErrorMsg(ex.ToString());
            }
            finally
            {
                objParameters = null;
                objSql = null;
            }
        }

        public string UpdateRecevingDL_Initial(string strBarcode, string strQty, string strStatus)
        {
            SqlDataLayer objSql = new SqlDataLayer();
            SqlParameter[] objParameters = new SqlParameter[4];
            try
            {
                objParameters[0] = new SqlParameter("@BARCODE", SqlDbType.VarChar);
                objParameters[1] = new SqlParameter("@QTY", SqlDbType.Float);
                objParameters[2] = new SqlParameter("@STATUS", SqlDbType.VarChar);
                objParameters[3] = new SqlParameter("@RESULT", SqlDbType.VarChar, 100);

                objParameters[0].Value = strBarcode.Trim();
                objParameters[1].Value = Convert.ToDouble(strQty.Trim());
                objParameters[2].Value = Convert.ToInt32(strStatus);
                objParameters[3].Direction = ParameterDirection.Output;

                if (objSql.ExecuteProcedureParam(SqlDataLayer.strLocal, "sp_ScanningPicking_Initial", objParameters, "@RESULT", "@RESULT") != "")
                {
                    //BL_Common.BL_Write(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, 2, ex.ToString(), strUserID);
                    return "0~" + objParameters[3].Value.ToString();
                }
                else
                {
                    return "1~Error in Save.";
                }

                //BL_Common.dataTableToString(objSql.ExecuteProcedure_Table(SqlDataLayer.strLocal, "sp_ScanningPicking_Initial", objParameters));

                //return "0~" + objParameters[3].Value.ToString();
            }
            catch (Exception ex)
            {
                return "1~" + objSql.getErrorMsg(ex.ToString());
            }
            finally
            {
                objParameters = null;
                objSql = null;
            }
        }


        public string getReceiptData(string strReceiptNo)
        {
            SqlDataLayer objSql = new SqlDataLayer();
            try
            {
                if (string.IsNullOrEmpty(strReceiptNo) == true)
                {
                    return "0~" + BL_Common.dataTableToString(objSql.ExecuteDataset(SqlDataLayer.strLocal, "SELECT LOTNO, RECEIPT_NUM, ITEM_CODE, PROJECT_NUMBER, CAST(TXN_QTY AS VARCHAR(50)) + ' ' + UOM AS TXN_QTY, STATUS, ITEM_DESCRIPTION FROM TBLQC_DETAILS WHERE STATUS = 1").Tables[0]);
                }
                else
                {
                    return "0~" + BL_Common.dataTableToString(objSql.ExecuteDataset(SqlDataLayer.strLocal, "SELECT LOTNO, RECEIPT_NUM, ITEM_CODE, PROJECT_NUMBER, CAST(TXN_QTY AS VARCHAR(50)) + ' ' + UOM AS TXN_QTY, STATUS, ITEM_DESCRIPTION FROM TBLQC_DETAILS WHERE STATUS = 1 AND RECEIPT_NUM = '" + strReceiptNo.Trim() + "'").Tables[0]);
                }
            }
            catch (Exception ex)
            {
                BL_Common.BL_Write(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, 0, ex.ToString(), "-");
                return "1~" + objSql.getErrorMsg(ex.ToString());
            }
            finally
            {
                objSql = null;
            }
        }

        public string getPickSlipNo(string strUserID)
        {
            SqlDataLayer objSql = new SqlDataLayer();
            try
            {
                return "0~" + BL_Common.dataTableToString(objSql.ExecuteDataset(SqlDataLayer.strLocal, "SELECT DISTINCT (A.PICK_SLIP_NO) AS PICK_SLIP_NO FROM DBO.TBLPICK_SLIP_INWORD A INNER JOIN DBO.TBLPICK_SLIP_ASSIGN B ON A.PICK_SLIP_NO = B.PICK_SLIP_NO AND A.ST in (1, 2) AND B.ST = 0 AND B.PICKER = '" + strUserID.Trim() + "' and (isnull(A.ISSUED_QTY, 0) - Isnull(A.PICKED_QTY, 0)) > 0").Tables[0]);
            }
            catch (Exception ex)
            {
                BL_Common.BL_Write(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, 0, ex.ToString(), "-");
                return "1~" + objSql.getErrorMsg(ex.ToString());
            }
            finally
            {
                objSql = null;
            }
        }

        public string getPickSlipData(string strPackSlip, string strUserID)
        {
            SqlDataLayer objSql = new SqlDataLayer();
            try
            {
                return "0~" + BL_Common.dataTableToString(objSql.ExecuteDataset(SqlDataLayer.strLocal, "SELECT A.PICK_SLIP_NO, A.WORK_ORDER_NO, A.ITEM_CODE, REPLACE(REPLACE(A.DESCRIPTION, '@', ''), '$', '') AS DESCRIPTION, A.LOCTAOR AS LOCATOR, SUM(Isnull(A.ISSUED_QTY, 0)) AS PICK_QTY, SUM(Isnull(A.PICKED_QTY, 0)) AS PICKED_QTY, A.UOM FROM DBO.TBLPICK_SLIP_INWORD A INNER JOIN DBO.TBLPICK_SLIP_ASSIGN B ON A.PICK_SLIP_NO = B.PICK_SLIP_NO AND A.ST in (1, 2) AND B.ST = 0 AND B.PICKER = '" + strUserID.Trim() + "' AND A.PICK_SLIP_NO = '" + strPackSlip.Trim() + "'  and (isnull(A.ISSUED_QTY, 0) - Isnull(A.PICKED_QTY, 0)) > 0 group by A.PICK_SLIP_NO, A.ITEM_CODE, A.WORK_ORDER_NO, A.DESCRIPTION, A.LOCTAOR, A.UOM").Tables[0]);
            }
            catch (Exception ex)
            {
                BL_Common.BL_Write(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, 0, ex.ToString(), "-");
                return "1~" + objSql.getErrorMsg(ex.ToString());
            }
            finally
            {
                objSql = null;
            }
        }

        public string UpdatePicking(string strPackSlip, string strLocation, string strItemCode, string strQty, string strPacket, string strStatus, string strUserID)
        {
            SqlDataLayer objSql = new SqlDataLayer();
            SqlParameter[] objParameters = new SqlParameter[8];
            try
            {
                objParameters[0] = new SqlParameter("@PACKSLIP", SqlDbType.VarChar);
                objParameters[1] = new SqlParameter("@PACKBARCODE", SqlDbType.VarChar);
                objParameters[2] = new SqlParameter("@ITEMBARCODE", SqlDbType.VarChar);
                objParameters[3] = new SqlParameter("@LOCATION", SqlDbType.VarChar);
                objParameters[4] = new SqlParameter("@USERNAME", SqlDbType.VarChar);
                objParameters[5] = new SqlParameter("@QTY", SqlDbType.Float);
                objParameters[6] = new SqlParameter("@STATUS", SqlDbType.Int);
                objParameters[7] = new SqlParameter("@RESULT", SqlDbType.VarChar, 100);

                objParameters[0].Value = strPackSlip.Trim();
                objParameters[1].Value = strPacket.Trim();
                objParameters[2].Value = strItemCode.Trim();
                objParameters[3].Value = strLocation.Trim();
                objParameters[4].Value = strUserID.Trim();
                objParameters[5].Value = Convert.ToDouble(strQty);
                objParameters[6].Value = Convert.ToInt32(strStatus);
                objParameters[7].Direction = ParameterDirection.Output;

                if (objSql.ExecuteProcedureParam(SqlDataLayer.strLocal, "sp_ScanningPicking", objParameters, "@RESULT", "@RESULT") != "")
                {
                    BL_Common.BL_Write(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, 2, "Material picking successfully", strUserID);
                    return "0~" + objParameters[7].Value.ToString();
                }
                else
                {
                    BL_Common.BL_Write(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, 0, "Material picking failed", strUserID);
                    return "1~Error in picking.";
                }
            }
            catch (Exception ex)
            {
                BL_Common.BL_Write(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, 0, ex.ToString(), strUserID);
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
