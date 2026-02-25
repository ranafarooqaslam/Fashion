<%@ Page Title="" Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmProductSearch2.aspx.cs" Inherits="Forms_frmProductSearch2" %>

    
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="Server">
        <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
<script language="JavaScript" type="text/javascript">
    function pageLoad() {
        $("select").searchable();
    }
 </script>
 <link href="../css/POStyle2.css" rel="stylesheet" type="text/css" />

    <div id="right_data">
      <table>
            <tr>
                <td width="250px">
                    <strong>
                        <asp:Label ID="Label21" runat="server" Width="250px" Text="Enter Item code Or Name To Search:"></asp:Label></strong>
                </td>
                <td width="300px">
                    <asp:TextBox ID="txtSearch" runat="server" BorderColor="Black" Width="260px" Height="20px"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="Button" 
                        onclick="btnSearch_Click" />
             </td>
            </tr>
            <tr>
                <td colspan="3"><br />
                    <div class="menu2" style="width: 98.5%">
                        <div class="main">
                            <ul>
                                <li class="sku-c">Location</li>
                                <li class="sperator"></li>
                                <li class="sku-c">Item Code</li>
                                <li class="sperator"></li>
                                <li class="sku-nameproduct">Item Name</li>
                                <li class="sperator"></li>
                                <li class="color">Color</li>
                                <li class="sperator"></li>
                                <li class="size">Size</li>
                                <li class="sperator"></li>
                                <li class="u-prize">Unit Price</li>
                                <li class="sperator"></li>
                                <li class="u-prize">Closing Stock</li>
                                <li class="sperator"></li>
                            </ul>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <div>
                       <%-- <asp:ScriptManager runat="server" ID="smPOs">
                        </asp:ScriptManager>--%>
                        <asp:UpdatePanel ID="updatpanel12" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="Panel2" runat="server" Height="99%" ScrollBars="Vertical" Width="99%"
                                    BorderColor="Silver" BorderStyle="Groove" BorderWidth="3px">
                                    <asp:GridView ID="GrdPurchase" runat="server" ForeColor="SteelBlue" SelectedRowStyle-BackColor="DarkBlue"
                                        AutoGenerateColumns="false" BackColor="White"
                                        HorizontalAlign="Center" BorderColor="White" RowStyle-Height="25px" ShowHeader="false"
                                        Width="98.5%">
                                        <%--  <PagerSettings FirstPageText="" LastPageText="" Mode="NextPrevious" NextPageText="Next"
                            PreviousPageText="Previous"></PagerSettings>--%>
                                        <Columns>
                                            <asp:BoundField DataField="SKU_ID" HeaderText="SKU_ID">
                                                <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                <ItemStyle CssClass="HidePanel"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DISTRIBUTOR_NAME" HeaderText="Location">
                                                <ItemStyle HorizontalAlign="Left" BorderColor="Silver" BorderWidth="2px" BorderStyle="Solid"
                                                    Width="130px"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SKU_CODE" HeaderText="Item Code">
                                                <ItemStyle HorizontalAlign="Left" BorderColor="Silver" BorderWidth="2px" BorderStyle="Solid"
                                                    Width="140px"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SKU_NAME" HeaderText="Item Name">
                                                <ItemStyle HorizontalAlign="Left" BorderColor="Silver" BorderWidth="2px" BorderStyle="Solid"
                                                    Width="225px"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="COLOR" HeaderText="Item Color">
                                                <ItemStyle HorizontalAlign="Left" BorderColor="Silver" BorderWidth="2px" BorderStyle="Solid"
                                                    Width="100px"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PACKSIZE" HeaderText="Size">
                                                <ItemStyle HorizontalAlign="Left" BorderColor="Silver" BorderWidth="2px" BorderStyle="Solid"
                                                    Width="91px"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="TRADE_PRICE" HeaderText="Unit PRICE" DataFormatString="{0:F2}">
                                                <ItemStyle HorizontalAlign="Left" BorderColor="Silver" BorderWidth="2px" BorderStyle="Solid"
                                                    Width="100px"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CLOSING_STOCK" HeaderText="Closing Stock" >
                                                <ItemStyle HorizontalAlign="Left" BorderColor="Silver" BorderWidth="2px" BorderStyle="Solid"
                                                    Width="105px"></ItemStyle>
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:Label ID="lblNoRecords" Text="No records to display" runat="server" ForeColor="red" ></asp:Label>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
