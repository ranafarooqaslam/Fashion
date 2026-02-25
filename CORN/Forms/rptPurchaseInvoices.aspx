<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="rptPurchaseInvoices.aspx.cs" Inherits="Forms_rptPurchaseInvoices" Title="CORN :: Sale Tax Return on Purchase" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">
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
                                        <td align="left" colspan="3">
                                            <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 95px" align="left">
                                            <strong>
                                                <asp:Label ID="lbltoLocation" runat="server" Width="70px" Text="Location" CssClass="lblbox"></asp:Label></strong>
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td style="height: 25px" align="left">
                                            <asp:DropDownList ID="drpDistributor" runat="server" Width="200px" CssClass="DropList">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 95px" align="left">
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
                                        <td style="width: 95px" align="left">
                                            <strong>
                                                <asp:Label ID="Label3" runat="server" Width="76px" Height="13px" Text="From Date"></asp:Label></strong>
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td style="height: 25px" align="left">
                                            &nbsp;<asp:TextBox ID="txtStartDate" onkeyup="BlockStartDateKeyPress()" runat="server"
                                                Width="150px" CssClass="txtBox" MaxLength="10"></asp:TextBox>
                                            <asp:ImageButton ID="ibtnStartDate" runat="server" Width="16px" ImageUrl="~/App_Themes/Granite/Images/date.gif">
                                            </asp:ImageButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 95px" align="left">
                                            <strong>
                                                <asp:Label ID="Label4" runat="server" Width="80px" Height="13px" Text="To Date"></asp:Label></strong>
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td style="height: 25px" align="left">
                                            &nbsp;<asp:TextBox ID="txtEndDate" onkeyup="BlockEndDateKeyPress()" runat="server"
                                                Width="150px" CssClass="txtBox " MaxLength="10"></asp:TextBox>
                                            <asp:ImageButton ID="ibnEndDate" runat="server" Width="16px" ImageUrl="~/App_Themes/Granite/Images/date.gif">
                                            </asp:ImageButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 95px" align="left">
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td style="height: 25px" align="left">
                                            <%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
                                            <cc1:CalendarExtender ID="CEStartDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnStartDate"
                                                TargetControlID="txtStartDate">
                                            </cc1:CalendarExtender>
                                            <cc1:CalendarExtender ID="CEEndDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibnEndDate"
                                                TargetControlID="txtEndDate">
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
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
