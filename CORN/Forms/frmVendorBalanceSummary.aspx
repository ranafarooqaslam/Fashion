<%@ Page Title="SAMS :: Vendor Balance Summary" Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmVendorBalanceSummary.aspx.cs" Inherits="Forms_frmVendorBalanceSummary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function pageLoad() {
            $("select").searchable();
        }
    </script>
    <div id="right_data">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tbody>
                      
                        <tr>
                            
                            <td align="left">
                                <strong>
                                    <asp:Label ID="lbltoLocation" runat="server" Width="94px" Text="Location" CssClass="lblbox"></asp:Label></strong>
                            </td>
                            <td style="height: 25px" align="left">
                                <asp:DropDownList ID="drpDistributor" runat="server" Width="240px" CssClass="DropList">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <strong>
                                    <asp:Label ID="lblCustomer" runat="server" Width="79px" Text="Vendor" CssClass="lblbox"></asp:Label></strong>
                            </td>
                            <td style="height: 25px" align="left">
                                <asp:DropDownList ID="DrpCustomer" runat="server" Width="240px" CssClass="DropList">
                                </asp:DropDownList>
                            </td>
                        </tr>
                         <tr>
                                    
                                    <td align="left">
                                        <strong>
                                            <asp:Label ID="Label3" runat="server" Height="13px" Text="From Date" Width="70px"></asp:Label></strong>
                                    </td>
                                    
                                    <td align="left" style="height: 25px">
                                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="txtBox" MaxLength="10" onkeyup="BlockStartDateKeyPress()"
                                            Width="150px"></asp:TextBox>
                                        <asp:ImageButton ID="ibtnStartDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                            Width="16px" />
                                    </td>
                                </tr>
                                <tr>
                                    
                                    <td align="left">
                                        <strong>
                                            <asp:Label ID="Label4" runat="server" Height="13px" Text="To Date" Width="80px"></asp:Label></strong>
                                    </td>
                                   
                                    <td align="left" style="height: 25px">
                                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="txtBox " MaxLength="10" onkeyup="BlockEndDateKeyPress()"
                                            Width="150px"></asp:TextBox>
                                        <asp:ImageButton ID="ibnEndDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                            Width="16px" />
                                    </td>
                                </tr>
                                <tr>
                                   
                                   
                                    <td align="left">
                                    </td>
                                    <td align="left" style="height: 25px">
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
    &nbsp;&nbsp;
    <asp:Button ID="btnPDF" runat="server" CssClass="Button" Text="View PDF" Width="90"
        OnClick="btnPDF_Click" />
    <asp:Button ID="btnExcel" runat="server" CssClass="Button" Text="View Excel" Width="90"
        OnClick="btnExcel_Click" />
    </div>
</asp:Content>
