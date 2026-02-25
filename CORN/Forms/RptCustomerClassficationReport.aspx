<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" CodeFile="RptCustomerClassficationReport.aspx.cs"
    Inherits="Forms_RptCustomerClassficationReport" Title="CORN :: Customer Classification Report" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="content1" runat="server" ContentPlaceHolderID="cphPage">
            <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
<script language="JavaScript" type="text/javascript">
    function pageLoad() {
        $("select").searchable();
    }</script>
    <div id="right_data">
        <table width="100%">
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table>
                                <tbody>
                                    <tr>
                                        <td align="left" colspan="4">
                                            <strong>
                                                <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label></strong> </td>
                                        <td style="width: 1px" align="left" colspan="1"></td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 1px; height: 1px"></td>
                                        <td align="left" style="width: 19px; height: 1px">
                                            <strong>
                                                <asp:Label ID="Label5" runat="server" CssClass="lblbox" Text="Customer Classification"
                                                    Width="95px"></asp:Label></strong></td>
                                        <td align="left" style="height: 1px"></td>
                                        <td align="left" style="width: 203px; height: 1px">
                                            <asp:RadioButtonList ID="RBReportType" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Selected="True">Summary Report</asp:ListItem>
                                                <asp:ListItem>Detail Report</asp:ListItem>
                                            </asp:RadioButtonList></td>
                                        <td align="left" style="width: 1px; height: 1px"></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 1px; height: 1px" align="left"></td>
                                        <td style="width: 19px; height: 1px" align="left">
                                            <strong>
                                                <asp:Label ID="Label2" runat="server" Width="48px" Text="Location" CssClass="lblbox"></asp:Label></strong></td>
                                        <td style="height: 1px" align="left"></td>
                                        <td style="width: 203px; height: 1px" align="left">
                                            <asp:DropDownList ID="DrpLocation" runat="server" Width="200px" CssClass="DropList" AutoPostBack="True"></asp:DropDownList></td>
                                        <td style="width: 1px; height: 1px" align="left"></td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 1px"></td>
                                        <td align="left" style="width: 19px">
                                            <strong>
                                                <asp:Label ID="lblNickName" runat="server" CssClass="lblbox" Text="Channel Type"
                                                    Width="79px"></asp:Label></strong></td>
                                        <td align="left"></td>
                                        <td align="left" style="width: 203px; height: 25px">
                                            <asp:DropDownList ID="drpChannelType" runat="server" CssClass="DropList" Width="200px">
                                            </asp:DropDownList></td>
                                        <td align="left" style="width: 1px; height: 25px"></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 1px" align="left"></td>
                                        <td style="width: 19px" align="left">
                                            <strong>
                                                <asp:Label ID="lbltoLocation" runat="server" Width="88px" Text="SKU Principal" CssClass="lblbox"></asp:Label></strong></td>
                                        <td align="left"></td>
                                        <td style="width: 203px; height: 25px" align="left">
                                            <asp:DropDownList ID="DrpPrincipal" runat="server" Width="200px" CssClass="DropList" AutoPostBack="True" OnSelectedIndexChanged="DrpPrincipal_SelectedIndexChanged">
                                            </asp:DropDownList></td>
                                        <td style="width: 1px; height: 25px" align="left"></td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 1px"></td>
                                        <td align="left" style="width: 19px">
                                            <strong>
                                                <asp:Label ID="Label4" runat="server" CssClass="lblbox" Text="SKU Division" Width="73px"></asp:Label></strong></td>
                                        <td align="left"></td>
                                        <td align="left" style="width: 203px; height: 25px">
                                            <asp:DropDownList ID="ddskuDivision" runat="server" AutoPostBack="True" CssClass="DropList"
                                                OnSelectedIndexChanged="ddskuDivision_SelectedIndexChanged" Width="200px">
                                            </asp:DropDownList></td>
                                        <td align="left" style="width: 1px; height: 25px"></td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 1px"></td>
                                        <td align="left" style="width: 19px">
                                            <strong>
                                                <asp:Label ID="Label7" runat="server" CssClass="lblbox" Text="SKU Category" Width="101px"></asp:Label></strong></td>
                                        <td align="left"></td>
                                        <td align="left" style="width: 203px; height: 25px">
                                            <asp:DropDownList ID="ddskuCategory" runat="server" AutoPostBack="True" CssClass="DropList"
                                                OnSelectedIndexChanged="ddskuCategory_SelectedIndexChanged" Width="200px">
                                            </asp:DropDownList></td>
                                        <td align="left" style="width: 1px; height: 25px"></td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 1px"></td>
                                        <td align="left" style="width: 19px">
                                            <strong>
                                                <asp:Label ID="Label9" runat="server" CssClass="lblbox" Text="SKU Brand" Width="101px"></asp:Label></strong></td>
                                        <td align="left"></td>
                                        <td align="left" style="width: 203px; height: 25px">
                                            <asp:DropDownList ID="ddskuBrand" runat="server" AutoPostBack="True" CssClass="DropList"
                                                OnSelectedIndexChanged="ddskuBrand_SelectedIndexChanged" Width="200px">
                                            </asp:DropDownList></td>
                                        <td align="left" style="width: 1px; height: 25px"></td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 1px"></td>
                                        <td align="left" style="width: 19px">
                                            <strong>
                                                <asp:Label ID="Label6" runat="server" Text="SKU Name" Width="100px"></asp:Label></strong></td>
                                        <td align="left"></td>
                                        <td align="left" style="width: 203px; height: 25px">
                                            <asp:DropDownList ID="ddskuName" runat="server" CssClass="DropList"
                                                Width="200px">
                                            </asp:DropDownList></td>
                                        <td align="left" style="width: 1px; height: 25px"></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 1px; height: 25px" align="left"></td>
                                        <td style="width: 19px; height: 25px" align="left">
                                            <strong>
                                                <asp:Label ID="Label1" runat="server" Width="59px" Text="From Date" CssClass="lblbox"></asp:Label></strong></td>
                                        <td style="height: 25px" align="left"></td>
                                        <td style="width: 203px; height: 25px" align="left">
                                            <asp:TextBox ID="txtFromDate" runat="server" Width="153px" CssClass="txtBox" MaxLength="10"></asp:TextBox>
                                            <asp:ImageButton ID="ImgBntFromCalc" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"></asp:ImageButton></td>
                                        <td style="width: 1px; height: 25px" align="left"></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 1px; height: 25px" align="left"></td>
                                        <td style="width: 19px; height: 25px" align="left">
                                            <strong>
                                                <asp:Label ID="Label3" runat="server" Width="57px" Text="To  Date" CssClass="lblbox"></asp:Label></strong></td>
                                        <td style="height: 25px" align="left"></td>
                                        <td style="width: 203px; height: 25px" align="left">
                                            <asp:TextBox ID="txtToDate" runat="server" Width="153px" CssClass="txtBox" MaxLength="10"></asp:TextBox>
                                            <asp:ImageButton ID="ImgToDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"></asp:ImageButton></td>
                                        <td style="width: 1px; height: 25px" align="left"></td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 1px; height: 25px"></td>
                                        <td align="left" style="width: 19px; height: 25px">
                                            <strong>
                                                <asp:Label ID="lblSaleForce" runat="server" Width="91px" Text="Report Type" CssClass="lblbox"></asp:Label></strong></td>
                                        <td align="left" style="height: 25px"></td>
                                        <td align="left" style="width: 203px; height: 25px">
                                            <asp:RadioButtonList ID="RbList" runat="server" RepeatDirection="Horizontal" Width="127px">
                                                <asp:ListItem Selected="True">Carton</asp:ListItem>
                                                <asp:ListItem>Value</asp:ListItem>
                                            </asp:RadioButtonList></td>
                                        <td align="left" style="width: 1px; height: 25px"></td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 1px; height: 25px"></td>
                                        <td align="left" colspan="3" style="height: 25px">
                                            <asp:GridView ID="GrdCustomerRange" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                BorderColor="White" CssClass="gridRow2" ForeColor="SteelBlue" HorizontalAlign="Center"
                                                PageSize="8" Width="400px">
                                                <PagerSettings FirstPageText="" LastPageText="" Mode="NextPrevious" NextPageText="Next"
                                                    PreviousPageText="Previous" />
                                                <RowStyle ForeColor="Black" />
                                                <Columns>
                                                    <asp:BoundField DataField="SerialNo" HeaderText="Serial No">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="From Range">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtFromRange" runat="server" CssClass="txtBox " MaxLength="12" Width="100%"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="To Range">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txttoRange" runat="server" CssClass="txtBox " MaxLength="12" Width="100%"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle BackColor="White" />
                                                <PagerStyle BackColor="Transparent" />
                                                <HeaderStyle BackColor="#007395" Font-Bold="True" ForeColor="White" HorizontalAlign="Center"
                                                    VerticalAlign="Middle" />
                                                <AlternatingRowStyle BackColor="#F2F2F2" CssClass="GridAlternateRowStyle" ForeColor="#333333" />
                                            </asp:GridView>
                                        </td>
                                        <td align="left" style="width: 1px; height: 25px"></td>
                                    </tr>
                                </tbody>
                            </table>
                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate" PopupButtonID="ImgBntFromCalc" Format="dd-MMM-yyyy" EnableViewState="False">
                            </cc1:CalendarExtender>
                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate" PopupButtonID="ImgToDate" Format="dd-MMM-yyyy" EnableViewState="False">
                            </cc1:CalendarExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Button ID="btnViewPDF" runat="server" CssClass="Button"
                        Text="View PDF" Width="90" OnClick="btnViewPDF_Click" />
                    <asp:Button ID="btnViewExcel" runat="server" CssClass="Button" Text="View Excel"
                        Width="90" OnClick="btnViewExcel_Click" /></td>
            </tr>
        </table>

    </div>
</asp:Content>
