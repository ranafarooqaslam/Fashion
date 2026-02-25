<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmSKUData.aspx.cs" Inherits="Forms_frmSKUData" Title="CORN :: Item Information" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">
    <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
    <script language="JavaScript" type="text/javascript">
        function pageLoad() {
            $("select").searchable();
        }

        function readURL(input) {
            
            if (input.files[0].size >= 30720)
            {
                alert('Maximum file size is 30kb and Image Resolution should be 250x250');
                
                return false;
            }
             if (input.files && input.files[0]) {
                
                var reader = new FileReader();
                reader.onload = function (e) {
                 
                    //$('#imgSKU').attr('src', e.target.result).width(170).height(170);
                    $("[id$='imgSKU']").attr("src", e.target.result);

                };
                reader.readAsDataURL(input.files[0]);
            }
        }
    </script>
    <div id="right_data">
        <div>
            <table width="100%">
                <tr>
                    <td>
                        <table>
                            <tbody>
                            <asp:HiddenField ID="hfSkuId" runat="server" />
                                <tr>
                                    <td align="left" colspan="6">
                                        <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100px" align="left">
                                        <strong>Principal</strong>
                                    </td>
                                    <td align="left" width="300px">
                                        <asp:DropDownList ID="ddskuPrincipal" runat="server" Width="200px" CssClass="DropList">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                    </td>
                                    <td style="width: 80px" align="left">
                                        <strong>Division</strong>
                                    </td>
                                    <td align="left" width="300px">
                                        <asp:DropDownList ID="ddskudivision" runat="server" Width="200px" CssClass="DropList">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <strong>Category</strong>
                                    </td>
                                    <td align="left" width="300px">
                                        <asp:DropDownList ID="ddskucategory" runat="server" Width="200px" CssClass="DropList"
                                            OnSelectedIndexChanged="ddskucategory_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                    </td>
                                    <td align="left">
                                        <strong>Sub Category</strong>
                                    </td>
                                    <td align="left" width="300px">
                                        <asp:DropDownList ID="ddskuSubCategory" runat="server" Width="200px" CssClass="DropList">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100px" align="left">
                                        <strong>Gender</strong>
                                    </td>
                                    <td align="left" width="300px">
                                        <asp:DropDownList ID="ddskuTag" runat="server" Width="200px" CssClass="DropList">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                    </td>
                                    <td align="left">
                                        <strong>Brand</strong>
                                    </td>
                                    <td align="left" width="300px">
                                        <asp:DropDownList ID="ddskuBrand" runat="server" Width="200px" CssClass="DropList">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100px" align="left">
                                        <strong>Origin</strong>
                                    </td>
                                    <td align="left" width="300px">
                                        <asp:DropDownList ID="drpCOuntry" runat="server" Width="200px" CssClass="DropList">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                    </td>
                                    <td align="left">
                                        <strong>GST On</strong>
                                    </td>
                                    <td align="left" width="300px">
                                        <asp:DropDownList ID="DrpSKUTaxType" runat="server" Width="200px" CssClass="DropList">
                                            <asp:ListItem Value="T">Trade Price</asp:ListItem>
                                            <asp:ListItem Value="R">Retail Price</asp:ListItem>
                                            <asp:ListItem Value="E">Exempted</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100px" align="left">
                                        <strong>Style Code</strong>
                                    </td>
                                    <td align="left" width="300px">
                                        <asp:TextBox ID="txtskucode" runat="server" Width="192px" CssClass="txtBox "></asp:TextBox>
                                    </td>
                                    <td>
                                    </td>
                                    <td align="left">
                                        <strong>Season</strong>
                                    </td>
                                    <td align="left" width="300px">
                                        <asp:DropDownList ID="drpSeason" runat="server" Width="200px" CssClass="DropList">
                                            <asp:ListItem Value="Spring">Spring</asp:ListItem>
                                            <asp:ListItem Value="Fall">Fall</asp:ListItem>
                                            <asp:ListItem Value="Winter">Winter</asp:ListItem>
                                            <asp:ListItem Value="Summer">Summer</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100px" align="left">
                                        <strong>Bar Code</strong>
                                    </td>
                                    <td align="left" width="300px">
                                        <asp:TextBox ID="txtbarcode" runat="server" Width="192px" CssClass="txtBox "></asp:TextBox>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td align="left" rowspan="6" style="border:thin; border-color:gainsboro">
                                        <asp:Image runat="server" ID="imgSKU" ImageUrl="../images/no-image.jpg" Width="150px" Height="130px" />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100px; height: 20px" align="left">
                                        <strong>Item Name</strong>
                                    </td>
                                    <td align="left" width="300px">
                                        <asp:TextBox ID="txtskuname" runat="server" Width="192px" CssClass="txtBox "></asp:TextBox>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100px" align="left">
                                        <strong>Size</strong>
                                    </td>
                                    <td width="300px">
                                        <asp:TextBox ID="txtpacksize" runat="server" Width="192px" CssClass="txtBox "></asp:TextBox>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100px" align="left">
                                        <strong>Color</strong>
                                    </td>
                                    <td width="300px">
                                        <asp:TextBox ID="txtcolor" runat="server" Width="192px" CssClass="txtBox "></asp:TextBox>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100px" align="left">
                                        <strong>SKU</strong>
                                    </td>
                                    <td width="300px">
                                        <asp:TextBox ID="txtSKU" runat="server" Width="192px" CssClass="txtBox "></asp:TextBox>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>

                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100px" align="left">
                                        <strong>Year</strong>
                                    </td>
                                    <td width="300px">
                                        <asp:TextBox ID="txtYear" runat="server" Width="192px" CssClass="txtBox "></asp:TextBox>
                                    </td>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtYear"
                                        ValidChars="0123456789" FilterType="Custom">
                                    </cc1:FilteredTextBoxExtender>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>

                                    </td>
                                </tr>
                               
                                <tr>
                                    <td > </td>
                                    <td > </td>
                                   <td>
                                    </td>
                                     <td style="width: 100px" align="left">
                                        <strong>
                                            <asp:Label ID="Label11" runat="server" Text="Upload Image"></asp:Label></strong>
                                    </td>
                                    <td width="300px">
                                        <asp:FileUpload runat="server" ID="fuImageSku"  onchange="readURL(this);" Width="200px" />
                                      <%--  <asp:Button ID="btnUploadImage" CssClass="Button" runat="server" Text="Upload" OnClick="btnUploadImage_Click" />
                                   --%> </td>
                                   <td>
                                    </td>
                                </tr>
                                <tr>
                                     <td style="width: 100px" align="left">
                                        <strong>Material</strong>
                                    </td>
                                   <td align="left" width="300px">
                                        <asp:DropDownList ID="drpMaterial" runat="server" Width="200px" CssClass="DropList">
                                            <asp:ListItem Value="0">Not Applicable</asp:ListItem>
                                            <asp:ListItem Value="1">Cotton</asp:ListItem>
                                            <asp:ListItem Value="2">Polyester</asp:ListItem>
                                            <asp:ListItem Value="3">Wool</asp:ListItem>
                                            <asp:ListItem Value="4">Silk</asp:ListItem>
                                            <asp:ListItem Value="5">Linen</asp:ListItem>
                                            <asp:ListItem Value="6">Chiffon</asp:ListItem>
                                            <asp:ListItem Value="7">Canvas</asp:ListItem>
                                            <asp:ListItem Value="8">Organza</asp:ListItem>
                                            <asp:ListItem Value="9">Fleece</asp:ListItem>
                                            <asp:ListItem Value="10">Velvet</asp:ListItem>
                                            <asp:ListItem Value="11">Satin</asp:ListItem>
                                            <asp:ListItem Value="12">Twill</asp:ListItem>
                                            <asp:ListItem Value="13">Rayon</asp:ListItem>
                                            <asp:ListItem Value="14">Nylon</asp:ListItem>
                                            <asp:ListItem Value="15">Denim</asp:ListItem>
                                            <asp:ListItem Value="16">Iron</asp:ListItem>
                                            <asp:ListItem Value="17">Stainless Steel</asp:ListItem>
                                            <asp:ListItem Value="18">Plastic</asp:ListItem>
                                            <asp:ListItem Value="19">Fibre</asp:ListItem>
                                            <asp:ListItem Value="20">Cotton</asp:ListItem>
                                            <asp:ListItem Value="21">Nylon</asp:ListItem>
                                            <asp:ListItem Value="22">Rubber</asp:ListItem>
                                            <asp:ListItem Value="23">Wooden</asp:ListItem>
                                            <asp:ListItem Value="24">Vinyl</asp:ListItem>
                                            <asp:ListItem Value="25">Mix</asp:ListItem>
                                            <asp:ListItem Value="26">Foam</asp:ListItem>
                                            <asp:ListItem Value="27">Electric</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                   
                                    <td>
                                    </td>
                                    <td>  <strong>
                                            <asp:Label ID="Label1" runat="server" Text="Show on POS" Width="100px"></asp:Label></strong>
                                    </td>
                                    <td style="width: 37px; height: 12px">
                                        <asp:CheckBox runat="server" ID="chbSHowOnPOS" Text="" />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                               
                                <tr>
                                     <td style="width: 100px" align="left">
                                        <strong>Fit</strong>
                                    </td>
                                   <td align="left" width="300px">
                                        <asp:DropDownList ID="drpFit" runat="server" Width="200px" CssClass="DropList">
                                            <asp:ListItem Value="0">Not Applicable</asp:ListItem>
                                            <asp:ListItem Value="1">Regular</asp:ListItem>
                                            <asp:ListItem Value="2">Slim</asp:ListItem>
                                            <asp:ListItem Value="3">Athletic</asp:ListItem>
                                            <asp:ListItem Value="4">Relaxed</asp:ListItem>
                                            <asp:ListItem Value="5">Loose</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                   
                                    <td>
                                    </td>
                                    <td>  <strong>
                                            <asp:Label ID="Label2" runat="server" Text="Weight" Width="100px"></asp:Label></strong>
                                    </td>
                                    <td style="width: 37px; height: 12px">
                                        <asp:TextBox ID="txtWeight" runat="server" Width="192px" CssClass="txtBox "></asp:TextBox>
                                    </td>
                                     <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtWeight"
                                        ValidChars=".0123456789" FilterType="Custom">
                                    </cc1:FilteredTextBoxExtender>
                                    <td>
                                    </td>
                                </tr>

                                 <tr>
                                     <td style="width: 100px" align="left">
                                        <strong>HS Code</strong>
                                    </td>
                                   <td align="left" width="300px">
                                        <asp:TextBox ID="txtKarat" runat="server" Width="192px" CssClass="txtBox "></asp:TextBox>
                                    </td>
                                   
                                    <td>
                                    </td>
                                    <td>  <strong>
                                            <asp:Label ID="Label3" runat="server" Text="Making Charge" Width="100px" Visible="false"></asp:Label></strong>
                                    </td>
                                    <td style="width: 37px; height: 12px">
                                        <asp:TextBox ID="txtMakeCharge" runat="server" Width="192px" CssClass="txtBox" Visible="false"></asp:TextBox>
                                    </td>
                                     <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtMakeCharge"
                                        ValidChars=".0123456789" FilterType="Custom">
                                    </cc1:FilteredTextBoxExtender>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                     <td style="width: 100px" align="left">
                                    </td>
                                   <td align="left" width="300px">
                                    </td>
                                   
                                    <td>
                                    </td>
                                    <td>  
                                    </td>
                                    <td style="width: 37px; height: 12px">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100px; height: 12px">
                                    </td>
                                    <td style="height: 12px" align="left" colspan="4">
                                        <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Width="86px" Font-Size="8pt"
                                            Text="Save" CssClass="Button" />
                                    </td>
                                    <td style="width: 37px; height: 12px" align="left">
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <asp:UpdateProgress ID="UpdateProgress5" runat="server">
                            <ProgressTemplate>
                                <asp:ImageButton ID="ImageButton1" runat="server" Width="26px" Height="23px" ImageUrl="~/App_Themes/Granite/Images/image003.gif">
                                </asp:ImageButton>&nbsp; Loading....
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
            </table>
        </div>
        <div>
           <%-- <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>--%>
                    <table class="tblhead">
                        <tbody>
                            <tr>
                                <td style="color: White; font-weight: bold; width: 153px">
                                    Select Searching Type
                                </td>
                                <td style="width: 170px; height: 22px" align="left">
                                    <asp:DropDownList ID="ddSearchType" runat="server" Width="200px" CssClass="DropList">
                                        <asp:ListItem Value="SKU_code">All Records</asp:ListItem>
                                        <asp:ListItem Value="Principal">Principal</asp:ListItem>
                                        <asp:ListItem Value="Division">Division</asp:ListItem>
                                        <asp:ListItem Value="Category">Category</asp:ListItem>
                                          <asp:ListItem Value="SKU_NAME">Name</asp:ListItem>
                                          <asp:ListItem Value="COLOR">Color</asp:ListItem>
                                          <asp:ListItem Value="SKU_CODE">BarCode</asp:ListItem>
                                          <asp:ListItem Value="BAR_CODE">Style Code</asp:ListItem>
                                        <asp:ListItem Value="Brand">Brand</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 224px; height: 22px" align="left">
                                    <asp:TextBox ID="txtSeach" runat="server" Width="200px" CssClass="txtBox "></asp:TextBox>
                                </td>
                                <td style="width: 250px; height: 22px" align="left">
                                    <asp:Button ID="btnFilter" runat="server" Width="85px" Font-Size="8pt" Text="Filter"
                                        OnClick="btnFilter_Click"></asp:Button>
                                </td>
                                <td style="width: 95px; height: 22px">
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <asp:Panel ID="Panel1" runat="server" Width="100%" Height="200px" ScrollBars="Auto"
                        BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px">
                        <asp:GridView ID="grdSKUData" runat="server" Width="100%" ForeColor="SteelBlue" HorizontalAlign="Center"
                            BorderColor="SteelBlue" BackColor="White" AutoGenerateColumns="False" OnRowEditing="grdSKUData_RowEditing"
                            OnRowDeleting="grdSKUData_RowDeleting">
                             <alternatingrowstyle backcolor="#E0E0E0"/>
                            <Columns>
                                <asp:BoundField DataField="Principal_Id">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Division_Id">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Category_Id" HeaderText="Category_Id">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Brand_Id" HeaderText="Brand_Id">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="TAG_ID" HeaderText="TAG_ID">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="SKU_ID" HeaderText="SKU_ID">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Principal" HeaderText="Principal">
                                     <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                    <%--<ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>--%>
                                </asp:BoundField>
                                <asp:BoundField DataField="Division" HeaderText="Division">
                                     <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                    <%--<ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>--%>
                                </asp:BoundField>
                                <asp:BoundField DataField="Category" HeaderText="Category">
                                    <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Brand" HeaderText="Brand">
                                     <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                    <%--<ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>--%>
                                </asp:BoundField>
                                <asp:BoundField DataField="TAG" HeaderText="Gender">
                                     <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                    <%--<ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>--%>
                                </asp:BoundField>
                                <asp:BoundField DataField="SKU_CODE" HeaderText="Bar Code">
                                    <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="BAR_CODE" HeaderText="Style Code">
                                    <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="SKU_NAME" HeaderText="Name">
                                    <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="PACKSIZE" HeaderText="Size">
                                    <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="COLOR" HeaderText="Color">
                                    <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="GST_ON" HeaderText="GST">
                                    <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="SKU_COUNTRY">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="SKU_SEASON">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="SKU">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="year">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="SUBCATEGORY_ID">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                  <asp:BoundField DataField="IP_ADDRESS" >
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                 <asp:BoundField DataField="SHOW_ON_POS" >
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                 <asp:BoundField DataField="Material" HeaderText="Material">
                                    <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
                                </asp:BoundField>
                                 <asp:BoundField DataField="Fit" HeaderText="Fit">
                                    <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
                                </asp:BoundField>
                                 <asp:BoundField DataField="Weight" HeaderText="Weight">
                                    <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
                                </asp:BoundField>
                                 <asp:BoundField DataField="Karat" HeaderText="Karat">
                                   <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
                                </asp:BoundField>
                                 <asp:BoundField DataField="MakeCharge" HeaderText="Making Charge">
                                    <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
                                </asp:BoundField>
                                 

                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text="Delete">
                                                                    <img src="../images/edit.gif" />
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" Width="45px" HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Delete">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnDelete" ToolTip="Delete" runat="server" Text="Delete" OnClientClick="javascript:return confirm('Are you sure you want to Delete?');return false;"
                                            CommandName="Delete">
                                                 <img src="../images/delete.gif" width="16" height="16" alt="Delete">
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid" Width="45px" HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="tblhead"></HeaderStyle>
                        </asp:GridView>
                    </asp:Panel>
                <%--</ContentTemplate>
            </asp:UpdatePanel>--%>
        </div>
    </div>
</asp:Content>
