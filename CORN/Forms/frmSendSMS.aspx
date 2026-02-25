<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmSendSMS.aspx.cs" Inherits="Forms_frmSendSMS" Title="SAMS :: Send SMS" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="content1" runat="server" ContentPlaceHolderID="cphPage">
    <script type="text/javascript" src="../AjaxLibrary/jquery.maxlength.js"></script>
    <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
    <script language="JavaScript" type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            var oControl = args.get_postBackElement();
            oControl.value = "Wait...";
            oControl.disabled = true;
        }
        $(document).ready(function ($) {
            $().maxlength();
        })


        function pageLoad() {
            $("select").searchable();
            var spanDisplay = document.getElementById('spanDisplay');
            spanDisplay.innerHTML = 0;
        }
        function SelectAll() {
            var chkBoxList = document.getElementById('<%= LstCustomer.ClientID %>');
            var chkBox = document.getElementById('<%= ChbAllCustomer.ClientID %>');
            if (chkBox.checked == true) {
                var chkBoxCount = chkBoxList.getElementsByTagName("input");

                for (var i = 0; i < chkBoxCount.length; i++) {
                    chkBoxCount[i].checked = true;
                }
            }
            else {
                var chkBoxCount = chkBoxList.getElementsByTagName("input");

                for (var i = 0; i < chkBoxCount.length; i++) {
                    chkBoxCount[i].checked = false;
                }
            }
        }

        function UnCheckSelectAll() {
            var chkBox = document.getElementById('<%= ChbAllCustomer.ClientID %>');
            var chkBoxList = document.getElementById('<%= LstCustomer.ClientID %>');
            var chkBoxCount = chkBoxList.getElementsByTagName("input");
            var count = 0;
            for (var i = 0; i < chkBoxCount.length; i++) {
                if (chkBoxCount[i].checked == false) {
                    count += 1;
                }
            }
            if (count > 0) {
                chkBox.checked = false;
            }
            else {
                chkBox.checked = true;
            }
        }


        function onlyNumbers(txt, event) {
            var charCode = (event.which) ? event.which : event.keyCode;

            if (charCode == 9 || charCode == 8) {
                return true;
            }
            if (charCode == 46) {
                return false;
            }
            if (charCode == 31 || charCode < 48 || charCode > 57)
                return false;

            return true;
        }
        function onlyDotsAndNumbers(txt, event) {
            var charCode = (event.which) ? event.which : event.keyCode;

            if (charCode == 9 || charCode == 8) {
                return true;
            }
            if (charCode == 46) {
                if (txt.value.indexOf(",") < 5)
                    return true;
                return false;
            }
            if (charCode == 31 || charCode < 48 || charCode > 57)
                return false;
            return true;
        }
        function count(clientId) {
            var txtInput = document.getElementById(clientId);
            var spanDisplay = document.getElementById('spanDisplay');
            if (txtInput.value.length <= 765) {

                if (txtInput.value.length == "") {
                    spanDisplay.innerHTML = 0;
                    spanDisplay.innerHTML = txtInput.value.length;
                    document.getElementById('<%= SMSCount.ClientID %>').innerHTML = 0;
                }
                else if (txtInput.value.length <= 160) {
                    spanDisplay.innerHTML = txtInput.value.length;
                    document.getElementById('<%= SMSCount.ClientID %>').innerHTML = 1;
            }
            else if (txtInput.value.length <= 320) {
                spanDisplay.innerHTML = txtInput.value.length;
                document.getElementById('<%= SMSCount.ClientID %>').innerHTML = 2;
            }
            else if (txtInput.value.length <= 480) {
                spanDisplay.innerHTML = txtInput.value.length;
                document.getElementById('<%= SMSCount.ClientID %>').innerHTML = 3;
            }
            else if (txtInput.value.length <= 640) {
                spanDisplay.innerHTML = txtInput.value.length;
                document.getElementById('<%= SMSCount.ClientID %>').innerHTML = 4;
            }
            else if (txtInput.value.length <= 765) {
                spanDisplay.innerHTML = txtInput.value.length;
                document.getElementById('<%= SMSCount.ClientID %>').innerHTML = 5;
            }
            else {
                document.getElementById('<%= SMSCount.ClientID %>').innerHTML = 5;
                var SubVal = txtInput.value.substring(0, 765);
                spanDisplay.innerHTML = SubVal.value.length;
                document.getElementById('<%= txtSMS.ClientID %>').value == SubVal;
            }
}

}

    </script>
    <div id="right_data">
        <div style="z-index: 101; left: 60%; width: 100px; position: absolute; top: 1%; height: 80px">
            &nbsp;<asp:Panel ID="Panel21" runat="server">
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <asp:ImageButton ID="ImageButton1" runat="server" Height="28px" ImageUrl="~/App_Themes/Granite/Images/image003.gif"
                            Width="31px" />
                        Wait....
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </asp:Panel>
        </div>
        <table width="100%">
            <tr>
                <td align="left">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <table>
                                <tbody>
                                    <tr>
                                        <td>
                                            <table>
                                                <tbody>
                                                    <tr>
                                                        <td style="height: 25px" align="left">
                                                            <strong>
                                                                <asp:Label ID="lblfromLocation" runat="server" Width="94px" Text="Location" CssClass="lblbox"></asp:Label></strong>
                                                        </td>
                                                        <td style="height: 25px">
                                                            <asp:DropDownList ID="drpDistributor" runat="server" Width="200px" CssClass="DropList"
                                                                AutoPostBack="True" OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged1">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td></td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td style="width:15%;">
                                                                        <strong>
                                                                            From Date
                                                                        </strong>
                                                                    </td>
                                                                    <td style="width:15%;">
                                                                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="txtBox" MaxLength="10" onkeyup="BlockStartDateKeyPress()" Width="90%"></asp:TextBox>
                                                                    </td>
                                                                    <td style="width:5%;">
                                                                        <asp:ImageButton ID="ibtnStartDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif" Width="16px" />
                                                                        <cc1:CalendarExtender ID="CEStartDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnStartDate"
                                                                            TargetControlID="txtStartDate">
                                                                        </cc1:CalendarExtender>
                                                                    </td>
                                                                    <td style="width:10%;">
                                                                        <strong>
                                                                            To Date
                                                                        </strong>
                                                                    </td>
                                                                    <td style="width:15%;">
                                                                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="txtBox" MaxLength="10" onkeyup="BlockStartDateKeyPress()" Width="90%"></asp:TextBox>                                                                        
                                                                    </td>
                                                                    <td style="width:5%;">
                                                                        <asp:ImageButton ID="ibnEndDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif" Width="16px" />
                                                                        <cc1:CalendarExtender ID="CEEndDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibnEndDate"
                                                                            TargetControlID="txtEndDate">
                                                                        </cc1:CalendarExtender>
                                                                    </td>
                                                                    <td style="width:15%;">
                                                                        <asp:Button ID="btnFiler" OnClick="btnFiler_Click" runat="server" Text="Filter" CssClass="Button" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100px" align="left" colspan="2">
                                                            <asp:CheckBox ID="ChbAllCustomer" onclick="SelectAll()" runat="server" Font-Bold="true" Text="All Customer"></asp:CheckBox>
                                                        </td>
                                                        <td><strong>
                                                            <asp:Label ID="Label5" runat="server" Width="94px" Text="Total Customer:" CssClass="lblbox"></asp:Label></strong>
                                                        </td>
                                                        <td><strong>
                                                            <asp:Label ID="lblCustomerCount" runat="server" Width="94px" Text="0" CssClass="lblbox"></asp:Label></strong>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td rowspan="4" colspan="4">
                                                            <asp:Panel ID="Panel2" runat="server" Width="450px" Height="550px" ScrollBars="Vertical"
                                                                BorderWidth="1px" BorderStyle="Groove">
                                                                <asp:CheckBoxList ID="LstCustomer" onclick="UnCheckSelectAll()" runat="server">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                        <td valign="top" align="left">
                                            <table>
                                                <tbody>
                                                    <tr>
                                                        <td></td>
                                                        <td><strong>
                                                            <asp:Label ID="lblSMSBalance" runat="server" Text="" CssClass="lblbox"></asp:Label>
                                                        </strong>
                                                            <strong>
                                                                <asp:Label ID="Label3" runat="server" Width="5px" Text=" | " CssClass="lblbox"></asp:Label>
                                                            </strong>
                                                            <strong>
                                                                <asp:Label ID="lblExpiry" runat="server" Text="" CssClass="lblbox"></asp:Label>
                                                            </strong></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <strong>
                                                                <asp:Label ID="Label1" runat="server" Width="100px" Style="margin-left: 50px;" Text="Phone #" CssClass="lblbox"></asp:Label>
                                                            </strong>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPhoneNo" runat="server" Style="margin-bottom: 7px;" Width="354px" CssClass="lblbox"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="ftbePhoneNo" ValidChars=",0123456789" runat="server"
                                                                TargetControlID="txtPhoneNo">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label2" runat="server" Width="100px" Style="margin-left: 50px;" Text="SMS Message" CssClass="lblbox"></asp:Label>
                                                            </strong>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSMS" runat="server" Style="margin-bottom: 7px;" Width="350px" placeholder="Type your message ..." TextMode="MultiLine" Height="100px" CssClass="lblbox" MaxLength="765"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="right">
                                                            <asp:Panel ID="Panel1" runat="server" Height="20px" ScrollBars="None"
                                                                BorderWidth="0px" BorderStyle="Dashed">
                                                                <span id="spanDisplay"></span>/
                                                                        <asp:Label ID="MsgTotal" runat="server" Text="765"></asp:Label>
                                                                [<asp:Label ID="SMSCount" runat="server" Text="0"></asp:Label>/<asp:Label ID="Label4" runat="server" Text="5 SMS"></asp:Label>]
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Button ID="btnSendSMS" OnClick="btnSendSMS_Click" runat="server" Style="margin-left: 154px;" Text="Send SMS" CssClass="Button" />
                                                            <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Text="Cancel" CssClass="Button" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:HiddenField ID="hfNumber" runat="server"></asp:HiddenField>
                </td>
            </tr>
            <tr>
            </tr>
        </table>
    </div>
</asp:Content>