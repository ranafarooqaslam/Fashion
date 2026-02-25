<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmBarcodeBulk.aspx.cs" Inherits="Forms_frmBarcodeBulk" Title="CORN :: Bulk SKU Barcode" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="Server">
     <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function pageLoad() {
            $("select").searchable();
        }
        function SelectAll() {
            var chkBoxList = document.getElementById('<%= cblCategory.ClientID %>');
            var chkBox = document.getElementById('<%= ChbAllCategory.ClientID %>');
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
            var chkBox = document.getElementById('<%= ChbAllCategory.ClientID %>');
            var chkBoxList = document.getElementById('<%= cblCategory.ClientID %>');
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

        function ValidateForm() {
            var str;

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


    </script>
    <div id="right_data">
        <asp:UpdatePanel ID="up_pnl1" runat="server">
            <ContentTemplate>
                <div>
                    <table width="90%">
                        <tr>
                            <td style="width:15%;">
                                <strong>Select Sheet</strong>
                            </td>
                            <td style="width:85%;">
                                <select ID="ddlSheet" name="ddlSheet" runat="server" Width="200px">
                                    <option value="1">Barcode Printer (4 x 1 inches)</option>
                                    <option value="2">A4 Sticker Sheet</option>
                                    <option value="3">Single Sticker Barcode Printer</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%;">
                                <strong>Category</strong>
                            </td>
                            <td style="width:85%;">
                                <asp:DropDownList ID="ddlCategory" runat="server" Width="305" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%;">
                                <strong>Sub Category</strong>
                            </td>
                            <td style="width:85%;">
                                <asp:DropDownList ID="ddlSubCategory" runat="server" Width="305" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlSubCategory_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%;">
                                <strong>Item Name</strong>
                            </td>
                            <td style="width:85%;">
                                <asp:CheckBox ID="ChbAllCategory" onclick="SelectAll()" runat="server" Text="All">
                                </asp:CheckBox>
                                <br />
                                <asp:Panel ID="Panel2" runat="server" Width="305px" Height="250px" ScrollBars="Vertical"
                                    BorderWidth="1px" BorderStyle="Groove">
                                    <asp:CheckBoxList ID="cblCategory" onclick="UnCheckSelectAll()" runat="server">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <strong>Print</strong>
                            </td>
                            <td>
                                <asp:CheckBox ID="cbCompany" runat="server" Text="Company Name" Checked="true" />
                                &nbsp;
                                <asp:CheckBox ID="cbName" runat="server" Text="Item Name" Checked="true"/>
                                &nbsp;
                                <asp:CheckBox ID="cbPrice" runat="server" Text="Item Price" Checked="true"/>
                                &nbsp;
                                <asp:CheckBox ID="cbSize" runat="server" Text="Item Size" Checked="true" />
                                &nbsp;
                                <asp:CheckBox ID="cbColor" runat="server" Text="Item Color" Checked="true" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%;">
                                <asp:Label Text="No Of Row" runat="server" Visible="true" Font-Bold="true"></asp:Label>
                            </td>
                            <td style="width:85%;">
                                <asp:TextBox ID="txt_row" runat="server" Width="60" MaxLength="2" Text="10" Visible="true"></asp:TextBox>
                                <asp:RangeValidator ID="RangeValidator1" runat="server"  ControlToValidate="txt_row"
                                MaximumValue="10" MinimumValue="1" SetFocusOnError="true" ValidationGroup="vg"></asp:RangeValidator>
                                <asp:TextBox ID="txt_col" runat="server" Width="60" MaxLength="1" Text="3"  ReadOnly="true" Visible="false"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td colspan="3">
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
                                            <asp:Button ID="btnGenerate" runat="server" Text="Generate" OnClick="btnGenerate_Click"
                                                Width="80" CssClass="Button" CausesValidation="true" ValidationGroup="vg"/>
                                        </td>
                                    </tr>
                                </table>
                                </div>
    </div>
</asp:Content>
