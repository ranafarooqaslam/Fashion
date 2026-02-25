<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmSlipNotes.aspx.cs" Inherits="Forms_frmSlipNotes" Title="Slip Notes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">
           <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
<script language="JavaScript" type="text/javascript">
    function pageLoad() {
        $("select").searchable();
    }

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            var oControl = args.get_postBackElement();
            oControl.value = "Wait...";
            oControl.disabled = true;
        }

        function CheckBoxListSelect() {
            var chkBoxList = document.getElementById('<%= ChbDistributorList.ClientID %>');
            var chkBox = document.getElementById('<%= ChbSelectAll.ClientID %>');
            var chkBoxCount;
            var i;
            if (chkBox.checked == true) {
                chkBoxCount = chkBoxList.getElementsByTagName("input");
                for (i = 0; i < chkBoxCount.length; i++) {
                    chkBoxCount[i].checked = true;
                }
            } else {
                chkBoxCount = chkBoxList.getElementsByTagName("input");
                for (i = 0; i < chkBoxCount.length; i++) {

                    chkBoxCount[i].checked = false;

                }
            }
        }
        function UnCheckSelectAll() {
            var chkBox = document.getElementById('<%= ChbSelectAll.ClientID %>');
            var chkBoxList = document.getElementById('<%= ChbDistributorList.ClientID %>');
            var chkBoxCount = chkBoxList.getElementsByTagName("input");
            var count = 0;
            for (var i = 0; i < chkBoxCount.length; i++) {
                if (chkBoxCount[i].checked == false) {
                    count += 1;
                }
            }
            if (count > 0) {
                chkBox.checked = false;
            }
            else {
                chkBox.checked = true;
            }
        }
    </script>
    <style type="text/css">
        .cmp input {
            width: 90%;
        }

        .cmp select {
            width: 90%;
        }

        .list input {
            width: 20%;
        }

        .list {
            width: 90%;
        }

        .tblheading {
            background: #006699;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
        }

            .tblheading td {
                color: #ffffff;
                padding: 5px 5px 5px 5px;
            }
    </style>
    <div id="right_data">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>
                    <table>
                        <tr>
                            <td colspan="5">
                                <asp:CheckBox ID="ChbSelectAll" onclick="CheckBoxListSelect()" runat="server" Width="255px"
                                    Font-Size="10pt" Text="Location"></asp:CheckBox></td>
                        </tr>
                        <tr>
                            <td rowspan="4">

                                <asp:Panel ID="Panel3" runat="server" Width="255px" Height="200px" ScrollBars="Vertical"
                                    BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver" BackColor="White">

                                    <asp:CheckBoxList ID="ChbDistributorList" onclick="UnCheckSelectAll()" runat="server" Width="236px" Font-Size="10pt">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                            </td>
                            <td colspan="4"></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:TextBox ID="txtChannelName" CssClass="txtBox" Width="255px" Height="25px" TextMode="MultiLine" runat="server" placeholder="Add Note"></asp:TextBox>
                            </td>
                            <td>

                                <asp:Button ID="btnSaveChannelType" Width="70px" OnClick="btnSaveChannelType_Click" runat="server" Text="Save" ValidationGroup="vg" CssClass="Button" />
                                <asp:Button ID="btnCancel" OnClick="btnCancel_Click" Width="70px" runat="server" Style="margin-left: 5px" Text="Cancel" CssClass="Button" />
                            </td>
                             <td colspan="2">
                                    <asp:Literal ID="lblErrorMsg" runat="server" Visible="false"></asp:Literal>
                                </td>
                            
                            <tr>

                                <td></td>
                                <td colspan="4" valign="top">
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <asp:Panel ID="Panel2" runat="server" Width="100%" ScrollBars="Vertical" Height="157px"
                                                BorderStyle="Solid" BorderWidth="1px" BorderColor="Silver">
                                                <asp:GridView ID="grdChannelData" runat="server" ForeColor="SteelBlue" HorizontalAlign="Center"
                                                    BorderColor="SteelBlue" BackColor="White" AutoGenerateColumns="False"
                                                    AllowPaging="true" PageSize="7" OnPageIndexChanging="grdChannelData_PageIndexChanging"
                                                    OnRowEditing="grdChannelData_RowEditing" OnRowDeleting="grdChannelData_RowDeleting">
                                                     <alternatingrowstyle backcolor="#E0E0E0"/>
                                                    <Columns>
                                                        <asp:BoundField DataField="NOTE_ID" HeaderText="NOTE_ID" ReadOnly="true">
                                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="SLIP_NOTE" HeaderText="Note" ReadOnly="true">
                                                            <ItemStyle HorizontalAlign="Left" CssClass="grdDetail" />
                                                            <HeaderStyle HorizontalAlign="Center" CssClass="grdHead" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="IS_ACTIVE" HeaderText="" ReadOnly="true">
                                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="STATUS" HeaderText="Status" ReadOnly="true">
                                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
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
                                                    <PagerStyle CssClass="GridPager" HorizontalAlign="Right" />
                                                </asp:GridView>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>

                            </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
