<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmPurchaseEntry.aspx.cs" Inherits="Forms_frmPurchaseEntry" Title="CORN :: Stock Register" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../AjaxLibrary/zebra_dialog.js"></script>
    <link href="../css/zebra_dialog.css" rel="stylesheet" />
    <script language="JavaScript" type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) { var oControl = args.get_postBackElement(); oControl.disabled = true; }

        function pageLoad() {
            $("select").searchable();
        }
        function Error(Msg) {

            $.Zebra_Dialog("Must Enter "+ Msg, { 'title': 'Error', 'type': 'error' });
        }
        function LocationError() {
                
            $.Zebra_Dialog("Transfer to Location must be different",  { 'title': 'Error', 'type': 'error' });
        }
        function WrongLocation() {

            $.Zebra_Dialog("Wrong Location", { 'title': 'Error', 'type': 'error' });
        }
        function DayClose() {

            $.Zebra_Dialog("Please Check Transfer To Location Date", { 'title': 'Error', 'type': 'error' });
        }
        function DetailError() {

            $.Zebra_Dialog("At least one Item enter", { 'title': 'Error', 'type': 'error' });
        }
        function PriceError() {

            $.Zebra_Dialog("Please enter Price", { 'title': 'Error', 'type': 'error' });
        }
        function ValidateForm() {
            var str;

            str = document.getElementById('<%=txtQuantity.ClientID%>').value;
            if (str == null || str.length == 0) {

                Error("Quantity");

                document.getElementById('<%=txtQuantity.ClientID%>').focus();

                return false;
            }
            str = document.getElementById('<%=txtDocumentNo.ClientID%>').value;
            lblInvoice = document.getElementById('<%=lblInvoice.ClientID%>').innerHTML;
            if (str == null || str.length == 0) {
                if (lblInvoice == 'Driver Name') {
                    Error("Driver Name");
                } else {
                    Error("Invoice/DC No");
                }
                document.getElementById('<%=txtDocumentNo.ClientID%>').focus();
                return false;
            }
            str = document.getElementById('<%=txtBuiltyNo.ClientID%>').value;
            if (str == null || str.length == 0) {
                
                Error("Builty No");
                document.getElementById('<%=txtBuiltyNo.ClientID%>').focus();
                return false;
            }
            return true;
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
        function onlyNumbers(txt, event) {
            var charCode = (event.which) ? event.which : event.keyCode;
            if (charCode == 9 || charCode == 8) {
                return true;
            }
            if (charCode == 46) {
                return true;
            }
            if (charCode == 31 || charCode < 48 || charCode > 57)
                return false;

            return true;
        } 
    </script>
    <div id="right_data">
        <div>
            <table width="65%">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tbody>
                                        <tr>
                                            <td style="height: 24px" align="left">
                                                <strong>
                                                    <asp:Label ID="Label2" runat="server" CssClass="lblbox" Height="14px" Text="Transaction Type"
                                                        Width="98px"></asp:Label></strong>
                                            </td>
                                            <td style="height: 24px">
                                                <asp:DropDownList ID="DrpDocumentType" runat="server" AutoPostBack="True" CssClass="DropList"
                                                    OnSelectedIndexChanged="DrpDocumentType_SelectedIndexChanged" Width="200px">
                                                    <asp:ListItem Value="2">Purchase</asp:ListItem>
                                                    <asp:ListItem Value="5">Transfer Out</asp:ListItem>
                                                    <asp:ListItem Value="3">Purchase Return</asp:ListItem>
                                                    <%-- <asp:ListItem Value="4">Transfer In</asp:ListItem>--%>
                                                    <asp:ListItem Value="6">Shop Damage</asp:ListItem>
                                                    <asp:ListItem Value="10">Import Damage</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td style="height: 24px">
                                            </td>
                                            <td align="left" style="height: 25px">
                                                <strong>
                                                    <asp:Label ID="lblDocumentNo" runat="server" Text="Document No" Width="94px"></asp:Label></strong>
                                            </td>
                                             <td style="height: 25px">
                                                <asp:DropDownList ID="drpDocumentNo" runat="server" AutoPostBack="True" CssClass="DropList"
                                                    OnSelectedIndexChanged="drpDocumentNo_SelectedIndexChanged" Width="200px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>                                       
                                        <tr>
                                            <td align="left" style="height: 25px">
                                                <strong>
                                                    <asp:Label ID="lbltoLocation" runat="server" CssClass="lblbox" Text="Vendor" Width="94px"></asp:Label></strong>
                                            </td>
                                            <td style="height: 25px">
                                                <asp:DropDownList ID="drpPrincipal" runat="server"  CssClass="DropList"
                                                     Width="200px">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="height: 25px">
                                            </td>
                                             <td align="left" style="height: 25px">
                                                <strong>
                                                    <asp:Label ID="lblfromLocation" runat="server" CssClass="lblbox" Text="Purchase For"
                                                        Width="94px"></asp:Label></strong>
                                            </td>
                                               <td style="height: 25px">
                                                <asp:DropDownList ID="drpDistributor" runat="server" AutoPostBack="True" CssClass="DropList"
                                                    Width="200px" OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>                                      
                                        <tr>
                                            <td style="height: 25px" align="left">
                                                <strong>
                                                    <asp:Label ID="Label4" runat="server" CssClass="lblbox" Text="Transfer To" Visible="False"
                                                        Width="82px"></asp:Label></strong>
                                            </td>
                                            <td style="height: 25px">
                                                <asp:DropDownList ID="DrpTransferFor" runat="server" CssClass="DropList" Visible="False"
                                                    Width="200px">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="height: 25px">
                                            </td>
                                             <td style="height: 25px;" align="left">
                                                <strong>
                                                    <asp:Label ID="lblInvoice" runat="server" CssClass="lblbox" Text="INV/DC  No" Width="94px"></asp:Label></strong>
                                            </td>
                                              <td>
                                                <asp:TextBox ID="txtDocumentNo" runat="server" CssClass="txtBox" Width="195px"></asp:TextBox>
                                            </td>
                                        </tr>                                       
                                        <tr>
                                            <td style="height: 25px;" align="left">
                                                <strong>
                                                    <asp:Label ID="Label3" runat="server" CssClass="lblbox" Text="Builty No" Width="94px"></asp:Label></strong>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBuiltyNo" runat="server" CssClass="txtBox" Width="195px"></asp:TextBox>
                                            </td>
                                            <td style="width: 1px" valign="top">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox Checked="true" ID="chkScan" runat="server" Text="ByScan" Font-Bold="true"
                                                    OnCheckedChanged="chkScan_CheckedChanged" AutoPostBack="true" />
                                                <%-- By Scan<input type="checkbox" id="chkScan" checked="checked"  onclick="myFunction();"/>--%>
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
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div style="margin-left: 65%; margin-top: -10%; position: absolute;">
                    <b>Working Date:</b>
                    <asp:Label ID="lblWorkDate" ForeColor="Red" runat="server" CssClass="lblbox" Text=""></asp:Label>
                    <br />
                    <br />
                    <b><asp:Label ID="lblStock" ForeColor="Red" runat="server" Text="Closing Stock: 0"></asp:Label> </b>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <table width="100%">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:HiddenField runat="server" ID="hfRowNo" />
                                <table>
                                    <tbody>
                                        <tr>
                                            <td class="lblDetail" colspan="2">
                                                Item Description
                                            </td>
                                            <td>
                                            </td>
                                            <td class="lblDetail" align="center">
                                                Color
                                            </td>
                                            <td class="lblDetail" align="center">
                                                Size
                                            </td>
                                            <td class="lblDetail">
                                                Quantity
                                            </td>
                                            <td class="lblDetail">
                                                Price
                                            </td>
                                            <td class="lblDetail">
                                                Amount
                                            </td>
                                            <%--<td>
                                                <asp:Label ID="lblFreeSKU" runat="server" Width="75px" Height="16px" ForeColor="White"
                                                    Font-Bold="True" Text="Free SKU" CssClass="lblbox" BackColor="#006699"></asp:Label>
                                            </td>--%>
                                            <%-- <td style="height: 16px" align="center">
                                                <asp:Label ID="lblBatchNo" runat="server" Width="100%" Height="16px" ForeColor="White"
                                                    Font-Bold="True" Text="Batch No" CssClass="lblbox" BackColor="#006699" Enabled="False"></asp:Label>
                                            </td>--%>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtskuCode" runat="server" Width="340px" placeholder="Please enter Code here" AutoPostBack="true" OnTextChanged="txtskuCode_TextChanged"></asp:TextBox>
                                                <asp:TextBox ID="txtskuName" runat="server" Width="200px" Font-Bold="True" CssClass="txtBox"
                                                    Enabled="False" Visible="false"></asp:TextBox>
                                                <asp:DropDownList ID="drpSkus" runat="server" Width="340px" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="drpSkus_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtcolor" runat="server" Width="76px" CssClass="txtBox" Enabled="False"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtsize" runat="server" Width="76px" CssClass="txtBox" Enabled="False"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtQuantity" runat="server" Width="70px" Text="1" CssClass="txtBox "
                                                onkeypress="return onlyNumbers(this,event);"></asp:TextBox>
                                            </td>
                                            <%-- <td>
                                                <asp:TextBox ID="txtFreeSKU" runat="server" Width="70px" CssClass="txtBox" Enabled="False">0</asp:TextBox>
                                            </td>--%>
                                            <%--<td>
                                                <asp:TextBox ID="txtBatchNo" runat="server" Width="76px" CssClass="txtBox" Enabled="False">N/A</asp:TextBox>
                                            </td>--%>
                                            <td>
                                                <asp:TextBox ID="txtPrice" runat="server" Width="70px" CssClass="txtBox "
                                                onkeypress="return onlyDotsAndNumbers(this,event);"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAmount" runat="server" Width="70px" CssClass="txtBox " Enabled="false"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Button AccessKey="A" ID="btnSave" OnClick="btnSave_Click" runat="server" Width="100px"
                                                    Font-Size="8pt" Text="Add" ValidationGroup="vg" CssClass="Button" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="8">
                                                <asp:Panel ID="Panel2" runat="server" Width="852px" Height="270px" ScrollBars="Vertical"
                                                    BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver">
                                                    <asp:GridView ID="GrdPurchase" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                        ForeColor="SteelBlue" HorizontalAlign="Center" OnRowDeleting="GrdPurchase_RowDeleting"
                                                        OnRowEditing="GrdPurchase_RowEditing" ShowHeader="False" Width="100%" OnRowDataBound="GrdPurchase_RowDataBound">
                                                        <RowStyle ForeColor="Black" />
                                                        <Columns>
                                                            <asp:BoundField DataField="SKU_ID" HeaderText="SKU_ID">
                                                                <HeaderStyle CssClass="HidePanel" />
                                                                <ItemStyle CssClass="HidePanel" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SKU_CODE" HeaderText="Item Code">
                                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Left"
                                                                    Width="85px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SKU_NAME" HeaderText="Item Name">
                                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Left"
                                                                    Width="205px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="FREE_SKU" HeaderText="Free SKU">
                                                                <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                                <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="BATCH_NO" HeaderText="BatchNo">
                                                                <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                                <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="COLOR" HeaderText="COLOR">
                                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Right"
                                                                    Width="75px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="PACKSIZE" HeaderText="PACKSIZE">
                                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Right"
                                                                    Width="75px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Quantity" HeaderText="Quantity">
                                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Right"
                                                                    Width="75px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Price">
                                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Right"
                                                                    Width="75px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Amount">
                                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Right"
                                                                    Width="75px" />
                                                            </asp:BoundField>
                                                            <asp:CommandField HeaderText="Edit" ShowEditButton="True">
                                                                <ItemStyle BorderColor="Silver" BorderWidth="1px" Width="40px" />
                                                            </asp:CommandField>
                                                            <asp:TemplateField HeaderText="Delete">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" OnClientClick="javascript:return confirm('Are you sure you want to Delete?');return false;"
                                                                        Text="Delete"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" Width="45px" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <FooterStyle BackColor="White" />
                                                        <PagerStyle BackColor="Transparent" />
                                                        <HeaderStyle BackColor="#007395" Font-Bold="True" ForeColor="White" HorizontalAlign="Center"
                                                            VerticalAlign="Middle" />
                                                        <AlternatingRowStyle BackColor="#F2F2F2" CssClass="GridAlternateRowStyle" ForeColor="#333333" />
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
                                            <td valign="bottom">
                                               <strong>
                                                   <asp:Label ID="lblDiscount" runat="server" Text="Discount"></asp:Label>
                                               </strong>
                                                <br />
                                                <asp:TextBox ID="txtDiscount" runat="server" Width="88px" CssClass="txtBox"
                                                    onkeypress="return onlyNumbers(this,event);"></asp:TextBox>
                                               <br />
                                                <br />
                                                <strong>
                                                   <asp:Label ID="lblNetAmount" runat="server" Text="Net Amount"></asp:Label>
                                               </strong>
                                                <br />
                                                <asp:TextBox ID="txtNetAmount" runat="server" Width="88px" CssClass="txtBox" ReadOnly="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <strong>
                                    <asp:Label ID="Label7" runat="server" Width="103px" Height="16px" Text="Total Quantity"></asp:Label></strong>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTotalQuantity" runat="server" Width="88px" CssClass="txtBox"
                                    ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <strong>
                                    <asp:Label ID="Label1" runat="server" Width="103px" Height="16px" Text="Total Amount"></asp:Label></strong>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTotalAmount" runat="server" Width="88px" CssClass="txtBox"
                                    ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                        <asp:Button AccessKey="S" ID="btnSaveDocument" runat="server" Width="119px" Font-Size="8pt"
                            Text="Save Document" UseSubmitBehavior="False" OnClick="btnSaveDocument_Click"
                            CssClass="Button" />
                        <asp:Button AccessKey="C" ID="btnCancel" runat="server" Width="120px" Font-Size="8pt"
                            Text="Cancel" UseSubmitBehavior="False" OnClick="btnCancel_Click" CssClass="Button" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
