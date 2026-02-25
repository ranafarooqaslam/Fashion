<%@ Page Language="C#" MasterPageFile="~/Forms/masterPOS.master" AutoEventWireup="true"
    CodeFile="frmNewCustomerPOS.aspx.cs" Inherits="Forms_frmNewCustomerPOS" Title="CORN :: New Customer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <link href="../css/POSstyle.css" rel="stylesheet" type="text/css" />
    <asp:ScriptManager ID="MainScriptManager" runat="server" AsyncPostBackTimeout="300"
        EnablePartialRendering="true" />
        <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
<script language="JavaScript" type="text/javascript">
    function pageLoad() {
        $("select").searchable();
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
        <div class="menu2" style="width: 60.66%; margin-left: 300px">
            <div class="main">
                <ul>
                    <li>Customer Information</li>
                </ul>
            </div>
        </div>
        <div style="margin-left: 300px;">
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
                                    <%--  <strong>
                                                    <asp:Label ID="Label4" runat="server" Width="56px" Text="Town" CssClass="lblbox"></asp:Label></strong>--%>
                                    <strong>
                                        <asp:Label ID="Label3" runat="server" Width="120px" Text="Customer Group" CssClass="lblbox"></asp:Label></strong>
                                </td>
                                <td style="width: 219px">
                                    <asp:DropDownList ID="DrpVolumeClass" runat="server" Width="205px" CssClass="DropList">
                                    </asp:DropDownList>
                                    <%--  <asp:DropDownList ID="DrpTown" runat="server" Width="205px" CssClass="DropList" OnSelectedIndexChanged="DrpTown_SelectedIndexChanged"
                                                    AutoPostBack="True">
                                                </asp:DropDownList>--%>
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
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Custom"
                                    ValidChars="0123456789" TargetControlID="txtPhoneNo" />
                                </td>
                                <td style="width: 1px; height: 26px" valign="top">
                                </td>
                                <td align="left">
                                    <strong>
                                        <asp:Label ID="lbldesignationID" runat="server" Width="100px" Text="Date Of Birth"
                                            CssClass="lblbox"></asp:Label></strong>
                                </td>
                                <td style="width: 219px; height: 26px">
                                    <asp:TextBox ID="txtRegdate" runat="server" Width="185px" CssClass="txtBox"></asp:TextBox>
                                    <asp:ImageButton ID="ibndob" runat="server" Width="16px" ImageUrl="~/App_Themes/Granite/Images/date.gif">
                                    </asp:ImageButton>
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
                                    <%--<strong>
                                                    <asp:Label ID="lblNTN" runat="server" Text="NTN #" CssClass="lblbox"></asp:Label></strong>--%>
                                    <strong>
                                        <asp:Label ID="Label2" runat="server" Width="110px" Text="Email Address" CssClass="lblbox"></asp:Label></strong>
                                </td>
                                <td style="width: 219px; height: 26px">
                                    <%--  <asp:TextBox ID="txtNTN" runat="server" Width="200px" CssClass="txtBox"></asp:TextBox>--%>
                                    <asp:TextBox ID="txtEmailAddress" runat="server" Width="205px" CssClass="txtBox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 26px" align="left">
                                <strong>Credit Limilt</strong>
                                </td>
                                <td style="width: 175px; height: 26px">
                                <asp:TextBox ID="txtCreditLimit" runat="server" Width="205px" CssClass="txtBox" Enabled="false"></asp:TextBox>
                                                
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
                                <td style="width: 21px">
                                </td>
                                <td style="width: 175px" colspan="3" align="center">
                                    <asp:Button ID="btnSave" runat="server" CssClass="ButtonPOS" Font-Size="8pt" OnClick="btnSave_Click"
                                        Text="Save" ValidationGroup="vg" Width="100px" />
                                    &nbsp;
                                    <asp:Button ID="btnCancel" runat="server" CssClass="ButtonPOS" Font-Size="8pt" OnClick="btnCancel_Click"
                                        Text="Cancel" Width="100px" />
                                    &nbsp;
                                    <asp:Button ID="btnback" runat="server" CssClass="ButtonPOS" Font-Size="8pt" Text="Back"
                                        Width="100px" OnClick="btnback_Click" />
                                </td>
                                <td style="width: 219px">
                                    <asp:CheckBox ID="chkIsActive" runat="server" Checked="True" Text="IsActive" Width="93px" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 21px">
                                </td>
                                <td align="left" colspan="5">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    &nbsp;
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <%-- <div style="z-index: 101; left: 640px; width: 100px; position: absolute; top: 244px;
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
            </div>--%>
        <div style="margin-left: 300px; width: 60.66%">
         
                    <table style="border: thin solid silver;" cellpadding="2px">
                        <tbody>
                            <tr>
                                <td style="font-weight: bold; height: 28px">
                                    <asp:Label ID="Label10" runat="server" Width="160px" Text="Select Searching Type"></asp:Label>
                                </td>
                                <td style="width: 170px; height: 22px" align="left">
                                    <asp:DropDownList ID="ddSearchType" runat="server" Width="200px" CssClass="DropList">
                                        <asp:ListItem Value="CUSTOMER_CODE">Customer Code</asp:ListItem>
                                        <asp:ListItem Value="CUSTOMER_NAME">Customer Name</asp:ListItem>
                                        <asp:ListItem Value="CONTACT_NUMBER">Contact Number</asp:ListItem>
                                        <asp:ListItem Value="ADDRESS">Address</asp:ListItem>
                                        <asp:ListItem Value="EMAIL_ADDRESS">Email Address</asp:ListItem>
                                        <asp:ListItem>CNIC</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 224px; height: 22px" align="center">
                                    <asp:TextBox ID="txtSeach" runat="server" Width="200px"></asp:TextBox>
                                </td>
                                <td style="height: 22px" align="left" width="250">
                                    <asp:Button ID="btnSearch" runat="server" Width="85px" Font-Size="8pt" Text="Filter"
                                        OnClick="btnSearch_Click" CssClass="Button"></asp:Button>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <asp:Panel ID="Panel2" runat="server" Width="100%" Height="200px" ScrollBars="Vertical">
                        <asp:GridView ID="Grid_users" runat="server" Width="99.9%" ForeColor="SteelBlue"
                            HorizontalAlign="Center" AutoGenerateColumns="False" BackColor="White" BorderColor="White"
                            OnRowEditing="Grid_users_RowEditing">
                            <Columns>
                                <asp:BoundField DataField="CUSTOMER_ID" HeaderText="Customer Id">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DISTRIBUTOR_ID" HeaderText="DISTRIBUTOR_ID">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="BUSINESS_TYPE_ID" HeaderText="BUSINESS_TYPE_ID">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="PROMOTION_CLASS" HeaderText="PROMOTION_CLASS">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="CHANNEL_TYPE_ID" HeaderText="CHANNEL_TYPE_ID">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="TOWN_ID" HeaderText="TOWN_ID">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="AREA_ID" HeaderText="AREA_ID">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="ROUTE_ID" HeaderText="ROUTE_ID">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="CUSTOMER_CODE" HeaderText="Code">
                                    <HeaderStyle CssClass="grdHead" />
                                    <ItemStyle CssClass="grdDetail"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="CUSTOMER_NAME" HeaderText="Name">
                                    <HeaderStyle CssClass="grdHead" />
                                    <ItemStyle CssClass="grdDetail"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="CONTACT_PERSON" HeaderText="Contact Person">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="CONTACT_NUMBER" HeaderText="Contact Number">
                                    <HeaderStyle CssClass="grdHead" />
                                    <ItemStyle CssClass="grdDetail"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="EMAIL_ADDRESS" HeaderText="Email">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="ADDRESS" HeaderText="ADDRESS">
                                    <HeaderStyle CssClass="grdHead" />
                                    <ItemStyle CssClass="grdDetail"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="GST_NUMBER" HeaderText="Gst No">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="ChannelType" HeaderText="Channel Type">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="GEO_NAME" HeaderText="Town">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="AREA_NAME" HeaderText="Route">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="ROUTE_NAME" HeaderText="Market">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="IS_ACTIVE" HeaderText="Status">
                                    <HeaderStyle CssClass="grdHead" />
                                    <ItemStyle CssClass="grdDetail"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="REGDATE">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="IS_STAND" >
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="IS_COOLER" >
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="CNIC" >
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                
                                 <asp:BoundField DataField="creditLimit" >
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:CommandField ShowEditButton="True" HeaderText="Action">
                                    <HeaderStyle CssClass="grdHead" />
                                    <ItemStyle CssClass="grdDetail" HorizontalAlign="Center"></ItemStyle>
                                </asp:CommandField>
                            </Columns>
                            <HeaderStyle CssClass="grdHead" />
                        </asp:GridView>
                    </asp:Panel>
               
        </div>
    </div>
</asp:Content>
