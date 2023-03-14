<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="LeedsWithoutMovil.aspx.cs" Inherits="Leeds_LeedsMainWothoutMovil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--bootstrap validation Library Script End-->
    <!--Demo form validation  Script Start-->

  

    <script type="text/javascript">

        var mydata;

        var StatusList;
        var SubCategoryList;
        var CurrentPage = 1;
        var items;

        $(document).ready(function () {

            //InitFormValidation("fmrReqDetails");
            //GetComboItems("Codes", "TableId=5", "#ddlArea", "ValueCode", "ValueDesc");
            //GetComboItems("Codes", "TableId=7", "#ddlShift", "ValueCode", "ValueDesc");
            //GetComboItems("Codes", "TableId=1", "#ddlDayCode", "ValueCode", "ValueDesc");

            //$("#txtSearch").addClear({
            //    closeSymbol: '<i class="material-icons">close</i>'
            //});

            InitDates();


            InitCombo();

            //InitDateTimePickerPlugin('#txtStartDate,#txtEndDate', getDateTimeNowFormat(), 0);
            //FillData();

            //$("#ddlArea").change(function () {
            //    FillData();
            //});
            FillData();



        });


        function InitDates() {
            $("#txtEndDate").val(addDays(new Date(), -1, 1));// getDateTimeNowFormat()
            $("#txtStartDate").val(addmonth(new Date(), -2, 1));

        }

        function InitCombo() {

            var CategoryList = Ajax("Crm_GetComboData", "Type=1");
            BuildCombo(CategoryList, "#ddlCategories", "group_id", "group_name");

            StatusList = Ajax("Crm_GetComboData", "Type=3");
            BuildCombo(StatusList, ".ddlStatus", "sId", "sName");



            //var CityList = Ajax("Crm_GetComboData", "Type=4");
            //BuildCombo(CityList, ".ddlCity", "afteruCity", "afteruCity");

            //SubCategoryList = Ajax("Crm_GetComboData", "Type=2");

            //  ChangeSubCategory();









        }


        var page = 0;
        function FillData(isFromPaging) {
            //   ChangeSubCategory();



            var StartDate = $("#txtStartDate").val();
            var EndDate = $("#txtEndDate").val();
            var Status = $("#ddlStatus").val();
            var CategoryId = $("#ddlCategories").val();
            var SearchText = $("#txtSearch").val();
            var LeedsSug = $("#ddlLeedsSug").val();
            if (!StartDate || !EndDate || !Status || !CategoryId || !LeedsSug) {

                UIkit.modal.alert('לא ניתן לחפש לידים ללא כל שדות החיפוש!', {
                    center: true, labels: { Ok: 'אישור' }
                });
                return;

            }

            if (!isFromPaging)
                CurrentPage = 1;

            var Category = ($("#ddlCategories option:selected").text());

            $(".spcategoryDesc").text(Category);
            $("#dvReqContainer").html("");

            mydata = Ajax("GetLeedsWithoutMovil", "Type=0&StartDate=" + StartDate + "&EndDate=" + EndDate + "&Status=" + Status +
                "&CategoryId=" + CategoryId + "&CurrentPage=" + CurrentPage + "&SearchText=" + SearchText + "&LeedsSug=" + LeedsSug);

            if (mydata.length == 0) {
                ReqHtml = $("#dvReqTemplateEmpty").html();
                $("#dvReqContainer").append(ReqHtml);
            } else {

                if (!isFromPaging) {
                    var pageCount = mydata[0].pageCount;
                    page = Math.ceil(pageCount / 100);

                    Paging(0, 0);

                }

            }



            for (var i = 0; i < mydata.length; i++) {

                ReqHtml = $("#dvReqTemplate").html();


                var lId = mydata[i].db_ident;
                ReqHtml = ReqHtml.replace(/@lId/g, lId);
                ReqHtml = ReqHtml.replace(/@fullName/g, mydata[i].fullName + " " + mydata[i].area);
                ReqHtml = ReqHtml.replace(/@phone/g, mydata[i].phone);
                ReqHtml = ReqHtml.replace(/@insert_date_onlyDate/g, mydata[i].insert_date_onlyDate);
                ReqHtml = ReqHtml.replace(/@insert_date_onlyTime/g, mydata[i].insert_date_onlyTime);

                var Movil_uid = mydata[i].Movil_uid;
                var dirug = mydata[i].dirug;
                var Color = "";

                // בחירת מוביל ללא דירוג
                if (Movil_uid > 0 && !dirug) {

                    Color = "#e6ffff";
                }

                // בחירת מוביל עם דירוג
                if (Movil_uid > 0 && dirug > 0) {

                    Color = "#ffffcc";
                }
                ReqHtml = ReqHtml.replace(/@Color/g, Color);


                $("#dvReqContainer").append(ReqHtml);

                $("#ddlStatusInner_" + lId).val(mydata[i].sId);







                // eventDateInt_onlyDate
                //$textarea = $("#txtDetails_" + lId);
                //if ($textarea.hasClass('autosized')) {
                //    autosize.destroy($textarea);
                //}

                //debugger

                //autosize($textarea);
                //$textarea.addClass('autosized');
                $("#ddlStatusInner_" + lId).addClass('kUI_combobox');

                // $("#ddlSubCategory_" + lId + ",#ddlCityFrom_" + lId + ",#ddlCityTo_" + lId).addClass("selectAlter");



            }

            $('.kUI_combobox').kendoComboBox();



        }

        function Paging(type, selectedPage) {

            if (type == 1) CurrentPage = selectedPage;

            if (type == 2) CurrentPage = 1;
            if (type == 3) CurrentPage = page;


            $(".uk-pagination").html("");
                <%-- <ul class="uk-pagination" >
                 <li class="uk-disabled"><span><i class="uk-icon-angle-double-left"></i></span></li>
            <li class="uk-active"><span>1</span></li>
            <li><a href="#">2</a></li>
            <li><a href="#">3</a></li>
            <li><a href="#">4</a></li>
            <li><span>&hellip;</span></li>
            <li><a href="#">20</a></li>
             <li><a href="#">21</a></li>
             <li><a href="#">22</a></li>
             <li><a href="#">22</a></li>
            <li><a href="#"><i class="uk-icon-angle-double-right"></i></a></li>--%>

            var HtmlReq = "";
            if (page <= 10) {
                HtmlReq += '<li class="uk-disabled"><span><i class="uk-icon-angle-double-left"></i></span></li>';
                for (var i = 1; i <= page; i++) {
                    if (i == CurrentPage)
                        HtmlReq += ' <li class="uk-active"><span>' + i + '</span></li>';
                    else
                        HtmlReq += ' <li><a onclick="Paging(1,' + i + ')">' + i + '</a></li>';
                }
                HtmlReq += '<li class="uk-disabled"><span><i class="uk-icon-angle-double-right"></i></span></li>';
            }


            else if ((CurrentPage + 9) <= page) {

                HtmlReq += '<li><a onclick="Paging(2)"><i class="uk-icon-angle-double-left" ></i></a></li>';
                for (var i = CurrentPage; i <= ((CurrentPage + 9)); i++) {


                    if (i == CurrentPage)
                        HtmlReq += ' <li class="uk-active"><span>' + i + '</span></li>';
                    else
                        HtmlReq += ' <li><a onclick="Paging(1,' + i + ')">' + i + '</a></li>';
                }
                HtmlReq += '<li><a onclick="Paging(3)"><i class="uk-icon-angle-double-right" ></i></a></li>';
            }
            else {

                HtmlReq += '<li><a onclick="Paging(2)"><i class="uk-icon-angle-double-left" ></i></a></li>';
                for (var i = (page - 9); i <= page; i++) {


                    if (i == CurrentPage)
                        HtmlReq += ' <li class="uk-active"><span>' + i + '</span></li>';
                    else
                        HtmlReq += ' <li><a onclick="Paging(1,' + i + ')">' + i + '</a></li>';
                }
                HtmlReq += '<li><a onclick="Paging(3)"><i class="uk-icon-angle-double-right" ></i></a></li>';



            }

            $(".uk-pagination").append(HtmlReq);


            if (type != 0) FillData(true);

        }

        function CreateDatatable(leadId) {
            try {
                items.destroy();
            } catch (e) {

            }
            jQuery.noConflict();
            items = $('#items').DataTable({
                "orderClasses": false,
                "autoWidth": false,
                "processing": true,
                "serverSide": true,
                "searching": false,
                "sorting": false,
                "ordering": false,
                "paging": false,
                "searching": false,
                "colReorder": true,
                "responsive": true,
                "language": {
                    "decimal": "",
                    "emptyTable": "טבלה ריקה",
                    "info": "מידע",
                    "infoEmpty": "טבלה ריקה",
                    "infoFiltered": '',
                    "infoPostFix": "",
                    "thousands": ",",
                    "lengthMenu": "אורך",
                    "loadingRecords": "העלה רשומות",
                    "processing": "",
                    "search": "חפש",
                    "zeroRecords": "אין רשומות"
                },
                "ajax": {
                    "url": "/WebService.asmx/GetPhoneReturnHistory",
                    "async": false,
                    "data": {
                        "LeadId": leadId
                    },
                    "type": "POST",
                    "dataType": "json",
                    error: function (xhr, textStatus, errorThrown) {
                        console.log(xhr.responseText);
                    },
                    success: function (data) {
                        $($.parseJSON(data)).each(function (index, value) {
                            $("#items").append("<tr><td style='padding-right: 26px;'>" + value.fullName + "</td><td style='padding-right: 26px;'>" + value.phone + "</td><td style='padding-right: 26px;'>" + value.lId + "</td><td style='padding-right: 26px;'>"
                                + value.affiName + "</td><td style='padding-right: 26px;'>" + (value.siteURL == '' ? 'אין קישור' : value.siteURL) + "</td><td style='padding-right: 26px;'>" + moment(value.insert_date).format("YYYY-MM-DD hh:mm") + "</td><td style='padding-right: 26px;'>"
                                + value.income + "</td><td style='padding-right: 26px;'>" + value.Suppliers + "</td><td style='padding-right: 26px;'>" + value.numOfSuppliers + "</td></tr>");
                        });
                    },
                }
            });

            $("#myModal").modal();
        }


        function OpenMailBox(elem) {



            //string Type = GetParamsValueIfExist("Type");
            //string leadId = GetParamsValueIfExist("leadId");
            //string dealPrice = GetParamsValueIfExist("dealPrice");
            //string eventDate = GetParamsValueIfExist("eventDate");
            //string uId = GetParamsValueIfExist("uId");
            //string Comment = GetParamsValueIfExist("Comment");


            var id = $(elem).attr("id");

            if ($("#" + id).find(".md-card-list-item-content-wrapper").length == 0) {

                id = id.replace("li_", "");
                var ReqHtml = $("#dvReqTemplateInner").html();

                ReqHtml = ReqHtml.replace(/@lId/g, id);


                $("#li_" + id).append(ReqHtml);


                var mydataRow = mydata.filter(x => x.db_ident == id);

                var Movil_uid = mydataRow[0].Movil_uid;


                

                if (mydataRow[0].dealPrice)
                     $("#txtCost_" + id).val(mydataRow[0].dealPrice).parent().addClass("md-input-filled");


               
                if (Movil_uid==0)
                    $("#NoDeal_" + id).prop("checked", true);
                
                if (mydataRow[0].hovala_date_onlyDate)
                    $("#txteventDate_" + id).val(mydataRow[0].hovala_date_onlyDate).parent().addClass("md-input-filled");

                //else if (mydataRow[0].eventDateInt_onlyDate != "0")
                //    $("#txteventDate_" + id).val(mydataRow[0].eventDateInt_onlyDate).parent().addClass("md-input-filled");
                





                if (mydataRow.length > 0) {


                    var msg = mydataRow[0].msg.split("\r\n");
                    var onlymsg = "";
                    var allmsg = "";
                    for (var i = 0; i < msg.length; i++) {



                        allmsg += msg[i] + "<br/>";


                    }


                    $("#txtAllDetails_" + id).html(allmsg);

                    LoadCommentsByLeadId(id);

                }


                var movilimList = Ajax("Crm_GetSetLeadChooseMovil", "Type=1&leadId=" + id +
                    "&dealPrice=" + "" + "&eventDate=" + "" + "&uId=" + "" + "&sId=" + ""  + "&addedPrice=" + "" + "&service=" + "" +
                    "&quality=" + "" + "&timeto=" + "" + "&ratio=" +
                    "&dirug=" + "" + "&Comment=" + ""
                );

                
                if (movilimList.length > 0) {
                    $("#dvNehagimList_" + id).html("");
                   

                    for (var i = 0; i < movilimList.length; i++) {
                        var ReqHtml = $("#dvNehagimTemplate").html();
                        ReqHtml = ReqHtml.replace(/@lId/g, id);
                        ReqHtml = ReqHtml.replace(/@uid/g, movilimList[i].uId);
                        ReqHtml = ReqHtml.replace(/@fName/g, movilimList[i].fName);
                     
                        var Check = "";
                        if (movilimList[i].uId == Movil_uid) {

                            Check = "Check";
                        }
                       

                        ReqHtml = ReqHtml.replace(/@ClassStar/g, "star" + Check);
                        ReqHtml = ReqHtml.replace(/@ClassLabel/g, "starLabel" + Check);


                        $("#dvNehagimList_" + id).append(ReqHtml);


                        //<div class="uk-width-medium-1-1" onclick="CheckNahag(@lId,@uid)">
                        //    <span id="spStar_@lId_@uid" class="@ClassStar"></span>
                        //    <span id="spLabel_@lId_@uid" class="@ClassLabel"></span>
                        //</div>


                    }



                }


            }


            $("#NoDeal_" + id).attr("data-switchery", "");

            altair_forms.switches();


        }


        function LoadCommentsByLeadId(id) {


            var mycomments = Ajax("Crm_GetSetLead", "Type=1&leadId=" + id +
                "&area=" + "" + "&eventType=" + "" + "&eventDate=" + "" + "&eventDateInt=" + 0 + "&msg=" + "" +
                "&usermsg=" + "" + "&title=" + "" + "&sId=" + 0);

            $("#dvMainComments_" + id).html("");

            if (mycomments.length > 0) {


                for (var i = 0; i < mycomments.length; i++) {

                    var DescArray = mycomments[i].uDesc.split("<br>");

                    for (var j in DescArray) {

                        var TitleDate = GetStrBetweenTwoCharcter("<u><b>", "</b></u>", DescArray[j]);
                        var Message = DescArray[j].replace(TitleDate, "");
                        if (!Message) continue;
                        var ReqCommentHtml = $("#dvReqComments").html();
                        ReqCommentHtml = ReqCommentHtml.replace(/@lId/g, id);
                        ReqCommentHtml = ReqCommentHtml.replace(/@uId/g, mycomments[i].uId);
                        ReqCommentHtml = ReqCommentHtml.replace(/@fName/g, mycomments[i].fName + ' ' + TitleDate);
                        ReqCommentHtml = ReqCommentHtml.replace(/@uDesc/g, Message);
                        $("#dvMainComments_" + id).append(ReqCommentHtml);

                    }

                }




            }



        }


        function GetStrBetweenTwoCharcter(start, end, str) {

            var mySubString = str.substring(
                str.indexOf(start) + 6,
                str.lastIndexOf(end)
            );

            return mySubString;

        }

        function SaveData(leadId, type) {

           
            var dealPrice = $("#txtCost_" + leadId).val();
            var eventDate = $("#txteventDate_" + leadId).val();
            var IsNoSagar = $("#NoDeal_" + leadId).is(":checked");
            var sId = $("#ddlStatusInner_" + leadId).val();
            
            var uId=0;
            $('[id^="spStar_' + leadId + '"]').each(function () {
                if ($(this)[0].className.indexOf("Check") > -1) {

                    uId = $(this).attr("id");
                    uId = uId.replace("spStar_" + leadId + "_","");
                    
                }
                
            });

            if (uId == 0 && !IsNoSagar) {

                UIkit.modal.alert("חובה לבחור ספק או לסמן שלא נסגר ספק", {
                    center: true, labels: { Ok: 'אישור' }
                });
                return;

            }


            eventDate = ConvertHebrewDateToInt(eventDate);
            if (eventDate == 0) eventDate = "";

            var res = Ajax("Crm_GetSetLeadChooseMovil", "Type=2&leadId=" + leadId +
                "&dealPrice=" + dealPrice + "&eventDate=" + eventDate + "&uId=" + uId + "&sId=" + sId  + "&addedPrice=" + "" + "&service=" + "" +
                "&quality=" + "" + "&timeto=" + "" + "&ratio=" +
                "&dirug=" + "" + "&Comment=" + ""
            );

            
            LoadCommentsByLeadId(leadId);
            UIkit.modal.alert('נתונים נשמרו בהצלחה!', {
                center: true, labels: { Ok: 'אישור' }
            });


        }

        function CheckNahag(leadId, uid) {
            //debugger
            var currentStar = $("#spStar_" + leadId + "_" + uid);// spStar_@lId_ @uid
            var currentLabel = $("#spLabel_" + leadId + "_" + uid);

            if (currentStar[0].className.indexOf("Check") > -1) {

                currentStar.addClass('star');
                currentStar.removeClass('starCheck');

                currentLabel.addClass('starLabel');
                currentLabel.removeClass('starLabelCheck');



            } else {


                $('[id^="spStar_' + leadId + '"]').addClass('star').removeClass('starCheck');
                $('[id^="spLabel_' + leadId + '"]').addClass('starLabel').removeClass('starLabelCheck');

                currentStar.addClass('starCheck');
                currentStar.removeClass('star');

                currentLabel.addClass('starLabelCheck');
                currentLabel.removeClass('starLabel');


             //  $("#NoDeal_" + leadId).prop("checked", false);

               
               $("#NoDeal_" + leadId).parent().find(".switchery").remove();//   removeAttr("data-switchery");
               $("#NoDeal_" + leadId).prop("checked", false);
               altair_forms.switches();
            }

        }


        function ChangeSagar(leadId) {

            if ($("#NoDeal_" + leadId).is(":checked")) {
                $('[id^="spStar_' + leadId + '"]').addClass('star').removeClass('starCheck');
                $('[id^="spLabel_' + leadId + '"]').addClass('starLabel').removeClass('starLabelCheck');

                $("#txtCost_" + leadId).val("").parent().removeClass("md-input-filled");
                $("#txteventDate_" + leadId).val("").parent().removeClass("md-input-filled");
            }

           // alert($("#NoDeal_" + leadId).is(":checked"));
        }


        function SaveDataComment(leadId, type) {

           
            var usermsg = $("#txtComment_" + leadId).val();

            if (!usermsg) {

                UIkit.modal.alert('לא ניתן להוסיף הערה ריקה', {
                    center: true, labels: { Ok: 'אישור' }
                });
                return;

            }




            var mycomments = Ajax("Crm_GetSetLead", "Type=" + 4 + "&leadId=" + leadId +
                "&area=" + "" + "&eventType=" + "" + "&eventDate=" + "" + "&eventDateInt=" + "" + "&msg=" + "" +
                "&usermsg=" + usermsg + "&title=" + "" + "&sId=" + "");
            LoadCommentsByLeadId(leadId);



        }


    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">



    <div class="md-card uk-width-large-9-10 uk-container-center">
        <div class="md-card-content">


            <div class="uk-form-row">
                <div class="uk-grid">


                    <div class="uk-width-medium-1-4">
                        <div class="uk-input-group">
                            <span class="uk-input-group-addon"><i class="uk-input-group-icon uk-icon-calendar"></i></span>
                            <label for="uk_dp_1">מתאריך</label>
                            <input class="md-input" type="text" id="txtStartDate" data-uk-datepicker="{format:'DD.MM.YYYY'}">
                        </div>
                    </div>
                    <div class="uk-width-medium-1-4">
                        <div class="uk-input-group">
                            <span class="uk-input-group-addon"><i class="uk-input-group-icon uk-icon-calendar"></i></span>
                            <label for="uk_dp_1">עד תאריך</label>
                            <input class="md-input" type="text" id="txtEndDate" data-uk-datepicker="{format:'DD.MM.YYYY'}">
                        </div>
                    </div>

                    <div class="uk-width-medium-1-4">

                        <div class="uk-input-group">
                            <span class="uk-input-group-addon"><i class="uk-input-group-icon  uk-icon-list-ul"></i></span>
                            <select id="ddlCategories" class="selectAlter" name="select_adv_single">
                            </select>
                        </div>
                    </div>

                    <div class="uk-width-medium-1-4">
                        <div class="uk-input-group">
                            <span class="uk-input-group-addon"><i class="uk-input-group-icon  uk-icon-list-ul"></i></span>
                            <select id="ddlStatus" class="selectAlter ddlStatus" name="select_adv_single">
                                <option value="0">הכל</option>
                            </select>
                        </div>
                    </div>

                    <div class="uk-width-medium-1-4">
                        <div class="uk-input-group">
                            <span class="uk-input-group-addon"><i class="uk-input-group-icon  uk-icon-text-height"></i></span>
                            <label>טקסט לחיפוש</label>
                            <input type="text" class="md-input md-input-success" id="txtSearch" value="" />
                            <span class="uk-form-help-block md-color-light-green-900">(ניתן לחפש גם לפי טלפון שנחשף)</span>
                        </div>
                    </div>

                    <div class="uk-width-medium-1-4">
                        <div class="uk-input-group">
                            <span class="uk-input-group-addon"><i class="uk-input-group-icon  uk-icon-list-ul"></i></span>
                            <select id="ddlLeedsType" name="select_adv_single" class="selectAlter">
                                <option value="">הצג את כל הלידים</option>
                                <option value="1">הצג רק לידים שנחשפו</option>
                                <option value="-1">הצג לידים שלא נחשפו</option>
                            </select>
                        </div>
                    </div>
                      <div class="uk-width-medium-1-4">
                        <div class="uk-input-group">
                            <span class="uk-input-group-addon"><i class="uk-input-group-icon  uk-icon-list-ul"></i></span>
                            <select id="ddlLeedsSug" name="select_adv_single" class="selectAlter">
                                <option value="0" selected>כל הלידים</option>
                                 <option value="-1">לידים ללא מוביל</option>
                                <option value="1">לידים עם בחירת מוביל</option>
                                <option value="2">לידים אחרי דירוג</option>
                            </select>
                        </div>
                    </div>
                    <div class="uk-width-medium-1-4" style="padding-top: 10px;">
                        <div class="uk-grid">

                            <div class="uk-width-medium-1-2">
                                <a class="md-btn md-btn-primary md-btn-block md-btn-wave-light md-btn-icon" href="javascript:FillData()">
                                    <i class="uk-icon-search"></i>
                                    חיפוש
                                </a>
                            </div>

                           <%-- <div class="uk-width-medium-1-3">
                                <a class="md-btn md-btn-success md-btn-block md-btn-wave-light md-btn-icon" href="javascript:void(0)">
                                    <i class="uk-icon-file-excel-o"></i>
                                    ייצוא לאקסל
                                </a>
                            </div>--%>
                            <div class="uk-width-medium-1-2">
                                <a class="md-btn md-btn-warning md-btn-block md-btn-wave-light md-btn-icon" href="javascript:void(0)">
                                    <i class="uk-icon-print"></i>
                                    הדפסה
                                </a>
                            </div>
                        </div>
                    </div>


                </div>
            </div>

        </div>
    </div>

    <br />



    <div>
            <div class="mikra">
            <div class="dvWithMovil" >
               לידים עם בחירת מוביל
            </div>

             <div class="dvWithDirug" >
               לידים אחרי דירוג
            </div>

        </div>
        <ul class="uk-pagination">
       
        </ul>

        <div class="md-card-list-wrapper" id="mailbox">
            <div class="uk-width-large-9-10 uk-container-center">
                <div class="md-card-list">

                    <ul class="hierarchical_slide" id="dvReqContainer">


                        <li>
                            <span class="uk-form-help-block" style="font-size: 24px;">לא נמצאו תוצאות לחיפוש !</span>
                        </li>




                    </ul>
                </div>


            </div>
        </div>

    </div>


    <div id="dvReqTemplate" style="display: none">

        <li id="li_@lId" style="background-color:@Color">
            <div class="md-card-list-item-menu" data-uk-dropdown="{mode:'click',pos:'bottom-left'}">
                <a href="#" class="md-icon material-icons">&#xE5D4;</a>
                <div class="uk-dropdown uk-dropdown-small">
                    <ul class="uk-nav">
                        <li><a href="#"><i class="material-icons">&#xE15E;</i> Reply</a></li>
                        <li><a href="#"><i class="material-icons">&#xE149;</i> Archive</a></li>
                        <li><a href="#"><i class="material-icons">&#xE872;</i> Delete</a></li>
                    </ul>
                </div>
            </div>
            <span class="md-card-list-item-date uk-text-bold md-color-light-green-900">@insert_date_onlyDate - @insert_date_onlyTime</span>
            <div class="md-card-list-item-select">
                <input type="checkbox" data-md-icheck />
            </div>
            <div class="md-card-list-item-avatar-wrapper noOpen" onclick="CreateDatatable(@lId)">
                <img src="../assets/img/avatars/avatar_02_tn.png" class="md-card-list-item-avatar" alt="" />
            </div>


            <div class="md-card-list-item-sender">
                <span class="uk-text-bold">@fullName</span>


            </div>
            <div class="md-card-list-item-subject">
                <div class="uk-form-row" style="line-height: 34px;">
                    <div class="uk-grid">
                        <%--  <div class="uk-width-medium-1-4">
                                            <div class="md-card-list-item-sender-small">
                                                <span>לקוח מתל אביב</span>
                                            </div>
                                        </div>--%>
                       <div class="uk-width-medium-1-3 noOpen">
                          <a href="https://api.whatsapp.com/send?phone=@phone&text=הי" target="_blank"><i class="uk-icon-whatsapp uk-icon-medium md-color-light-green-900"></i>&nbsp;  &nbsp;  <span>@phone</span></a>  
                        </div>
                        <div class="uk-width-medium-1-3">
                            <span class="uk-text-bold spcategoryDesc"></span>
                        </div>

                        <div class="uk-width-medium-1-3">

                            <div class="uk-form-stacked k-rtl">
                                <select id="ddlStatusInner_@lId" class="uk-form-width-medium ddlStatus" <%--onchange="SaveData(@lId,3)"--%>>
                                </select>
                            </div>
                        </div>
                    </div>
                </div>


            </div>




        </li>



    </div>

    <div id="dvReqTemplateEmpty" style="display: none">
        <li>
            <span class="uk-form-help-block" style="font-size: 24px;">לא נמצאו תוצאות לחיפוש !</span>
        </li>

    </div>

    <div id="dvReqTemplateInner" style="display: none">
        <div class="md-card-list-item-content-wrapper" style="background-color:white;">
            <div style="border-top: solid 1px gray; margin-top: 10px;">
                <div class="uk-grid">

                    <div class="uk-width-medium-1-1" style="padding-top: 10px;">

                        <div class="uk-alert uk-alert-success uk-text-italic " data-uk-alert>

                            <div class="uk-grid">
                                <div class="uk-width-medium-1-3">
                                    <h3>בחירת ספק</h3>
                                </div>

                                <div class="uk-width-medium-1-3">
                                    <h3>פרטי ליד</h3>
                                </div>

                                <div class="uk-width-medium-1-3">
                                    <h3>הערות ושינויים</h3>
                                </div>

                            </div>
                        </div>
                    </div>

                    <div class="uk-width-medium-1-3">


                        <div class="uk-grid" style="padding-top: 7px;">

                            <div id="dvNehagimList_@lId">
                            </div>


                       

                            <div class="uk-width-medium-1-1">
                                <br />
                            </div>
                        
                            <div class="uk-width-medium-1-1" style="">

                                <div class="uk-input-group">
                                    <label>תאריך</label>
                                    <input class="md-input" type="text" id="txteventDate_@lId" data-uk-datepicker="{format:'DD.MM.YYYY'}">
                                </div>


                            </div>

                            <div class="uk-width-medium-1-1" style="">

                                <div class="uk-input-group">
                                    <label>סכום</label>
                                    <input class="md-input" type="number" id="txtCost_@lId">
                                </div>


                            </div>



                            <div class="uk-width-medium-1-1" style="">
                                <br />
                            </div>
                            <div class="uk-width-medium-1-1" style="">

                                <input type="checkbox" data-switchery-size="large" id="NoDeal_@lId" onchange="ChangeSagar(@lId)" />
                                <label for="NoDeal_@lId" class="inline-label uk-text-bold">לא סגר</label>

                            </div>








                            <style>
                                .star-cb-group {
                                    /* remove inline-block whitespace */
                                    font-size: 0;
                                    /* flip the order so we can use the + and ~ combinators */
                                    unicode-bidi: bidi-override;
                                    direction: rtl;
                                    /* the hidden clearer */
                                }

                                .starLabel {
                                    font-size: 20px;
                                    font-style: italic;
                                    cursor: pointer;
                                }

                                .starLabelCheck {
                                    font-size: 20px;
                                    text-decoration:underline;
                                    font-style: italic;
                                    cursor: pointer;
                                    font-weight:bold;
                                }

                                .star::before {
                                    content: "★";
                                    color: gray;
                                    font-size: 2rem;
                                    cursor: pointer;
                                }


                                .starCheck::before {
                                    content: "★";
                                    color: #e52;
                                    cursor: pointer;
                                    font-size: 2rem;
                                }



                                .star-cb-group * {
                                    font-size: 3rem;
                                }

                                .star-cb-group > input {
                                    display: none;
                                }

                                    .star-cb-group > input + label {
                                        /* only enough room for the star */
                                        display: inline-block;
                                        overflow: hidden;
                                        text-indent: 9999px;
                                        width: 1em;
                                        white-space: nowrap;
                                        cursor: pointer;
                                    }

                                        .star-cb-group > input + label:before {
                                            display: inline-block;
                                            text-indent: -9999px;
                                            content: "☆";
                                            color: #888;
                                        }

                                        .star-cb-group > input:checked ~ label:before, .star-cb-group > input + label:hover ~ label:before, .star-cb-group > input + label:hover:before {
                                            content: "★";
                                            color: #e52;
                                            text-shadow: 0 0 1px #333;
                                        }

                                .star-cb-group > .star-cb-clear + label {
                                    text-indent: -9999px;
                                    width: .5em;
                                    margin-left: -.5em;
                                }

                                    .star-cb-group > .star-cb-clear + label:before {
                                        width: .5em;
                                    }

                                .star-cb-group:hover > input + label:before {
                                    content: "☆";
                                    color: #888;
                                    text-shadow: none;
                                }

                                .star-cb-group:hover > input + label:hover ~ label:before, .star-cb-group:hover > input + label:hover:before {
                                    content: "★";
                                    color: #e52;
                                    text-shadow: 0 0 1px #333;
                                }
                            </style>


                            <div class="uk-width-medium-1-1" style="">
                                <br />
                                <br />
                                <a class="md-btn md-btn-primary  md-btn-wave-light md-btn-icon" onclick="SaveData(@lId,2)">
                                    <i class="uk-icon-save"></i>
                                    שמירת נתונים
                                </a>
                            </div>

                        </div>


                    </div>

                    <div class="uk-width-medium-1-3">
                        <div class="uk-grid" style="padding-top: 7px;">

                            <div class="uk-width-medium-1-1">

                                <div class="uk-alert uk-alert-info uk-text-italic" id="txtAllDetails_@lId" data-uk-alert>
                                    <span></span>
                                </div>
                            </div>




                            <div class="uk-width-medium-1-1">
                                <br />
                            </div>
                            <div class="uk-width-medium-1-2 uk-text-italic">
                                <h4><u>הערות</u> </h4>
                            </div>
                            <div class="uk-width-medium-1-1">
                            </div>
                            <div class="uk-width-medium-2-2">
                                <div class="uk-form-row">

                                    <textarea class="md-input" id="txtComment_@lId" placeholder="לחצי כאן בכדי להכניס הערה..."></textarea>
                                </div>
                            </div>

                            <div class="uk-width-medium-1-1">
                                <br />
                                <a class="md-btn md-btn-primary md-btn-block md-btn-wave-light md-btn-icon" onclick="SaveDataComment(@lId)">
                                    <i class="uk-icon-edit"></i>
                                    הוסף הערה
                                </a>
                            </div>
                        </div>

                    </div>

                    <div class="uk-width-medium-1-3">
                        <div style="padding-top: 7px; height: 400px; overflow: auto" id="dvMainComments_@lId">
                        </div>





                    </div>
                </div>
            </div>

        </div>

    </div>


    <div id="dvReqComments" style="display: none">

        <div class="uk-alert uk-alert-info uk-text-italic " data-uk-alert>

            <span id="spLogUserName" class="uk-text-bold"><u>@fName</u></span>:<br />
            <div class="dvUserComment">@uDesc</div>

        </div>
    </div>

    <div id="dvNehagimTemplate" style="display: none">
        <div class="uk-width-medium-1-1" onclick="CheckNahag(@lId,@uid)">
            <span id="spStar_@lId_@uid" class="@ClassStar"></span>
            <span id="spLabel_@lId_@uid" class="@ClassLabel">@fName</span>
        </div>
    </div>

        <!-- Modal -->
  <div class="modal fade" id="myModal" role="dialog">
    <div class="modal-dialog" style="width:60%">

      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <%--<button type="button" class="close" data-dismiss="modal">&times;</button>
          <h4 style="color:red;"><span class="glyphicon glyphicon-lock"></span> Login</h4>--%>
        </div>
        <div class="modal-body">
          <div class="table-responsive" dir="rtl">
                <table id="items" class="display sh_table" style="width:100%">
                    <thead>
                        <tr>
                            <th style="text-align:center" class="th-table text-white">שם</th>
                            <th style="text-align:center" class="th-table text-white">טלפון</th>
                            <th style="text-align:center" class="th-table text-white">סיווג</th>
                            <th style="text-align:center" class="th-table text-white">שם שותף</th>
                            <th style="text-align:center" class="th-table text-white">מקור הליד</th>
                            <th style="text-align:center" class="th-table text-white">תאריך כניסה</th>
                            <th style="text-align:center" class="th-table text-white">סה"כ הכנסה</th>
                            <th style="text-align:center" class="th-table text-white">מובילים</th>
                            <th style="text-align:center" class="th-table text-white">מספר מובילים</th>
                        </tr>
                    </thead>
                </table>
            </div>
        </div>
        <div class="modal-footer">          
        </div>
      </div>
    </div>
  </div>


    <script src="../assets/js/pages/page_mailbox.js"></script>

</asp:Content>
