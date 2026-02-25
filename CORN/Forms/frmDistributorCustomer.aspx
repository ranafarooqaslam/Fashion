<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmDistributorCustomer.aspx.cs" Inherits="Forms_frmDistributorCustomer"
    Title="CORN :: New Customer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
<script language="JavaScript" type="text/javascript">
   
        function NameValidation(event) {
            // Allow: backspace, delete, tab and escape
            if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 32 ||
            // Allow: Ctrl+A
            (event.keyCode == 65 && event.ctrlKey === true) ||
            // Allow: home, end, left, right
            (event.keyCode >= 35 && event.keyCode <= 39) ||
            // Allow: Dash, Underscoor
            (event.keyCode == 189) ||
            //Allow Comma,Period
            ((event.keyCode == 190 || event.keyCode == 188) && event.shiftKey === false) ||
            //Allow a-z
            (event.keyCode >= 65 && event.keyCode <= 90)) {
                // let it happen, don't do anything
                return;
            }
            else {
                // Ensure that it is a number and stop the keypress
                event.preventDefault();
            }
        }

        function AddressValidation(event) {
            // Allow: backspace, delete, tab , escape and space bar
            if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 32 ||
            // Allow: Ctrl+A
            (event.keyCode == 65 && event.ctrlKey === true) ||
            // Allow: home, end, left, right
            (event.keyCode >= 35 && event.keyCode <= 39) ||
            // Allow: Dash, Underscoor
            (event.keyCode == 189) ||
            // Allow: Open bracket, Close bracket
            ((event.keyCode == 57 || event.keyCode == 48) && event.shiftKey === true) ||
            //Allow Comma,Period
            ((event.keyCode == 190 || event.keyCode == 188) && event.shiftKey === false) ||
            //Allow 0-9
            ((event.keyCode >= 48 && event.keyCode <= 57) && event.shiftKey === false) || //Standard Numbers
            (event.keyCode >= 96 && event.keyCode <= 105) || //Keypad numbers
            //Allow a-z
            (event.keyCode >= 65 && event.keyCode <= 90)) {
                // let it happen, don't do anything
                return;
            }
            else {
                // Ensure that it is a number and stop the keypress
                event.preventDefault();
            }
        }

        function PhoneValidation(event) {
            // Allow: backspace, delete, tab , escape
            if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 ||
            // Allow: Ctrl+A
            (event.keyCode == 65 && event.ctrlKey === true) ||
            // Allow: home, end, left, right
            (event.keyCode >= 35 && event.keyCode <= 39) ||
            // Allow: Dash
            (event.keyCode == 189 && event.shiftKey === false) ||
            //Allow 0-9
            ((event.keyCode >= 48 && event.keyCode <= 57) && event.shiftKey === false) || //Standard Numbers
            //Keypad numbers
            (event.keyCode >= 96 && event.keyCode <= 105)) {
                // let it happen, don't do anything
                return;
            }
            else {
                // Ensure that it is a number and stop the keypress
                event.preventDefault();
            }
        }

        function pageLoad() {
            $("select").searchable();
            $('#<%=txtCNIC.ClientID %>').mask("99999-9999999-9");
           
          

            $('#<%=Grid_users.ClientID %>').tablesorter(
	     {
	         headers: {
	             25: {
	                 sorter: false
	             }
	         }
	     }
	     );

        }
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(startRequest);

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);

        function startRequest(sender, e) {
            document.getElementById('<%=btnSave.ClientID%>').disabled = true;
            document.getElementById('<%=btnSearch.ClientID%>').disabled = true;
            document.getElementById('<%=btnCancel.ClientID%>').disabled = true;

        }

        function endRequest(sender, e) {

            document.getElementById('<%=btnSave.ClientID%>').disabled = false;
            document.getElementById('<%=btnSearch.ClientID%>').disabled = false;
            document.getElementById('<%=btnCancel.ClientID%>').disabled = false;
        }


        function ValidateForm() {
            var str;
            str = document.getElementById('<%=txtCustomerName.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Must enter Customer Name');
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
            str = document.getElementById('<%=txtRegdate.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Must enter Register Date');
                return false;
            }

            str = document.getElementById('<%=txtCNIC.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Must enter CNIC #');
                return false;
            }



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
        <div>
            <table width="100%">
                <tr>
                    <td style="width: 100px">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tbody>
                                        <tr>
                                            <td style="width: 143px" align="left">
                                            </td>
                                            <td style="width: 175px">
                                                <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                            </td>
                                            <td style="width: 1px">
                                            </td>
                                            <td align="left">
                                            </td>
                                            <td style="width: 219px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 25px" align="left">
                                                <strong>
                                                    <asp:Label ID="Label7" runat="server" Width="77px" Text="Location" CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="width: 175px">
                                                <asp:DropDownList ID="DrpDistributor" runat="server" Width="205px" CssClass="DropList"
                                                    OnSelectedIndexChanged="DrpDistributor_SelectedIndexChanged" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 30px">
                                            </td>
                                            <td align="left">
                                                <strong>
                                                    <asp:Label ID="Label3" runat="server" Width="110px" Text="Promotion Class" CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="width: 219px">
                                                <asp:DropDownList ID="DrpVolumeClass" runat="server" Width="205px" CssClass="DropList">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 26px" align="left">
                                                <strong>
                                                    <asp:Label ID="Label1" runat="server" Width="77px" Text="Name" CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="width: 175px; height: 26px">
                                                <asp:TextBox ID="txtCustomerName" runat="server" Width="205px" CssClass="txtBox"
                                                    MaxLength="100"></asp:TextBox>
                                            </td>
                                            <td style="width: 1px; height: 26px" valign="top">
                                            </td>
                                            <td align="left">
                                                <strong>
                                                    <asp:Label ID="lblAddress1" runat="server" Width="77px" Text="Address " CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="width: 219px; height: 26px">
                                                <asp:TextBox ID="txtAddress" runat="server" Width="205px" CssClass="txtBox " MaxLength="250"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 26px" align="left">
                                                <strong>
                                                    <asp:Label ID="lblPhNo" runat="server" Width="77px" Text="Phone No" CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="width: 175px; height: 26px">
                                                <asp:TextBox ID="txtPhoneNo" runat="server" Width="205px" CssClass="txtBox" MaxLength="15"></asp:TextBox>
                                                <asp:TextBox ID="TextBox1" runat="server" Width="205px" CssClass="txtBox" MaxLength="15"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Custom"
                                    ValidChars="0123456789" TargetControlID="txtPhoneNo" />
                                            </td>
                                            <td style="width: 1px; height: 26px" valign="top">
                                            </td>
                                            <td align="left">
                                                <strong>
                                                    <asp:Label ID="lbldesignationID" runat="server" Width="77px" Text="Date Of Birth"
                                                        CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="width: 219px; height: 26px">
                                                <asp:TextBox ID="txtRegdate" runat="server" Width="185px" CssClass="txtBox" Enabled="false"></asp:TextBox>
                                                <asp:ImageButton ID="ibndob" runat="server" Width="16px" ImageUrl="~/App_Themes/Granite/Images/date.gif">
                                                </asp:ImageButton>
                                                <%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
                                                <cc1:CalendarExtender ID="CEStartDate" runat="server" TargetControlID="txtRegdate"
                                                    PopupButtonID="ibndob" Format="dd-MMM-yyyy">
                                                </cc1:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 26px" align="left">
                                                <strong>
                                                    <asp:Label ID="Label8" runat="server" Width="77px" Text="CNIC #" CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="width: 175px; height: 26px">
                                                <asp:TextBox ID="txtCNIC" runat="server" Width="205px" CssClass="txtBox"></asp:TextBox>
                                            </td>
                                            <td style="width: 1px; height: 26px" valign="top">
                                            </td>
                                            <td align="left">
                                                <strong>
                                                    <asp:Label ID="Label2" runat="server" Width="110px" Text="Email Address" CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="width: 219px; height: 26px">
                                                <asp:TextBox ID="txtEmailAddress" runat="server" Width="205px" CssClass="txtBox"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 26px" align="left">
                                                <strong>Credit Limit </strong>
                                            </td>
                                            <td style="width: 175px; height: 26px">
                                                <asp:TextBox ID="txtCreditLimit" runat="server" Width="205px" CssClass="txtBox"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="ftbCreditLimit" runat="server" TargetControlID="txtCreditLimit"
                                                    ValidChars=".0123456789">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td style="width: 1px; height: 26px" valign="top">
                                            </td>
                                            <td align="left">
                                            </td>
                                            <td style="width: 219px; height: 26px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 26px" align="left">
                                            </td>
                                            <td style="width: 175px; height: 26px">
                                            </td>
                                            <td style="width: 1px; height: 26px" valign="top">
                                            </td>
                                            <td align="left">
                                            </td>
                                            <td style="width: 219px; height: 26px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="5">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 143px">
                                            </td>
                                            <td style="width: 175px">
                                                <asp:Button ID="btnSave" runat="server" CssClass="Button" Font-Size="8pt" OnClick="btnSave_Click"
                                                    Text="Save" ValidationGroup="vg" Width="80px" />
                                                &nbsp;
                                                <asp:Button ID="btnCancel" runat="server" CssClass="Button" Font-Size="8pt" OnClick="btnCancel_Click"
                                                    Text="Cancel" Width="80px" />
                                            </td>
                                            <td style="width: 1px">
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkIsActive" runat="server" Checked="True" Text="IsActive" Width="93px" />
                                            </td>
                                            <td style="width: 219px">
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div style="z-index: 101; left: 640px; width: 100px; position: absolute; top: 244px;
                            height: 100px">
                            &nbsp;<asp:Panel ID="Panel21" runat="server">
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
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
        </div>
        <div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table style="border-right: silver thin inset; border-top: silver thin inset; border-left: silver thin inset;
                        width: 100%; border-bottom: silver thin inset; background-color: silver">
                        <tbody>
                            <tr>
                                <td style="height: 21px; width: 15%" align="left">
                                    <asp:Label ID="Label10" runat="server" Width="153px" Text="Select Searching Type"></asp:Label>
                                </td>
                                <td style="width: 170px; height: 21px" align="left">
                                    <asp:DropDownList ID="ddSearchType" runat="server" Width="200px" CssClass="DropList">
                                        <asp:ListItem Value="CUSTOMER_CODE">Customer Code</asp:ListItem>
                                        <asp:ListItem Value="CUSTOMER_NAME">Customer Name</asp:ListItem>
                                        <asp:ListItem Value="CONTACT_NUMBER">Contact Number</asp:ListItem>
                                        <asp:ListItem Value="ADDRESS">Address</asp:ListItem>
                                        <asp:ListItem Value="EMAIL_ADDRESS">Email Address</asp:ListItem>
                                        <asp:ListItem>CNIC</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 20% height: 21px" align="left">
                                    <asp:TextBox ID="txtSeach" runat="server" Width="180px" CssClass="txtBox "></asp:TextBox>
                                </td>
                                <td style="height: 21px; width: 60%" align="left">
                                    <asp:Button ID="btnSearch" runat="server" Width="85px" Font-Size="8pt" Text="Filter"
                                        OnClick="btnSearch_Click"></asp:Button>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <asp:Panel ID="Panel2" runat="server" Width="100%" Height="200px" ScrollBars="Vertical"
                        BorderStyle="Solid" BorderWidth="1px" BorderColor="Silver">
                        <asp:GridView ID="Grid_users" runat="server" Width="100%" ForeColor="SteelBlue" HorizontalAlign="Center"
                            AutoGenerateColumns="False" BackColor="White" BorderColor="White" OnRowEditing="Grid_users_RowEditing">
                            <RowStyle ForeColor="Black"></RowStyle>
                            <Columns>
                                <asp:BoundField DataField="CUSTOMER_ID" HeaderText="Customer Id">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DISTRIBUTOR_ID" HeaderText="DISTRIBUTOR_ID">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="PROMOTION_CLASS" HeaderText="PROMOTION_CLASS">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="CUSTOMER_CODE" HeaderText="Code">
                                    <ItemStyle CssClass="grdDetail" Width="10%"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="CUSTOMER_NAME" HeaderText="Name">
                                    <ItemStyle CssClass="grdDetail" Width="25%"></ItemStyle>
                                    <HeaderStyle CssClass="grdHead" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CONTACT_NUMBER" HeaderText="Contact Number">
                                    <ItemStyle CssClass="grdDetail" Width="15%"></ItemStyle>
                                    <HeaderStyle CssClass="grdHead" />
                                </asp:BoundField>
                                <asp:BoundField DataField="EMAIL_ADDRESS" HeaderText="Email">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="ADDRESS" HeaderText="Address">
                                    <ItemStyle CssClass="grdDetail" Width="30%"></ItemStyle>
                                    <HeaderStyle CssClass="grdHead" />
                                </asp:BoundField>
                                <asp:BoundField DataField="REGDATE">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="CNIC">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="creditLimit" HeaderText="Credit Limit">
                                    <ItemStyle CssClass="grdDetail" HorizontalAlign="Right" Width="8%"></ItemStyle>
                                    <HeaderStyle CssClass="grdHead" />
                                </asp:BoundField>
                                <asp:BoundField DataField="IS_ACTIVE" HeaderText="Status">
                                    <ItemStyle CssClass="grdDetail" Width="7%"></ItemStyle>
                                    <HeaderStyle CssClass="grdHead" />
                                </asp:BoundField>
                                 <asp:TemplateField HeaderText="Edit" >
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit"                                                              Text="Delete">
                                                                    <img src="../images/edit.gif" />
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" Width="5%" HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="grdHead"></HeaderStyle>
                            <AlternatingRowStyle BackColor="#F2F2F2" CssClass="GridAlternateRowStyle" ForeColor="#333333">
                            </AlternatingRowStyle>
                        </asp:GridView>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
