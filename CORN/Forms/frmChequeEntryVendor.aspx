<%@ Page Title="SAMS :: Principal Chq Entry" Language="C#" MasterPageFile="~/Forms/PageMaster.master"
    AutoEventWireup="true" CodeFile="frmChequeEntryVendor.aspx.cs" Inherits="Forms_frmChequeEntryVendor" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
<script language="JavaScript" type="text/javascript">
    function pageLoad() {
        $("select").searchable();
    }

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(startRequest);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);

        function startRequest(sender, e) {

            document.getElementById('<%=btnSave.ClientID%>').disabled = true;
            document.getElementById('<%=btnCancel.ClientID%>').disabled = true;

        }

        function endRequest(sender, e) {

            document.getElementById('<%=btnSave.ClientID%>').disabled = false;
            document.getElementById('<%=btnCancel.ClientID%>').disabled = false;

        }

        function ValidateForm() {
            var str;
            str = document.getElementById("<%= txtAmount.ClientID %>").value;
            if (str == null || str.length == 0) {
                alert('Must enter Amount');
                return false;
            }

        }
      
    </script>
    <div id="right_data">
        <div>
            <table width="100%">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline">
                            <ContentTemplate>
                                <div style="z-index: 101; left: 900px; width: 100px; position: absolute; top: 10px;
                                    height: 100px">
                                    <asp:Panel ID="Panel21" runat="server">
                                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                            <ProgressTemplate>
                                                <asp:ImageButton ID="ImageButton1" runat="server" Height="26px" ImageUrl="~/App_Themes/Granite/Images/image003.gif"
                                                    Width="23px" />
                                                Wait Update
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                    </asp:Panel>
                                </div>
                                <table>
                                    <tbody>
                                        <tr>
                                            <td align="left" colspan="2">
                                                <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                            </td>
                                            <td style="width: 104px" align="left" colspan="1">
                                            </td>
                                            <td align="left" colspan="1">
                                            </td>
                                            <td style="width: 55px" align="left" colspan="1">
                                            </td>
                                            <td align="left" colspan="1">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 17px" align="left">
                                                <strong>
                                                    <asp:Label ID="Label12" runat="server" Width="92px" Text="Payment Type" CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="width: 201px; height: 17px" align="left">
                                                <asp:DropDownList ID="DrpAccountType" runat="server" Width="226px" CssClass="DropList"
                                                    OnSelectedIndexChanged="DrpAccountType_SelectedIndexChanged" AutoPostBack="True">
                                                    <asp:ListItem Value="18">Cheque Payment</asp:ListItem>
                                                    <asp:ListItem Value="21">Cash Payment</asp:ListItem>
                                                    <asp:ListItem Value="33">Online Transfer</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                            </td>
                                            <td style="height: 17px" align="left">
                                            <strong>Vendor</strong> 
                                                  </td>
                                            <td style="width: 201px; height: 17px" align="left">
                                                <asp:DropDownList ID="drpVendor" runat="server" Width="240px" CssClass="DropList"
                                                    OnSelectedIndexChanged="drpVendor_SelectedIndexChanged" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="height: 17px" align="left">

                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 17px" align="left">
                                                <strong>
                                                    <asp:Label ID="lblfromLocation" runat="server" Width="94px" Text="Location" CssClass="lblbox"></asp:Label></strong>
                                            </td>

                                            <td style="width: 201px; height: 17px" align="left">
                                                <asp:DropDownList ID="drpDistributor" runat="server" Width="226px" CssClass="DropList"
                                                    OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                            
                                            <td style="width: 55px; height: 17px" align="left">
                                           
                                            </td>
                                           <td style="width: 201px; height: 17px" align="left" rowspan="9" colspan="2">
                                             <asp:Panel ID="Panel1" runat="server" Height="200px" ScrollBars="Vertical" BorderColor="Silver"
                                                    BorderStyle="Groove" BorderWidth="1px" Width="320px">
                                                    <asp:GridView ID="GrdCredit" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                        BorderColor="SteelBlue" ForeColor="SteelBlue" HorizontalAlign="Center" Width="100%"
                                                        DataKeyNames="PURCHASE_MASTER_ID">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="ChbIsAssigned" runat="server" Width="14px" />
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="MANUAL_INVOICE_ID">
                                                                <HeaderStyle CssClass="HidePanel" />
                                                                <ItemStyle CssClass="HidePanel" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="INVOICE_NO" HeaderText="INV No">
                                                                <ItemStyle CssClass="grdDetail" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DOCUMENT_DATE" HeaderText="INV Date">
                                                                <ItemStyle CssClass="grdDetail" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CURRENT_CREDIT_AMOUNT" HeaderText="Credit Amount" DataFormatString="{0:F2}">
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="MANUAL_DOCUMENT_NO" HeaderText="Type">
                                                                <ItemStyle CssClass="grdDetail" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                       <HeaderStyle BackColor="SteelBlue" Font-Bold="True" ForeColor="White" HorizontalAlign="Center"
                                                    VerticalAlign="Middle" CssClass="grdHead"/>
                                                        <AlternatingRowStyle BackColor="#F2F2F2" CssClass="GridAlternateRowStyle" ForeColor="#333333" />
                                                    </asp:GridView>                                                 
                                                </asp:Panel>
                                               <div style="float:right;vertical-align:bottom;">
                                                     <strong>
                                                         <asp:Label ID="lblTotalInvoiceAmount" runat="server" Text="Total Amount"></asp:Label>
                                                     </strong>
                                                     <asp:TextBox ID="txtInvoiceToalAmount" runat="server" Enabled="false" style="text-align: right"></asp:TextBox>
                                                 </div>
                                           </td>
                                            <td align="left" rowspan="11" style="font-size: 15px; vertical-align: top;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <strong>
                                                    <asp:Label ID="lblStatus" runat="server" Width="86px" Text="Status" CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="width: 201px" align="left">
                                                <asp:DropDownList ID="DrpStatus" runat="server" Width="226px" CssClass="DropList"
                                                    OnSelectedIndexChanged="DrpStatus_SelectedIndexChanged" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 55px" align="left">
                                            </td>
                                             <td style="width: 201px; height: 17px" align="left" rowspan="9" colspan="2">
                                                
                                            </td>
                                            <td align="left" rowspan="11" style="font-size: 15px; vertical-align: top;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <strong>
                                                    <asp:Label ID="Label14" runat="server" Width="98px" Text="Bank Account" CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="DrpBankAccount" runat="server" Width="226px" CssClass="DropList"
                                                    OnSelectedIndexChanged="DrpStatus_SelectedIndexChanged" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 55px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="left">
                                                <strong>
                                                    <asp:Label ID="Label6" runat="server" Width="74px" Text="Amount" CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="width: 201px" valign="top" align="left">
                                                <strong>
                                                    <asp:Label ID="lblChequeNo" runat="server" Width="76px" Text="Cheque No" CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="width: 55px" valign="top" align="left">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="left">
                                                <asp:TextBox ID="txtAmount" runat="server" Width="113px" CssClass="txtBox"></asp:TextBox>
                                            </td>
                                            <td style="width: 201px" valign="top" align="left">
                                                <asp:TextBox ID="txtChequeNo" runat="server" Width="132px" CssClass="txtBox"></asp:TextBox>
                                            </td>
                                            <td style="width: 55px" valign="top" align="left">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <strong>
                                                    <asp:Label ID="lblChequeDate" runat="server" Width="113px" Text="Cheque Date" CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td align="left">
                                            </td>
                                            <td style="width: 55px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:TextBox ID="txtStartDate" runat="server" Width="113px" CssClass="txtBox" Enabled="false"></asp:TextBox>
                                                
                                            </td>
                                            <td valign="top" align="left">
                                                <asp:TextBox ID="txtBankName" runat="server" Width="192px" CssClass="txtBox" Visible="false"></asp:TextBox>
                                                <asp:ImageButton ID="ibtnStartDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                                    Width="16px" />
                                            </td>
                                            <td style="width: 55px" valign="top" align="left">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="left">
                                                <strong>
                                                    <asp:Label ID="Label13" runat="server" Width="94px" Text="Remarks" CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="width: 55px" valign="top" align="left">
                                            </td>
                                            <td style="width: 55px" valign="top" align="left">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="left" colspan="2">
                                                <asp:TextBox ID="txtRemarks" runat="server" Width="343px" CssClass="txtBox"></asp:TextBox>
                                            </td>
                                            <td style="width: 55px" valign="top" align="left">
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td valign="top" align="left">
                                            </td>
                                            <td style="width: 201px" valign="top" align="left">
                                                <strong>
                                                    <asp:Label ID="Label1" Visible="false" runat="server" Width="100px" Text="Received Date"
                                                        CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="width: 55px" valign="top" align="left">
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td valign="top" align="left">
                                            </td>
                                            <td style="width: 201px" valign="top" align="left">
                                                <asp:TextBox ID="txtReceivedDate" runat="server" Width="128px" CssClass="txtBox "
                                                    ReadOnly="True" Visible="false"></asp:TextBox>
                                                    
                                            </td>
                                            <td style="width: 55px" valign="top" align="left">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="left">
                                                <asp:Button AccessKey="S" ID="btnSave" OnClick="btnSave_Click" runat="server" Width="100px"
                                                     Text="Save" CssClass="Button" />
                                            </td>
                                            <td style="width: 201px" valign="top" align="left">
                                                <asp:Button AccessKey="C" ID="btnCancel" runat="server" Width="100px"
                                                    Text="Cancel" OnClick="btnCancel_Click" CssClass="Button" />
                                            </td>
                                            <td colspan="3" style="text-align: right; font-size: medium; color: steelBlue; display: none;">
                                                <strong>Today's Paid Payment:
                                                    <asp:Label runat="server" ID="lblAmount"></asp:Label>
                                                </strong>
                                            </td>
                                            <td>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Custom"
                                                    ValidChars="0123456789." TargetControlID="txtAmount">
                                                </cc1:FilteredTextBoxExtender>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Custom"
                                                    ValidChars="0123456789" TargetControlID="txtChequeNo">
                                                </cc1:FilteredTextBoxExtender>
                                                <%-- <cc1:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="txtStartDate"
                                    Mask="99/99/9999" MaskType="Date">
                                </cc1:MaskedEditExtender>--%>
                                                <cc1:CalendarExtender ID="CEStartDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ibtnStartDate"
                                                    TargetControlID="txtStartDate">
                                                </cc1:CalendarExtender>
                                                <asp:HiddenField ID="HFChqueProcessId" runat="server"></asp:HiddenField>
                                               
                                            </td>
                                            <td>
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
        <div>
            <table width="100%">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <table style="border-right: silver thin inset; border-top: silver thin inset; border-left: silver thin inset;
                                    width: 650px; border-bottom: silver thin inset">
                                    <tbody>
                                        <tr>
                                            <td style="height: 20px" align="left" colspan="5">
                                                <asp:Panel ID="Panel12" runat="server" Width="774px" Height="200px" ScrollBars="Vertical">
                                                    <table style="border-right: silver thin inset; border-top: silver thin inset; border-left: silver thin inset;
                                                        border-bottom: silver thin inset; background-color: silver" width="100%">
                                                        <tbody>
                                                            <tr>
                                                                <td style="height: 21px" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label110" runat="server" Width="154px" Text="Select Searching Type"></asp:Label></strong>
                                                                </td>
                                                                <td style="width: 170px; height: 21px" align="left">
                                                                    <asp:DropDownList ID="ddSearchType" runat="server" Width="200px" CssClass="DropList">
                                                                        <asp:ListItem Value="CHEQUE_NO">All Records</asp:ListItem>
                                                                        <asp:ListItem Value="VENDOR_NAME">Principal</asp:ListItem>
                                                                        <asp:ListItem Value="account_name">Bank Account </asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="width: 224px; height: 21px" align="left">
                                                                    <asp:TextBox ID="txtSeach" runat="server" Width="200px" CssClass="txtBox "></asp:TextBox>
                                                                </td>
                                                                <td style="width: 250px; height: 21px" align="left">
                                                                    <asp:Button ID="btnFilter" runat="server" Width="85px" Font-Size="8pt" Text="Filter"
                                                                        OnClick="btnFilter_Click"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                    <asp:GridView ID="GrdCheque" runat="server" Width="100%" ForeColor="SteelBlue" BorderColor="SteelBlue"
                                                        HorizontalAlign="Center" BackColor="White" AutoGenerateColumns="False" OnRowEditing="GrdCheque_RowEditing"
                                                        OnRowDeleting="GrdCheque_RowDeleting">
                                                        <Columns>
                                                            <asp:BoundField DataField="CHEQUE_PROCESS_ID" HeaderText="CHEQUE_PROCESS_ID">
                                                                <HeaderStyle CssClass="HidePanel" />
                                                                <ItemStyle CssClass="HidePanel" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="VENDOR_ID" HeaderText="VENDOR_ID">
                                                                <HeaderStyle CssClass="HidePanel" />
                                                                <ItemStyle CssClass="HidePanel" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="VENDOR_NAME" HeaderText="Vendor">
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Left" Width="15%" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CHEQUE_NO" HeaderText="Cheque No">
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Left" Width="10%"/>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CHEQUE_DATE" HeaderText="Chq.Date" DataFormatString="{0:dd/MM/yyyy}">
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Center" Width="10%"/>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RECEIVED_DATE" HeaderText="Paid Date" DataFormatString="{0:dd/MM/yyyy}">
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Center" Width="10%"/>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CHEQUE_AMOUNT" DataFormatString="{0:F2}" HeaderText="Amount">
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Right" Width="10%"/>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Remarks" HeaderText="Remarks">
                                                                <HeaderStyle CssClass="HidePanel" />
                                                                <ItemStyle CssClass="HidePanel" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="account_name" HeaderText="Bank Account">
                                                                
                                                                <ItemStyle CssClass="grdDetail" Width="20%"/>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="account_head_id" HeaderText="account_head_id">
                                                                <HeaderStyle CssClass="HidePanel" />
                                                                <ItemStyle CssClass="HidePanel" />
                                                            </asp:BoundField>
                                                            
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="btnEdit" CommandName="Edit" Text="Edit" />
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Center" Width="5%"/>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle BackColor="SteelBlue" Font-Bold="True" ForeColor="White" HorizontalAlign="Center"
                                                    VerticalAlign="Middle" CssClass="grdHead"/>
                                                        <AlternatingRowStyle BackColor="#F2F2F2" CssClass="GridAlternateRowStyle" ForeColor="#333333" />
                                                    </asp:GridView>
                                                    <asp:GridView ID="GrdCO" runat="server" Width="100%" ForeColor="SteelBlue" CssClass="gridRow2"
                                                        BorderColor="SteelBlue" HorizontalAlign="Center" BackColor="White" AutoGenerateColumns="False"
                                                        Visible="false" OnRowDeleting="GrdCO_RowDeleting">
                                                        <Columns>
                                                            <asp:BoundField DataField="VENDOR_ID" HeaderText="VENDOR_ID">
                                                                <HeaderStyle CssClass="HidePanel" />
                                                                <ItemStyle CssClass="HidePanel" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="VENDOR_NAME" HeaderText="Vendor">
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Left" Width="15%"/>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CHEQUE_NO" HeaderText="Inv.No">
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Left" Width="10%"/>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CHEQUE_DATE" HeaderText="Transfer Date" DataFormatString="{0:dd/MM/yyyy}">
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Center" Width="10%"/>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RECEIVED_DATE" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}">
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Center" Width="10%"/>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CHEQUE_AMOUNT" DataFormatString="{0:F2}" HeaderText="Amount">
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Right" Width="10%"/>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Remarks" HeaderText="Remarks">
                                                                <HeaderStyle CssClass="HidePanel" />
                                                                <ItemStyle CssClass="HidePanel" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="account_name" HeaderText="Bank Account">
                                                                <ControlStyle CssClass="grdDetail" />
                                                                <ItemStyle CssClass="grdDetail" Width="20%"/>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="account_head_id" HeaderText="account_head_id">
                                                                <HeaderStyle CssClass="HidePanel" />
                                                                <ItemStyle CssClass="HidePanel" />
                                                            </asp:BoundField>
                                                             <asp:BoundField DataField="Manual_Document_no" >
                                                                <HeaderStyle CssClass="HidePanel" />
                                                                <ItemStyle CssClass="HidePanel" />
                                                            </asp:BoundField>
                                                              <asp:BoundField DataField="Voucher_no" >
                                                                <HeaderStyle CssClass="HidePanel" />
                                                                <ItemStyle CssClass="HidePanel" />
                                                                
                                                            </asp:BoundField>
                                                             <asp:BoundField DataField="voucher_type_id" >
                                                                <HeaderStyle CssClass="HidePanel" />
                                                                <ItemStyle CssClass="HidePanel" />
                                                                
                                                            </asp:BoundField>
                                                              <asp:BoundField DataField="Document_no" >
                                                                <HeaderStyle CssClass="HidePanel" />
                                                                <ItemStyle CssClass="HidePanel" />
                                                                
                                                            </asp:BoundField>
                                                            
                                                             <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="btnDelete" CommandName="Delete" Text="Delete" />
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="grdDetail" HorizontalAlign="Center" Width="5%"/>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle BackColor="SteelBlue" CssClass="grdHead" Font-Bold="True" ForeColor="White" HorizontalAlign="Center"
                                                    VerticalAlign="Middle" />
                                                        <AlternatingRowStyle BackColor="#F2F2F2" CssClass="GridAlternateRowStyle" ForeColor="#333333" />
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table width="90%">
                                    <tr>
                                        <td style="text-align: center;">
                                            <strong>Total Amount: </strong>
                                            <asp:Label ID="lblTotalAmount" runat="server" Text="0" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
