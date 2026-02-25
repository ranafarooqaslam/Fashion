<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="RptDistributorReports.aspx.cs" Inherits="Forms_RptDistributorReports"
    Title="CORN :: Sales & Closing Stock" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
             <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
<script language="JavaScript" type="text/javascript">
    function pageLoad() {
        $("select").searchable();
    }
      
        function checkDocument() {
            var Id = document.getElementById("<%=DrpReportType.ClientID %>");
            var selectedValue = Id.value;

            if (selectedValue == "4") {
                document.getElementById('<%= lblEndDate.ClientID %>').style.visibility = "hidden";
                document.getElementById('<%= txtEndDate.ClientID %>').style.visibility = "hidden";
                document.getElementById('<%= ibnEndDate.ClientID %>').style.visibility = "hidden";
		document.getElementById('<%= Label3.ClientID %>').innerHTML = "Date";
            } else {

                if (document.getElementById('<%= lblEndDate.ClientID %>').style.visibility == "hidden") {
                    document.getElementById('<%= lblEndDate.ClientID %>').style.visibility = "visible";
                    document.getElementById('<%= txtEndDate.ClientID %>').style.visibility = "visible";
                    document.getElementById('<%= ibnEndDate.ClientID %>').style.visibility = "visible";
document.getElementById('<%= Label3.ClientID %>').innerHTML = "From Date";
                }
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
                                                <asp:Label ID="Label1" runat="server" CssClass="lblbox" Text="Report Type" Width="78px"></asp:Label></strong>
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td style="height: 25px" align="left">
                                            <asp:DropDownList ID="DrpReportType" runat="server" Width="200px" onchange="checkDocument()"
                                                 CssClass="DropList">
                                                <asp:ListItem Value="0">Sales</asp:ListItem>
                                                <asp:ListItem Value="1">Sales Return</asp:ListItem>
                                                <asp:ListItem Value="2">Shop Damage</asp:ListItem>
                                                <asp:ListItem Value="3">Import Damage</asp:ListItem>
                                                <%-- <asp:ListItem>Opening Stock</asp:ListItem>--%>
                                                <asp:ListItem Value="4">Closing Stock</asp:ListItem>
                                                <asp:ListItem Value="5">Purchase</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="display:none;">
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                            <strong>
                                               Value Type</strong>
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td align="left" style="height: 25px">
                                            <asp:DropDownList ID="DrpUnitType" runat="server" Visible="false" Width="200px" CssClass="DropList">
                                                <asp:ListItem>Pieces</asp:ListItem>
                                                <asp:ListItem>Value</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                            <strong>
                                                <asp:Label ID="Label7" runat="server" CssClass="lblbox" Text="Location Type" Width="101px"></asp:Label></strong>
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td align="left" style="height: 25px">
                                            <asp:DropDownList ID="ddDistributorType" runat="server" Width="200px" CssClass="DropList"
                                                OnSelectedIndexChanged="ddDistributorType_SelectedIndexChanged" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                            <strong>
                                                <asp:Label ID="lbltoLocation" runat="server" CssClass="lblbox" Text="Location" Width="66px"></asp:Label></strong>
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td align="left" style="height: 25px">
                                            <asp:DropDownList ID="drpDistributor" runat="server" Width="200px" CssClass="DropList">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                            <strong>
                                                <asp:Label ID="Label3" runat="server" Height="13px" Text="From Date" Width="70px"></asp:Label></strong>
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td align="left" style="height: 25px">
                                            &nbsp;<asp:TextBox ID="txtStartDate" runat="server" CssClass="txtBox" MaxLength="10"
                                                onkeyup="BlockStartDateKeyPress()" Width="150px"></asp:TextBox>
                                            <asp:ImageButton ID="ibtnStartDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                                Width="16px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                            <strong>
                                                <asp:Label ID="lblEndDate" runat="server" Height="13px" Text="To Date" Width="80px"></asp:Label></strong>
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td align="left" style="height: 25px">
                                            &nbsp;<asp:TextBox ID="txtEndDate" runat="server" CssClass="txtBox " MaxLength="10"
                                                onkeyup="BlockEndDateKeyPress()" Width="150px"></asp:TextBox>
                                            <asp:ImageButton ID="ibnEndDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                                Width="16px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                           &nbsp;
                                        </td>
                                    </tr>
                                    <%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
                                    <cc1:CalendarExtender ID="CEStartDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnStartDate"
                                        TargetControlID="txtStartDate">
                                    </cc1:CalendarExtender>
                                    <cc1:CalendarExtender ID="CEEndDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibnEndDate"
                                        TargetControlID="txtEndDate">
                                    </cc1:CalendarExtender>
                                </table>
                            </asp:Panel>
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
