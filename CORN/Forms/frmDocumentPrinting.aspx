<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" CodeFile="frmDocumentPrinting.aspx.cs"
 Inherits="Forms_frmDocumentPrinting" Title="CORN :: Print Sale Document" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
        <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
<script language="JavaScript" type="text/javascript">
    function pageLoad() {
        $("select").searchable();
    }
 
    function ValidateForm()
	{
			
		return true;	  		
	}

    </script>
    <div id="right_data">
     <div >
        <table width="100%">
            <tr>
                <td >
                    <asp:UpdatePanel id="UpdatePanel1" runat="server">
                        <contenttemplate>
<TABLE><TBODY><TR><TD align=left colSpan=1></TD><TD align=left colSpan=4>
<asp:Label id="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label> </TD></TR><TR><TD style="HEIGHT: 26px" align=left colSpan=1></TD><TD style="HEIGHT: 26px" align=left colSpan=1></TD>
        <TD style="HEIGHT: 26px" align=left colSpan=3></TD></TR><TR><TD style="HEIGHT: 23px" align=left colSpan=1></TD>
    <TD style="HEIGHT: 23px" align=left colSpan=4><DIV id="divFilter" class="containeRadioButtons"><TABLE width="100%"><TBODY><TR><TD align=left><asp:RadioButtonList id="rblCustomerType" runat="server" Width="300px" RepeatDirection="Horizontal"><asp:ListItem Selected="True" Value="-1">All</asp:ListItem>
<asp:ListItem Value="1">Registered</asp:ListItem>
<asp:ListItem Value="0">Unregistered</asp:ListItem>
</asp:RadioButtonList></TD></TR></TBODY></TABLE></DIV></TD></TR>
<TR><TD align=left></TD><TD align=left></TD><TD align=left>
<strong><asp:Label id="Label2" runat="server" Width="95px" Text="Document Type" CssClass="lblbox"></asp:Label></strong></TD><TD align=left></TD><TD style="HEIGHT: 25px" align=left><asp:DropDownList id="DrpLedgerType" runat="server" Width="200px" CssClass="DropList" OnSelectedIndexChanged="DrpLedgerType_SelectedIndexChanged" AutoPostBack="True">
            <asp:ListItem Value="0">Order </asp:ListItem>
            <asp:ListItem Value="1">Invoice</asp:ListItem>
            <asp:ListItem Value="2">Sale Return</asp:ListItem>
            <asp:ListItem Value="3">Delivery Challan</asp:ListItem>
            <asp:ListItem Value="4">Invoice USD</asp:ListItem>
