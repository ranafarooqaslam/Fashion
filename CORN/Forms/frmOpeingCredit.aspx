<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" CodeFile="frmOpeingCredit.aspx.cs"
    Inherits="Forms_frmOpeingCredit" Title="CORN :: Opening Credit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
       <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
<script language="JavaScript" type="text/javascript">
    function pageLoad() {
        $("select").searchable();
    }

        function ValidateForm() {
            var str;

            str = document.getElementById("<%= txtAmount.ClientID %>").value;
            if (str == null || str.length == 0) {
                alert('Must Enter Amount');
                return false;
            }
            str = document.getElementById("<%= txtFromdate.ClientID %>").value;
            if (str == null || str.length <= 1) {
                alert('Must Enter Invoice Date');
                return false;
            }
            str = document.getElementById("<%= txtInvoiceNo.ClientID %>").value;
            if (str == null || str.length <= 1) {
                alert('Must Enter Invoice No');
                return false;
            }
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

    </script>
    <div id="right_data">
        <div style="z-index: 101; left: 503px; width: 100px; position: absolute; top: 251px; height: 100px">
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
                                            <td style="width: 50px" align="left" colspan="1"></td>
                                            <td align="left" colspan="1"></td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 90px;">
                                                <strong>Opening Type
                                                </strong>
                                            </td>
                                            <td align="left">
                                                <asp:RadioButtonList ID="DrpCreditType" runat="server" Width="200px" RepeatDirection="Horizontal"
                                                    OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Value="25" Selected="True">Debit</asp:ListItem>
                                                    <asp:ListItem Value="26">Credit</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="width: 50px" align="left"></td>

                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <strong>Location
                                                </strong>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="drpDistributor" runat="server" AutoPostBack="True" CssClass="DropList"
                                                    OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged" Width="200px">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 50px" align="left"></td>

                                        </tr>
                                       
                                        <tr>
                                            <td align="left">
                                                <strong>Customer</strong>
                                            </td>
                                            <td valign="bottom" align="left">
                                                <asp:DropDownList ID="ddlCustomer" runat="server" Width="200px">
                                                </asp:DropDownList>

                                            </td>
                                            <td>

                                                <asp:HiddenField ID="hfLegendID" runat="server" Visible="False"
                                                    Value="-1"></asp:HiddenField>
                                                <asp:HiddenField ID="hfCustomerID" runat="server" Value="-1"></asp:HiddenField>
                                                <asp:HiddenField ID="hfSaleInvoiceID" runat="server" Value="-1"></asp:HiddenField>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 21px" align="left">
                                                <strong>Bill Date
                                                </strong>
                                            </td>
                                            <td style="height: 21px">
                                                <asp:TextBox ID="txtFromdate" runat="server" MaxLength="1"
                                                    Width="176px"></asp:TextBox>
                                                 <asp:ImageButton ID="ImgBntFromCalc" runat="server" CausesValidation="False" ImageUrl="~/App_Themes/Granite/Images/date.gif" />
                                           
                                                <ajaxToolkit:CalendarExtender ID="txtFromdate_CalendarExtender" runat="server" Format="dd-MMM-yyyy"
                                                    PopupButtonID="ImgBntFromCalc" TargetControlID="txtFromdate">

                                                </ajaxToolkit:CalendarExtender>
                                            </td>
                                            <td style="height: 21px" align="left">
                                                </td>
                                        </tr>

                                        <tr>
                                            <td align="left">
                                                <strong>Invoice No</strong>
                                            </td>
                                            <td style="width: 201px" valign="top" align="left">
                                                <asp:TextBox ID="txtInvoiceNo" runat="server" Width="192px"
                                                    CssClass="uppercase" MaxLength="10"></asp:TextBox>

                                            </td>
                                            <td valign="top" align="left"></td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="left">
                                                <strong>Amount</strong>
                                            </td>
                                            <td valign="top" align="left">
                                                <asp:TextBox ID="txtAmount" runat="server" Width="192px" onkeypress="return onlyDotsAndNumbers(this,event);"></asp:TextBox>
                                            </td>
                                            <td valign="top" align="left"></td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" align="left">
                                                <strong>Remarks</strong>
                                            </td>
                                            <td valign="top" align="left" colspan="2">
                                                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Columns="53"></asp:TextBox></td>
                                            <td valign="top" align="left"></td>
                                        </tr>

                                        <tr>
                                            <td style="height: 36px" align="left"></td>
                                            <td style="width: 201px; height: 36px" valign="middle" align="left">
                                                <asp:Button AccessKey="S" ID="btnSave" OnClick="btnSave_Click" runat="server" Width="90px"
                                                    Font-Size="8pt" Text="Save" CssClass="Button" />&nbsp;
                                                <asp:Button AccessKey="C" ID="btnCancel" OnClick="btnCancel_Click" runat="server"
                                                    Width="90px" Font-Size="8pt" Text="Cancel" CssClass="Button" />
                                            </td>
                                            <td style="width: 201px; height: 36px" valign="middle" align="left"></td>
                                            <td style="width: 201px; height: 36px" valign="middle" align="left"></td>
                                        </tr>
                                    </tbody>
                                </table>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                    FilterType="Custom" ValidChars="0123456789." TargetControlID="txtAmount">
                                </ajaxToolkit:FilteredTextBoxExtender>

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
                                                <asp:BoundField DataField="CUSTOMER_ID" HeaderText="CUSTOMER_ID">
                                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                </asp:BoundField>

                                                <asp:BoundField DataField="CUSTOMER_NAME" HeaderText="Customer">
                                                    <ItemStyle CssClass="grdDetail" Width="30%"></ItemStyle>
                                                    <HeaderStyle CssClass="grdHead"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Invoice Date">

                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# Bind("DOCUMENT_DATE", "{0:dd-MMM-yyyy}") %>' ID="Label1"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="grdDetail" Width="15%"></ItemStyle>
                                                    <HeaderStyle CssClass="grdHead"></HeaderStyle>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="SALE_INVOICE_ID" HeaderText="Invoice No">
                                                    <ItemStyle CssClass="grdDetail" Width="10%"></ItemStyle>
                                                    <HeaderStyle CssClass="grdHead"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CURRENT_CREDIT_AMOUNT" HeaderText="Amount" DataFormatString="{0:f2}">
                                                    <ItemStyle CssClass="grdDetail" Width="10%" HorizontalAlign="Right"></ItemStyle>
                                                    <HeaderStyle CssClass="grdHead"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="LEGEND_ID" HeaderText="LEGEND_ID">
                                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                </asp:BoundField>

                                                <asp:BoundField DataField="DOCUMENT_DATE" HeaderText="DOCUMENT_DATE">
                                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="SALE_INVOICE_MASTER_ID" HeaderText="SALE_INVOICE_MASTER_ID">
                                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="REMARKS">
                                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
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





