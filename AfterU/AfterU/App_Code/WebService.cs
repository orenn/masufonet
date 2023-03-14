using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.ServiceModel.Web;
using System.Web.Script.Serialization;
using System.Collections;
using System.Text;
using System.Web.Security;
using System.Activities.Statements;
using System.IO;
using System.Configuration;
using System.Web.Configuration;
using System.Net.Mail;
using System.Net;
using RestSharp;
using System.Web.Script.Services;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService
{

    public WebService()
    {


    }


    #region User

    [WebMethod]
    public void User_GetUserEnter()
    {

        //   BasePage bp = new BasePage();

        string UserName = GetParams("UserName");
        string Password = GetParams("Password");

        string ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        string user_agent = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
        string cookieFromUser = "";
        string url = HttpContext.Current.Request.Url.ToString();
        string post = HttpContext.Current.Request.Form.ToString();

        string unickDevice = "";//bp.UnickDevice;
        foreach (string key in HttpContext.Current.Request.Cookies.AllKeys)
        {
            cookieFromUser += String.Format("{0}={1},", key, HttpContext.Current.Request.Cookies[key].Value);
        }
        // DataTable dt = Dal.ExeSp("aftercms.[afterLeadNew_CheckLoginUAExtra]", UserName, Password);
        DataTable dt = Dal.ExeSp("aftercms.[afterLeadNew_CheckLoginUAExtra]", UserName, Password, ip, user_agent, cookieFromUser, url, post, unickDevice);



        if (dt.Rows.Count > 0)
        {

            int UserId = Helper.ConvertToInt(dt.Rows[0]["uId"].ToString());

            if (UserId != 0)
            {
                dt = Dal.ExeSp("aftercms.[afterLeadNew_get_userDetails]", UserId);

            }

        }

        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    return Convert.ToInt32(ds.Tables[0].Rows[0]["uId"]);
        //}

        //   DataTable dt = Dal.ExeSp("User_GetUserEnter", UserName, Password);
        //   dt.Columns.Add("RandomNumber");

        if (dt.Rows.Count > 0)
        {
            System.Web.HttpCookie cookie = new System.Web.HttpCookie("UserData");
            cookie["UserId"] = dt.Rows[0]["uId"].ToString();
            cookie["RoleId"] = dt.Rows[0]["userPermission"].ToString();
            cookie["UserName"] = Server.UrlEncode(dt.Rows[0]["fName"].ToString());
            cookie["ip"] = ip;
            cookie["user_agent"] = user_agent;
            cookie["url"] = url;
          
            //cookie["HebDate"] = Server.UrlEncode(dt.Rows[0]["HebDate"].ToString());
            cookie["Name"] = Server.UrlEncode(dt.Rows[0]["fName"].ToString());
            //cookie["SchoolId"] = dt.Rows[0]["SchoolId"].ToString();

            // FormsAuthentication.RedirectFromLoginPage(dt.Rows[0]["UserName"].ToString(), true);


            //Random rnd = new Random();

            //int RanNumber = rnd.Next(1000,9999);
            //dt.Rows[0]["RandomNumber"] = RanNumber.ToString();


            //SendSMS(RanNumber.ToString());

            cookie.Expires = DateTime.Now.AddYears(90);
            HttpContext.Current.Response.Cookies.Add(cookie);

        }


        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }

    private void SendSMS(string RanNumber)
    {



        var client = new RestClient("https://noproblem.afteru.co.il/SendSMS/sendNowWhatsApp.aspx");
        // client.Authenticator = new HttpBasicAuthenticator(username, password);
        var request = new RestRequest("resource/{id}");
        request.AddParameter("affiId", "1979");
        request.AddParameter("groupId", "1979");
        request.AddParameter("smsMsg", "קוד כניסה למערכת אפטר יו הוא " + RanNumber);
        request.AddParameter("ip", "95.86.91.81");
        request.AddParameter("phones", "0505913817");
        // request.AddHeader("header", "value");
        //  request.AddFile("file", path);
        var response = client.Post(request);
        // var content = response.Content; // Raw content as string

    }


    #endregion
    #region Leeds
    [WebMethod]
    public void Crm_GetComboData()
    {

        string Type = GetParams("Type");
     

        DataTable dt = Dal.ExeSp("Crm_GetComboData",Type);


        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }


    [WebMethod]
    public void GetLeeds()
    {

        string Type = GetParamsValueIfExist("Type");
        string StartDate = GetParamsValueIfExist("StartDate");
        string EndDate = GetParamsValueIfExist("EndDate");
        string Status = GetParamsValueIfExist("Status");
        string CategoryId = GetParamsValueIfExist("CategoryId");
        string CurrentPage = GetParamsValueIfExist("CurrentPage");
        string SearchText = GetParamsValueIfExist("SearchText");
        string LeedsSug = GetParamsValueIfExist("LeedsSug");

        string UserId = HttpContext.Current.Request.Cookies["UserData"]["UserId"];
        string ip = HttpContext.Current.Request.Cookies["UserData"]["ip"];
        string user_agent = HttpContext.Current.Request.Cookies["UserData"]["user_agent"];
        string url = HttpContext.Current.Request.Cookies["UserData"]["url"];


     

        DataTable dt = Dal.ExeSp("[aftercms].[Crm_afterLeadNew_get_leads_list_4userP]","0", CategoryId, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate), Status,100,
                                 CurrentPage, UserId,-1,SearchText, LeedsSug, 0,ip,user_agent);
       


        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }

    [WebMethod]
    public void GetHistory()
    {

        string leadId = GetParamsValueIfExist("leadId");

        DataTable dt = Dal.ExeSp("[dbo].[smart_getLeadListByParentId_New]", Convert.ToInt32(leadId));

        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }


    [WebMethod]
    public void GetLeedsWithoutMovil()
    {

        string Type = GetParamsValueIfExist("Type");
        string StartDate = GetParamsValueIfExist("StartDate");
        string EndDate = GetParamsValueIfExist("EndDate");
        string Status = GetParamsValueIfExist("Status");
        string CategoryId = GetParamsValueIfExist("CategoryId");
        string CurrentPage = GetParamsValueIfExist("CurrentPage");
        string SearchText = GetParamsValueIfExist("SearchText");
        string LeedsSug = GetParamsValueIfExist("LeedsSug");

        string UserId = HttpContext.Current.Request.Cookies["UserData"]["UserId"];
        string ip = HttpContext.Current.Request.Cookies["UserData"]["ip"];
        string user_agent = HttpContext.Current.Request.Cookies["UserData"]["user_agent"];
        string url = HttpContext.Current.Request.Cookies["UserData"]["url"];




        DataTable dt = Dal.ExeSp("[aftercms].[Crm_afterLeadNew_get_leads_WithoutMovil]", "0", CategoryId, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate), Status, 100,
                                 CurrentPage, UserId, -1, SearchText, LeedsSug, 0, ip, user_agent);



        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }



    [WebMethod]
    public void GetLeedsInMovil()
    {

        string Type = GetParamsValueIfExist("Type");
        string StartDate = GetParamsValueIfExist("StartDate");
        string EndDate = GetParamsValueIfExist("EndDate");
        string Status = GetParamsValueIfExist("Status");
        string CategoryId = GetParamsValueIfExist("CategoryId");
        string CurrentPage = GetParamsValueIfExist("CurrentPage");
        string SearchText = GetParamsValueIfExist("SearchText");
        string LeedsSug = GetParamsValueIfExist("LeedsSug");

        string UserId = HttpContext.Current.Request.Cookies["UserData"]["UserId"];
        string ip = HttpContext.Current.Request.Cookies["UserData"]["ip"];
        string user_agent = HttpContext.Current.Request.Cookies["UserData"]["user_agent"];
        string url = HttpContext.Current.Request.Cookies["UserData"]["url"];




        DataTable dt = Dal.ExeSp("[aftercms].[Crm_afterLeadNew_get_leads_InMovil]", "0", CategoryId, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate), Status, 100,
                                 CurrentPage, UserId, -1, SearchText, LeedsSug, 0, ip, user_agent);



        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }


    [WebMethod]
    public void Crm_GetSetLeadChooseMovil()
    {

        string Type = GetParamsValueIfExist("Type");
        string leadId = GetParamsValueIfExist("leadId");
        string dealPrice = GetParamsValueIfExist("dealPrice");
        string eventDate = GetParamsValueIfExist("eventDate");
        string uId = GetParamsValueIfExist("uId");
        string sId = GetParamsValueIfExist("sId");
        string addedPrice = GetParamsValueIfExist("addedPrice");
        string service = GetParamsValueIfExist("service");
        string quality = GetParamsValueIfExist("quality");
        string timeto = GetParamsValueIfExist("timeto");
        string ratio = GetParamsValueIfExist("ratio");
        string dirug = GetParamsValueIfExist("dirug");
        string Comment = GetParamsValueIfExist("Comment");

        string UserId = HttpContext.Current.Request.Cookies["UserData"]["UserId"];

        DataTable dt = Dal.ExeSp("[aftercms].[Crm_GetSetLeadChooseMovil]", Type, leadId, dealPrice, eventDate, uId, sId,
                                 addedPrice,service,quality,timeto,ratio,dirug, Comment, UserId);


        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }



    



    [WebMethod]
    public void Crm_GetSetLead()
    {

        string Type = GetParamsValueIfExist("Type");
        string leadId = GetParamsValueIfExist("leadId");
        string area = GetParamsValueIfExist("area");
        string eventType = GetParamsValueIfExist("eventType");
        string eventDate = GetParamsValueIfExist("eventDate");
        string eventDateInt = GetParamsValueIfExist("eventDateInt");
        string msg = GetParamsValueIfExist("msg");
       if(Type=="2") msg = GetMessageSplit(msg);
       
       
        
        string usermsg = GetParamsValueIfExist("usermsg");
        string title = GetParamsValueIfExist("title");
        string sId = GetParamsValueIfExist("sId");
        
        string UserId = HttpContext.Current.Request.Cookies["UserData"]["UserId"];
    



        DataTable dt = Dal.ExeSp("[aftercms].[Crm_GetSetLead]", Type, leadId, 'מ' + area, 'ל' +eventType, eventDate, eventDateInt,
                                 msg, usermsg, title, sId, UserId);



        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }

    private string GetMessageSplit(string msg)
    {
        string res = "";

        if (string.IsNullOrEmpty(msg))
            return res;

        var msgSplit = msg.Split('\n');

        bool IsFirst = true;

        foreach (var item in msgSplit)
        {
            if (!string.IsNullOrEmpty(item))
            {
                if(IsFirst)
                      res = item;
                else
                     res += "\r\n" + item;

                IsFirst = false;

            }
        }



        return res;
        //msg = msg.Replace("\n", "\r\n");



    }

    // dal_Lead.New_UpdateUserLeadDescPlus(UserId, LeadId, "<br>" + string.Format("עדכון סטטוס ל{0} ב: {1}", statusName, DateTime.Now.ToString()));




    #endregion




    #region BetKneset


    [WebMethod]
    public void BetKneset_GetIfPending()
    {
        DataTable dt = new DataTable();

        string IsPending = File.ReadAllText(Server.MapPath("~/BetKneset/Pending.txt"));// webConfigApp.AppSettings.Settings["IsPending"].Value;

        dt.Columns.Add("res");

        DataRow dr = dt.NewRow();

        dr[0] = IsPending;

        dt.Rows.Add(dr);
        UpdatePendingWeb(0);
        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));

    }


    [WebMethod]
    public void BetKneset_UpdateHTML()
    {

        string html = GetParams("html");
        string Type = GetParams("Type");

        DataTable dt = Dal.ExeSp("BetKneset_UpdateHTML", Type, 1, html);
        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));

        //    string pref = GetAllPrev();
        //   string end = GetAllend();
        //  File.WriteAllText(Server.MapPath("~/BetKneset/Screen.html"), pref + html + end);
        //  UpdatePendingWeb(1);



    }

    [WebMethod]
    public void BetKneset_GetHTML()
    {

        string Type = GetParams("Type");
        string IsFromScreen = GetParams("IsFromScreen");

        DataTable dt = Dal.ExeSp("BetKneset_GetHTML", Type, "1", IsFromScreen);
        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));

        //    string pref = GetAllPrev();
        //   string end = GetAllend();
        //  File.WriteAllText(Server.MapPath("~/BetKneset/Screen.html"), pref + html + end);
        //  UpdatePendingWeb(1);



    }




    private string GetAllend()
    {
        string end = @"
 </div>
    </form>
</body>
</html>
            ";

        return end;
    }

    private string GetAllPrev()
    {
        string prev = @"

        <!DOCTYPE html>
        <head>
            <meta charset='utf-8' />
             <title> בית כנסת - מעגלים </title>
                <script src = '../assets/js/Generic.js'></script>
                <link rel = 'stylesheet' href = '../assets/css/bootstrap-rtl.css'>
                <script type = 'text/javascript' src = '../assets/js/bootstrap.min.js'></script>
                <link href = '../assets/css/rtl-css/style-rtl.css' rel = 'stylesheet' />
                <script src = '../assets/js/lib/jquery-2.1.1.min.js' ></script>
                <script type='text/javascript'>
               

                   setInterval('CheckIfPending()', 60000);

                   $(document).ready(function () {

                   if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
                         $('body').css('overflow', 'auto');

                        $('.dvBox').css('width', '99%').css('font-size','60px');
                        $('.dvbadageTzahi,.dvbadageTzahi span').css('font-size','60px');

                        $('.dvZmanim').removeClass('dvZmanim');
                        $('.dvZmanimSP').removeClass('dvZmanimSP');
                        $('.spComment').css('font-size','40px');
                       $('.dvAlertMessage,.dvAlertMessage span div').css('font-size','40px');

                        $('.dvPageTitle').css('font-size','60px');
                        
                        $('.dvAlertMessage').css('width', '99%');
                        $('.dvAlighnRight').removeClass('dvAlighnRight');
               
                        $('div,span').prop('contenteditable', false);

                        $('.dvCotainer,.dvCotainerM1,.dvCotainerM2').height('100%');
                    

                  }else{               

                      $('body').css('overflow', 'hidden');
                    var CurrentHeight = $(document).height();
                    $('.dvCotainer').height(CurrentHeight * 0.95);
                    $('.dvCotainerM1').height(CurrentHeight * 0.95);
                   // $('.dvCotainerM2').height(CurrentHeight * 0.17);
}

                });


              function CheckIfPending() {
                    mydata = Ajax('BetKneset_GetIfPending');
                    if (mydata[0].res == '1') {

                        location.reload();


                    }

                }



                 </script>



               
          </head>
        <body>
        <form id = 'form1'>
        <div class='dvInMain' id= 'dvInMain'>

        ";

        return prev;
    }


    public void UpdatePendingWeb(int val)
    {

        File.WriteAllText(Server.MapPath("~/BetKneset/Pending.txt"), val.ToString());

        //Configuration webConfigApp = WebConfigurationManager.OpenWebConfiguration("~");
        ////Modifying the AppKey from AppValue to AppValue1
        //webConfigApp.AppSettings.Settings["IsPending"].Value = val.ToString();
        ////Save the Modified settings of AppSettings.
        //webConfigApp.Save();
    }

    #endregion

    #region Tunis
    [WebMethod]
    public void Tunis_UpdateHTML()
    {
        try
        {
            string flName = GetParams("flName");
            string flPhone = GetParams("flPhone");
            string flmail = GetParams("flmail");


            SmtpClient client = new SmtpClient("smtp.office365.com", 25);
            client.Credentials = new System.Net.NetworkCredential("dglaw@dgtracking.co.il", "But60041");
            client.EnableSsl = true;


            //var client = new SmtpClient("smtp.gmail.com", 587)
            //{
            //   // UseDefaultCredentials = false,
            //    Credentials = new NetworkCredential("brokeryogev@gmail.com", "tirlulim"),
            //  // 
            //    EnableSsl = true
            //};

            //shay@softwareasi.com
            //dafna@softwareasi.com
            //,shay@softwareasi.com,dafna@softwareasi.com
            string Message = "להלן מידע שהתקבל:" + "<br><br><b>" + "שם ושם משפחה: </b>" + flName + "<br><b>" + "טלפון:</b> " + flPhone + "<br><b>" + "מייל: </b>" + flmail + "<br>";

            MailMessage Msg = new MailMessage("dglaw@dgtracking.co.il", "tzahi556@gmail.com,shay@softwareasi.com,dafna@softwareasi.com");
            Msg.Subject = "בקשה חדשה הגיעה מדף נחיתה" + DateTime.Now.ToString("dd/MM/yyyy HH:mm ");

            Msg.Body = Message;
            Msg.IsBodyHtml = true;

            client.Send(Msg);
        }
        catch (Exception ex)
        {

            Dal.ExeSp("BetKneset_UpdateHTML", 1, 2, ex.InnerException + " " + ex.Message);
        }
        // client.Send("brokeryogev@gmail.com", "tzahi556@gmail.com", "בקשה חדשה הגיעה מדף נחיתה" + DateTime.Now.ToString("dd/MM/yyyy HH:mm "), Message);


        // DataTable dt = Dal.ExeSp("BetKneset_GetHTML", Type, "1", IsFromScreen);
        // HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));

        //    string pref = GetAllPrev();
        //   string end = GetAllend();
        //  File.WriteAllText(Server.MapPath("~/BetKneset/Screen.html"), pref + html + end);
        //  UpdatePendingWeb(1);



    }

    #endregion

    #region Assign


    [WebMethod]
    public void Assign_FillAssignment()
    {

        string StartDate = GetParams("StartDate");
        string EndDate = GetParams("EndDate");
        string OrgUnitCode = GetParams("OrgUnitCode");

        DataTable dt = Dal.ExeSp("Assign_FillAssignment", StartDate, EndDate, OrgUnitCode);
        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }

    //[WebMethod]
    //public void Assign_GetAssignment()
    //{

    //    string Date = GetParams("Date");
    //    string OrgUnitCode = GetParams("OrgUnitCode");

    //    DataTable dt = Dal.ExeSp("Assign_GetAssignment", Date, OrgUnitCode);

    //    HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));


    //}

    [WebMethod]
    public void Assign_GetAssignmentForPortal()
    {

        string Date = GetParams("Date");
        string OrgUnitCode = GetParams("OrgUnitCode");

        DataTable dt = Dal.ExeSp("Assign_GetAssignmentForPortal", Date, OrgUnitCode);

        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));


    }

    [WebMethod]
    public void Assign_SetEmpForEmptyPosition()
    {

        string SearchDate = GetParams("SearchDate");
        string OrgUnitCode = GetParams("OrgUnitCode");

        DataTable dt = Dal.ExeSp("Assign_SetEmpForEmptyPosition", SearchDate, OrgUnitCode);
        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }

    [WebMethod]
    public void Assignment_GetRequiremntsNonAuto()
    {

        string ShiftDate = GetParams("ShiftDate");
        string ShiftCode = GetParams("ShiftCode");
        string OrgUnitCode = GetParams("OrgUnitCode");




        DataTable dt = Dal.ExeSp("Assignment_GetRequiremntsNonAuto", ShiftDate, ShiftCode, OrgUnitCode);
        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));


    }

    [WebMethod]
    public void Assignment_InsertRequiremntsNonAutoToAssignment()
    {
        string RequirementId = GetParams("RequirementId");
        string ShiftDate = GetParams("ShiftDate");
        string ShiftCode = GetParams("ShiftCode");
        string OrgUnitCode = GetParams("OrgUnitCode");



        string HarigaId = GetParams("HarigaId");
        string HarigaFree = GetParams("HarigaFree");
        string HadId = GetParams("HadId");
        string HadFree = GetParams("HadFree");



        DataTable dt = Dal.ExeSp("Assignment_InsertRequiremntsNonAutoToAssignment", RequirementId,
            ShiftDate, ShiftCode, OrgUnitCode,
            HarigaId, HarigaFree, HadId, HadFree);

        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));


    }

    [WebMethod]
    public void Assignment_SetNonAutoPosion()
    {
        string AssignmentId = GetParams("AssignmentId");
        string Type = GetParams("Type");





        DataTable dt = Dal.ExeSp("Assignment_SetNonAutoPosion", AssignmentId, Type);
        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }




    [WebMethod]
    public void Assignment_SetHoursForWorker()
    {


        string AssignmentId = GetParams("AssignmentId");
        string WorkerHours = GetParams("WorkerHours");
        string Seq = GetParams("Seq");


        DataTable dt = Dal.ExeSp("Assignment_SetHoursForWorker", AssignmentId, WorkerHours, Seq, HttpContext.Current.Request.Cookies["UserData"]["UserId"]);

        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));


    }

    [WebMethod]
    public void Assignment_InsertManualAssign()
    {
        string TargetAssignmentId = GetParams("TargetAssignmentId");
        string SourceAssignmentId = GetParams("SourceAssignmentId");
        string SourceEmpNo = GetParams("SourceEmpNo");
        string Type = GetParams("Type");


        DataTable dt = Dal.ExeSp("Assignment_InsertManualAssign", TargetAssignmentId, SourceAssignmentId, SourceEmpNo, Type, HttpContext.Current.Request.Cookies["UserData"]["UserId"]);

        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));


    }


    [WebMethod]
    public void Assignment_GetAddedHours()
    {
        string OrgUnitCode = GetParams("OrgUnitCode");
        string SearchDate = GetParams("SearchDate");

        DataTable dt = Dal.ExeSp("Assignment_GetAddedHours", SearchDate, OrgUnitCode);
        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));


    }



    [WebMethod]
    public void Assignment_GetPrivateAssign()
    {
        string EmpNo = GetParams("EmpNo");

        DataTable dt = Dal.ExeSp("Assignment_GetPrivateAssign", EmpNo);
        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));


    }



    #endregion

    #region General





    [WebMethod]

    public void Gen_GetTable()
    {

        string TableName = GetParams("TableName");
        string Condition = GetParams("Condition");

        DataTable dt = Dal.ExeSp("Gen_GetTable", TableName, Condition);
        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));

    }

    //[WebMethod]
    //public void Gen_DeleteTable()
    //{
    //    string TableName = GetParams("TableName");
    //    string ColName = GetParams("ColName");
    //    string Val = GetParams("Val");

    //    DataTable dt = Dal.ExeSp("Gen_DeleteTable", TableName, ColName, Val);
    //    HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));

    //}


    //[WebMethod]
    //public void Gen_GetJobsInArea()
    //{

    //    string AreaId = HttpContext.Current.Request.Form["AreaId"].ToString();

    //    DataTable dt = Dal.ExeSp("Gen_GetJobsInArea", AreaId);

    //    HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));


    //}


    //[WebMethod]
    //public void Gen_SetJobsInArea()
    //{

    //    string JobId = GetParams("JobId");
    //    string AreaId = GetParams("AreaId");

    //    string Alias = GetParams("Alias");
    //    string Desc = GetParams("Desc");

    //    DataTable dt = Dal.ExeSp("Gen_SetJobsInArea", JobId, AreaId, Desc, Alias);

    //    HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));


    //}

    //[WebMethod]
    //public void Gen_DeleteJobsInArea()
    //{

    //    string JobId = GetParams("JobId");


    //    DataTable dt = Dal.ExeSp("Gen_DeleteJobsInArea", JobId);

    //    HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));


    //}



    //[WebMethod]
    //public void Gen_GetShifts()
    //{

    //    string ShiftId = HttpContext.Current.Request.Form["ShiftId"].ToString();

    //    DataTable dt = Dal.ExeSp("Gen_GetShifts", ShiftId);

    //    HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));


    //}

    //[WebMethod]
    //public void Gen_GetAreaMessages()
    //{

    //    string AreaId = GetParams("AreaId");
    //    string Date = GetParams("Date");
    //    string Mode = GetParams("Mode");

    //    DataTable dt = Dal.ExeSp("Gen_GetAreaMessages", AreaId, Date, Mode);

    //    HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));


    //}

    //[WebMethod]
    //public void Gen_SetAreaMessages()
    //{

    //    string AreaId = GetParams("AreaId");
    //    string Message = GetParams("Message");
    //    string MessageRoutine = GetParams("MessageRoutine");
    //    string UserId = GetParams("UserId");

    //    DataTable dt = Dal.ExeSp("Gen_SetAreaMessages", AreaId, Message, MessageRoutine, UserId);

    //    HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));


    //}


    //[WebMethod]
    //public void Gen_GetAllTasksInArea()
    //{

    //    string AreaId = GetParams("AreaId");

    //    DataTable dt = Dal.ExeSp("Gen_GetAllTasksInArea", AreaId);

    //    HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));


    //}


    //[WebMethod]
    //public void Gen_GetUserConfirm()
    //{
    //    string Module = GetParams("Module");
    //    string AreaId = GetParams("AreaId");
    //    string type = GetParams("type");
    //    DataTable dt = Dal.ExeSp("Gen_GetUserConfirm", Module, AreaId, type);
    //    HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    //}
    //[WebMethod]
    //public void Gen_SetUserConfirm()
    //{
    //    string UserId = GetParams("UserId");
    //    string Module = GetParams("Module");
    //    string AreaId = GetParams("AreaId");
    //    DataTable dt = Dal.ExeSp("Gen_SetUserConfirm", UserId, Module, AreaId);
    //    HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    //}
    //[WebMethod]
    //public void Gen_GetAreas()
    //{

    //    string AreaId = GetParams("AreaId");
    //    DataTable dt = Dal.ExeSp("Gen_GetAreas", AreaId);
    //    HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    //}
    //[WebMethod]
    //public void Gen_GetUser()
    //{

    //    string AreaId = GetParams("AreaId");
    //    DataTable dt = Dal.ExeSp("Gen_GetUser", AreaId);
    //    HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    //}



    #endregion

    #region SchoolConfig

    [WebMethod]
    public void School_UpdateConfigHours()
    {

        string Hours = GetParams("Hours");

        DataTable dt = Dal.ExeSp("School_UpdateConfigHours", Hours, HttpContext.Current.Request.Cookies["UserData"]["ConfigurationId"], "", "");
        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }


    [WebMethod]
    public void School_UpdateHour()
    {

        string HourId = GetParams("HourId");
        string Mode = GetParams("Mode");

        DataTable dt = Dal.ExeSp("School_UpdateHour", HourId, Mode, HttpContext.Current.Request.Cookies["UserData"]["ConfigurationId"]);
        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }




    #endregion

    #region Teacher

    [WebMethod]
    public void Teacher_GetTeacherList()
    {

        string TeacherId = GetParams("TeacherId");

        DataTable dt = Dal.ExeSp("Teacher_GetTeacherList", TeacherId, HttpContext.Current.Request.Cookies["UserData"]["ConfigurationId"], "", "");
        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }

    [WebMethod]
    public void Teacher_GetTeacherHours()
    {

        string TeacherId = GetParams("TeacherId");

        DataTable dt = Dal.ExeSp("Teacher_GetTeacherHours", TeacherId, HttpContext.Current.Request.Cookies["UserData"]["ConfigurationId"]);
        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }

    [WebMethod]
    public void Teacher_SetTeacherHours()
    {

        string TeacherId = GetParams("TeacherId");
        string Type = GetParams("Type");
        string HourId = GetParams("HourId");

        DataTable dt = Dal.ExeSp("Teacher_SetTeacherHours", TeacherId, HourId, Type, HttpContext.Current.Request.Cookies["UserData"]["ConfigurationId"]);
        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }





    [WebMethod]
    public void Teacher_DML()
    {

        string TeacherId = GetParams("TeacherId");
        string Tafkid = GetParams("Tafkid");
        string ProfessionalId = GetParams("ProfessionalId");

        string FirstName = GetParams("FirstName");
        string LastName = GetParams("LastName");
        string Email = GetParams("Email");
        string Type = GetParams("Type");
        string Frontaly = GetParams("Frontaly");
        string FreeDay = GetParams("FreeDay");

        string Tz = GetParams("Tz");
        string Shehya = GetParams("Shehya");
        string Partani = GetParams("Partani");




        DataTable dt = Dal.ExeSp("Teacher_DML", TeacherId, Tafkid, FirstName, LastName, Email,
          Frontaly, FreeDay, Tz, Shehya, Partani, ProfessionalId,
            HttpContext.Current.Request.Cookies["UserData"]["ConfigurationId"], Type);
        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }

    [WebMethod]
    public void Teacher_GetShehyaGroup()
    {

        string HourId = GetParams("HourId");
        string TeacherId = GetParams("TeacherId");
        DataTable dt = Dal.ExeSp("Teacher_GetShehyaGroup", HourId, TeacherId, HttpContext.Current.Request.Cookies["UserData"]["ConfigurationId"]);
        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }

    [WebMethod]
    public void Teacher_SetGroupShehya()
    {

        string HourId = GetParams("HourId");
        string TeachersIds = GetParams("TeachersIds");

        string ShehyaGroupId = GetParams("ShehyaGroupId");
        string NewGroup = GetParams("NewGroup");

        DataTable dt = Dal.ExeSp("Teacher_SetGroupShehya", HourId, TeachersIds, ShehyaGroupId, NewGroup, HttpContext.Current.Request.Cookies["UserData"]["ConfigurationId"]);
        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }


    [WebMethod]
    public void Teacher_SetPartani()
    {

        string HourId = GetParams("HourId");
        string TeacherId = GetParams("TeacherId");

        string Type = GetParams("Type");


        DataTable dt = Dal.ExeSp("Teacher_SetPartani", HourId, TeacherId, Type, HttpContext.Current.Request.Cookies["UserData"]["ConfigurationId"]);
        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }






    [WebMethod]
    public void Teacher_GetTeachersForShehya()
    {

        string HourId = GetParams("HourId");
        string ShehyaGroupId = GetParams("ShehyaGroupId");
        string TeacherId = GetParams("TeacherId");
        DataTable dt = Dal.ExeSp("Teacher_GetTeachersForShehya", HourId, ShehyaGroupId, TeacherId, HttpContext.Current.Request.Cookies["UserData"]["ConfigurationId"]);
        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }

    [WebMethod]
    public void Teacher_GetAllTeacherHours()
    {

        string TeacherId = GetParams("TeacherId");
        DataTable dt = Dal.ExeSp("Teacher_GetAllTeacherHours", TeacherId, HttpContext.Current.Request.Cookies["UserData"]["ConfigurationId"]);
        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }
    [WebMethod]
    public void Teacher_GetAllMissClass()
    {

        string TeacherId = GetParams("TeacherId");
        DataTable dt = Dal.ExeSp("Teacher_GetAllMissClass", TeacherId, HttpContext.Current.Request.Cookies["UserData"]["ConfigurationId"]);
        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }





    #endregion

    #region Professional
    [WebMethod]
    public void Professional_DML()
    {

        //string Type = GetParams("Type");
        //string ProfessionalId = GetParams("ProfessionalId");
        //string Name = GetParams("Name");
        //string isTwoHour = GetParams("isTwoHour");
        DataTable dt = Dal.ExeSp("GetLastRequest");
        foreach (DataRow row in dt.Rows)
        {
            string[] msgSplit = row["msg"].ToString().Split(new string[] { "\r\n" }, StringSplitOptions.None);

            row["travelType"] = msgSplit[0].Replace("מה צריך להוביל?:", "");
            row["travelfrom"] = msgSplit[1].Replace("מאיפה ההובלה?:", "");
            row["travelto"] = msgSplit[2].Replace("לאיפה ההובלה?:", "");
            row["travelDate"] = msgSplit[3].Replace("למתי ההובלה?:", "");


        }


        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }



    #endregion

    #region Class

    [WebMethod]
    public void Class_GetAllClass()
    {



        DataTable dt = Dal.ExeSp("Class_GetAllClass", HttpContext.Current.Request.Cookies["UserData"]["ConfigurationId"]);
        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }

    [WebMethod]
    public void Class_GetClassStatus()
    {



        DataTable dt = Dal.ExeSp("Class_GetClassStatus", HttpContext.Current.Request.Cookies["UserData"]["ConfigurationId"]);
        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }

    [WebMethod]
    public void Class_GetClassByLayerId()
    {

        string LayerId = GetParams("LayerId");

        DataTable dt = Dal.ExeSp("Class_GetClassByLayerId", LayerId, HttpContext.Current.Request.Cookies["UserData"]["ConfigurationId"]);
        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }

    [WebMethod]
    public void Class_SetClassData()
    {

        string ClassId = GetParams("ClassId");
        string LayerId = GetParams("LayerId");
        string ClassName = GetParams("ClassName");

        string Seq = GetParams("Seq");
        string mode = GetParams("mode");

        DataTable dt = Dal.ExeSp("Class_SetClassData", ClassId, LayerId,
              ClassName, Seq, mode,
            HttpContext.Current.Request.Cookies["UserData"]["ConfigurationId"]);
        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }
    [WebMethod]
    public void Class_SetTeacherToClass()
    {

        string ClassId = GetParams("ClassId");
        string TeacherId = GetParams("TeacherId");
        string Hour = GetParams("Hour");

        string TargetHakbatza = GetParams("TargetHakbatza");
        string SourceHakbatza = GetParams("SourceHakbatza");
        string TargetIhud = GetParams("TargetIhud");
        string SourceIhud = GetParams("SourceIhud");
        string TargetClassTeacherId = GetParams("TargetClassTeacherId");
        string SourceClassTeacherId = GetParams("SourceClassTeacherId");
        string Type = GetParams("Type");


        DataTable dt = Dal.ExeSp("Class_SetTeacherToClass", ClassId, TeacherId, Hour,
            TargetHakbatza, SourceHakbatza, TargetIhud, SourceIhud,
            TargetClassTeacherId, SourceClassTeacherId, Type,
            HttpContext.Current.Request.Cookies["UserData"]["ConfigurationId"]);


        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }

    #endregion

    #region Assign



    [WebMethod]
    public void Assign_ShibutzAuto()
    {
        // to do add paremter ConfigId


        DataSet ds = Dal.ExeDataSetSp("Assign_GetInitDataForShibutz", HttpContext.Current.Request.Cookies["UserData"]["ConfigurationId"]);

        Shibutz sh = new Shibutz(ds);
        sh.StartShibutz();

        HttpContext.Current.Response.Write(ConvertDataTabletoString(ds.Tables[0]));

    }

    [WebMethod]
    public void Assign_GetDataForAssignAuto()
    {



        string LayerId = GetParams("LayerId");
        DataSet ds = Dal.ExeDataSetSp("Assign_GetDataForAssignAuto", LayerId, HttpContext.Current.Request.Cookies["UserData"]["ConfigurationId"]);


        AssignAuto aa = new AssignAuto(ds);
        bool isFinish = aa.StartAssign();

        //int counter = 0;
        //if (isFinish)
        //{
        //    DataTable dt = Dal.ExeSp("Class_GetClassStatus", HttpContext.Current.Request.Cookies["UserData"]["ConfigurationId"]);
        //    foreach (DataRow item in dt.Rows)
        //    {
        //        if (item["ClassId"].ToString() != "" && item["ClassHour"].ToString() != "38")
        //        {
        //            counter++;

        //            // Assign_GetDataForAssignAuto();

        //        }

        //    }

        //  //  if(counter!=0) Assign_GetDataForAssignAuto();
        //}

        HttpContext.Current.Response.Write(ConvertDataTabletoString(ds.Tables[0]));
    }

    [WebMethod]
    public void Assign_DeleteAssignAuto()
    {


        string IsAuto = GetParams("IsAuto");
        DataTable dt = Dal.ExeSp("Assign_DeleteAssignAuto", IsAuto, HttpContext.Current.Request.Cookies["UserData"]["ConfigurationId"]);

        //  AssignAuto aa = new AssignAuto(ds);
        // aa.StartAssign();


        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }





    [WebMethod]
    public void Assign_GetFreeTeacher()
    {


        string ClassId = GetParams("ClassId");
        DataTable dt = Dal.ExeSp("Assign_GetFreeTeacher", ClassId, HttpContext.Current.Request.Cookies["UserData"]["ConfigurationId"]);
        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }
    [WebMethod]
    public void Assign_GetAssignment()
    {


        string LayerId = GetParams("LayerId");
        DataTable dt = Dal.ExeSp("Assign_GetAssignment", LayerId, HttpContext.Current.Request.Cookies["UserData"]["ConfigurationId"]);
        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }

    [WebMethod]
    public void Assign_SetConfiguration()
    {
        string MaxHourInShibutz = GetParams("MaxHourInShibutz");
        string MinForPitzul = GetParams("MinForPitzul");


        DataTable dt = Dal.ExeSp("Assign_SetConfiguration", MaxHourInShibutz, MinForPitzul, HttpContext.Current.Request.Cookies["UserData"]["ConfigurationId"]);
        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }

    [WebMethod]
    public void Assign_GetAllTeacherOptional()
    {
        string ClassId = GetParams("ClassId");
        string HourId = GetParams("HourId");


        DataTable dt = Dal.ExeSp("Assign_GetAllTeacherOptional", ClassId, HourId, HttpContext.Current.Request.Cookies["UserData"]["ConfigurationId"]);
        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }





    [WebMethod]
    public void HourExtra_DML()
    {
        string Type = GetParams("Type");
        string HourExtraId = GetParams("HourExtraId");
        string TeacherId = GetParams("TeacherId");
        string ClassId = GetParams("ClassId");
        string DayId = GetParams("DayId");
        string HourExtra = GetParams("HourExtra");

        DataTable dt = Dal.ExeSp("HourExtra_DML", Type, HourExtraId,
            TeacherId, ClassId, DayId, HourExtra,
            HttpContext.Current.Request.Cookies["UserData"]["ConfigurationId"]);
        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }



    [WebMethod]
    public void Assign_SetAssignManual()
    {




        string Type = GetParams("Type");

        string SourceId = GetParams("SourceId");
        string SourceTeacherId = GetParams("SourceTeacherId");
        string SourceClassId = GetParams("SourceClassId");
        string SourceHourId = GetParams("SourceHourId");
        string SourceProfessionalId = GetParams("SourceProfessionalId");
        string SourceHakbatza = GetParams("SourceHakbatza");
        string SourceIhud = GetParams("SourceIhud");

        string TargetId = GetParams("TargetId");
        string TargetTeacherId = GetParams("TargetTeacherId");
        string TargetClassId = GetParams("TargetClassId");
        string TargetHourId = GetParams("TargetHourId");
        string TargetProfessionalId = GetParams("TargetProfessionalId");
        string TargetHakbatza = GetParams("TargetHakbatza");
        string TargetIhud = GetParams("TargetIhud");



        DataTable dt = Dal.ExeSp("Assign_SetAssignManual", Type,
            SourceId, SourceTeacherId, SourceClassId, SourceHourId, SourceProfessionalId, SourceHakbatza, SourceIhud,
           TargetId, TargetTeacherId, TargetClassId, TargetHourId, TargetProfessionalId, TargetHakbatza, TargetIhud,
            HttpContext.Current.Request.Cookies["UserData"]["ConfigurationId"]);
        HttpContext.Current.Response.Write(ConvertDataTabletoString(dt));
    }



    #endregion

    #region Upload

    [WebMethod]
    public void UploadFile()
    {
        string SchoolId = GetParams("SchoolId");
        string path = HttpContext.Current.Server.MapPath("~/assets/images/SchoolLogo/");

        string[] fileEntries = Directory.GetFiles(path);
        foreach (string fileName in fileEntries)
        {
            if (fileName.Contains(SchoolId + "_"))
            {
                try
                {
                    File.Delete(fileName);

                }
                catch (Exception ex)
                {

                }
            }

        }






        var httpPostedFile = HttpContext.Current.Request.Files["File"];

        httpPostedFile.SaveAs(path + SchoolId + "_.png");


    }


    [WebMethod]
    public void DeleteFile()
    {

        string SchoolId = GetParams("SchoolId");
        string path = HttpContext.Current.Server.MapPath("~/assets/images/SchoolLogo/");

        string[] fileEntries = Directory.GetFiles(path);
        foreach (string fileName in fileEntries)
        {
            if (fileName.Contains(SchoolId + "_"))
            {
                try
                {
                    File.Delete(fileName);

                }
                catch (Exception ex)
                {

                }
            }

        }

    }


    #endregion


    private bool GetParamsIfExist(string Param)
    {
        try
        {
            HttpContext.Current.Request.Form[Param].ToString();
            return true;

        }
        catch (Exception ex)
        {


            return false;
        }
    }

    private string GetParamsValueIfExist(string Param)
    {
        try
        {

            return HttpContext.Current.Request.Form[Param].ToString();

        }
        catch (Exception ex)
        {


            return "";
        }
    }

    private string GetParams(string Param)
    {
        return HttpContext.Current.Request.Form[Param].ToString();
    }

    public static string ConvertDataTabletoString(DataTable dt)
    {


        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row;
        foreach (DataRow dr in dt.Rows)
        {
            row = new Dictionary<string, object>();
            foreach (DataColumn col in dt.Columns)
            {
                row.Add(col.ColumnName, dr[col]);
            }
            rows.Add(row);
        }
        return serializer.Serialize(rows);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void GetPhoneReturnHistory(int LeadId)
    {
        string UserId = HttpContext.Current.Request.Cookies["UserData"]["UserId"];

        DataTable dt = Dal.ExeSp("[dbo].[smart_getLeadListByParentId_New]", LeadId);
        string ReturnData = ConvertDataTabletoString(dt);
        //ReturnData = "{data: " + ReturnData + "}";
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        HttpContext.Current.Response.Write(js.Serialize(ReturnData));
    }

}
