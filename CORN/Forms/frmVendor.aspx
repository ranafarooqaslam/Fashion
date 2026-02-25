<%@ Page Title="SAMS :: Add Vendor" Language="C#" MasterPageFile="~/Forms/PageMaster.master"
    AutoEventWireup="true" CodeFile="frmVendor.aspx.cs" Inherits="Forms_frmVendor" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
           <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
<script language="JavaScript" type="text/javascript">
    function pageLoad() {
        $("select").searchable();
    }
        function ValidateForm() {
            var str;
            str = document.getElementById('<%=txtVendorName.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Must enter Customer Name');
                return false;
            }
            str = document.getElementById('<%=txtContactPerson.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Must enter Contact Person Name');
                return false;
            }
            str = document.getElementById('<%=txtAddress.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Must enter Address');
                return false;
            }


            str = document.getElementById('<%=txtPhoneNo.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Must enter Phone No');
                return false;
            }

            return true;
        }
        function SearchRecord() {
            var str;
            str = document.getElementById('<%=txtSeach.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Must enter Key Word for Searching');
                return false;
            }
            return true;
        }
	
    </script>
    <div id="right_data">
        <table width="100%">
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <table>
                                <tbody>
                                    <tr>
                                        <td style="width: 100px" align="left">
                                        </td>
                                        <td style="width: 175px">
                                            <strong>
                                                <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label></strong>
                                        </td>
                                        <td style="width: 1px">
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td style="width: 219px">
                                        </td>
                                    </tr>
                                   
                                    <tr>
                                        <td style="height: 26px" align="left">
                                            <strong>
                                                Vendor Name</strong>
                                        </td>
                                        <td style="width: 175px; height: 26px">
                                            <asp:TextBox ID="txtVendorName" runat="server" Width="200px" CssClass="txtBox"></asp:TextBox>
                                        </td>
                                        <td style="width: 1px; height: 26px" valign="top">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 25px" align="left">
                                            <strong>
                                                Address </strong>
                                        </td>
                                        <td style="width: 175px">
                                            <asp:TextBox ID="txtAddress" runat="server" Width="200px" CssClass="txtBox " MaxLength="255"></asp:TextBox>
                                        </td>
                                        <td style="width: 1px">
                                        </td>
                                       
                                    </tr>
                                    <tr>
                                        <td style="height: 25px" align="left">
                                            <strong>
                                               Contact Person</strong>
                                        </td>
                                        <td style="width: 175px; height: 25px">
                                            <asp:TextBox ID="txtContactPerson" runat="server" Width="200px" CssClass="txtBox"></asp:TextBox>
                                        </td>
                                        <td style="width: 1px; height: 25px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 25px" align="left">
                                            <strong>
                                                Phone No</strong>
                                        </td>
                                        <td style="width: 175px; height: 25px">
                                            <asp:TextBox ID="txtPhoneNo" runat="server" Width="200px" CssClass="txtBox "></asp:TextBox>
                                        </td>
                                        <td style="width: 1px; height: 25px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 25px" align="left">
                                            <strong>
                                                Email Address</strong>
                                        </td>
                                        <td style="width: 175px; height: 25px">
                                            <asp:TextBox ID="txtemail" runat="server" Width="200px" CssClass="txtBox "></asp:TextBox>
                                        </td>
                                        <td style="width: 1px; height: 25px">
                                        </td>
                                        <tr>
                                            <td style="height: 25px" align="left">
                                                <strong>
                                                   Fax</strong>
                                            </td>
                                            <td style="width: 175px; height: 25px">
                                                <asp:TextBox ID="txtFax" runat="server" Width="200px" CssClass="txtBox "></asp:TextBox>
                                            </td>
                                            <td style="width: 1px; height: 25px">
                                            </td>
                                        </tr>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px" align="left">
                                        </td>
                                        <td style="width: 175px">
                                            <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Width="80px" Font-Size="8pt"
                                                Text="Save" ValidationGroup="vg" CssClass="Button" />&nbsp;
                                        </td>
                                        <td style="width: 1px">
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsActive" runat="server" Width="93px" Text="IsActive" Checked="True">
                                            </asp:CheckBox>
                                        </td>
                                        <td style="width: 219px">
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div style="z-index: 101; left: 790px; width: 100px; position: absolute; top: 250px;
                        height: 100px">
                        &nbsp;<asp:Panel ID="Panel21" runat="server">
                            <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                <ProgressTemplate>
                                    <asp:ImageButton ID="ImageButton1" runat="server" Height="26px" ImageUrl="~/App_Themes/Granite/Images/image003.gif"
                                        Width="23px" />
                                    Wait Update
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </asp:Panel>
                    </div>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table style="border-right: silver thin inset; border-top: silver thin inset; border-left: silver thin inset;
                    width: 98%; border-bottom: silver thin inset; background-color: silver">
                    <tbody>
                        <tr>
                            <td style="height: 21px" align="left">
                                <strong>
                                    <asp:Label ID="Label10" runat="server" Width="153px" Text="Select Searching Type"></asp:Label></strong>
                            </td>
                            <td style="width: 170px; height: 21px" align="left">
                                <asp:DropDownList ID="ddSearchType" runat="server" Width="200px" CssClass="DropList">
                                    <asp:ListItem Value="VENDOR_ID">All Records</asp:ListItem>
                                    <asp:ListItem Value="VENDOR_NAME">Vendor Name</asp:ListItem>
                                    <asp:ListItem Value="CONTACT_PERSON">Contact Person</asp:ListItem>
                                    <asp:ListItem Value="CONTACT_NO">Contact No</asp:ListItem>
                                    <asp:ListItem Value="ADDRESS1">Address</asp:ListItem>
                                    <asp:ListItem Value="ADDRESS2">Email Address</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 224px; height: 21px" align="left">
                                <asp:TextBox ID="txtSeach" runat="server" Width="200px" CssClass="txtBox "></asp:TextBox>
                            </td>
                            <td style="height: 21px" align="left" width="250">
                                <asp:Button ID="btnSearch" OnClick="btnSearch_Click" runat="server" Width="85px"
                                    Font-Size="8pt" Text="Filter" CssClass="btn" />
                            </td>
                        </tr>
                    </tbody>
                </table>
                <asp:Panel ID="Panel2" runat="server" Height="200px" ScrollBars="Vertical" Width="98%" 
                BorderStyle="Solid" BorderWidth="1px" BorderColor="Silver">
                    <asp:GridView ID="gvVendor" runat="server" Width="100%" ForeColor="SteelBlue" CssClass="gridRow2"
                        OnRowEditing="gvVendor_RowEditing" BorderColor="White" BackColor="White" AutoGenerateColumns="False"
                        HorizontalAlign="Center">
                        <RowStyle ForeColor="Black"></RowStyle>
                        <Columns>
                            
                            <asp:BoundField DataField="VENDOR_ID">
                                <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                <ItemStyle CssClass="HidePanel"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="VENDOR_NAME" HeaderText="Vendor Name">
                                <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid" Width="20%"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="CONTACT_PERSON" HeaderText="Contact Person" >
                                <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid" Width="15%">
                                </ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="CONTACT_NO" HeaderText="Contact Number" >
                                <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid" Width="15%"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ADDRESS1" HeaderText="Address" >
                                <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid" Width="25%"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ADDRESS2" HeaderText="Email" >
                                <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid" Width="15%"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ADDRESS3">
                                <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                <ItemStyle CssClass="HidePanel"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="IS_ACTIVE" HeaderText="Status">
                                <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid" Width="5%"></ItemStyle>
                            </asp:BoundField>
                            <asp:CommandField ShowEditButton="True" >
                                <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid" HorizontalAlign="Center" Width="5%"></ItemStyle>
                            </asp:CommandField>
                        </Columns>
                        <FooterStyle BackColor="White"></FooterStyle>
                        <PagerStyle BackColor="Transparent"></PagerStyle>
                        <HeaderStyle CssClass="tblhead" ForeColor="White"></HeaderStyle>
                        <AlternatingRowStyle BackColor="#F2F2F2" CssClass="GridAlternateRowStyle" ForeColor="#333333">
                        </AlternatingRowStyle>
                    </asp:GridView>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
