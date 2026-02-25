<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" CodeFile="frmItemStockLevel.aspx.cs"
    Inherits="Forms_frmItemStockLevel" Title="CORN :: Item Stock Level" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<script runat="server">

    protected void chbDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void cblCategory_TextChanged(object sender, EventArgs e)
    {

    }
</script>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
    <script language="JavaScript" type="text/javascript">
        function pageLoad() {
            $("select").searchable();
        }
        function SelectAlldistributor() {
            var chkBoxList = document.getElementById('<%= chbDistributor.ClientID %>');
            var chkBox = document.getElementById('<%= chbAllDistributor.ClientID %>');
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

        function UnCheckSelectAlldistributor() {
            var chkBox = document.getElementById('<%= chbAllDistributor.ClientID %>');
            var chkBoxList = document.getElementById('<%= chbDistributor.ClientID %>');
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

            str = document.getElementById("<%= txtmaxStockLevel.ClientID %>").value;
            if (str == null || str.length == 0) {
                alert('Must Enter Maximum Stock Level');
                return false;
            }
            str = document.getElementById("<%= txtMinStkLevel.ClientID %>").value;
            if (str == null || str.length <= 1) {
                alert('Must Enter minumum Stock Level');
                return false;
            }
            str = document.getElementById("<%= txtReOrderLevel.ClientID %>").value;
            if (str == null || str.length <= 1) {
                alert('Must Enter ReOrder Level');
                return false;
            }
        }
    </script>
    <div id="right_data">
        <div style="z-index: 101; left: 300px; width: 100px; position: absolute; top: 251px; height: 100px">
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
                <ProgressTemplate>
                    <asp:ImageButton ID="ImageButton1" runat="server" Height="23px" ImageUrl="~/App_Themes/Granite/Images/image003.gif"
                        Width="22px" />
                    Record Update
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
        <div>
            <table width="100%">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline">
                            <ContentTemplate>
                                <table>
                                    <tbody>
                                        <tr>
                                            <td align="left" colspan="2">
                                                <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                            </td>
                                             <td style="width :5px" ></td>
                                            <td ></td>
                                            <td ></td>
                                        </tr>
                                        <tr>
                                            <td><strong>Category</strong></td>
                                            <td>
                                                <asp:DropDownList ID="ddlCategory" runat="server" Width="247px" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                                </asp:DropDownList></td>
                                             <td style="width :5px" ></td>
                                            <td><strong>Minumum Stock Level</strong></td>
                                            <td>
                                                <asp:TextBox ID="txtMinStkLevel" runat="server" Width="247px" CssClass="txtBox "></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td><strong>Sub Category</strong></td>
                                            <td>
                                                <asp:DropDownList ID="ddlSubCategory" runat="server" Width="247px" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlSubCategory_SelectedIndexChanged">
                                                </asp:DropDownList></td>
                                             <td style="width :5px" ></td>
                                            <td><strong>
                                                <asp:Label ID="Label2" runat="server" Text="Maximum Stock Level"></asp:Label></strong></td>

                                            <td>
                                                <asp:TextBox ID="txtmaxStockLevel" runat="server" Width="247px" CssClass="txtBox "></asp:TextBox>

                                                <%--  <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtcolor"
                                                    FilterType="Numbers">
                                                </cc1:FilteredTextBoxExtender>--%> </td>

                                        </tr>
                                        <tr>
                                            <td colspan="2"></td>
                                             <td style="width :5px" ></td>
                                            <td align="left">
                                                <strong>Re Order Level</strong>
                                            </td>
                                            <td width="300px">
                                                <asp:TextBox ID="txtReOrderLevel" runat="server" Width="247px" CssClass="txtBox "></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>Item Name</strong>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="ChbAllCategory" onclick="SelectAll()" runat="server" Text="All"></asp:CheckBox>
                                                <br />
                                                <asp:Panel ID="Panel1" runat="server" Width="247px" Height="150px" ScrollBars="Vertical"
                                                    BorderWidth="1px" BorderStyle="Groove">
                                                    <asp:CheckBoxList ID="cblCategory" onclick="UnCheckSelectAll()" runat="server">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                            </td>
                                             <td style="width :30px" ></td>
                                            <td align="left">
                                                <strong>Location
                                                </strong>
                                            </td>
                                            <td align="left">
                                                <asp:CheckBox ID="chbAllDistributor" onclick="SelectAlldistributor()" runat="server" Text="All"></asp:CheckBox>
                                                <br />
                                                <asp:Panel ID="Panel2" runat="server" Width="247px" Height="150px" ScrollBars="Vertical"
                                                    BorderWidth="1px" BorderStyle="Groove">
                                                    <asp:CheckBoxList ID="chbDistributor" onclick="UnCheckSelectAlldistributor()" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="chbDistributor_SelectedIndexChanged1">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>

                                            </td>



                                        </tr>




                                        <tr>
                                            <td></td>
                                            <td style="width: 201px; height: 36px" valign="middle" align="left">
                                                <asp:Button AccessKey="S" ID="btnSave" OnClick="btnSave_Click" runat="server" Width="90px"
                                                    Font-Size="8pt" Text="Save" CssClass="Button"  />&nbsp;
                                                <asp:Button AccessKey="C" ID="btnCancel" OnClick="btnCancel_Click" runat="server"
                                                    Width="90px" Font-Size="8pt" Text="Cancel" CssClass="Button" />
                                            </td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                    </tbody>
                                </table>
                                <%--  <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                    FilterType="Custom" ValidChars="0123456789." TargetControlID="txtAmount">
                                </ajaxToolkit:FilteredTextBoxExtender>--%>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table style="border-right: silver thin inset; border-top: silver thin inset; border-left: silver thin inset; width: 650px; border-bottom: silver thin inset">
                        <tbody>
                            <tr>
                                <td style="height: 20px" align="left" colspan="5">
                                    <asp:Panel ID="Panel12" runat="server" Width="750px" Height="200px" ScrollBars="Vertical">
                                        <asp:GridView ID="GrdOrder" runat="server" Width="99.7%" ForeColor="SteelBlue"
                                            AutoGenerateColumns="False" BackColor="White" BorderColor="SteelBlue"
                                            OnRowEditing="GrdOrder_RowEditing" OnRowDeleting="GrdOrder_RowDeleting">

                                            <Columns>
                                                <asp:BoundField DataField="DISTRIBUTOR_ID" HeaderText="DISTRIBUTOR_ID">
                                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="SKU_ID" HeaderText="SKU_ID">
                                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CETEGORY_ID" HeaderText="CETEGORY_ID">
                                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DISTRIBUTOR_NAME" HeaderText="Location">
                                                    <ItemStyle CssClass="grdDetail" Width="15%"></ItemStyle>
                                                    <HeaderStyle CssClass="grdHead"></HeaderStyle>
                                                </asp:BoundField>
                                              <%--  <asp:TemplateField HeaderText="Document Date">

                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# Bind("DOCUMENT_DATE", "{0:dd-MMM-yyyy}") %>' ID="Label1"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="grdDetail" Width="15%"></ItemStyle>
                                                    <HeaderStyle CssClass="grdHead"></HeaderStyle>
                                                </asp:TemplateField>--%>
                                                <asp:BoundField DataField="SKU_NAME" HeaderText="Item Name">
                                                    <ItemStyle CssClass="grdDetail" Width="20%"></ItemStyle>
                                                    <HeaderStyle CssClass="grdHead"></HeaderStyle>
                                                </asp:BoundField>
                                                  <asp:BoundField DataField="SKU_HIE_NAME" HeaderText="Cetegory">
                                                    <ItemStyle CssClass="grdDetail" Width="20%"></ItemStyle>
                                                    <HeaderStyle CssClass="grdHead"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="MIN_LEVEL" HeaderText="Minumum Level" DataFormatString="{0:f2}">
                                                    <ItemStyle CssClass="grdDetail" Width="10%" HorizontalAlign="Right"></ItemStyle>
                                                    <HeaderStyle CssClass="grdHead"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="MAX_LEVEL" HeaderText="Maximum Level" DataFormatString="{0:f2}">
                                                    <ItemStyle CssClass="grdDetail" Width="10%" HorizontalAlign="Right"></ItemStyle>
                                                    <HeaderStyle CssClass="grdHead"></HeaderStyle>
                                                </asp:BoundField>
                                                 <asp:BoundField DataField="RE_ORDER_LEVEL" HeaderText="Re Order Level" DataFormatString="{0:f2}">
                                                    <ItemStyle CssClass="grdDetail" Width="10%" HorizontalAlign="Right"></ItemStyle>
                                                    <HeaderStyle CssClass="grdHead"></HeaderStyle>
                                                </asp:BoundField>

                                                <asp:CommandField ShowEditButton="True" ShowHeader="True">
                                                    <ItemStyle CssClass="grdDetail" HorizontalAlign="Center" Width="8%" />

                                                </asp:CommandField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" OnClientClick="javascript:return confirm('Are you sure you want to Delete?');return false;"
                                                            CommandName="Delete"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="grdDetail" HorizontalAlign="Center" Width="8%" />

                                                </asp:TemplateField>

                                            </Columns>

                                            <HeaderStyle VerticalAlign="Middle" CssClass="grdHead"></HeaderStyle>
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>





