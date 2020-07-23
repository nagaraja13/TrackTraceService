using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using BusinessLayer;
using System.ServiceModel.Activation;
using System.Data;
using TrackTraceService.Classes;

namespace TrackTraceService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TrackTraceService" in code, svc and config file together.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class TrackTraceService : ITrackTraceService
    {
        BL_Common objCommon = new BL_Common();
        BL_RMStores objRMStores = new BL_RMStores();
        BL_WIP objWIP = new BL_WIP();
        BL_ASSET objAsset = new BL_ASSET();

        //BL_Common objCommon = new BL_Common();

        string strResult = "";

        public string LoginUser(string strUsername, string strPassword)
        {
            try
            {
                return objCommon.BL_AuthenticateLogin(strUsername, strPassword);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }

        public Tuple<string, List<string>, List<string>> User_Login_Permission(string strUserID, string strPassword)
        {
            List<string> ListFilter = new List<string>();
            List<string> ListFilter1 = new List<string>();
            try
            {
                string strUserName = objCommon.BL_AuthenticateLogin(strUserID, strPassword);
                if (!strUserName.Contains("Invalid login details"))
                {
                    strResult = objCommon.BL_GetMyRights(strUserID);
                    if (string.IsNullOrEmpty(strResult) != true)
                    {
                        string[] strFilter = strResult.Split(',');
                        ListFilter = strFilter.ToList();
                    }

                    strResult = objCommon.BL_GetMyRights1(strUserID);
                    if (string.IsNullOrEmpty(strResult) != true)
                    {
                        string[] strFilter = strResult.Split(',');
                        ListFilter1 = strFilter.ToList();
                    }
                }

                return Tuple.Create(strUserName, ListFilter, ListFilter1);
            }
            catch (Exception ex)
            {
                ListFilter.Add("Invalid");
                ListFilter1.Add("Invalid");
                return Tuple.Create(ex.ToString(), ListFilter, ListFilter1);
            }
        }

        public List<Rm_Receiving> RM_RECEIVING(string strBarcode, string strStatus, string strUserID)
        {
            DataTable dt = new DataTable();
            List<Rm_Receiving> listRmReceiving = new List<Rm_Receiving>();
            try
            {

                dt = objRMStores.UpdateRecevingDL(strBarcode, strStatus, strUserID);
                listRmReceiving = dt.AsEnumerable().Select(x => new Rm_Receiving()
                {
                    ITEM_CODE = x.Field<String>("ITEM_CODE"),
                    ITEM_DESCRIPTION = x.Field<String>("ITEM_DESCRIPTION"),
                    TXN_QTY = x.Field<decimal>("TXN_QTY"),
                    RECEIPT_NUM = x.Field<String>("RECEIPT_NUM"),
                    PROJECT_NUMBER = x.Field<String>("PROJECT_NUMBER"),

                }).ToList();
                return listRmReceiving;
            }
            catch (Exception ex)
            {
                listRmReceiving.Add(new Rm_Receiving { Result = ex.ToString() });
                return listRmReceiving;
            }
        }

        public string RM_RECEIVING_START(string strBarcode, string strQty, string strStatus)
        {
            try
            {
                strResult = objRMStores.UpdateRecevingDL_Initial(strBarcode, strQty, strStatus);
                return strResult;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }


        public List<Rm_ReceivingDetails> RM_RECEIVINGDETAILS(string strReceiptNo)
        {
            DataTable dt = new DataTable();
            List<Rm_ReceivingDetails> listRmReceivingDetails = new List<Rm_ReceivingDetails>();
            try
            {
                dt = objRMStores.getReceiptData(strReceiptNo);
                listRmReceivingDetails = dt.AsEnumerable().Select(x => new Rm_ReceivingDetails
                {
                    LOTNO = x.Field<String>("LOTNO"),
                    RECEIPT_NUM = x.Field<String>("RECEIPT_NUM"),
                    ITEM_CODE = x.Field<String>("ITEM_CODE"),
                    PROJECT_NUMBER = x.Field<String>("PROJECT_NUMBER"),
                    TXN_QTY = x.Field<string>("TXN_QTY"),
                    STATUS = x.Field<decimal>("STATUS"),
                    ITEM_DESCRIPTION = x.Field<String>("ITEM_DESCRIPTION"),
                }).ToList();
                return listRmReceivingDetails;
            }
            catch (Exception ex)
            {
                listRmReceivingDetails.Add(new Rm_ReceivingDetails { Result = ex.ToString() });
                return listRmReceivingDetails;
            }
        }

        #region RM Picking

        public List<string> RM_GETPICKINGNO(string strUserID)
        {
            DataTable dt = new DataTable();
            List<string> List_RM_GETPICKINGNO = new List<string>();
            try
            {
                dt = objRMStores.getPickSlipNo(strUserID);
                List_RM_GETPICKINGNO = dt.AsEnumerable().Select(r => r.Field<string>("PICK_SLIP_NO")).ToList();
                return List_RM_GETPICKINGNO;
            }
            catch (Exception ex)
            {
                List_RM_GETPICKINGNO.Add(ex.ToString());
                return List_RM_GETPICKINGNO;
            }
        }

        public List<Rm_GetpickingData> RM_GETPICKINGDATA(string strPackSlip, string strUserID)
        {
            DataTable dt = new DataTable();
            List<Rm_GetpickingData> listRmGetpickingData = new List<Rm_GetpickingData>();
            try
            {
                dt = objRMStores.getPickSlipData(strPackSlip, strUserID);
                listRmGetpickingData = dt.AsEnumerable().Select(x => new Rm_GetpickingData
                {
                    PICK_SLIP_NO = x.Field<String>("PICK_SLIP_NO"),
                    WORK_ORDER_NO = x.Field<String>("WORK_ORDER_NO"),
                    ITEM_CODE = x.Field<String>("ITEM_CODE"),
                    DESCRIPTION = x.Field<String>("DESCRIPTION"),
                    LOCTAORs = x.Field<String>("LOCTAOR"),
                    PICK_QTY = x.Field<decimal>("PICK_QTY"),
                    PICKED_QTY = x.Field<decimal>("PICKED_QTY"),
                    UOM = x.Field<String>("UOM"),
                }).ToList();
                return listRmGetpickingData;
            }
            catch (Exception ex)
            {
                listRmGetpickingData.Add(new Rm_GetpickingData { Result = ex.ToString() });
                return listRmGetpickingData;
            }
        } 

        public string RM_PICKING(string strPackSlip, string strLocation, string strItemCode, string strQty, string strPacket, string strStatus, string strUserID)
        {
            try
            {
                strResult = objRMStores.UpdatePicking(strPackSlip, strLocation, strItemCode, strQty, strPacket, strStatus, strUserID);
                return strResult;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        #endregion

        public List<string> WIP_GETKITREC(string strUsername)
        {
            DataTable dt = new DataTable();
            List<string> List_Wip_Getkitrec = new List<string>();
            try
            {
                dt = objWIP.PickSlip_KitMat(strUsername);
                List_Wip_Getkitrec = dt.AsEnumerable().Select(r => r.Field<string>("PICK_SLIP_NO")).ToList();
                return List_Wip_Getkitrec;
            }
            catch (Exception ex)
            {
                List_Wip_Getkitrec.Add(ex.ToString());
                return List_Wip_Getkitrec;
            }
        }




        public List<string> WIP_GETWO()
        {
            DataTable dt = new DataTable();
            List<string> List_Wip_Getwo = new List<string>();
            try
            {
                dt = objWIP.getWorkOrder_PS();
                List_Wip_Getwo = dt.AsEnumerable().Select(r => r.Field<string>("WO_NO")).ToList();
                return List_Wip_Getwo;
            }
            catch (Exception ex)
            {
                List_Wip_Getwo.Add(ex.ToString());
                return List_Wip_Getwo;
            }
        }

        public List<Wip_GetwoDetails> WIP_GETWODETAILS(string strWorkOrder)
        {
            DataTable dt = new DataTable();
            List<Wip_GetwoDetails> List_GetwoDetails = new List<Wip_GetwoDetails>();
            try
            {
                dt = objWIP.getWorkOrder_Details_PS(strWorkOrder);
                List_GetwoDetails = dt.AsEnumerable().Select(x => new Wip_GetwoDetails
                {
                    ASSY_ITEM_CODE = x.Field<String>("ASSY_ITEM_CODE"),
                    SER = x.Field<String>("SER"),
                }).ToList();
                return List_GetwoDetails;
            }
            catch (Exception ex)
            {
                List_GetwoDetails.Add(new Wip_GetwoDetails { Result = ex.ToString() });
                return List_GetwoDetails;
            }
        }

        //need to check
        public List<string> WIP_PSETUP_PRODTYPE(string strWorkOrder)
        {
            DataTable dt = new DataTable();
            List<string> List_Wip_Psetup_Prodtype = new List<string>();
            try
            {
                dt = objWIP.getWorkOrder_getProdType(strWorkOrder);
                List_Wip_Psetup_Prodtype = dt.AsEnumerable().Select(r => r.Field<string>("PRODTYPE")).ToList();
                return List_Wip_Psetup_Prodtype;
            }
            catch (Exception ex)
            {
                List_Wip_Psetup_Prodtype.Add(ex.ToString());
                return List_Wip_Psetup_Prodtype;
            }

        }

        public string WIP_VALIDATEPACKET(string strWork_Order, string strPacketBarcode)
        {
            try
            {
                strResult = objWIP.Validate_Packet(strWork_Order, strPacketBarcode);
                return strResult;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        //need top check
        public string WIP_VALIDATETOOLS(string strProcessType, string strProcessStage, string strAssetCode)
        {
            try
            {
                strResult = objWIP.Validate_Tools(strProcessType, strProcessStage, strAssetCode);
                return strResult;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public string WIP_VALIDATEWIP(string strBarcode, string strProcessCard, string strFlag)
        {
            try
            {
                strResult = objWIP.Validate_WIP(strBarcode, strProcessCard, strFlag);
                return strResult;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public List<string> WIP_PROCESSSTAGES(string strProcessCard, string strUsername)
        {

            DataTable dt = new DataTable();
            List<string> List_Wip_ProcessStages = new List<string>();
            try
            {
                dt = objWIP.Get_Processes(strProcessCard, strUsername);
                List_Wip_ProcessStages = dt.AsEnumerable().Select(r => r.Field<string>("PROCESSSTAGE")).ToList();
                return List_Wip_ProcessStages;
            }
            catch (Exception ex)
            {
                List_Wip_ProcessStages.Add(ex.ToString());
                return List_Wip_ProcessStages;
            }

        }

        public string WIP_INSERTPROCESSSETUP(string strWorkOrder, string strProduction, string strStage, string strSetupTime, string dt_Serial, string dt_Tools, string strUserID, string strPlant)
        {
            try
            {
                strResult = objWIP.InsertProcessSetup(strWorkOrder, strProduction, strStage, strSetupTime, BL_Common.StringToDataTable(dt_Serial), BL_Common.StringToDataTable(dt_Tools), strUserID, strPlant);
                return strResult;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public string WIP_INSERTCOMPSCAN(List<dtPackage> dtPackage, string strWorkOrder, string strPartNo, string strDescription, string strUserID, string strPlant)
        {
            try
            {
                DataTable dt = ListtoDataTableConverter.ToDataTable(dtPackage);
                //strResult = objWIP.InsertCompScan(strWorkOrder, strPartNo, strDescription, BL_Common.StringToDataTable(dtPackage), strUserID, strPlant);
                strResult = objWIP.InsertCompScan(strWorkOrder, strPartNo, strDescription, dt, strUserID, strPlant);
                return strResult;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public string WIP_INSERTPRODSCAN(string strProcessBarcode, string strProcess, string strSerialno, string dtCompBarcode, string dtSerBarcode, string strRemark, string strMode, string intRework, string strUserID, string strPlant, string intRef)
        {
            try
            {
                strResult = objWIP.InsertProduction(strProcessBarcode, strProcess, strSerialno, BL_Common.StringToDataTable(dtCompBarcode), BL_Common.StringToDataTable(dtSerBarcode), strRemark, strMode, Convert.ToInt32(intRework), strUserID, strPlant, Convert.ToInt32(intRef));
                return strResult;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }



        public string ASSET_CALIBRATE(string strBarcode, string strLastCaliDate, string strUsername)
        {
            try
            {
                if (strLastCaliDate == "1")
                {
                    strLastCaliDate = "";
                }
                strResult = objAsset.Asset_Calibration(strBarcode, strLastCaliDate, strUsername);
                return strResult;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        // need to check
        public List<string> EXECUTE_TABLE_ProcessSetup(string strQuery, string strUsername)
        {
            List<string> ListFilter = new List<string>();
            try
            {
                strResult = objCommon.getTable(strQuery, strUsername);
                if (string.IsNullOrEmpty(strResult) != true)
                {
                    string[] strFilter = strResult.Split(',');
                    ListFilter = strFilter.ToList();
                }

                return ListFilter;
            }
            catch (Exception ex)
            {
                ListFilter.Add("Invalid");
                return ListFilter;
            }
        }

        public List<string> GET_PRODTYPE(string strWONO, string strPARTNO)
        {
            List<string> ListFilter = new List<string>();
            try
            {
                string strQuery = "SELECT DISTINCT PRODTYPE FROM TBLPROCESSCARD WHERE WONO='" + strWONO + "' AND PARTNO='" + strPARTNO + "'";
                strResult = objCommon.getTable(strQuery, strPARTNO);
                if (string.IsNullOrEmpty(strResult) != true)
                {
                    string[] strFilter = strResult.Split(',');
                    ListFilter = strFilter.ToList();
                }

                return ListFilter;
            }
            catch (Exception ex)
            {
                ListFilter.Add("Invalid");
                return ListFilter;
            }
        }


        public List<string> GET_PROCESSSTAGE(string strWONO, string strPRODTYPE)
        {
            List<string> ListFilter = new List<string>();
            try
            {
                string strQuery = "SELECT DISTINCT PROCESSSTAGE FROM TBLPROCESSCARD_SUBITEM WHERE DBO.FNSPLIT(BARCODE, '-', 1) = '" + strWONO + "' AND PRODTYPE = '" + strPRODTYPE + "'";
                strResult = objCommon.getTable(strQuery, strPRODTYPE);
                if (string.IsNullOrEmpty(strResult) != true)
                {
                    string[] strFilter = strResult.Split(',');
                    ListFilter = strFilter.ToList();
                }

                return ListFilter;
            }
            catch (Exception ex)
            {
                ListFilter.Add("Invalid");
                return ListFilter;
            }
        }


        public List<string> GET_CATEGORY(string strBARCODE, string strUsername)
        {
            List<string> ListFilter = new List<string>();
            try
            {
                string strQuery = "SELECT CATEGORY + '=' + [DESCRIPTION] FROM TBLTOOLS WHERE BARCODE = '" + strBARCODE + "'";
                strResult = objCommon.getTable(strQuery, strUsername);
                if (string.IsNullOrEmpty(strResult) != true)
                {
                    string[] strFilter = strResult.Split(',');
                    ListFilter = strFilter.ToList();
                }

                return ListFilter;
            }
            catch (Exception ex)
            {
                ListFilter.Add("Invalid");
                return ListFilter;
            }
        }









        #region Barcode Details

        public List<BarcodeDetails_Accepted_Rejected> Barcode_Details_Accepted_Rejected(string strBarcodeType, string strBarcode)
        {
            DataTable dt = new DataTable();
            List<BarcodeDetails_Accepted_Rejected> List_Get_Details = new List<BarcodeDetails_Accepted_Rejected>();
            try
            {
                strBarcodeType = strBarcodeType.Replace("-", " / ");
                dt = objCommon.getTable1("exec sp_barcode_details '" + strBarcodeType + "', '" + strBarcode + "'", "");
                List_Get_Details = dt.AsEnumerable().Select(x => new BarcodeDetails_Accepted_Rejected
                {
                    LOTNO = x.Field<string>("LOTNO"),
                    RECEIPT_NUM = x.Field<string>("RECEIPT_NUM"),
                    WO_NUMBER = x.Field<string>("WO_NUMBER"),
                    SUPPLIER_NAME = x.Field<string>("SUPPLIER_NAME"),
                    ITEM_CODE = x.Field<string>("ITEM_CODE"),
                    ITEM_DESCRIPTION = x.Field<string>("ITEM_DESCRIPTION"),
                    PROJECT_NUMBER = x.Field<string>("PROJECT_NUMBER"),
                    TASK_NUMBER = x.Field<string>("TASK_NUMBER"),
                    QTY = x.Field<decimal>("QTY"),
                    UOM = x.Field<string>("UOM"),
                    DWG_NUMBER = x.Field<string>("DWG_NUMBER"),
                    WCH_ISSUE_NUMBER = x.Field<string>("WCH_ISSUE_NUMBER"),
                    SHELF_EXP_DATE = x.Field<string>("SHELF_EXP_DATE"),
                    BATCH_NUMBER = x.Field<string>("BATCH_NUMBER"),
                    MS_LEVEL = x.Field<string>("MS_LEVEL"),
                    TYPE = x.Field<string>("TYPE"),
                    CRUSER = x.Field<string>("CRUSER"),
                    CRDATE = x.Field<DateTime>("CRDATE"),
                    PART_REV_NO_INSPN = x.Field<string>("PART_REV_NO_INSPN"),
                    MFGSERIAL = x.Field<string>("MFGSERIAL"),
                }).ToList();
                return List_Get_Details;
            }
            catch (Exception ex)
            {
                List_Get_Details.Add(new BarcodeDetails_Accepted_Rejected { Result = ex.ToString() });
                return List_Get_Details;
            }
        }


        public List<BarcodeDetails_Pick_Slip> Barcode_Details_Pick_Slip(string strBarcodeType, string strBarcode)
        {
            DataTable dt = new DataTable();
            List<BarcodeDetails_Pick_Slip> List_Get_Details = new List<BarcodeDetails_Pick_Slip>();
            try
            {
                strBarcodeType = strBarcodeType.Replace("-", " / ");
                dt = objCommon.getTable1("exec sp_barcode_details '" + strBarcodeType + "', '" + strBarcode + "'", "");
                List_Get_Details = dt.AsEnumerable().Select(x => new BarcodeDetails_Pick_Slip
                {
                    SERIALNO = x.Field<string>("SERIALNO"),
                    PICK_SLIP_NO = x.Field<string>("PICK_SLIP_NO"),
                    WORK_ORDER_NO = x.Field<string>("WORK_ORDER_NO"),
                    ITEM_CODE = x.Field<string>("ITEM_CODE"),
                    DESCRIPTION = x.Field<string>("DESCRIPTION"),
                    SUB_INVENTORY = x.Field<string>("SUB_INVENTORY"),
                    LOCATOR = x.Field<string>("LOCATOR"),
                    PROJECT = x.Field<string>("PROJECT"),
                    TASK = x.Field<string>("TASK"),
                    ISSUED_QTY = x.Field<decimal>("ISSUED_QTY"),
                    PICKED_QTY = x.Field<decimal>("PICKED_QTY"),
                    UOM = x.Field<string>("UOM"),
                    LOTNO = x.Field<string>("LOTNO"),

                }).ToList();
                return List_Get_Details;
            }
            catch (Exception ex)
            {
                List_Get_Details.Add(new BarcodeDetails_Pick_Slip { Result = ex.ToString() });
                return List_Get_Details;
            }
        }



        public List<BarcodeDetails_Assembly> Barcode_Details_Assembly(string strBarcodeType, string strBarcode)
        {
            DataTable dt = new DataTable();
            List<BarcodeDetails_Assembly> List_Get_Details = new List<BarcodeDetails_Assembly>();
            try
            {
                strBarcodeType = strBarcodeType.Replace("-", " / ");
                dt = objCommon.getTable1("exec sp_barcode_details '" + strBarcodeType + "', '" + strBarcode + "'", "");
                List_Get_Details = dt.AsEnumerable().Select(x => new BarcodeDetails_Assembly
                {
                    TRACKING_ID = x.Field<string>("TRACKING_ID"),
                    WO_NO = x.Field<string>("WO_NO"),
                    ASSY_ITEM_CODE = x.Field<string>("ASSY_ITEM_CODE"),
                    DESCRIPTION = x.Field<string>("DESCRIPTION"),
                    WO_QTY = x.Field<decimal>("WO_QTY"),
                    SER = x.Field<string>("SER"),
                    ITEM_TYPE = x.Field<string>("ITEM_TYPE"),
                    PROJECT_NUMBER = x.Field<string>("PROJECT_NUMBER"),
                    TASK_NUMBER = x.Field<string>("TASK_NUMBER"),
                    WO_STATUS = x.Field<string>("WO_STATUS"),

                }).ToList();
                return List_Get_Details;
            }
            catch (Exception ex)
            {
                List_Get_Details.Add(new BarcodeDetails_Assembly { Result = ex.ToString() });
                return List_Get_Details;
            }
        }


        public List<BarcodeDetails_Process_Card> Barcode_Details_Process_Card(string strBarcodeType, string strBarcode)
        {
            DataTable dt = new DataTable();
            List<BarcodeDetails_Process_Card> List_Get_Details = new List<BarcodeDetails_Process_Card>();
            try
            {
                strBarcodeType = strBarcodeType.Replace("-", " / ");
                dt = objCommon.getTable1("exec sp_barcode_details '" + strBarcodeType + "', '" + strBarcode + "'", "");
                List_Get_Details = dt.AsEnumerable().Select(x => new BarcodeDetails_Process_Card
                {
                    BARCODE = x.Field<string>("BARCODE"),
                    CTRPLAN = x.Field<string>("CTRPLAN"),
                    WONO = x.Field<string>("WONO"),
                    PARTNO = x.Field<string>("PARTNO"),
                    PRODTYPE = x.Field<string>("PRODTYPE"),
                    DESCRIPTION = x.Field<string>("DESCRIPTION"),
                    PROJECT = x.Field<string>("PROJECT"),
                    TASKNO = x.Field<string>("TASKNO"),
                    PROCESSQTY = x.Field<decimal>("PROCESSQTY"),
                    APPLOCATION = x.Field<string>("APPLOCATION"),
                    ASYTYPE = x.Field<string>("APPLOCATION"),

                }).ToList();
                return List_Get_Details;
            }
            catch (Exception ex)
            {
                List_Get_Details.Add(new BarcodeDetails_Process_Card { Result = ex.ToString() });
                return List_Get_Details;
            }
        }


        public List<BarcodeDetails_Asset> Barcode_Details_Asset(string strBarcodeType, string strBarcode)
        {
            DataTable dt = new DataTable();
            List<BarcodeDetails_Asset> List_Get_Details = new List<BarcodeDetails_Asset>();
            try
            {
                strBarcodeType = strBarcodeType.Replace("-", " / ");
                dt = objCommon.getTable1("exec sp_barcode_details '" + strBarcodeType + "', '" + strBarcode + "'", "");
                List_Get_Details = dt.AsEnumerable().Select(x => new BarcodeDetails_Asset
                {
                    BARCODE = x.Field<string>("BARCODE"),
                    CATEGORY = x.Field<string>("CATEGORY"),
                    SUBCATEGORY = x.Field<string>("SUBCATEGORY"),
                    ERPASSETCODE = x.Field<string>("ERPASSETCODE"),
                    DESCRIPTION = x.Field<string>("DESCRIPTION"),
                    MAKE = x.Field<string>("MAKE"),
                    MODEL = x.Field<string>("MODEL"),
                    SLNO = x.Field<string>("SLNO"),
                    I_NUMBER = x.Field<string>("I_NUMBER"),
                    VALUE = x.Field<string>("VALUE"),
                    VENDOR = x.Field<string>("VENDOR"),
                    PARRENT_ASSET = x.Field<string>("PARRENT_ASSET"),
                    RANGE = x.Field<string>("RANGE"),
                    LEAST_COUNT = x.Field<string>("LEAST_COUNT"),
                    CRITICAL_INST = x.Field<string>("CRITICAL_INST"),
                    COMS_ON = x.Field<string>("COMS_ON"),
                    INSTALLED_ON = x.Field<DateTime?>("INSTALLED_ON"),
                    WARRANTY_DUE_ON = x.Field<DateTime?>("WARRANTY_DUE_ON"),
                    AMC_DUE = x.Field<DateTime?>("AMC_DUE"),
                    PR_MAINT_INTER = x.Field<string>("PR_MAINT_INTER"),
                    CALIBRATE_INTER = x.Field<string>("CALIBRATE_INTER"),
                    CALIBRATED_ON = x.Field<DateTime?>("CALIBRATED_ON"),
                    CALIBRATED_DUE = x.Field<DateTime?>("CALIBRATED_DUE"),
                    CALIBRATE_REP_NO = x.Field<string>("CALIBRATE_REP_NO"),
                    VAL_INTER = x.Field<string>("VAL_INTER"),
                    VALIDATED_ON = x.Field<DateTime?>("VALIDATED_ON"),
                    VALIDATED_DUE = x.Field<DateTime?>("VALIDATED_DUE"),
                    TOBEUSED = x.Field<string>("TOBEUSED"),
                    P_M_MANUAL = x.Field<string>("P_M_MANUAL"),
                    CHECK_METHOD = x.Field<string>("CHECK_METHOD"),
                    APP_TO = x.Field<string>("APP_TO"),
                    BORROWER = x.Field<string>("BORROWER"),
                    ISSUE_DT = x.Field<DateTime?>("ISSUE_DT"),
                    REMARK = x.Field<string>("REMARK"),
                    CRUSER = x.Field<string>("CRUSER"),
                    CRDATE = x.Field<string>("CRDATE"),
                    MDUSER = x.Field<string>("MDUSER"),
                    MDDATE = x.Field<string>("MDDATE"),
                    PLANT = x.Field<string>("PLANT"),
                    ST = x.Field<decimal>("ST"),
                }).ToList();
                return List_Get_Details;
            }
            catch (Exception ex)
            {
                List_Get_Details.Add(new BarcodeDetails_Asset { Result = ex.ToString() });
                return List_Get_Details;
            }
        }


        public List<BarcodeDetails_Asset_History> Barcode_Details_Asset_History(string strBarcodeType, string strBarcode)
        {
            DataTable dt = new DataTable();
            List<BarcodeDetails_Asset_History> List_Get_Details = new List<BarcodeDetails_Asset_History>();
            try
            {
                strBarcodeType = strBarcodeType.Replace("-", " / ");
                dt = objCommon.getTable1("exec sp_barcode_details '" + strBarcodeType + "', '" + strBarcode + "'", "");
                List_Get_Details = dt.AsEnumerable().Select(x => new BarcodeDetails_Asset_History
                {
                    Barcode = x.Field<string>("Barcode"),
                    I_Number = x.Field<string>("I_Number"),
                    Sub_Category = x.Field<string>("Sub Category"),
                    Description = x.Field<string>("Description"),
                    Make = x.Field<string>("Make"),
                    Model = x.Field<string>("Model"),
                    SLNO = x.Field<string>("SLNO"),
                    Range = x.Field<string>("Range"),
                    Calibration_Interval = x.Field<string>("Calibration Interval"),
                    Calibrated_On = x.Field<DateTime?>("Calibrated On"),
                    Calibrated_Due_On = x.Field<DateTime?>("Calibrated Due On"),
                    Calibrate_Rep_On = x.Field<string>("Calibrate Rep On"),
                    Calibration_Type = x.Field<string>("Calibration Type"),
                    Calibration_Agency = x.Field<string>("Calibration Agency"),
                    Applicable_To = x.Field<string>("Applicable To"),
                    Borrowers_Name = x.Field<string>("Borrowers Name"),
                    Borrowers_Dept = x.Field<string>("Borrowers Dept"),
                    Issue_Date = x.Field<DateTime?>("Issue Date"),
                    Acceptance_Criteria = x.Field<string>("Acceptance Criteria"),
                    Acceptance_Decision = x.Field<string>("Acceptance Decision"),
                    Remark = x.Field<string>("Remark"),
                    Last_Modified_By = x.Field<string>("Last Modified By"),
                    Last_Modified_Date = x.Field<DateTime?>("Last Modified Date"),

                }).ToList();
                return List_Get_Details;
            }
            catch (Exception ex)
            {
                List_Get_Details.Add(new BarcodeDetails_Asset_History { Result = ex.ToString() });
                return List_Get_Details;
            }
        }

        #endregion

        //public List<GetProcessCard> Get_ProcessCard(string strSerial_barcode)
        //{
        //    DataTable dt = new DataTable();
        //    List<GetProcessCard> List_Get_Details = new List<GetProcessCard>();
        //    try
        //    {

        //        dt = objCommon.getTable1("SELECT PROCESSCARD,SER FROM TBLWO_INWARD WHERE TRACKING_ID = '" + strSerial_barcode.Trim() + "'", "");
        //        List_Get_Details = dt.AsEnumerable().Select(x => new GetProcessCard
        //        {
        //            strProcessCard = x.Field<string>("PROCESSCARD"),
        //            strSerialNo = x.Field<string>("SER"),
        //        }).ToList();
        //        return List_Get_Details;
        //    }
        //    catch (Exception ex)
        //    {
        //        List_Get_Details.Add(new GetProcessCard { Result = ex.ToString() });
        //        return List_Get_Details;
        //    }
        //}

        #region Production Scanning

        public Tuple<List<GetProcessCard>, List<GetMyProcess_wip>> GETMYPROCESSES_WIP(string strSerial_barcode, string strUsername)
        {
            DataTable dt1 = new DataTable();
            List<GetProcessCard> List_Get_Details = new List<GetProcessCard>();
            DataTable dt = new DataTable();
            List<GetMyProcess_wip> List_GetMyProcess_wip = new List<GetMyProcess_wip>();
            try
            {

                dt1 = objCommon.getTable1("SELECT PROCESSCARD,SER FROM TBLWO_INWARD WHERE TRACKING_ID = '" + strSerial_barcode.Trim() + "'", "");
                List_Get_Details = dt1.AsEnumerable().Select(x => new GetProcessCard
                {
                    strProcessCard = x.Field<string>("PROCESSCARD"),
                    strSerialNo = x.Field<string>("SER"),
                }).ToList();




                dt = objWIP.GetmyProcessWIP(strSerial_barcode, strUsername);
                List_GetMyProcess_wip = dt.AsEnumerable().Select(x => new GetMyProcess_wip
                {
                    Ref = x.Field<int>("Ref"),
                    ProcessStage = x.Field<String>("ProcessStage"),
                    Rework = x.Field<String>("Rework"),
                }).ToList();
                return Tuple.Create(List_Get_Details, List_GetMyProcess_wip);
            }
            catch (Exception ex)
            {
                List_GetMyProcess_wip.Add(new GetMyProcess_wip { Result = ex.ToString() });
                return Tuple.Create(List_Get_Details, List_GetMyProcess_wip);
            }
        }


        public Tuple<List<Get_ProcessDetails1>, List<Get_ProcessDetails2>, List<Get_String_List>> Get_ProcessDetails(string strProcessCard, string strSerial_barcode, string strProcessStage)
        {
            DataTable dt1 = new DataTable();
            List<Get_ProcessDetails1> List_Get_Details1 = new List<Get_ProcessDetails1>();
            DataTable dt2 = new DataTable();
            List<Get_ProcessDetails2> List_Get_Details2 = new List<Get_ProcessDetails2>();
            DataTable dt3 = new DataTable();
            List<Get_String_List> List_Get_Details3 = new List<Get_String_List>();
            try
            {
                strProcessStage = strProcessStage.Replace('-', '/');
                dt1 = objCommon.getTable1("SELECT COMP AS BARCODE, (SELECT ITEM_CODE FROM TBLPICKING WHERE SERIALNO = COMP) AS ASSETNAME, (SELECT DESCRIPTION FROM TBLPICKING WHERE SERIALNO = COMP) AS CATEGORY FROM TBLPRODUCTION_COMP WHERE PROCESS_CARD = '" + strProcessCard.Trim() + "' AND SER = '" + strSerial_barcode.Trim() + "' AND PROCESS = '" + strProcessStage.Trim() + "'", "");
                List_Get_Details1 = dt1.AsEnumerable().Select(x => new Get_ProcessDetails1
                {
                    BARCODE = x.Field<string>("BARCODE"),
                    ASSETNAME = x.Field<string>("ASSETNAME"),
                    CATEGORY = x.Field<string>("CATEGORY"),
                }).ToList();


                dt2 = objCommon.getTable1("SELECT COMP_SER, PARTNO, SRNO, DESCRIPTION FROM TBLPRODUCTION_SER WHERE WO_NO = '" + strProcessCard.Split('-').GetValue(0).ToString() + "' AND SER = '" + strSerial_barcode.Trim() + "'", "");
                List_Get_Details2 = dt2.AsEnumerable().Select(x => new Get_ProcessDetails2
                {
                    COMP_SER = x.Field<string>("COMP_SER"),
                    PARTNO = x.Field<string>("PARTNO"),
                    SRNO = x.Field<string>("SRNO"),
                    DESCRIPTION = x.Field<string>("DESCRIPTION"),
                }).ToList();


                dt3 = objCommon.getTable1("SELECT SERIALNO AS BARCODE FROM TBLPICKING A FULL JOIN TBLCOMPSCAN B ON A.SERIALNO = B.PACKET WHERE A.ST = 2 AND A.WORK_ORDER_NO = '" + strProcessCard.ToString().Split('-').GetValue(0).ToString().Trim() + "'", "");
                List_Get_Details3 = dt3.AsEnumerable().Select(x => new Get_String_List
                {
                    Details = x.Field<string>("BARCODE"),

                }).ToList();

                return Tuple.Create(List_Get_Details1, List_Get_Details2, List_Get_Details3);
            }
            catch (Exception ex)
            {
                List_Get_Details1.Add(new Get_ProcessDetails1 { Result = ex.ToString() });
                List_Get_Details2.Add(new Get_ProcessDetails2 { Result = ex.ToString() });
                List_Get_Details3.Add(new Get_String_List { Result = ex.ToString() });
                return Tuple.Create(List_Get_Details1, List_Get_Details2, List_Get_Details3); ;
            }
        }


        public Tuple<List<Get_String_List>, List<Get_String_List>> Get_ComponentBarcode(string strProcessCard, string strComponentBarcode)
        {

            DataTable dt2 = new DataTable();
            List<Get_String_List> List_Get_Details2 = new List<Get_String_List>();
            DataTable dt3 = new DataTable();
            List<Get_String_List> List_Get_Details3 = new List<Get_String_List>();
            try
            {

                dt2 = objCommon.getTable1("SELECT CAST(ST AS VARCHAR(50)) as BARCODE FROM TBLPICKING WHERE SERIALNO = '" + strComponentBarcode.Trim() + "' AND WORK_ORDER_NO ='" + strProcessCard.Split('-').GetValue(0).ToString().Trim() + "'", "");
                List_Get_Details2 = dt2.AsEnumerable().Select(x => new Get_String_List
                {
                    Details = x.Field<string>("BARCODE"),
                }).ToList();


                dt3 = objCommon.getTable1("SELECT ITEM_CODE + '=' + DESCRIPTION as BARCODE FROM TBLPICKING WHERE WORK_ORDER_NO = '" + strProcessCard.Split('-').GetValue(0).ToString().Trim() + "' AND SERIALNO = '" + strComponentBarcode.Trim() + "'", "");
                List_Get_Details3 = dt3.AsEnumerable().Select(x => new Get_String_List
                {
                    Details = x.Field<string>("BARCODE"),

                }).ToList();

                return Tuple.Create(List_Get_Details2, List_Get_Details3);
            }
            catch (Exception ex)
            {

                List_Get_Details2.Add(new Get_String_List { Result = ex.ToString() });
                List_Get_Details3.Add(new Get_String_List { Result = ex.ToString() });
                return Tuple.Create(List_Get_Details2, List_Get_Details3); ;
            }
        }


        public Tuple<List<Get_String_List>, List<Get_String_List>> Get_Critical_Comp(string strPart_Number, string strSerialNo)
        {

            DataTable dt2 = new DataTable();
            List<Get_String_List> List_Get_Details2 = new List<Get_String_List>();
            DataTable dt3 = new DataTable();
            List<Get_String_List> List_Get_Details3 = new List<Get_String_List>();
            try
            {
                if (strPart_Number.Length == 10)
                {
                    dt2 = objCommon.getTable1("SELECT ITEM_CODE + '=' + DESCRIPTION FROM TBLPICKING WHERE SERIALNO = '" + strPart_Number.Trim() + "'", "");
                    List_Get_Details2 = dt2.AsEnumerable().Select(x => new Get_String_List
                    {
                        Details = x.Field<string>("BARCODE"),
                    }).ToList();
                }

                if (strPart_Number.Length == 11)
                {
                    dt2 = objCommon.getTable1("SELECT ASSY_ITEM_CODE + '=' + DESCRIPTION as BARCODE FROM TBLWO_INWARD WHERE TRACKING_ID = '" + strPart_Number.Trim() + "'", "");
                    List_Get_Details2 = dt2.AsEnumerable().Select(x => new Get_String_List
                    {
                        Details = x.Field<string>("BARCODE"),
                    }).ToList();
                }

                dt3 = objCommon.getTable1("SELECT CAST(ISNULL(COUNT(COMP_SER), 0) AS VARCHAR(50)) as BARCODE FROM TBLPRODUCTION_SER WHERE SER != '" + strSerialNo.Trim() + "' AND COMP_SER = '" + strPart_Number.Trim() + "'", "");
                List_Get_Details3 = dt3.AsEnumerable().Select(x => new Get_String_List
                {
                    Details = x.Field<string>("BARCODE"),

                }).ToList();

                return Tuple.Create(List_Get_Details2, List_Get_Details3);
            }
            catch (Exception ex)
            {

                List_Get_Details2.Add(new Get_String_List { Result = ex.ToString() });
                List_Get_Details3.Add(new Get_String_List { Result = ex.ToString() });
                return Tuple.Create(List_Get_Details2, List_Get_Details3); ;
            }
        }

        #endregion

        #region Rework
        public Tuple<List<Get_String_List>, List<Get_ReworkStage>> WIP_Rework_Scanning_Packet(string strProcessCard, string strSerialNo)
        {

            DataTable dt2 = new DataTable();
            List<Get_String_List> List_Get_Details2 = new List<Get_String_List>();
            DataTable dt3 = new DataTable();
            List<Get_ReworkStage> List_Get_Details3 = new List<Get_ReworkStage>();
            try
            {
                dt2 = objCommon.getTable1("SELECT PROCESSCARD FROM TBLWO_INWARD WHERE TRACKING_ID = '" + strSerialNo.Trim() + "'", "");
                List_Get_Details2 = dt2.AsEnumerable().Select(x => new Get_String_List
                {
                    Details = x.Field<string>("PROCESSCARD"),
                }).ToList();




                dt3 = objCommon.getTable1("EXEC sp_GetReworkStages '" + List_Get_Details2[0].Details.Trim().Split('-').GetValue(0).ToString() + "', '" + strSerialNo + "'", "");
                List_Get_Details3 = dt3.AsEnumerable().Select(x => new Get_ReworkStage
                {
                    PROCESSDESC = x.Field<string>("PROCESSDESC"),
                    REFNO = x.Field<decimal>("REFNO"),

                }).ToList();

                return Tuple.Create(List_Get_Details2, List_Get_Details3);
            }
            catch (Exception ex)
            {

                List_Get_Details2.Add(new Get_String_List { Result = ex.ToString() });
                List_Get_Details3.Add(new Get_ReworkStage { Result = ex.ToString() });
                return Tuple.Create(List_Get_Details2, List_Get_Details3); ;
            }
        }


        public string WIP_REWORK(string dtTemp, string strWorkOrder, string strSerialNumber, string strRemark, string intStatus, string strPlant, string strUsername)
        {
            try
            {
                dtTemp = dtTemp.Replace("-", " / ");
                strResult = objWIP.Save_Rework(BL_Common.StringToDataTable(dtTemp), strWorkOrder, strSerialNumber, strRemark, Convert.ToInt32(intStatus), strPlant, strUsername);
                return strResult;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        #endregion

        #region Component Scanning
        public List<GetWIP_CompScan> GET_WIP_CompScan(string strWONO, string strUsername)
        {
            DataTable dt = new DataTable();
            List<GetWIP_CompScan> listGetWIP_CompScan = new List<GetWIP_CompScan>();
            try
            {
                string strQuery = "SELECT DISTINCT ASSY_ITEM_CODE, DESCRIPTION, WO_QTY FROM TBLWO_INWARD WHERE WO_NO = '" + strWONO + "' AND WO_STATUS = 'Released'";
                dt = objCommon.getTable1(strQuery, strUsername);
                listGetWIP_CompScan = dt.AsEnumerable().Select(x => new GetWIP_CompScan
                {
                    ASSY_ITEM_CODE = x.Field<String>("ASSY_ITEM_CODE"),
                    DESCRIPTION = x.Field<String>("DESCRIPTION"),
                    WO_QTY = x.Field<decimal>("WO_QTY"),

                }).ToList();
                return listGetWIP_CompScan;
            }
            catch (Exception ex)
            {
                listGetWIP_CompScan.Add(new GetWIP_CompScan { Result = ex.ToString() });
                return listGetWIP_CompScan;
            }
        }


        //public List<WIP_CompScan_Scanning_Packet> WIP_Scanning_Packet(string strWONO, string strItemCode)
        //{
        //    DataTable dt = new DataTable();
        //    List<WIP_CompScan_Scanning_Packet> listWIP_Scanning_Packet = new List<WIP_CompScan_Scanning_Packet>();
        //    try
        //    {
        //        string strQuery = "SELECT SERIALNO, ST FROM TBLPICKING WHERE WORK_ORDER_NO = '" + strWONO + "' AND ITEM_CODE = '" + strItemCode + "'";
        //        dt = objCommon.getTable1(strQuery, strItemCode);
        //        listWIP_Scanning_Packet = dt.AsEnumerable().Select(x => new WIP_CompScan_Scanning_Packet
        //        {
        //            SERIALNO = x.Field<String>("SERIALNO"),
        //            ST = x.Field<decimal>("ST"),
        //        }).ToList();
        //        return listWIP_Scanning_Packet;
        //    }
        //    catch (Exception ex)
        //    {
        //        listWIP_Scanning_Packet.Add(new WIP_CompScan_Scanning_Packet { Result = ex.ToString() });
        //        return listWIP_Scanning_Packet;
        //    }
        //}


        //public string EXECUTE_FIELD(string strSERIALNO, string strWONO, string strStatus)
        //{
        //    try
        //    {
        //        string strQuery = "";
        //        if (strStatus == "1")
        //        {
        //            strQuery = "SELECT CAST(COUNT(SERIALNO) AS VARCHAR(50)) FROM TBLPICKING WHERE SERIALNO = '" + strSERIALNO + "' AND WORK_ORDER_NO = '" + strWONO + "'";
        //        }
        //        else if (strStatus == "2")
        //        {
        //            strQuery = "SELECT CAST(ST AS VARCHAR(100)) FROM TBLPICKING WHERE SERIALNO = '" + strSERIALNO + "' AND WORK_ORDER_NO = '" + strWONO + "'";
        //        }
        //        else if (strStatus == "3")
        //        {
        //            strQuery = "SELECT CAST(ST AS VARCHAR(50)) FROM TBLPICKING WHERE SERIALNO = '" + strSERIALNO + "' AND WORK_ORDER_NO = '" + strWONO + "'";
        //        }
        //        else if (strStatus == "4")
        //        {
        //            strQuery = "SELECT ITEM_CODE FROM TBLPICKING WHERE SERIALNO = '" + strSERIALNO + "' AND WORK_ORDER_NO = '" + strWONO + "'";
        //        }
        //        else if (strStatus == "5")
        //        {
        //            strQuery = "SELECT TOP(1) CAST(SERIALNO AS VARCHAR(50)) FROM TBLPICKING WHERE WORK_ORDER_NO = '" + strWONO + "' AND ITEM_CODE = '" + strSERIALNO + "' AND ST = 3";
        //        }
        //        else if (strStatus == "6")
        //        {
        //            strQuery = "SELECT ITEM_CODE + '=' + DESCRIPTION FROM TBLPICKING WHERE WORK_ORDER_NO = '" + strWONO + "' AND SERIALNO = '" + strSERIALNO + "'";
        //        }

        //        strResult = objCommon.getField(strQuery, strStatus);
        //        return strResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.ToString();
        //    }
        //}


        public Tuple<List<Get_String_List>, List<Get_String_List>, List<Get_String_List>, List<Get_String_List>, List<WIP_CompScan_Scanning_Packet>, List<Get_String_List>, List<Get_String_List>> WIP_CompScan_Scanning_Packet(string strSERIALNO, string strWONO)
        {
            DataTable dt = new DataTable();
            List<WIP_CompScan_Scanning_Packet> listWIP_Scanning_Packet = new List<WIP_CompScan_Scanning_Packet>();
            DataTable dt1 = new DataTable();
            List<Get_String_List> List_Get_Details1 = new List<Get_String_List>();
            DataTable dt2 = new DataTable();
            List<Get_String_List> List_Get_Details2 = new List<Get_String_List>();
            DataTable dt3 = new DataTable();
            List<Get_String_List> List_Get_Details3 = new List<Get_String_List>();
            DataTable dt4 = new DataTable();
            List<Get_String_List> List_Get_Details4 = new List<Get_String_List>();
            DataTable dt5 = new DataTable();
            List<Get_String_List> List_Get_Details5 = new List<Get_String_List>();
            DataTable dt6 = new DataTable();
            List<Get_String_List> List_Get_Details6 = new List<Get_String_List>();
            try
            {
                dt1 = objCommon.getTable1("SELECT CAST(COUNT(SERIALNO) AS VARCHAR(50)) as Barcode FROM TBLPICKING WHERE SERIALNO = '" + strSERIALNO + "' AND WORK_ORDER_NO = '" + strWONO + "'", "");
                List_Get_Details1 = dt1.AsEnumerable().Select(x => new Get_String_List
                {
                    Details = x.Field<string>("Barcode"),
                }).ToList();


                dt2 = objCommon.getTable1("SELECT CAST(ST AS VARCHAR(100)) as Barcode FROM TBLPICKING WHERE SERIALNO = '" + strSERIALNO + "' AND WORK_ORDER_NO = '" + strWONO + "'", "");
                List_Get_Details2 = dt2.AsEnumerable().Select(x => new Get_String_List
                {
                    Details = x.Field<string>("Barcode"),
                }).ToList();

                dt3 = objCommon.getTable1("SELECT CAST(ST AS VARCHAR(50)) as Barcode FROM TBLPICKING WHERE SERIALNO = '" + strSERIALNO + "' AND WORK_ORDER_NO = '" + strWONO + "'", "");
                List_Get_Details3 = dt3.AsEnumerable().Select(x => new Get_String_List
                {
                    Details = x.Field<string>("Barcode"),

                }).ToList();

                dt4 = objCommon.getTable1("SELECT ITEM_CODE FROM TBLPICKING WHERE SERIALNO = '" + strSERIALNO + "' AND WORK_ORDER_NO = '" + strWONO + "'", "");
                List_Get_Details4 = dt4.AsEnumerable().Select(x => new Get_String_List
                {
                    Details = x.Field<string>("ITEM_CODE"),

                }).ToList();


                dt = objCommon.getTable1("SELECT SERIALNO, ST FROM TBLPICKING WHERE WORK_ORDER_NO = '" + strWONO + "' AND ITEM_CODE = '" + List_Get_Details4[0].Details + "'", "");
                listWIP_Scanning_Packet = dt.AsEnumerable().Select(x => new WIP_CompScan_Scanning_Packet
                {
                    SERIALNO = x.Field<String>("SERIALNO"),
                    ST = x.Field<decimal>("ST"),
                }).ToList();


                dt5 = objCommon.getTable1("SELECT TOP(1) CAST(SERIALNO AS VARCHAR(50)) as Barcode FROM TBLPICKING WHERE WORK_ORDER_NO = '" + strWONO + "' AND ITEM_CODE = '" + List_Get_Details4[0].Details + "' AND ST = 3", "");
                List_Get_Details5 = dt5.AsEnumerable().Select(x => new Get_String_List
                {
                    Details = x.Field<string>("Barcode"),

                }).ToList();

                dt6 = objCommon.getTable1("SELECT ITEM_CODE + '=' + DESCRIPTION as Barcode FROM TBLPICKING WHERE WORK_ORDER_NO = '" + strWONO + "' AND SERIALNO = '" + strSERIALNO + "'", "");
                List_Get_Details6 = dt6.AsEnumerable().Select(x => new Get_String_List
                {
                    Details = x.Field<string>("Barcode"),

                }).ToList();

                return Tuple.Create(List_Get_Details1, List_Get_Details2, List_Get_Details3, List_Get_Details4, listWIP_Scanning_Packet, List_Get_Details5, List_Get_Details6);
            }
            catch (Exception ex)
            {

                List_Get_Details2.Add(new Get_String_List { Result = ex.ToString() });
                List_Get_Details3.Add(new Get_String_List { Result = ex.ToString() });
                return Tuple.Create(List_Get_Details1, List_Get_Details2, List_Get_Details3, List_Get_Details4, listWIP_Scanning_Packet, List_Get_Details5, List_Get_Details6);
            }
        }

        #endregion

        #region Receipt of kit


        public List<Get_String_List> Get_Pick_Slip_No(string strPickSearch)
        {

            DataTable dt2 = new DataTable();
            List<Get_String_List> List_Get_Details2 = new List<Get_String_List>();

            try
            {
                dt2 = objCommon.getTable1("SELECT DISTINCT(PICK_SLIP_NO) AS PICK_SLIP_NO FROM DBO.TBLPICKING WHERE ST = 1 AND PICK_SLIP_NO LIKE '%" + strPickSearch + "%' ORDER BY PICK_SLIP_NO ASC", "");
                List_Get_Details2 = dt2.AsEnumerable().Select(x => new Get_String_List
                {
                    Details = x.Field<string>("PICK_SLIP_NO"),
                }).ToList();

                return List_Get_Details2;
            }
            catch (Exception ex)
            {
                List_Get_Details2.Add(new Get_String_List { Result = ex.ToString() });
                return List_Get_Details2;
            }
        }

        public List<Wip_GetkitDetails> WIP_GETKITDETAILS(string strPackSlip)
        {
            DataTable dt = new DataTable();
            List<Wip_GetkitDetails> listWipGetkitDetails = new List<Wip_GetkitDetails>();
            try
            {
                dt = objWIP.PickSlip_KitMat_Details(strPackSlip);
                listWipGetkitDetails = dt.AsEnumerable().Select(x => new Wip_GetkitDetails
                {
                    PICK_SLIP_NO = x.Field<String>("PICK_SLIP_NO"),
                    ITEM_CODE = x.Field<String>("ITEM_CODE"),
                    DESCRIPTION = x.Field<String>("DESCRIPTION"),
                    ISSUED_QTY = x.Field<decimal>("ISSUED_QTY"),
                    PICK_QTY = x.Field<decimal>("PICK_QTY"),
                    UOM = x.Field<String>("UOM"),
                }).ToList();
                return listWipGetkitDetails;
            }
            catch (Exception ex)
            {
                listWipGetkitDetails.Add(new Wip_GetkitDetails { Result = ex.ToString() });
                return listWipGetkitDetails;
            }
        }

        public List<Get_Packet_Details> Get_PacketDetails(string strPickSlip, string strPacket)
        {

            DataTable dt2 = new DataTable();
            List<Get_Packet_Details> List_Get_Details2 = new List<Get_Packet_Details>();
            try
            {
                strPickSlip = strPickSlip.Replace('-', ':');
                dt2 = objCommon.getTable1("SELECT ITEM_CODE, DESCRIPTION, ISSUED_QTY, PICK_QTY, UOM FROM DBO.TBLPICKING WHERE ST = 1 AND PICK_SLIP_NO = '" + strPickSlip.Split(':').GetValue(1).ToString().Trim() + "' AND SERIALNO = '" + strPacket.Trim() + "' ORDER BY PICK_SLIP_NO ASC", "");
                List_Get_Details2 = dt2.AsEnumerable().Select(x => new Get_Packet_Details
                {
                    ITEM_CODE = x.Field<string>("ITEM_CODE"),
                    DESCRIPTION = x.Field<string>("DESCRIPTION"),
                    ISSUED_QTY = x.Field<decimal>("ISSUED_QTY"),
                    PICK_QTY = x.Field<decimal>("PICK_QTY"),
                    UOM = x.Field<string>("UOM"),

                }).ToList();

                return List_Get_Details2;
            }
            catch (Exception ex)
            {
                List_Get_Details2.Add(new Get_Packet_Details { Result = ex.ToString() });
                return List_Get_Details2;
            }
        }

        public string WIP_UPDATEKITREC(string strPackSlip, string strPacketNo, string strUsername)
        {
            try
            {
                strResult = objWIP.Update_KitMat(strPackSlip, strPacketNo, strUsername);
                return strResult;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }


        #endregion


        #region Physical stock Count

        //public List<Get_physical_count> Get_Physical_Stock_Count(string strBarcode)
        //{

        //    DataTable dt2 = new DataTable();
        //    List<Get_physical_count> List_Get_Details2 = new List<Get_physical_count>();

        //    try
        //    {
        //        dt2 = objCommon.getTable1("SELECT DESCRIPTION,SLNO,CALIBRATED_ON,REMARK FROM DBO.TBLPHYSICALCOUNT WHERE BARCODE='" + strBarcode + "'", "");
        //        List_Get_Details2 = dt2.AsEnumerable().Select(x => new Get_physical_count
        //        {
        //            DESCRIPTION = x.Field<string>("DESCRIPTION"),
        //            SLNO = x.Field<string>("SLNO"),
        //            CALIBRATED_ON = x.Field<DateTime>("CALIBRATED_ON"),
        //            REMARK = x.Field<string>("REMARK"),
        //        }).ToList();

        //        return List_Get_Details2;
        //    }
        //    catch (Exception ex)
        //    {
        //        List_Get_Details2.Add(new Get_physical_count { Result = ex.ToString() });
        //        return List_Get_Details2;
        //    }
        //}

        //public List<inData> Get_Physical_Stock_Count1(string strBarcode)
        //{

        //    DataTable dt2 = new DataTable();
        //    List<Get_physical_count> List_Get_Details2 = new List<Get_physical_count>();
        //    List<inData> List_Get_Details1 = new List<inData>();

        //    try
        //    {
        //        dt2 = objCommon.getTable1("SELECT DESCRIPTION,SLNO,CALIBRATED_ON,REMARK FROM DBO.TBLPHYSICALCOUNT WHERE BARCODE='" + strBarcode + "'", "");
        //        List_Get_Details2 = dt2.AsEnumerable().Select(x => new Get_physical_count
        //        {
        //            DESCRIPTION = x.Field<string>("DESCRIPTION"),
        //            SLNO = x.Field<string>("SLNO"),
        //            CALIBRATED_ON = x.Field<DateTime>("CALIBRATED_ON"),
        //            REMARK = x.Field<string>("REMARK"),
        //        }).ToList();


        //        List_Get_Details1 = dt2.AsEnumerable().Select(x => new inData
        //        {
        //            name = "abc",
        //            datalst = List_Get_Details2,


        //        }).ToList();

        //        return List_Get_Details1;
        //    }
        //    catch (Exception ex)
        //    {
        //        List_Get_Details2.Add(new Get_physical_count { Result = ex.ToString() });
        //        return List_Get_Details1;
        //    }
        //}


        //public string Insert_Physical_Stock_Count(List<SavePhysicalCount> data)
        //{

        //    try
        //    {
        //        DataTable dt = ListtoDataTableConverter.ToDataTable(data);
        //        if (dt.Rows.Count > 0)
        //        {
        //            // strResult = objWIP.InsertPhysicalCount(dr["BARCODE"].ToString(), dr["DESCRIPTION"].ToString(), dr["SLNO"].ToString(), Convert.ToDateTime(dr["CALIBRATED_ON"].ToString()), dr["REMARK"].ToString(), dr["CRUSER"].ToString(), dr["PLANT"].ToString());
        //            strResult = objWIP.InsertPhysicalCount1(dt);
        //        }
        //        return strResult;

        //    }
        //    catch (Exception ex)
        //    {

        //        return ex.ToString();
        //    }
        //}


        public Tuple<string,List<online_physical_count>> Online_Physical_Stock_Count(string strBarcode, string strUser)
        {

            DataTable dt2 = new DataTable();
            List<online_physical_count> List_Get_Details2 = new List<online_physical_count>();

            try
            {
                strResult = objWIP.OnlineInsertPhysicalCount(strBarcode, strUser);
               


                dt2 = objCommon.getTable1("SELECT 	[CATEGORY],[DESCRIPTION],[SLNO],[CALIBRATED_ON],[REMARK] from TBLPHYSICALCOUNT where BARCODE='" + strBarcode + "'", "");
                List_Get_Details2 = dt2.AsEnumerable().Select(x => new online_physical_count
                {
                    CATEGORY = x.Field<string>("CATEGORY"),
                    DESCRIPTION = x.Field<string>("DESCRIPTION"),
                    SLNO = x.Field<string>("SLNO"),
                    CALIBRATED_ON = x.Field<DateTime?>("CALIBRATED_ON").ToString(),
                    REMARK = x.Field<string>("REMARK"),
                }).ToList();

                return Tuple.Create(strResult, List_Get_Details2);
            }
            catch (Exception ex)
            {
                List_Get_Details2.Add(new online_physical_count { Result = ex.ToString() });
                return Tuple.Create(ex.ToString(), List_Get_Details2);
            }
        }


        public string Offline_Physical_Stock_Count(List<offline_physical_count> data)
        {

            try
            {
                DataTable dt = ListtoDataTableConverter.ToDataTable(data);
                if (dt.Rows.Count > 0)
                {
                    // strResult = objWIP.InsertPhysicalCount(dr["BARCODE"].ToString(), dr["DESCRIPTION"].ToString(), dr["SLNO"].ToString(), Convert.ToDateTime(dr["CALIBRATED_ON"].ToString()), dr["REMARK"].ToString(), dr["CRUSER"].ToString(), dr["PLANT"].ToString());
                    strResult = objWIP.OfflineInsertPhysicalCount(dt);
                }
                return strResult;

            }
            catch (Exception ex)
            {

                return ex.ToString();
            }
        }

        public string Delete_Physical_Stock_Count(string strBarcode, string strUser)
        {
            try
            {
                strResult = objWIP.DeletePhysicalCount(strBarcode, strUser);

                return strResult;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

       

        #endregion

    }
}
