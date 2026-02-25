<%@ Page Title="CORN :: Pending Tax Authority Invoicess" Language="C#" MasterPageFile="~/Forms/PageMaster.master"
    AutoEventWireup="true" CodeFile="frmPendingTaxAuthority.aspx.cs" Inherits="Forms_frmPendingTaxAuthority" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="Server">
    <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
    <script language="JavaScript" type="text/javascript">
        function pageLoad() {
            $("select").searchable();
        }

        function SelectAllAccountHead() {
            var chkBoxList = document.getElementById('<%= gvPendingInvoices.ClientID %>');
            var chkBox = document.getElementById('<%= cbAll.ClientID %>');
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
            var chkBox = document.getElementById('<%= cbAll.ClientID %>');
            var chkBoxList = document.getElementById('<%= gvPendingInvoices.ClientID %>');
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
    </script>
    <div id="right_data">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="footer" style="height: 200px;">
                    <div class="main">
                        <div class="address">
                            <table>
                                <tr>
                                    <td>
                                        <b>Location:&nbsp;&nbsp;&nbsp;&nbsp; </b></td>
                                    <td><span>
                                        <asp:DropDownList CssClass="DropList" ID="drpDistributor" runat="server" Width="170px" OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </span></td>
                                </tr>
                            </table>
                            <p>
                                <b>From Date:&nbsp;&nbsp; </b><span>
                                    <asp:TextBox ID="txtstartDate" runat="server" Width="150px"></asp:TextBox>
                                    <asp:ImageButton ID="ibnstartDate" runat="server" Width="16px" ImageUrl="~/App_Themes/Granite/Images/date.gif"></asp:ImageButton>
                                </span>
                            </p>
                            <p>
                                <b>To Date: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</b><span>
                                    <asp:TextBox ID="txtEndDate" runat="server" Width="150px" CssClass="txtBox " MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton ID="ibnEndDate" runat="server" Width="16px" ImageUrl="~/App_Themes/Granite/Images/date.gif"></asp:ImageButton>
                                </span>
                            </p>
                            <p>
                                &nbsp;
                            </p>
                            <p>
                                <span></span>
                                <ajaxToolkit:CalendarExtender ID="CEStartDate" runat="server" TargetControlID="txtstartDate"
                                    PopupButtonID="ibnstartDate" Format="dd-MMM-yyyy" PopupPosition="TopLeft">
                                </ajaxToolkit:CalendarExtender>
                                <ajaxToolkit:CalendarExtender ID="CEEndDate" runat="server" TargetControlID="txtEndDate"
                                    PopupButtonID="ibnEndDate" Format="dd-MMM-yyyy" PopupPosition="TopLeft">
                                </ajaxToolkit:CalendarExtender>
                            </p>
                            <p>
                                <span style="padding-left: 70px;">
                                    <asp:Button runat="server" ID="btnGetDate" OnClick="btnGetDate_Click" CssClass="Button" Text="Get Data"></asp:Button>
                                </span>
                            </p>
                        </div>
                    </div>
                    <br />
                    <p>
                        <span>
                            <asp:CheckBox ID="cbAll" runat="server" Text="Select All" onclick="SelectAllAccountHead()" />
                        </span>
                    </p>

                    <br />
                    <asp:Panel ID="pnlGridView" runat="server" Height="300px" ScrollBars="Vertical"
                        Width="100%" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px">
                        <asp:GridView ID="gvPendingInvoices" runat="server" RowStyle-Height="22px"
                            BackColor="SteelBlue" HorizontalAlign="Center" AutoGenerateColumns="False" 
                            BorderColor="SteelBlue" Width="99.8%" OnRowDataBound="gvPendingInvoices_RowDataBound">
                            <HeaderStyle CssClass="tblhead" Height="25px" Font-Bold="true" />
                            <Columns>
                                <asp:TemplateField HeaderText="Select">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbInvoice" runat="server" onclick="UnCheckSelectAll();" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" Width="50px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="SALE_INVOICE_ID" HeaderText="Invoice Id">
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SALE_ID_DISTRIBUTOR_WISE" HeaderText="Invoice ID">
                                    <FooterStyle VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" BorderColor="Silver" BorderWidth="2px"
                                        BorderStyle="Solid" Font-Bold="true" Font-Size="12px" Width="60px"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DOCUMENT_DATE" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}">
                                    <FooterStyle VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" BorderColor="Silver" BorderWidth="2px"
                                        BorderStyle="Solid" Font-Bold="true" Font-Size="12px" Width="100px"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="CUSTOMER" HeaderText="Customer">
                                    <FooterStyle VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" BorderColor="Silver" BorderWidth="2px"
                                        BorderStyle="Solid" Font-Bold="true" Font-Size="12px" Width="100px"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="TOTAL_AMOUNT" HeaderText="Gross Sale" DataFormatString="{0:F2}">
                                    <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Left"
                                        VerticalAlign="Middle" Font-Bold="true" Font-Size="12px" Width="80px"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="EXTRA_DISCOUNT_AMOUNT" HeaderText="Discount" DataFormatString="{0:F2}">
                                    <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Left"
                                        VerticalAlign="Middle" Font-Bold="true" Font-Size="12px" Width="65px"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="GST_AMOUNT" HeaderText="Sales Tax" DataFormatString="{0:F2}">
                                    <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Left"
                                        VerticalAlign="Middle" Font-Bold="true" Font-Size="12px" Width="65px"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="TOTAL_NET_AMOUNT" HeaderText="Net Amount" DataFormatString="{0:F2}">
                                    <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Left"
                                        VerticalAlign="Middle" Font-Bold="true" Font-Size="12px" Width="80px"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="STANDARD_DISCOUNT_AMOUNT" HeaderText="Cash Received" DataFormatString="{0:F2}">
                                    <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Left"
                                        VerticalAlign="Middle" Font-Bold="true" Font-Size="12px" Width="80px"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="TIME_STAMP">
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                </asp:BoundField>
                            </Columns>
                            <RowStyle Height="25px" />
                        </asp:GridView>
                    </asp:Panel>
                    <br />
                    <p>
                        <span style="padding-left: 70px;">
                            <asp:Button runat="server" ID="btnSyncData" OnClick="btnSyncData_Click" CssClass="Button" Text="Sync Data"></asp:Button>
                        </span>
                    </p>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
