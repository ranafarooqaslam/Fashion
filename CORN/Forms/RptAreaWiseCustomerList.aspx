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
                                    <td align="left">
                                    </td>
                                    <td align="left" width="70px">
                                        <strong>
                                            <asp:Label ID="lbltoLocation" runat="server" CssClass="lblbox" Text="Location" Width="61px"></asp:Label></strong>
                                    </td>
                                    <td style="height: 25px" align="left">
                                        <asp:DropDownList ID="drpDistributor" runat="server" Width="180px" CssClass="DropList"
                                            AutoPostBack="True" OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                    </td>
                                    <td align="left" width="70px">
                                        <strong>
                                            <asp:Label ID="Label3" runat="server" CssClass="lblbox" Text="Customer" Width="100px"></asp:Label></strong>
                                    </td>
                                    <td align="left" style="height: 25px">
                                        <asp:DropDownList ID="ddl_customer" runat="server" Width="180px" CssClass="DropList">
                                            <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="InActive" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
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
