<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmOpeningStock.aspx.cs" Inherits="Forms_frmOpeningStock" Title="CORN :: Stock Adjustment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
      
<script language="JavaScript" type="text/javascript">
    
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
    function BeginRequestHandler(sender, args) { var oControl = args.get_postBackElement(); oControl.disabled = true; }

        function ConfirmDelete() {
            if (confirm("Do you want to Cancel this record?") == true)
                return true;

            else {
                return false;
            }
        }

        function ValidateForm() {
            var str;

            str = document.getElementById('<%=txtskuName.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Must Select SKU Name');
                return false;
            }
            str = document.getElementById('<%=txtQuantity.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Must Enter Quantity');
                return false;
            }

            return true;
        }
    function SearchList() {
       
            var l = document.getElementById('<%= lstCode.ClientID %>');
        var tb = document.getElementById('<%= txtskuCode.ClientID %>');
      
            var str;
            if (tb.value == "") {
                ClearSelection(l);
            }
            else {
                for (var i = 0; i < l.options.length; i++) {
                   
                    if (l.options[i].value.toLowerCase().match(tb.value.toLowerCase())) {
                        l.options[i].selected = true;
                      
                        str = l.options[i].text;
                       
                       if (str.length > 0) {
               // document.getElementById("<%= txtskuCode.ClientID %>").value = str.substring(0, str.indexOf('-'));
                document.getElementById("<%= txtskuName.ClientID %>").value = str.substring(str.indexOf('-') + 1, str.indexOf('('));
                document.getElementById("<%= txtcolor.ClientID %>").value = str.substring(str.indexOf('(') + 1, str.indexOf(')'));
                document.getElementById("<%= txtsize.ClientID %>").value = str.substring(str.indexOf(')') + 1);
            //    document.getElementById("<%= Panel1.ClientID %>").className = "HidePanel";
            }
                        return false;
                    }
                    else {
                        ClearSelection(l);
                    }
                }
            }
    }
    function onlyNumbers(txt, event) {
        var charCode = (event.which) ? event.which : event.keyCode;
        if (charCode == 9 || charCode == 8) {
            return true;
        }
        if (charCode == 46) {
            return true;
        }
        if (charCode == 13)
        {
            document.getElementById("<%= txtskuCode.ClientID %>").focus();
        }
        if (charCode == 31 || charCode < 48 || charCode > 57)
            return false;

        return true;
    }
    function SearchedCode() {
       
            var l = document.getElementById('<%= lstCode.ClientID %>');
            var str;
            for (var i = 0; i < l.options.length; i++) {
                if (l.options[i].selected) {
                    str = l.options[i].value;
                    ClearSelection(l);
                    break;
                }
                else {
                    str = "";
                }
            }
            var stroption = document.getElementById("<%= txtskuCode.ClientID %>").value;
            if (str.length > 0) {
                document.getElementById("<%= txtskuCode.ClientID %>").value = str.substring(0, str.indexOf('-'));
                document.getElementById("<%= txtskuName.ClientID %>").value = str.substring(str.indexOf('-') + 1, str.indexOf('('));
                document.getElementById("<%= txtcolor.ClientID %>").value = str.substring(str.indexOf('(') + 1, str.indexOf(')'));
                document.getElementById("<%= txtsize.ClientID %>").value = str.substring(str.indexOf(')') + 1);
                document.getElementById("<%= Panel1.ClientID %>").className = "HidePanel";
            }
            else if (stroption.length == 0) {
            }

        }
    function SelectCode(e) {

        if (e.keyCode == 13)
        {
                var str = document.getElementById("<%= lstCode.ClientID %>").value;
                document.getElementById("<%= txtskuCode.ClientID %>").value = str.substring(0, str.indexOf('-'));
                document.getElementById("<%= txtskuName.ClientID %>").value = str.substring(str.indexOf('-') + 1);
                document.getElementById("<%= txtcolor.ClientID %>").value = str.substring(str.indexOf('(') + 1, str.indexOf(')'));
                document.getElementById("<%= txtsize.ClientID %>").value = str.substring(str.indexOf(')') + 1);
                document.getElementById("<%= Panel1.ClientID %>").className = "HidePanel";
                document.getElementById("<%= txtQuantity.ClientID %>").focus();

            }
        }
    function ClearSelection(lb) {
        lb.selectedIndex = -1;
    }
 
    </script>
    <div id="right_data">
        <div>
            <table width="100%">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tbody>
                                        <tr>
                                            <td align="left" colspan="2">
                                                <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                            </td>
                                            <td align="left" colspan="1">
                                                &nbsp; &nbsp; &nbsp; &nbsp;
                                            </td>
                                            <td style="width: 316px" valign="middle" align="center" colspan="1" rowspan="7">
                                                <strong>
                                                    <asp:Label ID="Label4" runat="server" Width="239px" Height="8px" CssClass="lblbox"></asp:Label></strong>
                                                &nbsp; &nbsp;&nbsp;
                                                <asp:Panel ID="Panel1" runat="server" Width="250px" Height="170px" CssClass="HidePanel"
                                                    BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver" BackColor="Silver">
                                                    <strong>
                                                        <asp:Label ID="Label3" runat="server" CssClass="lblbox" Width="170px">Select from SKU List</asp:Label></strong><br />
                                                    <asp:ListBox ID="lstCode" runat="server" Height="154px"  onkeyup="SelectCode(event)"
                                                        Width="245px"></asp:ListBox>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 25px" align="left">
                                                <strong>
                                                    <asp:Label ID="Label2" runat="server" Width="102px" Height="14px" Text="Transaction Type"
                                                        CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="height: 25px">
                                                <asp:DropDownList ID="DrpDocumentType" runat="server" Width="200px" CssClass="DropList"
                                                    AutoPostBack="True" OnSelectedIndexChanged="DrpDocumentType_SelectedIndexChanged">
                                                    <asp:ListItem Value="7">Opening Stock</asp:ListItem>
                                                    <asp:ListItem Value="8">Short</asp:ListItem>
                                                    <asp:ListItem Value="9">Excess</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td style="height: 25px">
                                                <strong>
                                                    <asp:Label ID="Label5" runat="server" Width="50px" CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 25px" align="left">
                                                <strong>
                                                    <asp:Label ID="lblDocumentNo" runat="server" Width="94px" Text="Document No" CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="height: 25px">
                                                <asp:DropDownList ID="drpDocumentNo" runat="server" Width="200px" CssClass="DropList"
                                                    AutoPostBack="True" OnSelectedIndexChanged="drpDocumentNo_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="height: 25px">
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td style="height: 25px" align="left">
                                                <strong>
                                                    <asp:Label ID="lblfromLocation" runat="server" Width="94px" Text="Location" CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="height: 25px">
                                                <asp:DropDownList ID="drpDistributor" runat="server" Width="200px" CssClass="DropList"
                                                    AutoPostBack="True" OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="height: 25px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 25px" align="left">
                                                <strong>
                                                    <asp:Label ID="Label1" runat="server" Width="94px" Text="Remarks" CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="height: 25px">
                                                <asp:TextBox ID="txtDocumentNo" runat="server" Width="195px" CssClass="txtBox"></asp:TextBox>
                                            </td>
                                            <td style="height: 25px">
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
                                <asp:HiddenField runat="server" ID="hfRowNo" />
                                <table>
                                    <tbody>
                                        <tr>
                                            <td class="lblDetail">
                                                 Item Code
                                            </td>
                                            <td class="lblDetail">
                                                Item Name
                                            </td>
                                             <td class="lblDetail" align="center">
                                               Color
                                            </td>
                                            <td class="lblDetail" align="center">
                                               Size
                                            </td>
                                            <td class="lblDetail">
                                                Quantity
                                            </td>
                                         
                                            
                                            <td style="width: 100px">
                                              
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtskuCode" onkeyup="SearchList()" runat="server" Width="145px" CssClass="txtBox"></asp:TextBox>
                                            </td>
                                            <td style="width: 100px">
                                                <asp:TextBox ID="txtskuName" runat="server" Width="200px" Font-Bold="True" CssClass="txtBox"
                                                    Enabled="False"></asp:TextBox>
                                            </td>
                                             <td>
                                                <asp:TextBox ID="txtcolor" runat="server" Width="76px" CssClass="txtBox" Enabled="False"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtsize" runat="server" Width="76px" CssClass="txtBox" Enabled="False"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtQuantity"  onkeypress="return onlyNumbers(this,event);" runat="server" Width="70px" Text="1"
                                                    CssClass="txtBox "></asp:TextBox>
                                            </td>
                                        
                                            
                                            <td style="width: 100px">
                                                <asp:Button AccessKey="A" ID="btnSave" OnClick="btnSave_Click" runat="server" Width="95px"
                                                    Font-Size="8pt" Text="Add Sku" ValidationGroup="vg" CssClass="Button" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="6">
                                                <asp:Panel ID="Panel2" runat="server" Width="710px" Height="140px" ScrollBars="Vertical"
                                                    BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver">
                                                    <asp:GridView ID="GrdPurchase" runat="server" Width="690px" ForeColor="SteelBlue"
                                                        CssClass="gridRow2" BorderColor="White" BackColor="White" ShowHeader="False"
                                                        OnRowDataBound="GrdPurchase_RowDataBound"
                                                        OnRowDeleting="GrdPurchase_RowDeleting"  OnRowEditing="GrdPurchase_RowEditing" HorizontalAlign="Center" AutoGenerateColumns="False">
                                                        <PagerSettings FirstPageText="" LastPageText="" Mode="NextPrevious" NextPageText="Next"
                                                            PreviousPageText="Previous" />
                                                        <RowStyle ForeColor="Black" />
                                                        <Columns>
                                                            <asp:BoundField DataField="SKU_ID" HeaderText="SKU_ID">
                                                                <HeaderStyle CssClass="HidePanel" />
                                                                <ItemStyle CssClass="HidePanel" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SKU_CODE" HeaderText="SKU Code">
                                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Left"
                                                                    Width="85px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SKU_NAME" HeaderText="SKU Name">
                                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Left"
                                                                    Width="205px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="FREE_SKU" HeaderText="Free SKU">
                                                                 <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                                <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="BATCH_NO" HeaderText="BatchNo">
                                                              <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                                <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                            </asp:BoundField>
                                                             <asp:BoundField DataField="PACKSIZE" HeaderText="PACKSIZE">
                                                               <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Right"
                                                                    Width="75px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="COLOR" HeaderText="COLOR">
                                                               <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Right"
                                                                    Width="75px" />
                                                            </asp:BoundField>
                                                             <asp:BoundField DataField="Quantity" HeaderText="Quantity">
                                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Right"
                                                                    Width="75px" />
                                                            </asp:BoundField>
                                                            <asp:CommandField HeaderText="Edit" ShowEditButton="True">
                                                                <ItemStyle BorderColor="Silver" BorderWidth="1px" Width="40px" />
                                                            </asp:CommandField>
                                                            <asp:TemplateField HeaderText="Delete">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" OnClientClick="javascript:return confirm('Are you sure you want to Delete?');return false;"
                                                                        Text="Delete"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" Width="45px" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <FooterStyle BackColor="White" />
                                                        <PagerStyle BackColor="Transparent" />
                                                        <HeaderStyle BackColor="#007395" Font-Bold="True" ForeColor="White" HorizontalAlign="Center"
                                                            VerticalAlign="Middle" />
                                                        <AlternatingRowStyle BackColor="#F2F2F2" CssClass="GridAlternateRowStyle" ForeColor="#333333" />
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <asp:Button AccessKey="S" ID="btnSaveDocument" OnClick="btnSaveDocument_Click" runat="server"
                                    Width="119px" Font-Size="8pt" Text="Save Document" UseSubmitBehavior="False"
                                    CssClass="Button" />
                                <asp:Button AccessKey="C" ID="btnCancel" OnClick="btnCancel_Click" runat="server"
                                    Width="120px" Font-Size="8pt" Text="Cancel" UseSubmitBehavior="False" CssClass="Button" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            &nbsp;
        </div>
    </div>
</asp:Content>