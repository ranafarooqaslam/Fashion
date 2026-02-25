<%@ Page Title="SAMS :: Vendor Opening" Language="C#" MasterPageFile="~/Forms/PageMaster.master"
    AutoEventWireup="true" CodeFile="fmPrincipalOpening.aspx.cs" Inherits="Forms_fmPrincipalOpening" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="Server">
    <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
<script type="text/javascript">
    function pageLoad() {


        $("select").searchable();
    }
    function onlyDotsAndNumbers(txt, event) {
        var charCode = (event.which) ? event.which : event.keyCode;

        if (charCode == 9 || charCode == 8) {
            return true;
        }
        if (charCode == 46) {
            if (txt.value.indexOf(".") < 0)
                return true;
            return false;
        }
        if (charCode == 31 || charCode < 48 || charCode > 57)
            return false;
        return true;
    }
</script>

    <div id="right_data">
        <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <table width="100%">
                            <tr>
                                    <td style="width: 100%">
                                        <strong>Location</strong>
                                    </td>
                                    <td style="width: 5%">
                                    </td>
                                    <td style="width: 45%">
                                        <asp:DropDownList ID="drpDistributor" runat="server" Width="210" AutoPostBack="true"
                                            OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 10%">
                                        
                                    </td>
                                </tr>
                                
                                 <tr>
                                    <td style="width: 100%">
                                        <strong>Vendor</strong>
                                    </td>
                                    <td style="width: 5%">
                                    </td>
                                    <td style="width: 45%">
                                        <asp:DropDownList ID="drpVendor" runat="server" Width="210" AutoPostBack="true"
                                            OnSelectedIndexChanged="drpVendor_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 10%">
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td >
                                        <strong>Type</strong>
                                    </td>
                                    <td >
                                    </td>
                                    <td >
                                        <asp:RadioButtonList ID="rblOpening" runat="server" Width="100%" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="0" Text="Credit" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Debit"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                     <td >
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td >
                                        <strong>Date</strong>
                                    </td>
                                    <td >
                                    </td>
                                    <td >
                                        <asp:TextBox ID="txtOpeningDate" runat="server" Width="90%"></asp:TextBox>
                                       
                                    </td>
                                   
                                    <td >
                                         <asp:ImageButton ID="ibOpeningDate" runat="server" Width="16px" ImageUrl="~/App_Themes/Granite/Images/date.gif">
                                        </asp:ImageButton>
                                        <cc1:CalendarExtender ID="ceOpeningDate" runat="server" TargetControlID="txtOpeningDate"
                                            PopupButtonID="ibOpeningDate" Format="dd-MMM-yyyy">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td >
                                        <strong>Opening Balance</strong>
                                    </td>
                                    <td >
                                    </td>
                                    <td >
                                        <asp:TextBox ID="txtOpeningBalance" runat="server" Width="100%"
                                        onkeypress="return onlyDotsAndNumbers(this,event);"></asp:TextBox>
                                    </td>
                                     <td>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td ">
                                        <strong>Remarks</strong>
                                    </td>
                                    <td >
                                    </td>
                                    <td >
                                        <asp:TextBox ID="txtOpeningBalanceRemarks" runat="server" Width="100%"></asp:TextBox>
                                    </td>
                                     <td >
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="center">
                                        <br />
                                        <asp:Button ID="btnSaveOpeningBalance" runat="server" OnClick="btnOpeningBalance_Click"
                                            CssClass="Button" Text="Save" Width="85px" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
