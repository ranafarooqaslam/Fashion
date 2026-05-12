<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" 
    CodeFile="frmDayReverse.aspx.cs" Inherits="Forms_frmDayReverse" Title="CORN: Day Reverse" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="content1" runat="server" ContentPlaceHolderID="cphPage">
    <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
    <script language="JavaScript" type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(startRequest);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);
        
        function startRequest(sender, e) {
            if (document.getElementById('<%=btnViewPDF.ClientID %>') != null) {
                document.getElementById('<%=btnViewPDF.ClientID %>').disabled = true;
            }
        }
        
        function endRequest(sender, e) {
            if (document.getElementById('<%=btnViewPDF.ClientID %>') != null) {
                document.getElementById('<%=btnViewPDF.ClientID %>').disabled = false;
            }
        }
        
        function confirmDayReverse() {
            return confirm('Are you sure to confirm Day Reverse? Data will be lost');
        }
        function pageLoad() {
            $("select").searchable();
        }
    </script>
    
    <div id="right_data">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table width="100%" cellpadding="4" cellspacing="0" border="0">
                    <tbody>
                        <!-- Error Message Row -->
                        <tr>
                            <td align="left" width="100%" colspan="5">
                                <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                             </td>
                        </tr>
                        
                        <!-- Location Dropdown Row -->
                        <tr>
                            <td align="left" style="width: 150px;">
                                <strong>
                                <asp:Label ID="lblfromLocation" runat="server" Text="Location" CssClass="control-label col-form-label"></asp:Label>
                                    </strong>
                             </td>
                            <td align="left" style="width: 300px;">
                                <asp:DropDownList ID="drpDistributor" runat="server" AutoPostBack="True" 
                                    OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged" Visible="true"
                                    CssClass="DropList form-control" style="width: 250px;"></asp:DropDownList>
                             </td>
                            <td align="left">
                                &nbsp;
                             </td>
                            <td align="left" style="width: 100px;">
                                <strong>
                                <asp:Label ID="Label1" runat="server" Text="Working Date: " CssClass="control-label col-form-label"></asp:Label>
                                    </strong>
                             </td>
                            <td align="left">
                                <span style="color:red" runat="server" id="workDate"></span>
                             </td>
                        </tr>
                        
                        <!-- Day Reverse Button Row -->
                        <tr>
                            <td align="left" valign="top">
                                &nbsp;
                             </td>
                            <td align="left">
                                <asp:Button ID="btnViewPDF" runat="server" CssClass="Button btn btn-primary" 
                                    OnClientClick="javascript:return confirmDayReverse();"
                                    OnClick="btnViewPDF_Click" Text="Day Reverse" Width="125px" 
                                    Style="margin-top: 15px; margin-bottom: 20px; background-color: #006699; font-size: 15px;" />
                             </td>
                            <td align="left">
                                &nbsp;
                             </td>

                             <td align="left" colspan="2">
                                <span style="color:red"> * Before Day Reverse please make sure all the users of the selected location are log out. </span>
                             </td>
                        </tr>
                        
                        
                        <!-- Update Progress Row -->
                        <tr>
                            <td align="center" colspan="3">
                                <div style="z-index: 101; width: 100px; position: relative; height: 50px; text-align: center;">
                                    <asp:Panel ID="Panel1" runat="server">
                                        <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
                                            <ProgressTemplate>
                                                &nbsp;<asp:ImageButton ID="btnImage" runat="server" Height="33px" Width="31px" ImageUrl="~/App_Themes/Granite/Images/image003.gif" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                    </asp:Panel>
                                </div>
                             </td>
                        </tr>
                    </tbody>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>