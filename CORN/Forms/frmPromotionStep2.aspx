<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" CodeFile="frmPromotionStep2.aspx.cs"
    Inherits="Forms_frmPromotionStep2" Title="CORN :: Promotion Wizard Step 1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">
    <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
    <script language="JavaScript" type="text/javascript">
        function pageLoad() {
            $("select").searchable();
        }

        function ValidateForm() {
            var str;

            str = document.getElementById('<%=txtStartDate.ClientID%>').value;
			if (str == null || str.length == 0) {
			    alert('Must select Start Date');
			    return false;
			}
			str = document.getElementById('<%=txtEndDate.ClientID%>').value;
			if (str == null || str.length == 0) {
			    alert('Must select End Date');
			    return false;
			}
			str = document.getElementById('<%=txtPromotionName.ClientID%>').value;
			if (str == null || str.length == 0) {
			    alert('Must enter Promotion Name');
			    return false;
			}
			return true;
        }
        function BlockEndDateKeyPress() {
            document.getElementById('<%=txtEndDate.ClientID%>').value = '';
	    alert('Click Clender Button Select Date');
	}
	function BlockStartDateKeyPress() {
	    document.getElementById('<%=txtStartDate.ClientID%>').value = '';
	    alert('Click Clender Button Select Date');
	}
    </script>
    <div id="right_data">
        <table width="100%">
            <tr>
                <td>
                    <h2>Promotion Wizard Step 1</h2>
                </td>
            </tr>
            <tr>
                <td>

                    <table>
                        <tbody>
                            <tr>
                                <td style="width: 121px"></td>
                                <td style="width: 100px"></td>
                                <td style="width: 100px"></td>
                            </tr>
                            <tr>
                                <td style="width: 121px; height: 22px" align="left">
                                    <asp:RadioButton ID="rBtnExisting" runat="server" AutoPostBack="True" Checked="True"
                                        Font-Names="Verdana" Font-Size="8pt" OnCheckedChanged="rBtnExisting_CheckedChanged"
                                        Text="Existing" Width="72px" /></td>
                                <td align="left">
                                    <asp:DropDownList ID="drpExisting" runat="server" Width="254px" CssClass="DropList">
                                    </asp:DropDownList></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 121px; height: 22px">
                                    <asp:RadioButton ID="rBtnNew" runat="server" AutoPostBack="True" Font-Names="Verdana"
                                        Font-Size="8pt" OnCheckedChanged="rBtnNew_CheckedChanged" Text="New" Width="72px" /></td>
                                <td align="left">
                                    <asp:TextBox ID="txtNew" runat="server" CssClass="txtBox" Enabled="False"
                                        Width="250px" MaxLength="50"></asp:TextBox></td>
                                <td></td>
                            </tr>

                            <asp:Label ID="Label4" runat="server" Height="13px" Text="Promotion For" Width="112px" Visible="false"></asp:Label></strong>
        
            <asp:RadioButtonList ID="rdbbtncheck" runat="server" Font-Names="Verdana" Font-Size="9pt"
                RepeatDirection="Horizontal" Width="231px" Visible="false">
                <asp:ListItem Value="0">Primary Sale</asp:ListItem>
                <asp:ListItem Selected="True" Value="1">Secondary Sale</asp:ListItem>
            </asp:RadioButtonList>

                            <tr style="visibility :hidden">
                                <td align="left" style="width: 121px; height: 22px">
                                    <strong>
                                        <asp:Label ID="Label5" runat="server" Height="13px" Text="Principal" Width="112px"></asp:Label></strong></td>
                                <td align="left">
                                    <asp:DropDownList ID="DrpPrincipal" runat="server" Width="254px" CssClass="DropList">
                                    </asp:DropDownList></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 121px; height: 22px">
                                    <strong>
                                        <asp:Label ID="Label3" runat="server" Height="13px" Text="Promotion" Width="90px"></asp:Label></strong></td>
                                <td align="left">
                                    <asp:TextBox ID="txtPromotionName" runat="server" CssClass="txtBox" Width="250px" MaxLength="50"></asp:TextBox></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 121px; height: 22px"></td>
                                <td align="left">
                                    <asp:TextBox ID="txtPromotionDescription" runat="server" CssClass="txtBox " Height="64px"
                                        MaxLength="255" TextMode="MultiLine" Width="250px"></asp:TextBox></td>
                                <td></td>
                            </tr>

                            <asp:CheckBox ID="chkClaimable" runat="server" Font-Names="Verdana" Font-Size="8pt"
                                Text="Is Claimable Discount" Width="172px" Visible="false" />
                            <tr>
                                <td align="left" style="width: 121px; height: 22px"></td>
                                <td align="left" colspan="2">
                                    <asp:RadioButtonList ID="chkScheme" runat="server" Font-Names="Verdana" Font-Size="9pt"
                                        RepeatDirection="Horizontal" Width="250px">
                                        <asp:ListItem Selected="True" Value="0">Discount</asp:ListItem>
                                        <asp:ListItem Value="1">UpSell</asp:ListItem>
                                    </asp:RadioButtonList></td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 121px">
                                    <strong>
                                        <asp:Label ID="c" runat="server" Width="90px" Height="13px" Text="From Date"></asp:Label></strong></td>
                                <td align="left">&nbsp;<asp:TextBox ID="txtStartDate" runat="server" onkeyup="BlockStartDateKeyPress()" CssClass="txtBox" MaxLength="10"
                                    Width="150px"></asp:TextBox>
                                    <asp:ImageButton ID="ibtnStartDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif" Width="16px" /></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 121px">
                                    <strong>
                                        <asp:Label ID="Label2" runat="server" Width="90px" Height="13px" Text="To Date"></asp:Label></strong></td>
                                <td align="left">&nbsp;<asp:TextBox ID="txtEndDate" runat="server" CssClass="txtBox " onkeyup="BlockEndDateKeyPress()" MaxLength="10" Width="150px"></asp:TextBox>
                                    <asp:ImageButton ID="ibnEndDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif" Width="16px" /></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td style="width: 121px"></td>
                                <td style="width: 100px">
                                    <cc1:CalendarExtender ID="CEStartDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnStartDate"
                                        TargetControlID="txtStartDate">
                                    </cc1:CalendarExtender>
                                    <cc1:CalendarExtender ID="CEEndDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibnEndDate"
                                        TargetControlID="txtEndDate">
                                    </cc1:CalendarExtender>
                                </td>
                                <td style="width: 100px"></td>
                            </tr>
                            <tr>
                                <td align="left" colspan="3">&nbsp;
    <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Width="90" Text="Cancel" CssClass="Button" />
                                    <asp:Button ID="btnNext" runat="server" Width="90" Text="Next" OnClick="btnNext_Click" CssClass="Button" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

