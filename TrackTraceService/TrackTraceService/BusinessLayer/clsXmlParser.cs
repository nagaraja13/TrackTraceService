using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Xml;
using System.Data;
using System.IO;

namespace BusinessLayer
{
    public class clsXmlParser
    {
        public bool Consume_Xml_Structure(string strFilename)
        {
            XmlDocument doc = new XmlDocument();
            System.IO.StreamReader sr = new System.IO.StreamReader(strFilename);
            XmlNodeList CollaborationContext;
            XmlNodeList Stations;
            XmlNodeList Station;
            XmlNodeList Track;
            XmlNodeList Processes;
            XmlNodeList ProcessClassList;

            StringWriter sw = new StringWriter();
            while (!sr.EndOfStream)
            {
                string strLine = sr.ReadLine();
                if (strLine.StartsWith("<") == true && strLine.EndsWith(">") == true)
                {
                    sw.WriteLine(strLine);
                }
            }

            DataTable @TEMP_TEMPCC = new DataTable();
            DataTable @TEMP_TEMPCC_CNFG = new DataTable();
            DataTable @TEMP_TEMPCC_OPERATIONS = new DataTable();
            DataTable @TEMP_TEMPCC_RESOURCES = new DataTable();
            DataTable @TEMP_TEMPCC_STATIONS = new DataTable();
            DataTable @TEMP_TEMPCC_TOOLS_CONSUMABLES = new DataTable();
            DataTable @TEMP_TEMPCC_PROCESSES_PROCESS = new DataTable();
            DataTable @TEMP_TEMPCC_DOCUMENTS = new DataTable();
            DataTable @TEMP_APPLPROCESSCLASSLIST = new DataTable();
            DataTable @TEMP_SUCCESSORS = new DataTable();
            DataTable @TEMP_PREDECESSORS = new DataTable();

            string strCCid = string.Empty;
            string strStation = string.Empty;
            string strStation_name = string.Empty;
            string strOperation = string.Empty;
            string ProcessRoot = string.Empty;
            string EBOMRoot = string.Empty;
            string MBOMRoot = string.Empty;
            string PlantBOMRoot = string.Empty;
            string strAsmblName = "";

            string strSEQ = string.Empty;
            string strOperation_for = string.Empty;

            System.Data.DataSet ds = new DataSet();
            SqlDataLayer objSql = new SqlDataLayer();
            SqlParameter[] objParameters = new SqlParameter[13];
            try
            {
                #region Collebration Context Header Tables
                @TEMP_TEMPCC.Columns.Add("CC_ID", typeof(string));
                @TEMP_TEMPCC.Columns.Add("REVRULE", typeof(string));
                @TEMP_TEMPCC.Columns.Add("VARRULE", typeof(string));
                @TEMP_TEMPCC.Columns.Add("FILENAME", typeof(string));

                @TEMP_TEMPCC_CNFG.Columns.Add("CC_ID", typeof(string));
                @TEMP_TEMPCC_CNFG.Columns.Add("PROCESSROOT_ID", typeof(string));
                @TEMP_TEMPCC_CNFG.Columns.Add("PROCESSROOT_NAME", typeof(string));
                @TEMP_TEMPCC_CNFG.Columns.Add("PROCESSROOT_REVISION", typeof(string));
                @TEMP_TEMPCC_CNFG.Columns.Add("EBOMROOT_ID", typeof(string));
                @TEMP_TEMPCC_CNFG.Columns.Add("EBOMROOT_NAME", typeof(string));
                @TEMP_TEMPCC_CNFG.Columns.Add("EBOMROOT_REVISION", typeof(string));
                @TEMP_TEMPCC_CNFG.Columns.Add("MBOMROOT_ID", typeof(string));
                @TEMP_TEMPCC_CNFG.Columns.Add("MBOMROOT_NAME", typeof(string));
                @TEMP_TEMPCC_CNFG.Columns.Add("MBOMROOT_REVISION", typeof(string));

                @TEMP_TEMPCC_STATIONS.Columns.Add("CC_ID", typeof(string));
                @TEMP_TEMPCC_STATIONS.Columns.Add("STATION_ID", typeof(string));
                @TEMP_TEMPCC_STATIONS.Columns.Add("STATION_NAME", typeof(string));
                @TEMP_TEMPCC_STATIONS.Columns.Add("STATION_REVISION", typeof(string));

                @TEMP_TEMPCC_OPERATIONS.Columns.Add("CC_ID", typeof(string));
                @TEMP_TEMPCC_OPERATIONS.Columns.Add("STATION_ID", typeof(string));
                @TEMP_TEMPCC_OPERATIONS.Columns.Add("ID", typeof(string));
                @TEMP_TEMPCC_OPERATIONS.Columns.Add("NAME", typeof(string));
                @TEMP_TEMPCC_OPERATIONS.Columns.Add("REVISION", typeof(string));
                @TEMP_TEMPCC_OPERATIONS.Columns.Add("SEQ", typeof(string));
                @TEMP_TEMPCC_OPERATIONS.Columns.Add("URL", typeof(string));
                @TEMP_TEMPCC_OPERATIONS.Columns.Add("OCCID", typeof(string));
                @TEMP_TEMPCC_OPERATIONS.Columns.Add("OPERATIONSETUPREQUIRED", typeof(string));

                @TEMP_TEMPCC_TOOLS_CONSUMABLES.Columns.Add("CC_ID", typeof(string));
                @TEMP_TEMPCC_TOOLS_CONSUMABLES.Columns.Add("STATION_ID", typeof(string));
                @TEMP_TEMPCC_TOOLS_CONSUMABLES.Columns.Add("SEQ", typeof(string));
                @TEMP_TEMPCC_TOOLS_CONSUMABLES.Columns.Add("OP_ID", typeof(string));
                @TEMP_TEMPCC_TOOLS_CONSUMABLES.Columns.Add("TYPE", typeof(string));
                @TEMP_TEMPCC_TOOLS_CONSUMABLES.Columns.Add("ID", typeof(string));
                @TEMP_TEMPCC_TOOLS_CONSUMABLES.Columns.Add("NAME", typeof(string));
                @TEMP_TEMPCC_TOOLS_CONSUMABLES.Columns.Add("QUANTITY", typeof(string));
                @TEMP_TEMPCC_TOOLS_CONSUMABLES.Columns.Add("REVISION", typeof(string));

                @TEMP_TEMPCC_RESOURCES.Columns.Add("CC_ID", typeof(string));
                @TEMP_TEMPCC_RESOURCES.Columns.Add("STATION_ID", typeof(string));
                @TEMP_TEMPCC_RESOURCES.Columns.Add("OP_ID", typeof(string));
                @TEMP_TEMPCC_RESOURCES.Columns.Add("ID", typeof(string));
                @TEMP_TEMPCC_RESOURCES.Columns.Add("NAME", typeof(string));
                @TEMP_TEMPCC_RESOURCES.Columns.Add("REVISION", typeof(string));

                @TEMP_SUCCESSORS.Columns.Add("CC_ID", typeof(string));
                @TEMP_SUCCESSORS.Columns.Add("STATION_ID", typeof(string));
                @TEMP_SUCCESSORS.Columns.Add("OP_ID", typeof(string));
                @TEMP_SUCCESSORS.Columns.Add("TYPE", typeof(string));
                @TEMP_SUCCESSORS.Columns.Add("ID", typeof(string));

                @TEMP_PREDECESSORS.Columns.Add("CC_ID", typeof(string));
                @TEMP_PREDECESSORS.Columns.Add("STATION_ID", typeof(string));
                @TEMP_PREDECESSORS.Columns.Add("OP_ID", typeof(string));
                @TEMP_PREDECESSORS.Columns.Add("TYPE", typeof(string));
                @TEMP_PREDECESSORS.Columns.Add("ID", typeof(string));

                #endregion

                #region Process Card Tables
                @TEMP_TEMPCC_PROCESSES_PROCESS.Columns.Add("CC_ID", typeof(string));
                @TEMP_TEMPCC_PROCESSES_PROCESS.Columns.Add("TYPE", typeof(string));
                @TEMP_TEMPCC_PROCESSES_PROCESS.Columns.Add("ID", typeof(string));
                @TEMP_TEMPCC_PROCESSES_PROCESS.Columns.Add("NAME", typeof(string));
                @TEMP_TEMPCC_PROCESSES_PROCESS.Columns.Add("REVISION", typeof(string));

                @TEMP_TEMPCC_DOCUMENTS.Columns.Add("CC_ID", typeof(string));
                @TEMP_TEMPCC_DOCUMENTS.Columns.Add("TYPE", typeof(string));
                @TEMP_TEMPCC_DOCUMENTS.Columns.Add("ID", typeof(string));
                @TEMP_TEMPCC_DOCUMENTS.Columns.Add("NAME", typeof(string));
                @TEMP_TEMPCC_DOCUMENTS.Columns.Add("REVISION", typeof(string));

                @TEMP_APPLPROCESSCLASSLIST.Columns.Add("CC_ID", typeof(string));
                @TEMP_APPLPROCESSCLASSLIST.Columns.Add("CATEGORY", typeof(string));
                @TEMP_APPLPROCESSCLASSLIST.Columns.Add("SUB-CATEGORY", typeof(string));

                #endregion

                doc.LoadXml(sw.ToString());

                CollaborationContext = doc.GetElementsByTagName("CollaborationContext");

                #region Collebration Header Details
                DataRow dr_CNFG = TEMP_TEMPCC_CNFG.NewRow();
                foreach (XmlElement node in CollaborationContext)
                {
                    if (node.Attributes.Count > 0)
                    {
                        if (node.HasAttribute("name"))
                        {
                            strCCid = node.Attributes["name"].InnerXml;
                            dr_CNFG[0] = strCCid;
                            if (string.IsNullOrEmpty(strCCid.Trim()) == true)
                            {
                                throw new Exception("Collaboration Context Number is blank.");
                            }

                            DataRow dr_CCinfo = TEMP_TEMPCC.NewRow();
                            dr_CCinfo[0] = strCCid;
                            try
                            {
                                dr_CCinfo[1] = node.Attributes["revRule"].InnerXml;
                                dr_CCinfo[2] = node.Attributes["varRule"].InnerXml;
                            }
                            catch { }
                            TEMP_TEMPCC.Rows.Add(dr_CCinfo);
                            TEMP_TEMPCC.AcceptChanges();
                        }
                    }
                }

                TEMP_TEMPCC.Rows[0]["FILENAME"] = strFilename;

                foreach (XmlElement node in doc.GetElementsByTagName("ProcessRoot"))
                {
                    if (node.Attributes.Count > 0)
                    {
                        if (node.HasAttribute("id"))
                        {
                            dr_CNFG[1] = node.Attributes["id"].InnerXml;
                            dr_CNFG[2] = node.Attributes["name"].InnerXml;
                            dr_CNFG[3] = node.Attributes["revision"].InnerXml;
                        }
                    }
                }

                foreach (XmlElement node in doc.GetElementsByTagName("EBOMRoot"))
                {
                    if (node.Attributes.Count > 0)
                    {
                        if (node.HasAttribute("id"))
                        {
                            dr_CNFG[4] = node.Attributes["id"].InnerXml;
                            dr_CNFG[5] = node.Attributes["name"].InnerXml;
                            dr_CNFG[6] = node.Attributes["revision"].InnerXml;
                        }
                    }
                }

                foreach (XmlElement node in doc.GetElementsByTagName("MBOMRoot"))
                {
                    if (node.Attributes.Count > 0)
                    {
                        if (node.HasAttribute("id"))
                        {
                            dr_CNFG[7] = node.Attributes["id"].InnerXml;
                            dr_CNFG[8] = node.Attributes["name"].InnerXml;
                            dr_CNFG[9] = node.Attributes["revision"].InnerXml;
                        }
                    }
                }
                TEMP_TEMPCC_CNFG.Rows.Add(dr_CNFG);
                TEMP_TEMPCC_CNFG.AcceptChanges();
                #endregion

                #region Station Details
                Stations = doc.GetElementsByTagName("station");

                foreach (XmlElement node in Stations)
                {
                    if (node.Attributes.Count > 0)
                    {
                        #region Stations
                        if (node.HasAttribute("id"))
                        {
                            strStation = node.Attributes["id"].InnerXml;
                            strStation_name = node.Attributes["name"].InnerXml;
                            DataRow dr_CNFGinfo = TEMP_TEMPCC_STATIONS.NewRow();
                            dr_CNFGinfo["CC_ID"] = strCCid;
                            dr_CNFGinfo["STATION_ID"] = node.Attributes["id"].InnerXml;
                            dr_CNFGinfo["STATION_NAME"] = node.Attributes["name"].InnerXml;
                            dr_CNFGinfo["STATION_REVISION"] = node.Attributes["revision"].InnerXml;
                            TEMP_TEMPCC_STATIONS.Rows.Add(dr_CNFGinfo);
                            TEMP_TEMPCC_STATIONS.AcceptChanges();
                        }
                        #endregion

                        foreach (XmlNode strST in node)
                        {
                            Track = strST.ChildNodes;
                            foreach (XmlNode sTrack in Track)
                            {
                                ds = ConverttYourXmlNodeToDataSet(sTrack);

                                if (ds.Tables.Count == 0)
                                {
                                    throw new Exception("Incomplete BOP. This BOP does not have proper details.");
                                }

                                foreach (DataTable dt_dsTemp in ds.Tables)
                                {
                                    foreach (DataRow dr_dsTemp in dt_dsTemp.Rows)
                                    {
                                        if (dt_dsTemp.TableName.ToUpper() == "OPERATION")
                                        {
                                            DataRow dr_OperationInfo = @TEMP_TEMPCC_OPERATIONS.NewRow();
                                            dr_OperationInfo["CC_ID"] = strCCid;
                                            dr_OperationInfo["STATION_ID"] = strStation;
                                            dr_OperationInfo["ID"] = dr_dsTemp["ID"].ToString();
                                            strOperation_for = dr_dsTemp["ID"].ToString();
                                            dr_OperationInfo["NAME"] = dr_dsTemp["NAME"].ToString();
                                            dr_OperationInfo["REVISION"] = dr_dsTemp["REVISION"].ToString();
                                            dr_OperationInfo["SEQ"] = dr_dsTemp["SEQUENCE"].ToString();
                                            strSEQ = dr_dsTemp["SEQUENCE"].ToString();
                                            dr_OperationInfo["URL"] = dr_dsTemp["URL"].ToString();
                                            dr_OperationInfo["OCCID"] = dr_dsTemp["OCCID"].ToString();
                                            try
                                            {
                                                dr_OperationInfo["OPERATIONSETUPREQUIRED"] = dr_dsTemp["OPERATIONSETUPREQUIRED"].ToString();
                                            }
                                            catch { }
                                            @TEMP_TEMPCC_OPERATIONS.Rows.Add(dr_OperationInfo);
                                            @TEMP_TEMPCC_OPERATIONS.AcceptChanges();
                                        }
                                        if (dt_dsTemp.TableName.ToUpper() == "TOOL")
                                        {
                                            DataRow dr_OperationInfo = TEMP_TEMPCC_TOOLS_CONSUMABLES.NewRow();
                                            dr_OperationInfo["CC_ID"] = strCCid;
                                            dr_OperationInfo["STATION_ID"] = strStation;
                                            dr_OperationInfo["SEQ"] = strSEQ;
                                            dr_OperationInfo["OP_ID"] = strOperation_for;
                                            dr_OperationInfo["TYPE"] = "TOOL";
                                            dr_OperationInfo["ID"] = dr_dsTemp["id"].ToString();
                                            dr_OperationInfo["NAME"] = dr_dsTemp["name"].ToString();
                                            dr_OperationInfo["QUANTITY"] = dr_dsTemp["quantity"].ToString();
                                            dr_OperationInfo["REVISION"] = dr_dsTemp["revision"].ToString();
                                            TEMP_TEMPCC_TOOLS_CONSUMABLES.Rows.Add(dr_OperationInfo);
                                            TEMP_TEMPCC_TOOLS_CONSUMABLES.AcceptChanges();
                                        }
                                        if (dt_dsTemp.TableName.ToUpper() == "WIS")
                                        {
                                            DataRow dr_OperationInfo = TEMP_TEMPCC_TOOLS_CONSUMABLES.NewRow();
                                            dr_OperationInfo["CC_ID"] = strCCid;
                                            dr_OperationInfo["STATION_ID"] = strStation;
                                            dr_OperationInfo["SEQ"] = strSEQ;
                                            dr_OperationInfo["OP_ID"] = strOperation_for;
                                            dr_OperationInfo["TYPE"] = "WIS";
                                            dr_OperationInfo["ID"] = "";
                                            dr_OperationInfo["NAME"] = dr_dsTemp["id"].ToString();
                                            dr_OperationInfo["QUANTITY"] = "";
                                            dr_OperationInfo["REVISION"] = ""; // dr_dsTemp["rev"].ToString();
                                            TEMP_TEMPCC_TOOLS_CONSUMABLES.Rows.Add(dr_OperationInfo);
                                            TEMP_TEMPCC_TOOLS_CONSUMABLES.AcceptChanges();
                                        }
                                        if (dt_dsTemp.TableName.ToUpper() == "CONSUMABLE")
                                        {
                                            DataRow dr_OperationInfo = TEMP_TEMPCC_TOOLS_CONSUMABLES.NewRow();
                                            dr_OperationInfo["CC_ID"] = strCCid;
                                            dr_OperationInfo["STATION_ID"] = strStation;
                                            dr_OperationInfo["SEQ"] = strSEQ;
                                            dr_OperationInfo["OP_ID"] = strOperation_for;
                                            dr_OperationInfo["TYPE"] = "CONSUMABLE";
                                            dr_OperationInfo["ID"] = dr_dsTemp["id"].ToString();
                                            dr_OperationInfo["NAME"] = dr_dsTemp["name"].ToString();
                                            dr_OperationInfo["QUANTITY"] = dr_dsTemp["quantity"].ToString();
                                            dr_OperationInfo["REVISION"] = dr_dsTemp["revision"].ToString();
                                            TEMP_TEMPCC_TOOLS_CONSUMABLES.Rows.Add(dr_OperationInfo);
                                            TEMP_TEMPCC_TOOLS_CONSUMABLES.AcceptChanges();
                                        }
                                        if (dt_dsTemp.TableName.ToUpper() == "ASSIGNEDPART")
                                        {
                                            DataRow dr_OperationInfo = TEMP_TEMPCC_TOOLS_CONSUMABLES.NewRow();
                                            dr_OperationInfo["CC_ID"] = strCCid;
                                            dr_OperationInfo["STATION_ID"] = strStation;
                                            dr_OperationInfo["SEQ"] = strSEQ;
                                            dr_OperationInfo["OP_ID"] = strOperation_for;
                                            dr_OperationInfo["TYPE"] = "ASSIGNEDPART";
                                            dr_OperationInfo["ID"] = dr_dsTemp["id"].ToString();
                                            dr_OperationInfo["NAME"] = dr_dsTemp["name"].ToString();
                                            dr_OperationInfo["QUANTITY"] = dr_dsTemp["quantity"].ToString();
                                            dr_OperationInfo["REVISION"] = dr_dsTemp["revision"].ToString();
                                            TEMP_TEMPCC_TOOLS_CONSUMABLES.Rows.Add(dr_OperationInfo);
                                            TEMP_TEMPCC_TOOLS_CONSUMABLES.AcceptChanges();
                                        }
                                        if (dt_dsTemp.TableName.ToUpper() == "RESOURCE")
                                        {
                                            DataRow dr_OperationInfo = TEMP_TEMPCC_RESOURCES.NewRow();
                                            dr_OperationInfo["CC_ID"] = strCCid;
                                            dr_OperationInfo["STATION_ID"] = strStation;
                                            dr_OperationInfo["OP_ID"] = strOperation_for;
                                            dr_OperationInfo["ID"] = dr_dsTemp["ID"].ToString();
                                            dr_OperationInfo["NAME"] = dr_dsTemp["NAME"].ToString();
                                            dr_OperationInfo["REVISION"] = dr_dsTemp["REVISION"].ToString();
                                            TEMP_TEMPCC_RESOURCES.Rows.Add(dr_OperationInfo);
                                            TEMP_TEMPCC_RESOURCES.AcceptChanges();
                                        }
                                        if (dt_dsTemp.TableName.ToUpper() == "SUCCESSOR")
                                        {
                                            DataRow dr_OperationInfo = TEMP_SUCCESSORS.NewRow();
                                            dr_OperationInfo["CC_ID"] = strCCid;
                                            dr_OperationInfo["STATION_ID"] = strStation;
                                            dr_OperationInfo["OP_ID"] = strOperation_for;
                                            dr_OperationInfo["TYPE"] = "SUCCESSOR";
                                            dr_OperationInfo["ID"] = dr_dsTemp["ID"].ToString();
                                            TEMP_SUCCESSORS.Rows.Add(dr_OperationInfo);
                                            TEMP_SUCCESSORS.AcceptChanges();
                                        }
                                        if (dt_dsTemp.TableName.ToUpper() == "PREDECESSOR")
                                        {
                                            DataRow dr_OperationInfo = TEMP_PREDECESSORS.NewRow();
                                            dr_OperationInfo["CC_ID"] = strCCid;
                                            dr_OperationInfo["STATION_ID"] = strStation;
                                            dr_OperationInfo["OP_ID"] = strOperation_for;
                                            dr_OperationInfo["TYPE"] = "PREDECESSOR";
                                            dr_OperationInfo["ID"] = dr_dsTemp["ID"].ToString();
                                            TEMP_PREDECESSORS.Rows.Add(dr_OperationInfo);
                                            TEMP_PREDECESSORS.AcceptChanges();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Stations not found");
                    }
                }

                #endregion


                #region Processes Details
                Processes = doc.GetElementsByTagName("process");

                foreach (XmlElement node in Processes)
                {
                    if (node.Attributes.Count > 0)
                    {
                        #region Stations
                        if (node.HasAttribute("id"))
                        {
                            strAsmblName = node.Attributes["name"].InnerXml;
                        }
                        #endregion

                        foreach (XmlNode strST in node)
                        {
                            Track = strST.ChildNodes;
                            foreach (XmlNode sTrack in Track)
                            {
                                ds = ConverttYourXmlNodeToDataSet(sTrack);

                                if (ds.Tables.Count == 0)
                                {
                                    throw new Exception("Incomplete BOP. This BOP does not have proper details.");
                                }

                                foreach (DataTable dt_dsTemp in ds.Tables)
                                {
                                    foreach (DataRow dr_dsTemp in dt_dsTemp.Rows)
                                    {
                                        if (dt_dsTemp.TableName.ToUpper() == "DOCUMENT")
                                        {
                                            DataRow dr_OperationInfo = TEMP_TEMPCC_DOCUMENTS.NewRow();
                                            dr_OperationInfo["CC_ID"] = strCCid;
                                            dr_OperationInfo["TYPE"] = "DOCUMENT";
                                            dr_OperationInfo["ID"] = dr_dsTemp["ID"].ToString();
                                            dr_OperationInfo["NAME"] = strAsmblName;
                                            dr_OperationInfo["REVISION"] = dr_dsTemp["revision"].ToString();
                                            TEMP_TEMPCC_DOCUMENTS.Rows.Add(dr_OperationInfo);
                                            TEMP_TEMPCC_DOCUMENTS.AcceptChanges();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Processes not found");
                    }
                }

                #endregion

                #region Process Class Details
                ProcessClassList = doc.GetElementsByTagName("ProcessClass");

                foreach (XmlElement node in ProcessClassList)
                {
                    if (node.Attributes.Count > 0)
                    {
                        #region Stations
                        if (node.HasAttribute("Category"))
                        {
                            DataRow dr_OperationInfo = TEMP_APPLPROCESSCLASSLIST.NewRow();
                            dr_OperationInfo["CC_ID"] = strCCid;
                            dr_OperationInfo["CATEGORY"] = node.Attributes["Category"].InnerXml;
                            dr_OperationInfo["SUB-CATEGORY"] = node.Attributes["Sub-category"].InnerXml;
                            TEMP_APPLPROCESSCLASSLIST.Rows.Add(dr_OperationInfo);
                            TEMP_APPLPROCESSCLASSLIST.AcceptChanges();
                        }
                        #endregion
                    }
                    else
                    {
                        throw new Exception("Process Class not found");
                    }
                }

                #endregion

                objParameters[0] = new SqlParameter("@TEMP_TEMPCC", SqlDbType.Structured);
                objParameters[1] = new SqlParameter("@TEMP_TEMPCC_CNFG", SqlDbType.Structured);
                objParameters[2] = new SqlParameter("@TEMP_TEMPCC_OPERATIONS", SqlDbType.Structured);
                objParameters[3] = new SqlParameter("@TEMP_TEMPCC_RESOURCES", SqlDbType.Structured);
                objParameters[4] = new SqlParameter("@TEMP_TEMPCC_STATIONS", SqlDbType.Structured);
                objParameters[5] = new SqlParameter("@TEMP_TEMPCC_TOOLS_CONSUMABLES", SqlDbType.Structured);
                objParameters[6] = new SqlParameter("@TEMP_TEMPCC_PROCESS_CARD", SqlDbType.Structured);
                objParameters[7] = new SqlParameter("@TEMP_TEMPCC_DOCUMENTS", SqlDbType.Structured);
                objParameters[8] = new SqlParameter("@TEMP_TEMPCC_PROCESSCLASS", SqlDbType.Structured);
                objParameters[9] = new SqlParameter("@TEMP_SUCCESSORS", SqlDbType.Structured);
                objParameters[10] = new SqlParameter("@TEMP_PREDECESSORS", SqlDbType.Structured);
                objParameters[11] = new SqlParameter("@USERNAME", SqlDbType.VarChar);
                objParameters[12] = new SqlParameter("@RESULT", SqlDbType.VarChar, 1000);

                objParameters[0].Value = @TEMP_TEMPCC;
                objParameters[1].Value = @TEMP_TEMPCC_CNFG;
                objParameters[2].Value = @TEMP_TEMPCC_OPERATIONS;
                objParameters[3].Value = @TEMP_TEMPCC_RESOURCES;
                objParameters[4].Value = @TEMP_TEMPCC_STATIONS;
                objParameters[5].Value = @TEMP_TEMPCC_TOOLS_CONSUMABLES;
                objParameters[6].Value = @TEMP_TEMPCC_PROCESSES_PROCESS;
                objParameters[7].Value = @TEMP_TEMPCC_DOCUMENTS;
                objParameters[8].Value = @TEMP_APPLPROCESSCLASSLIST;
                objParameters[9].Value = @TEMP_SUCCESSORS;
                objParameters[10].Value = @TEMP_PREDECESSORS;
                objParameters[11].Value = "SET";
                objParameters[12].Direction = ParameterDirection.Output;

                objSql.ExecuteProcedureParam(SqlDataLayer.strLocal, "sp_SyncXML", objParameters, "@RESULT", "@RESULT");

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                objParameters = null;
                objSql = null;
                doc = null;
                sr.Close();
                sr = null;
                sw.Close();
                sw = null;
                CollaborationContext = null;
                Stations = null;
                strStation_name = null;
                Station = null;
                Track = null;
                Processes = null;

                @TEMP_TEMPCC = null;
                @TEMP_TEMPCC_CNFG = null;
                @TEMP_TEMPCC_OPERATIONS = null;
                @TEMP_TEMPCC_RESOURCES = null;
                @TEMP_TEMPCC_STATIONS = null;
                @TEMP_TEMPCC_TOOLS_CONSUMABLES = null;
                @TEMP_APPLPROCESSCLASSLIST = null;
                @TEMP_SUCCESSORS = null;
                @TEMP_PREDECESSORS = null;
            }
        }

        public bool Consume_Xml_Structure_Tool(string strFilename)
        {
            XmlDocument doc = new XmlDocument();
            System.IO.StreamReader sr = new System.IO.StreamReader(strFilename);
            XmlNodeList Tools;

            StringWriter sw = new StringWriter();
            while (!sr.EndOfStream)
            {
                string strLine = sr.ReadLine();
                if (strLine.StartsWith("<") == true && strLine.EndsWith(">") == true)
                {
                    sw.WriteLine(strLine);
                }
            }


            DataTable @TEMP_TEMPCC = new DataTable();

            string strCCid = string.Empty;

            System.Data.DataSet ds = new DataSet();
            SqlDataLayer objSql = new SqlDataLayer();
            SqlParameter[] objParameters = new SqlParameter[2];
            try
            {
                doc.LoadXml(sw.ToString());

                Tools = doc.GetElementsByTagName("Tool");

                foreach (XmlElement node in Tools)
                {
                    ds = ConverttYourXmlNodeToDataSet(node);
                }

                objParameters[0] = new SqlParameter("@TEMP", SqlDbType.Structured);
                objParameters[1] = new SqlParameter("@RESULT", SqlDbType.VarChar, 1000);

                objParameters[0].Value = ds.Tables[0];
                objParameters[1].Direction = ParameterDirection.Output;

                objSql.ExecuteProcedureParam(SqlDataLayer.strLocal, "sp_Sync_Tools", objParameters, "@RESULT", "@RESULT");

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                objParameters = null;
                objSql = null;
                doc = null;
                sr.Close();
                sr = null;
                sw.Close();
                sw = null;
                Tools = null;
            }
        }

        public bool Consume_Xml_Structure_PlantBOM(string strFilename)
        {
            string strRefNo = "";
            bool flag = false;
            SqlDataLayer objSql = new SqlDataLayer();
            SqlParameter[] objParameters = new SqlParameter[4];
            SqlParameter[] objParameters1 = new SqlParameter[1];

            XmlDocument doc = new XmlDocument();
            System.IO.StreamReader sr = new System.IO.StreamReader(strFilename);
            XmlNodeList Plant;
            XmlNodeList Zone;
            XmlNodeList Line;
            XmlNodeList Station;

            StringWriter sw = new StringWriter();
            while (!sr.EndOfStream)
            {
                string strLine = sr.ReadLine();
                if (strLine.StartsWith("<") == true && strLine.EndsWith(">") == true)
                {
                    sw.WriteLine(strLine);
                }
            }

            try
            {
                doc.LoadXml(sw.ToString());
                DataSet ds = new DataSet();

                objParameters1[0] = new SqlParameter("@RESULT", SqlDbType.VarChar, 500);

                objParameters1[0].Direction = ParameterDirection.Output;

                strRefNo = objSql.ExecuteProcedureParam(SqlDataLayer.strLocal, "sp_GetRefNo", objParameters1, "@RESULT", "@RESULT");

                Plant = doc.GetElementsByTagName("Plant");
                foreach (XmlNode x in Plant)
                {
                    ds = ConverttYourXmlNodeToDataSet(x);

                    if (ds.Tables.Contains("Plant") == true)
                    {
                        if (ds.Tables["Plant"].Rows.Count == 0)
                        {
                            flag = false;
                        }
                        else
                        {
                            flag = true;
                        }
                    }
                    else
                    {
                        flag = false;
                    }
                }

                if (flag == true)
                {
                    Station = doc.GetElementsByTagName("Station");
                    foreach (XmlNode x in Station)
                    {
                        ds = ConverttYourXmlNodeToDataSet(x);

                        if (ds.Tables.Count > 0)
                        {
                            objParameters[0] = new SqlParameter("@TEMP_PARENT", SqlDbType.Structured);
                            objParameters[1] = new SqlParameter("@TEMP_CHILD", SqlDbType.Structured);
                            objParameters[2] = new SqlParameter("@FLAG", SqlDbType.VarChar);
                            objParameters[3] = new SqlParameter("@REFNO", SqlDbType.VarChar);

                            objParameters[0].Value = ds.Tables[0];
                            if (ds.Tables.Count > 1)
                            {
                                objParameters[1].Value = ds.Tables[1];
                            }
                            objParameters[2].Value = "STATION";
                            objParameters[3].Value = strRefNo;

                            objSql.ExecuteProcedure_DataSet(SqlDataLayer.strLocal, "sp_PlantBOM", objParameters);
                        }
                    }

                    Line = doc.GetElementsByTagName("Line");
                    foreach (XmlNode x in Line)
                    {
                        ds = ConverttYourXmlNodeToDataSet(x);

                        if (ds.Tables.Count > 0)
                        {
                            objParameters[0] = new SqlParameter("@TEMP_PARENT", SqlDbType.Structured);
                            objParameters[1] = new SqlParameter("@TEMP_CHILD", SqlDbType.Structured);
                            objParameters[2] = new SqlParameter("@FLAG", SqlDbType.VarChar);
                            objParameters[3] = new SqlParameter("@REFNO", SqlDbType.VarChar);

                            objParameters[0].Value = ds.Tables[0].DefaultView.ToTable(true, "ID", "NAME", "REVISION", "SUBTYPE");
                            if (ds.Tables.Count > 1)
                            {
                                objParameters[1].Value = ds.Tables[1].DefaultView.ToTable(true, "CHILD_ID");
                            }
                            objParameters[2].Value = "LINE";
                            objParameters[3].Value = strRefNo;

                            objSql.ExecuteProcedure_DataSet(SqlDataLayer.strLocal, "sp_PlantBOM", objParameters);
                        }
                    }

                    Zone = doc.GetElementsByTagName("Zone");
                    foreach (XmlNode x in Zone)
                    {
                        ds = ConverttYourXmlNodeToDataSet(x);

                        if (ds.Tables.Count > 0)
                        {
                            objParameters[0] = new SqlParameter("@TEMP_PARENT", SqlDbType.Structured);
                            objParameters[1] = new SqlParameter("@TEMP_CHILD", SqlDbType.Structured);
                            objParameters[2] = new SqlParameter("@FLAG", SqlDbType.VarChar);
                            objParameters[3] = new SqlParameter("@REFNO", SqlDbType.VarChar);

                            objParameters[0].Value = ds.Tables[0].DefaultView.ToTable(true, "ID", "NAME", "REVISION", "SUBTYPE");
                            if (ds.Tables.Count > 1)
                            {
                                objParameters[1].Value = ds.Tables[1].DefaultView.ToTable(true, "CHILD_ID");
                            }
                            objParameters[2].Value = "ZONE";
                            objParameters[3].Value = strRefNo;

                            objSql.ExecuteProcedure_DataSet(SqlDataLayer.strLocal, "sp_PlantBOM", objParameters);
                        }
                    }

                    Plant = doc.GetElementsByTagName("Plant");
                    foreach (XmlNode x in Plant)
                    {
                        ds = ConverttYourXmlNodeToDataSet(x);

                        if (ds.Tables.Count > 0)
                        {
                            objParameters[0] = new SqlParameter("@TEMP_PARENT", SqlDbType.Structured);
                            objParameters[1] = new SqlParameter("@TEMP_CHILD", SqlDbType.Structured);
                            objParameters[2] = new SqlParameter("@FLAG", SqlDbType.VarChar);
                            objParameters[3] = new SqlParameter("@REFNO", SqlDbType.VarChar);

                            objParameters[0].Value = ds.Tables[0].DefaultView.ToTable(true, "ID", "NAME", "REVISION", "SUBTYPE");
                            //objParameters[1].Value = ds.Tables[1].DefaultView.ToTable(true, "CHILD_ID");
                            objParameters[2].Value = "PLANT";
                            objParameters[3].Value = strRefNo;

                            objSql.ExecuteProcedure_DataSet(SqlDataLayer.strLocal, "sp_PlantBOM", objParameters);
                        }
                    }
                    return flag;
                }
                else
                {
                    return flag;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                objParameters = null;
                objSql = null;
                doc = null;
                sr.Close();
                sr = null;
                sw.Close();
                sw = null;
                Plant = null;
                Zone = null;
                Line = null;
                Station = null;
            }
        }

        public static DataSet ConverttYourXmlNodeToDataSet(XmlNode xmlnodeinput)
        {
            //declaring data set object
            DataSet dataset = null;
            if (xmlnodeinput != null)
            {
                XmlTextReader xtr = new XmlTextReader(xmlnodeinput.OuterXml, XmlNodeType.Element, null);
                dataset = new DataSet();
                dataset.ReadXml(xtr);
            }

            return dataset;
        }

        public static System.Data.DataTable ConvertXmlNodeListToDataTable(XmlNodeList xnl)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            int TempColumn = 0;

            foreach (XmlNode node in xnl.Item(0).ChildNodes)
            {
                TempColumn++;
                DataColumn dc = new DataColumn(node.Name, System.Type.GetType("System.String"));
                if (dt.Columns.Contains(node.Name))
                {
                    dt.Columns.Add(dc.ColumnName = dc.ColumnName + TempColumn.ToString());
                }
                else
                {
                    dt.Columns.Add(dc);
                }
            }

            int ColumnsCount = dt.Columns.Count;
            for (int i = 0; i < xnl.Count; i++)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < ColumnsCount; j++)
                {
                    dr[j] = xnl.Item(i).ChildNodes[j].InnerText;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public virtual bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return false;
        }
    }
}
