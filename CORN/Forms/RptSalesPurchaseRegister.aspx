<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="RptSalesPurchaseRegister.aspx.cs" Inherits="Forms_RptSalesPurchaseRegister"
    Title="CORN :: Sales Purchase Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">
                <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
<script language="JavaScript" type="text/javascript">
    function pageLoad() {
        $("select").searchable();
    }
       

        function onCalendarShown() {
            var cal = $find("calendar1");     //Setting the default mode to month   
            cal._switchMode("months", true);          //Iterate every month Item and attach click event to it   
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.addHandler(row.cells[j].firstChild, "click", call);
                    }
                }
            }
        }

        function onCalendarHidden() {
            var cal = $find("calendar1");
            //Iterate every month Item and remove click event from it
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild, "click", call);
                    }
                }
            }
        }

        function call(eventElement) {
            var target = eventElement.target;
            switch (target.mode) {
                case "month":
                    var cal = $find("calendar1");
                    cal._visibleDate = target.date;
                    cal.set_selectedDate(target.date);
                    cal._switchMonth(target.date);
                    cal._blur.post(true);
                    cal.raiseDateSelectionChanged();
                    break;
            }
        }
    </script>
    <div id="right_data">
        <table width="100%">
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table>
                                <tbody>
                                    <tr>
                                        <td align="left" colspan="4">
                                            <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                        </td>
                                        <td valign="top" align="left">
                                            <strong>
                                                <asp:Label ID="lblType" runat="server" Width="76px" Height="13px" Text="Report Type"></asp:Label></strong>
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td style="height: 25px" align="left">
                                            <asp:RadioButtonList ID="rblReportType" runat="server" __designer:wfdid="w1">
                                                <asp:ListItem Selected="True" Value="0">Taxable Sales</asp:ListItem>
                                                <asp:ListItem Value="1">III Shedule Sales</asp:ListItem>
                                                <asp:ListItem Value="2">Exempt Sales</asp:ListItem>
                                                <asp:ListItem Value="3">Taxable Purchases</asp:ListItem>
                                                <asp:ListItem Value="4">III Schedule Purchase</asp:ListItem>
                                                <asp:ListItem Value="5">Exempt Purchase</asp:ListItem>
                                                <asp:ListItem Value="6">Stock Register</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                            <strong>
                                                <asp:Label ID="lblLocation" runat="server" Width="76px" Height="13px" Text="Location"></asp:Label></strong>
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td style="height: 25px" align="left">
                                            <asp:DropDownList ID="drpDistributor" runat="server" Width="200px" CssClass="DropList">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                            <strong>
                                                <asp:Label ID="lblPrincipal" runat="server" Width="76px" Height="13px" Text="Principal"></asp:Label></strong>
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td style="height: 25px" align="left">
                                            <asp:DropDownList ID="DrpPrincipal" runat="server" Width="200px" CssClass="DropList">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                            <strong>
                                                <asp:Label ID="Label3" runat="server" Width="76px" Height="13px" Text="For Month"></asp:Label></strong>
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td style="height: 25px" align="left">
                                            &nbsp;<asp:TextBox ID="txtMonth" onkeyup="BlockStartDateKeyPress()" runat="server"
                                                Width="70px" CssClass="txtBox" MaxLength="10"></asp:TextBox>
                                            <asp:ImageButton ID="ibtnStartDate" runat="server" Width="16px" ImageUrl="~/App_Themes/Granite/Images/date.gif">
                                            </asp:ImageButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td style="height: 25px" align="left">
                                            <%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
                                            <cc1:CalendarExtender ID="CEMonth" runat="server" Format="MMM-yyyy" PopupButtonID="ibtnStartDate"
                                                TargetControlID="txtMonth" BehaviorID="calendar1" OnClientShown="onCalendarShown"
                                                OnClientHidden="onCalendarHidden">
                                            </cc1:CalendarExtender>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Button ID="btnPDF" runat="server" Text="View PDF" CssClass="Button" Width="90"
                        OnClick="btnPDF_Click" />
                    <asp:Button ID="btnExcel" runat="server" Text="View Excel" CssClass="Button" Width="90"
                        OnClick="btnExcel_Click" />
                </td>
            </tr>
        </table>
        &nbsp;
    </div>
</asp:Content>
