<%@ Page Title="CORN :: Product Sale Detail" Language="C#" MasterPageFile="~/Forms/PageMaster.master"
    AutoEventWireup="true" CodeFile="RptProductSaleDetail.aspx.cs" Inherits="Forms_RptProductSaleDetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
           <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
<script language="JavaScript" type="text/javascript">
    function pageLoad() {
        $("select").searchable();
    }
        function ValidateForm() {

            return true;
        }

    function SelectAll() {
            var chkBoxList = document.getElementById('<%= LstCategory.ClientID %>');
            var chkBox = document.getElementById('<%= ChbAllCategory.ClientID %>');
            if (chkBox.checked == true) {
                var chkBoxCount = chkBoxList.getElementsByTagName("input");

                for (var i = 0; i < chkBoxCount.length; i++) {
                    chkBoxCount[i].checked = true;
                }
            }
            else {
                var chkBoxCount = chkBoxList.getElementsByTagName("input");

                for (var i = 0; i < chkBoxCount.length; i++) {
                    chkBoxCount[i].checked = false;
                }
            }
        }

        function UnCheckSelectAll() {
            var chkBox = document.getElementById('<%= ChbAllCategory.ClientID %>');
            var chkBoxList = document.getElementById('<%= LstCategory.ClientID %>');
            var chkBoxCount = chkBoxList.getElementsByTagName("input");
            var count = 0;
            for (var i = 0; i < chkBoxCount.length; i++) {
                if (chkBoxCount[i].checked == false) {
                    count += 1;
                }
            }
            if (count > 0) {
                chkBox.checked = false;
            }
            else {
                chkBox.checked = true;
            }
        }

    // Sub Category
    function SelectAllSubCategory() {
            var chkBoxList = document.getElementById('<%= LstSubCategory.ClientID %>');
            var chkBox = document.getElementById('<%= ChbAllSubCategory.ClientID %>');
            if (chkBox.checked == true) {
                var chkBoxCount = chkBoxList.getElementsByTagName("input");

                for (var i = 0; i < chkBoxCount.length; i++) {
                    chkBoxCount[i].checked = true;
                }
            }
            else {
                var chkBoxCount = chkBoxList.getElementsByTagName("input");

                for (var i = 0; i < chkBoxCount.length; i++) {
                    chkBoxCount[i].checked = false;
                }
            }
        }

    function UnCheckSelectAllSubCategory() {
            var chkBox = document.getElementById('<%= ChbAllSubCategory.ClientID %>');
            var chkBoxList = document.getElementById('<%= LstSubCategory.ClientID %>');
            var chkBoxCount = chkBoxList.getElementsByTagName("input");
            var count = 0;
            for (var i = 0; i < chkBoxCount.length; i++) {
                if (chkBoxCount[i].checked == false) {
                    count += 1;
                }
            }
            if (count > 0) {
                chkBox.checked = false;
            }
            else {
                chkBox.checked = true;
            }
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
                                             <td align="left"></td>
                                             <td align="left"></td>
                                             <td align="right">
                                                 <asp:RadioButtonList ID="rblRptType" runat="server" Width="202px" __designer:wfdid="w2"
                                                     RepeatDirection="Horizontal">
                                                     <asp:ListItem Selected="True" Value="0">Date Wise</asp:ListItem>
                                                     <asp:ListItem Value="2">Invoice Wise</asp:ListItem>
                                                 </asp:RadioButtonList>
                                             </td>
                                             <td align="left"></td>
                                             <td align="left"></td>
                                             <td align="left"></td>
                                        </tr>
                                        <tr>
                                            
                                            <td align="left" style="width: 80px;">
                                                <strong>Location</strong>
                                            </td>
                                            <td align="left"></td>
                                            <td style="height: 25px" align="left">
                                                <asp:DropDownList ID="drpDistributor" runat="server" Width="200px" CssClass="DropList"
                                                    AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left"></td>
                                            <td align="left"></td>
                                            <td align="left"></td>
                                        </tr>

                                        <tr id="catgeorySelectAllRow" runat="server">
                                            <td style="width: 90px; height: 2px" align="left">
                                                <strong>
                                                    <asp:Label ID="Label5" runat="server" Width="84px" Text="Category" CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="width: 1px; height: 2px" align="left"></td>
                                            <td style="width: 204px; height: 2px" align="left">
                                                <asp:CheckBox ID="ChbAllCategory" onclick="SelectAll()" AutoPostBack="true"
                                                    OnCheckedChanged="ChbAllCategory_CheckedChanged" Checked="true" runat="server" Text="All"></asp:CheckBox>
                                            </td>
                                            <td style="width: 90px; height: 2px" align="left">
                                                <strong>
                                                    <asp:Label ID="Label1" runat="server" Width="84px" Text="Sub Category" CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="width: 1px; height: 2px" align="left"></td>
                                            <td style="width: 204px; height: 2px" align="left">
                                                <asp:CheckBox ID="ChbAllSubCategory" onclick="SelectAllSubCategory()" Checked="true" runat="server" Text="All"></asp:CheckBox>
                                            </td>
                                        </tr>
                            <tr id="categoryRow" runat="server">
                                <td style="height: 2px" align="left" colspan="3">
                                    <asp:Panel ID="Panel1" runat="server" Width="302px" Height="150px" ScrollBars="Vertical"
                                        BorderWidth="1px" BorderStyle="Groove">
                                        <asp:CheckBoxList ID="LstCategory" AutoPostBack="true" OnSelectedIndexChanged="LstCategory_SelectedIndexChanged" onclick="UnCheckSelectAll()" runat="server">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                </td>
                                 <td style="height: 2px" align="left" colspan="3">
                                    <asp:Panel ID="Panel2" runat="server" Width="302px" Height="150px" ScrollBars="Vertical"
                                        BorderWidth="1px" BorderStyle="Groove">
                                        <asp:CheckBoxList ID="LstSubCategory" onclick="UnCheckSelectAllSubCategory()" runat="server">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                </td>
                            </tr>
                                       
                                        <tr>
                                            <td align="left">
                                                <strong>
                                                    From Date</strong>
                                            </td>
                                            <td align="left">
                                            </td>
                                            <td style="height: 25px" align="left">
                                                <asp:TextBox ID="txtStartDate" onkeyup="BlockStartDateKeyPress()" runat="server"
                                                    Width="150px" CssClass="txtBox" MaxLength="10"></asp:TextBox>
                                                <asp:ImageButton ID="ibtnStartDate" runat="server" Width="16px" ImageUrl="~/App_Themes/Granite/Images/date.gif">
                                                </asp:ImageButton>
                                            </td>
                                             <td align="left"></td>
                                            <td align="left"></td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <strong>
                                                   To Date</strong>
                                            </td>
                                            <td align="left">
                                            </td>
                                            <td style="height: 25px" align="left">
                                                <asp:TextBox ID="txtEndDate" onkeyup="BlockEndDateKeyPress()" runat="server" Width="150px"
                                                    CssClass="txtBox " MaxLength="10"></asp:TextBox>
                                                <asp:ImageButton ID="ibnEndDate" runat="server" Width="16px" ImageUrl="~/App_Themes/Granite/Images/date.gif">
                                                </asp:ImageButton>
                                            </td>
                                             <td align="left"></td>
                                            <td align="left"></td>
                                        </tr>
                                        <tr>

                                            <td>
                                                &nbsp;
                                            </td>
                                             <td align="left"></td>
                                            <td align="left"></td>
                                             <td align="left"></td>
                                            <td align="left"></td>
                                            <td align="left"></td>
                                        </tr>
                                        <%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
                                        <cc1:CalendarExtender ID="CEStartDate" runat="server" TargetControlID="txtStartDate"
                                            PopupButtonID="ibtnStartDate" Format="dd-MMM-yyyy">
                                        </cc1:CalendarExtender>
                                        <cc1:CalendarExtender ID="CEEndDate" runat="server" TargetControlID="txtEndDate"
                                            PopupButtonID="ibnEndDate" Format="dd-MMM-yyyy">
                                        </cc1:CalendarExtender>
                                    </tbody>
                                </table>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    &nbsp; &nbsp;
                    <asp:Button ID="btnViewPDF" runat="server" CssClass="Button" Text="View PDF" Width="90"
                        OnClick="btnViewPDF_Click" />
                    <asp:Button ID="btnViewExcel" runat="server" CssClass="Button" Text="View Excel"
                        Width="90" OnClick="btnViewExcel_Click" />
                </td>
            </tr>
        </table>
        &nbsp;
    </div>
</asp:Content>
