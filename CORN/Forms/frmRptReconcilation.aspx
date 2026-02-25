<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" CodeFile="frmRptReconcilation.aspx.cs" 
Inherits="Forms_frmRptReconcilation" Title="CORN :: Value Reconciliation" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
         <script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
<script language="JavaScript" type="text/javascript">
    function pageLoad() {
        $("select").searchable();
    }

     $(document).ready(function() {
            $('#<%=cbSelectAll.ClientID %>').click(function() {
                 $("INPUT[type='checkbox']").attr('checked', $('#<%=cbSelectAll.ClientID %>').is(':checked'));
             });
             
             $('#<%=cblReportFilter.ClientID %> input:checkbox').click(function(){
                CheckSelectAll();
        });
        
        function CheckSelectAll()
        {
          var i = 0,j=0,k=0;
           $('#<%=cblReportFilter.ClientID %> input:checkbox').each(function() {
                i=i+1;
                if(this.checked == false)
                {
                    j=j+1;
                }
                else
                {
                    k=k+1;
                }
            });
            if(j < i && j > 0)
            {
              $('#<%= cbSelectAll.ClientID %>').attr('checked',false);
            }
            else if(k == i)
              {
                $('#<%= cbSelectAll.ClientID %>').attr('checked',true);
              }
        }
             
             
         });
         
    function CheckReportType()
     {
        var i = 0;
        
        $('#<%=cblReportFilter.ClientID %> input:checkbox').each(function() {
                if(this.checked == true)
                {
                    i=i+1;
                }
            });            
        if(i <= 0)
        {
            alert('Please select atleast one report option');
            return false;  
        }
        else
        {
            return true;
        }
     }
             
                      
    function ValidateForm()
	{
			
		return true;	  		
	}

    </script>
     <div id="right_data">
        <table width="100%">
            <tr>
                <td >
                    <asp:UpdatePanel id="UpdatePanel1" runat="server">
                        <contenttemplate>
<TABLE><TBODY><TR><TD align=left colSpan=4><asp:Label id="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label> </TD></TR><TR><TD align=left></TD><TD style="WIDTH: 95px" align=left>
<strong><asp:Label id="lbltoLocation" runat="server" Width="94px" Text="Location" CssClass="lblbox"></asp:Label></strong></TD><TD style="WIDTH: 1px" align=left></TD><TD style="HEIGHT: 25px" align=left><asp:DropDownList id="drpDistributor" runat="server" Width="200px" CssClass="DropList">
    </asp:DropDownList></TD></TR><TR><TD align=left></TD><TD style="WIDTH: 95px" align=left>
    <strong><asp:Label id="Label6" runat="server" Width="78px" Text="Principal" CssClass="lblbox"></asp:Label></strong></TD><TD style="WIDTH: 1px" align=left></TD><TD style="HEIGHT: 25px" align=left><asp:DropDownList id="DrpPrincipal" runat="server" Width="200px" CssClass="DropList">
            </asp:DropDownList></TD></TR><TR><TD align=left></TD><TD style="WIDTH: 95px" align=left>
            <strong><asp:Label id="Label3" runat="server" Width="76px" Height="13px" Text="From Date"></asp:Label></strong></TD><TD style="WIDTH: 1px" align=left></TD><TD style="HEIGHT: 25px" align=left>&nbsp;<asp:TextBox id="txtStartDate" onkeyup="BlockStartDateKeyPress()" runat="server" Width="150px" CssClass="txtBox" MaxLength="10"></asp:TextBox> <asp:ImageButton id="ibtnStartDate" runat="server" Width="16px" ImageUrl="~/App_Themes/Granite/Images/date.gif"></asp:ImageButton></TD></TR><TR><TD align=left></TD><TD style="WIDTH: 95px" align=left>
            <strong><asp:Label id="Label4" runat="server" Width="80px" Height="13px" Text="To Date"></asp:Label></strong></TD><TD style="WIDTH: 1px" align=left></TD><TD style="HEIGHT: 25px" align=left>&nbsp;<asp:TextBox id="txtEndDate" onkeyup="BlockEndDateKeyPress()" runat="server" Width="150px" CssClass="txtBox " MaxLength="10"></asp:TextBox> <asp:ImageButton id="ibnEndDate" runat="server" Width="16px" ImageUrl="~/App_Themes/Granite/Images/date.gif"></asp:ImageButton></TD></TR><TR><TD colSpan=4>&nbsp;</TD></TR><TR><TD align=left></TD><TD style="WIDTH: 95px" align=left><asp:CheckBox id="cbSelectAll" runat="server" Width="86px" Text="Select All" Checked="True"></asp:CheckBox> </TD><TD style="WIDTH: 1px" align=left></TD><TD style="HEIGHT: 25px" align=left></TD></TR><TR><TD align=left colSpan=4><DIV id="divFilter" class="containeRadioButtons"><TABLE width="100%"><TBODY><TR><TD align=left><asp:CheckBoxList id="cblReportFilter" runat="server" Width="350px" RepeatDirection="Horizontal"><asp:ListItem Selected="True" Value="0">A-Inventory</asp:ListItem>
<asp:ListItem Selected="True" Value="1">B-Sales</asp:ListItem>
<asp:ListItem Selected="True" Value="2">C-Cash</asp:ListItem>
<asp:ListItem Selected="True" Value="3">D-Credit</asp:ListItem>
</asp:CheckBoxList> </TD></TR></TBODY></TABLE></DIV></TD></TR><TR><TD align=left></TD><TD style="WIDTH: 95px" align=left></TD><TD style="WIDTH: 1px" align=left></TD><TD style="HEIGHT: 25px" align=left><%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %><cc1:CalendarExtender id="CEStartDate" runat="server" TargetControlID="txtStartDate" PopupButtonID="ibtnStartDate" Format="dd-MMM-yyyy">
            </cc1:CalendarExtender> <cc1:CalendarExtender id="CEEndDate" runat="server" TargetControlID="txtEndDate" PopupButtonID="ibnEndDate" Format="dd-MMM-yyyy">
            </cc1:CalendarExtender> </TD></TR></TBODY></TABLE>
</contenttemplate>
                    </asp:UpdatePanel>
                    &nbsp; &nbsp;
        <asp:Button ID="btnViewPDF" runat="server" CssClass="Button" Width="90"
                Text="View PDF" OnClick="btnViewPDF_Click" OnClientClick="return CheckReportType();" />
                    <asp:Button ID="btnViewExcel" runat="server" CssClass="Button" Width="90"
                Text="View Excel" OnClick="btnViewExcel_Click" OnClientClick="return CheckReportType();" /></td>
            </tr>
        </table>
         &nbsp;
        
           </div>
       
</asp:Content>
