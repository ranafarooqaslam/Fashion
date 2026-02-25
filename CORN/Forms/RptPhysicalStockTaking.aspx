<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="RptPhysicalStockTaking.aspx.cs" Inherits="Forms_RptPhysicalStockTaking"
    Title="CORN :: Physical Stock Report" %>

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

    // Function to handle checkbox click event
        function handleCheckboxClick(checkbox) {
             var checkBox1 = document.getElementById("<%= chkExcess.ClientID %>");
            var checkBox2 = document.getElementById("<%= chkShort.ClientID %>");

        if (checkbox.id === checkBox1.id && checkBox1.checked) {
            checkBox2.checked = false;
        } else if (checkbox.id === checkBox2.id && checkBox2.checked) {
            checkBox1.checked = false;
        }
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
                                        <td align="left" style="width: 1px; height: 40px">
                                        </td>
                                        <td align="left" style="height: 40px">
                                            <strong>
                                                <asp:Label ID="Label3" runat="server" CssClass="lblbox" Text="Report Type" Width="78px"></asp:Label></strong>
                                        </td>
                                        <td align="left" style="width: 1px; height: 40px">
                                        </td>
                                        <td align="left" style="width: 203px; height: 40px">
                                            <asp:RadioButtonList ID="RblType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="RblType_SelectedIndexChanged"
                                                RepeatDirection="Horizontal" Width="300px">
                                                <asp:ListItem Selected="True">Item Wise</asp:ListItem>
                                                <asp:ListItem>Value Wise</asp:ListItem>
                                                <asp:ListItem>Document Wise</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td align="left" style="width: 1px; height: 40px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 1px; height: 30px" align="left">
                                        </td>
                                        <td style="height: 30px" align="left">
                                            <strong>
                                                <asp:Label ID="Label2" runat="server" Width="48px" Text="Location" CssClass="lblbox"></asp:Label></strong>
                                        </td>
                                        <td style="width: 1px; height: 30px" align="left">
                                        </td>
                                        <td style="width: 203px; height: 30px" align="left">
                                            <asp:DropDownList ID="DrpLocation" runat="server" Width="210px" CssClass="DropList"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 1px; height: 30px" align="left">
                                        </td>
                                    </tr>
                                   <%-- <tr>
                                        <td style="width: 1px; height: 33px;" align="left">
                                        </td>
                                        <td style="height: 33px;" align="left">
                                            <strong>
                                                <asp:Label ID="lbltoLocation" runat="server" Width="61px" Text="Principal" CssClass="lblbox"></asp:Label></strong>
                                        </td>
                                        <td style="width: 1px; height: 33px;" align="left">
                                        </td>
                                        <td style="width: 203px; height: 33px" align="left">
                                            <asp:DropDownList ID="drpPrincipal" runat="server" Width="210px" CssClass="DropList"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 1px; height: 33px" align="left">
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td style="width: 1px; height: 25px" align="left">
                                        </td>
                                        <td style="width: 29px; height: 25px" align="left">
                                            <strong>
                                                <asp:Label ID="Label1" runat="server" Width="59px" Text="Date" CssClass="lblbox"></asp:Label></strong>
                                        </td>
                                        <td style="width: 1px; height: 25px" align="left">
                                        </td>
                                        <td style="width: 203px; height: 25px" align="left">
                                            <asp:TextBox ID="txtFromDate" runat="server" Width="176px" CssClass="txtBox" MaxLength="10"></asp:TextBox>
                                            <asp:ImageButton ID="ImgBntFromCalc" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif">
                                            </asp:ImageButton>
                                        </td>
                                        <td style="width: 1px; height: 25px" align="left">
                                        </td>
                                    </tr>
                                    <tr runat="server" id="toDateRow" visible="false">
                                        <td style="width: 1px; height: 25px" align="left">
                                        </td>
                                        <td style="width: 29px; height: 25px" align="left">
                                            <strong>
                                                <asp:Label ID="Label4" runat="server" Width="59px" Text="To Date" CssClass="lblbox"></asp:Label></strong>
                                        </td>
                                        <td style="width: 1px; height: 25px" align="left">
                                        </td>
                                        <td style="width: 203px; height: 25px" align="left">
                                            <asp:TextBox ID="txtToDate" runat="server" Width="176px" CssClass="txtBox" MaxLength="10"></asp:TextBox>
                                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif">
                                            </asp:ImageButton>
                                        </td>
                                        <td style="width: 1px; height: 25px" align="left">
                                        </td>
                                    </tr>
                                    <tr runat="server" id="sortRow">
                                        <td style="width: 1px; height: 25px" align="left">
                                        </td>
                                        <td style="width: 29px; height: 25px" align="left">
                                            <strong>
                                                <asp:Label ID="Label5" runat="server" Width="59px" Text="Sort By" CssClass="lblbox"></asp:Label></strong>
                                        </td>
                                        <td style="width: 1px; height: 25px" align="left">
                                        </td>
                                        <td align="left" style="height: 25px;width:70px">
                                            <asp:CheckBox runat="server" ID="chkExcess" Checked="false" onclick="handleCheckboxClick(this)" Text="Excess" />
                                             &nbsp;&nbsp;<asp:CheckBox runat="server" ID="chkShort" Checked="false" onclick="handleCheckboxClick(this)" Text="Short" />
                                        </td>
                                        <td style="width: 1px; height: 25px" align="left">
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" EnableViewState="False"
                                Format="dd-MMM-yyyy" PopupButtonID="ImgBntFromCalc" TargetControlID="txtFromDate">
                            </cc1:CalendarExtender>
                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" EnableViewState="False"
                                Format="dd-MMM-yyyy" PopupButtonID="ImageButton1" TargetControlID="txtToDate">
                            </cc1:CalendarExtender>
                            &nbsp;
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />&nbsp;&nbsp;
                    <asp:Button ID="btnViewPDF" runat="server" CssClass="Button" Text="View PDF" Width="90"
                        OnClick="btnViewPDF_Click" />
                    <asp:Button ID="btnViewExcel" runat="server" CssClass="Button" Text="View Excel"
                        Width="90" OnClick="btnViewExcel_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
