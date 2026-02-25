<%@ Page Title="CORN :: Re-Print Invoice" Language="C#" MasterPageFile="~/Forms/PageMaster.master"
    AutoEventWireup="true" CodeFile="frmRePrint.aspx.cs" Inherits="Forms_frmRePrint" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="Server">
    <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
    <script language="JavaScript" type="text/javascript">
        function pageLoad() {
            $("select").searchable();
        }
        function onlyNumbers(txt, event) {
            var charCode = (event.which) ? event.which : event.keyCode;

            if (charCode == 9 || charCode == 8) {
                return true;
            }
            if (charCode == 31 || charCode < 48 || charCode > 57)
                return false;
            return true;

        }
    </script>
    <div id="right_data">

        <div class="footer" style="height: 200px;">
            <div class="main">
                <div class="address">
                   <table><tr><td>
                        <b>Location:&nbsp;&nbsp;&nbsp;&nbsp; </b></td><td><span>

                            <asp:DropDownList CssClass="DropList" ID="drpDistributor" runat="server" Width="170px" OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </span></td></tr></table>
                  
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
                        <b>Invoice Id:&nbsp;&nbsp; </b><span>
                            <asp:TextBox ID="txtInvoiceNo" runat="server" Width="150px" onkeypress="return onlyNumbers(this,event);"></asp:TextBox>
                        </span>
                    </p>
                    <p>
                        <span>
                            <asp:Button runat="server" ID="btnRePrint" OnClick="btnRePrint_Click" CssClass="Button" Text="Filter"></asp:Button>
                        </span>
                    </p>
                </div>
            </div>
            <br />
            <asp:Panel ID="PnlPrintInvoice2" runat="server" Height="320px" ScrollBars="Vertical"
                Width="100%" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px">
                <asp:GridView ID="GrdPrintInvoice" runat="server" RowStyle-Height="22px"
                    BackColor="SteelBlue" HorizontalAlign="Center" AutoGenerateColumns="False" BorderColor="SteelBlue"
                    Width="99.8%"
                    OnRowDeleting="GrdPrintInvoice_RowDeleting" OnRowDataBound="GrdPrintInvoice_RowDataBound">

                    <HeaderStyle CssClass="tblhead" Height="25px" Font-Bold="true" />
                    <Columns>
                        <asp:BoundField DataField="SALE_INVOICE_ID" HeaderText="Invoice Id">
                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                            <HeaderStyle CssClass="HidePanel" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SALE_ID_DISTRIBUTOR_WISE" HeaderText="Invoice Id">
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
                        <asp:BoundField DataField="TYPE" HeaderText="Invoice Type" DataFormatString="{0:F2}">
                            <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Left"
                                VerticalAlign="Middle" Font-Bold="true" ForeColor="brown" Font-Size="12px" Width="80px"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="InvoiceNumberFBR" HeaderText="InvoiceNumberFBR">
                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                            <HeaderStyle CssClass="HidePanel" />
                        </asp:BoundField>
                        <asp:TemplateField>
                            <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Center"
                                VerticalAlign="Middle" Font-Size="12px" Width="20px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtn_print" runat="server" Text="Print" ForeColor="Blue" CommandName="Delete"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle Height="25px" />
                </asp:GridView>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
