<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptTransferIn.aspx.cs" Inherits="Forms_rptTransferIn"
    MasterPageFile="~/Forms/PageMaster.master" Title="CORN :: Transfer In/Out Report" %>

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
                            <table>
                                <tbody>
                                   
                                    <tr>
                                        <td style="width: 1px; height: 1px" align="left">
                                        </td>
                                        <td style="width: 29px; height: 1px" align="left">
                                        </td>
                                        <td style="width: 1px; height: 1px" align="left">
                                        </td>
                                        <td style="width: 203px; height: 1px" align="left">
                                            <asp:RadioButtonList ID="RbTransferType" runat="server" Width="199px" Height="20px"
                                                RepeatDirection="Horizontal">
                                                <asp:ListItem Selected="True" Value="Transfer In">Transfer In</asp:ListItem>
                                                <asp:ListItem>Transfer Out</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td style="width: 1px; height: 1px" align="left">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 1px; height: 1px">
                                        </td>
                                        <td align="left" style="width: 29px; height: 1px">
                                            <strong>
                                                <asp:Label ID="Label4" runat="server" CssClass="lblbox" Text="Report In" Width="70px"></asp:Label></strong>
                                        </td>
                                        <td align="left" style="width: 1px; height: 1px">
                                        </td>
                                        <td align="left" style="width: 203px; height: 1px">
                                            <asp:DropDownList ID="DrpReportType" runat="server" Width="200px" CssClass="DropList"
                                                AutoPostBack="True" OnSelectedIndexChanged="DrpReportType_SelectedIndexChanged">
                                                <asp:ListItem>Unit</asp:ListItem>
                                                <%--<asp:ListItem>Carton</asp:ListItem>--%>
                                                <asp:ListItem>Value</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td align="left" style="width: 1px; height: 1px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 1px" align="left">
                                        </td>
                                        <td style="width: 29px" align="left">
                                            <strong>
                                                <asp:Label ID="Label2" runat="server" Width="48px" Text="Location" CssClass="lblbox"></asp:Label></strong>
                                        </td>
                                        <td style="width: 1px" align="left">
                                        </td>
                                        <td style="width: 203px; height: 25px" align="left">
                                            <asp:DropDownList ID="DrpLocation" runat="server" Width="200px" CssClass="DropList"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 1px; height: 25px" align="left">
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td style="width: 1px; height: 25px" align="left">
                                        </td>
                                        <td style="width: 29px; height: 25px" align="left">
                                            <strong>
                                                <asp:Label ID="Label1" runat="server" Width="59px" Text="From Date" CssClass="lblbox"></asp:Label></strong>
                                        </td>
                                        <td style="width: 1px; height: 25px" align="left">
                                        </td>
                                        <td style="width: 203px; height: 25px" align="left">
                                            <asp:TextBox ID="txtFromDate" runat="server" Width="153px" CssClass="txtBox" MaxLength="10"></asp:TextBox>
                                            <asp:ImageButton ID="ImgBntFromCalc" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif">
                                            </asp:ImageButton>
                                        </td>
                                        <td style="width: 1px; height: 25px" align="left">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 1px; height: 25px" align="left">
                                        </td>
                                        <td style="width: 29px; height: 25px" align="left">
                                            <strong>
                                                <asp:Label ID="Label3" runat="server" Width="54px" Text="To  Date" CssClass="lblbox"></asp:Label></strong>
                                        </td>
                                        <td style="width: 1px; height: 25px" align="left">
                                        </td>
                                        <td style="width: 203px; height: 25px" align="left">
                                            <asp:TextBox ID="txtToDate" runat="server" Width="153px" CssClass="txtBox" MaxLength="10"></asp:TextBox>
                                            <asp:ImageButton ID="ImgToDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif">
                                            </asp:ImageButton>
                                        </td>
                                        <td style="width: 1px; height: 25px" align="left">
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                EnableViewState="False" PopupButtonID="ImgBntFromCalc" TargetControlID="txtFromDate">
                            </cc1:CalendarExtender>
                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                EnableViewState="False" PopupButtonID="ImgToDate" TargetControlID="txtToDate">
                            </cc1:CalendarExtender>
                            &nbsp;&nbsp;
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    &nbsp;&nbsp;<br />
                    <asp:Button ID="btnViewPDF" runat="server" Width="90" Text="View PDF" OnClick="btnViewPDF_Click"
                        CssClass="Button" />
                    <asp:Button ID="btnViewExcel" runat="server" Text="View Excel" Width="90" OnClick="btnViewExcel_Click"
                        CssClass="Button" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
