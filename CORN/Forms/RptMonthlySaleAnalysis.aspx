<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="RptMonthlySaleAnalysis.aspx.cs" Inherits="Forms_RptMonthlySaleAnalysis"
    Title="CORN :: Monthly Sale Report" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
             <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
<script language="JavaScript" type="text/javascript">
    function pageLoad() {
        $("select").searchable();
    }
      
        function onCalendarShown() {
            var cal = $find("calendar1");
            cal._switchMode("years", true);
            if (cal._yearsBody) {
                for (var i = 0; i < cal._yearsBody.rows.length; i++) {
                    var row = cal._yearsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.addHandler(row.cells[j].firstChild, "click", call);
                    }
                }
            }
        }

        function onCalendarHidden() {
            var cal = $find("calendar1");
            if (cal._yearsBody) {
                for (var i = 0; i < cal._yearsBody.rows.length; i++) {
                    var row = cal._yearsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild, "click", call);
                    }
                }
            }
        }

        function call(eventElement) {
            var target = eventElement.target;
            switch (target.mode) {
                case "year":
                    var cal = $find("calendar1");
                    cal.set_selectedDate(target.date);
                    cal._blur.post(true);
                    cal.raiseDateSelectionChanged(); break;
            }
        }


        function onCalendarShown2() {
            var cal = $find("calendar2");
            cal._switchMode("years", true);
            if (cal._yearsBody) {
                for (var i = 0; i < cal._yearsBody.rows.length; i++) {
                    var row = cal._yearsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.addHandler(row.cells[j].firstChild, "click", call2);
                    }
                }
            }
        }

        function onCalendarHidden2() {
            var cal = $find("calendar2");
            if (cal._yearsBody) {
                for (var i = 0; i < cal._yearsBody.rows.length; i++) {
                    var row = cal._yearsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild, "click", call2);
                    }
                }
            }
        }

        function call2(eventElement) {
            var target = eventElement.target;
            switch (target.mode) {
                case "year":
                    var cal = $find("calendar2");
                    cal.set_selectedDate(target.date);
                    cal._blur.post(true);
                    cal.raiseDateSelectionChanged(); break;
            }
        }


        function onCalendarShown3() {
            var cal = $find("calendar3");
            cal._switchMode("months", true);
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.addHandler(row.cells[j].firstChild, "click", call3);
                    }
                }
            }
        }

        function onCalendarHidden3() {
            var cal = $find("calendar3");

            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild, "click", call3);
                    }
                }
            }
        }

        function call3(eventElement) {
            var target = eventElement.target;
            switch (target.mode) {
                case "month":
                    var cal = $find("calendar3");
                    cal._visibleDate = target.date;
                    cal.set_selectedDate(target.date);
                    cal._switchMonth(target.date);
                    cal._blur.post(true);
                    cal.raiseDateSelectionChanged();
                    break;
            }
        }


        function onCalendarShown4() {
            var cal = $find("calendar4");
            cal._switchMode("months", true);
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.addHandler(row.cells[j].firstChild, "click", call4);
                    }
                }
            }
        }

        function onCalendarHidden4() {
            var cal = $find("calendar4");

            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild, "click", call4);
                    }
                }
            }
        }

        function call4(eventElement) {
            var target = eventElement.target;
            switch (target.mode) {
                case "month":
                    var cal = $find("calendar4");
                    cal._visibleDate = target.date;
                    cal.set_selectedDate(target.date);
                    cal._switchMonth(target.date);
                    cal._blur.post(true);
                    cal.raiseDateSelectionChanged();
                    break;
            }
        }



        function onCalendarShown5() {
            var cal = $find("calendar5");
            cal._switchMode("months", true);
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.addHandler(row.cells[j].firstChild, "click", call5);
                    }
                }
            }
        }

        function onCalendarHidden5() {
            var cal = $find("calendar5");

            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild, "click", call5);
                    }
                }
            }
        }

        function call5(eventElement) {
            var target = eventElement.target;
            switch (target.mode) {
                case "month":
                    var cal = $find("calendar5");
                    cal._visibleDate = target.date;
                    cal.set_selectedDate(target.date);
                    cal._switchMonth(target.date);
                    cal._blur.post(true);
                    cal.raiseDateSelectionChanged();
                    break;
            }
        }

    </script>
    <script language="JavaScript" type="text/javascript">
        function ValidateForm() {
            return true;
        }

    </script>
    <div id="right_data">
        <table width="100%">
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnl_rpt" runat="server">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td style="height: 12px" align="left" colspan="4">
                                                <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                            </td>
                                        </tr>
                                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" Width="210px" RepeatDirection="Horizontal"
                                            Visible="false">
                                            <asp:ListItem Selected="True">Trade Price</asp:ListItem>
                                            <asp:ListItem>Purchase Price</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <tr>
                                            <td align="left">
                                            </td>
                                            <td align="left">
                                                <strong>
                                                    <asp:Label ID="Label1" runat="server" Width="78px" Text="Report Type" CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="width: 1px" align="left">
                                            </td>
                                            <td style="width: 201px; height: 25px" align="left">
                                                <asp:DropDownList ID="DrpReportType" runat="server" Width="200px" CssClass="DropList">
                                                    <asp:ListItem>Gross Sale </asp:ListItem>
                                                    <asp:ListItem>Sales Return </asp:ListItem>
                                                    <asp:ListItem>Purchase</asp:ListItem>                                                  
                                                    <asp:ListItem>Transfer In</asp:ListItem>
                                                    <asp:ListItem>Transfer Out</asp:ListItem>                                                    
                                                    <asp:ListItem>Discount</asp:ListItem>
                                                    <asp:ListItem>Purchase Return</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                            </td>
                                            <td align="left">
                                                <strong>
                                                    <asp:Label ID="Label5" runat="server" Width="78px" Text="Value Type" CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="width: 1px" align="left">
                                            </td>
                                            <td style="width: 201px; height: 25px" align="left">
                                                <asp:DropDownList ID="DrpUnitType" runat="server" Width="200px" CssClass="DropList"
                                                    AutoPostBack="True" OnSelectedIndexChanged="DrpUnitType_SelectedIndexChanged">
                                                    <asp:ListItem Value="0">Year Wise</asp:ListItem>
                                                    <asp:ListItem Value="1">Month Wise</asp:ListItem>
                                                    <asp:ListItem Value="2">Date Wise</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                            </td>
                                            <td align="left">
                                                <strong>
                                                    <asp:Label ID="lbltoLocation" runat="server" Width="66px" Text="Location" CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="width: 1px" align="left">
                                            </td>
                                            <td style="width: 201px; height: 25px" align="left">
                                                <asp:DropDownList ID="drpDistributor" runat="server" Width="200px" CssClass="DropList">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                            </td>
                                            <td align="left">
                                                <strong>
                                                    <asp:Label ID="Label6" runat="server" Width="78px" Text="Principal" CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="width: 1px" align="left">
                                            </td>
                                            <td style="width: 201px; height: 25px" align="left">
                                                <asp:DropDownList ID="DrpPrincipal" runat="server" Width="200px" CssClass="DropList">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 68px" align="left" colspan="4">
                                                <div id="divYear" class="divYear" runat="server">
                                                    <table width="100%">
                                                        <tbody>
                                                            <tr>
                                                                <td style="padding-left: 5px; width: 8px; height: 25px">
                                                                    <strong>
                                                                        <asp:Label ID="Label3" runat="server" Width="70px" Text="From Year"></asp:Label></strong>
                                                                </td>
                                                                <td style="width: 20px">
                                                                </td>
                                                                <td style="padding-left: 7px; width: 204px; height: 25px" align="left">
                                                                    &nbsp;
                                                                    <asp:TextBox ID="txtStartYear" onkeyup="BlockStartDateKeyPress()" runat="server"
                                                                        Width="50px" CssClass="txtBox" MaxLength="10"></asp:TextBox>
                                                                    <asp:ImageButton ID="ibtnStartYear" runat="server" Width="16px" ImageUrl="~/App_Themes/Granite/Images/date.gif">
                                                                    </asp:ImageButton>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left: 5px; width: 8px">
                                                                    <strong>
                                                                        <asp:Label ID="Label4" runat="server" Width="80px" Text="To Year"></asp:Label></strong>
                                                                </td>
                                                                <td style="width: 10px">
                                                                </td>
                                                                <td style="padding-left: 7px; width: 204px; height: 25px">
                                                                    &nbsp;
                                                                    <asp:TextBox ID="txtEndYear" onkeyup="BlockEndDateKeyPress()" runat="server" Width="50px"
                                                                        CssClass="txtBox " MaxLength="10"></asp:TextBox>
                                                                    <asp:ImageButton ID="ibtnEndYear" runat="server" Width="16px" ImageUrl="~/App_Themes/Granite/Images/date.gif">
                                                                    </asp:ImageButton>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left: 5px; width: 8px;">
                                                                    <strong>
                                                                        <asp:Label ID="lblMonth" runat="server" Width="78px" Text="Month" CssClass="lblbox"></asp:Label></strong>
                                                                </td>
                                                                <td style="width: 10px;">
                                                                </td>
                                                                <td style="padding-left: 7px; width: 204px; height: 25px">
                                                                    &nbsp;<asp:TextBox ID="txtMonth" onkeyup="BlockStartDateKeyPress()" runat="server"
                                                                        Width="60px" CssClass="txtBox" MaxLength="10"></asp:TextBox>
                                                                    <asp:ImageButton ID="ImageButton1" runat="server" Width="16px" ImageUrl="~/App_Themes/Granite/Images/date.gif">
                                                                    </asp:ImageButton>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                                <div id="divDate" class="divDate" runat="server">
                                                    <table width="100%">
                                                        <tbody>
                                                            <tr style="display:none;">
                                                                <td style="padding-left: 5px; width: 8px; height: 25px">
                                                                    <strong>Year</strong>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td style="padding-left: 7px; width: 220px; height: 25px">
                                                                    <asp:DropDownList ID="DrpYear" runat="server" AutoPostBack="false" OnSelectedIndexChanged="DrpYear_SelectedIndexChanged">
                                                                        <asp:ListItem Text="2013" Value="2013"></asp:ListItem>
                                                                        <asp:ListItem Text="2014" Value="2014"></asp:ListItem>
                                                                        <asp:ListItem Text="2015" Value="2015"></asp:ListItem>
                                                                        <asp:ListItem Text="2016" Value="2016"></asp:ListItem>
                                                                        <asp:ListItem Text="2017" Value="2017"></asp:ListItem>
                                                                        <asp:ListItem Text="2018" Value="2018"></asp:ListItem>
                                                                        <asp:ListItem Text="2019" Value="2019"></asp:ListItem>
                                                                        <asp:ListItem Text="2020" Value="2020"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:DropDownList ID="DrpWeekFrom" runat="server">
                                                                        <asp:ListItem Text="Week 1" Selected="True" Value="1"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 2" Value="2"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 3" Value="3"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 4" Value="4"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 5" Value="5"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 6" Value="6"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 7" Value="7"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 8" Value="8"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 9" Value="9"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 10" Value="10"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 11" Value="11"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 12" Value="12"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 13" Value="13"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 14" Value="14"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 15" Value="15"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 16" Value="16"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 17" Value="17"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 18" Value="18"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 19" Value="19"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 20" Value="20"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 21" Value="21"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 22" Value="22"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 23" Value="23"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 24" Value="24"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 25" Value="25"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 26" Value="26"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 27" Value="27"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 28" Value="28"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 29" Value="29"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 30" Value="30"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 31" Value="31"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 32" Value="32"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 33" Value="33"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 34" Value="34"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 35" Value="35"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 36" Value="36"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 37" Value="37"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 38" Value="38"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 39" Value="39"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 40" Value="40"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 41" Value="41"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 42" Value="42"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 43" Value="43"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 44" Value="44"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 45" Value="45"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 46" Value="46"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 47" Value="47"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 48" Value="48"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 49" Value="49"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 50" Value="50"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 51" Value="51"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 52" Value="52"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:DropDownList ID="DrpWeekTo" runat="server">
                                                                        <asp:ListItem Text="Week 1" Selected="True" Value="1"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 2" Value="2"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 3" Value="3"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 4" Value="4"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 5" Value="5"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 6" Value="6"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 7" Value="7"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 8" Value="8"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 9" Value="9"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 10" Value="10"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 11" Value="11"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 12" Value="12"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 13" Value="13"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 14" Value="14"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 15" Value="15"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 16" Value="16"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 17" Value="17"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 18" Value="18"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 19" Value="19"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 20" Value="20"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 21" Value="21"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 22" Value="22"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 23" Value="23"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 24" Value="24"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 25" Value="25"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 26" Value="26"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 27" Value="27"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 28" Value="28"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 29" Value="29"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 30" Value="30"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 31" Value="31"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 32" Value="32"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 33" Value="33"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 34" Value="34"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 35" Value="35"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 36" Value="36"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 37" Value="37"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 38" Value="38"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 39" Value="39"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 40" Value="40"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 41" Value="41"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 42" Value="42"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 43" Value="43"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 44" Value="44"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 45" Value="45"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 46" Value="46"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 47" Value="47"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 48" Value="48"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 49" Value="49"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 50" Value="50"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 51" Value="51"></asp:ListItem>
                                                                        <asp:ListItem Text="Week 52" Value="52"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left: 5px; width: 8px; height: 25px">
                                                                    <strong>
                                                                        <asp:Label ID="lblFromDate" runat="server" Width="78px" Text="From Date"></asp:Label></strong>
                                                                </td>
                                                                <td style="width: 20px">
                                                                </td>
                                                                <td style="padding-left: 7px; width: 204px; height: 25px" align="left">
                                                                    &nbsp;
                                                                    <asp:TextBox ID="txtStartDate" runat="server" Width="100px" CssClass="txtBox" MaxLength="11"></asp:TextBox>
                                                                     <asp:ImageButton ID="ibtnStartDate" runat="server" Width="16px"  ImageUrl="~/App_Themes/Granite/Images/date.gif">
                                                                        </asp:ImageButton>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left: 5px; width: 8px; height: 25px">
                                                                    <strong>
                                                                        <asp:Label ID="lblToDate" runat="server" Width="78px" Text="To Date" CssClass="lblbox"></asp:Label></strong>
                                                                </td>
                                                                <td style="width: 1px">
                                                                </td>
                                                                <td style="padding-left: 7px; width: 204px; height: 25px">
                                                                    &nbsp;
                                                                    <asp:TextBox ID="txtEndDate" runat="server" Width="100px" CssClass="txtBox" MaxLength="11"></asp:TextBox>
                                                                     <asp:ImageButton ID="ibtnEndDate" runat="server" Width="16px" Enabled="true" ImageUrl="~/App_Themes/Granite/Images/date.gif">
                                                                        </asp:ImageButton>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                                <div id="divMonth" class="divMonth" runat="server">
                                                    <table width="100%">
                                                        <tbody>
                                                            <tr>
                                                                <td style="padding-left: 5px; width: 8px; height: 25px">
                                                                    <strong>
                                                                        <asp:Label ID="lblFromMonth" runat="server" Width="78px" Text="From Month" CssClass="lblbox"></asp:Label></strong>
                                                                </td>
                                                                <td style="width: 20px">
                                                                </td>
                                                                <td style="padding-left: 7px; width: 204px; height: 25px" align="left">
                                                                    &nbsp;<asp:TextBox ID="txtFromMonth" onkeyup="BlockStartDateKeyPress()" runat="server"
                                                                        Width="70px" CssClass="txtBox" MaxLength="10"></asp:TextBox>
                                                                    <asp:ImageButton ID="ibtnStartMonth" runat="server" Width="16px" ImageUrl="~/App_Themes/Granite/Images/date.gif">
                                                                    </asp:ImageButton>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left: 5px; width: 8px; height: 25px">
                                                                    <strong>
                                                                        <asp:Label ID="lblToMonth" runat="server" Width="78px" Text="To Month" CssClass="lblbox"></asp:Label></strong>
                                                                </td>
                                                                <td style="width: 1px; height: 25px">
                                                                </td>
                                                                <td style="padding-left: 7px; width: 204px; height: 25px">
                                                                    &nbsp;<asp:TextBox ID="txtToMonth" onkeyup="BlockStartDateKeyPress()" runat="server"
                                                                        Width="70px" CssClass="txtBox" MaxLength="10"></asp:TextBox>
                                                                    <asp:ImageButton ID="ibtnEndMonth" runat="server" Width="16px" ImageUrl="~/App_Themes/Granite/Images/date.gif">
                                                                    </asp:ImageButton>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
                                        <cc1:CalendarExtender ID="CEStartYear" runat="server" BehaviorID="calendar1" OnClientShown="onCalendarShown"
                                            OnClientHidden="onCalendarHidden" Format="yyyy" PopupButtonID="ibtnStartYear"
                                            TargetControlID="txtStartYear">
                                        </cc1:CalendarExtender>
                                        <cc1:CalendarExtender ID="CEEndYear" runat="server" BehaviorID="calendar2" OnClientShown="onCalendarShown2"
                                            OnClientHidden="onCalendarHidden2" Format="yyyy" PopupButtonID="ibtnEndYear"
                                            TargetControlID="txtEndYear">
                                        </cc1:CalendarExtender>
                                        <cc1:CalendarExtender ID="CEStartMonth" runat="server" BehaviorID="calendar3" OnClientShown="onCalendarShown3"
                                            OnClientHidden="onCalendarHidden3" Format="MMM-yyyy" PopupButtonID="ibtnStartMonth"
                                            TargetControlID="txtFromMonth">
                                        </cc1:CalendarExtender>
                                        <cc1:CalendarExtender ID="CESEndMonth" runat="server" BehaviorID="calendar4" OnClientShown="onCalendarShown4"
                                            OnClientHidden="onCalendarHidden4" Format="MMM-yyyy" PopupButtonID="ibtnEndMonth"
                                            TargetControlID="txtToMonth">
                                        </cc1:CalendarExtender>
                                        <cc1:CalendarExtender ID="CEStartDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnStartDate"
                                            TargetControlID="txtStartDate">
                                        </cc1:CalendarExtender>
                                        <cc1:CalendarExtender ID="CEEndDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnEndDate"
                                            TargetControlID="txtEndDate">
                                        </cc1:CalendarExtender>
                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" BehaviorID="calendar5"
                                            OnClientShown="onCalendarShown5" OnClientHidden="onCalendarHidden5" Format="MMM"
                                            PopupButtonID="ImageButton1" TargetControlID="txtMonth">
                                        </cc1:CalendarExtender>
                                        <caption>
                                            &nbsp;&nbsp;&nbsp;
                                        </caption>
                                    </tbody>
                                </table>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    &nbsp; &nbsp;
                    <asp:Button ID="btnViewPDF" runat="server" CssClass="Button" Text="View PDF" OnClick="btnViewPDF_Click" />
                    <asp:Button ID="btnViewExcel" runat="server" CssClass="Button" Text="View Excel"
                        OnClick="btnViewExcel_Click" />
                </td>
            </tr>
        </table>
        <!-- Visible=false Location Type-->
        &nbsp;
        <tr>
            <td align="left">
            </td>
            <td align="left">
                <strong>
                    <asp:Label ID="Label2" runat="server" Width="78px" Text="Location Type" CssClass="lblbox"
                        Visible="false"></asp:Label></strong>
            </td>
            <td style="width: 1px" align="left">
            </td>
            <td style="width: 201px; height: 25px" align="left">
                <asp:DropDownList ID="ddDistributorType" runat="server" Width="200px" CssClass="DropList"
                    Visible="false" AutoPostBack="True" OnSelectedIndexChanged="ddDistributorType_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
    </div>
</asp:Content>
