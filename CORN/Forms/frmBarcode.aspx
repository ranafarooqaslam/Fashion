<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmBarcode.aspx.cs" Inherits="Forms_frmBarcode" Title="CORN :: SKU Barcode" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="Server">
    <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function pageLoad() {
            $("select").searchable();
        }
        function ValidateForm() {
            var str;

            str = document.getElementById('<%=txt_row.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Plz Insert no of Rows..');
                return false;
            }
            str = document.getElementById('<%=txt_col.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Plz Insert no of colmns..');
                return false;
            }
            var str;

            str = document.getElementById('<%=txt_productName.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Plz Select Item');
                return false;
            }
            str = document.getElementById('<%=txt_code.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Plz Insert Code');
                return false;
            }
            str = document.getElementById('<%=txt_price.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Plz Insert Code');
                return false;
            }
            return true;
        }

        function ValidateForm2() {
            var str;

            str = document.getElementById('<%=txt_productName.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Plz Select Item');
                return false;
            }
            str = document.getElementById('<%=txt_code.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Plz Insert Code');
                return false;
            }
            str = document.getElementById('<%=txt_price.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Plz Insert Price');
                return false;
            }
            str = document.getElementById('<%=txt_row.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Plz Insert no of Rows..');
                return false;
            }
            else if (str.length > 10) {
                alert('Range is not valid..');
            }

            return true;
        }
        function ProductSelected(source, eventArgs) {
            var SKUDetail = eventArgs.get_text();
            var num = eventArgs.get_value();
            num = Math.round(num);

            document.getElementById("<%= txt_price.ClientID %>").value = num;

            document.getElementById("<%=txt_code.ClientID %>").value = SKUDetail.substring(0, SKUDetail.indexOf('-'));
            document.getElementById("<%=txt_productName.ClientID %>").value = SKUDetail.substring(SKUDetail.indexOf('-') + 1);
            document.getElementById("<%= btnDummy.ClientID %>").click();
        }
    </script>
    <div id="right_data">
        <asp:UpdatePanel ID="up_pnl1" runat="server">
            <ContentTemplate>
                <div>
                    <table cellspacing="10">
                        <tr>
                            <td>
                                <strong>Select Sheet</strong>
                            </td>
                            <td>
                                <select ID="ddlSheet" name="ddlSheet" runat="server" Width="200px">
                                    <option value="1">Barcode Printer (4 x 1 inches)</option>
                                    <option value="2">A4 Sticker Sheet</option>
                                    <option value="3">Single Sticker Barcode Printer</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <strong>Select Sticker Size</strong>
                            </td>
                            <td>
                                <select ID="ddlStickerSize" name="ddlStickerSize" runat="server" Width="200px">
                                    <option value="1">2 x 1 inches</option>
                                    <option value="2">1.18 x 0.74 inches</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <strong>Select Item</strong>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSKU" runat="server" Width="200px" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlSKU_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <strong>Item Name</strong>
                                <asp:Button ID="btnDummy" runat="server" CssClass="HidePanel" />
                            </td>
                            <td>
                                <asp:TextBox ID="txt_productName" runat="server" Width="200px"></asp:TextBox>
                                <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
                                <cc1:AutoCompleteExtender ID="aceProduct" runat="server" TargetControlID="txt_productName"
                                    ServicePath="wsProductList.asmx" MinimumPrefixLength="1" CompletionInterval="500"
                                    CompletionSetCount="10" CompletionListCssClass="autocomplete_completionListElement"
                                    CompletionListItemCssClass="autocomplete_listItem" EnableCaching="true" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    OnClientItemSelected="ProductSelected" FirstRowSelected="true" ServiceMethod="GetBarcodeProducts">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td style="width: 100px;"></td>
                        </tr>
                        <tr>
                            <td>
                                <strong>Item Code</strong>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_code" runat="server" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <strong>Price</strong>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_price" runat="server" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <strong>Size</strong>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSize" runat="server" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <strong>Color</strong>
                            </td>
                            <td>
                                <asp:TextBox ID="txtColor" runat="server" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <strong>Print</strong>
                            </td>
                            <td>
                                <asp:CheckBox ID="cbCompany" runat="server" Text="Company Name" Checked="true" />
                                &nbsp;
                                <asp:CheckBox ID="cbName" runat="server" Text="Item Name" Checked="true" />
                                &nbsp;
                                <asp:CheckBox ID="cbPrice" runat="server" Text="Item Price" Checked="true" />
                                &nbsp;
                                <asp:CheckBox ID="cbSize" runat="server" Text="Item Size" Checked="true" />
                                &nbsp;
                                <asp:CheckBox ID="cbColor" runat="server" Text="Item Color" Checked="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label Text="No Of Row's" runat="server" Visible="true" Font-Bold="true"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_row" runat="server" Width="60" MaxLength="2" Text="10" Visible="true"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="ftbe_txtrow" runat="server" ValidChars="0123456789"
                                    TargetControlID="txt_row">
                                </cc1:FilteredTextBoxExtender>
                                <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txt_row"
                                    MaximumValue="10" MinimumValue="1" SetFocusOnError="true" ValidationGroup="vg"></asp:RangeValidator>
                                <asp:TextBox ID="txt_col" runat="server" Width="60" MaxLength="1" Text="3" ReadOnly="true" Visible="false"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="ftbe_txtcol" runat="server" ValidChars="012345"
                                    TargetControlID="txt_col">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <hr />
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="Panel1" runat="server" Visible="false">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_companyname" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_color" runat="server"></asp:Label>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_pcode" runat="server"></asp:Label>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_pprice" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Image ID="img_brcode" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btn_print" runat="server" Text="Print" OnClick="btn_print_Click"
                            Width="80" CssClass="Button" Visible="false" />
                        <asp:Button ID="btn_generate" runat="server" Text="Generate" OnClick="btn_generate_Click"
                            Width="80" CssClass="Button" CausesValidation="true" ValidationGroup="vg" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>