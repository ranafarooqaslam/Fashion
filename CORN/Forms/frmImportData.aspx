<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmImportData.aspx.cs" Inherits="Forms_frmImportData" Title="CORN :: Import Data" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">
        <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
<script language="JavaScript" type="text/javascript">
    function pageLoad() {
        $("select").searchable();
    }
 </script>
    <div id="right_data">
        <table width="100%">
            <tr>
                <td>
                    <div style="z-index: 101; left: 486px; width: 100px; position: absolute; top: 191px;
                        height: 100px">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <asp:ImageButton ID="ImageButton1" runat="server" Height="23px" ImageUrl="~/App_Themes/Granite/Images/image003.gif"
                                    Width="22px" />
                                Record Update
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </div>
                    <table>
                        <tbody>
                            <tr>
                                <td align="left" style="width: 59px; height: 25px">
                                </td>
                                <td align="left">
                                    <strong>
                                        <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" Width="175px"></asp:Label></strong>
                                </td>
                                <td style="height: 10px">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 59px; height: 25px">
                                    <strong>
                                        <asp:Label ID="Label4" runat="server" CssClass="lblbox" Text="File Type" Width="58px"></asp:Label></strong>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="cboFileTypes" runat="server" Width="200px" 
                                        CssClass="DropList" AutoPostBack="true" OnSelectedIndexChanged="cboFileTypes_SelectedIndexChanged">
                                        <asp:ListItem Value="3">Items</asp:ListItem>
                                        <asp:ListItem Value="4">Item Price</asp:ListItem>
                                        <asp:ListItem Value="5">Purchase</asp:ListItem>
                                        <asp:ListItem Value="6">Physical Stock Taking</asp:ListItem>
                                        <asp:ListItem Value="7">Opening Stock</asp:ListItem>
                                        <asp:ListItem Value="8">Customer</asp:ListItem>                                      
                                    </asp:DropDownList>
                                </td>
                                <td style="height: 10px">
                                    <asp:LinkButton ID="lnkFormat" runat="server" Text="Click to download Format" OnClick="lnkFormat_Click"></asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 25px; width: 59px;" align="left">
                                    <strong>
                                        <asp:Label ID="Label1" runat="server" Width="58px" Text="Location" CssClass="lblbox"></asp:Label></strong>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="DrpDistributor" runat="server" Width="200px" 
                                        CssClass="DropList" AutoPostBack="true" OnSelectedIndexChanged="DrpDistributor_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="height: 10px">
                                </td>
                            </tr>
                     
                             
                            <tr>
                                <td align="left" style="width: 59px; height: 25px">
                                    <strong>
                                        <asp:Label ID="Label3" runat="server" CssClass="lblbox" Text="Principal" Width="46px"></asp:Label></strong>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="DrpPrincipal" runat="server" Width="200px" CssClass="DropList">
                                    </asp:DropDownList>
                                </td>
                                <td style="height: 10px">
                                </td>
                            </tr>
                            <tr runat="server" id="dateRow" visible="false">
                                <td align="left" style="width: 59px; height: 25px">
                                            <strong>
                                                <asp:Label ID="Label2" runat="server" Height="13px" Text=" Date" Width="50px"></asp:Label></strong>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="txtBox" MaxLength="10"
                                                onkeyup="BlockStartDateKeyPress()" Width="160px"></asp:TextBox>
                                            <asp:ImageButton ID="ibtnStartDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                                Width="16px" />
                                        </td>
                                              <td align="center" style="height: 10px">
                                            </td>
                                             
                                    </tr>
                            <cc1:CalendarExtender ID="CEStartDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnStartDate"
                                        TargetControlID="txtStartDate">
                                    </cc1:CalendarExtender>

                            <tr>
                                <td align="left" style="width: 59px; height: 25px">
                                </td>
                                <td align="left">
                                </td>
                                <td style="height: 10px">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2" style="height: 25px">
                                    <asp:FileUpload ID="txtFile" runat="server" Width="287px" />
                                </td>
                                <td style="height: 10px">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 59px; height: 25px">
                                </td>
                                <td align="left">
                                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" ValidationGroup="vg"
                                        CssClass="Button" />
                                </td>
                                <td style="height: 10px">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2" style="height: 25px">
                                </td>
                                <td style="height: 10px">
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
