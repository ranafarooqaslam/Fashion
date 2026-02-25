<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="RptDailyPurchaseTransfer.aspx.cs" Inherits="Forms_RptDailyPurchaseTransfer"
    Title="CORN :: Date Wise Stock Report" %>

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


                                <asp:RadioButtonList ID="rblRate" runat="server" Width="200px" RepeatDirection="Horizontal" Visible="false">
                                    <asp:ListItem Value="0" Selected="True">Trade Price</asp:ListItem>
                                    <asp:ListItem Value="1">Purchase Price</asp:ListItem>
                                </asp:RadioButtonList>

                                <tr>
                                    <td align="left">
                                        <strong>
                                            <asp:Label ID="Label2" runat="server" CssClass="lblbox" Height="14px" Text="Transaction Type"
                                                Width="98px"></asp:Label></strong>
                                    </td>
                                    <td align="left"></td>
                                    <td align="left" style="height: 25px">
                                        <asp:DropDownList ID="DrpDocumentType" runat="server" CssClass="DropList" Width="200px">
                                            <asp:ListItem Value="2">Purchase</asp:ListItem>
                                            <asp:ListItem Value="5">Transfer Out</asp:ListItem>
                                            <asp:ListItem Value="3">Purchase Return</asp:ListItem>
                                            <asp:ListItem Value="4">Transfer In</asp:ListItem>
                                            <asp:ListItem Value="6">Damage</asp:ListItem>
                                            <%-- <asp:ListItem Value="7">Opening Stock</asp:ListItem>--%>
                                            <asp:ListItem Value="8">Short</asp:ListItem>
                                            <asp:ListItem Value="9">Excess</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <strong>
                                            <asp:Label ID="lbltoLocation" runat="server" CssClass="lblbox" Text="Location" Width="94px"></asp:Label></strong>
                                    </td>
                                    <td align="left"></td>
                                    <td style="height: 25px" align="left">
                                        <asp:DropDownList ID="drpDistributor" runat="server" Width="200px" CssClass="DropList">
                                        </asp:DropDownList>
                                    </td>
                                </tr>

                                <tr>
                                    <td align="left">
                                        <strong>
                                            <asp:Label ID="Label3" runat="server" Height="13px" Text="From Date" Width="76px"></asp:Label></strong>
                                    </td>
                                    <td align="left"></td>
                                    <td align="left" style="height: 25px">&nbsp;<asp:TextBox ID="txtStartDate" runat="server" CssClass="txtBox" MaxLength="10"
                                        onkeyup="BlockStartDateKeyPress()" Width="150px"></asp:TextBox>
                                        <asp:ImageButton ID="ibtnStartDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                            Width="16px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <strong>
                                            <asp:Label ID="Label4" runat="server" Height="13px" Text="To Date" Width="80px"></asp:Label></strong>
                                    </td>
                                    <td align="left"></td>
                                    <td align="left" style="height: 25px">&nbsp;<asp:TextBox ID="txtEndDate" runat="server" CssClass="txtBox " MaxLength="10"
                                        onkeyup="BlockEndDateKeyPress()" Width="150px"></asp:TextBox>
                                        <asp:ImageButton ID="ibnEndDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                            Width="16px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left"></td>
                                    <td align="left"></td>
                                    <td align="left" style="height: 25px">
                                        <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
                                        <cc1:CalendarExtender ID="CEStartDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnStartDate"
                                            TargetControlID="txtStartDate">
                                        </cc1:CalendarExtender>
                                        <cc1:CalendarExtender ID="CEEndDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibnEndDate"
                                            TargetControlID="txtEndDate">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    &nbsp; &nbsp;
                    <asp:Button ID="btnViewPDF" runat="server" CssClass="Button" Width="90" Text="View PDF"
                        OnClick="btnViewPDF_Click" />
                    <asp:Button ID="btnViewExcel" runat="server" CssClass="Button" Width="90" Text="View Excel"
                        OnClick="btnViewExcel_Click" />
                </td>
            </tr>
        </table>
        &nbsp;
    </div>
</asp:Content>
