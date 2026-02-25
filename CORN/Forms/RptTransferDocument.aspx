<%@ Page Title="CORN :: Transfer Document" Language="C#" MasterPageFile="~/Forms/PageMaster.master"
    AutoEventWireup="true" CodeFile="RptTransferDocument.aspx.cs" Inherits="Forms_RptTransferDocument" %>

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
                                   
                                    <tr>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td  align="left">
                                            <asp:RadioButtonList ID="RdbTransferType" runat="server" style="margin-bottom: 20px; margin-left: -50px;" Width="500px" AutoPostBack="true"
                                                RepeatDirection="Horizontal" OnSelectedIndexChanged="RdbTransferType_SelectedIndexChanged">
                                                <asp:ListItem Selected="True" Text="Transfer In" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="Transfer Out" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="Transfer Out / Transfer In Summary" Value="100"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                     <tr runat="server" id="rptTypeRow">
                                        <td align="left">
                                        </td>
                                         <td align="left">
                                             <strong>Report Type</strong>
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td  align="left">
                                           
                                             <asp:DropDownList ID="drpReportType" runat="server" Width="200px" CssClass="DropList">
                                                  <asp:ListItem Selected="True" Text="With Value" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="WithOut Value" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                        </td>
                                        <td align="left" style="width:70px;">
                                            <strong runat="server" id="lbllocation">
                                                Location</strong>
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td style="height: 25px" align="left">
                                            <asp:DropDownList ID="drpDistributor" runat="server" Width="200px" CssClass="DropList">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr runat="server" id="tolocationRow" visible="false">
                                        <td align="left">
                                        </td>
                                        <td align="left" style="width:70px;">
                                            <strong>
                                                To Location</strong>
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td style="height: 25px" align="left">
                                            <asp:DropDownList ID="drpDistributorTo" runat="server" Width="200px" CssClass="DropList">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                            <strong>
                                                From Date</strong>
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td align="left" style="height: 25px">
                                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="txtBox" MaxLength="10"
                                                Width="176px"></asp:TextBox>
                                            <asp:ImageButton ID="ibtnStartDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                                Width="16px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                            <strong>
                                                To Date</strong>
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td align="left" style="height: 25px">
                                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="txtBox " MaxLength="10"
                                                Width="176px"></asp:TextBox>
                                            <asp:ImageButton ID="ibnEndDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                                Width="16px" />
                                        </td>
                                    </tr>
                                     <tr>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                           
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td align="left" style="height: 25px">
                                           <asp:CheckBox runat="server" Text="With Image" ID="chbWithImage" Checked="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
                                    <cc1:CalendarExtender ID="CEStartDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnStartDate"
                                        TargetControlID="txtStartDate">
                                    </cc1:CalendarExtender>
                                    <cc1:CalendarExtender ID="CEEndDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibnEndDate"
                                        TargetControlID="txtEndDate">
                                    </cc1:CalendarExtender>
                                </table>
                           
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
