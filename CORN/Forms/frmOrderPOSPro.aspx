<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmOrderPOSPro.aspx.cs" Inherits="Forms_frmOrderPOS" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>CORN :: Point Of Sales</title>
    <link rel="shortcut icon" href="../images/favicon.png" />
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Granite/Popup.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Granite/GridSorter.css" rel="stylesheet" type="text/css" />
    <link href="../css/POSstyle.css" rel="stylesheet" type="text/css" />
    <link href="../css/bootstrap.css" rel="stylesheet" type="text/css" />
     
<script language="JavaScript" type="text/javascript">
   


        function SearchProduct() {
            if (document.getElementById('<%=txtskuCode.ClientID%>').value == '') {

                document.getElementById('<%=txtAuthorisedBy.ClientID%>').focus();
            }

            else {
                var obj = jQuery.parseJSON($("#<%=hfProduct.ClientID %>").val());

                $('#<%=lblfound.ClientID%>').text('');
                $('#<%=lblClosingStock.ClientID%>').text('');
                var Productflag = 0;
                var Stockflag = 0;
                for (var i = 0; i < obj.length; i++) {

                    var item = obj[i];
                    if (item.SKU_CODE == document.getElementById('<%=txtskuCode.ClientID%>').value) {
                        Productflag = 1;
                        document.getElementById("<%= txtskuID.ClientID %>").value = item.SKU_ID;
                        document.getElementById("<%= txtskuName.ClientID %>").value = item.SKU_NAME;
                        document.getElementById("<%= txtcolor.ClientID %>").value = item.COLOR;
                        document.getElementById("<%= txtsize.ClientID %>").value = item.PACKSIZE;
                        document.getElementById("<%= txtUnitRate.ClientID %>").value = item.TRADE_PRICE;
                        if (item.FILEEXTENSION == 'noimage') {
                            $("[id$='imgSKU']").attr("src", '../images/cloth.png');

                        } else {
                            //document.getElementById("<%=imgSKU.ClientID %>").value = item.FILEEXTENSION
                            //  alert(item.SKU_ID + 'this is else' + item.FILEEXTENSION);
                            $("[id$='imgSKU']").attr("src", '../SkuImages/' + item.SKU_ID + item.FILEEXTENSION);
                        }
                        $('#<%=lblClosingStock.ClientID%>').text(item.CLOSING_STOCK);

                        var table = document.getElementById('<%=dataTable.ClientID%>');
                        var qty = document.getElementById("<%= txtQuantity.ClientID %>").value;

                        if (table.rows.length == "0") {

                            if (parseFloat(qty) > item.CLOSING_STOCK) {

                                Stockflag = 0;
                            }
                            else {
                                Stockflag = 1;
                            }
                        }
                        else {

                            $('#<%=dataTable.ClientID%>').find('tr').each(function () {

                                var td1 = $(this).find("td:eq(0)").text();

                                if (item.SKU_CODE == td1) {

                                    var CurrentQty = $(this).find("td:eq(4)").text();

                                    CurrentQty = parseFloat(CurrentQty) + parseFloat(qty);

                                    var ClosingStock = item.CLOSING_STOCK;

                                    if (parseFloat(CurrentQty) > parseFloat(ClosingStock)) {

                                        Stockflag = 0;
                                    }
                                    else {
                                        Stockflag = 1;
                                    }
                                }
                                else {

                                    if (parseFloat(qty) > item.CLOSING_STOCK) {
                                        Stockflag = 0;
                                    }
                                    else {

                                        Stockflag = 1;
                                    }
                                }
                            });
                        }
                    }
                }
                if (Productflag == 0) {

                    $('#<%=lblfound.ClientID%>').text('Product not found.');
                    document.getElementById("<%= txtskuCode.ClientID %>").focus();

                    return false;
                }


                if (Stockflag == 0) {

                    <%-- $('#<%=lblfound.ClientID%>').text('Stock:  ' + $('#<%=lblClosingStock.ClientID%>').text());--%>
                    $('#<%=lblfound.ClientID%>').text('Insufficient Stock !  ');
                    document.getElementById("<%= txtskuCode.ClientID %>").focus();

                    return false;
                }

                document.getElementById("<%= txtskuCode.ClientID %>").focus();
                return true;
            }
        }
        function duplicationCheck(skuCode) {


        }
        function storeTblValues() {

            var tableData = new Array();
            $('#<%=dataTable.ClientID%> tr').each(function (row, tr) {

                tableData[row] = {
                    "SKU_Code": $(tr).find('td:eq(0)').text()
                    , "SKU_Name": $(tr).find('td:eq(1)').text()
                    , "QUANTITY_UNIT": $(tr).find('td:eq(4)').text()
                    , "STANDARD_DISCOUNT": $(tr).find('td:eq(5)').text()
                    , "UNIT_PRICE": $(tr).find('td:eq(6)').text()
                    , "NET_AMOUNT": $(tr).find('td:eq(7)').text()

                    //hidden col's
                    , "SKU_ID": $(tr).find('td:eq(8)').text()
                    , "COLOR": $(tr).find('td:eq(2)').text()
                    , "PACKSIZE": $(tr).find('td:eq(3)').text()
                    , "AMOUNT": $(tr).find('td:eq(9)').text()
                    , "CHECK_DELETE": 0
                    , "GST_RATE": 0
                    , "GST_AMOUNT": 0
                    , "TST_AMOUNT": 0
                    , "STANDARD_DISCOUNT_TEMP": 0
                    , "STANDARD_DISCOUNT_PER": 0
                    , "BATCH_NO": 0
                }
            });

            return tableData;

        }

        function addRow() {

            if (SearchProduct()) {

                var flag = 0;
                var skuCode = $('#<%=txtskuCode.ClientID%>').val();

                var e = document.getElementById('<%=DrpDiscount.ClientID%>');
                var discType = e.options[e.selectedIndex].value;

                var table = document.getElementById('<%=dataTable.ClientID%>');
                var rowCount = table.rows.length;

                var b;
                var net;
                var disc;
                var i;
                var tableData;
                var row;
                var perDisc;
                var cell1;
                var cell2;
                var cell3;
                var cell4;
                var cell5;
                var cell6;
                var cell7;
                var cell8;
                var cell9;
                var cell10;
                var cell11;
                var mode = document.getElementById("<%=btnToggleMode.ClientID%>").value;

                if (table.rows.length == "0") {

                    row = table.insertRow(rowCount);
                    cell1 = row.insertCell(0);
                    cell1.innerHTML = document.getElementById('<%=txtskuCode.ClientID%>').value;
                    cell1.style.width = "240px";
                    cell2 = row.insertCell(1);
                    cell2.innerHTML = document.getElementById('<%=txtskuName.ClientID%>').value;
                    cell2.style.width = "280px";

                    cell3 = row.insertCell(2);
                    cell3.innerHTML = document.getElementById('<%=txtcolor.ClientID%>').value;
                    cell3.style.display = 'none';

                    cell4 = row.insertCell(3);
                    cell4.innerHTML = document.getElementById('<%=txtsize.ClientID%>').value;
                    cell4.style.width = "92px";

                    cell5 = row.insertCell(4);
                    cell5.innerHTML = document.getElementById('<%=txtQuantity.ClientID%>').value;
                    cell5.style.width = "85px";

                    cell6 = row.insertCell(5);
                    if (discType == 0) {

                        perDisc = (document.getElementById('<%=txtUnitRate.ClientID%>').value * document.getElementById('<%=txtQuantity.ClientID%>').value) * (document.getElementById('<%=txtDiscount.ClientID%>').value / 100);
                        cell6.innerHTML = perDisc;

                    } else {

                        if (mode == 'SALE MODE') {

                            perDisc = document.getElementById('<%=txtDiscount.ClientID%>').value;
                            cell6.innerHTML = perDisc;
                        } else {

                            perDisc = document.getElementById('<%=txtDiscount.ClientID%>').value * -1;
                            cell6.innerHTML = perDisc;
                        }


                    }
                    cell6.style.width = "89px";

                    cell7 = row.insertCell(6);
                    cell7.innerHTML = document.getElementById('<%=txtUnitRate.ClientID%>').value;
                    cell7.style.width = "85px";

                    cell8 = row.insertCell(7);
                    cell8.innerHTML = (document.getElementById('<%=txtUnitRate.ClientID%>').value * document.getElementById('<%=txtQuantity.ClientID%>').value) - perDisc;
                    cell8.style.width = "70px";
                    //hidden cell's
                    cell9 = row.insertCell(8);
                    cell9.innerHTML = document.getElementById('<%=txtskuID.ClientID%>').value;
                    cell9.style.display = 'none';
                    cell10 = row.insertCell(9);
                    cell10.innerHTML = (document.getElementById('<%=txtUnitRate.ClientID%>').value * document.getElementById('<%=txtQuantity.ClientID%>').value);
                    cell10.style.display = 'none';

                    cell11 = row.insertCell(10);
                    cell11.innerHTML = '<input type="button" value = "X" style="color:white;background-color:red;cursor:pointer" onClick="Javacsript:deleteRow(this)">';

                    //Calculation for loop getting cell values
                    b = 0;
                    net = 0;
                    disc = 0;
                    for (i = 0; i < table.rows.length; i++) {

                        b = table.rows[i].cells[9].innerHTML;

                        disc = parseFloat(table.rows[i].cells[5].innerHTML).toFixed(2);
                        net = (parseFloat(net) + parseFloat(b)).toFixed(2);

                    }
                    document.getElementById('<%=txtGrossAmount.ClientID%>').value = net;
                    document.getElementById('<%=numtxtTotalExtraDiscnt.ClientID%>').value = disc;
                    document.getElementById('<%=numTxtTotlAmnt.ClientID%>').value = (parseFloat(net) - parseFloat(disc)).toFixed(2);


                    //set data in json and store in hidden field through storeTblValues()
                    tableData = storeTblValues();
                    tableData = $.toJSON(tableData);
                    document.getElementById('<%=tab.ClientID%>').value = tableData;
                } else { //Work on duplication
                    var duplicrw;

                    $('#<%=dataTable.ClientID%>').find('tr').each(function () {

                        var td1 = $(this).find("td:eq(0)").text();
                        if (skuCode == td1) {

                            duplicrw = $(this);

                            flag = 1;
                        }
                    });

                    if (flag == "1") {
                        var perDisc2;
                        var unitrate = duplicrw.find("td:eq(6)").text();
                        var discount = duplicrw.find("td:eq(5)").text();
                        var qty = duplicrw.find("td:eq(4)").text();
                        if (discType == 0) {

                            perDisc2 = (document.getElementById('<%=txtUnitRate.ClientID%>').value * document.getElementById('<%=txtQuantity.ClientID%>').value) * (document.getElementById('<%=txtDiscount.ClientID%>').value / 100);
                        } else {


                            perDisc2 = document.getElementById('<%=txtDiscount.ClientID%>').value;

                        }
                        duplicrw.find("td:eq(4)").text(parseInt(qty, 10) + parseInt(document.getElementById('<%=txtQuantity.ClientID%>').value, 10));
                        duplicrw.find("td:eq(5)").text((parseFloat(perDisc2) + parseFloat(discount)).toFixed(0));

                        duplicrw.find("td:eq(7)").text((parseInt(qty, 10) + parseInt(document.getElementById('<%=txtQuantity.ClientID%>').value, 10)) * parseInt(unitrate, 10) - ((parseFloat(perDisc2) + parseFloat(discount)).toFixed(0)));
                        duplicrw.find("td:eq(9)").text((parseInt(qty, 10) + parseInt(document.getElementById('<%=txtQuantity.ClientID%>').value, 10)) * parseInt(unitrate, 10));


                        b = 0;
                        net = 0;
                        var disc3 = 0;

                        for (i = 0; i < table.rows.length; i++) {

                            b = table.rows[i].cells[9].innerHTML;
                            disc3 = (parseFloat(disc3) + parseFloat(table.rows[i].cells[5].innerHTML)).toFixed(2);
                            net = (parseFloat(net) + parseFloat(b)).toFixed(2);

                        }

                        document.getElementById('<%=txtGrossAmount.ClientID%>').value = net;

                        document.getElementById('<%=numtxtTotalExtraDiscnt.ClientID%>').value = disc3;
                        document.getElementById('<%=numTxtTotlAmnt.ClientID%>').value = (parseFloat(net) - parseFloat(disc3)).toFixed(2);

                        tableData = storeTblValues();
                        tableData = $.toJSON(tableData);

                        document.getElementById('<%=tab.ClientID%>').value = tableData;

                        ClearControls();

                    } else {
                        row = table.insertRow(rowCount);
                        cell1 = row.insertCell(0);
                        cell1.innerHTML = document.getElementById('<%=txtskuCode.ClientID%>').value;
                        cell1.style.width = "240px";

                        cell2 = row.insertCell(1);
                        cell2.innerHTML = document.getElementById('<%=txtskuName.ClientID%>').value;
                        cell2.style.width = "280px";

                        cell3 = row.insertCell(2);
                        cell3.innerHTML = document.getElementById('<%=txtcolor.ClientID%>').value;
                        cell3.style.display = 'none';

                        cell4 = row.insertCell(3);
                        cell4.innerHTML = document.getElementById('<%=txtsize.ClientID%>').value;
                        cell4.style.width = "92px";

                        cell5 = row.insertCell(4);
                        cell5.innerHTML = document.getElementById('<%=txtQuantity.ClientID%>').value;
                        cell5.style.width = "85px";

                        cell6 = row.insertCell(5);
                        if (discType == 0) {

                            perDisc = (document.getElementById('<%=txtUnitRate.ClientID%>').value * document.getElementById('<%=txtQuantity.ClientID%>').value) * (document.getElementById('<%=txtDiscount.ClientID%>').value / 100);
                            cell6.innerHTML = perDisc;
                        } else {
                            if (mode == 'SALE MODE') {

                                perDisc = document.getElementById('<%=txtDiscount.ClientID%>').value;
                                cell6.innerHTML = perDisc;
                            } else {

                                perDisc = document.getElementById('<%=txtDiscount.ClientID%>').value * -1;
                                cell6.innerHTML = perDisc;
                            }

                        }
                        cell6.style.width = "89px";

                        cell7 = row.insertCell(6);
                        cell7.innerHTML = document.getElementById('<%=txtUnitRate.ClientID%>').value;
                        cell7.style.width = "85px"; cell8 = row.insertCell(7);

                        cell8.innerHTML = (document.getElementById('<%=txtUnitRate.ClientID%>').value * document.getElementById('<%=txtQuantity.ClientID%>').value) - perDisc;
                        cell8.style.width = "70px";
                        //hidden cell's
                        cell9 = row.insertCell(8);
                        cell9.innerHTML = document.getElementById('<%=txtskuID.ClientID%>').value;
                        cell9.style.display = 'none';
                        cell10 = row.insertCell(9);
                        cell10.innerHTML = (document.getElementById('<%=txtUnitRate.ClientID%>').value * document.getElementById('<%=txtQuantity.ClientID%>').value);
                        cell10.style.display = 'none';
                        cell11 = row.insertCell(10);
                        cell11.innerHTML = '<input type="button" value = "X" style="color:white;background-color:red;cursor:pointer" onClick="Javacsript:deleteRow(this)">';

                        //Calculation for loop getting cell values
                        b = 0;
                        net = 0;
                        var disc2 = 0;

                        for (i = 0; i < table.rows.length; i++) {

                            b = table.rows[i].cells[9].innerHTML;

                            disc2 = (parseFloat(disc2) + parseFloat(table.rows[i].cells[5].innerHTML)).toFixed(2);

                            net = (parseFloat(net) + parseFloat(b)).toFixed(2);

                        }
                        document.getElementById('<%=txtGrossAmount.ClientID%>').value = net;
                        document.getElementById('<%=numtxtTotalExtraDiscnt.ClientID%>').value = disc2;
                        document.getElementById('<%=numTxtTotlAmnt.ClientID%>').value = (parseFloat(net) - parseFloat(disc2)).toFixed(2);


                        //set data in json and store in hidden field through storeTblValues()
                        tableData = storeTblValues();

                        tableData = $.toJSON(tableData);

                        document.getElementById('<%=tab.ClientID%>').value = tableData;

                    }
                }
            }
        }
        function deleteRow(obj) {

            var index = obj.parentNode.parentNode.rowIndex;
            var table = document.getElementById("<%=dataTable.ClientID %>");

            table.deleteRow(index);

            var b = 0;

            var net = 0;
            var disc = 0;

            for (var i = 0; i < table.rows.length; i++) {

                b = table.rows[i].cells[9].innerHTML;

                disc = table.rows[i].cells[5].innerHTML;
                net = (parseFloat(net) + parseFloat(b)).toFixed(0);

            }


            document.getElementById('<%=txtGrossAmount.ClientID%>').value = (parseFloat(net)).toFixed(2);
            document.getElementById('<%=numtxtTotalExtraDiscnt.ClientID%>').value = (parseFloat(disc)).toFixed(2);
            document.getElementById('<%=numTxtTotlAmnt.ClientID%>').value = (parseFloat(net) - parseFloat(disc)).toFixed(2);

            Calculate2();

            if (table.rows.length == "0") {
                document.getElementById('<%=txtCashRecieved2.ClientID%>').value = "";
                document.getElementById('<%=txtBalance.ClientID%>').value = "";
                document.getElementById("<%= btnToggleMode.ClientID %>").disabled = false;
            }
            document.getElementById('<%=txtskuCode.ClientID%>').focus();

            var tableData = storeTblValues();
            tableData = $.toJSON(tableData);
            document.getElementById('<%=tab.ClientID%>').value = tableData;
        }

        function ClearControls() {
            document.getElementById('<%=txtskuID.ClientID%>').value = "";
             document.getElementById('<%=txtskuCode.ClientID%>').value = "";
             document.getElementById('<%=txtskuName.ClientID%>').value = "";
             var mode = document.getElementById("<%= btnToggleMode.ClientID %>").value;
             if (mode == 'SALE MODE') {
                 document.getElementById('<%=txtQuantity.ClientID%>').value = "1";
            }
            else {
                document.getElementById('<%=txtQuantity.ClientID%>').value = "-1";
            }
            document.getElementById('<%=txtDiscount.ClientID%>').value = "0";
             document.getElementById('<%=txtcolor.ClientID%>').value = "";
             document.getElementById('<%=txtsize.ClientID%>').value = "";
             document.getElementById('<%=txtUnitRate.ClientID%>').value = "";
             document.getElementById("<%=btnToggleMode.ClientID%>").disabled = true;
         }

         function Calculate(e) {

             var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
             if (key == 13) {
                 e.preventDefault();
                 if (document.getElementById('<%=txtskuCode.ClientID%>').value != "") {
                    addRow();
                    ClearControls();
                    document.getElementById('<%=txtskuCode.ClientID%>').focus();
                }
                else {
                    document.getElementById('<%=txtAuthorisedBy.ClientID%>').focus();
                }

            }
        }
        function CalculateBalance(e) {
            var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
            if (key == 13) {
                e.preventDefault();

                var cashRcd = document.getElementById('<%=txtCashRecieved2.ClientID%>').value;
                if (cashRcd == "") {
                    cashRcd = 0;
                }
                var netAmount = document.getElementById('<%=numTxtTotlAmnt.ClientID%>').value;
                var disc = document.getElementById('<%=numtxtTotalExtraDiscnt.ClientID%>').value;
                var balce = 0;
                var mode = document.getElementById("<%= btnToggleMode.ClientID %>").value;
                if (mode == 'SALE MODE') {

                    document.getElementById('<%=txtBalance.ClientID%>').value = (parseFloat(cashRcd) - parseFloat(netAmount)).toFixed(0);

                }
                else {
                    if ((cashRcd > 0) && (netAmount < 0)) {

                        balce = (parseFloat(netAmount) - parseFloat(disc) + parseFloat(cashRcd)).toFixed(0);
                        document.getElementById('<%=txtBalance.ClientID%>').value = balce;
                    }
                    else if ((cashRcd > 0) && (netAmount > 0)) {

                        balce = (parseFloat(netAmount) - parseFloat(disc) - parseFloat(cashRcd)).toFixed(0);
                        document.getElementById('<%=txtBalance.ClientID%>').value = balce;
                    }
            }
            document.getElementById('<%=LinkButton2.ClientID%>').focus();
            }
        }
        function Calculate2() {

            var cashRcd = document.getElementById('<%=txtCashRecieved2.ClientID%>').value;
            if (cashRcd == "") {
                cashRcd = 0;
            }
            var netAmount = document.getElementById('<%=numTxtTotlAmnt.ClientID%>').value;
            var disc = document.getElementById('<%=numtxtTotalExtraDiscnt.ClientID%>').value;
            var balce = 0;
            var mode = document.getElementById("<%= btnToggleMode.ClientID %>").value;
            if (mode == 'SALE MODE') {

                if (cashRcd > 0) {
                    document.getElementById('<%=txtBalance.ClientID%>').value = (parseFloat(cashRcd) - parseFloat(netAmount)).toFixed(0);
                }
                else {
                    document.getElementById('<%=txtBalance.ClientID%>').value = parseFloat(netAmount).toFixed(0);
                }
            }
            else {
                if ((cashRcd > 0) && (netAmount < 0)) {

                    balce = (parseFloat(netAmount) - parseFloat(disc) + parseFloat(cashRcd)).toFixed(0);
                    document.getElementById('<%=txtBalance.ClientID%>').value = balce;
            }
            else if ((cashRcd > 0) && (netAmount > 0)) {

                balce = (parseFloat(netAmount) - parseFloat(disc) - parseFloat(cashRcd)).toFixed(0);
                document.getElementById('<%=txtBalance.ClientID%>').value = balce;
                }
        }
    }



    function ValidateForm() {

        var str = document.getElementById('<%=txtQuantity.ClientID%>').value;
        if (str == null || str.length == 0) {
            alert('Must Enter Quantity');
            return false;
        }

        return true;
    }



    function SetFocusTocashRecived(e) {
        var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
        if (key == 13) {
            e.preventDefault();
            setTimeout(function () { document.getElementById("<%= txtAuthorisedBy.ClientID %>").focus(); }, 10);
            }
        }
        function SetFocusTocode(e) {
            var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
            if (key == 13) {
                document.getElementById("<%= txtskuCode.ClientID %>").focus();

            }
            if (document.getElementById("<%= btnToggleMode.ClientID %>").value == 'SALE MODE') {
                if (document.getElementById("<%= txtQuantity.ClientID %>").value == '-') {

                    alert('- is not allowed in sale mode!');
                    document.getElementById("<%= txtQuantity.ClientID %>").value = '';
                }

            }
        }

        function FocusToCash(e) {
            var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
            if (key == 13) {
                e.preventDefault();

                setTimeout(function () { document.getElementById("<%= txtCashRecieved2.ClientID %>").focus(); }, 10);

            }
        }


        function ProductSelected(source, eventArgs) {
            var skuDetail = eventArgs.get_text();
            var num = eventArgs.get_value();

            document.getElementById("<%=txtskuCode.ClientID %>").value = skuDetail.substring(0, skuDetail.indexOf('-'));
        }
        function toggle(t) {

            var mode = document.getElementById("<%=btnToggleMode.ClientID%>").value;

            if (mode == 'SALE MODE') {
                document.getElementById("<%=btnToggleMode.ClientID %>").value = "REFUND MODE";
                document.getElementById("<%=hfToggleMode.ClientID %>").value = "REFUND MODE";
                document.getElementById("<%=txtQuantity.ClientID %>").value = "-1";
                document.getElementById("<%=btnToggleMode.ClientID %>").setAttribute("CssClass", "BtnModereturn");
                document.getElementById("<%=txtskuCode.ClientID %>").focus();
            } else if (mode == 'REFUND MODE') {
                document.getElementById("<%= btnToggleMode.ClientID %>").value = "SALE MODE";
                document.getElementById("<%=hfToggleMode.ClientID %>").value = "SALE MODE";
                document.getElementById("<%= txtQuantity.ClientID %>").value = "1";
                document.getElementById("<%= btnToggleMode.ClientID %>").setAttribute("CssClass", "BtnModesale");
                document.getElementById("<%=txtskuCode.ClientID %>").focus();
            }
    }

    function CheckCreditLimit() {
        var e = document.getElementById("<%= DrpPayMode.ClientID %>");
        var payMode = e.options[e.selectedIndex].value;

        if (payMode == "218") {

            var balanceCeiling = document.getElementById("lblAllowLimit").innerHTML;
            var NetAmount = document.getElementById("<%= numTxtTotlAmnt.ClientID %>").value;

                if (parseFloat(balanceCeiling) < parseFloat(NetAmount)) {

                    alert('Please Check Customer Balance Ceiling');

                    return false;
                }
            }
            else if (payMode == "214") {
                var mode = document.getElementById("<%=btnToggleMode.ClientID%>").value;
                 if (mode == 'SALE MODE') {
                     var NetAmount = document.getElementById("<%= txtBalance.ClientID %>").value;

                     if (NetAmount == null || NetAmount.length == 0) {
                         alert('Please enter Payment');
                         document.getElementById("<%= txtCashRecieved2.ClientID %>").focus();
                         return false;
                     }
                     else if (parseFloat(NetAmount) < 0) {

                         alert('Receive amount not match with Net Amount');
                         document.getElementById("<%= txtCashRecieved2.ClientID %>").focus();
                         return false;
                     }
             }
         }
     return true;
 }

        <%-- //////////////////Print Invoice region\\\\\\\\\\\\\\\\\\\\\--%>
        function PrintSaleInvoice() {
            var reptpostyp = document.getElementById("<%= hfPosReportType.ClientID %>").value
            if (reptpostyp == 0) {
                if (CheckCreditLimit()) {
                    var mode = document.getElementById("<%=btnToggleMode.ClientID%>").value;
                    if (mode == 'REFUND MODE') {
                        $("#invoiceMode").text("Sale Return");
                    }
                    else {
                        $("#invoiceMode").text("Sale Invoice");
                    }
                    var payMode = document.getElementById("<%= DrpPayMode.ClientID %>");
                    $("#payMode").text(payMode.options[payMode.selectedIndex].text);
                    var saleMan = document.getElementById("<%= ddsalesForce.ClientID %>");
                    $("#saleMan").text(saleMan.options[saleMan.selectedIndex].text);
                    var CustomerName = document.getElementById("<%= ddlCustomer.ClientID %>");
                    $("#lblCustomerName").text(CustomerName.options[CustomerName.selectedIndex].text);
                   
                    var Units = 0;
                    $('#<%=dataTable.ClientID%>').find('tr').each(function () {
                        Units += parseInt($(this).find("td:eq(4)").text());
                    });
                    $("#Units").text(Units);
                    var orderedProducts = document.getElementById('<%=tab.ClientID%>').value;
                    orderedProducts = eval(orderedProducts);
                    $('#invoiceDetailBody').empty(); // clear all skus  from invoice
                    for (var i = 0, len = orderedProducts.length; i < len; i++) {
                        var row = $('<tr><td>' + orderedProducts[i].SKU_Code + '<br />' + orderedProducts[i].SKU_Name + '</td><td class="text-right">' + orderedProducts[i].QUANTITY_UNIT + '</td><td class="text-right">' + parseFloat(orderedProducts[i].UNIT_PRICE).toFixed(0) + '</td><td class="text-right">' + parseFloat(orderedProducts[i].STANDARD_DISCOUNT).toFixed(0) + '</td><td class="text-right">' + parseFloat(orderedProducts[i].NET_AMOUNT).toFixed(0) + '</td></tr>');
                        $('#invoiceDetailBody').append(row);
                    }
                    var gross = document.getElementById('<%=txtGrossAmount.ClientID%>').value;
                    var discount = document.getElementById('<%=numtxtTotalExtraDiscnt.ClientID%>').value;
                    var amountDue = document.getElementById('<%=numTxtTotlAmnt.ClientID%>').value;
                    var paid = document.getElementById('<%=txtCashRecieved2.ClientID%>').value;
                    var balance = document.getElementById('<%=txtBalance.ClientID%>').value;
                    $("#TotalValue").text(gross);
                    $("#DiscountTotal").text(parseFloat(discount).toFixed(0));
                    $("#GrandTotal").text(parseFloat(amountDue).toFixed(0));
                    $("#Paid").text(parseFloat(paid).toFixed(0));
                    $("#Balance").text(parseFloat(balance).toFixed(0));


                    if ($("#invoiceDetailBody tr").length > 0) {

                        $.print("#dvSaleInvoice");

                    }
                    SaveInvoiceInDataBase();

                }
            } else {

                if (CheckCreditLimit()) {
                    var mode = document.getElementById("<%=btnToggleMode.ClientID%>").value;

                    var payMode = document.getElementById("<%= DrpPayMode.ClientID %>");

                    var saleMan = document.getElementById("<%= ddsalesForce.ClientID %>");
                    $("#saleMan2").text(saleMan.options[saleMan.selectedIndex].text);
                    var CustomerName = document.getElementById("<%= ddlCustomer.ClientID %>");
                    $("#lblCustomerName2").text(CustomerName.options[CustomerName.selectedIndex].text);

                    <%--  var Units = 0;
                $('#<%=dataTable.ClientID%>').find('tr').each(function () {
                    Units += parseInt($(this).find("td:eq(4)").text());
                });
                $("#Units2").text(Units);--%>
                    var orderedProducts = document.getElementById('<%=tab.ClientID%>').value;
                    orderedProducts = eval(orderedProducts);
                    $('#invoiceDetailBody2').empty(); // clear all skus  from invoice
                    for (var i = 0, len = orderedProducts.length; i < len; i++) {
                        var row = $('<tr><td>' + (parseInt( i) + 1) + '</td><td>' + orderedProducts[i].SKU_Code + '</td><td>' + orderedProducts[i].SKU_Name + '</td><td class="text-center">' + orderedProducts[i].QUANTITY_UNIT + '</td><td class="text-right">' + parseFloat(orderedProducts[i].UNIT_PRICE).toFixed(2) + '</td><td class="text-right">'+'-' + '</td><td class="text-right">' + parseFloat(orderedProducts[i].NET_AMOUNT).toFixed(2) + '</td></tr>');
                        $('#invoiceDetailBody2').append(row);
                    }
                    var gross = document.getElementById('<%=txtGrossAmount.ClientID%>').value;
                    var discount = document.getElementById('<%=numtxtTotalExtraDiscnt.ClientID%>').value;
                    var amountDue = document.getElementById('<%=numTxtTotlAmnt.ClientID%>').value;
                    var paid = document.getElementById('<%=txtCashRecieved2.ClientID%>').value;
                    var balance = document.getElementById('<%=txtBalance.ClientID%>').value;
                    var amountdueinword = number2words(amountDue);
                    amountdueinword = amountdueinword + ' Rs. Only';
                    $("#TotalValue2").text(gross);
                    $("#DiscountTotal2").text(parseFloat(discount).toFixed(0));
                    $("#GrandTotal2").text(parseFloat(amountDue).toFixed(2));
                    $("#Paid2").text(parseFloat(paid).toFixed(0));
                    $("#Balance2").text(parseFloat(balance).toFixed(0));
                    $("#GrandTotalText").text(amountdueinword);

                    if ($("#invoiceDetailBody2 tr").length > 0) {

                        $.print("#dvSaleInvoice2");

                    }
                    SaveInvoiceInDataBase();

                }
            }


        }
        function getcustomerDetail()
        {
            $.ajax({
                type: "POST",
                url: "frmOrderPOS.aspx/getCustomerDetail", //page/method name
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({ customerId: document.getElementById("<%= ddlCustomer.ClientID %>").value, })
                      
            });
        }

        function SaveInvoiceInDataBase() {

            $.ajax
                (
                    {
                        type: "POST", //HTTP method
                        url: "frmOrderPOS.aspx/InsertInvoice", //page/method name
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({ orderedProducts: document.getElementById('<%=tab.ClientID%>').value, amountDue: document.getElementById('<%=txtGrossAmount.ClientID%>').value, author: document.getElementById('<%=txtAuthorisedBy.ClientID%>').value, discount: $('#<%=numtxtTotalExtraDiscnt.ClientID%>').val(), netAmount: $('#<%=numTxtTotlAmnt.ClientID%>').val(), paidIn: $('#<%=txtCashRecieved2.ClientID%>').val(), payType: document.getElementById("<%= DrpPayMode.ClientID %>").value, Gst: document.getElementById("<%=numTxtTotalGST.ClientID %>").value, manualId: document.getElementById("<%= hfToggleMode.ClientID %>").value, customerId: document.getElementById("<%= ddlCustomer.ClientID %>").value, saleForce: document.getElementById("<%= ddsalesForce.ClientID %>").value, NewCustomerNam: document.getElementById("<%=txtNewCustomer.ClientID %>").value, NewCustomerContactNumber: document.getElementById("<%=txtNewCustomerCOntactNumer.ClientID %>").value, }),
                        success: invoiceSaved,
                        error: invoiceNotSaved
                    }
            );
           
                }

    function invoiceSaved(inId) {
     
        var hidid = $("#hfMaxId").text()
       <%-- var hidid = document.getElementById('<%=hfMaxId.ClientID%>').value;--%>
        hidid =parseInt( hidid) + 1;
        $("#hfMaxId").text(hidid);
        document.getElementById('<%=tab.ClientID%>').value = "";
      
                $('#<%=dataTable.ClientID%> tr').empty();

                    
                Clear();
              
                __doPostBack('UpdatePanel1', '');

                UpdateCreditLimit();
               
            }

            function invoiceNotSaved() {
                alert('Some error occurred');
                Clear();
               
            }

            // Update Limit After Insertion

            function UpdateCreditLimit() {
                var e = document.getElementById("<%= DrpPayMode.ClientID %>");
        var payMode = e.options[e.selectedIndex].value;

        if (payMode == "218") {
            document.getElementById('<%= btnUpdateLimit.ClientID %>').click();
        }
    }

    function Clear() {
        $('#<%=txtGrossAmount.ClientID%>').val('');
       $('#<%=txtAuthorisedBy.ClientID%>').val('');
       $('#<%=numtxtTotalExtraDiscnt.ClientID%>').val('');
       $('#<%=numTxtTotlAmnt.ClientID%>').val('');
       $('#<%=txtCashRecieved2.ClientID%>').val('');
       $('#<%=txtBalance.ClientID%>').val('');
       $('#<%=numTxtTotalGST.ClientID %>').val('');
       $('#<%=txtNewCustomer.ClientID %>').val('');
       $('#<%=txtNewCustomerCOntactNumer.ClientID %>').val('');
       var mode = document.getElementById("<%=btnToggleMode.ClientID%>").value;

       if (mode == 'REFUND MODE') {
           document.getElementById("<%= txtQuantity.ClientID %>").value = "-1";
            }
            else {
                document.getElementById("<%= txtQuantity.ClientID %>").value = "1";
            }
        }
        function PaymentMode() {

            var e = document.getElementById("<%= DrpPayMode.ClientID %>");
            var payMode = e.options[e.selectedIndex].value;
            if (payMode == "215" || payMode == "218") {

                document.getElementById("<%= txtCashRecieved2.ClientID %>").value = "";

                document.getElementById("<%= txtBalance.ClientID %>").value = document.getElementById("<%= numTxtTotlAmnt.ClientID %>").value;
                document.getElementById("<%= txtCashRecieved2.ClientID %>").readOnly = true;
            } else {
                document.getElementById("<%= txtCashRecieved2.ClientID %>").readOnly = false;
            }
            document.getElementById("<%=txtskuCode.ClientID %>").focus();
        }
        function numberToEnglish(n) {

            var string = n.toString(), units, tens, scales, start, end, chunks, chunksLen, chunk, ints, i, word, words, and = 'and';

            /* Remove spaces and commas */
            string = string.replace(/[, ]/g, "");

            /* Is number zero? */
            if (parseInt(string) === 0) {
                return 'zero';
            }

            /* Array of units as words */
            units = ['', 'one', 'two', 'three', 'four', 'five', 'six', 'seven', 'eight', 'nine', 'ten', 'eleven', 'twelve', 'thirteen', 'fourteen', 'fifteen', 'sixteen', 'seventeen', 'eighteen', 'nineteen'];

            /* Array of tens as words */
            tens = ['', '', 'twenty', 'thirty', 'forty', 'fifty', 'sixty', 'seventy', 'eighty', 'ninety'];

            /* Array of scales as words */
            scales = ['', 'thousand', 'million', 'billion', 'trillion', 'quadrillion', 'quintillion', 'sextillion', 'septillion', 'octillion', 'nonillion', 'decillion', 'undecillion', 'duodecillion', 'tredecillion', 'quatttuor-decillion', 'quindecillion', 'sexdecillion', 'septen-decillion', 'octodecillion', 'novemdecillion', 'vigintillion', 'centillion'];

            /* Split user arguemnt into 3 digit chunks from right to left */
            start = string.length;
            chunks = [];
            while (start > 0) {
                end = start;
                chunks.push(string.slice((start = Math.max(0, start - 3)), end));
            }

            /* Check if function has enough scale words to be able to stringify the user argument */
            chunksLen = chunks.length;
            if (chunksLen > scales.length) {
                return '';
            }

            /* Stringify each integer in each chunk */
            words = [];
            for (i = 0; i < chunksLen; i++) {

                chunk = parseInt(chunks[i]);

                if (chunk) {

                    /* Split chunk into array of individual integers */
                    ints = chunks[i].split('').reverse().map(parseFloat);

                    /* If tens integer is 1, i.e. 10, then add 10 to units integer */
                    if (ints[1] === 1) {
                        ints[0] += 10;
                    }

                    /* Add scale word if chunk is not zero and array item exists */
                    if ((word = scales[i])) {
                        words.push(word);
                    }

                    /* Add unit word if array item exists */
                    if ((word = units[ints[0]])) {
                        words.push(word);
                    }

                    /* Add tens word if array item exists */
                    if ((word = tens[ints[1]])) {
                        words.push(word);
                    }

                    /* Add 'and' string after units or tens integer if: */
                    if (ints[0] || ints[1]) {

                        /* Chunk has a hundreds integer or chunk is the first of multiple chunks */
                        if (ints[2] || !i && chunksLen) {
                            words.push(and);
                        }

                    }

                    /* Add hundreds word if array item exists */
                    if ((word = units[ints[2]])) {
                        words.push(word + ' hundred');
                    }

                }

            }

            return words.reverse().join(' ');

        }
        var num = "Zero One Two Three Four Five Six Seven Eight Nine Ten Eleven Twelve Thirteen Fourteen Fifteen Sixteen Seventeen Eighteen Nineteen".split(" ");
        var tens = "Twenty Thirty Forty Fifty Sixty Seventy Eighty Ninety".split(" ");

        function number2words(n) {
            if (n < 20) return num[n];
            var digit = n % 10;
            if (n < 100) return tens[~~(n / 10) - 2] + (digit ? "-" + num[digit] : "");
            if (n < 1000) return num[~~(n / 100)] + " Hundred" + (n % 100 == 0 ? "" : " " + number2words(n % 100));
            return number2words(~~(n / 1000)) + " Thousand" + (n % 1000 != 0 ? " " + number2words(n % 1000) : "");
        }
    </script>
    <style type="text/css">
        input[type=text], textarea {
            border: 1px solid #ccc;
        }

            input[type=text]:focus, textarea:focus {
                background-color: #CEE3F6;
                border: 1px solid #ccc;
            }
    </style>
