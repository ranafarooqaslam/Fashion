<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="RptNCSVsDepositDayWise.aspx.cs" Inherits="Forms_RptNCSVsDepositDayWise"
    Title="CORN :: Daily NCS vs Deposit" %>

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
                                        <td align="left">
                                            <strong>
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
                                </tbody>
                            </table>
                            <cc1:CalendarExtender ID="CEStartDate" runat="server" TargetControlID="txtDocmentDate"
                                PopupButtonID="ibtnStartDate" Format="dd-MMM-yyyy">
                            </cc1:CalendarExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>&nbsp;&nbsp;
                    <asp:Button ID="btnViewPDF" runat="server" CssClass="Button" Text="View PDF" Width="80"
                        OnClick="btnViewPDF_Click" />
                    <asp:Button ID="btnViewExcel" runat="server" CssClass="Button" Text="View Excel"
                        Width="80" OnClick="btnViewExcel_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
