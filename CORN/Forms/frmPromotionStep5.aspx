<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmPromotionStep5.aspx.cs" Inherits="Forms_frmPromotionStep5" Title="CORN :: Promotion Wizard Step 4" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">
       <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
<script language="JavaScript" type="text/javascript">
    function pageLoad() {
        $("select").searchable();
    }
        function ConfirmCancel() {
            if (confirm("Are you sure? You want to Return Home?") == true)
                return true;

            else
            { return false; }
        }
    </script>
    <div id="right_data">
        <table width="100%">
            <tr>
                <td>
                    <h2>
                        Promotion Wizard Step 4</h2>
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                    <asp:Label ID="lblErrorMessage" runat="server" AutoUpdateAfterCallBack="True" CssClass="lblbox"
                        Font-Names="Verdana" Font-Size="9pt" ForeColor="Red" Width="344px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table>
                                <tbody>
                                    <tr>
                                        <td style="width: 100px">
                                            <asp:TextBox ID="txtSummary" runat="server" Width="437px" Height="214px" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px; height: 12px">
                                            &nbsp;
                                            <table>
                                                <tbody>
                                                    <tr>
                                                        <td style="width: 100px">
                                                            <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Width="90" CssClass="Button"
                                                                Text="Home"></asp:Button>
                                                        </td>
                                                        <td style="width: 100px">
                                                            <asp:Button ID="btnBack" OnClick="btnBack_Click" runat="server" Width="90" CssClass="Button"
                                                                Text="Back"></asp:Button>
                                                        </td>
                                                        <td style="width: 100px">
                                                            <asp:Button ID="btnFinish" OnClick="btnFinish_Click" runat="server" Width="90" CssClass="Button"
                                                                Text="Finish"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                        <ProgressTemplate>
                            <asp:Image ID="Image1" runat="server" Width="33px" Height="23px" ImageUrl="~/App_Themes/Granite/Images/image003.gif">
                            </asp:Image>.. Wait Loading.....
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Panel ID="PanRPSDetail" runat="server" Height="266px" Style="overflow-y: scroll"
                        Width="94%">
                        <p>
                            <asp:GridView ID="grdPromotion" runat="server" AutoGenerateColumns="False" BackColor="White"
                                BorderColor="White" CssClass="gridRow2" Font-Overline="False" Font-Size="8pt"
                                Font-Underline="False" ForeColor="SteelBlue" HorizontalAlign="Center" Width="100%">
                                <PagerSettings FirstPageText="" LastPageText="" Mode="NextPrevious" NextPageText="Next"
                                    PreviousPageText="Previous" />
                                <RowStyle ForeColor="Black" />
                                <Columns>
                                    <asp:BoundField DataField="BASKET NO" HeaderText="Basket No   ">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Is_Multiple" HeaderText="Is Mutiple">
                                        <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Multiple_of" HeaderText="Mutiple Of">
                                        <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SKU" HeaderText="SKU">
                                        <HeaderStyle HorizontalAlign="Center" Width="200px" />
                                        <ItemStyle HorizontalAlign="Center" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="UOM" HeaderText="UOM">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Basket On" HeaderText="Basket On">
                                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                        <ItemStyle HorizontalAlign="Center" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="From" HeaderText="From">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="To" HeaderText="To">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Discount" HeaderText="Discount">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SKU Offer" HeaderText="SKU Offer">
                                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                        <ItemStyle HorizontalAlign="Center" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SKU Quantity" HeaderText="SKU Quantity">
                                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                        <ItemStyle HorizontalAlign="Center" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                                    </asp:BoundField>
                                </Columns>
                                <FooterStyle BackColor="White" />
                                <PagerStyle BackColor="Transparent" />
                                <HeaderStyle BackColor="#007395" ForeColor="White" />
                                <AlternatingRowStyle CssClass="GridAlternateRowStyle" />
                            </asp:GridView>
                        </p>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