</head>
<body>
    <form id="form1" method="POST" runat="server">
        <asp:ScriptManager runat="server" ID="smPOs" AsyncPostBackTimeout="30000" EnablePartialRendering="true">
        </asp:ScriptManager>
        <script src="../AjaxLibrary/jquery-2.0.2.min.js" type="text/javascript"></script>
        <script src="../AjaxLibrary/jquery.json-2.4.min.js" type="text/javascript"></script>
        <script src="../AjaxLibrary/jQuery.print.js" type="text/javascript"></script>
        <div class="header">
            <div class="main">
                <div class="header-t">
                    <div class="logo">
                    </div>
                    <div class="menu-top">
                        <ul>
                            <li>
                                <asp:LinkButton runat="server" ID="btnNewCustomer" Enabled="true" OnClick="btnNewCustomer_Click"><img src="../images/icon-user.png" alt=""/><span>Customers<br /><b></b></span></asp:LinkButton>
                            </li>
                        </ul>
                        <div>
                            <table>
                                <tr>
                                    <td style="width: 10px"></td>
                                    <td style="text-align: center; vertical-align: central">
                                        <strong>
                                            <label style="font-size: 17px; font-weight: bold;">
                                                Customer Name</label></strong>
                                    </td>
                                    <td style="width: 5px"></td>
                                    <td>
                                        <asp:TextBox ID="txtNewCustomer" runat="server" class="sku-c-input"></asp:TextBox></td>
                                    <td style="width: 20px"></td>
                                    <td><strong>
                                        <label style="font-size: 17px; font-weight: bold;">
                                            Contact Number</label></strong></td>
                                    <td style="width: 5px"></td>
                                    <td>
                                        <asp:TextBox ID="txtNewCustomerCOntactNumer" runat="server" class="sku-c-input"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="ftbePhoneNo" ValidChars=",0123456789" runat="server"
                                            TargetControlID="txtNewCustomerCOntactNumer">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="shadow-t-menu">
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="mainPOS">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:LinkButton ID="btnUpdateLimit" runat="server" OnClick="btnUpdateLimit_Click" />
                    <asp:HiddenField ID="hfNots" runat="server" />
                    <asp:HiddenField ID="hfCompanyName" runat="server" />
                    <asp:HiddenField ID="hfLocationName" runat="server" />
                    <asp:HiddenField ID="hfLocationPic" runat="server" />
                    <asp:HiddenField ID="hfPosReportType" runat="server" />
                    <asp:HiddenField ID="hfAddess" runat="server" />
                    <asp:HiddenField ID="hfaddress2" runat="server" />
                      <asp:HiddenField ID="CustomerNamePrint" runat="server" />
                      <asp:HiddenField ID="CustomerAddressPrint" runat="server" />
                      <asp:HiddenField ID="CustomerPhonPrint" runat="server" />
                    <asp:HiddenField ID="hfContactNo" runat="server" />
                    <%--<asp:HiddenField ID="hfMaxId" runat="server" EnableViewState="false"/>--%>
                    <asp:HiddenField ID="hfProduct" runat="server" Value="0" />
                    <asp:HiddenField ID="txtskuID" runat="server" />
                    <asp:HiddenField ID="tab" runat="server" />
                    <div style="z-index: 101; left: 612px; width: 100px; position: absolute; top: 369px; height: 100px">
                        &nbsp;<asp:Panel ID="Panel21" runat="server">
                            <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                <ProgressTemplate>
                                    <asp:ImageButton ID="ImageButton1" runat="server" Height="28px" ImageUrl="~/App_Themes/Granite/Images/image003.gif"
                                        Width="31px" />
                                    Wait Update
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </asp:Panel>
                    </div>
                    <div class="header-form">
                        <span style="width: 360px;"><strong>
                            <label style="font-size: 17px; font-weight: bold;">
                                Customer</label>
                        </strong>
                            <asp:DropDownList ID="ddlCustomer" runat="server" Height="30px" Width="265px" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged">
                            </asp:DropDownList>
                        </span><span style="width: 271px"><strong>
                            <label style="font-size: 17px; font-weight: bold;">
                                Payment Mode</label>
                        </strong>
                            <asp:DropDownList ID="DrpPayMode" runat="server" onchange="PaymentMode()" Width="140px"
                                Height="30px">
                                <asp:ListItem Selected="True" Value="214">Cash</asp:ListItem>
                                <asp:ListItem Value="217">Cash & Credit Card</asp:ListItem>
                                <asp:ListItem Value="215">Credit Card</asp:ListItem>
                                <asp:ListItem Value="218">Credit</asp:ListItem>
                            </asp:DropDownList>
                        </span><span style="width: 173px"><strong>
                            <label style="font-size: 17px; font-weight: bold;">
                                Discount</label>
                            <select id="DrpDiscount" runat="server" style="height: 30px; width: 90px">
                                <option value="0">% age</option>
                                <option value="1">Value</option>
                            </select>
                        </strong></span><span style="width: 475px;"><strong>
                            <asp:Label runat="server" ID="lblsaleforce" Text="Sales Person" Width="114px" Font-Bold="true"
                                Font-Size="17px"></asp:Label></strong>
                            <asp:DropDownList ID="ddsalesForce" runat="server" Width="140px" CssClass="DropList"
                                Height="30px">
                            </asp:DropDownList>
                            <input type="button" id="btnToggleMode" runat="server" value="SALE MODE" class="BtnModesale"
                                onclick="toggle(this);" />
                            <asp:HiddenField runat="server" ID="hfToggleMode" Value="SALE MODE" />
                        </span>
                    </div>
                    <div class="menu2">
                        <div class="main">
                            <ul>
                                <li class="sku-c">Item Code</li>
                                <li class="sperator"></li>
                                <li class="sku-name">Item Name</li>
                                <li class="sperator"></li>
                                <li class="size">Size</li>
                                <li class="sperator"></li>
                                <li class="qty">Quantity</li>
                                <li class="sperator"></li>
                                <li class="discount">Discount</li>
                                <li class="sperator"></li>
                                <li class="u-prize">Unit Price</li>
                                <li class="sperator"></li>
                                <li class="u-prize">Amount</li>
                                <li class="sperator"></li>
                                <li><span style="font-size: large; color: white; margin-left: 20px;">
                                    <asp:Label Width="180px" Height="25px" Text="" runat="server" ID="lblLoacation"></asp:Label></span></li>
                            </ul>
                        </div>
                    </div>
                    <div class="main">
                        <div class="r-pannel">
                            <span>
                                <asp:TextBox ID="txtskuCode" runat="server" class="sku-c-input" onkeypress="Calculate(event);"></asp:TextBox>
                                <cc1:AutoCompleteExtender ID="AutoComplete" runat="server" TargetControlID="txtskuCode"
                                    ServicePath="wsProductList.asmx" MinimumPrefixLength="4" CompletionInterval="500"
                                    UseContextKey="true" BehaviorID="AutoCompleteBehavior" CompletionSetCount="10"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="autocomplete_listItem"
                                    EnableCaching="true" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    OnClientItemSelected="ProductSelected" FirstRowSelected="true" ServiceMethod="GetPosProducts">
                                </cc1:AutoCompleteExtender>
                            </span><span>
                                <asp:TextBox ID="txtskuName" runat="server" class="sku-name-input" Enabled="False"
                                    Font-Bold="True"></asp:TextBox>
                                <asp:TextBox ID="txtcolor" runat="server" class="color-input" Enabled="False"></asp:TextBox>
                            </span><span>
                                <asp:TextBox ID="txtsize" runat="server" class="size-input" Enabled="False"></asp:TextBox>
                            </span><span>
                                <asp:TextBox ID="txtQuantity" runat="server" onkeypress="SetFocusTocode(event)" class="qty-input"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Custom"
                                    ValidChars="-0123456789." TargetControlID="txtQuantity" />
                            </span><span>
                                <asp:TextBox ID="txtDiscount" runat="server" class="discount-input"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Custom"
                                    ValidChars="0123456789." TargetControlID="txtDiscount" />
                            </span><span>
                                <asp:TextBox ID="txtUnitRate" runat="server" class="u-prize-input" Enabled="False"></asp:TextBox>
                            </span><span>
                                <asp:TextBox ID="txtTotalPrice" runat="server" class="u-prize-input" Enabled="False"></asp:TextBox>
                            </span>
                            <div class="clr">
                            </div>
                            <div class="grid">
                                <asp:Panel ID="Panel2" runat="server" Height="305px" ScrollBars="Vertical" Width="99.3%"
                                    BorderColor="Silver" BorderStyle="Groove" BorderWidth="3px">
                                    <asp:Label ID="lblfound" ForeColor="Red" Font-Size="Medium" runat="server"></asp:Label>
                                    <asp:Label ID="lblClosingStock" ForeColor="White" runat="server"></asp:Label>
                                    <asp:Table ID="dataTable" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed">
                                    </asp:Table>
                                    <asp:GridView ID="GrdPurchase" runat="server" ForeColor="SteelBlue" RowStyle-Height="30px"
                                        Visible="false" BackColor="White" HorizontalAlign="Center" AutoGenerateColumns="False"
                                        BorderColor="White" ShowHeader="False" Width="100%" EnableModelValidation="True">
                                        <Columns>
                                            <asp:BoundField DataField="SKU_ID" HeaderText="SKU_ID">
                                                <FooterStyle VerticalAlign="Middle" />
                                                <HeaderStyle CssClass="HidePanel" VerticalAlign="Middle"></HeaderStyle>
                                                <ItemStyle CssClass="HidePanel" VerticalAlign="Middle"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SKU_CODE" HeaderText="Item Code">
                                                <FooterStyle VerticalAlign="Middle" />
                                                <HeaderStyle VerticalAlign="Middle" />
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" BorderColor="Silver" BorderWidth="2px"
                                                    BorderStyle="Solid" Font-Bold="true" Font-Size="16px" Width="128px"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SKU_NAME" HeaderText="Item Name">
                                                <FooterStyle VerticalAlign="Middle" />
                                                <HeaderStyle VerticalAlign="Middle" />
                                                <ItemStyle HorizontalAlign="Left" BorderColor="Silver" BorderWidth="2px" BorderStyle="Solid"
                                                    VerticalAlign="Middle" Font-Bold="true" Font-Size="16px" Width="270px"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="COLOR" HeaderText="COLOR">
                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" Font-Bold="true"
                                                    VerticalAlign="Middle" HorizontalAlign="Left" Font-Size="16px" Width="90px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PACKSIZE" HeaderText="PACKSIZE">
                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Left"
                                                    VerticalAlign="Middle" Font-Bold="true" Font-Size="16px" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="QUANTITY_UNIT" HeaderText="QUANTITY_UNIT">
                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Left"
                                                    VerticalAlign="Middle" Font-Bold="true" Font-Size="16px" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="STANDARD_DISCOUNT" HeaderText="DISCOUNT" DataFormatString="{0:F2}">
                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Right"
                                                    VerticalAlign="Middle" Font-Bold="true" Font-Size="16px" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UNIT_PRICE" HeaderText="PRICE" DataFormatString="{0:F2}">
                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Right"
                                                    VerticalAlign="Middle" Font-Bold="true" Font-Size="16px" Width="90px"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NET_AMOUNT" HeaderText="Amount" DataFormatString="{0:F2}">
                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Right"
                                                    VerticalAlign="Middle" Font-Bold="true" Font-Size="16px" Width="65px"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Amount" HeaderText="Amount">
                                                <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                <ItemStyle CssClass="HidePanel"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CHECK_DELETE" HeaderText="CHECK_DELETE">
                                                <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                <ItemStyle CssClass="HidePanel"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnDelete" runat="server" BorderColor="Red" Text="Void" OnClientClick="javascript:return confirm('Are you sure you want to Void/Unvoid?');return false;"
                                                        CommandName="Delete"></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle BorderColor="Silver" HorizontalAlign="Center" BorderWidth="2px" BorderStyle="Solid"
                                                    Width="30px" Font-Bold="true" ForeColor="Red" Font-Overline="true" Font-Size="14px"></ItemStyle>
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle Height="30px" />
                                    </asp:GridView>
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="l-pannel">
                            <ul>
                                <li><span></span>
                                    <asp:Image runat="server" ID="imgSKU" ImageUrl="../images/cloth.png" AlternateText="No Image Found" />
                                    <span></span></li>
                                <li>
                                    <label>
                                        Gross Sale</label>
                                    <asp:TextBox ID="txtGrossAmount" runat="server"></asp:TextBox>
                                </li>
                                <li><span></span>
                                    <label>
                                        Discount &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp Authorised By</label>
                                    <asp:TextBox ID="numtxtTotalExtraDiscnt" runat="server" Width="112" ValidationGroup="NumbersOnly"></asp:TextBox>
                                    <asp:TextBox ID="txtAuthorisedBy" runat="server" Width="112" TabIndex="20" onkeypress="FocusToCash(event)"></asp:TextBox>
                                </li>
                                <li><span></span>
                                    <label>
                                        Sales Tax</label>
                                    <asp:TextBox ID="numTxtTotalGST" runat="server"></asp:TextBox>
                                </li>
                                <li><span></span>
                                    <label>
                                        Net Amount</label>
                                    <asp:TextBox ID="numTxtTotlAmnt" runat="server"></asp:TextBox>
                                </li>
                                <li><span></span>
                                    <label>
                                        Cash Received</label>
                                    <asp:TextBox ID="txtCashRecieved2" runat="server" onkeypress="CalculateBalance(event);"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Custom"
                                        ValidChars="0123456789." TargetControlID="txtCashRecieved2" />
                                </li>
                                <li><span></span>
                                    <label>
                                        Balance</label>
                                    <asp:TextBox ID="txtBalance" runat="server"></asp:TextBox>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <div class="footer" style="height: 200px;">
                        <div class="main">
                            <div class="address">
                                <table>
                                    <tr>
                                        <td>
                                            <h2>Sales Report
                                            </h2>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Report Type: </b>
                                        </td>
                                        <td><span>
                                            <asp:DropDownList ID="ddlReportType" runat="server" Width="160px" CssClass="DropList">
                                                <asp:ListItem Text="Summary" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Detail Report" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                        </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Sales Person:</b></td>
                                        <td><span>
                                            <asp:DropDownList ID="ddl_saleforce2" runat="server" Width="160px" CssClass="DropList">
                                            </asp:DropDownList>
                                        </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>From Date:</b></td>
                                        <td><span>
                                            <asp:TextBox ID="txtstartDate" runat="server" Width="158px" MaxLength="11"></asp:TextBox>
                                            <%--<asp:ImageButton ID="ibnstartDate" runat="server" Width="16px" ImageUrl="~/App_Themes/Granite/Images/date.gif">
                                    </asp:ImageButton>--%>
                                        </span>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td><b>To Date:</b></td>
                                        <td><span>
                                            <asp:TextBox ID="txtEndDate" runat="server" Width="158px" MaxLength="11"></asp:TextBox>
                                            <%--<asp:ImageButton ID="ibnEndDate" runat="server" Width="16px" ImageUrl="~/App_Themes/Granite/Images/date.gif">
                                    </asp:ImageButton>--%>
                                        </span>
                                            <%--
                            <cc1:CalendarExtender ID="CEEndDate" runat="server" TargetControlID="txtEndDate"
                            PopupButtonID="ibnEndDate" Format="dd-MMM-yyyy" PopupPosition="TopLeft" />--%>
                                        </td>
                                    </tr>
                                </table>

                                <p>
                                    <span style="margin-top: 2px;">&nbsp;&nbsp;
                                    <asp:Button runat="server" ID="btnViewSalesReport" CssClass="view" ToolTip="View"
                                        OnClick="btnViewSalesReport_Click" />
                                    </span><span></span>
                                </p>

                            </div>
                            <div class="shadow-footer">
                            </div>
                            <div class="user-login">
                                <img src="../images/user-login.png" alt="" />
                                <span width="200px">
                                    <h2>User Login</h2>
                                    <h3>
                                        <asp:Label runat="server" ID="lbluserlogin" Text="Administrator"></asp:Label></h3>
                                    <br />
                                    <br />
                                    <p>
                                    </p>
                                    <p>
                                        <b>Working Date:</b>
                                        <h3>
                                            <asp:Label runat="server" ID="lblCurrentWorkingDate" Text="Temp"></asp:Label>
                                        </h3>
                                        <br />
                                        <br />
                                        <p>
                                        </p>
                                        <p>
                                            <b>Loged in at</b><br />
                                            <asp:Label ID="lbllogintimedate" runat="server" Text="time"></asp:Label>
                                        </p>
                                        <p>
                                        </p>
                                        <p>
                                        </p>
                                        <p>
                                        </p>
                                        <p>
                                        </p>
                                        <p>
                                        </p>
                                    </p>
                                </span>
                            </div>
                            <div class="shadow-footer">
                            </div>
                            <div class="free-sku">
                                <h2></h2>
                                <div style="width: 350px;">
                                    <table style="width: 350px;">

                                        <tr>
                                            <td>
                                                <h2>Credit Ceiling :</h2>
                                            </td>
                                            <td>
                                                <h2>
                                                    <asp:Label ID="lblCreditLimit" runat="server"></asp:Label></h2>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <h2>Ledger Balance : </h2>
                                            </td>
                                            <td>
                                                <h2>
                                                    <asp:Label ID="lblLedgerBalance" runat="server"></asp:Label></h2>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 55%;">
                                                <h2>Balance Ceiling :</h2>
                                            </td>
                                            <td style="width: 45%;">
                                                <h2>
                                                    <asp:Label ID="lblAllowLimit" runat="server"></asp:Label></h2>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <p>
                                    <asp:LinkButton runat="server" ID="btnSaveOrder" OnClick="btnSaveOrder_Click" ToolTip="Save"
                                        CssClass="save-n-print" AccessKey="S" Visible="false"></asp:LinkButton>
                                    <input type="button" id="LinkButton2" class="save-n-print" runat="server" onclick="PrintSaleInvoice();" />
                                    <asp:LinkButton runat="server" ID="btnVoid" OnClick="btnVoid_Click" CssClass="void">
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" ID="btnCancel" OnClick="btnCancel_Click" CssClass="exit"></asp:LinkButton>
                                </p>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnViewSalesReport" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div style="display: none; width: 2.6in;">
            <div id="dvSaleInvoice">
                <style type="text/css">
                    #dvSaleInvoice {
                        width: 2.6in;
                    }

                    #SaleInvoice {
                        width: 2.6in;
                    }

                    #CompanyName {
                        font-size: 20px;
                        font-weight: bold;
                    }

                    #SaleInvoiceText {
                        font-size: 14px;
                    }

                    #InvoiceDate {
                        font-weight: bold;
                    }

                    #CustomerType {
                        font-weight: bold;
                    }

                    #phoneNo {
                        font-weight: bold;
                    }

                    #hrSaleInvoiceHead {
                        border: #333333 solid 1px;
                    }

                    #invoiceDetail {
                        width: 98%;
                        margin-top: 10px;
                    }

                    #invoiceDetailBody tr td {
                        border: #333333 solid 1px;
                        font-family: Sans-Serif;
                        font-size: 12px;
                        padding: 2px;
                    }

                    .text-right {
                        text-align: right;
                    }

                    #invoiceDetailFoot tr td {
                        font-family: Sans-Serif;
                        font-size: 14px;
                        font-weight: bold;
                    }
                </style>
                <table id="SaleInvoice">
                    <tr>
                        <%-- <td colspan="2" align="center">
                        <span id="CompanyName">
                            <%=hfCompanyName.Value%></span>
                    </td>--%>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <span style="font-size: 10px;">
                                <img src="<%=hfLocationPic.Value%>" alt="" style="width: 1in; height: 1in" />
                            </span>

                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <span style="font-size: 10px;">
                                <%=hfContactNo.Value%></span>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="font-size: 12px; font-family:Verdana; font-style: italic;">
                            <label id="invoiceMode">
                                Sale Invoice</label>
                        </td>
                        <td>MOP: &nbsp;<span id="Span1" style="font-style: italic; font-family:Verdana; font-size: 12px;"><label
                            id="payMode">
                            Cash</label></span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr style="border: 1px solid black; background-color: Black; margin-bottom: 2px; margin-top: 1px;" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="font-style: italic; font-family: Verdana; font-size: 12px;">Date : <span style="font-style: normal; font-family: Sans-Serif; font-size: 12px;">
                            <%=lbllogintimedate.Text %></span>
                        </td>
                        <td style="font-style: italic; font-family: Verdana ;  font-size: 12px;">No of Units : &nbsp;<label style="font-style: normal; font-family: Sans-Serif; font-size: 12px;"
                            id="Units">
                        </label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="font-style: italic; font-family: Verdana; font-size: 12px;">Inv #
                            <%--<label style="font-style: normal; font-family: Sans-Serif; font-size: 12px;"
                            id="CustomerType">
                            <%=hfMaxId.Value %>
                        </label>--%>
                             <label id="hfMaxId" runat="server" style="font-style: normal; font-family: Sans-Serif; font-size: 14px;font-weight:bold;">
                                            </label>
                        </td>
                        <td style="font-style: italic; font-family: Verdana; font-size: 12px;">Saleman: <span style="font-style: normal; font-family: Sans-Serif; font-size: 12px;">
                            <label id="saleMan">
                                Cash</label></span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr style="border: 1px solid black; background-color: Black; margin-bottom: 2px; margin-top: 1px;" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="4" style="font-style: italic; font-size: 12px; margin-bottom: 5px;">CUSTOMER: &nbsp;<span id="customerName" style="font-style: normal; font-size: 12px;"><label
                            id="lblCustomerName">Walk In Customer</label></span>
                            <hr style="border: 1px solid black; background-color: Black; margin-bottom: -8px; margin-top: 2px;" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table id="invoiceDetail">
                                <thead id="invoiceDetailHead">
                                    <tr>
                                        <td style="text-align: left; font-size: 12px; font-family: Sans-Serif; width: 32%">Item Name
                                        </td>
                                        <td align="center" style="font-size: 12px; font-family: Sans-Serif; width: 10%">Qty
                                        </td>
                                        <td align="center" style="font-size: 12px; font-family: Sans-Serif; width: 15%">Price
                                        </td>
                                        <td align="center" style="font-size: 12px; font-family: Sans-Serif; width: 10%">Disc
                                        </td>
                                        <td align="center" style="font-size: 12px; font-family: Sans-Serif; width: 15%">Amount
                                        </td>
                                    </tr>
                                </thead>
                                <tbody id="invoiceDetailBody">
                                </tbody>
                                <tfoot id="invoiceDetailFoot">
                                    <tr style="display: none;">
                                        <td colspan="3" align="right">
                                            <label id="TotalValue-text">
                                                GROSS AMOUNT :
                                            </label>
                                        </td>
                                        <td align="right">
                                            <label id="TotalValue">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" colspan="3">
                                            <label id="GrandTotal-text">
                                                AMOUNT-DUE :
                                            </label>
                                        </td>
                                        <td align="right">
                                            <label id="DiscountTotal">
                                                0.0</label>
                                        </td>
                                        <td align="right">
                                            <label id="GrandTotal">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="right">
                                            <label id="Paid-text">
                                                CASH-PAID-IN :
                                            </label>
                                        </td>
                                        <td align="right" colspan="2">
                                            <label id="Paid">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="right">
                                            <label id="Balance-text">
                                                BALANCE :
                                            </label>
                                        </td>
                                        <td align="right" colspan="2">
                                            <label id="Balance">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">CASHIER : <span style="font-style: normal; font-family: Sans-Serif; font-size: 12px;">
                                            <%=lbluserlogin.Text%>
                                        </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <asp:Literal runat="server" ID="ltrnotes"></asp:Literal>
                                            <%-- <%=hfNots.Value%>--%>

                                        </td>

                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5" align="center">

                                            <hr style="border: 1px solid black; background-color: Black; margin-bottom: 2px; margin-top: 1px;" />
                                            <span style="font-size: 10px;">Powered by:FastServices.pk</span>
                                        </td>
                                    </tr>
                                </tfoot>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <br />
        </div>

        <%--   new report formate           --%>
        <div style="display: none; width: 8.0in;">
            <div id="dvSaleInvoice2">
                <style type="text/css">
                    #dvSaleInvoice2 {
                        width: 7.0in;
                    }

                    #SaleInvoice2 {
                        width: 7.0in;
                    }
                    .tdcustomercopy {
                    background-color:darkgray;
                    border:1px solid black !important;
                
                    
                    -webkit-print-color-adjust: exact; 
                        font-weight:bold !important;
                          font-size:14px;
                    }
                    .lblspecialnot {
                        background-color:#341d1d;
                        color:white;
                       
                    border:1px solid black !important;
                    -webkit-print-color-adjust: exact; 
                     font-weight:bold !important;
                     font-size:15px;
                     width:20%;
                    }
                    #CompanyName2 {
                        font-size: 20px;
                        font-weight: bold;
                    }

                    #SaleInvoiceText2 {
                        font-size: 14px;
                    }

                    #InvoiceDate2 {
                        font-weight: bold;
                    }

                    #CustomerType2 {
                        font-weight: bold;
                    }

                    #phoneNo2 {
                        font-weight: bold;
                    }

                    #hrSaleInvoiceHead2 {
                        border: #333333 solid 1px;
                    }

                    #invoiceDetail2 {
                        width: 100%;
                        margin-top: 10px;
                    }

                    #invoiceDetailHead2 tr td {
                        border: #333333 solid 1px;
                        font-family: Sans-Serif;
                        font-size: 14px;
                        padding: 2px;
                        font-weight:bold !important;
                       
                    }

                    #invoiceDetailBody2 tr td {
                        border: #333333 solid 1px;
                        font-family: Sans-Serif;
                        font-size: 12px;
                        padding: 2px;
                       
                    }

                    .text-right {
                        text-align: right;
                    }
                    .text-center {
                    text-align :center;}

                    #invoiceDetailFoot2 tr td {
                        border: #333333 solid 1px;
                        font-family: Sans-Serif;
                        font-size: 14px;
                        font-weight: bold !important;
                       
                    }
                   
                </style>
                <table id="SaleInvoice2">

                    <tr>
                        <td align="center" rowspan="4" width="10%" >
                            <span style="font-size: 10px;">
                                <img src="<%=hfLocationPic.Value%>" alt="" style="width:1in;height:0.8in"  />
                            </span>

                        </td>
                        <td width="60%"  >
                       
                           &nbsp; <span style="font-size: 10px;">
                                <%=hfLocationName.Value%></span>

                        </td>
                        <td class="tdcustomercopy" width="30%" rowspan="2" align="center">Customer Copy</td>
                    </tr>
                    <tr>
                        
                        <td style="font-style: italic; font-family: Sans-Serif; font-size: 12px;" align="left">&nbsp;<span style="font-size: 10px;">
                            <%=hfAddess.Value%></span>
                        </td>
                       
                    </tr>
                    <tr>
                         <td  style="font-style: italic; font-family: Sans-Serif; font-size: 12px;" align="left">&nbsp;Tell:<span style="font-size: 10px;">
                            <%=hfContactNo.Value%></span>
                        </td>
                        <td></td>                       
                    </tr>
                    <tr>
                        
                        <td style="font-style: italic; font-family: Sans-Serif; font-size: 12px;" align="left">&nbsp; Email:<span style="font-size: 10px;">
                            <%=hfaddress2.Value%></span>
                        </td>
                        <td ></td>
                    </tr>
                    <tr style="height:15px"><td colspan="3"></td> </tr>
                    <tr>
                        <td style="font-style: italic; font-family: Sans-Serif; font-size: 12px;" colspan="3">
                            <hr style="border: 1px solid ; background-color: #9ceeb1; margin-bottom: 2px; margin-top: 1px;" />
                        </td>
                    </tr>
                    <tr><td ></td><td align="center" style="font-size:14px ;font-weight:bold">Sale Invoice</td><td align="right">Page 1 of 1</td></tr>  
                                     <tr>
                        <td style="font-style: italic; font-family: Sans-Serif; font-size: 12px;" colspan="3">
                            <hr style="border: 1px solid ; background-color: #9ceeb1; margin-bottom: 2px; margin-top: 1px;" />
                        </td>
                    </tr>
                    <tr style="height:15px"><td colspan="3"></td> </tr>
                    <tr><td align="left">&nbsp;Invoice #</td>
                        <td align="left" style="font-style: italic; font-family: Sans-Serif; font-size: 14px;font-weight:bold;">
                            
                           <%-- <label style="font-style: normal; font-family: Sans-Serif; font-size: 14px;font-weight:bold;"
                            id="customername">
                            <%=hfMaxId.Value %>
                        </label>--%>
                             <label id="hfMaxId2" style="font-style: normal; font-family: Sans-Serif; font-size: 14px;font-weight:bold;">
                                            </label>
                        </td>
                        <td style=" font-family: Sans-Serif; font-size: 12px;">Date : &nbsp;[<span style="font-style: normal; font-family: Sans-Serif; font-size: 12px;">
                            <%=lbllogintimedate.Text %></span>]
                        </td>
                    </tr>
                    <tr><td align="left">&nbsp;Customer :</td>
                        <td align="left" style="font-style: italic; font-family: Sans-Serif; font-size: 12px;" colspan="2">
                            <span id="customerNameprint" style="font-style: normal; font-size: 12px;"><label
                            id="lblCustomerName222"><%=CustomerNamePrint.Value %></label></span>
                        </td>
                    </tr>
                     <tr><td></td>
                        <td style="font-style: italic; font-family: Sans-Serif; font-size: 12px;" colspan="2" valign="top">
                            <hr style="border: 1px solid ; background-color: #9ceeb1; margin-bottom: 2px; margin-top: 1px;" />
                        </td>
                    </tr>
                     <tr><td align="left">&nbsp;Address :</td>
                        <td align="left" style="font-style: italic; font-family: Sans-Serif; font-size: 12px;" colspan="2">
                            <span id="customerAddress" style="font-style: normal; font-size: 12px;"><label
                            id="lblCustomerAddress"><%=CustomerAddressPrint.Value %></label></span>
                        </td>
                    </tr>
                     <tr><td></td>
                        <td style="font-style: italic; font-family: Sans-Serif; font-size: 12px;" colspan="2" valign="top">
                            <hr style="border: 1px solid ; background-color: #9ceeb1; margin-bottom: 2px; margin-top: 1px;" />
                        </td>
                    </tr>
                     <tr><td align="left">&nbsp;Phone :</td>
                        <td align="left" style="font-style: italic; font-family: Sans-Serif; font-size: 12px;" colspan="2">
                            <span id="customerPhone" style="font-style: normal; font-size: 12px;"><label
                            id="lblCustomerPhone"><%=CustomerPhonPrint.Value %></label></span>
                        </td>
                    </tr>
                     <tr><td></td>
                        <td style="font-style: italic; font-family: Sans-Serif; font-size: 12px;" colspan="2" valign="top">
                            <hr style="border: 1px solid ; background-color: #9ceeb1; margin-bottom: 2px; margin-top: 1px;" />
                        </td>
                    </tr>

                     <tr><td align="left">&nbsp;Note :</td>
                        <td align="left" style="font-style: italic; font-family: Sans-Serif; font-size: 12px;" colspan="2">
                            <span id="customerNote" style="font-style: normal; font-size: 12px;"><label
                            id="lblCustomerNote"></label></span>
                        </td>
                    </tr>
                     <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                     <tr>
                        <td colspan="3">
                            <table id="invoiceDetail2">
                                <thead id="invoiceDetailHead2">
                                    <tr>
                                        <td style=" width: 5%">No.
                                        </td>
                                         <td style="width: 10%">Item ID
                                        </td>
                                        <td align="center" style=" width: 40%">Item Description
                                        </td>
                                        <td align="center" style=" width:8%">Qty
                                        </td>
                                        <td align="center" style=" width: 12%">Rate
                                        </td>
                                         <td align="center" style=" width: 10%">Dis
                                        </td>
                                        <td align="center" style=" width: 15%">Amount
                                        </td>
                                    </tr>
                                </thead>
                                <tbody id="invoiceDetailBody2">
                                </tbody>
                                <tfoot id="invoiceDetailFoot2">
                                  
                                    <tr style="height:55px">
                                        <td colspan="5" align="left" >
                                           <label id="GrandTotalText">
                                            </label>
                                        </td>
                                        <td align="right">
                                            <label id="GrandTotal-text2">
                                                Total
                                            </label>
                                        </td>

                                        <td align="right">
                                            <label id="GrandTotal2">
                                            </label>
                                        </td>
                                    </tr>

                                </tfoot>
                            </table>
                        </td>
                    </tr>
                    <tr><td colspan="3">&nbsp;</td></tr>
                    <tr align="center">
                        <td colspan="3" align="center">
                           
                                        <table width="100%" style="align-self:center;align-items:center;align-content:center">
                                            <tr style="border:1px solid black;">
                                                <td width="2%"></td>
                                                <td width="98%">
                                                    <table>
                                                        <tr>
                                                            <td  height="50px" width="20%" class="lblspecialnot" >
                                                                Special Note 

                                                            </td>
                                                            <td width="80%"></td>

                                                        </tr>
                                                        <tr style="height:5px">
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                            
                        </td>
                    </tr>
                     <tr style="height:80px"><td colspan="3">&nbsp;</td></tr>
                    <tr>
                        <td colspan="3" align="left"  valign="bottom"> <hr   style="border: 1px solid ; width:170px; background-color: #9ceeb1; margin-bottom: 2px; margin-top: 1px;" /></td>
                     
                    </tr>
                     <tr>
                        <td align="left" colspan="3" valign="top">Signature </td>
                      
                    </tr>
                </table>
            </div>
            <br />
            <br />
        </div>
        <%-- new report formate --%>
    </form>
</body>
</html>
