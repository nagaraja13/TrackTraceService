using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using System.Data;
using TrackTraceService.Classes;

namespace TrackTraceService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITrackTraceService" in both code and config file together.
    [ServiceContract]
    public interface ITrackTraceService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "LoginUser/{strUsername}/{strPassword}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        string LoginUser(string strUsername, string strPassword);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "User_Login_Permission/{strUserID}/{strPassword}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Tuple<string, List<string>, List<string>> User_Login_Permission(string strUserID, string strPassword);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "RM_RECEIVING/{strBarcode}/{strStatus}/{strUserID}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Rm_Receiving> RM_RECEIVING(string strBarcode, string strStatus, string strUserID);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "RM_RECEIVING_START/{strBarcode}/{strQty}/{strStatus}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        string RM_RECEIVING_START(string strBarcode, string strQty, string strStatus);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "RM_RECEIVINGDETAILS/{strReceiptNo}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Rm_ReceivingDetails> RM_RECEIVINGDETAILS(string strReceiptNo);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "RM_GETPICKINGNO/{strUserID}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<string> RM_GETPICKINGNO(string strUserID);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "RM_GETPICKINGDATA/{strPackSlip}/{strUserID}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Rm_GetpickingData> RM_GETPICKINGDATA(string strPackSlip, string strUserID);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "RM_PICKING/{strPackSlip}/{strLocation}/{strItemCode}/{strQty}/{strPacket}/{strStatus}/{strUserID}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        string RM_PICKING(string strPackSlip, string strLocation, string strItemCode, string strQty, string strPacket, string strStatus, string strUserID);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "WIP_GETKITREC/{strUsername}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<string> WIP_GETKITREC(string strUsername);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "WIP_GETKITDETAILS/{strPackSlip}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Wip_GetkitDetails> WIP_GETKITDETAILS(string strPackSlip);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "WIP_UPDATEKITREC/{strPackSlip}/{strPacketNo}/{strUsername}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        string WIP_UPDATEKITREC(string strPackSlip, string strPacketNo, string strUsername);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "WIP_GETWO", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<string> WIP_GETWO();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "WIP_GETWODETAILS/{strWorkOrder}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Wip_GetwoDetails> WIP_GETWODETAILS(string strWorkOrder);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "WIP_PSETUP_PRODTYPE/{strWorkOrder}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<string> WIP_PSETUP_PRODTYPE(string strWorkOrder);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "WIP_VALIDATEPACKET/{strWork_Order}/{strPacketBarcode}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        string WIP_VALIDATEPACKET(string strWork_Order, string strPacketBarcode);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "WIP_VALIDATETOOLS/{strProcessType}/{strProcessStage}/{strAssetCode}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        string WIP_VALIDATETOOLS(string strProcessType, string strProcessStage, string strAssetCode);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "WIP_VALIDATEWIP/{strBarcode}/{strProcessCard}/{strFlag}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        string WIP_VALIDATEWIP(string strBarcode, string strProcessCard, string strFlag);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "WIP_PROCESSSTAGES/{strProcessCard}/{strUsername}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<string> WIP_PROCESSSTAGES(string strProcessCard, string strUsername);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "WIP_INSERTPROCESSSETUP/{strWorkOrder}/{strProduction}/{strStage}/{strSetupTime}/{dt_Serial}/{dt_Tools}/{strUserID}/{strPlant}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        string WIP_INSERTPROCESSSETUP(string strWorkOrder, string strProduction, string strStage, string strSetupTime, string dt_Serial, string dt_Tools, string strUserID, string strPlant);

        //[OperationContract]
        //[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json,UriTemplate = "WIP_INSERTCOMPSCAN/{strWorkOrder}/{strPartNo}/{strDescription}/{dtPackage}/{strUserID}/{strPlant}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        //string WIP_INSERTCOMPSCAN(string strWorkOrder, string strPartNo, string strDescription, string dtPackage, string strUserID, string strPlant);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "WIP_INSERTCOMPSCAN", BodyStyle = WebMessageBodyStyle.Wrapped)]
        string WIP_INSERTCOMPSCAN(List<dtPackage> dtPackage, string strWorkOrder, string strPartNo, string strDescription, string strUserID, string strPlant);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "WIP_INSERTPRODSCAN/{strProcessBarcode}/{strProcess}/{strSerialno}/{dtCompBarcode}/{dtSerBarcode}/{strRemark}/{strMode}/{intRework}/{strUserID}/{strPlant}/{intRef}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        string WIP_INSERTPRODSCAN(string strProcessBarcode, string strProcess, string strSerialno, string dtCompBarcode, string dtSerBarcode, string strRemark, string strMode, string intRework, string strUserID, string strPlant, string intRef);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "WIP_REWORK/{dtTemp}/{strWorkOrder}/{strSerialNumber}/{strRemark}/{intStatus}/{strPlant}/{strUsername}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        string WIP_REWORK(string dtTemp, string strWorkOrder, string strSerialNumber, string strRemark, string intStatus, string strPlant, string strUsername);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "ASSET_CALIBRATE/{strBarcode}/{strLastCaliDate}/{strUsername}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        string ASSET_CALIBRATE(string strBarcode, string strLastCaliDate, string strUsername);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "EXECUTE_TABLE/{strQuery}/{strUsername}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<string> EXECUTE_TABLE_ProcessSetup(string strQuery, string strUsername);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GET_PRODTYPE/{strWONO}/{strPARTNO}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<string> GET_PRODTYPE(string strWONO, string strPARTNO);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GET_PROCESSSTAGE/{strWONO}/{strPRODTYPE}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<string> GET_PROCESSSTAGE(string strWONO, string strPRODTYPE);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GET_CATEGORY/{strBARCODE}/{strUsername}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<string> GET_CATEGORY(string strBARCODE, string strUsername);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GET_WIP_CompScan/{strWONO}/{strUsername}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<GetWIP_CompScan> GET_WIP_CompScan(string strWONO, string strUsername);

        //[OperationContract]
        //[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "WIP_Scanning_Packet/{strWONO}/{strItemCode}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        //List<WIP_CompScan_Scanning_Packet> WIP_Scanning_Packet(string strWONO, string strItemCode);


        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GETMYPROCESSES_WIP/{strSerial_barcode}/{strUsername}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Tuple<List<GetProcessCard>, List<GetMyProcess_wip>> GETMYPROCESSES_WIP(string strSerial_barcode, string strUsername);

        //[OperationContract]
        //[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "EXECUTE_FIELD/{strQuery}/{strUsername}/{strStatus}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        //string EXECUTE_FIELD(string strQuery, string strUsername, string strStatus);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "Barcode_Details_Accepted_Rejected/{strBarcodeType}/{strBarcode}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<BarcodeDetails_Accepted_Rejected> Barcode_Details_Accepted_Rejected(string strBarcodeType, string strBarcode);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "Barcode_Details_Pick_Slip/{strBarcodeType}/{strBarcode}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<BarcodeDetails_Pick_Slip> Barcode_Details_Pick_Slip(string strBarcodeType, string strBarcode);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "Barcode_Details_Assembly/{strBarcodeType}/{strBarcode}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<BarcodeDetails_Assembly> Barcode_Details_Assembly(string strBarcodeType, string strBarcode);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "Barcode_Details_Process_Card/{strBarcodeType}/{strBarcode}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<BarcodeDetails_Process_Card> Barcode_Details_Process_Card(string strBarcodeType, string strBarcode);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "Barcode_Details_Asset/{strBarcodeType}/{strBarcode}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<BarcodeDetails_Asset> Barcode_Details_Asset(string strBarcodeType, string strBarcode);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "Barcode_Details_Asset_History/{strBarcodeType}/{strBarcode}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<BarcodeDetails_Asset_History> Barcode_Details_Asset_History(string strBarcodeType, string strBarcode);


        //[OperationContract]
        //[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GET_Processcard/{strSerial_barcode}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        //List<GetProcessCard> Get_ProcessCard(string strSerial_barcode);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "Get_ProcessDetails/{strProcessCard}/{strSerial_barcode}/{strProcessStage}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Tuple<List<Get_ProcessDetails1>, List<Get_ProcessDetails2>, List<Get_String_List>> Get_ProcessDetails(string strProcessCard, string strSerial_barcode, string strProcessStage);


        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "Get_ComponentBarcode/{strProcessCard}/{strComponentBarcode}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Tuple<List<Get_String_List>, List<Get_String_List>> Get_ComponentBarcode(string strProcessCard, string strComponentBarcode);


        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "Get_Critical_Comp/{strPart_Number}/{strSerialNo}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Tuple<List<Get_String_List>, List<Get_String_List>> Get_Critical_Comp(string strPart_Number, string strSerialNo);


        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "WIP_Rework_Scanning_Packet/{strProcessCard}/{strSerialNo}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Tuple<List<Get_String_List>, List<Get_ReworkStage>> WIP_Rework_Scanning_Packet(string strProcessCard, string strSerialNo);


        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "WIP_CompScan_Scanning_Packet/{strSERIALNO}/{strWONO}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Tuple<List<Get_String_List>, List<Get_String_List>, List<Get_String_List>, List<Get_String_List>, List<WIP_CompScan_Scanning_Packet>, List<Get_String_List>, List<Get_String_List>> WIP_CompScan_Scanning_Packet(string strSERIALNO, string strWONO);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "Get_Pick_Slip_No/{strPickSearch}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Get_String_List> Get_Pick_Slip_No(string strPickSearch);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "Get_PacketDetails/{strPickSlip}/{strPacket}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Get_Packet_Details> Get_PacketDetails(string strPickSlip, string strPacket);

        //[OperationContract]
        //[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "Get_Physical_Stock_Count/{strBarcode}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        //List<Get_physical_count> Get_Physical_Stock_Count(string strBarcode);

        //[OperationContract]
        //[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "Get_Physical_Stock_Count1/{strBarcode}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        //List<inData> Get_Physical_Stock_Count1(string strBarcode);

        //[OperationContract]
        //[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "Insert_Physical_Stock_Count", BodyStyle = WebMessageBodyStyle.Bare)]
        //string Insert_Physical_Stock_Count(List<SavePhysicalCount> data1);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "Online_Physical_Stock_Count/{strBarcode}/{strUser}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Tuple<string, List<online_physical_count>> Online_Physical_Stock_Count(string strBarcode, string strUser);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "Offline_Physical_Stock_Count", BodyStyle = WebMessageBodyStyle.Wrapped)]
        string Offline_Physical_Stock_Count(List<offline_physical_count> data);

        [OperationContract]
        [WebInvoke(Method = "DELETE", ResponseFormat = WebMessageFormat.Json, UriTemplate = "Delete_Physical_Stock_Count/{strBarcode}/{strUser}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        string Delete_Physical_Stock_Count(string strBarcode, string strUser);

    
    }
}
