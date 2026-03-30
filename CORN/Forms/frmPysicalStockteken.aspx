<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmPysicalStockteken.aspx.cs" Inherits="Forms_frmPysicalStockteken"
    Title="CORN :: Physical Stock Taking" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
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
        if (charCode == 46) {
            return true;
        }
        if (charCode == 31 || charCode < 48 || charCode > 57)
            return false;

        return true;
    }

        function ValidateForm() {
            var str;

            str = document.getElementById('<%=txtskuName.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Must Select Item Name');
                return false;
            }
            str = document.getElementById('<%=txtQuantity.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Must Enter Quantity');
                return false;
            }


            return true;
        }
        function SearcSKUList() {
            var l = document.getElementById('<%= lstCode.ClientID %>');
            var tb = document.getElementById('<%= txtskuCode.ClientID %>');

            if (tb.value == "") {
                ClearSelection(l);
            }
            else {
                for (var i = 0; i < l.options.length; i++) {
                    if (l.options[i].value.toLowerCase().match(tb.value.toLowerCase())) {
                        l.options[i].selected = true;
                        return false;
                    }
                    else {
                        ClearSelection(l);
                    }
                }
            }
        }

       

        function SearchSKUCode() {
            var str = document.getElementById("<%= lstCode.ClientID %>").value;
            var stroption = document.getElementById("<%= txtskuCode.ClientID %>").value;

            if (str.length > 0) {
                document.getElementById("<%= txtskuCode.ClientID %>").value = str.substring(0, str.indexOf('-'));
                document.getElementById("<%= txtskuName.ClientID %>").value = str.substring(str.indexOf('-') + 1, str.indexOf(':'));
                document.getElementById("<%= txtUnitRate.ClientID %>").value = str.substring(str.indexOf(':') + 1,str.indexOf('['));
                document.getElementById("<%= Panel3.ClientID %>").className = "HidePanel";

            }
            else if (stroption.length == 0) {
                document.getElementById("<%= Panel3.ClientID %>").className = "ShowPanel";
                document.getElementById("<%= lstCode.ClientID %>").focus();
            }
            ClearSelection(document.getElementById('<%= lstCode.ClientID %>'));

        }
        function SelectSkuCode(e) {
          var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
            if (key == 13)
                e.preventDefault();
                {
                
                var str = document.getElementById("<%= lstCode.ClientID %>").value;
                document.getElementById("<%= txtskuCode.ClientID %>").value = str.substring(0, str.indexOf('-'));
                document.getElementById("<%= txtskuName.ClientID %>").value = str.substring(str.indexOf('-') + 1);
                document.getElementById("<%= txtUnitRate.ClientID %>").value = str.substring(str.indexOf(':') + 1, str.indexOf('['));
                document.getElementById("<%= Panel3.ClientID %>").className = "HidePanel";

                document.getElementById("<%= txtUnitRate.ClientID %>").focus();
            }
        }

        function ClearSelection(lb) {
            lb.selectedIndex = -1;
        }

        function HandleKeyPress(event) {
            debugger;
            if (event.keyCode === 13) { // Check if Enter key is pressed (key code 13)
                if (document.getElementById('<%= chkScan.ClientID %>').checked == true) {
                    var autoCompleteDropdownVisible = $find('<%= AutoComplete.ClientID %>').get_element().style.display !== 'none';
                    if (!autoCompleteDropdownVisible) {
                        event.preventDefault(); // Prevent the default form submission
                        document.getElementById('<%= btnAddRecord.ClientID %>').click(); // Trigger the click event of the hidden button
                    }
                }
                else {
                    document.getElementById('<%= btnAddRecord.ClientID %>').click();
                }
            }
        }
    function HandleKeyPressQty(event) {
            debugger;
            if (event.keyCode === 13) { // Check if Enter key is pressed (key code 13)
                document.getElementById('<%= btnAddRecord.ClientID %>').click();
            }
        }

        function ProductSelected(source, eventArgs) {
        var skuDetail = eventArgs.get_text();
        var num = eventArgs.get_value();

        document.getElementById("<%=txtskuCode.ClientID %>").value = skuDetail.substring(0, skuDetail.indexOf('-'));

        $.ajax(
        {
            type: "POST", //HTTP method
            url: "frmPysicalStockteken.aspx/GetSKUDetail", //page/method name
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ itemID: Math.round(eventArgs.get_value()) }),
            dataType: "json",
            success: LoadSKUDetail
        }
    );
    }

    function LoadSKUDetail(data) {
                data = eval(data.d);
                if (data.length > 0) {
                    document.getElementById("<%=txtcolor.ClientID%>").value = data[0].COLOR;
                    document.getElementById("<%=txtsize.ClientID%>").value = data[0].PACKSIZE;
                    document.getElementById("<%=txtUnitRate.ClientID%>").value = parseFloat(data[0].DISTRIBUTOR_PRICE);
                }
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
                                            <td style="height: 25px" align="left">
                                                <strong>
                                                    <asp:Label Width="74px" CssClass="lblbox" ID="lblDocumentNo" runat="server" Text="Location"></asp:Label></strong>
                                            </td>
                                            <td style="height: 25px">
                                                <asp:DropDownList CssClass="DropList" ID="drpDistributor" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged" Width="200px">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="center" style="width: 316px" colspan="1" rowspan="2" valign="middle">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 25px" align="left">
                                                <strong>
                                                    <asp:Label Width="74px" CssClass="lblbox" ID="Label2" runat="server" Text="Document No"></asp:Label></strong>
                                            </td>
                                            <td style="height: 25px">
                                                <asp:DropDownList CssClass="DropList" ID="drpDocumentNo" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpDocumentNo_SelectedIndexChanged" Width="200px">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="center" style="width: 316px" colspan="1" rowspan="2" valign="middle">
                                            </td>
                                        </tr>
                                         <tr>
                                        <td align="left">
                                            <strong>
                                                <asp:Label ID="Label3" runat="server" Height="13px" Text=" Date" Width="70px"></asp:Label></strong>
                                        </td>
                                        <td align="left" style="height: 25px">
                                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="txtBox" MaxLength="10"
                                                onkeyup="BlockStartDateKeyPress()" Width="160px"></asp:TextBox>
                                            <asp:ImageButton ID="ibtnStartDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                                Width="16px" />
                                        </td>
                                              <td align="center" style="width: 316px" colspan="1" rowspan="2" valign="middle">
                                            </td>
                                             <cc1:CalendarExtender ID="CEStartDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnStartDate"
                                        TargetControlID="txtStartDate">
                                    </cc1:CalendarExtender>
                                    </tr>

                                        <asp:HiddenField ID="hfMaxDOCID" runat="server" />
                                       
                                        <tr style="display:none">
                                            <td align="left" colspan="2" style="height: 25px">
                                                <asp:Panel ID="Panel3" runat="server" Width="500px" Height="150px" BorderWidth="1px"
                                                    BorderStyle="Inset" BorderColor="White" BackColor="Silver" CssClass="HidePanel">
                                                    <asp:ListBox ID="lstCode" runat="server" Width="99%"
                                                        Height="98%" SelectionMode="Single"></asp:ListBox>
                                                </asp:Panel>
                                            </td>
                                            <td style="width: 316px" align="center" colspan="1" rowspan="1" valign="middle">
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            &nbsp;</div>
          <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div style="margin-left: 35%; margin-top: -11%; position: absolute;">
                    <b>Working Date:</b>
                    <asp:Label ID="lblWorkDate" ForeColor="Red" runat="server" CssClass="lblbox" Text=""></asp:Label>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <div style="padding-left: 5px;">
                <asp:CheckBox ID="chkScan" OnCheckedChanged="chkScan_CheckedChanged" AutoPostBack="true" runat="server" Text="By Scan" Checked="true" />
            </div>
            <table width="100%">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td style="height: 16px">
                                            <strong>
                                                <asp:Label ID="lblskuname" runat="server" Style="padding-left: 2px;" BackColor="#006699" CssClass="lblbox" Font-Bold="True"
                                                    ForeColor="White" Height="16px" Text=" Item Description" Width="345px"></asp:Label></strong>
                                        </td>
                                       
                                      
                                        <td style="height: 16px">
                                            <strong>
                                                <asp:Label ID="Label1" runat="server" Style="padding-left: 2px;" BackColor="#006699" CssClass="lblbox" Font-Bold="True"
                                                    ForeColor="White" Height="16px" Text=" Color" Width="80px"></asp:Label></strong>
                                        </td>
                                          <td style="height: 16px;">
                                            <strong>
                                                <asp:Label ID="lblFreeSKU" runat="server" Style="padding-left: 2px;" BackColor="#006699" CssClass="lblbox" Font-Bold="True"
                                                    ForeColor="White" Height="16px" Text=" Size" Width="87px"></asp:Label></strong>
                                        </td>
                                         <td style="height: 16px">
                                            <strong>
                                                <asp:Label ID="lblquantity" Style="padding-left: 2px;" runat="server" BackColor="#006699" CssClass="lblbox"
                                                    Font-Bold="True" ForeColor="White" Height="16px" Text=" Qty" Width="80px"></asp:Label></strong>
                                        </td>
                                        <td style="height: 16px">
                                            <asp:TextBox ID="txtUnitRate" runat="server" Width="200px" Font-Bold="True" CssClass="txtBox"
                                                   Style="display:none;"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 9px">
                                             <asp:TextBox ID="txtskuCode" runat="server" class="sku-c-input" Width="340px"
                                              onkeydown="HandleKeyPress(event);" placeholder="Please enter Code here"></asp:TextBox>
                                            <cc1:AutoCompleteExtender ID="AutoComplete" runat="server" TargetControlID="txtskuCode"
                                                ServicePath="wsProductList.asmx" MinimumPrefixLength="4" CompletionInterval="500"
                                                UseContextKey="true" BehaviorID="AutoCompleteBehavior" CompletionSetCount="10"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="autocomplete_listItem"
                                                EnableCaching="true" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                OnClientItemSelected="ProductSelected" FirstRowSelected="true" ServiceMethod="GetPosProductsWithSKU_ID">
                                            </cc1:AutoCompleteExtender>
                                            <asp:TextBox ID="txtskuName" runat="server" Font-Bold="True" CssClass="txtBox"
                                                    Style="display:none;"></asp:TextBox>
                                                <asp:DropDownList ID="drpSkus" runat="server" Width="345px" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="drpSkus_SelectedIndexChanged">
                                                </asp:DropDownList>
                                        </td>
                                         <td style="height: 9px">
                                            <asp:TextBox ID="txtcolor" runat="server" CssClass="txtBox" Width="75px" Enabled="False"></asp:TextBox>
                                        </td>
                                        <td style="height: 9px;">
                                            <asp:TextBox ID="txtsize" runat="server" CssClass="txtBox" Enabled="False"
                                                Width="80px"></asp:TextBox>
                                        </td>
                                       
                                        <td style="height: 9px">
                                            <asp:TextBox ID="txtQuantity" runat="server" CssClass="txtBox" onkeydown="HandleKeyPressQty(event);" onkeypress="return onlyNumbers(this,event);"  
                                                Width="80px"></asp:TextBox>
                                        </td>
                                        <td style="height: 9px">
                                            <asp:Button ID="btnAddRecord" runat="server" style="display: none" OnClick="btnAddRecord_Click" />
                                        </td>
                                    </tr>
                                    </table>
                                <table>
                                    <tr>
                                        <td align="left" colspan="5">
                                            <asp:Panel ID="Panel2" runat="server" BorderColor="Silver" BorderStyle="Groove" BorderWidth="1px"
                                                Height="280px" ScrollBars="Vertical" Width="690px">
                                                <asp:GridView ID="GrdPurchase" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                    BorderColor="White" CssClass="gridRow2" ForeColor="SteelBlue" HorizontalAlign="Center"
                                                    OnRowDeleting="GrdPurchase_RowDeleting" ShowHeader="False" Width="670px"
                                                    OnRowDataBound="GrdPurchase_RowDataBound">
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
                                                                Width="230px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="COLOR" HeaderText="Color">
                                                            <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" Width="70px"
                                                                HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="PACKSIZE" HeaderText="Size">
                                                            <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Left"
                                                                Width="82px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="SALEABLE_QUANTITY" HeaderText="Qty">
                                                            <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Left"
                                                                Width="82px" />
                                                        </asp:BoundField>                                                        
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
                                </table>
                                &nbsp; &nbsp;
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        &nbsp;&nbsp;
                         <div class="tblRow">
                             <asp:Button ID="btnSave" runat="server" AccessKey="A" Font-Size="8pt" OnClick="btnSave_Click"
                                 Text="Save" ValidationGroup="vg" Width="87px" CssClass="Button" />
                         </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
