<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmTransferOutIn.aspx.cs" Inherits="Forms_frmTransferOutIn" Title="CORN :: Transfer In" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
            <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
<script language="JavaScript" type="text/javascript">

    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
    function BeginRequestHandler(sender, args) { var oControl = args.get_postBackElement(); oControl.disabled = true; }

    function pageLoad() {
        $("select").searchable();
    }</script>
    <div id="right_data">
        <table width="100%">
            <tr>

                <td align="left">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <table>
                                <tbody>

                                    <tr>
                                        <td width="5px"></td>
                                        <td style="width:90px; height: 25px" align="left">
                                            <strong>
                                                Document No
                                            </strong>
                                        </td>
                                        <td style="height: 25px">
                                            <asp:DropDownList ID="drpDocumentNo" runat="server" Width="200px" CssClass="DropList"
                                                AutoPostBack="True" OnSelectedIndexChanged="drpDocumentNo_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="5px"></td>
                                        <td style="height: 25px" align="left">
                                            <strong>
                                                Transfer From</strong>
                                        </td>
                                        <td style="height: 25px">
                                            <asp:DropDownList ID="DrpTransferFor" runat="server" Width="200px" CssClass="DropList" Enabled="false">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="5px"></td>
                                        <td style="height: 25px" align="left">
                                            <strong>
                                                Transfer To</strong>
                                        </td>
                                        <td style="height: 25px">
                                            <asp:DropDownList ID="drpDistributor" runat="server" Width="200px" CssClass="DropList"
                                                Enabled="false">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td width="5px"></td>
                                        <td style="height: 25px" align="left">
                                            <strong>
                                                Order No</strong>
                                        </td>
                                        <td style="height: 25px">
                                            <asp:TextBox ID="txtDocumentNo" runat="server" Width="195px" CssClass="txtBox"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="5px"></td>
                                        <td style="height: 25px" align="left">
                                            <strong>
                                               Builty No</strong>
                                        </td>
                                        <td style="height: 25px">
                                            <asp:TextBox ID="txtBuiltyNo" runat="server" Width="195px" CssClass="txtBox"></asp:TextBox>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>

            </tr>
        </table>
        <table width="100%">
            <tr>
                <td width="5px"></td>
                <td align="left" style="height: 220px">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table width="50%">
                                <tbody>
                                    <tr>
                                        <td class="lblDetail" style="width: 150px;">Item Code
                                        </td>
                                        <td class="lblDetail" style="width: 230px;">Item Name
                                        </td>

                                        <td class="lblDetail" style="width: 80px;">Quantity
                                        </td>


                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td align="left" colspan="5">
                                            <asp:Panel ID="Panel2" runat="server" Width="480px" Height="140px" ScrollBars="Vertical"
                                                BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver">
                                                <asp:GridView ID="GrdPurchase" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                    BorderColor="White" CssClass="gridRow2" ForeColor="SteelBlue" HorizontalAlign="Center"
                                                    Width="100%">

                                                    <RowStyle ForeColor="Black" />
                                                    <Columns>
                                                        <asp:BoundField DataField="SKU_ID" HeaderText="SKU_ID">
                                                            <HeaderStyle CssClass="HidePanel" />
                                                            <ItemStyle CssClass="HidePanel" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="SKU_CODE" HeaderText="Item Code">
                                                            <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Left"
                                                                Width="85px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="SKU_NAME" HeaderText="Item Name">
                                                            <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Left"
                                                                Width="205px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Quantity" HeaderText="Quantity">
                                                            <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Right"
                                                                Width="75px" />
                                                        </asp:BoundField>

                                                    </Columns>
                                                    <HeaderStyle BackColor="#007395" Font-Bold="True" ForeColor="White" HorizontalAlign="Center"
                                                        VerticalAlign="Middle" />
                                                    <AlternatingRowStyle BackColor="#F2F2F2" CssClass="GridAlternateRowStyle" ForeColor="#333333" />
                                                </asp:GridView>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            &nbsp;&nbsp;
                                <asp:Button AccessKey="S" ID="btnTransferIn" runat="server" Width="90px"
                                    Text="Transfer In" UseSubmitBehavior="False" OnClick="btnTransferIn_Click" CssClass="Button"></asp:Button>&nbsp;
                                <asp:Button AccessKey="C" ID="btnCancel" runat="server" Width="90px"
                                    Text="Cancel" UseSubmitBehavior="False" CssClass="Button"
                                    OnClick="btnCancel_Click"></asp:Button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td style="width: 100px;"></td>
            </tr>
        </table>
    </div>
</asp:Content>
