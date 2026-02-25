<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmPromotionStep3.aspx.cs" Inherits="Forms_frmPromotionStep3" Title="CORN :: Promotion Wizard Step 2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">
        <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
<script language="JavaScript" type="text/javascript">
    function pageLoad() {
        $("select").searchable();
    }
 </script>
    <div id="right_data">
        <table width="100%">
            <tr>
                <td>
                    <h2>
                        Promotion Wizard Step 2</h2>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <table>
                                <tbody>
                                    <tr>
                                        
                                        <td align="left" colspan="5">
                                            <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                        </td>
                                       
                                    </tr>
                                    <tr>
                                       
                                        <td style="height: 20px">
                                            <asp:CheckBox ID="ChbAllLocationType" runat="server" Text="All Type Location" AutoPostBack="True"
                                                OnCheckedChanged="ChbAllLocationType_CheckedChanged"></asp:CheckBox>
                                        </td>
                                      <td></td>
                                        <td style="height: 20px">
                                             <asp:CheckBox ID="chkSelectAllDistributors" runat="server" Text="All Location" AutoPostBack="True"
                                                OnCheckedChanged="chkSelectAllDistributors_CheckedChanged"></asp:CheckBox>
                                        </td>
                                        <td></td>
                                         <td style="height: 16px">
                                             <asp:CheckBox ID="ChbAllVolumeClass" runat="server" Text="All Customer Group" CssClass="lblbox"
                                                AutoPostBack="True" OnCheckedChanged="ChbAllVolumeClass_CheckedChanged"></asp:CheckBox>
                                        </td>

                                    </tr>
                                    <tr>
                                       
                                        <td style="height: 250px">
                                            <asp:Panel ID="Panel1" runat="server" Width="250px" Height="300px" ScrollBars="Vertical"
                                                BorderStyle="Groove" BorderWidth="1px">
                                                <asp:CheckBoxList ID="ChbDistributorType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ChbDistributorType_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                        </td>
                                     <td></td>
                                        <td  style="height: 250px">
                                            <asp:Panel ID="Panel6" runat="server" Width="250px" Height="300px" ScrollBars="Vertical"
                                                BorderStyle="Groove" BorderWidth="1px">
                                                <asp:CheckBoxList ID="chklDistributors" runat="server" CssClass="lblbox">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                        </td>
                                        <td></td>
                                         <td style="height: 250px" >
                                            <asp:Panel ID="Panel8" runat="server" Width="250px" Height="300px" ScrollBars="Vertical"
                                                BorderStyle="Groove" BorderWidth="1px">
                                                <asp:CheckBoxList ID="ChbVolumClass" runat="server">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                   
                                       
                                            <strong>
                                                <asp:Label ID="Label3" runat="server" Width="96px" Text="Channel Type" CssClass="lblbox" Visible="false"></asp:Label></strong>
                                            <asp:CheckBox ID="chkSelectAllCustomerType" runat="server" Text="All Channel Type"
                                                CssClass="lblbox" AutoPostBack="True" Visible="false" OnCheckedChanged="chkSelectAllCustomerType_CheckedChanged">
                                            </asp:CheckBox>
                                   
                                        
                                            <asp:Panel ID="Panel5" runat="server" Height="150px" ScrollBars="Vertical"
                                                BorderStyle="Groove" BorderWidth="1px" Visible="false">
                                                <asp:CheckBoxList ID="chklCustomerType" Visible="false"  runat="server">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                    <tr>
                                      
                                        <td style="width: 170px">
                                            <asp:RadioButton ID="rBtnBasketPromotion" runat="server" Width="121px" Font-Size="8pt"
                                                Text="Basket Promotion" AutoPostBack="True" Visible="False"></asp:RadioButton><br />
                                            <asp:RadioButton ID="rBtnSlabPromotion" runat="server" Width="114px" Font-Size="8pt"
                                                Text="Slab Promotion" AutoPostBack="True" Checked="True" Visible="false"></asp:RadioButton>
                                        </td>
                                        <td>
                                        </td>
                                        <td style="width: 30px">
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <div>
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Width="90px"
                                    Text="Cancel" ValidationGroup="vg" CausesValidation="False" CssClass="Button" />
                                &nbsp;
                                <asp:Button ID="btnBack" OnClick="btnBack_Click" runat="server" Width="90" Text="Back"
                                    ValidationGroup="vg" CssClass="Button" />
                                <asp:Button ID="btnNext" OnClick="btnNext_Click" runat="server" Width="90" Text="Next"
                                    ValidationGroup="vg" CssClass="Button" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
