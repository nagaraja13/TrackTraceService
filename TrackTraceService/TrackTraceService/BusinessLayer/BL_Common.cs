using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Reflection;
using Newtonsoft.Json;


namespace BusinessLayer
{

    public class BL_Common
    {
        public string BL_AuthenticateLogin(string strUsername, string strPassword)
        {
            SqlDataLayer objSql = new SqlDataLayer();
            SqlParameter[] objParameters = new SqlParameter[3];
            try
            {
                objParameters[0] = new SqlParameter("@USERNAME", SqlDbType.VarChar);
                objParameters[1] = new SqlParameter("@PASSWORD", SqlDbType.VarChar);
                objParameters[2] = new SqlParameter("@RESULT", SqlDbType.VarChar, 100);

                objParameters[0].Value = strUsername.Trim();
                objParameters[1].Value = strPassword.Trim();
                objParameters[2].Direction = ParameterDirection.Output;

                if (objSql.ExecuteProcedureParam(SqlDataLayer.strLocal, "sp_Login", objParameters, "@RESULT", "@RESULT") != "")
                {
                    BL_Common.BL_Write(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, 2, "Login Succesfully", strUsername);
                    return "0~" + objParameters[2].Value.ToString();
                }
                else
                {
                    BL_Common.BL_Write(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, 0, "Login Failed", strUsername);
                    return "1~Invalid login details";
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

        public string getTable(string strQuery, string strUsername)
        {
            SqlDataLayer objSql = new SqlDataLayer();
            try
            {
                return  BL_Common.dataTableToString1(objSql.ExecuteDataset(SqlDataLayer.strLocal, strQuery).Tables[0]);
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

        public DataTable getTable1(string strQuery, string strUsername)
        {
            SqlDataLayer objSql = new SqlDataLayer();
            try
            {
                return objSql.ExecuteDataset(SqlDataLayer.strLocal, strQuery).Tables[0];
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

        public string getField(string strQuery, string strUsername)
        {
            SqlDataLayer objSql = new SqlDataLayer();
            try
            {
                return objSql.ExecuteScalarString(SqlDataLayer.strLocal, strQuery);
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

        public string BL_GetMyRights(string strUserID)
        {
            SqlDataLayer objSql = new SqlDataLayer();
            string strResult = string.Empty;
            try
            {
                strResult = objSql.ExecuteScalarString(SqlDataLayer.strLocal, "SELECT RGHTS FROM TBLUSER WHERE UID = '" + strUserID.Trim() + "'");

                if (strResult.Trim() != string.Empty)
                {
                    BL_Common.BL_Write(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, 2, "Fetching user rights successfully", strUserID);
                    return strResult.Trim();
                }
                else
                {
                    BL_Common.BL_Write(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, 0, "Fetching user rights failed.", strUserID);
                    return "1~Do not have device operational rights.";
                }
            }
            catch (Exception ex)
            {
                BL_Common.BL_Write(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, 0, ex.ToString(), strUserID);
                return "1~" + objSql.getErrorMsg(ex.ToString());
            }
            finally
            {
                objSql = null;
            }
        }

        public string BL_GetMyRights1(string strUserID)
        {
            SqlDataLayer objSql = new SqlDataLayer();
            string strResult = string.Empty;
            try
            {
                strResult = objSql.ExecuteScalarString(SqlDataLayer.strLocal, "SELECT UTYPE FROM TBLUSER WHERE UID = '" + strUserID.Trim() + "'");

                if (strResult.Trim() != string.Empty)
                {
                    BL_Common.BL_Write(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, 2, "Fetching user rights successfully", strUserID);
                    return strResult.Trim();
                }
                else
                {
                    BL_Common.BL_Write(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, 0, "Fetching user rights failed.", strUserID);
                    return "1~Do not have device operational rights.";
                }
            }
            catch (Exception ex)
            {
                BL_Common.BL_Write(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, 0, ex.ToString(), strUserID);
                return "1~" + objSql.getErrorMsg(ex.ToString());
            }
            finally
            {
                objSql = null;
            }
        }

        public static DataTable StringToDataTable(string str)
        {
            string[] strArr, strArr1;
            strArr = str.Split('@');
            DataTable dt = new DataTable();
            DataRow drow;
            if (strArr.Length == 0)
                return dt;
            strArr1 = strArr[0].Split('$');
            for (int i = 0; i < strArr1.Length - 1; i++)
                dt.Columns.Add(strArr1[i]);

            for (int j = 1; j < strArr.Length; j++)
            {
                drow = dt.NewRow();
                strArr1 = strArr[j].Split('$');
                for (int i = 0; i < strArr1.Length - 1; i++)
                    drow[i] = strArr1[i];
                dt.Rows.Add(drow);
            }
            dt.AcceptChanges();
            return dt;

        }

        //public static string DataTableToJSONWithJSONNet(DataTable table)
        //{
        //    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
        //    string JSONString = string.Empty;
        //    //JSONString = JsonConvert.SerializeObject( table);
        //    JSONString = ser.Serialize(table);
        //    return JSONString;
        //}

        public static string DataTableToJSONWithJSONNet(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }

        public static string DataTableToJSONWithStringBuilder(DataTable table)
        {
            var JSONString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            return JSONString.ToString();
        }

        public static string dataTableToString(DataTable dt)
        {
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < dt.Columns.Count; i++)
                str.Append(Convert.ToString(dt.Columns[i]) + "$");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str.Append("@");
                for (int j = 0; j < dt.Columns.Count; j++)
                    str.Append(Convert.ToString(dt.Rows[i][j]) + "$");
            }
            //return "0~" + str + "~" + dt.Rows.Count;
            return str.ToString();
        }

        public static string dataTableToString1(DataTable dt)
        {
            StringBuilder str = new StringBuilder();
           

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                    str.Append(Convert.ToString(dt.Rows[i][j]) + ",");
            }
            //return "0~" + str + "~" + dt.Rows.Count;
            return str.ToString();
        }

        public static string dataTableToStringWH(DataTable dt)
        {
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < dt.Columns.Count; i++)
                str.Append(Convert.ToString(dt.Columns[i]) + "$");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str.Append("@");
                for (int j = 0; j < dt.Columns.Count; j++)
                    str.Append(Convert.ToString(dt.Rows[i][j]) + "$");
            }
            return str + "~" + dt.Rows.Count;
        }

        public static int BL_Write(string strModule, string strMethod, int intType, string strDetails, string strUser)
        {
            if (strDetails != "")
            {
                string strErrorType;

                switch (intType)
                {
                    case 0:
                        strErrorType = "Error";
                        break;
                    case 1:
                        strErrorType = "Worning";
                        break;
                    case 2:
                        strErrorType = "Success";
                        break;
                    default:
                        strErrorType = "Error";
                        break;
                }

                string strPlantCode = string.Empty;

                if (strUser.Trim() == null)
                {
                    strPlantCode = "'-'";
                }
                else
                {
                    strPlantCode = "(SELECT TOP(1) PLANT FROM TBLUSER WHERE UID = '" + strUser.Trim() + "')";
                }

                SqlDataLayer objSql = new SqlDataLayer();
                string strQuery = string.Empty;
                try
                {
                    if (strDetails.Contains("This SqlTransaction has completed") == false)
                    {
                        strQuery = "INSERT INTO TBLEVENTS (MODULE, METHOD, TYPE, DETAILS, CRDATE, CRUSER, PLANT, PROGRAM) VALUES (" +
                                   "'" + strModule.Trim() + "', '" + strMethod.Trim() + "', " +
                                   "'" + strErrorType.Trim() + "', '" + strDetails.Replace("System.Exception:", "").Trim() + "', GETDATE(), '" + strUser.Trim() + "'," + strPlantCode + ", 'RF SERVER')";
                        return objSql.ExecuteNonQuery(SqlDataLayer.strLocal, strQuery);
                    }
                }
                catch //(Exception ex)
                {
                    //throw new Exception(ex.ToString());
                }
                finally
                {
                    strQuery = string.Empty;
                    objSql = null;
                }
            }
            return 1;
        }
    }
}

