<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmAccountHeadAssignmentDis.aspx.cs" Inherits="Forms_frmAccountHeadAssignmentDis"
    Title="CORN :: Account Head Assignment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">
    <script language="javascript" type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(startRequest);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);
        function startRequest(sender, e) {
            document.getElementById('<%=btnAssign.ClientID%>').disabled = true;
        }

        function endRequest(sender, e) {
            document.getElementById('<%=btnAssign.ClientID%>').disabled = false;
        }

        function SelectAllAccountHead() {
            var chkBoxList = document.getElementById('<%= ChAccountHead.ClientID %>');
            var chkBox = document.getElementById('<%= ChAll.ClientID %>');
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
            var chkBox = document.getElementById('<%= ChAll.ClientID %>');
            var chkBoxList = document.getElementById('<%= ChAccountHead.ClientID %>');
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
    </script>
    <div id="right_data">
        <div>
            <table width="100%">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <table width="500" align="center">
                                    <tbody>
                                         <tr>
                                            <td style="width: 100%" colspan="5">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 5%; height: 22px">
                                                &nbsp;
                                            </td>
                                            <td style="width: 25%; height: 22px" align="right">
                                                <strong>Type:</strong>
                                            </td>
                                            <td style="width: 5%; height: 22px">
                                                &nbsp;
                                            </td>
                                            <td style="width: 50%; height: 22px" align="left">
                                                <fieldset style="width: 60%;">
                                                    <asp:RadioButtonList ID="rblType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblType_SelectedIndexChanged">
                                                        <asp:ListItem Selected="True" Value="0">General Account Head</asp:ListItem>
                                                        <asp:ListItem Value="1">Petty Head</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                    <legend></legend>
                                                </fieldset>
                                            </td>
                                            <td style="width: 15%; height: 22px">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 5%; height: 22px">
                                            </td>
                                            <td align="right" style="width: 25%; height: 22px">
                                                <strong>Location:</strong>
                                            </td>
                                            <td style="width: 5%; height: 22px">
                                            </td>
                                            <td align="left" style="width: 60%; height: 22px">
                                                <asp:DropDownList ID="DrpDistributor" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DrpDistributor_SelectedIndexChanged"
                                                    Width="200px">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 5%; height: 22px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 5%">
                                            </td>
                                            <td style="width: 25%" align="left">
                                                <strong>Account Category</strong>
                                            </td>
                                            <td style="width: 5%">
                                            </td>
                                            <td style="width: 60%" align="left">
                                                <asp:DropDownList ID="DrpAccountCategory" runat="server" Width="200px" OnSelectedIndexChanged="DrpAccountCategory_SelectedIndexChanged"
                                                    AutoPostBack="True" __designer:wfdid="w103" CssClass="DropList">
                                                    <asp:ListItem>Balance Sheet Account</asp:ListItem>
                                                    <asp:ListItem>Income Statment Account</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 5%">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 5%; height: 24px">
                                            </td>
                                            <td style="width: 25%; height: 24px" align="left">
                                                <strong>Main Account Type:</strong>
                                            </td>
                                            <td style="width: 5%; height: 24px">
                                            </td>
                                            <td style="width: 60%; height: 24px" align="left">
                                                <asp:DropDownList ID="DrpMainAccountType" runat="server" Width="200px" OnSelectedIndexChanged="DrpMainAccountType_SelectedIndexChanged"
                                                    AutoPostBack="True" __designer:wfdid="w80" CssClass="DropList">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 5%; height: 24px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 5%">
                                            </td>
                                            <td style="width: 25%" align="left">
                                                <strong>Sub Account Type:</strong>
                                            </td>
                                            <td style="width: 5%">
                                            </td>
                                            <td style="width: 60%" align="left">
                                                <asp:DropDownList ID="DrpSubAccountType" runat="server" Width="200px" OnSelectedIndexChanged="DrpSubAccountType_SelectedIndexChanged"
                                                    AutoPostBack="True" __designer:wfdid="w81" CssClass="DropList">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 5%">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 5%">
                                            </td>
                                            <td style="width: 25%" align="left">
                                                <strong>Detail Account Type:</strong>
                                            </td>
                                            <td style="width: 5%">
                                            </td>
                                            <td style="width: 60%" align="left">
                                                <asp:DropDownList ID="DrpDetailAccountType" runat="server" Width="200px" OnSelectedIndexChanged="DrpDetailAccountType_SelectedIndexChanged"
                                                    AutoPostBack="True" __designer:wfdid="w82" CssClass="DropList">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 5%">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%" colspan="5">
                                                <div style="z-index: 101; left: 495px; width: 100px; position: absolute; top: 470px;
                                                    height: 100px">
                                                    <asp:Panel ID="Panel1" runat="server">
                                                        <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
                                                            <ProgressTemplate>
                                                                &nbsp;<asp:ImageButton ID="btnImage" runat="server" Height="33px" Width="31px" ImageUrl="~/App_Themes/Granite/Images/image003.gif" />
                                                            </ProgressTemplate>
                                                        </asp:UpdateProgress>
                                                    </asp:Panel>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 5%">
                                            </td>
                                            <td align="left" colspan="3">
                                                <strong>
                                                    <asp:Label ID="lblAccountHead" runat="server" Visible="False" Text="Account Head"
                                                        __designer:wfdid="w1"></asp:Label></strong>
                                            </td>
                                            <td style="width: 5%">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 5%">
                                            </td>
                                            <td align="left" colspan="3">
                                                <strong>
                                                    <asp:CheckBox ID="ChAll" onclick="SelectAllAccountHead()" runat="server" Visible="False"
                                                        Width="75px" Font-Size="8pt" Text="Select All" __designer:wfdid="w2" CssClass="DropList">
                                                    </asp:CheckBox>
                                                </strong>
                                            </td>
                                            <td style="width: 5%">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 5%; height: 36px">
                                            </td>
                                            <td style="height: 36px" align="left" colspan="3">
                                                <asp:Panel ID="Panel2" runat="server" Visible="False" Width="350px" Height="150px"
                                                    ScrollBars="Vertical" __designer:wfdid="w3" BackColor="White" BorderWidth="1px"
                                                    BorderStyle="Groove" BorderColor="Silver">
                                                    <asp:CheckBoxList ID="ChAccountHead" onclick="UnCheckSelectAll()" runat="server"
                                                        __designer:wfdid="w4">
                                                    </asp:CheckBoxList>
                                                    <div style="z-index: 101; left: -97px; width: 100px; position: absolute; top: 216px;
                                                        height: 100px">
                                                    </div>
                                                </asp:Panel>
                                            </td>
                                            <td style="width: 5%; height: 36px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 5%">
                                            </td>
                                            <td align="center" colspan="3">
                                            </td>
                                            <td style="width: 5%">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 5%">
                                            </td>
                                            <td align="center" colspan="3">
                                                <asp:Button ID="btnAssign" OnClick="btnAssign_Click" runat="server" Text="Assign"
                                                    CssClass="Button" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </td>
                                            <td style="width: 5%">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 5%">
                                            </td>
                                            <td align="center" colspan="3">
                                            </td>
                                            <td style="width: 5%">
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
