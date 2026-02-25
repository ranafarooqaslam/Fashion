<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" CodeFile="frmUserAssignment.aspx.cs"
    Inherits="Forms_frmUserAssignment" Title="CORN :: User Assignment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">
    <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
    <script language="JavaScript" type="text/javascript">
        function pageLoad() {
            $("select").searchable();
        }
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(startRequest);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);
        function startRequest(sender, e) {
            document.getElementById('<%=btnSave.ClientID%>').disabled = true;
        document.getElementById('<%=btnAssign.ClientID%>').disabled = true;
        document.getElementById('<%=btnAssignLocation.ClientID%>').disabled = true;
        document.getElementById('<%=btnUnAssignLocation.ClientID%>').disabled = true;
        document.getElementById('<%=btnAssignPrincipal.ClientID%>').disabled = true;
        document.getElementById('<%=btnAssignAllPrincipal.ClientID%>').disabled = true;
        document.getElementById('<%=btnUnAssignPrincipal.ClientID%>').disabled = true;
        document.getElementById('<%=btnUnAssignAllPrincipal.ClientID%>').disabled = true;
    }

    function endRequest(sender, e) {
        document.getElementById('<%=btnSave.ClientID%>').disabled = false;
        document.getElementById('<%=btnAssign.ClientID%>').disabled = false;
        document.getElementById('<%=btnAssignLocation.ClientID%>').disabled = false;
        document.getElementById('<%=btnUnAssignLocation.ClientID%>').disabled = false;
        document.getElementById('<%=btnAssignPrincipal.ClientID%>').disabled = false;
        document.getElementById('<%=btnAssignAllPrincipal.ClientID%>').disabled = false;
        document.getElementById('<%=btnUnAssignPrincipal.ClientID%>').disabled = false;
        document.getElementById('<%=btnUnAssignAllPrincipal.ClientID%>').disabled = false;
    }
    </script>

    <div id="right_data">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table width="100%">
                    <tbody>
                        <tr>
                            <td align="left">
                                <table width="100%">
                                    <tbody>
                                        <tr>
                                            <td style="width : 50px">
                                                <strong>
                                                    <asp:Label ID="Label5" runat="server" Width="107px" Text="User"></asp:Label>
                                                </strong></td><td align="left">
                                                <asp:DropDownList ID="ddUser" runat="server" Width="250px" __designer:wfdid="w62" CssClass="DropList" OnSelectedIndexChanged="ddUser_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 345px">
                                <cc1:TabContainer ID="tbUserAssignment" runat="server" Width="550px" Height="260px" AutoPostBack="True" ActiveTabIndex="0" OnActiveTabChanged="tbUserAssignment_ActiveTabChanged">
                                    <cc1:TabPanel runat="server" ID="TabPanel1">
                                        <HeaderTemplate>
                                            Location Assignment
                                 
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tbody>
                                                    <tr>
                                                        <td style="width: 5%; height: 250px"></td>
                                                        <td style="width: 90%" valign="top">
                                                            <table>
                                                                <tbody>
                                                                    <tr>
                                                                        <td style="height: 13px ; width : 70px" align="left" >
                                                                            <strong>
                                                                                <asp:Label ID="Label1" runat="server" Width="112px" Text="Location Type"></asp:Label></strong>
                                                                           </td>
                                                                        <td> <asp:DropDownList ID="ddDistributorType" runat="server" Width="250px" __designer:wfdid="w20" CssClass="DropList" OnSelectedIndexChanged="ddDistributorType_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                                                            </td>
                                                                   </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <table>
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td style="width: 40%" rowspan="4">
                                                                                            <asp:ListBox ID="lstUnAssignDistributor" runat="server" Width="200px" Height="200px" __designer:wfdid="w21" CssClass="DropList"></asp:ListBox>
                                                                                        </td>
                                                                                        <td style="width: 52px" align="center"></td>
                                                                                        <td style="width: 40%" rowspan="4">
                                                                                            <asp:ListBox ID="lstAssignDistributor" runat="server" Width="200px" Height="200px" __designer:wfdid="w22" CssClass="DropList"></asp:ListBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 52px" align="center">
                                                                                            <asp:Button ID="btnAssignLocation" OnClick="btnAssignLocation_Click" runat="server" Width="30px" Font-Size="8pt" Text=">" CssClass="Button" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 52px" align="center">
                                                                                            <asp:Button ID="btnUnAssignLocation" OnClick="btnUnAssignLocation_Click" runat="server" Width="30px" Font-Size="8pt" Text="<" CssClass="Button" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 52px" align="center"></td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                        <td style="width: 5%; height: 250px"></td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </ContentTemplate>
                                    </cc1:TabPanel>
                                    <cc1:TabPanel runat="server" TabIndex="1" ID="TabPanel2">
                                        <HeaderTemplate>
                                            Principal Assignment
                                 
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tbody>
                                                    <tr>
                                                        <td style="width: 2%; height: 250px"></td>
                                                        <td style="width: 96%; height: 250px" colspan="3">
                                                            <table>
                                                                <tbody>
                                                                    <tr>
                                                                        <td style="height: 265px" colspan="3">
                                                                            <table>
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td style="border-right: gray thin solid; border-top: gray thin solid; border-left: gray thin solid; width: 102px; border-bottom: gray thin solid" rowspan="4">
                                                                                            <asp:Panel ID="pnllstUnAssignBran" runat="server" Width="200px" Height="200px" __designer:wfdid="w842" BorderColor="#404040" BackColor="White" HorizontalAlign="Left">
                                                                                                <asp:ListBox ID="lstUnAssignBrand" runat="server" Width="200px" Height="200px" CssClass="DropList" __designer:wfdid="w843"></asp:ListBox>
                                                                                            </asp:Panel>
                                                                                        </td>
                                                                                        <td style="width: 75px" align="center">
                                                                                            <asp:Button ID="btnAssignPrincipal" OnClick="btnAssignPrincipal_Click" runat="server" Width="30px" Font-Size="8pt" Text=">" CssClass="Button" />
                                                                                        </td>
                                                                                        <td style="border-right: gray thin solid; border-top: gray thin solid; border-left: gray thin solid; width: 112px; border-bottom: gray thin solid" rowspan="4">
                                                                                            <asp:Panel ID="pnllstAssignBran" runat="server" Width="220px" Height="200px" ScrollBars="Vertical" __designer:wfdid="w845" BorderColor="#404040" BackColor="White" HorizontalAlign="Left">
                                                                                                <asp:CheckBoxList ID="lstAssignBrand" runat="server" Width="200px" CssClass="DropList" __designer:wfdid="w846" BorderColor="White" RepeatLayout="Flow"></asp:CheckBoxList>
                                                                                            </asp:Panel>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 75px" align="center">
                                                                                            <asp:Button ID="btnAssignAllPrincipal" OnClick="btnAssignAllPrincipal_Click" runat="server" Width="30px" Font-Size="8pt" Text=">>" CssClass="Button" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 75px" align="center">
                                                                                            <asp:Button ID="btnUnAssignPrincipal" OnClick="btnUnAssignPrincipal_Click" runat="server" Width="30px" Font-Size="8pt" Text="<<" CssClass="Button" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 75px" align="center">
                                                                                            <asp:Button ID="btnUnAssignAllPrincipal" OnClick="btnUnAssignAllPrincipal_Click" runat="server" Width="30px" Font-Size="8pt" Text="<" CssClass="Button" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td rowspan="1"></td>
                                                                                        <td style="width: 20%; height: 14px"></td>
                                                                                        <td rowspan="1"></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="height: 21px" align="center" colspan="3" rowspan="1">
                                                                                            <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Width="80px" Font-Size="8pt" Text="Save" CssClass="Button" />
                                                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                        <td style="width: 2%; height: 250px"></td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </ContentTemplate>
                                    </cc1:TabPanel>
                                    <cc1:TabPanel runat="server" HeaderText="TabPanel3" TabIndex="2" ID="TabPanel3">
                                        <HeaderTemplate>
                                            Voucher Type Assignment
                                 
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tbody>
                                                    <tr>
                                                        <td style="width: 5%; height: 250px"></td>
                                                        <td style="width: 90%; height: 250px" valign="middle" align="center">
                                                            <table>
                                                                <tbody>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <table>
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td style="border-right: gray thin solid; border-top: gray thin solid; border-left: gray thin solid; width: 112px; border-bottom: gray thin solid" rowspan="4">
                                                                                            <asp:Panel ID="pnlVoucherType" runat="server" Width="220px" Height="150px" ScrollBars="Vertical" __designer:wfdid="w854" BorderColor="#404040" BackColor="White" HorizontalAlign="Left">
                                                                                                <asp:CheckBoxList ID="cblVoucherType" runat="server" Width="200px" CssClass="DropList" __designer:wfdid="w855" BorderColor="White" RepeatLayout="Flow"></asp:CheckBoxList></asp:Panel>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                            <br />
                                                            <asp:Button ID="btnAssign" OnClick="btnAssign_Click" runat="server" Width="80px" Font-Size="8pt" Text="Assign" CssClass="Button" />
                                                        </td>
                                                        <td style="width: 5%; height: 250px"></td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </ContentTemplate>
                                    </cc1:TabPanel>
                                </cc1:TabContainer></td>
                        </tr>
                    </tbody>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
