using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BusinessLayer
{
    public class BL_WIP
    {
        #region "RECEIPT OF KIT / MATERIAL"

        public DataTable PickSlip_KitMat(string strUsername)
        {
            SqlDataLayer objSql = new SqlDataLayer();
            try
            {
                return objSql.ExecuteDataset(SqlDataLayer.strLocal, "SELECT DISTINCT(PICK_SLIP_NO) AS PICK_SLIP_NO FROM DBO.TBLPICKING WHERE ST = 1 ORDER BY PICK_SLIP_NO ASC").Tables[0];
            }
            catch (Exception ex)
            {
                //return "1~" + objSql.getErrorMsg(ex.ToString());
                throw ex;
            }
            finally
            {
                objSql = null;
            }
        }

        public DataTable PickSlip_KitMat_Details(string strPackSlip)
        {
            SqlDataLayer objSql = new SqlDataLayer();
            try
            {
                return objSql.ExecuteDataset(SqlDataLayer.strLocal, "SELECT PICK_SLIP_NO, ITEM_CODE, REPLACE(REPLACE(DESCRIPTION, '@', ''), '$', '') AS DESCRIPTION, ISSUED_QTY, PICK_QTY, UOM FROM DBO.TBLPICKING WHERE ST = 1 AND PICK_SLIP_NO = '" + strPackSlip.Trim() + "' ORDER BY PICK_SLIP_NO ASC").Tables[0];
            }
            catch (Exception ex)
            {
               // return "1~" + objSql.getErrorMsg(ex.ToString());
                throw ex;
            }
            finally
            {
                objSql = null;
            }
        }

        public string Update_KitMat(string strPackSlip, string strPacketNo, string strUsername)
        {
            SqlDataLayer objSql = new SqlDataLayer();
            SqlParameter[] objParameters = new SqlParameter[4];
            try
            {
                objParameters[0] = new SqlParameter("@PACKSLIP", SqlDbType.VarChar);
                objParameters[1] = new SqlParameter("@PACKBARCODE", SqlDbType.VarChar);
                objParameters[2] = new SqlParameter("@USERNAME", SqlDbType.VarChar);
                objParameters[3] = new SqlParameter("@RESULT", SqlDbType.VarChar, 100);

                objParameters[0].Value = strPackSlip.Trim();
                objParameters[1].Value = strPacketNo.Trim();
                objParameters[2].Value = strUsername.Trim();
                objParameters[3].Direction = ParameterDirection.Output;

                if (objSql.ExecuteProcedureParam(SqlDataLayer.strLocal, "sp_ScanningKITMAT", objParameters, "@RESULT", "@RESULT") != "")
                {
                    return "0~" + objParameters[3].Value.ToString();
                }
                else
                {
                    return "1~" + string.Empty;
                }
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

        #endregion

        #region "PROCESS SETUP"

        public string Validate_Packet(string strWork_Order, string strPacketBarcode)
        {
            SqlDataLayer objSql = new SqlDataLayer();
            try
            {
                return objSql.ExecuteScalar(SqlDataLayer.strLocal, "SELECT COUNT(RECNO) FROM TBLPICKING WHERE WORK_ORDER_NO ='" + strWork_Order.Trim() + "' AND SERIALNO = '" + strPacketBarcode.Trim() + "' AND ST = 2").ToString();
            }
            catch (Exception ex)
            {
                return "1~" + objSql.getErrorMsg(ex.ToString());
            }
            finally
            {
                objSql = null;
            }
        }

        public DataTable getWorkOrder_PS()
        {
            SqlDataLayer objSql = new SqlDataLayer();
            try
            {
                return objSql.ExecuteDataset(SqlDataLayer.strLocal, "SELECT TOP 10 WO_NO FROM TBLWO_INWARD WHERE WO_STATUS = 'Released' ORDER BY WO_NO").Tables[0]; //SELECT DISTINCT(WO_NO) FROM TBLWO_INWARD WHERE WO_STATUS = 'Released'
            }
            catch (Exception ex)
            {
              //  return "1~" + objSql.getErrorMsg(ex.ToString());
                throw ex;
            }
            finally
            {
                objSql = null;
            }
        }

        public DataTable getWorkOrder_Details_PS(string strWorkOrder)
        {
            SqlDataLayer objSql = new SqlDataLayer();
            try
            {
                return objSql.ExecuteDataset(SqlDataLayer.strLocal, "SELECT DISTINCT ASSY_ITEM_CODE, SER FROM TBLWO_INWARD WHERE WO_NO = '" + strWorkOrder.Trim() + "' AND WO_STATUS = 'Released'").Tables[0];
            }
            catch (Exception ex)
            {
                //return "1~" + objSql.getErrorMsg(ex.ToString());
                throw ex;
            }
            finally
            {
                objSql = null;
            }
        }

        public DataTable getWorkOrder_getProdType(string strWorkOrder)
        {
            SqlDataLayer objSql = new SqlDataLayer();
            try
            {
                return objSql.ExecuteDataset(SqlDataLayer.strLocal, "SELECT DISTINCT PRODTYPE FROM TBLPROCESSCARD WHERE WO_NO = '" + strWorkOrder.Trim() + "' AND WO_STATUS = 'Released'").Tables[0];
            }
            catch (Exception ex)
            {
               // return "1~" + objSql.getErrorMsg(ex.ToString());
                throw ex;
            }
            finally
            {
                objSql = null;
            }
        }

        public string InsertProcessSetup(string strWorkOrder, string strProduction, string strStage, string strSetupTime, DataTable dt_Serial, DataTable dt_Tools, string strUserID, string strPlant)
        {
            SqlDataLayer objSql = new SqlDataLayer();
            SqlParameter[] objParameters = new SqlParameter[14];
            try
            {
                objParameters[0] = new SqlParameter("@TEMP", SqlDbType.Structured);
                objParameters[1] = new SqlParameter("@TEMP_SER", SqlDbType.Structured);
                objParameters[2] = new SqlParameter("@WONO", SqlDbType.VarChar);
                objParameters[3] = new SqlParameter("@PARTNO", SqlDbType.VarChar);
                objParameters[4] = new SqlParameter("@DESCRIPTION", SqlDbType.VarChar);
                objParameters[5] = new SqlParameter("@PRODTYPE", SqlDbType.VarChar);
                objParameters[6] = new SqlParameter("@STAGE", SqlDbType.VarChar);
                objParameters[7] = new SqlParameter("@STARTDATE", SqlDbType.VarChar);
                objParameters[8] = new SqlParameter("@ENDDATE", SqlDbType.VarChar);
                objParameters[9] = new SqlParameter("@STATUS", SqlDbType.Int);
                objParameters[10] = new SqlParameter("@PLANTNO", SqlDbType.VarChar);
                objParameters[11] = new SqlParameter("@USERNAME", SqlDbType.VarChar);
                objParameters[12] = new SqlParameter("@SETUPTIME", SqlDbType.VarChar);
                objParameters[13] = new SqlParameter("@RESULT", SqlDbType.VarChar, 500);

                objParameters[0].Value = dt_Tools;
                objParameters[1].Value = dt_Serial;
                objParameters[2].Value = strWorkOrder.Trim();
                objParameters[3].Value = "";
                objParameters[4].Value = "";
                objParameters[5].Value = strProduction.Trim();
                objParameters[6].Value = strStage.Trim();
                objParameters[7].Value = DateTime.Now;
                objParameters[8].Value = DateTime.Now;
                objParameters[9].Value = 0;
                objParameters[10].Value = strPlant;
                objParameters[11].Value = strUserID;
                objParameters[12].Value = strSetupTime;
                objParameters[13].Direction = ParameterDirection.Output;

                if (objSql.ExecuteProcedureParam(SqlDataLayer.strLocal, "sp_ProcessSetup", objParameters, "@RESULT", "@RESULT") != "")
                {
                    return  objParameters[13].Value.ToString();
                }
                else
                {
                    return "1~" + "";
                }
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

        #endregion

        #region "COMPONENT SCANNING"

        public string Validate_Tools(string strProcessType, string strProcessStage, string strAssetCode)
        {
            SqlDataLayer objSql = new SqlDataLayer();
            try
            {
                int intResult = objSql.ExecuteScalar(SqlDataLayer.strLocal, "SELECT DBO.VALIDATE_CALIBRATION('" + strAssetCode.Trim() + "')");

                if (intResult == 0)
                {
                    return "1~" + "Calibration due date expired.";
                }
                else if (intResult == 1)
                {
                    return "0~" + objSql.ExecuteScalarString(SqlDataLayer.strLocal, "SELECT dbo.TBLASSET.ASSETCODE + '=' + (SELECT dbo.TBLCATEGORY.CATNM FROM dbo.TBLCATEGORY INNER JOIN dbo.TBLASSET ON dbo.TBLCATEGORY.RECNO = dbo.TBLASSET.CATEGORY WHERE dbo.TBLCATEGORY.RECNO = dbo.TBLASSET.CATEGORY) FROM dbo.TBLASSET INNER JOIN dbo.TBLSTOCK ON dbo.TBLASSET.RECNO = dbo.TBLSTOCK.ASSETCODE WHERE DBO.TBLSTOCK.BARCODE = '" + strAssetCode.Trim() + "'");
                }
                else
                {
                    return "1~" + "Asset not found.";
                }

                //return "0~" + objSql.ExecuteScalarString(SqlDataLayer.strLocal, "SELECT CAST(COUNT(ASSETCODE) AS VARCHAR(50)) AS ASSETCODE FROM TBLTOOL_ASSIGN WHERE DBO.FNSPLIT(ASSETCODE , '=', 1) = '" + strAssetCode.Trim() + "' AND PRODUCTTYPE = '" + strProcessType.Trim() + "' AND PROCESS = '" + strProcessStage.Trim() + "'");
            }
            catch (Exception ex)
            {
                return "1~" + objSql.getErrorMsg(ex.ToString());
            }
            finally
            {
                objSql = null;
            }
        }

        public string InsertCompScan(string strWorkOrder, string strPartNo, string strDescription, DataTable dtPackage, string strUserID, string strPlant)
        {
            SqlDataLayer objSql = new SqlDataLayer();
            SqlParameter[] objParameters = new SqlParameter[9];
            try
            {
                objParameters[0] = new SqlParameter("@TEMP", SqlDbType.Structured);
                objParameters[1] = new SqlParameter("@WONO", SqlDbType.VarChar);
                objParameters[2] = new SqlParameter("@PARTNO", SqlDbType.VarChar);
                objParameters[3] = new SqlParameter("@DESCRIPTION", SqlDbType.VarChar);
                objParameters[4] = new SqlParameter("@REMARK", SqlDbType.VarChar);
                objParameters[5] = new SqlParameter("@STATUS", SqlDbType.Int);
                objParameters[6] = new SqlParameter("@PLANTNO", SqlDbType.VarChar);
                objParameters[7] = new SqlParameter("@USERNAME", SqlDbType.VarChar);
                objParameters[8] = new SqlParameter("@RESULT", SqlDbType.VarChar, 500);

                objParameters[0].Value = dtPackage;
                objParameters[1].Value = strWorkOrder.Trim();
                objParameters[2].Value = strPartNo.Trim();
                objParameters[3].Value = strDescription.Trim();
                objParameters[4].Value = "";
                objParameters[5].Value = 0;
                objParameters[6].Value = strPlant.Trim();
                objParameters[7].Value = strUserID.Trim();
                objParameters[8].Direction = ParameterDirection.Output;

                if (objSql.ExecuteProcedureParam(SqlDataLayer.strLocal, "sp_CompScanning", objParameters, "@RESULT", "@RESULT") != "")
                {
                    return  objParameters[8].Value.ToString();
                }
                else
                {
                    return "1~" + string.Empty;
                }
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

        #endregion

        #region "PRODUCTION SCANNING"

        public string Validate_WIP(string strBarcode, string strProcessCard, string strFlag)
        {
            SqlDataLayer objSql = new SqlDataLayer();
            try
            {
                if (strFlag == "ProcessCard")
                {
                    return objSql.ExecuteScalar(SqlDataLayer.strLocal, "SELECT COUNT(BARCODE) FROM TBLPROCESSCARD WHERE BARCODE = '" + strProcessCard.Trim() + "'").ToString();
                }
                else if (strFlag == "Serial")
                {
                    string strWo_No = objSql.ExecuteScalarString(SqlDataLayer.strLocal, "SELECT WONO FROM TBLPROCESSCARD WHERE BARCODE = '" + strBarcode.Trim() + "'");
                    return objSql.ExecuteScalar(SqlDataLayer.strLocal, "SELECT COUNT(SER) FROM TBLWO_INWARD WHERE WO_NO= '" + strWo_No.Trim() + "' AND SER = '" + strBarcode.Trim() + "' AND PRINTST = 1").ToString();
                }
                else
                {
                    return "0~" + objSql.ExecuteScalarString(SqlDataLayer.strLocal, "SELECT COUNT(BARCODE) FROM TBLSTOCK WHERE BARCODE = '" + strBarcode.Trim() + "'");
                }
            }
            catch (Exception ex)
            {
                return "1~" + objSql.getErrorMsg(ex.ToString());
            }
            finally
            {
                objSql = null;
            }
        }

        public DataTable Get_Processes(string strProcessCard, string strUsername)
        {
            SqlDataLayer objSql = new SqlDataLayer();
            try
            {
                if (objSql.ExecuteScalar(SqlDataLayer.strLocal, "SELECT COUNT(BARCODE) FROM TBLPROCESSCARD WHERE BARCODE = '" + strProcessCard.Trim() + "'") != 0)
                {
                    return objSql.ExecuteDataset(SqlDataLayer.strLocal, "SELECT DISTINCT PROCESSSTAGE FROM TBLPROCESSCARD_SUBITEM INNER JOIN TBLPROCESS_ASSIGN ON TBLPROCESSCARD_SUBITEM.PROCESSSTAGE = TBLPROCESS_ASSIGN.PROCESS WHERE DBO.FNSPLIT(TBLPROCESS_ASSIGN.OPERATOR, '=', 1) = '" + strUsername.Trim() + "' AND TBLPROCESSCARD_SUBITEM.BARCODE = '" + strProcessCard.Trim() + "' AND TBLPROCESSCARD_SUBITEM.STATUS = 0").Tables[0];
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("PROCESSSTAGE");
                    DataRow dtrow = dt.NewRow();
                    dtrow["PROCESSSTAGE"] = "Process card barcode not exist.";
                    dt.Rows.Add(dtrow);

                    return dt;
                }
            }
            catch (Exception ex)
            {
                //return "1~" + objSql.getErrorMsg(ex.ToString());
                throw ex;
            }
            finally
            {
                objSql = null;
            }
        }

        public DataTable GetmyProcessWIP(string strSerial_barcode, string strUsername)
        {
            SqlDataLayer objSql = new SqlDataLayer();
            DataTable dt_Process = new DataTable();
            DataTable dt_Final = new DataTable();
            string strProcessCard = "";
            try
            {
                strProcessCard = objSql.ExecuteScalarString(SqlDataLayer.strLocal, "SELECT PROCESSCARD FROM TBLWO_INWARD WHERE TRACKING_ID = '" + strSerial_barcode.Trim() + "'");

                if (Convert.ToInt32(objSql.ExecuteScalarString(SqlDataLayer.strLocal, "SELECT CAST(COUNT(*) AS VARCHAR(50)) FROM TBLWO_INWARD WHERE PROCESSCARD = '" + strProcessCard.Trim() + "' AND TRACKING_ID = '" + strSerial_barcode.Trim() + "'")) == 0)
                {
                    throw new Exception("Invalid Process card or serial number scanned.");
                }

                dt_Process = objSql.ExecuteDataset(SqlDataLayer.strLocal, "SELECT DISTINCT PROCESSSTAGE, AUTONUM FROM VW_PRODSTAGES WHERE BARCODE = '" + strProcessCard.Trim() + "' AND STATUS = 0 ORDER BY AUTONUM ASC").Tables[0]; //UID = '" + clsInit.GstrUserID.Trim() + "' AND 

                dt_Process.Columns.Add("Rework", typeof(string));

                foreach (DataRow dr in dt_Process.Rows)
                {
                    string strReworkFlag = string.Empty;
                    strReworkFlag = objSql.ExecuteScalarString(SqlDataLayer.strLocal, "SELECT CAST(RECNO AS VARCHAR(50)) FROM TBLREWORK WHERE WO_NO = '" + strProcessCard.Split('-').GetValue(0).ToString() + "' AND SER = '" + strSerial_barcode.Trim() + "' AND PROCESS = '" + dr["PROCESSSTAGE"].ToString() + "'  and Stage_Ref = " + dr["AUTONUM"] + " AND ST = 0;");
                    if (strReworkFlag.Trim() == "")
                    {
                        dr["Rework"] = "0";
                    }
                    else
                    {
                        dr["Rework"] = strReworkFlag; ;
                    }
                }

                dt_Final.Columns.Add("Ref", typeof(int));
                dt_Final.Columns.Add("ProcessStage", typeof(string));
                dt_Final.Columns.Add("Rework", typeof(string));

                foreach (DataRow dr in dt_Process.Rows)
                {
                    if (dr["Rework"].ToString() == "0")
                    {
                        if (Convert.ToInt32(objSql.ExecuteScalarString(SqlDataLayer.strLocal, "SELECT CAST(COUNT(RECNO) AS VARCHAR(50)) FROM TBLPRODUCTION WHERE BARCODE = '" + strProcessCard.Trim() + "' AND TRACKING_ID = '" + strSerial_barcode.Trim() + "' AND PROCESSDESC = '" + dr["PROCESSSTAGE"].ToString() + "' AND CRUSER = '" + strUsername.Trim() + "' AND ST = 2 and Stage_Ref = " + dr["AUTONUM"] + "")) == 0)
                        {
                            DataRow dr_Final = dt_Final.NewRow();
                            dr_Final["Ref"] = dr["AUTONUM"];
                            dr_Final["ProcessStage"] = dr["ProcessStage"];
                            dr_Final["Rework"] = dr["Rework"];
                            dt_Final.Rows.Add(dr_Final);
                            dt_Final.AcceptChanges();
                        }
                    }
                    else
                    {
                        DataRow dr_Final = dt_Final.NewRow();
                        dr_Final["Ref"] = dr["AUTONUM"];
                        dr_Final["ProcessStage"] = dr["ProcessStage"];
                        dr_Final["Rework"] = dr["Rework"];
                        dt_Final.Rows.Add(dr_Final);
                        dt_Final.AcceptChanges();
                    }
                }
                return dt_Final;
            }
            catch (Exception ex)
            {
                //return "1~" + objSql.getErrorMsg(ex.ToString());
                throw ex;
            }
            finally
            {
                objSql = null;
            }
        }

        public string InsertProduction(string strProcessBarcode, string strProcess, string strSerialno, DataTable dtCompBarcode, DataTable dtSerBarcode, string strRemark, string strMode, int intRework, string strUserID, string strPlant, int intRef)
        {
            SqlDataLayer objSql = new SqlDataLayer();
            SqlParameter[] objParameters = new SqlParameter[12];
            try
            {
                objParameters[0] = new SqlParameter("@BARCODE", SqlDbType.VarChar);
                objParameters[1] = new SqlParameter("@PROCESSDESC", SqlDbType.VarChar);
                objParameters[2] = new SqlParameter("@TRACKING_ID", SqlDbType.VarChar);
                objParameters[3] = new SqlParameter("@COMPBARCODE", SqlDbType.Structured);
                objParameters[4] = new SqlParameter("@SERBARCODE", SqlDbType.Structured);
                objParameters[5] = new SqlParameter("@REMARK", SqlDbType.VarChar);
                objParameters[6] = new SqlParameter("@MODE", SqlDbType.VarChar);
                objParameters[7] = new SqlParameter("@PLANTNO", SqlDbType.VarChar);
                objParameters[8] = new SqlParameter("@USERNAME", SqlDbType.VarChar);
                objParameters[9] = new SqlParameter("@REWORK", SqlDbType.Int);
                objParameters[10] = new SqlParameter("@STAGE_REF", SqlDbType.Int);
                objParameters[11] = new SqlParameter("@RESULT", SqlDbType.VarChar, 500);

                objParameters[0].Value = strProcessBarcode.Trim();
                objParameters[1].Value = strProcess.Trim();
                objParameters[2].Value = strSerialno.Trim();
                objParameters[3].Value = dtCompBarcode;
                objParameters[4].Value = dtSerBarcode;
                objParameters[5].Value = strRemark.Trim();
                objParameters[6].Value = strMode.Trim();
                objParameters[7].Value = strPlant;
                objParameters[8].Value = strUserID;
                objParameters[9].Value = intRework;
                objParameters[10].Value = intRef;
                objParameters[11].Direction = ParameterDirection.Output;

                if (objSql.ExecuteProcedureParam(SqlDataLayer.strLocal, "sp_WIPScanning", objParameters, "@RESULT", "@RESULT") != "")
                {
                    return  objParameters[11].Value.ToString();
                }
                else
                {
                    return "1~" + string.Empty;
                }
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

        #endregion

        #region "REWORK"

        public string Save_Rework(DataTable dtTemp, string strWorkOrder, string strSerialNumber, string strRemark, int intStatus, string strPlant, string strUsername)
        {
            SqlDataLayer objSql = new SqlDataLayer();
            SqlParameter[] objParameters = new SqlParameter[8];
            try
            {
                objParameters[0] = new SqlParameter("@TEMP", SqlDbType.Structured);
                objParameters[1] = new SqlParameter("@WONO", SqlDbType.VarChar);
                objParameters[2] = new SqlParameter("@SER", SqlDbType.VarChar);
                objParameters[3] = new SqlParameter("@REMARK", SqlDbType.VarChar);
                objParameters[4] = new SqlParameter("@PLANTNO", SqlDbType.VarChar);
                objParameters[5] = new SqlParameter("@STATUS", SqlDbType.Int);
                objParameters[6] = new SqlParameter("@USERNAME", SqlDbType.VarChar);
                objParameters[7] = new SqlParameter("@RESULT", SqlDbType.VarChar, 500);

                objParameters[0].Value = dtTemp;
                objParameters[1].Value = strWorkOrder.Trim();
                objParameters[2].Value = strSerialNumber.Trim();
                objParameters[3].Value = strRemark.Trim();
                objParameters[4].Value = strPlant;
                objParameters[5].Value = 0;
                objParameters[6].Value = strUsername;
                objParameters[7].Direction = ParameterDirection.Output;

                if (objSql.ExecuteProcedureParam(SqlDataLayer.strLocal, "sp_Rework", objParameters, "@RESULT", "@RESULT") != "")
                {
                    return  objParameters[7].Value.ToString();
                }
                else
                {
                    return "1~" + string.Empty;
                }
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

        #endregion

        //public string InsertPhysicalCount(string strBarcode, string strDescription, string strSlNo, DateTime CALIBRATED_ON, string strREMARK, string strCRUSER, string strPLANT)
        //{
        //    SqlDataLayer objSql = new SqlDataLayer();
        //    SqlParameter[] objParameters = new SqlParameter[8];
        //    try
        //    {
        //        objParameters[0] = new SqlParameter("@BARCODE", SqlDbType.VarChar, 50);
        //        objParameters[1] = new SqlParameter("@DESCRIPTION", SqlDbType.VarChar, 1000);
        //        objParameters[2] = new SqlParameter("@SLNO", SqlDbType.VarChar, 200);
        //        objParameters[3] = new SqlParameter("@CALIBRATED_ON", SqlDbType.DateTime);
        //        objParameters[4] = new SqlParameter("@REMARK", SqlDbType.VarChar, 1000);
        //        objParameters[5] = new SqlParameter("@CRUSER", SqlDbType.VarChar, 50);
        //        objParameters[6] = new SqlParameter("@PLANT", SqlDbType.VarChar, 50);
        //        objParameters[7] = new SqlParameter("@RESULT", SqlDbType.VarChar, 500);
        //        objParameters[0].Value = strBarcode;
        //        objParameters[1].Value = strDescription;
        //        objParameters[2].Value = strSlNo;
        //        objParameters[3].Value = CALIBRATED_ON;
        //        objParameters[4].Value = strREMARK;
        //        objParameters[5].Value = strCRUSER;
        //        objParameters[6].Value = strPLANT;
        //        objParameters[7].Direction = ParameterDirection.Output;

        //        if (objSql.ExecuteProcedureParam(SqlDataLayer.strLocal, "sp_Physical_count", objParameters, "@RESULT", "@RESULT") != "")
        //        {
        //            return objParameters[7].Value.ToString();
        //        }
        //        else
        //        {
        //            return "1~" + string.Empty;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return "1~" + objSql.getErrorMsg(ex.ToString());
        //    }
        //    finally
        //    {
        //        objParameters = null;
        //        objSql = null;
        //    }
        //}


        public string InsertPhysicalCount1(DataTable dtTemp)
        {
            SqlDataLayer objSql = new SqlDataLayer();
            SqlParameter[] objParameters = new SqlParameter[2];
            try
            {
                objParameters[0] = new SqlParameter("@TEMP", SqlDbType.Structured);
                objParameters[1] = new SqlParameter("@RESULT", SqlDbType.VarChar, 500);

                objParameters[0].Value = dtTemp;
                objParameters[1].Direction = ParameterDirection.Output;

                if (objSql.ExecuteProcedureParam(SqlDataLayer.strLocal, "sp_Physical_count", objParameters, "@RESULT", "@RESULT") != "")
                {
                    return objParameters[1].Value.ToString();
                }
                else
                {
                    return "1~" + string.Empty;
                }
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

        public string OnlineInsertPhysicalCount(string strBarcode, string strUser)
        {
            SqlDataLayer objSql = new SqlDataLayer();
            SqlParameter[] objParameters = new SqlParameter[3];
            try
            {
                objParameters[0] = new SqlParameter("@BARCODE", SqlDbType.VarChar);
                objParameters[1] = new SqlParameter("@USERNAME", SqlDbType.VarChar);
                objParameters[2] = new SqlParameter("@RESULT", SqlDbType.VarChar, 500);

                objParameters[0].Value = strBarcode.Trim();
                objParameters[1].Value = strUser.Trim();
                objParameters[2].Direction = ParameterDirection.Output;

                if (objSql.ExecuteProcedureParam(SqlDataLayer.strLocal, "sp_Physical_count", objParameters, "@RESULT", "@RESULT") != "")
                {
                    return objParameters[2].Value.ToString();
                }
                else
                {
                    return "1~" + string.Empty;
                }
                //DataTable dt = objSql.ExecuteProcedureParam(SqlDataLayer.strLocal, "sp_Physical_count", objParameters);
                //return dt;
            }
            catch (Exception ex)
            {
                throw ex;
               // return "1~" + objSql.getErrorMsg(ex.ToString());
            }
            finally
            {
                objParameters = null;
                objSql = null;
            }
        }

        public string OfflineInsertPhysicalCount(DataTable dtTemp)
        {
            SqlDataLayer objSql = new SqlDataLayer();
            SqlParameter[] objParameters = new SqlParameter[2];
            try
            {
                objParameters[0] = new SqlParameter("@TEMP", SqlDbType.Structured);
                objParameters[1] = new SqlParameter("@RESULT", SqlDbType.VarChar, 500);

                objParameters[0].Value = dtTemp;
                objParameters[1].Direction = ParameterDirection.Output;

                if (objSql.ExecuteProcedureParam(SqlDataLayer.strLocal, "sp_Offline_Physical_count", objParameters, "@RESULT", "@RESULT") != "")
                {
                    return objParameters[1].Value.ToString();
                }
                else
                {
                    return "1~" + string.Empty;
                }
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

        public string DeletePhysicalCount(string strBarcode, string strUser)
        {
            SqlDataLayer objSql = new SqlDataLayer();
            SqlParameter[] objParameters = new SqlParameter[3];
            try
            {
                objParameters[0] = new SqlParameter("@BARCODE", SqlDbType.VarChar);
                objParameters[1] = new SqlParameter("@USERNAME", SqlDbType.VarChar);
                objParameters[2] = new SqlParameter("@RESULT", SqlDbType.VarChar, 500);

                objParameters[0].Value = strBarcode.Trim();
                objParameters[1].Value = strUser.Trim();
                objParameters[2].Direction = ParameterDirection.Output;

                if (objSql.ExecuteProcedureParam(SqlDataLayer.strLocal, "sp_Delete_Physical_count", objParameters, "@RESULT", "@RESULT") != "")
                {
                    return objParameters[2].Value.ToString();
                }
                else
                {
                    return "1~" + string.Empty;
                }
                //DataTable dt = objSql.ExecuteProcedureParam(SqlDataLayer.strLocal, "sp_Physical_count", objParameters);
                //return dt;
            }
            catch (Exception ex)
            {
                throw ex;
                // return "1~" + objSql.getErrorMsg(ex.ToString());
            }
            finally
            {
                objParameters = null;
                objSql = null;
            }
        }

    }
}

