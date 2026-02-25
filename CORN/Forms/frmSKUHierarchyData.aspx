<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmSKUHierarchyData.aspx.cs" Inherits="frmSKUHierarchyData" Title="CORN :: Item Hierarchy" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">
            <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
<script language="JavaScript" type="text/javascript">
    function pageLoad() {
        $("select").searchable();
    
            $('#<%=txtNTN.ClientID %>').mask("9999999-9");
            $('#<%=txtSTRN.ClientID %>').mask("99-99-9999-999-99");
        }
 
    </script>
    <div id="right_data">
        <table width="100%">
            <tr>
                <td>
                    <cc1:TabContainer ID="TabContainer1" runat="server" Height="420px" Width="650px"
                        ActiveTabIndex="4">
                        <cc1:TabPanel ID="TabPanel1" runat="server">
                            <HeaderTemplate>
                                Principal
                            </HeaderTemplate>
                            <ContentTemplate>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 100px">
                                        </td>
                                        <td style="width: 100px">
                                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                <ContentTemplate>
                                                   <asp:Panel ID="pnl_dept" runat="server">
                                                    <fieldset>
                                                    <table width="100%">
                                                        <tbody>
                                                            <tr>
                                                                <td style="width: 100px">
                                                                </td>
                                                                <td colspan="2">
                                                                    <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True" ></asp:Label><br />
                                                                </td>
                                                                <td style="width: 100px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 100px; height: 28px">
                                                                </td>
                                                                <td style="width: 90px; height: 28px">
                                                                    <strong>
                                                                        Code</strong>
                                                                </td>
                                                                <td style="width: 100px; height: 28px">
                                                                    <asp:TextBox ID="txtPrincipalCode" runat="server" Width="200px" 
                                                                        Enabled="False" CssClass="txtBox "></asp:TextBox>
                                                                </td>
                                                                <td style="width: 100px; height: 28px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 100px; height: 29px">
                                                                </td>
                                                                <td style="width: 49px; height: 29px">
                                                                    <strong>
                                                                        Name</strong>
                                                                </td>
                                                                <td style="width: 100px; height: 29px">
                                                                    <asp:TextBox ID="txtPrincipalName" runat="server" Width="200px" 
                                                                        Enabled="False" CssClass="txtBox "></asp:TextBox>
                                                                </td>
                                                                <td style="width: 100px; height: 29px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 100px; height: 29px">
                                                                </td>
                                                                <td style="width: 49px; height: 29px">
                                                                    <strong>
                                                                        Address</strong>
                                                                </td>
                                                                <td style="width: 100px; height: 29px">
                                                                    <asp:TextBox ID="txtAddress" runat="server" Width="200px" 
                                                                        Enabled="False" CssClass="txtBox "></asp:TextBox>
                                                                </td>
                                                                <td style="width: 100px; height: 29px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 100px; height: 29px">
                                                                </td>
                                                                <td style="width: 49px; height: 29px">
                                                                    <strong>
                                                                        NTN</strong>
                                                                </td>
                                                                <td style="width: 100px; height: 29px">
                                                                    <asp:TextBox ID="txtNTN" runat="server" Width="200px"  Enabled="False"
                                                                        CssClass="txtBox "></asp:TextBox>
                                                                </td>
                                                                <td style="width: 100px; height: 29px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 100px; height: 29px">
                                                                </td>
                                                                <td style="width: 49px; height: 29px">
                                                                    <strong>
                                                                        STRN</strong>
                                                                </td>
                                                                <td style="width: 100px; height: 29px">
                                                                    <asp:TextBox ID="txtSTRN" runat="server" Width="200px" Enabled="False"
                                                                        CssClass="txtBox "></asp:TextBox>
                                                                </td>
                                                                <td style="width: 100px; height: 29px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 100px; height: 37px">
                                                                </td>
                                                                <td style="width: 49px; height: 37px" align="left">
                                                                </td>
                                                                <td style="width: 100px; height: 37px">
                                                                    <asp:CheckBox ID="ChIsMunalDiscount" runat="server" Width="190px" Text="Is Manual Dicount"
                                                                        ></asp:CheckBox>
                                                                </td>
                                                                <td style="width: 100px; height: 37px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 100px">
                                                                </td>
                                                                <td style="width: 49px" align="right">
                                                                    &nbsp;
                                                                </td>
                                                                <td style="width: 100px">
                                                                    <asp:Button ID="btnSavePrincipal" OnClick="btnSavePrincipal_Click" runat="server"
                                                                        Width="85px" Font-Size="8pt" Text="New" CssClass="Button" />
                                                                </td>
                                                                <td style="width: 100px">
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                    </fieldset>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel6">
                                                <ProgressTemplate>
                                                    <asp:ImageButton ID="ImageButton1" runat="server" Width="33px" Height="31px" 
                                                        ImageUrl="~/App_Themes/Granite/Images/image003.gif"></asp:ImageButton>
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </td>
                                        <td style="width: 100px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="3">
                                            <asp:Panel ID="Panel1" runat="server" Height="100px" ScrollBars="Vertical" Width="100%">
                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="GrdPrincipal" runat="server" Width="100%" ForeColor="SteelBlue" BorderWidth="1px"
                                                            Font-Size="9pt"  CssClass="gridRow2" BorderColor="SteelBlue"
                                                            BackColor="White" HorizontalAlign="Center" AutoGenerateColumns="False" OnRowEditing="GrdPrincipal_RowEditing"
                                                            OnRowDeleting="GrdPrincipal_RowDeleting" OnPageIndexChanging="GrdPrincipal_PageIndexChanging">
                                                            <PagerSettings PreviousPageText="Previous" Mode="NextPrevious" LastPageText="" FirstPageText=""
                                                                NextPageText="Next"></PagerSettings>
                                                             <alternatingrowstyle backcolor="#E0E0E0"/>
                                                            <Columns>
                                                                <asp:BoundField DataField="SKU_HIE_ID" HeaderText="Id">
                                                                    <ItemStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="Silver" HorizontalAlign="Left" Width="5%">
                                                                    </ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SKU_HIE_CODE" HeaderText="Code">
                                                                    <ItemStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="Silver" HorizontalAlign="Left" Width="10%">
                                                                    </ItemStyle>
                                                                    <HeaderStyle BorderStyle="Solid" BorderWidth="1px"  HorizontalAlign="Left">
                                                                    </HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SKU_HIE_NAME" HeaderText="Name">
                                                                    <ItemStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="Silver" HorizontalAlign="Left"  Width="45%">
                                                                    </ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="IS_MANUALDISCOUNT" HeaderText="Manual Discount">
                                                                 <ItemStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="Silver" HorizontalAlign="Left"  Width="20%">
                                                                    </ItemStyle></asp:BoundField>
                                                                <asp:BoundField DataField="ADDRESS" Visible="False" HeaderText="ADDRESS"></asp:BoundField>
                                                                <asp:BoundField DataField="NTN" Visible="False" HeaderText="NTN"></asp:BoundField>
                                                                <asp:BoundField DataField="STRN" Visible="False" HeaderText="STRN"></asp:BoundField>
                                                               <asp:TemplateField HeaderText="Edit">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text="Delete">
                                                                    <img src="../images/edit.gif" />
                                                                                </asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" Width="45px" HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                           
                                                                 <asp:TemplateField HeaderText="Delete" >
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="btnDelete" ToolTip="Delete" runat="server" Text="Delete" OnClientClick="javascript:return confirm('Are you sure you want to Delete?');return false;"
                                                                                    CommandName="Delete">
                                                                                    <img src="../images/delete.gif" width="16" height="16">
                                                                                </asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid" Width="45px" HorizontalAlign="Center">
                                                                            </ItemStyle>
                                                                        </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle CssClass="tblhead"></HeaderStyle>
                                                            <AlternatingRowStyle CssClass="GridAlternateRowStyle"></AlternatingRowStyle>
                                                        </asp:GridView>
                                                       <%-- <telerik:RadGrid ID="GrdPrincipal"runat="server" Width="99%" ForeColor="SteelBlue" BorderWidth="1px"
                                                            Font-Size="9pt" __designer:wfdid="w202" CssClass="gridRow2" BorderColor="White"
                                                            BackColor="White" HorizontalAlign="Center" AutoGenerateColumns="False" OnRowEditing="GrdPrincipal_RowEditing"
                                                            OnRowDeleting="GrdPrincipal_RowDeleting" OnPageIndexChanging="GrdPrincipal_PageIndexChanging">
                                                            <Columns>
                                                            
                                                                <telerik:GridBoundColumn DataField="SKU_HIE_ID" Visible="False" HeaderText="ID">
                                                                    <ItemStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="Silver" HorizontalAlign="Left">
                                                                    </ItemStyle>
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="SKU_HIE_CODE" HeaderText="Code">
                                                                    <ItemStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="Silver" HorizontalAlign="Left" Width="150px">
                                                                    </ItemStyle>
                                                                    <HeaderStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="Silver" HorizontalAlign="Left">
                                                                    </HeaderStyle>
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="SKU_HIE_NAME" HeaderText="Name">
                                                                    <ItemStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="Silver" HorizontalAlign="Left" Width="250px">
                                                                    </ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="IS_MANUALDISCOUNT" HeaderText="Manual Discount">
                                                                 <ItemStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="Silver" HorizontalAlign="Left" Width="100px">
                                                                    </ItemStyle></asp:BoundField>
                                                                <telerik:GridBoundColumn DataField="ADDRESS" Visible="False" HeaderText="ADDRESS"></asp:BoundField>
                                                                <telerik:GridBoundColumn DataField="NTN" Visible="False" HeaderText="NTN"></asp:BoundField>
                                                                <telerik:GridBoundColumn DataField="STRN" Visible="False" HeaderText="STRN"></asp:BoundField>
                                                                <telerik:CommandField HeaderText="Edit" ShowEditButton="True">
                                                                 <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
                                                                </telerik:CommandField>

                                                                <asp:TemplateField HeaderText="Delete">
                                                                 <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" OnClientClick="javascript:return confirm('Are you sure you want to Delete?');return false;"
                                                                            Text="Delete"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle CssClass="tblhead"></HeaderStyle>
                                                            <AlternatingRowStyle CssClass="GridAlternateRowStyle"></AlternatingRowStyle>
                                                        </telerik:RadGrid>--%>
                                                         
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </cc1:TabPanel>
                        <cc1:TabPanel ID="TabPanel2" runat="server">
                            <HeaderTemplate>
                                Division
                            </HeaderTemplate>
                            <ContentTemplate>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 100px;">
                                        </td>
                                        <td align="center">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                 <asp:Panel ID="Panel6" runat="server">
                                                    <fieldset>
                                                    <table width="100%">
                                                        <tbody>
                                                            <tr>
                                                                <td style="width: 100px; height: 8px">
                                                                </td>
                                                                <td style="height: 8px" align="left" colspan="2">
                                                                    <asp:Label ID="lblErrorMsgDivsion" runat="server" ForeColor="Red" Font-Bold="True"
                                                                        ></asp:Label><br />
                                                                </td>
                                                                <td style="width: 100px; height: 8px">
                                                                </td>
                                                            </tr>
                                                           
                                                           <%-- <tr>
                                                                <td style="width: 100px; height: 23px">
                                                                </td>
                                                                <td style="width: 159px" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label3" runat="server" Width="80px" Text="Principal" __designer:wfdid="w17"></asp:Label></strong>
                                                                    &nbsp;
                                                                </td>
                                                                <td style="width: 100px; height: 28px">
                                                                    <asp:DropDownList ID="dddivisonPrincipal" runat="server" Width="200px" __designer:wfdid="w18"
                                                                        CssClass="DropList" AutoPostBack="True" OnSelectedIndexChanged="dddivisonPrincipal_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="width: 100px; height: 23px">
                                                                </td>
                                                            </tr>--%>
                                                            <tr>
                                                                <td style="width: 100px; height: 28px">
                                                                </td>
                                                                <td style="width: 80px; height: 28px" align="left">
                                                                    <strong>
                                                                        Code</strong>
                                                                </td>
                                                                <td style="width: 100px; height: 28px">
                                                                    <asp:TextBox ID="txtDivisionCode" runat="server" Width="194px" 
                                                                        Enabled="False" CssClass="txtBox "></asp:TextBox>
                                                                </td>
                                                                <td style="width: 100px; height: 28px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 100px; height: 29px">
                                                                </td>
                                                                <td style="width: 80px; height: 29px" align="left">
                                                                    <strong>
                                                                        Name</strong>
                                                                </td>
                                                                <td style="width: 100px; height: 29px">
                                                                    <asp:TextBox ID="txtDivisionName" runat="server" Width="194px" 
                                                                        Enabled="False" CssClass="txtBox "></asp:TextBox>
                                                                </td>
                                                                <td style="width: 100px; height: 29px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 100px; height: 37px">
                                                                </td>
                                                                <td style="height: 37px" align="right">
                                                                    &nbsp;
                                                                </td>
                                                                <td style="width: 100px; height: 37px" align="left">
                                                                    <asp:Button ID="btnSaveDivison" OnClick="btnSaveDivison_Click" runat="server" Width="85px"
                                                                        Font-Size="8pt" Text="New" CssClass="Button" />
                                                                </td>
                                                                <td style="width: 100px; height: 37px">
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                    </fieldset>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                                <ProgressTemplate>
                                                    <asp:Image ID="Image2" runat="server" Width="30px" Height="28px" 
                                                        ImageUrl="~/App_Themes/Granite/Images/image003.gif"></asp:Image>&nbsp; Loading
                                                    .........
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </td>
                                        <td style="width: 100px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="3">
                                            <asp:Panel ID="Panel2" runat="server" Height="200px" ScrollBars="Vertical" Width="100%">
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="GrdDivision" runat="server" Width="100%" ForeColor="SteelBlue" Font-Size="9pt"
                                                             CssClass="gridRow2" BorderColor="SteelBlue" BackColor="White"
                                                            HorizontalAlign="Center" AutoGenerateColumns="False" OnRowEditing="GrdDivision_RowEditing"
                                                            OnRowDeleting="GrdDivision_RowDeleting" OnPageIndexChanging="GrdDivision_PageIndexChanging">
                                                            <PagerSettings FirstPageText="" LastPageText="" Mode="NextPrevious" NextPageText="Next"
                                                                PreviousPageText="Previous"></PagerSettings>
                                                             <alternatingrowstyle backcolor="#E0E0E0"/>
                                                            <Columns>
                                                                <asp:BoundField DataField="SKU_HIE_ID" HeaderText="Id" >
                                                                    <ItemStyle HorizontalAlign="Left" BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid" Width="5%">
                                                                    </ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SKU_HIE_CODE" HeaderText="Code">
                                                                    <ItemStyle HorizontalAlign="Left" BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid" Width="15%">
                                                                    </ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SKU_HIE_NAME" HeaderText="Name">
                                                                    <ItemStyle HorizontalAlign="Left" BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid" Width="60%">
                                                                    </ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Edit">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text="Delete">
                                                                    <img src="../images/edit.gif" />
                                                                                </asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" Width="45px" HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                           
                                                                 <asp:TemplateField HeaderText="Delete" >
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="btnDelete" ToolTip="Delete" runat="server" Text="Delete" OnClientClick="javascript:return confirm('Are you sure you want to Delete?');return false;"
                                                                                    CommandName="Delete">
                                                                                    <img src="../images/delete.gif" width="16" height="16">
                                                                                </asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid" Width="45px" HorizontalAlign="Center">
                                                                            </ItemStyle>
                                                                        </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle CssClass="tblhead" BorderColor="SteelBlue"></HeaderStyle>
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </ContentTemplate>
                        </cc1:TabPanel>
                        <cc1:TabPanel ID="TabPanel3" runat="server">
                            <HeaderTemplate>
                                Category
                            </HeaderTemplate>
                            <ContentTemplate>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 100px">
                                        </td>
                                        <td align="center" style="width: 100px">
                                            <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                                                <ContentTemplate>
                                                 <asp:Panel ID="Panel7" runat="server">
                                                    <fieldset>
                                                    <table width="100%">
                                                        <tbody>
                                                            <tr>
                                                                <td style="width: 100px; height: 16px">
                                                                </td>
                                                                <td style="height: 16px" align="left" colspan="2">
                                                                    <br />
                                                                    <asp:Label ID="lblErrorMsgCategory" runat="server" ForeColor="Red" Font-Bold="True"
                                                                        ></asp:Label>
                                                                </td>
                                                                <td style="width: 100px; height: 16px">
                                                                </td>
                                                            </tr>
                                                            
                                                           <%-- <tr>
                                                                <td style="width: 100px; height: 21px">
                                                                </td>
                                                                <td style="width: 69px; height: 21px" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label5" runat="server" Width="80px" Text="Principal" __designer:wfdid="w115"></asp:Label></strong>
                                                                </td>
                                                                <td style="width: 100px; height: 28px">
                                                                    <asp:DropDownList ID="DrpCategoryPrincipal" runat="server" Width="200px" __designer:wfdid="w116"
                                                                        CssClass="DropList" AutoPostBack="True" OnSelectedIndexChanged="DrpCategoryPrincipal_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="width: 100px; height: 21px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 100px; height: 21px">
                                                                </td>
                                                                <td style="width: 69px; height: 21px" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label4" runat="server" Width="80px" Text="Division" __designer:wfdid="w117"></asp:Label></strong>
                                                                </td>
                                                                <td style="width: 100px; height: 28px">
                                                                    <asp:DropDownList ID="ddCategoryDivision" runat="server" Width="200px" __designer:wfdid="w118"
                                                                        CssClass="DropList" AutoPostBack="True" OnSelectedIndexChanged="ddCategoryDivision_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="width: 100px; height: 21px">
                                                                </td>
                                                            </tr>--%>
                                                            <tr>
                                                                <td style="width: 100px; height: 28px">
                                                                </td>
                                                                <td style="width: 80px; height: 28px" align="left">
                                                                    <strong>
                                                                        Code</strong>
                                                                </td>
                                                                <td style="width: 100px; height: 28px">
                                                                    <asp:TextBox ID="txtCategoryCode" runat="server" Width="194px" 
                                                                        Enabled="False" CssClass="txtBox "></asp:TextBox>
                                                                </td>
                                                                <td style="width: 100px; height: 28px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 100px; height: 29px">
                                                                </td>
                                                                <td style="width: 80px; height: 29px" align="left">
                                                                    <strong>
                                                                        Name</strong>
                                                                </td>
                                                                <td style="width: 100px; height: 29px">
                                                                    <asp:TextBox ID="txtCategoryName" runat="server" Width="194px" 
                                                                        Enabled="False" CssClass="txtBox "></asp:TextBox>
                                                                </td>
                                                                <td style="width: 100px; height: 29px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 100px; height: 37px">
                                                                </td>
                                                                <td style="width: 80px; height: 37px" align="right">
                                                                    &nbsp;
                                                                </td>
                                                                <td style="width: 100px; height: 37px" align="left">
                                                                    <asp:Button ID="btnSaveCategory" OnClick="btnSaveCategory_Click" runat="server" Width="85px"
                                                                        Font-Size="8pt" Text="New" CssClass="Button" />
                                                                </td>
                                                                <td style="width: 100px; height: 37px">
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                    </fieldset>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpdatePanel21">
                                                <ProgressTemplate>
                                                    <asp:Image ID="Image3" runat="server" Width="30px" Height="28px" 
                                                        ImageUrl="~/App_Themes/Granite/Images/image003.gif"></asp:Image>&nbsp; Loading
                                                    .........
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </td>
                                        <td style="width: 100px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="3">
                                            <asp:Panel ID="Panel3" runat="server" Height="200px" ScrollBars="Vertical" Width="100%">
                                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="GrdCategory" runat="server" Width="100%" ForeColor="SteelBlue" Font-Size="9pt"
                                                            CssClass="gridRow2" BorderColor="SteelBlue" BackColor="White"
                                                            HorizontalAlign="Center" AutoGenerateColumns="False" OnRowEditing="GrdCategory_RowEditing"
                                                            OnRowDeleting="GrdCategory_RowDeleting" OnPageIndexChanging="GrdCategory_PageIndexChanging">
                                                            <PagerSettings FirstPageText="" LastPageText="" Mode="NextPrevious" NextPageText="Next"
                                                                PreviousPageText="Previous"></PagerSettings>
                                                             <alternatingrowstyle backcolor="#E0E0E0"/>
                                                            <Columns>
                                                                <asp:BoundField DataField="SKU_HIE_ID" HeaderText="Id" >
                                                                    <ItemStyle HorizontalAlign="Left" BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid" Width="5%">
                                                                    </ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SKU_HIE_CODE" HeaderText="Code">
                                                                    <ItemStyle HorizontalAlign="Left" BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid" Width="15%">
                                                                    </ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SKU_HIE_NAME" HeaderText="Name">
                                                                    <ItemStyle HorizontalAlign="Left" BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid" Width="60%">
                                                                    </ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Edit">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text="Delete">
                                                                    <img src="../images/edit.gif" />
                                                                                </asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" Width="45px" HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                           
                                                                 <asp:TemplateField HeaderText="Delete" >
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="btnDelete" ToolTip="Delete" runat="server" Text="Delete" OnClientClick="javascript:return confirm('Are you sure you want to Delete?');return false;"
                                                                                    CommandName="Delete">
                                                                                    <img src="../images/delete.gif" width="16" height="16">
                                                                                </asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid" Width="45px" HorizontalAlign="Center">
                                                                            </ItemStyle>
                                                                        </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle CssClass="tblhead"></HeaderStyle>
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </cc1:TabPanel>
                         <cc1:TabPanel ID="TabPanel6" runat="server">
                            <HeaderTemplate>
                                Sub Category
                            </HeaderTemplate>
                            <ContentTemplate>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 100px">
                                        </td>
                                        <td align="center" style="width: 100px">
                                            <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                <ContentTemplate>
                                                 <asp:Panel ID="Panel10" runat="server">
                                                    <fieldset>
                                                    <table width="100%">
                                                        <tbody>
                                                            <tr>
                                                                <td colspan="4">
                                                                    <asp:Label ID="lblErrorMsgSubCategory" runat="server" ForeColor="Red" Font-Bold="True"
                                                                        ></asp:Label><br />
                                                                </td>
                                                                <td style="width: 100px">
                                                                </td>
                                                            </tr>
                                                          
                                                            <tr>
                                                            <td style="width: 100px; height: 28px">
                                                                </td>
                                                                <td style="height: 25px" align="left">
                                                                    <strong>
                                                                        Category</strong>
                                                                </td>
                                                                <td style="width: 100px; height: 28px">
                                                                    <asp:DropDownList ID="ddSubCategory" runat="server" Width="200px" 
                                                                        CssClass="DropList" AutoPostBack="True" OnSelectedIndexChanged="ddSubCategory_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="width: 100px; height: 28px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                             <td style="width: 100px; height: 28px">
                                                                </td>
                                                                <td style="width: 80px; height: 28px" align="left">
                                                                    <strong>
                                                                        Code</strong>
                                                                </td>
                                                                <td style="width: 100px; height: 28px">
                                                                    <asp:TextBox ID="txtSubCategoryCode" runat="server" Width="194px" 
                                                                        Enabled="False" CssClass="txtBox "></asp:TextBox>
                                                                </td>
                                                                <td style="width: 100px; height: 28px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                             <td style="width: 100px; height: 28px">
                                                                </td>
                                                                <td style="width: 80px; height: 29px" align="left">
                                                                    <strong>
                                                                        Name</strong>
                                                                </td>
                                                                <td style="width: 100px; height: 29px">
                                                                    <asp:TextBox ID="txtSubCategoryName" runat="server" Width="194px" 
                                                                        Enabled="False" CssClass="txtBox "></asp:TextBox>
                                                                </td>
                                                                <td style="width: 100px; height: 29px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                             <td style="width: 100px; height: 37px">
                                                                </td>
                                                                <td style="width: 100px; height: 37px">
                                                                </td>
                                                                <td style="width: 80px; height: 37px" align="left">
                                                                    <asp:Button AccessKey="B" ID="btnSaveSubCategory" OnClick="btnSaveSubCategory_Click" runat="server"
                                                                        Width="85px" Font-Size="8pt" Text="New" CssClass="Button" />
                                                                </td>
                                                                <td style="width: 100px; height: 37px">
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                    </fieldset>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="UpdatePanel10">
                                                <ProgressTemplate>
                                                    <asp:Image ID="Image4" runat="server" Width="24px" Height="23px" 
                                                        ImageUrl="~/App_Themes/Granite/Images/image003.gif"></asp:Image>&nbsp; Loading.....
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </td>
                                        <td style="width: 100px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="3">
                                            <asp:Panel ID="Panel11" runat="server" Height="200px" ScrollBars="Vertical" Width="100%">
                                                <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="GrdSubCategory" runat="server" Width="100%" ForeColor="SteelBlue" Font-Size="9pt"
                                                             CssClass="gridRow2" BorderColor="SteelBlue" BackColor="White"
                                                            HorizontalAlign="Center" AutoGenerateColumns="False" OnRowEditing="GrdSubCategory_RowEditing"
                                                            OnRowDeleting="GrdSubCategory_RowDeleting" >
                                                             <alternatingrowstyle backcolor="#E0E0E0"/>
                                                            <Columns>
                                                                <asp:BoundField DataField="SKU_HIE_ID" HeaderText="Id">
                                                                    <ItemStyle HorizontalAlign="Left" BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"  Width="5%">
                                                                    </ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SKU_HIE_CODE" HeaderText="Code">
                                                                    <ItemStyle HorizontalAlign="Left" BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid" Width="15%">
                                                                    </ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SKU_HIE_NAME" HeaderText="Name">
                                                                    <ItemStyle HorizontalAlign="Left" BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid" Width="60%">
                                                                    </ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                 <asp:TemplateField HeaderText="Edit">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text="Delete">
                                                                    <img src="../images/edit.gif" />
                                                                                </asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" Width="45px" HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                           
                                                                 <asp:TemplateField HeaderText="Delete" >
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="btnDelete" ToolTip="Delete" runat="server" Text="Delete" OnClientClick="javascript:return confirm('Are you sure you want to Delete?');return false;"
                                                                                    CommandName="Delete">
                                                                                    <img src="../images/delete.gif" width="16" height="16">
                                                                                </asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid" Width="45px" HorizontalAlign="Center">
                                                                            </ItemStyle>
                                                                        </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle CssClass="tblhead"></HeaderStyle>
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </cc1:TabPanel>
                        <cc1:TabPanel ID="TabPanel4" runat="server">
                            <HeaderTemplate>
                                Brand
                            </HeaderTemplate>
                            <ContentTemplate>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 100px">
                                        </td>
                                        <td align="center" style="width: 100px">
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                <ContentTemplate>
                                                 <asp:Panel ID="Panel8" runat="server">
                                                    <fieldset>
                                                    <table width="100%">
                                                        <tbody>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:Label ID="lblErrorMsgBrand" runat="server" ForeColor="Red" Font-Bold="True"
                                                                        ></asp:Label><br />
                                                                </td>
                                                                <td style="width: 100px">
                                                                </td>
                                                            </tr>
                                                            <%--<tr>
                                                                <td style="height: 25px" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label6" runat="server" Width="80px" Text="Principal" __designer:wfdid="w149"></asp:Label></strong>
                                                                </td>
                                                                <td style="width: 100px">
                                                                    <asp:DropDownList ID="DrpBrandPrincipal" runat="server" Width="200px" __designer:wfdid="w150"
                                                                        CssClass="DropList" AutoPostBack="True" OnSelectedIndexChanged="DrpBrandPrincipal_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="width: 100px">
                                                                </td>
                                                            </tr>--%>
                                                          <%--  <tr>
                                                                <td style="height: 21px" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label10" runat="server" Width="64px" Text="Division" __designer:wfdid="w151"></asp:Label></strong>
                                                                </td>
                                                                <td style="height: 25px">
                                                                    <asp:DropDownList ID="DrpBrandDivision" runat="server" Width="200px" __designer:wfdid="w152"
                                                                        CssClass="DropList" AutoPostBack="True" OnSelectedIndexChanged="DrpBrandDivision_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="width: 100px; height: 21px">
                                                                </td>
                                                            </tr>--%>
                                                          <%--  <tr>
                                                                <td style="height: 25px" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label9" runat="server" Width="80px" Text="Category" __designer:wfdid="w153"></asp:Label></strong>
                                                                </td>
                                                                <td style="width: 100px; height: 28px">
                                                                    <asp:DropDownList ID="ddBrandCategory" runat="server" Width="200px" __designer:wfdid="w154"
                                                                        CssClass="DropList" AutoPostBack="True" OnSelectedIndexChanged="ddBrandCategory_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="width: 100px; height: 28px">
                                                                </td>
                                                            </tr>--%>
                                                            <tr>
                                                             <td style="width: 100px; height: 28px">
                                                                </td>
                                                                <td style="width: 80px; height: 28px" align="left">
                                                                    <strong>
                                                                        Code</strong>
                                                                </td>
                                                                <td style="width: 100px; height: 28px">
                                                                    <asp:TextBox ID="txtBrandCode" runat="server" Width="194px" 
                                                                        Enabled="False" CssClass="txtBox "></asp:TextBox>
                                                                </td>
                                                                <td style="width: 100px; height: 28px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                             <td style="width: 100px; height: 28px">
                                                                </td>
                                                                <td style="width: 80px; height: 29px" align="left">
                                                                    <strong>
                                                                        Name</strong>
                                                                </td>
                                                                <td style="width: 100px; height: 29px">
                                                                    <asp:TextBox ID="txtBrandName" runat="server" Width="194px" 
                                                                        Enabled="False" CssClass="txtBox "></asp:TextBox>
                                                                </td>
                                                                <td style="width: 100px; height: 29px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                             <td style="width: 100px; height: 37px">
                                                                </td>
                                                                <td style="width: 100px; height: 37px">
                                                                </td>
                                                                <td style="width: 80px; height: 37px" align="left">
                                                                    <asp:Button AccessKey="B" ID="btnSaveBrand" OnClick="btnSaveBrand_Click" runat="server"
                                                                        Width="85px" Font-Size="8pt" Text="New" CssClass="Button" />
                                                                </td>
                                                                <td style="width: 100px; height: 37px">
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                    </fieldset>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="UpdatePanel4">
                                                <ProgressTemplate>
                                                    <asp:Image ID="Image4" runat="server" Width="24px" Height="23px" 
                                                        ImageUrl="~/App_Themes/Granite/Images/image003.gif"></asp:Image>&nbsp; Loading.....
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </td>
                                        <td style="width: 100px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="3">
                                            <asp:Panel ID="Panel4" runat="server" Height="200px" ScrollBars="Vertical" Width="100%">
                                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="GrdBrand" runat="server" Width="100%" ForeColor="SteelBlue" Font-Size="9pt"
                                                             CssClass="gridRow2" BorderColor="SteelBlue" BackColor="White"
                                                            HorizontalAlign="Center" AutoGenerateColumns="False" OnRowEditing="GrdBrand_RowEditing"
                                                            OnRowDeleting="GrdBrand_RowDeleting" OnPageIndexChanging="GrdBrand_PageIndexChanging">
                                                            <PagerSettings FirstPageText="" LastPageText="" Mode="NextPrevious" NextPageText="Next"
                                                                PreviousPageText="Previous"></PagerSettings>
                                                             <alternatingrowstyle backcolor="#E0E0E0"/>
                                                            <Columns>
                                                                <asp:BoundField DataField="SKU_HIE_ID" HeaderText="Id">
                                                                    <ItemStyle HorizontalAlign="Left" BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"  Width="5%">
                                                                    </ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SKU_HIE_CODE" HeaderText="Code">
                                                                    <ItemStyle HorizontalAlign="Left" BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid" Width="15%">
                                                                    </ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SKU_HIE_NAME" HeaderText="Name">
                                                                    <ItemStyle HorizontalAlign="Left" BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid" Width="60%">
                                                                    </ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                              <asp:TemplateField HeaderText="Edit">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text="Delete">
                                                                    <img src="../images/edit.gif" />
                                                                                </asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" Width="45px" HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                           
                                                                 <asp:TemplateField HeaderText="Delete" >
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="btnDelete" ToolTip="Delete" runat="server" Text="Delete" OnClientClick="javascript:return confirm('Are you sure you want to Delete?');return false;"
                                                                                    CommandName="Delete">
                                                                                    <img src="../images/delete.gif" width="16" height="16">
                                                                                </asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid" Width="45px" HorizontalAlign="Center">
                                                                            </ItemStyle>
                                                                        </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle CssClass="tblhead"></HeaderStyle>
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </cc1:TabPanel>
                        <cc1:TabPanel ID="TabPanel5" runat="server">
                            <HeaderTemplate>
                                Gender
                            </HeaderTemplate>
                            <ContentTemplate>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 100px">
                                        </td>
                                        <td align="center" style="width: 100px">
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                 <asp:Panel ID="Panel9" runat="server">
                                                    <fieldset>
                                                    <table width="100%">
                                                        <tbody>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:Label ID="lblErrorMsgTag" runat="server" ForeColor="Red" Font-Bold="True" ></asp:Label><br />
                                                                </td>
                                                                <td style="width: 100px">
                                                                </td>
                                                            </tr>
                                                            <%--<tr>
                                                                <td style="height: 25px" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label16" runat="server" Width="80px" Text="Principal" __designer:wfdid="w149"></asp:Label></strong>
                                                                </td>
                                                                <td style="width: 100px">
                                                                    <asp:DropDownList ID="drpTagPrincipal" runat="server" Width="200px" __designer:wfdid="w150"
                                                                        CssClass="DropList" AutoPostBack="True" OnSelectedIndexChanged="drpTagPrincipal_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="width: 100px">
                                                                </td>
                                                            </tr>--%>
                                                          <%--  <tr>
                                                                <td style="height: 21px" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label17" runat="server" Width="64px" Text="Division" __designer:wfdid="w151"></asp:Label></strong>
                                                                </td>
                                                                <td style="height: 25px">
                                                                    <asp:DropDownList ID="DrpTagDivision" runat="server" Width="200px" __designer:wfdid="w152"
                                                                        CssClass="DropList" AutoPostBack="True" OnSelectedIndexChanged="DrpTagDivision_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="width: 100px; height: 21px">
                                                                </td>
                                                            </tr>--%>
                                                          <%--  <tr>
                                                                <td style="height: 25px" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label18" runat="server" Width="80px" Text="Category" __designer:wfdid="w153"></asp:Label></strong>
                                                                </td>
                                                                <td style="width: 100px; height: 28px">
                                                                    <asp:DropDownList ID="DrptagCategory" runat="server" Width="200px" __designer:wfdid="w154"
                                                                        CssClass="DropList" AutoPostBack="True" OnSelectedIndexChanged="DrptagCategory_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="width: 100px; height: 28px">
                                                                </td>
                                                            </tr>--%>
                                                            <%--<tr>
                                                                <td style="height: 25px" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label22" runat="server" Width="80px" Text="Type" __designer:wfdid="w153"></asp:Label></strong>
                                                                </td>
                                                                <td style="width: 100px; height: 28px">
                                                                    <asp:DropDownList ID="drpTagType" runat="server" Width="200px" __designer:wfdid="w154"
                                                                        CssClass="DropList" AutoPostBack="True" OnSelectedIndexChanged="drpTagType_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="width: 100px; height: 28px">
                                                                </td>
                                                            </tr>--%>
                                                            <tr>
                                                             <td style="width: 100px; height: 28px">
                                                                </td>
                                                                <td style="width: 80px; height: 28px" align="left">
                                                                    <strong>
                                                                        Code</strong>
                                                                </td>
                                                                <td style="width: 100px; height: 28px">
                                                                    <asp:TextBox ID="txtTagCode" runat="server" Width="194px" 
                                                                        Enabled="False" CssClass="txtBox "></asp:TextBox>
                                                                </td>
                                                                <td style="width: 100px; height: 28px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                             <td style="width: 100px; height: 28px">
                                                                </td>
                                                                <td style="width: 80px; height: 29px" align="left">
                                                                    <strong>
                                                                        Name</strong>
                                                                </td>
                                                                <td style="width: 100px; height: 29px">
                                                                    <asp:TextBox ID="txtTagName" runat="server" Width="194px" 
                                                                        Enabled="False" CssClass="txtBox "></asp:TextBox>
                                                                </td>
                                                                <td style="width: 100px; height: 29px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                             <td style="width: 100px; height: 37px">
                                                                </td>
                                                                <td style="width: 80px; height: 37px">
                                                                </td>
                                                                <td style="width: 100px; height: 37px" align="left">
                                                                    <asp:Button AccessKey="B" ID="btnSaveTag" OnClick="btnSaveTag_Click" runat="server"
                                                                        Width="85px" Font-Size="8pt" Text="New" CssClass="Button" />
                                                                </td>
                                                                <td style="width: 100px; height: 37px">
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                    </fieldset>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="UpdatePanel3">
                                                <ProgressTemplate>
                                                    <asp:Image ID="Image4" runat="server" Width="24px" Height="23px" 
                                                        ImageUrl="~/App_Themes/Granite/Images/image003.gif"></asp:Image>&nbsp; Loading.....
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </td>
                                        <td style="width: 100px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="3">
                                            <asp:Panel ID="Panel5" runat="server" Height="200px" ScrollBars="Vertical" Width="100%">
                                                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="grdTag" runat="server" Width="100%" ForeColor="SteelBlue" Font-Size="9pt"
                                                             BorderColor="SteelBlue" BackColor="White"
                                                            HorizontalAlign="Center" AutoGenerateColumns="False" OnRowEditing="grdTag_RowEditing"
                                                            OnRowDeleting="grdTag_RowDeleting" OnPageIndexChanging="grdTag_PageIndexChanging">
                                                            <PagerSettings FirstPageText="" LastPageText="" Mode="NextPrevious" NextPageText="Next"
                                                                PreviousPageText="Previous"></PagerSettings>
                                                             <alternatingrowstyle backcolor="#E0E0E0"/>
                                                            <Columns>
                                                                <asp:BoundField DataField="SKU_HIE_ID" HeaderText="Id" >
                                                                    <ItemStyle HorizontalAlign="Left" BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid" Width="5%" >
                                                                    </ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SKU_HIE_CODE" HeaderText="Code">
                                                                    <ItemStyle HorizontalAlign="Left" BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid" Width="15%" >
                                                                    </ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SKU_HIE_NAME" HeaderText="Name">
                                                                    <ItemStyle HorizontalAlign="Left" BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid" Width="60%" >
                                                                    </ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Edit">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text="Delete">
                                                                    <img src="../images/edit.gif" />
                                                                                </asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" Width="45px" HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                           
                                                                 <asp:TemplateField HeaderText="Delete" >
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="btnDelete" ToolTip="Delete" runat="server" Text="Delete" OnClientClick="javascript:return confirm('Are you sure you want to Delete?');return false;"
                                                                                    CommandName="Delete">
                                                                                    <img src="../images/delete.gif" width="16" height="16">
                                                                                </asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid" Width="45px" HorizontalAlign="Center">
                                                                            </ItemStyle>
                                                                        </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle CssClass="tblhead"></HeaderStyle>
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </cc1:TabPanel>
                    </cc1:TabContainer>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