</asp:DropDownList></TD></TR><TR><TD align=left></TD><TD align=left></TD><TD align=left>
<strong><asp:Label id="lbltoLocation" runat="server" Width="94px" Text="Location" CssClass="lblbox"></asp:Label></strong></TD><TD align=left></TD><TD style="HEIGHT: 25px" align=left><asp:DropDownList id="drpDistributor" runat="server" Width="200px" CssClass="DropList" OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged" AutoPostBack="True">
    </asp:DropDownList></TD></TR><TR><TD align=left></TD><TD align=left></TD><TD align=left>
    <strong><asp:Label id="Label6" runat="server" Width="78px" Text="Principal" CssClass="lblbox"></asp:Label></strong></TD><TD align=left></TD><TD style="HEIGHT: 25px" align=left><asp:DropDownList id="DrpPrincipal" runat="server" Width="200px" CssClass="DropList" OnSelectedIndexChanged="DrpPrincipal_SelectedIndexChanged" AutoPostBack="True">
            </asp:DropDownList></TD></TR><TR><TD align=left></TD><TD align=left></TD><TD align=left>
            <strong><asp:Label id="Label1" runat="server" Width="78px" Text="Sale Force" CssClass="lblbox"></asp:Label></strong></TD><TD align=left></TD><TD style="HEIGHT: 25px" align=left><asp:DropDownList id="DrpArea" runat="server" Width="200px" CssClass="DropList">
            </asp:DropDownList></TD></TR><TR><TD align=left></TD><TD align=left></TD><TD align=left>
            <strong><asp:Label id="lblfromLocation" runat="server" Width="94px" Text="Customer Route" CssClass="lblbox"></asp:Label></strong></TD><TD align=left></TD><TD style="HEIGHT: 25px" align=left><asp:DropDownList id="DrpRoute" runat="server" Width="200px" CssClass="DropList" OnSelectedIndexChanged="DrpRoute_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList></TD></TR><TR><TD align=left></TD><TD align=left></TD><TD align=left>
            <strong><asp:Label id="Label5" runat="server" Width="94px" Text="Customer" CssClass="lblbox"></asp:Label></strong></TD><TD align=left></TD><TD style="HEIGHT: 25px" align=left><asp:DropDownList id="DrpCustomer" runat="server" Width="200px" CssClass="DropList">
                </asp:DropDownList></TD></TR><TR>
        <td align="left">
        </td>
        <td align="left">
        </td>
        <td align="left">
            <strong>
            <asp:Label ID="Label3" runat="server" Height="13px" Text="From Date" 
                Width="90px"></asp:Label>
            </strong>
        </td>
        <td align="left">
        </td>
        <td align="left" style="HEIGHT: 25px">
            &nbsp;<asp:TextBox ID="txtStartDate" runat="server" CssClass="txtBox" MaxLength="10" Enabled="false"
                onkeyup="BlockStartDateKeyPress()" Width="150px"></asp:TextBox>
            <asp:ImageButton ID="ibtnStartDate" runat="server" 
                ImageUrl="~/App_Themes/Granite/Images/date.gif" Width="16px" />
        </td>
    </TR><TR>
        <td align="left">
        </td>
        <td align="left">
        </td>
        <td align="left">
            <strong>
            <asp:Label ID="Label4" runat="server" Height="13px" Text="To Date" Width="80px"></asp:Label>
            </strong>
        </td>
        <td align="left">
        </td>
        <td align="left" style="HEIGHT: 25px">
            &nbsp;<asp:TextBox ID="txtEndDate" runat="server" CssClass="txtBox " MaxLength="10"  Enabled="false"
                onkeyup="BlockEndDateKeyPress()" Width="150px"></asp:TextBox>
            <asp:ImageButton ID="ibnEndDate" runat="server" 
                ImageUrl="~/App_Themes/Granite/Images/date.gif" Width="16px" />
        </td>
    </TR><TR><TD <%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
            <cc1:CalendarExtender ID="CEStartDate" runat="server" Format="dd-MMM-yyyy"
                PopupButtonID="ibtnStartDate" TargetControlID="txtStartDate">
            </cc1:CalendarExtender>
            <cc1:CalendarExtender ID="CEEndDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibnEndDate"
                TargetControlID="txtEndDate">
            </cc1:CalendarExtender></TD></TR>
    <tr>
        <td align="left" colspan="5">
            <div ID="divSort" class="container2">
                <table width="100%">
                    <tbody>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <strong>
                                <asp:Label ID="lblSort" runat="server" CssClass="lblbox" Text="Sort By" 
                                    Width="78px"></asp:Label>
                                </strong>
                            </td>
                            <td>
                            </td>
                            <td align="right">
                                <asp:DropDownList ID="DrpSort" runat="server" CssClass="DropList" Width="240px">
                                    <asp:ListItem Value="0">Document No</asp:ListItem>
                                    <asp:ListItem Value="1">Customer Code</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <strong>
                                <asp:Label ID="lblSortOrder" runat="server" CssClass="lblbox" Text="Sort Order" 
                                    Width="78px"></asp:Label>
                                </strong>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rbtSortOrder" runat="server" 
                                    RepeatDirection="Horizontal" Width="192px">
                                    <asp:ListItem Selected="True" Text="Ascending"></asp:ListItem>
                                    <asp:ListItem Text="Descending"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                &nbsp;</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </td>
    </tr>
    </TBODY></TABLE>
</contenttemplate>
                    </asp:UpdatePanel>
                    &nbsp; &nbsp;
        <asp:Button ID="btnViewPDF" runat="server" Width="90" Text="View PDF" OnClick="btnViewPDF_Click" CssClass="Button" />
        <asp:Button ID="btnViewExcel" runat="server" Width="90" Text="View Excel" OnClick="btnViewExcel_Click" CssClass="Button" /></td>
            </tr>
        </table>
         &nbsp;
        
           </div>
    </div>
</asp:Content>
