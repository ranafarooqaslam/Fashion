<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="RptSalesPurchaseFormat.aspx.cs" Inherits="Forms_RptSalesPurchaseFormat"
    Title="CORN :: Sales Purchase Format" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="content1" runat="server" ContentPlaceHolderID="cphPage">
                 <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
<script language="JavaScript" type="text/javascript">
    function pageLoad() {
        $("select").searchable();
    }
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
                            <table cellspacing="4">
                                <tbody>
                                    <tr>
                                        <td style="width: 40%" align="left" colspan="2">
                                            <strong>
                                                <asp:Label ID="Label30" runat="server" Width="60px" Text="Report For" CssClass="lblbox"
                                                    __designer:wfdid="w47"></asp:Label></strong>
                                        </td>
                                        <td style="width: 60%" align="left">
                                            <asp:DropDownList ID="DrpReportType" runat="server" Width="200px" CssClass="DropList"
                                                __designer:wfdid="w48">
                                                <asp:ListItem Value="0">Sales Format</asp:ListItem>
                                                <asp:ListItem Value="1">Purchase Format</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%" align="left">
                                            <strong>
                                                <asp:Label ID="Label2" runat="server" Width="48px" Text="Location" CssClass="lblbox"
                                                    __designer:wfdid="w49"></asp:Label></strong>
                                        </td>
                                        <td style="width: 5%">
                                        </td>
                                        <td style="width: 75%" align="left">
                                            <asp:DropDownList ID="DrpLocation" runat="server" Width="200px" CssClass="DropList"
                                                __designer:wfdid="w50">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%" align="left">
                                            <strong>
                                                <asp:Label ID="lbltoLocation" runat="server" Width="61px" Text="Principal" CssClass="lblbox"
                                                    __designer:wfdid="w51"></asp:Label></strong>
                                        </td>
                                        <td style="width: 5%">
                                        </td>
                                        <td style="width: 75%" align="left">
                                            <asp:DropDownList ID="drpPrincipal" runat="server" Width="200px" CssClass="DropList"
                                                __designer:wfdid="w52" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%; height: 36px" align="left">
                                            <strong>
                                                <asp:Label ID="Label3" runat="server" Width="60px" Text="GST Type" CssClass="lblbox"
                                                    __designer:wfdid="w53"></asp:Label></strong>
                                        </td>
                                        <td style="width: 5%; height: 36px">
                                        </td>
                                        <td style="width: 75%; height: 36px" align="left">
                                            <asp:RadioButtonList ID="rblCustomerType" runat="server" RepeatDirection="Horizontal"
                                                __designer:wfdid="w54">
                                                <asp:ListItem Selected="True" Value="-1">All</asp:ListItem>
                                                <asp:ListItem Value="1">Registered</asp:ListItem>
                                                <asp:ListItem Value="0">Unregistered</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%" align="left">
                                            <strong>
                                                <asp:Label ID="Label1" runat="server" Width="59px" Text="Date From" CssClass="lblbox"
                                                    __designer:wfdid="w55"></asp:Label></strong>
                                        </td>
                                        <td style="width: 5%">
                                        </td>
                                        <td style="width: 75%" align="left">
                                            <asp:TextBox ID="txtFromDate" runat="server" Width="100px" CssClass="txtBox" __designer:wfdid="w56"
                                                MaxLength="10"></asp:TextBox>
                                            <asp:ImageButton ID="ImgBntFromCalc" runat="server" __designer:wfdid="w57" ImageUrl="~/App_Themes/Granite/Images/date.gif">
                                            </asp:ImageButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%" align="left">
                                            <strong>
                                                <asp:Label ID="Label4" runat="server" Width="59px" Text="Date To" CssClass="lblbox"
                                                    __designer:wfdid="w58"></asp:Label></strong>
                                        </td>
                                        <td style="width: 5%">
                                        </td>
                                        <td style="width: 75%" align="left">
                                            <asp:TextBox ID="txtToDate" runat="server" Width="100px" CssClass="txtBox" __designer:wfdid="w59"
                                                MaxLength="10"></asp:TextBox>
                                            <asp:ImageButton ID="ImgBntToCalc" runat="server" __designer:wfdid="w60" ImageUrl="~/App_Themes/Granite/Images/date.gif">
                                            </asp:ImageButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%" colspan="3">
                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" __designer:wfdid="w61"
                                                EnableViewState="False" Format="dd-MMM-yyyy" PopupButtonID="ImgBntFromCalc" TargetControlID="txtFromDate">
                                            </cc1:CalendarExtender>
                                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" __designer:wfdid="w62"
                                                EnableViewState="False" Format="dd-MMM-yyyy" PopupButtonID="ImgBntToCalc" TargetControlID="txtToDate">
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnViewPDF" OnClick="btnViewPDF_Click" runat="server" Width="90"
                        CssClass="Button" Text="View PDF"></asp:Button>
                    <asp:Button ID="btnViewExcel" OnClick="btnViewExcel_Click" runat="server" Width="90"
                        CssClass="Button" Text="View Excel"></asp:Button>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
