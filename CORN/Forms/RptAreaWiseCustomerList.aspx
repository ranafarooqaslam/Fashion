<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="RptAreaWiseCustomerList.aspx.cs" Inherits="Forms_RptAreaWiseCustomerList"
    Title="CORN :: Customer List" %>

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
        <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td align="left" colspan="3">
                                        <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                             <tr>
    <td></td>

    <!-- Location -->
    <td>
        <strong>
            <asp:Label ID="lbltoLocation" runat="server" Text="Location"></asp:Label>
        </strong>
    </td>
    <td>
        <asp:DropDownList ID="drpDistributor" runat="server" Width="180px"
            AutoPostBack="True" OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged">
        </asp:DropDownList>
    </td>
<td>
        <asp:CheckBox ID="chkShowCalendar" AutoPostBack="true"
             OnCheckedChanged="chkShowCalendar_CheckedChanged" runat="server" Text=" Date Wise " />
    </td>
    <!-- From Date -->
    <td style="padding-left: 20px;">
        <strong>
            <asp:Label ID="lblFromDate" runat="server" Text="From Date"></asp:Label>
        </strong>
    </td>
    <td>
        <asp:TextBox ID="txtStartDate" runat="server" Width="150px"></asp:TextBox>
        <asp:ImageButton ID="ibtnStartDate" runat="server" Width="16px"
            ImageUrl="~/App_Themes/Granite/Images/date.gif" />
    </td>
    
</tr>

<tr>
    <td></td>

    <!-- Customer -->
    <td>
        <strong>
            <asp:Label ID="Label3" runat="server" Text="Customer"></asp:Label>
        </strong>
    </td>
    <td>
        <asp:DropDownList ID="ddl_customer" runat="server" Width="180px">
            <asp:ListItem Text="Active" Value="1"></asp:ListItem>
            <asp:ListItem Text="InActive" Value="0"></asp:ListItem>
        </asp:DropDownList>
    </td>

    <td></td>

    <!-- To Date -->
    <td style="padding-left: 20px;">
        <strong>
            <asp:Label ID="lblToDate" runat="server" Text="To Date"></asp:Label>
        </strong>
    </td>
    <td>
        <asp:TextBox ID="txtEndDate" runat="server" Width="150px"></asp:TextBox>
        <asp:ImageButton ID="ibnEndDate" runat="server" Width="16px"
            ImageUrl="~/App_Themes/Granite/Images/date.gif" />
    </td>

</tr>
                                   
                  <cc1:CalendarExtender ID="CEStartDate" runat="server" TargetControlID="txtStartDate"
                      PopupButtonID="ibtnStartDate" Format="dd-MMM-yyyy">
                  </cc1:CalendarExtender>
                  <cc1:CalendarExtender ID="CEEndDate" runat="server" TargetControlID="txtEndDate"
                      PopupButtonID="ibnEndDate" Format="dd-MMM-yyyy">
                  </cc1:CalendarExtender>
                                      
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    &nbsp; &nbsp;
                    <asp:Button ID="btnViewPDF" runat="server" Text="View PDF" OnClick="btnViewPDF_Click"
                        CssClass="Button" Width="80" />
                    <asp:Button ID="btnViewExcel" runat="server" Text="View Excel" OnClick="btnViewExcel_Click"
                        CssClass="Button" Width="80" />
                </td>
            </tr>
        </table>
        &nbsp;
    </div>
    <div>
        <strong>
            <asp:Label ID="Label2" runat="server" CssClass="lblbox" Text="Town" Width="48px"
                Visible="false"></asp:Label></strong>
        <asp:DropDownList ID="DrpTown" runat="server"  CssClass="DropList" AutoPostBack="True"
            Visible="false">
        </asp:DropDownList>
        <asp:DropDownList ID="DrpPrincipal" runat="server"  CssClass="DropList"
            AutoPostBack="True" Visible="false">
        </asp:DropDownList>
        <strong>
            <asp:Label ID="Label1" runat="server" CssClass="lblbox" Text="Route" Width="52px"
                Visible="false"></asp:Label></strong>
        <asp:DropDownList ID="DrpRoute" runat="server" Width="200px" CssClass="DropList"
            AutoPostBack="True" Visible="false">
        </asp:DropDownList>
        <strong>
            <asp:Label ID="Label6" runat="server" CssClass="lblbox" Text="Channel Type" Width="100px"
                Visible="false"></asp:Label></strong>
        <asp:DropDownList ID="DrpChannelType" runat="server" AutoPostBack="True" CssClass="DropList"
            Visible="false" Width="200px">
        </asp:DropDownList>
    </div>
</asp:Content>
