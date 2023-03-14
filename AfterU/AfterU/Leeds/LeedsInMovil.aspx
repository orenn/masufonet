<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="LeedsInMovil.aspx.cs" Inherits="Leeds_LeedsInMovil" %>

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

            mydata = Ajax("GetLeedsInMovil", "Type=0&StartDate=" + StartDate + "&EndDate=" + EndDate + "&Status=" + Status +
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
                ReqHtml = ReqHtml.replace(/@Movil_Name/g, mydata[i].Movil_Name);
                ReqHtml = ReqHtml.replace(/@eventDate/g, mydata[i].hovala_date_onlyDate);

                var Movil_uid = mydata[i].Movil_uid;
                var dirug = mydata[i].dirug;
                var Color = "";

                //// בחירת מוביל ללא דירוג
                //if (Movil_uid > 0 && !dirug) {

                //    Color = "#e6ffff";
                //}

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

        function OpenMailBox(elem) {




            var id = $(elem).attr("id");

            if ($("#" + id).find(".md-card-list-item-content-wrapper").length == 0) {
               
                id = id.replace("li_", "");
                var ReqHtml = $("#dvReqTemplateInner").html();

                var mydataRow = mydata.filter(x => x.db_ident == id);

                var Movil_uid = mydataRow[0].Movil_uid;


                ReqHtml = ReqHtml.replace(/@lId/g, id);
                ReqHtml = ReqHtml.replace(/@Movil_Name/g, mydataRow[0].Movil_Name);
                ReqHtml = ReqHtml.replace(/@eventDate/g, mydataRow[0].hovala_date_onlyDate);
                ReqHtml = ReqHtml.replace(/@dealCost/g, mydataRow[0].dealPrice);
                ReqHtml = ReqHtml.replace(/@endPrice/g, mydataRow[0].dealPrice + mydataRow[0].addedPrice);



                $("#li_" + id).append(ReqHtml);
                if (mydataRow[0].addedPrice > 0)  $("#txtAddCost_" + id).val(mydataRow[0].addedPrice).parent().addClass("md-input-filled");
                $("#txtComment_" + id).val(mydataRow[0].Comment);


               
                var service = mydataRow[0].service;
                var quality = mydataRow[0].quality;
                var timeto = mydataRow[0].timeto;
                var ratio = mydataRow[0].ratio;



                if (service > 0)  $('#star' + service + '_' + id).prop('checked', true);
                if (quality > 0) $('#star' + (eval(quality) + 5) + '_' + id).prop('checked', true);
                if (timeto > 0) $('#star' + (eval(timeto) + 10) + '_' + id).prop('checked', true);
                if (ratio > 0) $('#star' + (eval(ratio) + 15) + '_' + id).prop('checked', true);


                LoadCommentsByLeadId(id);
                StartDirug(id);
                Dirug(id);


            }





        }


        function LoadCommentsByLeadId(id) {


            var mycomments = Ajax("Crm_GetSetLeadChooseMovil", "Type=4&leadId=" + id +
                "&dealPrice=" + "" + "&eventDate=" + "" + "&uId=" + "" + "&sId=" + "" + "&addedPrice=" + "" + "&service=" + "" +
                "&quality=" + "" + "&timeto=" + "" + "&ratio=" +
                "&dirug=" + "" + "&Comment=" + ""
            );


            $("#dvMainComments_" + id).html("");

            if (mycomments.length > 0) {


                for (var i = 0; i < mycomments.length; i++) {




                    var ReqCommentHtml = $("#dvReqComments").html();
                    ReqCommentHtml = ReqCommentHtml.replace(/@lId/g, id);
                    //ReqCommentHtml = ReqCommentHtml.replace(/@uId/g, mycomments[i].uId);
                    ReqCommentHtml = ReqCommentHtml.replace(/@fName/g, mycomments[i].fName + ' ' + mycomments[i].insert_date);
                    ReqCommentHtml = ReqCommentHtml.replace(/@uDesc/g, mycomments[i].Comment);
                    $("#dvMainComments_" + id).append(ReqCommentHtml);



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


        function StartDirug(id) {


           
                var service = $('input[name="service_' + id + '"]:checked').val();
                var quality = $('input[name="quality_' + id + '"]:checked').val();
                var timeto = $('input[name="timeto_' + id + '"]:checked').val();
                var ratio = $('input[name="ratio_' + id + '"]:checked').val();

                if (!service) service = 0;
                if (!quality) quality = 0;
                if (!timeto) timeto = 0;
                if (!ratio) ratio = 0;

                var total = (eval(service) + eval(quality) + eval(timeto) + eval(ratio)) / 4;

                $('#dirug_' + id).text(total);


           




        }


        function Dirug(id) {

            
            $('input[type=radio]').change(function () {
                var service = $('input[name="service_' + id+'"]:checked').val();
                var quality = $('input[name="quality_' + id +'"]:checked').val();
                var timeto = $('input[name="timeto_' + id +'"]:checked').val();
                var ratio = $('input[name="ratio_' + id +'"]:checked').val();

                if (!service) service = 0;
                if (!quality) quality = 0;
                if (!timeto) timeto = 0;
                if (!ratio) ratio = 0;

                var total = (eval(service) + eval(quality) + eval(timeto) + eval(ratio)) / 4;
             
                $('#dirug_' + id).text(total);


            });


           

        }


        function SaveData(leadId, type) {

            //var value = 5;
            //$("input[name=service][value=" + value + "]").attr('checked', 'checked');
           // $("input[name='service'][value=" + value + "]").prop('checked', true);

         

            var addedPrice = $("#txtAddCost_" + leadId).val();
            var Comment = $("#txtComment_" + leadId).val();

            var service = $('input[name="service_' + leadId+'"]:checked').val();
            var quality = $('input[name="quality_' + leadId +'"]:checked').val();
            var timeto = $('input[name="timeto_' + leadId +'"]:checked').val();
            var ratio = $('input[name="ratio_' + leadId +'"]:checked').val();

            if (!service) service = 0;
            if (!quality) quality = 0;
            if (!timeto) timeto = 0;
            if (!ratio) ratio = 0;

            var dirug = $('#dirug_' + leadId).text();

         



            var res = Ajax("Crm_GetSetLeadChooseMovil", "Type=3&leadId=" + leadId +
                "&dealPrice=" + "" + "&eventDate=" + "" + "&uId=" + "" + "&sId=" + "" + "&addedPrice=" + addedPrice + "&service=" + service +
                "&quality=" + quality + "&timeto=" + timeto + "&ratio=" + ratio +
                "&dirug=" + dirug + "&Comment=" + Comment
            );


            LoadCommentsByLeadId(leadId);
            UIkit.modal.alert('נתונים נשמרו בהצלחה!', {
                labels: { Ok: 'אישור' }
            });


        }


        function AddToPrice(leadId) {

           

            var addCostValue = $("#txtAddCost_" + leadId).val();
            if (!addCostValue) {
                addCostValue = 0;
            }


            
            var dealCost = $("#spdealCost_" + leadId).text();
           // var endPrice = $("#spendPrice_" + leadId).text();


            var total = eval(addCostValue) + eval(dealCost);

            $("#spendPrice_" + leadId).text(total);


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





            }

        }


        function ChangeSagar(leadId) {

            if ($("#NoDeal_" + leadId).is(":checked")) {
                $('[id^="spStar_' + leadId + '"]').addClass('star').removeClass('starCheck');
                $('[id^="spLabel_' + leadId + '"]').addClass('starLabel').removeClass('starLabelCheck');

                $("#txtCost_" + leadId).val("").parent().removeClass("md-input-filled");
                $("#txteventDate_" + leadId).val("").parent().removeClass("md-input-filled");
            }

           
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

        function OpenHistory(leadId) {
            return;
            //alert(leadId);
            //UIkit.modal('#modal_overflow');
            var modal = UIkit.modal('#modal_overflow');
            modal.toggle();
         //   $('#modal_overflow').addClass('uk-open').show();
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
                                <option value="-1" selected>לידים ללא דירוג</option>
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

<%--                            <div class="uk-width-medium-1-3">
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
            <div class="md-card-list-item-avatar-wrapper noOpen" onclick="CreateDatatable(@lId)" >
                <img src="../assets/img/avatars/avatar_02_tn.png" class="md-card-list-item-avatar" onclick="OpenHistory(@lId)" alt="" />
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
                        <div class="uk-width-medium-1-4 noOpen">
                          <a href="https://api.whatsapp.com/send?phone=@phone&text=הי" target="_blank"><i class="uk-icon-whatsapp uk-icon-medium md-color-light-green-900"></i>&nbsp;  &nbsp;  <span>@phone</span></a>  
                        </div>
                        <div class="uk-width-medium-2-4">

                             <div class="uk-grid" style="padding:0px">
                             <div class="uk-width-medium-1-3" style="padding:0px">
                                 <span class="uk-text-bold spcategoryDesc"></span>
                             </div>
                             <div class="uk-width-medium-1-3" style="padding:0px" title="מוביל נבחר">
                                  <i class="checkMovil"></i>&nbsp;
                                  <span class="uk-text-bold">@Movil_Name</span>
                             </div>
                              <div class="uk-width-medium-1-3" style="padding:0px" title="תאריך הובלה">
                                  <i class="uk-input-group-icon uk-icon-calendar"></i>&nbsp;
                                 <span class="uk-text-bold">@eventDate</span>
                             </div>
                          </div>
                           
                           
                        </div>

                        <div class="uk-width-medium-1-4">

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
                                    <h3>פרטי הובלה</h3>
                                </div>

                                <div class="uk-width-medium-1-3">
                                    <h3>דירוג</h3>
                                </div>

                                <div class="uk-width-medium-1-3">
                                    <h3>לוג שינויים</h3>
                                </div>

                            </div>
                        </div>
                    </div>

                    <div class="uk-width-medium-1-3">


                        <div class="uk-grid" style="padding-top: 7px;">






                            <div class="uk-width-medium-1-1" style="">
                                <h4>
                                    <label class="uk-text-italic uk-text-bold">מוביל מבצע:&nbsp;</label><span>@Movil_Name</span></h4>
                                <h4>
                                    <label class="uk-text-italic uk-text-bold">תאריך ביצוע:&nbsp;</label><span>@eventDate</span></h4>
                                <h4>
                                    <label class="uk-text-italic uk-text-bold">מחיר:&nbsp;</label>
                                    <span id="spdealCost_@lId">@dealCost</span>&nbsp;ש"ח</h4>
                                <h4>
                                    <label class="uk-text-italic uk-text-bold">מחיר סופי:&nbsp;</label>
                                    <span id="spendPrice_@lId">@endPrice</span>&nbsp;ש"ח</h4>
                                <%--  <div class="uk-input-group">
                                    <label>תאריך</label>
                                    <input class="md-input" type="text" id="txteventDate_@lId" data-uk-datepicker="{format:'DD.MM.YYYY'}">
                                </div>--%>
                            </div>

                            <div class="uk-width-medium-1-1" style="">
                                <br />
                                <br />
                                <div class="uk-input-group">
                                    <label>תוספת מחיר להובלה</label>
                                    <input class="md-input" type="number" id="txtAddCost_@lId" onchange="AddToPrice(@lId)">
                                </div>


                            </div>



                            <div class="uk-width-medium-1-1" style="">
                                <br />
                            </div>
                            <div class="uk-width-medium-1-1" style="">

                                <div class="uk-form-row">

                                    <textarea rows="2" class="md-input" id="txtComment_@lId" placeholder="לחצי כאן להוסיף הערה...">


                                    </textarea>
                                </div>

                            </div>










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
                            <div class="uk-width-medium-1-5" style="padding:0px">
                                <h2 style="padding-top:23px"> <label class="uk-text-italic uk-text-bold" >שירות:&nbsp;</label></h2>
                            </div>
                            <div class="uk-width-medium-4-5" style="text-align:right;padding:0px">
                                <div class="rate">
                                    <input type="radio" id="star5_@lId" name="service_@lId" value="5" />
                                    <label for="star5_@lId" title="5"></label>

                                    <input type="radio" id="star4_@lId" name="service_@lId" value="4" />
                                    <label for="star4_@lId" title="4"></label>

                                    <input type="radio" id="star3_@lId" name="service_@lId" value="3" />
                                    <label for="star3_@lId" title="3"></label>

                                    <input type="radio" id="star2_@lId" name="service_@lId" value="2" />
                                    <label for="star2_@lId" title="2"></label>

                                    <input type="radio" id="star1_@lId" name="service_@lId" value="1" />
                                    <label for="star1_@lId" title="1"></label>
                                </div>
                            </div>


                            <div class="uk-width-medium-1-5" style="padding:0px">
                                <h2 style="padding-top:23px"> <label class="uk-text-italic uk-text-bold" >איכות:&nbsp;</label></h2>
                            </div>
                            <div class="uk-width-medium-4-5" style="text-align:right;padding:0px">
                                <div class="rate">
                                    <input type="radio" id="star10_@lId" name="quality_@lId" value="5" />
                                    <label for="star10_@lId" title="5"></label>
                                    <input type="radio" id="star9_@lId" name="quality_@lId" value="4" />
                                    <label for="star9_@lId" title="4"></label>
                                    <input type="radio" id="star8_@lId" name="quality_@lId" value="3" />
                                    <label for="star8_@lId" title="3"></label>
                                    <input type="radio" id="star7_@lId" name="quality_@lId" value="2" />
                                    <label for="star7_@lId" title="2"></label>
                                    <input type="radio" id="star6_@lId" name="quality_@lId" value="1" />
                                    <label for="star6_@lId" title="1"></label>
                                </div>

                              

                            </div>

                                <div class="uk-width-medium-1-5" style="padding:0px">
                                <h2 style="padding-top:23px"> <label class="uk-text-italic uk-text-bold" >זמן:&nbsp;</label></h2>
                            </div>
                            <div class="uk-width-medium-4-5" style="text-align:right;padding:0px">
                                <div class="rate">
                                    <input type="radio" id="star15_@lId" name="timeto_@lId" value="5" />
                                    <label for="star15_@lId" title="5"></label>
                                    <input type="radio" id="star14_@lId" name="timeto_@lId" value="4" />
                                    <label for="star14_@lId" title="4"></label>
                                    <input type="radio" id="star13_@lId" name="timeto_@lId" value="3" />
                                    <label for="star13_@lId" title="3"></label>
                                    <input type="radio" id="star12_@lId" name="timeto_@lId" value="2" />
                                    <label for="star12_@lId" title="2"></label>
                                    <input type="radio" id="star11_@lId" name="timeto_@lId" value="1" />
                                    <label for="star11_@lId" title="1"></label>
                                </div>

                              

                            </div>

                               <div class="uk-width-medium-1-5" style="padding:0px">
                                <h2 style="padding-top:23px"> <label class="uk-text-italic uk-text-bold" >יחס:&nbsp;</label></h2>
                            </div>
                               <div class="uk-width-medium-4-5" style="text-align:right;padding:0px">
                                <div class="rate">
                                    <input type="radio" id="star20_@lId" name="ratio_@lId" value="5" />
                                    <label for="star20_@lId" title="5"></label>
                                    <input type="radio" id="star19_@lId" name="ratio_@lId" value="4" />
                                    <label for="star19_@lId" title="4"></label>
                                    <input type="radio" id="star18_@lId" name="ratio_@lId" value="3" />
                                    <label for="star18_@lId" title="3"></label>
                                    <input type="radio" id="star17_@lId" name="ratio_@lId" value="2" />
                                    <label for="star17_@lId" title="2"></label>
                                    <input type="radio" id="star16_@lId" name="ratio_@lId" value="1" />
                                    <label for="star16_@lId" title="1"></label>
                                </div>

                              

                            </div>

                             <div class="uk-width-medium-1-1" style="padding:0px;text-align:center">
                                <h3 style="padding-top:23px"> <label class="uk-text-italic uk-text-bold" >דירוג משוקלל:&nbsp;</label> <label id="dirug_@lId">0</label></h3>
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

     
      <div id="modal_overflow" class="uk-modal">
                                <div class="uk-modal-dialog uk-modal-dialog-large">
                                    <button type="button" class="uk-modal-close uk-close"></button>
                                    <h2 class="heading_a">Some text above the scrollable box</h2>

                                     <div class="uk-grid" data-uk-grid-margin="">
                <div class="uk-width-medium-1-1">
                    <div class="md-card">
                        <div class="md-card-content">
                            <div class="uk-overflow-container">
                                <table class="uk-table uk-table-nowrap uk-table-align-vertical table_tree">
                                    <thead>
                                    <tr>
                                        <!--<th class="small_col"></th>-->
                                        <th class="uk-width-4-10" >City1/User Name</th>
                                        <th class="uk-width-2-10">Email</th>
                                        <th class="uk-width-2-10">Phone</th>
                                        <th class="uk-width-2-10 uk-text-center">Actions</th>
                                    </tr>
                                    </thead>
                                    <tbody>
                                        <tr class="table-parent-row">
                                            <!--<td class="uk-text-center uk-table-middle small_col">
                                            <input type="checkbox" data-md-icheck class="check_childrens">
                                            </td>-->
                                            <td><a href="#" class="js-toggle-children-row  toggle-childrens">Zanderbury</a></td>
                                            <td><a href="#" class="js-toggle-children-row">sdsds</a></td>
                                            <td><a href="#" class="js-toggle-children-row">גדגדג</a></td>
                                            <td><a href="#" class="js-toggle-children-row">sdsdsdsd</a></td>
                                              <td><a href="#" class="js-toggle-children-row">sdsds</a></td>
                                            <td><a href="#" class="js-toggle-children-row">גדגדג</a></td>
                                            <td><a href="#" class="js-toggle-children-row">sdsdsdsd</a></td>
                                              <td><a href="#" class="js-toggle-children-row">sdsds</a></td>
                                            <td><a href="#" class="js-toggle-children-row">גדגדג</a></td>
                                            <td><a href="#" class="js-toggle-children-row">sdsdsdsd</a></td>
                                        </tr>
                                        <tr class="table-child-row">

                                            <td colspan="7">asasasas sdsd sd sdsdsdsd
                                             <br />
                                            sdsdsdsd
                                            
                                            </td>

                                           
                                        </tr>
                                       
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
               
            </div>
                                </div>
                            </div>

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
       <style>
                                    * {
                                        margin: 0;
                                        padding: 0;
                                    }

                                    .rate {
                                       
                                        height: 46px;
                                        padding: 0 10px;
                                    }

                                        .rate:not(:checked) > input {
                                           display:none;
                                        }

                                        .rate:not(:checked) > label {
                                            float: right;
                                            width: 50px;
                                            overflow: hidden;
                                            white-space: nowrap;
                                            cursor: pointer;
                                            font-size: 30px;
                                            color: #ccc;
                                        }

                                            .rate:not(:checked) > label:before {
                                                content: '★ ';
                                                font-size: 4rem;
                                               
                                            }

                                        .rate > input:checked ~ label {
                                          /*  color: #ffc700;*/
                                           color: #e52;
                                        }

                                        .rate:not(:checked) > label:hover,
                                        .rate:not(:checked) > label:hover ~ label {
                                          /*  color: #deb217;*/
                                            color: #e52;
                                        }

                                        .rate > input:checked + label:hover,
                                        .rate > input:checked + label:hover ~ label,
                                        .rate > input:checked ~ label:hover,
                                        .rate > input:checked ~ label:hover ~ label,
                                        .rate > label:hover ~ input:checked ~ label {
                                          /*  color: #c59b08;*/
                                             color: #e52;
                                        }


                                           .checkMovil:before {
                                                content: '★';
                                                font-size: 1rem;
                                                 color: #e52;
                                               
                                            }
                                </style>
</asp:Content>
