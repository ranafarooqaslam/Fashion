<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="RptOrderbookerWiseAging.aspx.cs" Inherits="Forms_RptOrderbookerWiseAging"
    Title="CORN :: OrderBooker Credit Aging" %>

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

    </script>
    <div id="right_data">
        <table width="100%">
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table>
                                <tbody>
                                    <tr>
                                        <td align="left" colspan="4">
                                            <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                        </td>
                                        <td style="width: 82px" valign="middle" align="left">
                                            <strong>
                                                <asp:Label ID="Label1" runat="server" Width="80px" Text="Report Type" CssClass="lblbox"></asp:Label></strong>
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td style="height: 25px" align="left">
                                            <asp:RadioButtonList ID="RadioButtonList1" runat="server">
                                                <asp:ListItem Selected="True">Invoice Date Wise</asp:ListItem>
                                                <asp:ListItem>Due Date Wise</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                        </td>
                                        <td style="width: 82px" align="left">
                                            <strong>
                                                <asp:Label ID="lbltoLocation" runat="server" Width="80px" Text="Location" CssClass="lblbox"></asp:Label></strong>
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td style="height: 25px" align="left">
                                            <asp:DropDownList ID="drpDistributor" runat="server" Width="200px" CssClass="DropList"
                                                AutoPostBack="True" OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                        </td>
                                        <td style="width: 82px" align="left">
                                            <strong>
                                                <asp:Label ID="Label6" runat="server" Width="78px" Text="Principal" CssClass="lblbox"></asp:Label></strong>
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td style="height: 25px" align="left">
                                            <asp:DropDownList ID="DrpPrincipal" runat="server" Width="200px" CssClass="DropList">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                        </td>
                                        <td style="width: 82px" align="left">
                                            <strong>
                                                <asp:Label ID="Label4" runat="server" Width="79px" Text="Sale Force"></asp:Label></strong>
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td style="height: 25px" align="left">
                                            <asp:DropDownList ID="ddlSaleForce" runat="server" Width="200px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                        </td>
                                        <td style="width: 82px" align="left">
                                            <strong>
                                                <asp:Label ID="Label5" runat="server" Width="79px" Text="Order Booker" CssClass="lblbox"></asp:Label></strong>
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td style="height: 25px" align="left">
                                            <asp:DropDownList ID="DrpOrderBooker" runat="server" Width="200px" CssClass="DropList">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                        </td>
                                        <td style="width: 82px" align="left">
                                            <strong>
                                                <asp:Label ID="lblNickName" runat="server" Width="79px" Text="Channel Type" CssClass="lblbox"
                                                    __designer:wfdid="w2"></asp:Label></strong>
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td style="height: 25px" align="left">
                                            <asp:DropDownList ID="drpChannelType" runat="server" Width="200px" CssClass="DropList"
                                                __designer:wfdid="w1">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                        </td>
                                        <td style="width: 82px" align="left">
                                            &nbsp; <strong>
                                                <asp:Label ID="Label3" runat="server" Width="70px" Height="13px" Text="Up to"></asp:Label></strong>
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td style="height: 25px" align="left">
                                            <asp:TextBox ID="txtDocmentDate" onkeyup="BlockStartDateKeyPress()" runat="server"
                                                Width="150px" CssClass="txtBox" MaxLength="10"></asp:TextBox><asp:ImageButton ID="ibtnStartDate"
                                                    runat="server" Width="16px" ImageUrl="~/App_Themes/Granite/Images/date.gif">
                                            </asp:ImageButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                        </td>
                                        <td style="width: 82px" align="left">
                                            <strong>
                                                <asp:Label ID="Label2" runat="server" Width="65px" Height="13px" Text="Days"></asp:Label></strong>
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td style="height: 25px" align="left">
                                            <asp:TextBox ID="txtDays" runat="server" Width="150px" CssClass="txtBox"></asp:TextBox>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <cc1:CalendarExtender ID="CEStartDate" runat="server" TargetControlID="txtDocmentDate"
                                PopupButtonID="ibtnStartDate" Format="dd-MMM-yyyy">
                            </cc1:CalendarExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
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
