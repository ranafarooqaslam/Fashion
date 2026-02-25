<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmOrderPOS.aspx.cs" Inherits="Forms_frmOrderPOS" %>
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
    <link href="../AjaxLibrary/select2/dist/css/select2.min.css" rel='stylesheet' type='text/css'>
    <script language="JavaScript" type="text/javascript">

        function CalculatePromotion(OrderedSKU_ID,OrderedQty,OrderedRate) {
            var tblPromotion = document.getElementById("hftblPromotion").value;
            tblPromotion = eval(tblPromotion);
            var IsValidGroup = 0;
            var Discount = 0;
            for (var i = 0, len = tblPromotion.length; i < len; ++i) {
                if (document.getElementById("<%= hfPromotionClass.ClientID %>").value == tblPromotion[i].CUSTOMER_VOLUMECLASS_ID) {
                    if (tblPromotion[i].SKU_GROUP_ID !== '') {

                        IsValidGroup = 0;
                        var tblGroupDetail = document.getElementById("hftblGroupDetail").value;
                        tblGroupDetail = eval(tblGroupDetail);
                        for (var j = 0, lenj = tblGroupDetail.length; j < lenj; ++j) {
                            if (tblGroupDetail[j].SKU_ID == OrderedSKU_ID &&
                                tblGroupDetail[j].SKU_GROUP_ID == tblPromotion[i].SKU_GROUP_ID) {
                                IsValidGroup = 1;
                                break;
                            }
                        }
                        if (IsValidGroup) {
                            if (tblPromotion[i].BASKET_ON == 82) {
                                if (parseFloat(OrderedQty) >= tblPromotion[i].MIN_VAL && parseFloat(OrderedQty) <= tblPromotion[i].MAX_VAL) {
                                    if (tblPromotion[i].DISCOUNT > 0) {
                                        Discount += (parseFloat(OrderedRate) * parseFloat(OrderedQty)) * (parseFloat(tblPromotion[i].DISCOUNT) / 100);
                                    }
                                    Discount += parseFloat(tblPromotion[i].OFFER_VALUE);
                                }
                            }
                            else {
                                if (parseFloat(OrderedQty) * parseFloat(OrderedRate) >= tblPromotion[i].MIN_VAL && parseFloat(OrderedQty) * parseFloat(OrderedRate) <= tblPromotion[i].MAX_VAL) {
                                    if (tblPromotion[i].DISCOUNT > 0) {
                                        Discount += (parseFloat(OrderedRate) * parseFloat(OrderedQty)) * (parseFloat(tblPromotion[i].DISCOUNT) / 100);
                                    }
                                    Discount += parseFloat(tblPromotion[i].OFFER_VALUE);
                                }
                            }
                        }
                    }
                    else {
                        if (tblPromotion[i].SKU_ID == OrderedSKU_ID) {
                            if (tblPromotion[i].BASKET_ON == 82) {
                                if (parseFloat(OrderedQty) >= tblPromotion[i].MIN_VAL && parseFloat(OrderedQty) <= tblPromotion[i].MAX_VAL) {
                                    if (tblPromotion[i].DISCOUNT > 0) {
                                        Discount += (parseFloat(OrderedRate) * parseFloat(OrderedQty)) * (parseFloat(tblPromotion[i].DISCOUNT) / 100);
                                    }
                                    Discount += parseFloat(tblPromotion[i].OFFER_VALUE);
                                }
                            }
                            else {
                                if (parseFloat(OrderedQty) * parseFloat(OrderedRate) >= tblPromotion[i].MIN_VAL && parseFloat(OrderedQty) * parseFloat(OrderedRate) <= tblPromotion[i].MAX_VAL) {
                                    if (tblPromotion[i].DISCOUNT > 0) {
                                        Discount += (parseFloat(OrderedRate) * parseFloat(OrderedQty)) * (parseFloat(tblPromotion[i].DISCOUNT) / 100);
                                    }
                                    Discount += parseFloat(tblPromotion[i].OFFER_VALUE);
                                }
                            }
                        }
                    }
                }
            }
            return Discount;
        }

        function ShowReport() {
            $.ajax(
            {
                type: "POST", //HTTP method
                url: "frmOrderPOS.aspx/LoadSaleReport2", //page/method name
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ type: document.getElementById("<%= ddlReportType.ClientID %>").value, userid: document.getElementById("<%= ddl_saleforce2.ClientID %>").value, startdate: $('#<%=txtstartDate.ClientID%>').val(), enddate: $('#<%=txtEndDate.ClientID%>').val()}),
                dataType: "json",
                success: LoadReport2
            });
        }
        function LoadReport2()
        {
            window.open("Default.aspx", "_blank", "toolbar=yes,scrollbars=yes,resizable=yes,top=500,left=500,width=400,height=400");
        }
        function LoadReport(data) {
            data = eval(data.d);
            var netamount = 0;
            var cashsale = 0;
            var netamount2 = 0;
            var cashsale2 = 0;
            var noofinvoices = 0;
            var grosssle = 0;
            var discount = 0;
            var salestax = 0;
            var creditcard = 0;
            var jazz = 0;
            var easypaisa = 0;
            var creditsale = 0;

            $('#rptSalesSummaryBody').empty();
            
            for (var i = 0, len = data.length; i < len; ++i) {
                noofinvoices += parseFloat(data[i].NumberOfInvouce);
                grosssle += parseFloat(data[i].TOTAL_AMOUNT);
                discount += parseFloat(data[i].DISCOUNT);
                salestax += parseFloat(data[i].GST_AMOUNT);
                creditcard += parseFloat(data[i].CREDIT_AMOUNT);
                jazz += parseFloat(data[i].NET_TOTAL);
                easypaisa += parseFloat(data[i].credit_sale);
                creditsale += parseFloat(data[i].CASH_SALE);

                netamount2 = parseFloat(data[i].TOTAL_AMOUNT) - parseFloat(data[i].DISCOUNT) + parseFloat(data[i].GST_AMOUNT);
                cashsale2 = parseFloat(data[i].TOTAL_AMOUNT) - parseFloat(data[i].DISCOUNT) + parseFloat(data[i].GST_AMOUNT) - parseFloat(data[i].CREDIT_AMOUNT) - parseFloat(data[i].NET_TOTAL) - parseFloat(data[i].CASH_SALE) - parseFloat(data[i].credit_sale);

                var row = $('<tr><td>' + data[i].USER_NAME + '</td><td class="text-right">' + data[i].NumberOfInvouce + '</td><td class="text-right">' + data[i].TOTAL_AMOUNT + '</td><td class="text-right">' + parseFloat(data[i].DISCOUNT).toFixed(0) + '</td><td class="text-right">' + parseFloat(netamount2).toFixed(0) + '</td><td class="text-right">' + data[i].CREDIT_AMOUNT + '</td><td class="text-right">' + data[i].NET_TOTAL + '</td><td class="text-right">' + data[i].credit_sale + '</td><td class="text-right">' + data[i].CASH_SALE + '</td><td class="text-right">' + parseFloat(cashsale2).toFixed(0) + '</td></tr>');
                $('#rptSalesSummaryBody').append(row);
            }

            netamount = parseFloat(grosssle) - parseFloat(discount) + parseFloat(salestax);
            cashsale = parseFloat(grosssle) - parseFloat(discount) + parseFloat(salestax) - parseFloat(creditcard) - parseFloat(jazz) - parseFloat(creditsale) - parseFloat(easypaisa);

            $("#lblNoOfInvoices").text(noofinvoices);
            $("#lblGrossSale").text(grosssle);
            $("#lblDiscount").text(discount);
            $("#lblSalesTax").text(salestax);            
            $("#lblNetAmount").text(netamount);
            $("#lblCreditCard").text(creditcard);
            $("#lblJazzCash").text(jazz);
            $("#lblEasyPaisa").text(easypaisa);
            $("#lblCreditSale").text(creditsale);
            $("#lblCashSale").text(cashsale);

            $("#lblTotalNoOfInvoice").text(noofinvoices);
            $("#lblTotalGrossSale").text(grosssle);
            $("#lblTotalDiscount").text(parseFloat(discount).toFixed(0));
            $("#lblTotalNetAmount").text(parseFloat(netamount).toFixed(0));
            $("#lblTotalCreditCard").text(creditcard);
            $("#lblTotalJazz").text(jazz);
            $("#lblTotalEasypaisa").text(easypaisa);
            $("#lblTotalCreditSale").text(creditsale);
            $("#lblTotalCashSale").text(parseFloat(cashsale).toFixed(0));

            $("#lblFromDate").text($('#<%=txtstartDate.ClientID%>').val());
            $("#lblToDate").text($('#<%=txtEndDate.ClientID%>').val());

            $.print("#dvSalesSummaryReport");
        }

        function btnCancelClick() {
            window.location.href = "Home.aspx";
        }
        function btnNewCustomerClick() {
            window.location.href = "frmNewCustomerPOS.aspx";
        }
        function btnVoidClick() {
            document.getElementById('<%=tab.ClientID%>').value = "";
        $('#<%=dataTable.ClientID%> tr').remove();
        Clear();
    }
        function SearchProduct() {
            $('#<%=lblfound.ClientID%>').text('');
        if (document.getElementById('<%=txtskuCode.ClientID%>').value == '') {
            if (document.getElementById("<%= hfItemType.ClientID %>").value != "1") {
                document.getElementById('<%=txtAuthorisedBy.ClientID%>').focus();
            }
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
                    document.getElementById("<%= hfTax.ClientID %>").value = item.GST_RATE_TP;
                    document.getElementById("<%= txtskuName.ClientID %>").value = item.SKU_NAME;
                    document.getElementById("<%= txtcolor.ClientID %>").value = item.COLOR;
                    document.getElementById("<%= txtsize.ClientID %>").value = item.PACKSIZE;
                    if (document.getElementById("<%= hfItemType.ClientID %>").value == "0") {
                        debugger;
                        if (document.getElementById("<%= txtUnitRate.ClientID %>").disabled == true) {
                            document.getElementById("<%= txtUnitRate.ClientID %>").value = item.TRADE_PRICE;
                        }
                        else { //if price is not entered then fill item price, else consider entered price.
                            var price = document.getElementById("<%= txtUnitRate.ClientID %>").value;
                            if (price == '' || price == undefined || isNaN(price) || price == null) {
                                price = 0;
                            }
                            if (price == 0) {
                                document.getElementById("<%= txtUnitRate.ClientID %>").value = item.TRADE_PRICE;
                            }
                        }
                    }

                    //if (item.FILEEXTENSION == 'noimage') {
                    //    $("[id$='imgSKU']").attr("src", '../images/cloth.png');

                    //} else {
                    //    $("[id$='imgSKU']").attr("src", '../SkuImages/' + item.SKU_ID + item.FILEEXTENSION);
                    //}
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
                                var CurrentQty = $(this).find("td:eq(4) input").val();
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
                    break;
                }
            }
            if (Productflag == 0) {
                $('#<%=lblfound.ClientID%>').text('Product not found.');
                if (document.getElementById("<%= hfItemType.ClientID %>").value == "1") {
                    document.getElementById('<%=ddlItem.ClientID%>').focus();
                }
                else {
                    document.getElementById('<%=txtskuCode.ClientID%>').focus();
                }
                return false;
            }
            if (Stockflag == 0) {
                $('#<%=lblfound.ClientID%>').text('Insufficient Stock !  ');
                if (document.getElementById("<%= hfItemType.ClientID %>").value == "1") {
                    document.getElementById('<%=ddlItem.ClientID%>').focus();
                }
                else {
                    document.getElementById('<%=txtAuthorisedBy.ClientID%>').focus();
                }

                return false;
            }

            if (document.getElementById("<%= hfItemType.ClientID %>").value == "1") {
                document.getElementById('<%=ddlItem.ClientID%>').focus();
            }
            else {
                document.getElementById('<%=txtAuthorisedBy.ClientID%>').focus();
            }
            return true;
        }
    }

    function storeTblValues() {
        var tableData = new Array();
        $('#<%=dataTable.ClientID%> tr').each(function (row, tr) {
            tableData[row] = {
                "SKU_Code": $(tr).find('td:eq(0)').text()
                , "SKU_Name": $(tr).find('td:eq(1)').text()
                , "QUANTITY_UNIT": $(tr).find('td:eq(4) input').val()
                , "STANDARD_DISCOUNT": $(tr).find('td:eq(5)').text()
                , "UNIT_PRICE": $(tr).find('td:eq(6)').text()
                , "NET_AMOUNT": $(tr).find('td:eq(7)').text()
                , "SKU_ID": $(tr).find('td:eq(8)').text()
                , "COLOR": $(tr).find('td:eq(2)').text()
                , "PACKSIZE": $(tr).find('td:eq(3)').text()
                , "AMOUNT": $(tr).find('td:eq(9)').text()
                , "GST_AMOUNT": $(tr).find('td:eq(10)').text()
                , "GST_RATE": $(tr).find('td:eq(11)').text()
                , "CHECK_DELETE": 0
                , "TST_AMOUNT": 0
                , "STANDARD_DISCOUNT_TEMP": 0
                , "STANDARD_DISCOUNT_PER": 0
                , "BATCH_NO": 0
            }
        });
        return tableData;
    }
        function GridQtyChanged(skuCode) {
            $('#<%=lblfound.ClientID%>').text('');
        var duplicrw;
        $('#<%=dataTable.ClientID%>').find('tr').each(function () {
            var td1 = $(this).find("td:eq(8)").text();
            if (skuCode == td1) {
                duplicrw = $(this);
                flag = 1;
            }
        });
        var e = document.getElementById('<%=DrpDiscount.ClientID%>');
        var discType = e.options[e.selectedIndex].value;
        var perDisc2;
        var b;
        var net;
        var table = document.getElementById('<%=dataTable.ClientID%>');
        var unitrate = duplicrw.find("td:eq(6)").text();
        var discount = duplicrw.find("td:eq(5)").text();
        var qty = duplicrw.find("td:eq(4) input").val();

        var stockqty = 0;
        var obj = jQuery.parseJSON($("#<%=hfProduct.ClientID %>").val());
        for (var i = 0; i < obj.length; i++) {
            var item = obj[i];
            if (item.SKU_CODE == duplicrw.find("td:eq(0)").text()) {
                stockqty = item.CLOSING_STOCK;
                break;
            }
        }

        if (parseFloat(qty) > parseFloat(stockqty)) {
            $('#<%=lblfound.ClientID%>').text('Insufficient Stock !  ');
            duplicrw.find("td:eq(4) input").focus();
            return;
        }

        if (document.getElementById("<%=hfAutoPromotion.ClientID %>").value.toLowerCase() == 'true') {
            perDisc2 = CalculatePromotion(duplicrw.find("td:eq(8)").text(), qty, unitrate);
            duplicrw.find("td:eq(5)").text((parseFloat(perDisc2)).toFixed(0));
            duplicrw.find("td:eq(7)").text(parseFloat(qty) * parseFloat(unitrate) - ((parseFloat(perDisc2)).toFixed(0)));
        }
        else {
            if (discType == 0) {
                perDisc2 = (document.getElementById('<%=txtUnitRate.ClientID%>').value * document.getElementById('<%=txtQuantity.ClientID%>').value) * (document.getElementById('<%=txtDiscount.ClientID%>').value / 100);
            } else {
                perDisc2 = parseFloat(document.getElementById('<%=txtDiscount.ClientID%>').value) * parseFloat(document.getElementById('<%=txtQuantity.ClientID%>').value);
            }
            duplicrw.find("td:eq(5)").text((parseFloat(perDisc2) + parseFloat(discount)).toFixed(0));
            duplicrw.find("td:eq(7)").text(parseFloat(qty) * parseFloat(unitrate) - ((parseFloat(perDisc2) + parseFloat(discount)).toFixed(0)));
        }

            duplicrw.find("td:eq(9)").text(parseFloat(qty) * parseFloat(unitrate));
            duplicrw.find("td:eq(10)").text(parseFloat(qty) * parseFloat(unitrate) * parseFloat(duplicrw.find("td:eq(11)").text()) / 100);
        b = 0;
        net = 0;
        tax = 0;
        var disc3 = 0;
        for (i = 0; i < table.rows.length; i++) {
            b = table.rows[i].cells[9].innerHTML;
            disc3 = (parseFloat(disc3) + parseFloat(table.rows[i].cells[5].innerHTML)).toFixed(2);
            net = (parseFloat(net) + parseFloat(b)).toFixed(2);
            if ($("#InvoiceCalculation").val() == "0") {
                tax = (parseFloat(tax) + parseFloat(table.rows[i].cells[10].innerHTML)).toFixed(2);
            }
        }
        document.getElementById('<%=txtGrossAmount.ClientID%>').value = net;
        document.getElementById('<%=numtxtTotalExtraDiscnt.ClientID%>').value = disc3;
        document.getElementById('<%=numTxtTotalGST.ClientID%>').value = tax;
        document.getElementById('<%=numTxtTotlAmnt.ClientID%>').value = (parseFloat(net) + parseFloat(tax) - parseFloat(disc3)).toFixed(2);
        tableData = storeTblValues();
        tableData = $.toJSON(tableData);
        document.getElementById('<%=tab.ClientID%>').value = tableData;

            var totalQty = 0;

            var dataAdded = JSON.parse(tableData);
            $.each(dataAdded, function (tr, value) {
                totalQty = totalQty + parseFloat(value.QUANTITY_UNIT);
            });

            $("#totalItems").text('Total Items: ' + dataAdded.length);
            $("#totalQty").text('Total Qty: ' + totalQty);
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
                var tax;
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
                var cell12;
                var cell13;
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
                    cell5.innerHTML = "<input type='text' id='txtqty" + table.rows.length + "' value='" + document.getElementById('txtQuantity').value + "' style='height:16px;'" + " onblur='GridQtyChanged(" + document.getElementById('<%=txtskuID.ClientID%>').value + ");'" + "/>";
                    cell5.style.width = "85px";

                    cell6 = row.insertCell(5);

                    if (document.getElementById("<%=hfAutoPromotion.ClientID %>").value.toLowerCase() == 'true') {
                        perDisc = CalculatePromotion(document.getElementById("<%= txtskuID.ClientID %>").value, document.getElementById('<%=txtQuantity.ClientID%>').value, document.getElementById('<%=txtUnitRate.ClientID%>').value);
                        cell6.innerHTML = (parseFloat(perDisc)).toFixed(2);
                    }
                    else {
                        if (discType == 0) {
                            perDisc = (document.getElementById('<%=txtUnitRate.ClientID%>').value * document.getElementById('<%=txtQuantity.ClientID%>').value) * (document.getElementById('<%=txtDiscount.ClientID%>').value / 100);
                            cell6.innerHTML = (parseFloat(perDisc)).toFixed(2);
                        } else {
                            if (mode == 'SALE MODE') {
                                perDisc = parseFloat(document.getElementById('<%=txtDiscount.ClientID%>').value) * parseFloat(document.getElementById('<%=txtQuantity.ClientID%>').value);
                                cell6.innerHTML = (parseFloat(perDisc)).toFixed(2);
                            } else {
                                perDisc = (parseFloat(document.getElementById('<%=txtDiscount.ClientID%>').value) * parseFloat(document.getElementById('<%=txtQuantity.ClientID%>').value));
                                cell6.innerHTML = (parseFloat(perDisc)).toFixed(2);
                            }
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

                    //Tax
                    cell11 = row.insertCell(10);
                    cell11.innerHTML = parseFloat(document.getElementById('<%=txtUnitRate.ClientID%>').value) * parseFloat(document.getElementById('<%=txtQuantity.ClientID%>').value) * (parseFloat(document.getElementById('<%=hfTax.ClientID%>').value) / 100);
                    cell11.style.display = 'none';

                    //TaxRate
                    cell12 = row.insertCell(11);
                    cell12.innerHTML = document.getElementById('<%=hfTax.ClientID%>').value;
                    cell12.style.display = 'none';

                    cell13 = row.insertCell(12);
                    cell13.innerHTML = '<input type="button" value = "X" style="color:white;background-color:red;cursor:pointer" onClick="Javacsript:deleteRow(this)">';

                    //Calculation for loop getting cell values
                    b = 0;
                    net = 0;
                    disc = 0;
                    tax = 0;
                    for (i = 0; i < table.rows.length; i++) {
                        b = table.rows[i].cells[9].innerHTML;
                        disc = parseFloat(table.rows[i].cells[5].innerHTML).toFixed(2);
                        net = (parseFloat(net) + parseFloat(b)).toFixed(2);
                        if ($("#InvoiceCalculation").val() == "0") {
                            tax = (parseFloat(tax) + parseFloat(table.rows[i].cells[10].innerHTML)).toFixed(2);
                        }
                    }
                    document.getElementById('<%=txtGrossAmount.ClientID%>').value = net;
                    document.getElementById('<%=numtxtTotalExtraDiscnt.ClientID%>').value = disc;
                    document.getElementById('<%=numTxtTotalGST.ClientID%>').value = tax;
                    document.getElementById('<%=numTxtTotlAmnt.ClientID%>').value = (parseFloat(net) + parseFloat(tax) - parseFloat(disc)).toFixed(2);

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
                        var qty = duplicrw.find("td:eq(4) input").val();

                        if (document.getElementById("<%=hfAutoPromotion.ClientID %>").value.toLowerCase() == 'true') {
                            perDisc2 = CalculatePromotion(document.getElementById("<%= txtskuID.ClientID %>").value, document.getElementById('<%=txtQuantity.ClientID%>').value, document.getElementById('<%=txtUnitRate.ClientID%>').value);
                        }
                        else {
                            if (discType == 0) {
                                perDisc2 = (document.getElementById('<%=txtUnitRate.ClientID%>').value * document.getElementById('<%=txtQuantity.ClientID%>').value) * (document.getElementById('<%=txtDiscount.ClientID%>').value / 100);
                            } else {
                                perDisc2 = parseFloat(document.getElementById('<%=txtDiscount.ClientID%>').value) * parseFloat(document.getElementById('<%=txtQuantity.ClientID%>').value);                                
                            }
                        }
                        duplicrw.find("td:eq(4) input").val(parseFloat(qty, 10) + parseFloat(document.getElementById('<%=txtQuantity.ClientID%>').value, 10));
                        duplicrw.find("td:eq(5)").text((parseFloat(perDisc2) + parseFloat(discount)).toFixed(0));
                        duplicrw.find("td:eq(7)").text((parseFloat(qty, 10) + parseFloat(document.getElementById('<%=txtQuantity.ClientID%>').value, 10)) * parseFloat(unitrate, 10) - ((parseFloat(perDisc2) + parseFloat(discount)).toFixed(0)));
                        duplicrw.find("td:eq(9)").text((parseFloat(qty, 10) + parseFloat(document.getElementById('<%=txtQuantity.ClientID%>').value, 10)) * parseFloat(unitrate, 10));
                        duplicrw.find("td:eq(10)").text((parseFloat(qty, 10) + parseFloat(document.getElementById('<%=txtQuantity.ClientID%>').value, 10)) * parseFloat(unitrate, 10) * (document.getElementById('<%=hfTax.ClientID%>').value / 100));
                        b = 0;
                        net = 0;
                        tax = 0;
                        var disc3 = 0;
                        for (i = 0; i < table.rows.length; i++) {
                            b = table.rows[i].cells[9].innerHTML;
                            disc3 = (parseFloat(disc3) + parseFloat(table.rows[i].cells[5].innerHTML)).toFixed(2);
                            net = (parseFloat(net) + parseFloat(b)).toFixed(2);
                            if ($("#InvoiceCalculation").val() == "0") {
                                tax = (parseFloat(tax) + parseFloat(table.rows[i].cells[10].innerHTML)).toFixed(2);
                            }
                        }
                        document.getElementById('<%=txtGrossAmount.ClientID%>').value = net;
                        document.getElementById('<%=numtxtTotalExtraDiscnt.ClientID%>').value = disc3;
                        document.getElementById('<%=numTxtTotalGST.ClientID%>').value = tax;
                        document.getElementById('<%=numTxtTotlAmnt.ClientID%>').value = (parseFloat(net) + parseFloat(tax) - parseFloat(disc3)).toFixed(2);
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
                        cell5.innerHTML = "<input type='text' id='txtqty" + table.rows.length + "' value='" + document.getElementById('txtQuantity').value + "' style='height:16px;'" + " onblur='GridQtyChanged(" + document.getElementById('<%=txtskuID.ClientID%>').value + ");'" + "/>";
                        cell5.style.width = "85px";

                        cell6 = row.insertCell(5);
                        if (document.getElementById("<%=hfAutoPromotion.ClientID %>").value.toLowerCase() == 'true') {
                            perDisc = CalculatePromotion(document.getElementById("<%= txtskuID.ClientID %>").value, document.getElementById('<%=txtQuantity.ClientID%>').value, document.getElementById('<%=txtUnitRate.ClientID%>').value);
                            cell6.innerHTML = (parseFloat(perDisc)).toFixed(2);
                        }
                        else {
                            if (discType == 0) {
                                perDisc = (document.getElementById('<%=txtUnitRate.ClientID%>').value * document.getElementById('<%=txtQuantity.ClientID%>').value) * (document.getElementById('<%=txtDiscount.ClientID%>').value / 100);
                                cell6.innerHTML = perDisc;
                            } else {
                                if (mode == 'SALE MODE') {
                                    perDisc = parseFloat(document.getElementById('<%=txtDiscount.ClientID%>').value) * parseFloat(document.getElementById('<%=txtQuantity.ClientID%>').value);
                                    cell6.innerHTML = (parseFloat(perDisc)).toFixed(2);
                                } else {
                                    perDisc = (parseFloat(document.getElementById('<%=txtDiscount.ClientID%>').value) * parseFloat(document.getElementById('<%=txtQuantity.ClientID%>').value));
                                    cell6.innerHTML = (parseFloat(perDisc)).toFixed(2);
                                }
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
                        cell10.style.display = 'none';;

                        //Tax
                        cell11 = row.insertCell(10);
                        cell11.innerHTML = parseFloat(document.getElementById('<%=txtUnitRate.ClientID%>').value) * parseFloat(document.getElementById('<%=txtQuantity.ClientID%>').value)  * (parseFloat(document.getElementById('<%=hfTax.ClientID%>').value)/100);
                        cell11.style.display = 'none';

                        //TaxRate
                        cell12 = row.insertCell(11);
                        cell12.innerHTML = document.getElementById('<%=hfTax.ClientID%>').value;
                        cell12.style.display = 'none';

                        cell13 = row.insertCell(12);
                        cell13.innerHTML = '<input type="button" value = "X" style="color:white;background-color:red;cursor:pointer" onClick="Javacsript:deleteRow(this)">';

                        //Calculation for loop getting cell values
                        b = 0;
                        net = 0;
                        tax = 0;
                        var disc2 = 0;
                        for (i = 0; i < table.rows.length; i++) {
                            b = table.rows[i].cells[9].innerHTML;
                            disc2 = (parseFloat(disc2) + parseFloat(table.rows[i].cells[5].innerHTML)).toFixed(2);
                            net = (parseFloat(net) + parseFloat(b)).toFixed(2);
                            if ($("#InvoiceCalculation").val() == "0") {
                                tax = (parseFloat(tax) + parseFloat(table.rows[i].cells[10].innerHTML)).toFixed(2);
                            }
                        }
                        document.getElementById('<%=txtGrossAmount.ClientID%>').value = net;
                        document.getElementById('<%=numtxtTotalExtraDiscnt.ClientID%>').value = disc2;
                        document.getElementById('<%=numTxtTotalGST.ClientID%>').value = tax;
                        document.getElementById('<%=numTxtTotlAmnt.ClientID%>').value = (parseFloat(net) + parseFloat(tax) - parseFloat(disc2)).toFixed(2);
                        tableData = storeTblValues();
                        tableData = $.toJSON(tableData);
                        document.getElementById('<%=tab.ClientID%>').value = tableData;
                    }
                }
            }

            var totalQty = 0;

            var dataAdded = JSON.parse(tableData);
            $.each(dataAdded, function(tr, value){
                totalQty = totalQty + parseFloat(value.QUANTITY_UNIT);
            });

            $("#totalItems").text('Total Items: ' + dataAdded.length);
            $("#totalQty").text('Total Qty: ' + totalQty);
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
            if (document.getElementById("<%=hfCanRefund.ClientID %>").value == "1") {
                document.getElementById("<%= btnToggleMode.ClientID %>").disabled = false;
            }
        }
        if (document.getElementById("<%= hfItemType.ClientID %>").value == "1") {
            document.getElementById('<%=ddlItem.ClientID%>').focus();
        }
        else {
            document.getElementById('<%=txtAuthorisedBy.ClientID%>').focus();
        }

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
            if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '0') {
                document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("1").toFixed(0);
            }
            else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '1') {
                document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("1").toFixed(1);
            }
            else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '2') {
                document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("1").toFixed(2);
            }
            else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '3') {
                document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("1").toFixed(3);
            }
            else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '4') {
                document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("1").toFixed(4);
            }
            else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '5') {
                document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("1").toFixed(5);
            }
            else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '6') {
                document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("1").toFixed(6);
            }
        }
        else {
            if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '0') {
                document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("-1").toFixed(0);
            }
            else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '1') {
                document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("-1").toFixed(1);
            }
            else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '2') {
                document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("-1").toFixed(2);
            }
            else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '3') {
                document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("-1").toFixed(3);
            }
            else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '4') {
                document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("-1").toFixed(4);
            }
            else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '5') {
                document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("-1").toFixed(5);
            }
            else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '6') {
                document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("-1").toFixed(6);
            };
        }

        document.getElementById('<%=txtcolor.ClientID%>').value = "";
        document.getElementById('<%=txtsize.ClientID%>').value = "";
        if (document.getElementById("<%= hfItemType.ClientID %>").value == "0") {
            document.getElementById('<%=txtUnitRate.ClientID%>').value = "";
        }
        if (document.getElementById("<%=hfCanRefund.ClientID %>").value == "1") {
            document.getElementById("<%=btnToggleMode.ClientID%>").disabled = true;
        }
    }
    function Calculate(e) {
        var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
        if (key == 13) {
            e.preventDefault();
            if (document.getElementById('<%=txtskuCode.ClientID%>').value != "") {
                addRow();
                document.getElementById("hfItemChange").value = 0;
                ClearControls();
                if (document.getElementById("<%= hfItemType.ClientID %>").value == "1") {
                    document.getElementById('<%=ddlItem.ClientID%>').focus();
                }
                else {
                    document.getElementById('<%=txtskuCode.ClientID%>').focus();
                }
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
            var extradisc = document.getElementById('<%=txtExtraDiscountValue.ClientID%>').value;
            var balce = 0;
            var mode = document.getElementById("<%= btnToggleMode.ClientID %>").value;
            if (mode == 'SALE MODE') {
                document.getElementById('<%=txtBalance.ClientID%>').value = (parseFloat(cashRcd) - parseFloat(netAmount)).toFixed(0);
            }
            else {
                if ((cashRcd > 0) && (netAmount < 0)) {

                    balce = (parseFloat(netAmount) - parseFloat(disc) - parseFloat(extradisc) + parseFloat(cashRcd)).toFixed(0);
                    document.getElementById('<%=txtBalance.ClientID%>').value = balce;
                }
                else if ((cashRcd > 0) && (netAmount > 0)) {

                    balce = (parseFloat(netAmount) - parseFloat(disc) - parseFloat(extradisc) - parseFloat(cashRcd)).toFixed(0);
                    document.getElementById('<%=txtBalance.ClientID%>').value = balce;
                }
        }
        document.getElementById('<%=btnSave.ClientID%>').focus();
        }
    }
    function Calculate2() {
        var cashRcd = document.getElementById('<%=txtCashRecieved2.ClientID%>').value;
        if (cashRcd == "") {
            cashRcd = 0;
        }
        var netAmount = document.getElementById('<%=numTxtTotlAmnt.ClientID%>').value;
        var disc = document.getElementById('<%=numtxtTotalExtraDiscnt.ClientID%>').value;
        var extradisc = document.getElementById('<%=txtExtraDiscountValue.ClientID%>').value;
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

                balce = (parseFloat(netAmount) - parseFloat(disc) - parseFloat(extradisc) + parseFloat(cashRcd)).toFixed(0);
                document.getElementById('<%=txtBalance.ClientID%>').value = balce;
            }
            else if ((cashRcd > 0) && (netAmount > 0)) {

                balce = (parseFloat(netAmount) - parseFloat(disc) - parseFloat(extradisc) - parseFloat(cashRcd)).toFixed(0);
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

        if (document.getElementById("<%= btnToggleMode.ClientID %>").value == 'REFUND MODE') {
            var qty = document.getElementById("<%= txtQuantity.ClientID %>").value;
            document.getElementById("<%= txtQuantity.ClientID %>").value = -Math.abs(qty); //If positive qty is entered
                                                                                 //in refund mode then turn that value into negative.
        }

        if (key == 13) {
            //if (document.getElementById("<%= hfItemType.ClientID %>").value == "1") {
                e.preventDefault();
                if (document.getElementById('<%=hfOpenRate.ClientID%>').value == "1") {
                    var purchaseprice = document.getElementById('<%=hfPurchasePrice.ClientID%>').value;
                    var saleprice = 0;
                    if (document.getElementById('<%=txtUnitRate.ClientID%>').value.length > 0) {
                        saleprice = document.getElementById('<%=txtUnitRate.ClientID%>').value;
                        if (parseFloat(saleprice) < parseFloat(purchaseprice)) {
                            alert('Price can not be less than Purchase Price');
                            return;
                        }
                    }
                }
                addRow();
                ClearControls();
            //}
            <%--else {
                if (document.getElementById("<%= hfItemType.ClientID %>").value == "1") {
                    document.getElementById('<%=ddlItem.ClientID%>').focus();
                }
                else {
                    document.getElementById('<%=txtAuthorisedBy.ClientID%>').focus();
                }
            }--%>
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
            var gross = 0;
            var discountextra = 0;
            var discount = 0;
            var gst = 0;
            if (document.getElementById("<%= txtGrossAmount.ClientID %>").value.length > 0) {
                gross = document.getElementById("<%= txtGrossAmount.ClientID %>").value;
            }
            if (document.getElementById("<%= txtExtraDiscountValue.ClientID %>").value.length > 0) {
                discountextra = document.getElementById("<%= txtExtraDiscountValue.ClientID %>").value;
            }
            if (document.getElementById("<%= numtxtTotalExtraDiscnt.ClientID %>").value.length > 0) {
                discount = document.getElementById("<%= numtxtTotalExtraDiscnt.ClientID %>").value;
            }
            if (document.getElementById("<%= numTxtTotalGST.ClientID %>").value.length > 0) {
                gst = document.getElementById("<%= numTxtTotalGST.ClientID %>").value;
            }
            document.getElementById("<%= numTxtTotlAmnt.ClientID %>").value = (parseFloat(gross) + parseFloat(gst) - parseFloat(discountextra) - parseFloat(discount)).toFixed(2);

            e.preventDefault();
            setTimeout(function () { document.getElementById("<%= txtCashRecieved2.ClientID %>").focus(); }, 10);
        }
    }
    function calculateNetAmount() {
        var gross = 0;
        var discountextra = 0;
        var discount = 0;
        var gst = 0;
        if (document.getElementById("<%= txtGrossAmount.ClientID %>").value.length > 0) {
            gross = document.getElementById("<%= txtGrossAmount.ClientID %>").value;
        }
        if (document.getElementById("<%= txtExtraDiscountValue.ClientID %>").value.length > 0) {
            discountextra = document.getElementById("<%= txtExtraDiscountValue.ClientID %>").value;
        }
        if (document.getElementById("<%= numtxtTotalExtraDiscnt.ClientID %>").value.length > 0) {
            discount = document.getElementById("<%= numtxtTotalExtraDiscnt.ClientID %>").value;
        }
        if (document.getElementById("<%= numTxtTotalGST.ClientID %>").value.length > 0) {
            gst = document.getElementById("<%= numTxtTotalGST.ClientID %>").value;
        }
        document.getElementById("<%= numTxtTotlAmnt.ClientID %>").value = (parseFloat(gross) + parseFloat(gst) - parseFloat(discountextra) - parseFloat(discount)).toFixed(2);
        setTimeout(function () { document.getElementById("<%= txtCashRecieved2.ClientID %>").focus(); }, 10);
    }
    function FocusToExtraDiscount(e) {
        var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
        if (key == 13) {
            e.preventDefault();
            setTimeout(function () { document.getElementById("<%= txtExtraDiscountPer.ClientID %>").focus(); }, 10);
            }
        }
        function FocusToExtraDiscountValue(e) {
            var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
            if (key == 13) {
                gross = 0;
                var discountper = 0;
                var discountextra = 0;
                var discount = 0;
                var gst = 0;
                if (document.getElementById("<%= txtGrossAmount.ClientID %>").value.length > 0) {
                gross = document.getElementById("<%= txtGrossAmount.ClientID %>").value;
            }
            if (document.getElementById("<%= txtExtraDiscountPer.ClientID %>").value.length > 0) {
                discountper = document.getElementById("<%= txtExtraDiscountPer.ClientID %>").value;
            }
            if (parseFloat(discountper) > 0) {
                document.getElementById("<%= txtExtraDiscountValue.ClientID %>").value = gross * discountper / 100;
            }
            if (document.getElementById("<%= txtExtraDiscountValue.ClientID %>").value.length > 0) {
                discountextra = document.getElementById("<%= txtExtraDiscountValue.ClientID %>").value;
            }
            if (document.getElementById("<%= numtxtTotalExtraDiscnt.ClientID %>").value.length > 0) {
                discount = document.getElementById("<%= numtxtTotalExtraDiscnt.ClientID %>").value;
            }
            if (document.getElementById("<%= numTxtTotalGST.ClientID %>").value.length > 0) {
                gst = document.getElementById("<%= numTxtTotalGST.ClientID %>").value;
            }
            document.getElementById("<%= numTxtTotlAmnt.ClientID %>").value = (parseFloat(gross) + parseFloat(gst) - parseFloat(discountextra) - parseFloat(discount)).toFixed(2);
            e.preventDefault();
            setTimeout(function () { document.getElementById("<%= txtExtraDiscountValue.ClientID %>").focus(); }, 10);
        }
    }
    function calculateExtraDiscount() {
        gross = 0;
        var discountper = 0;
        var discountextra = 0;
        var discount = 0;
        var gst = 0;
        if (document.getElementById("<%= txtGrossAmount.ClientID %>").value.length > 0) {
            gross = document.getElementById("<%= txtGrossAmount.ClientID %>").value;
        }
        if (document.getElementById("<%= txtExtraDiscountPer.ClientID %>").value.length > 0) {
            discountper = document.getElementById("<%= txtExtraDiscountPer.ClientID %>").value;
        }
        if (parseFloat(discountper) > 0) {
            document.getElementById("<%= txtExtraDiscountValue.ClientID %>").value = gross * discountper / 100;
        }
        if (document.getElementById("<%= txtExtraDiscountValue.ClientID %>").value.length > 0) {
            discountextra = document.getElementById("<%= txtExtraDiscountValue.ClientID %>").value;
        }
        if (document.getElementById("<%= numtxtTotalExtraDiscnt.ClientID %>").value.length > 0) {
            discount = document.getElementById("<%= numtxtTotalExtraDiscnt.ClientID %>").value;
        }
        if (document.getElementById("<%= numTxtTotalGST.ClientID %>").value.length > 0) {
            gst = document.getElementById("<%= numTxtTotalGST.ClientID %>").value;
        }
        document.getElementById("<%= numTxtTotlAmnt.ClientID %>").value = (parseFloat(gross) + parseFloat(gst) - parseFloat(discountextra) - parseFloat(discount)).toFixed(2);
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
                if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '0') {
                    document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("-1").toFixed(0);
                }
                else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '1') {
                    document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("-1").toFixed(1);
                }
                else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '2') {
                    document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("-1").toFixed(2);
                }
                else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '3') {
                    document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("-1").toFixed(3);
                }
                else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '4') {
                    document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("-1").toFixed(4);
                }
                else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '5') {
                    document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("-1").toFixed(5);
                }
                else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '6') {
                    document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("-1").toFixed(6);
                }

                document.getElementById("<%=btnToggleMode.ClientID %>").setAttribute("CssClass", "BtnModereturn");
                if (document.getElementById("<%= hfItemType.ClientID %>").value == "1") {
                    document.getElementById('<%=ddlItem.ClientID%>').focus();
                }
                else {
                    document.getElementById('<%=txtAuthorisedBy.ClientID%>').focus();
                }
            } else if (mode == 'REFUND MODE') {
                document.getElementById("<%= btnToggleMode.ClientID %>").value = "SALE MODE";
                document.getElementById("<%=hfToggleMode.ClientID %>").value = "SALE MODE";
                if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '0') {
                    document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("1").toFixed(0);
                }
                else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '1') {
                    document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("1").toFixed(1);
                }
                else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '2') {
                    document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("1").toFixed(2);
                }
                else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '3') {
                    document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("1").toFixed(3);
                }
                else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '4') {
                    document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("1").toFixed(4);
                }
                else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '5') {
                    document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("1").toFixed(5);
                }
                else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '6') {
                    document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("1").toFixed(6);
                }
                document.getElementById("<%= btnToggleMode.ClientID %>").setAttribute("CssClass", "BtnModesale");
                if (document.getElementById("<%= hfItemType.ClientID %>").value == "1") {
                    document.getElementById('<%=ddlItem.ClientID%>').focus();
                }
                else {
                    document.getElementById('<%=txtAuthorisedBy.ClientID%>').focus();
                }
            }
        }
function CheckCreditLimit() {
    var e = document.getElementById("<%= DrpPayMode.ClientID %>");
        var payMode = e.options[e.selectedIndex].value;
        if (payMode == "214") {
            var mode = document.getElementById("<%=btnToggleMode.ClientID%>").value;
            if (mode == 'SALE MODE') {
                var NetAmount = 0;
                try {
                    if (document.getElementById("<%= txtCashRecieved2.ClientID %>").value.length > 0) {
                        NetAmount = parseFloat(document.getElementById("<%= txtCashRecieved2.ClientID %>").value);
                    }
                } catch (e) {
                    NetAmount = 0;
                }
                if (NetAmount == null || NetAmount=='undefined')
                {
                    NetAmount = 0;
                }
                var extradisc = 0;
                if (document.getElementById('<%=txtExtraDiscountValue.ClientID%>').value.length > 0) {
                    extradisc = document.getElementById('<%=txtExtraDiscountValue.ClientID%>').value;
                }
                if (NetAmount == 0) {
                    alert('Please enter Payment');
                    document.getElementById("<%= txtCashRecieved2.ClientID %>").focus();
                    return false;
                }
                else if (parseFloat(NetAmount) < parseFloat(document.getElementById("<%= numTxtTotlAmnt.ClientID %>").value)) {
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
        <%-- //////////////////Region start added for balance calculation\\\\\\\\\\\\\\\\\\\\\--%>
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

                balce = (parseFloat(netAmount) - parseFloat(disc) - parseFloat(extradisc) + parseFloat(cashRcd)).toFixed(0);
                document.getElementById('<%=txtBalance.ClientID%>').value = balce;
            }
            else if ((cashRcd > 0) && (netAmount > 0)) {

                balce = (parseFloat(netAmount) - parseFloat(disc) - parseFloat(extradisc) - parseFloat(cashRcd)).toFixed(0);
                document.getElementById('<%=txtBalance.ClientID%>').value = balce;
            }
    }
        <%-- //////////////////Region end added for balance calculation\\\\\\\\\\\\\\\\\\\\\--%>

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

                var e = document.getElementById("<%= DrpPayMode.ClientID %>");
                var payModePrint = e.options[e.selectedIndex].value;
                if (payModePrint == '214') {
                    $("#Balance0Title").text('');
                    $("#Balance0Amunt").text('');
                }

                if (payModePrint == '217') {

                    $("#BalanceText").text('Credit Card Amount:');
                }
                else if (payModePrint == '230') {

                    $("#BalanceText").text('Jazz Cash Amount:');
                } else
                    if (payModePrint == '231') {

                        $("#BalanceText").text('Easy Paisa Amount:');
                    }
                    else
                        if (payModePrint == '215') {

                            $("#BalanceText").text('Credit Card Amount:');
                        }
                        else
                            if (payModePrint == '218') {

                                $("#BalanceText").text('Credit Amount:');
                            }
                            else {
                                $("#BalanceText").text('BALANCE:');
                            }
                $("#payMode").text(payMode.options[payMode.selectedIndex].text);
                var saleMan = document.getElementById("<%= ddsalesForce.ClientID %>");
                $("#saleMan").text(saleMan.options[saleMan.selectedIndex].text);
                var CustomerName = document.getElementById("<%= ddlCustomer.ClientID %>");
                if (document.getElementById("<%=txtNewCustomer.ClientID %>").value == '') {
                    $("#lblCustomerName").text(CustomerName.options[CustomerName.selectedIndex].text.substring(0, CustomerName.options[CustomerName.selectedIndex].text.indexOf("|")));
                }
                else {
                    $("#lblCustomerName").text(document.getElementById("<%=txtNewCustomer.ClientID %>").value);
                }
                $("#hfMaxId2").text($("#hfMaxId").text());
                var obj = jQuery.parseJSON($("#<%=hfCustomerList.ClientID %>").val());
                if (document.getElementById("<%=txtNewCustomer.ClientID %>").value == '') {
                    for (var i = 0; i < obj.length; i++) {
                        var item = obj[i];
                        if (item.CUSTOMER_ID === parseInt(document.getElementById("<%= ddlCustomer.ClientID %>").value)) {
                            $("#lblCustomerName222").text(item.CUSTOMER_NAME);
                            $("#lblCustomerAddress").text(item.ADDRESS);
                            $("#lblCustomerPhone").text(item.CONTACT_NUMBER);

                            break;
                        }
                    }
                }
                else {
                    $("#lblCustomerName222").text(document.getElementById("<%=txtNewCustomer.ClientID %>").value);
                    $("#lblCustomerAddress").text('');
                    $("#lblCustomerPhone").text(document.getElementById("<%=txtNewCustomerCOntactNumer.ClientID %>").value);
                }
                var Units = 0;
                $('#<%=dataTable.ClientID%>').find('tr').each(function () {
                    Units += parseFloat($(this).find("td:eq(4) input").val());
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
                var extradisc = 0;
                if (document.getElementById('<%=txtExtraDiscountValue.ClientID%>').value.length > 0) {
                    extradisc = document.getElementById('<%=txtExtraDiscountValue.ClientID%>').value;
                }
                var amountDue = document.getElementById('<%=numTxtTotlAmnt.ClientID%>').value;
                var paid = 0;
                if (document.getElementById('<%=txtCashRecieved2.ClientID%>').value == '')
                { } else {
                    paid = document.getElementById('<%=txtCashRecieved2.ClientID%>').value;
                }
                var balance = document.getElementById('<%=txtBalance.ClientID%>').value;
                if (balance < 0)
                { balance = balance * -1; }
                if (parseFloat(extradisc) > 0) {
                    $("#PrintExtraDiscount").text(parseFloat(extradisc).toFixed(0));
                }
                $("#TotalValue").text(gross);
                $("#DiscountTotal").text(parseFloat(discount).toFixed(0));
                $("#GrandTotal").text((parseFloat(amountDue) + parseFloat(extradisc)).toFixed(0));
                if (parseFloat(extradisc) > 0) {
                    $("#PrintNet").text(parseFloat($("#GrandTotal").text()).toFixed(0) - parseFloat(extradisc).toFixed(0));
                }
                $("#Paid").text(parseFloat(paid).toFixed(0));
                $("#Balance").text(parseFloat(balance).toFixed(0));
                if ($("#invoiceDetailBody tr").length > 0) {
                    $.print("#dvSaleInvoice");
                }
            }
        } else {

            if (CheckCreditLimit()) {
                var mode = document.getElementById("<%=btnToggleMode.ClientID%>").value;

                var payMode = document.getElementById("<%= DrpPayMode.ClientID %>");
                var e = document.getElementById("<%= DrpPayMode.ClientID %>");
                var payModePrint = e.options[e.selectedIndex].value;
                if (payModePrint == '217') {

                    $("#BalanceText").text('Credit Card Amount:');
                }
                else if (payModePrint == '230') {

                    $("#BalanceText").text('Jazz Cash Amount:');
                } else
                    if (payModePrint == '231') {

                        $("#BalanceText").text('Easy Paisa Amount:');
                    }
                    else
                        if (payModePrint == '215') {

                            $("#BalanceText").text('Credit Card Amount:');
                        }
                        else
                            if (payModePrint == '218') {

                                $("#BalanceText").text('Credit Amount:');
                            }
                            else {
                                $("#BalanceText").text('BALANCE:');
                            }
                var saleMan = document.getElementById("<%= ddsalesForce.ClientID %>");
                $("#saleMan2").text(saleMan.options[saleMan.selectedIndex].text);
                var CustomerName = document.getElementById("<%= ddlCustomer.ClientID %>");
                $("#lblCustomerName2").text(CustomerName.options[CustomerName.selectedIndex].text);
                $("#hfMaxId2").text($("#hfMaxId").text());
                var obj = jQuery.parseJSON($("#<%=hfCustomerList.ClientID %>").val());
                if (document.getElementById("<%=txtNewCustomer.ClientID %>").value == '') {
                    for (var i = 0; i < obj.length; i++) {
                        var item = obj[i];
                        if (item.CUSTOMER_ID === parseInt(document.getElementById("<%= ddlCustomer.ClientID %>").value)) {
                            $("#lblCustomerName222").text(item.CUSTOMER_NAME);
                            $("#lblCustomerAddress").text(item.ADDRESS);
                            $("#lblCustomerPhone").text(item.CONTACT_NUMBER);
                            break;
                        }
                    }
                }
                else {
                    $("#lblCustomerName222").text(document.getElementById("<%=txtNewCustomer.ClientID %>").value);
                    $("#lblCustomerAddress").text('');
                    $("#lblCustomerPhone").text(document.getElementById("<%=txtNewCustomerCOntactNumer.ClientID %>").value);
                }
                var orderedProducts = document.getElementById('<%=tab.ClientID%>').value;
                orderedProducts = eval(orderedProducts);
                $('#invoiceDetailBody2').empty(); // clear all skus  from invoice
                for (var i = 0, len = orderedProducts.length; i < len; i++) {
                    var row = $('<tr><td>' + (parseInt(i) + 1) + '</td><td>' + orderedProducts[i].SKU_Code + '</td><td>' + orderedProducts[i].SKU_Name + '</td><td class="text-center">' + orderedProducts[i].QUANTITY_UNIT + '</td><td class="text-right">' + parseFloat(orderedProducts[i].UNIT_PRICE).toFixed(2) + '</td><td class="text-right">' + orderedProducts[i].STANDARD_DISCOUNT + '</td><td class="text-right">' + parseFloat(orderedProducts[i].NET_AMOUNT).toFixed(2) + '</td></tr>');
                    $('#invoiceDetailBody2').append(row);
                }
                var gross = document.getElementById('<%=txtGrossAmount.ClientID%>').value;
                var discount = document.getElementById('<%=numtxtTotalExtraDiscnt.ClientID%>').value;
                var amountDue = document.getElementById('<%=numTxtTotlAmnt.ClientID%>').value;
                var extradisc = 0;
                if (document.getElementById('<%=txtExtraDiscountValue.ClientID%>').value.length > 0) {
                    extradisc = document.getElementById('<%=txtExtraDiscountValue.ClientID%>').value;
                }
                var paid = 0;
                if (document.getElementById('<%=txtCashRecieved2.ClientID%>').value == '')
                { } else {
                    paid = document.getElementById('<%=txtCashRecieved2.ClientID%>').value;
                }
                $("#PrintExtraDiscount2").text('0');
                if (parseFloat(extradisc) > 0) {
                    $("#PrintExtraDiscount2").text(parseFloat(extradisc).toFixed(0));
                }
                var balance = document.getElementById('<%=txtBalance.ClientID%>').value;
                var amountdueinword = number2words(amountDue);
                amountdueinword = amountdueinword + ' Rs. Only';
                $("#TotalValue2").text(gross);
                $("#DiscountTotal2").text(parseFloat(discount).toFixed(0));
                $("#GrandTotal2").text((parseFloat(amountDue) + parseFloat(extradisc)).toFixed(0));
                $("#Paid2").text(parseFloat(paid).toFixed(0));
                $("#Balance2").text(parseFloat(balance).toFixed(0));
                $("#GrandTotalText").text(amountdueinword);
                $("#PrintNet2").text('0');
                if (parseFloat(extradisc) > 0) {
                    $("#PrintNet2").text(parseFloat($("#GrandTotal2").text()).toFixed(0) - parseFloat(extradisc).toFixed(0));
                }
                else {
                    $("#PrintNet2").text(parseFloat($("#GrandTotal2").text()).toFixed(0));
                }
                if ($("#invoiceDetailBody2 tr").length > 0) {
                    $.print("#dvSaleInvoice2");
                }
            }
        }
    }
    function getcustomerDetail() {
        $.ajax(
            {
                type: "POST", //HTTP method
                url: "frmOrderPOS.aspx/LoadCustomerData", //page/method name
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: loadCustomer
            }
        );
    }
    function loadCustomer(data) {
        var data2 = JSON.stringify(data);
        var result = jQuery.parseJSON(data2.replace(/&quot;/g, '"'));
        data2 = eval(result.d);
        data2 = JSON.stringify(data2);
        $("#<%=hfCustomerList.ClientID %>").val(data2);

        data = eval(data.d);
        var listItems = "";
        $("[id$='ddlCustomer'] option").remove();
        for (var i = 0, len = data.length; i < len; ++i) {
            listItems += "<option value='" + data[i].CUSTOMER_ID + "'>" + data[i].CUSTOMER_DETAIL + "</option>";
        }
        $("[id$='ddlCustomer']").html(listItems);

        $("#lblAllowLimit").text('0');
        $("#lblCreditLimit").text('0');
        $("#lblLedgerBalance").text('0');
        document.getElementById("<%= hfPromotionClass.ClientID %>").value = data[0].PROMOTION_CLASS;
        $("#CustomerNamePrint").text(data[0].CUSTOMER_NAME);
        $("#CustomerAddressPrint").text(data[0].ADDRESS);
        $("#CustomerPhonPrint").text(data[0].CONTACT_NUMBER);
        var e = document.getElementById("<%= DrpPayMode.ClientID %>");
        var payMode = e.options[e.selectedIndex].value;
        if (payMode == "218") {
            $("#lblAllowLimit").text(parseFloat(data[0].ALLOWLIMIT) + parseFloat(data[0].USERLIMIT));
            $("#lblCreditLimit").text(data[0].ALLOWLIMIT);
            $("#lblLedgerBalance").text(data[0].USERLIMIT);
        }
    }
        function SaveInvoiceInDataBase() {
            if ($('#<%=hfIsCustomerInfoMandatory.ClientID%>').val() == '1')
            {
                if(document.getElementById("<%=txtNewCustomer.ClientID %>").value == '' || document.getElementById("<%=txtNewCustomerCOntactNumer.ClientID %>").value == '')
                {
                    alert('Enter Customer Name and Contact Number.');
                    if(document.getElementById("<%=txtNewCustomer.ClientID %>").value == '') {
                        document.getElementById("<%=txtNewCustomer.ClientID %>").focus();
                        return;
                    }
                    if (document.getElementById("<%=txtNewCustomerCOntactNumer.ClientID %>").value == '') {
                        document.getElementById("<%=txtNewCustomerCOntactNumer.ClientID %>").focus();
                        return;
                    }                    
                }
            }            
            if (CheckCreditLimit()) {
                $("#btnSave").attr("disabled", true);
                $.ajax
                    (
                        {
                            type: "POST", //HTTP method
                            url: "frmOrderPOS.aspx/InsertInvoice", //page/method name
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ orderedProducts: document.getElementById('<%=tab.ClientID%>').value, amountDue: document.getElementById('<%=txtGrossAmount.ClientID%>').value, author: document.getElementById('<%=txtAuthorisedBy.ClientID%>').value, discount: $('#<%=numtxtTotalExtraDiscnt.ClientID%>').val(), extradiscount: $('#<%=txtExtraDiscountValue.ClientID%>').val(), netAmount: $('#<%=numTxtTotlAmnt.ClientID%>').val(), paidIn: $('#<%=txtCashRecieved2.ClientID%>').val(), payType: document.getElementById("<%= DrpPayMode.ClientID %>").value, Gst: document.getElementById("<%=numTxtTotalGST.ClientID %>").value, manualId: document.getElementById("<%= hfToggleMode.ClientID %>").value, customerId: document.getElementById("<%= ddlCustomer.ClientID %>").value, saleForce: document.getElementById("<%= ddsalesForce.ClientID %>").value, NewCustomerNam: document.getElementById("<%=txtNewCustomer.ClientID %>").value, NewCustomerContactNumber: document.getElementById("<%=txtNewCustomerCOntactNumer.ClientID %>").value, CustomerName: $("#CustomerNamePrint").text(), CustomerContactNo: $("#CustomerPhonPrint").text(), InvoiceCalculation: $("#hfInvoiceCalculation").val() }),
                            success: invoiceSaved,
                            complete: function () {
                                $("#btnSave").attr("disabled", false);
                            },
                            error: invoiceNotSaved
                        }
                );
            }
        }
        function invoiceSaved() {
            getcustomerDetail();
            var hidid = $("#hfMaxId").text()
            hidid = parseInt(hidid) + 1;
            $("#hfMaxId").text(hidid);
            document.getElementById('<%=tab.ClientID%>').value = "";
            $('#<%=dataTable.ClientID%> tr').remove();
            Clear();
            window.open("Default.aspx", "_blank", "toolbar=yes,scrollbars=yes,resizable=yes,top=500,left=500,width=400,height=400");
        }
    function invoiceNotSaved() {
        alert('Some error occurred');
        Clear();
    }

    function Clear() {
        $('#<%=txtGrossAmount.ClientID%>').val('');
        $('#<%=txtAuthorisedBy.ClientID%>').val('');
        $('#<%=numtxtTotalExtraDiscnt.ClientID%>').val('');
        $('#<%=txtExtraDiscountPer.ClientID%>').val('');
        $('#<%=txtExtraDiscountValue.ClientID%>').val('');
        $('#<%=numTxtTotlAmnt.ClientID%>').val('');
        $('#<%=txtCashRecieved2.ClientID%>').val('');
        $('#<%=txtBalance.ClientID%>').val('');
        $('#<%=numTxtTotalGST.ClientID %>').val('');
        $('#<%=txtNewCustomer.ClientID %>').val('');
        $('#<%=txtNewCustomerCOntactNumer.ClientID %>').val('');
        $('#<%=DrpPayMode.ClientID %>').val('214');
        var mode = document.getElementById("<%=btnToggleMode.ClientID%>").value;
        if (mode == 'REFUND MODE') {
            if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '0') {
                document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("-1").toFixed(0);
            }
            else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '1') {
                document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("-1").toFixed(1);
            }
            else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '2') {
                document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("-1").toFixed(2);
            }
            else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '3') {
                document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("-1").toFixed(3);
            }
            else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '4') {
                document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("-1").toFixed(4);
            }
            else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '5') {
                document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("-1").toFixed(5);
            }
            else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '6') {
                document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("-1").toFixed(6);
            }
}
else {
    if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '0') {
                document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("1").toFixed(0);
            }
            else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '1') {
                document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("1").toFixed(1);
            }
            else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '2') {
                document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("1").toFixed(2);
            }
            else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '3') {
                document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("1").toFixed(3);
            }
            else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '4') {
                document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("1").toFixed(4);
            }
            else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '5') {
                document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("1").toFixed(5);
            }
            else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '6') {
                document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("1").toFixed(6);
            }
}
    if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '0') {
            document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("1").toFixed(0);
        }
        else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '1') {
            document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("1").toFixed(1);
        }
        else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '2') {
            document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("1").toFixed(2);
        }
        else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '3') {
            document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("1").toFixed(3);
        }
        else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '4') {
            document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("1").toFixed(4);
        }
        else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '5') {
            document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("1").toFixed(5);
        }
        else if (document.getElementById("<%=hfdecmalPlaces.ClientID %>").value == '6') {
            document.getElementById("<%=txtQuantity.ClientID %>").value = parseFloat("1").toFixed(6);
        }
}
function PaymentMode() {
    var e = document.getElementById("<%= DrpPayMode.ClientID %>");
        var payMode = e.options[e.selectedIndex].value;
        if (payMode == "215" || payMode == "218") {
            document.getElementById("<%= txtCashRecieved2.ClientID %>").value = "";
            document.getElementById("<%= txtBalance.ClientID %>").value = document.getElementById("<%= numTxtTotlAmnt.ClientID %>").value;
            document.getElementById("<%= txtCashRecieved2.ClientID %>").readOnly = true;
        }
        else {
            document.getElementById("<%= txtCashRecieved2.ClientID %>").readOnly = false;
        }
        if (document.getElementById("<%= hfItemType.ClientID %>").value == "1") {
            document.getElementById('<%=ddlItem.ClientID%>').focus();
        }
        else {
            document.getElementById('<%=txtAuthorisedBy.ClientID%>').focus();
        }

        $('#ddlCustomer').trigger('change');
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
                border: 1px solid #444;
            }

        :focus {
            outline: -webkit-focus-ring-color auto 1px;
        }

        input:focus {
            outline: none !important;
            border-color: #444 !important;
            box-shadow: 0 0 10px #719ECE !important;
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
        <script src="../AjaxLibrary/select2/dist/js/select2.min.js" type='text/javascript'></script>
        <script type="text/javascript">           

            function pageLoad() {                
                if(document.getElementById("<%=hfCanRefund.ClientID %>").value == "0")
                {
                    document.getElementById("<%=btnToggleMode.ClientID%>").disabled = true;
                }
                if (document.getElementById("<%=hfAutoPromotion.ClientID %>").value.toLowerCase() == 'true') {
                    document.getElementById("<%= txtDiscount.ClientID %>").disabled = true;
                    document.getElementById('<%=DrpDiscount.ClientID%>').disabled = true;
                    GetPromotion();
                    GetGroupDetail();
                }
                function GetPromotion() {
                    $.ajax(
                            {
                                type: "POST", //HTTP method
                                url: "frmOrderPOS.aspx/GetPromotionDetail", //page/method name
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: LoadPromotion
                            });
                }

                function LoadPromotion(data) {
                    data = JSON.stringify(data);
                    var result = jQuery.parseJSON(data.replace(/&quot;/g, '"'));
                    data = eval(result.d);
                    data = JSON.stringify(data);
                    document.getElementById("<%=hftblPromotion.ClientID %>").value = data;   
                }
                function GetGroupDetail() {
                    $.ajax(
                            {
                                type: "POST", //HTTP method
                                url: "frmOrderPOS.aspx/GetGroupDetail", //page/method name
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: LoadGroupDetail
                            });
                }

                function LoadGroupDetail(data) {
                    data = JSON.stringify(data);
                    var result = jQuery.parseJSON(data.replace(/&quot;/g, '"'));
                    data = eval(result.d);
                    data = JSON.stringify(data);
                    document.getElementById("<%=hftblGroupDetail.ClientID %>").value = data;   
                }
                const checkbox = $("#chkScan");
                checkbox.change(function (event) {
                    var checkbox = event.target;
                    if (checkbox.checked) {
                        document.getElementById('<%=hfItemType.ClientID%>').value = "0";
                        document.getElementById('<%=txtskuCode.ClientID%>').setAttribute("style", "display:block;");
                        document.getElementById('<%=txtskuName.ClientID%>').setAttribute("style", "display:block;");
                        document.getElementById('<%=ddlItem.ClientID%>').setAttribute("style", "display:none;");
                        document.getElementById("<%= txtUnitRate.ClientID %>").value = "";
                        var select2 = document.getElementsByClassName('select2 select2-container select2-container--default');
                        for (var i = 0; i < select2.length; i++) {
                            select2[i].style.display = 'none';
                        }

                    } else {
                        document.getElementById('<%=hfItemType.ClientID%>').value = "1";
                        document.getElementById('<%=txtskuCode.ClientID%>').setAttribute("style", "display:none;");
                        document.getElementById('<%=txtskuName.ClientID%>').setAttribute("style", "display:none;");
                        document.getElementById('<%=ddlItem.ClientID%>').setAttribute("style", "display:block;");                        
                        var select2 = document.getElementsByClassName('select2 select2-container select2-container--default');
                        for (var i = 0; i < select2.length; i++) {
                            select2[i].style.display = 'block';
                        }
                        $('#ddlItem').trigger('change');
                        document.getElementById('<%=ddlItem.ClientID%>').focus();
                    }
                });
                $("#ddlItem").select2();

                var select2 = document.getElementsByClassName('select2 select2-container select2-container--default');
                for (var i = 0; i < select2.length; i++) {
                    select2[i].style.display = 'none';
                }

                getcustomerDetail();
                if (document.getElementById('<%=hfOpenRate.ClientID%>').value == "1") {
                    document.getElementById("<%= txtUnitRate.ClientID %>").disabled = false;
                    var obj = jQuery.parseJSON($("#<%=hfProduct.ClientID %>").val());
                    for (var i = 0; i < obj.length; i++) {
                        var item = obj[i];
                        if (item.SKU_ID == $('#ddlItem').val()) {
                            document.getElementById('<%=txtskuCode.ClientID%>').value = item.SKU_CODE;
                            document.getElementById('<%=txtUnitRate.ClientID%>').value = item.TRADE_PRICE;
                            document.getElementById('<%=hfPurchasePrice.ClientID%>').value = item.DISTRIBUTOR_PRICE;
                            break;
                        }
                    }
                }
                if (document.getElementById('<%=hfItemType.ClientID%>').value == "1") {
                    document.getElementById('<%=txtskuCode.ClientID%>').setAttribute("style", "display:none;");
                    document.getElementById('<%=txtskuName.ClientID%>').setAttribute("style", "display:none;");
                }
                $('#ddlItem').change(function () {
                    document.getElementById("hfItemChange").value = 1;
                    var obj = jQuery.parseJSON($("#<%=hfProduct.ClientID %>").val());
                    for (var i = 0; i < obj.length; i++) {
                        var item = obj[i];
                        if (item.SKU_ID == $('#ddlItem').val()) {
                            document.getElementById('<%=txtskuCode.ClientID%>').value = item.SKU_CODE;
                            document.getElementById('<%=txtUnitRate.ClientID%>').value = item.TRADE_PRICE;
                            document.getElementById('<%=hfPurchasePrice.ClientID%>').value = item.DISTRIBUTOR_PRICE;
                            break;
                        }
                    }
                });

                $('#ddlCustomer').change(function () {
                    $("#lblAllowLimit").text('0');
                    $("#lblCreditLimit").text('0');
                    $("#lblLedgerBalance").text('0');
                    var obj = $("#<%=hfCustomerList.ClientID %>").val();
                    obj = eval(obj);
                    for (var i = 0; i < obj.length; i++) {
                        var cust = obj[i];
                        if (cust.CUSTOMER_ID == $('#ddlCustomer').val()) {
                            document.getElementById("<%= hfPromotionClass.ClientID %>").value = cust.PROMOTION_CLASS;
                            $("#CustomerNamePrint").text(cust.CUSTOMER_NAME);
                            $("#CustomerAddressPrint").text(cust.ADDRESS);
                            $("#CustomerPhonPrint").text(cust.CONTACT_NUMBER);
                            if (document.getElementById("<%= hfItemType.ClientID %>").value != "1") {
                                document.getElementById('<%=txtskuCode.ClientID%>').focus();
                            } else {
                                document.getElementById('<%=ddlItem.ClientID%>').focus();
                            }
                            var e = document.getElementById("<%= DrpPayMode.ClientID %>");
                            var payMode = e.options[e.selectedIndex].value;
                            if (payMode == "218") {
                                $("#lblAllowLimit").text(parseFloat(cust.ALLOWLIMIT) + parseFloat(cust.USERLIMIT));
                                $("#lblCreditLimit").text(cust.ALLOWLIMIT);
                                $("#lblLedgerBalance").text(cust.USERLIMIT);
                            }
                            break;
                        }
                    }
                });

                $(document).on('focus', '.select2', function (e) {
                    if (e.originalEvent) {
                        if (document.getElementById("hfItemChange").value == 1) {
                            $('#txtQuantity').focus();
                        }
                    }
                });
            }
            function QtyFocused(e) {
                e.select();
                document.getElementById("hfItemChange").value = 0;
            }
        </script>
        <div id="mainPOS">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="header">
                        <div class="main">
                            <div class="header-t">
                                <div class="logo">
                                </div>
                                <div class="menu-top">
                                    <ul>
                                        <li>
                                            <asp:LinkButton runat="server" ID="btnNewCustomer" OnClientClick="btnNewCustomerClick()"><img src="../images/icon-user.png" alt=""/><span>Customers<br /><b></b></span></asp:LinkButton>
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
                    <asp:HiddenField ID="hfCurrentTime" runat="server" />
                    <asp:HiddenField ID="hfProduct" runat="server" Value="0" />
                    <asp:HiddenField ID="txtskuID" runat="server" />
                    <asp:HiddenField ID="hfTax" runat="server" />
                    <asp:HiddenField ID="tab" runat="server" />
                    <asp:HiddenField ID="hfItemType" runat="server" Value="0" />
                    <asp:HiddenField ID="hfdecmalPlaces" runat="server" Value="0" />
                    <asp:HiddenField ID="hfCustomerList" runat="server" Value="" />
                    <asp:HiddenField ID="hfOpenRate" runat="server" Value="0" />
                    <asp:HiddenField ID="hfPurchasePrice" runat="server" Value="0" />
                    <asp:HiddenField ID="hfAutoPromotion" runat="server" Value="0" />
                    <asp:HiddenField ID="hftblPromotion" runat="server" Value="" />
                    <asp:HiddenField ID="hfPromotionClass" runat="server" Value="0" />
                    <asp:HiddenField ID="hftblGroupDetail" runat="server" Value="" />
                    <asp:HiddenField ID="hfIsCustomerInfoMandatory" runat="server" Value="0" />
                    <asp:HiddenField ID="hfInvoiceCalculation" runat="server" Value="0" />
                    <asp:HiddenField ID="hfCanRefund" runat="server" Value="1" />
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
                            <asp:DropDownList ID="ddlCustomer" runat="server" Height="30px" Width="265px">
                            </asp:DropDownList>
                        </span><span style="width: 250px"><strong>
                            <label style="font-size: 17px; font-weight: bold;">
                                Pay. Mode</label>
                        </strong>
                            <asp:DropDownList ID="DrpPayMode" runat="server" onchange="PaymentMode()" Width="140px"
                                Height="30px">
                                <asp:ListItem Selected="True" Value="214">Cash</asp:ListItem>
                                <asp:ListItem Value="217">Cash & Credit Card</asp:ListItem>
                                <asp:ListItem Value="215">Credit Card</asp:ListItem>
                                <asp:ListItem Value="218">Credit</asp:ListItem>
                                <asp:ListItem Value="230">Jazz Cash</asp:ListItem>
                                <asp:ListItem Value="231">Easy Paisa</asp:ListItem>
                                <asp:ListItem Value="232">Cash On Delivery</asp:ListItem>
                                <asp:ListItem Value="233">Bank Transfer</asp:ListItem>
                            </asp:DropDownList>
                        </span><span style="width: 220px"><strong>
                            <label style="font-size: 17px; font-weight: bold;">
                                Disc. Per Unit</label>
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
                    <div style="padding-left:25px;">
                        <asp:CheckBox ID="chkScan" runat="server" Text="By Scan" Checked="true"/>
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
                            </span>
                            <span>
                                <asp:DropDownList ID="ddlItem" runat="server" Width="525px" style="display:none;"></asp:DropDownList>
                                <asp:HiddenField runat="server" ID="hfItemChange" Value="0" />
                            </span>
                            <span>
                                <asp:TextBox ID="txtskuName" runat="server" class="sku-name-input" Enabled="False"
                                    Font-Bold="True"></asp:TextBox>
                                <asp:TextBox ID="txtcolor" runat="server" class="color-input" Enabled="False"></asp:TextBox>
                            </span>
                            <span>
                                <asp:TextBox ID="txtsize" runat="server" class="size-input" Enabled="False"></asp:TextBox>
                            </span><span>
                                <asp:TextBox ID="txtQuantity" runat="server" onkeypress="SetFocusTocode(event)" onfocus="QtyFocused(this,event);" class="qty-input"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Custom"
                                    ValidChars="-0123456789." TargetControlID="txtQuantity" />
                            </span><span>
                                <asp:TextBox ID="txtDiscount" runat="server" class="discount-input" onkeypress="SetFocusTocode(event)"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Custom"
                                    ValidChars="0123456789." TargetControlID="txtDiscount" />
                            </span><span>
                                <asp:TextBox ID="txtUnitRate" runat="server" class="u-prize-input" Enabled="False" onkeypress="SetFocusTocode(event)"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterType="Custom"
                                    ValidChars="0123456789." TargetControlID="txtUnitRate" />
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
                                <div style="height: 200px">
                                    <p style="color: red; font-size: 14px; margin: 5px;" id="totalItems">Total Items: 0</p>
                                    <p style="color: red; font-size: 14px; margin: 5px;" id="totalQty">Total Qty: 0</p>
                                </div>
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
                                    <asp:TextBox ID="numtxtTotalExtraDiscnt" runat="server" Width="110" ValidationGroup="NumbersOnly"></asp:TextBox>
                                    <asp:TextBox ID="txtAuthorisedBy" runat="server" Width="110" TabIndex="20" onkeypress="FocusToExtraDiscount(event)"></asp:TextBox>
                                </li>
                                <li><span></span>
                                    <label>
                                        Extra Discount &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp
                                    </label>
                                    <asp:TextBox ID="txtExtraDiscountPer" runat="server" Width="110" TabIndex="21" ValidationGroup="NumbersOnly" onkeypress="FocusToExtraDiscountValue(event)" placeholder="%age" onblur="calculateExtraDiscount()"></asp:TextBox>
                                    <asp:TextBox ID="txtExtraDiscountValue" runat="server" Width="110" TabIndex="22" onkeypress="FocusToCash(event)" placeholder="value" onblur="calculateNetAmount()"></asp:TextBox>
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
                                                <%--   <asp:ListItem Text="Detail Report" Value="2"></asp:ListItem>--%>
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
                                        </span>
                                        </td>
                                    </tr>
                                </table>

                                <p>
                                    <span style="margin-top: 2px;">&nbsp;&nbsp;
                                        <input type="button" id="btnViewSalesReport" class="view"  onclick="ShowReport();" />
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
                                    <input type="button" id="btnSave" class="save-n-print" runat="server" onclick="SaveInvoiceInDataBase();" />
                                    <asp:LinkButton runat="server" ID="btnVoid" OnClientClick="btnVoidClick()" CssClass="void">
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" ID="btnCancel" CssClass="exit" OnClientClick="btnCancelClick();"></asp:LinkButton>
                                </p>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
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
                        <td colspan="2" align="center">
                            <span style="font-size: 10px;">
                                <%=hfAddess.Value%></span>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="font-size: 14px;">
                            <label id="invoiceMode">Sale Invoice</label>
                        </td>
                        <td><span id="Span1" style="font-size: 14px;">
                            MOP: &nbsp; <label id="payMode"> Cash</label></span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr style="border: 1px solid black; background-color: Black; margin-bottom: 2px; margin-top: 1px;" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="font-style: italic; font-family: Verdana; font-size: 12px;">Date : <span style="font-style: normal; font-family: Sans-Serif; font-size: 12px;">
                            <%=lblCurrentWorkingDate.Text %>  <%=hfCurrentTime.Value %></span>
                        </td>
                        <td style="font-style: italic; font-family: Verdana; font-size: 12px;">No of Units : &nbsp;<label style="font-style: normal; font-family: Sans-Serif; font-size: 12px;"
                            id="Units">
                        </label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="font-style: italic; font-family: Verdana; font-size: 12px;">Inv #                            
                             <label id="hfMaxId" runat="server" style="font-style: normal; font-family: Sans-Serif; font-size: 14px; font-weight: bold;">
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
                                    <tr id="trExtraDisc">
                                        <td colspan="3" align="right">
                                            <label>
                                                EXTRA DISC :
                                            </label>
                                        </td>
                                        <td align="right" colspan="2">
                                            <label id="PrintExtraDiscount">
                                                0.0
                                            </label>
                                        </td>
                                    </tr>
                                    <tr id="trNetTotal">
                                        <td colspan="3" align="right">
                                            <label>
                                                NET TOTAL :
                                            </label>
                                        </td>
                                        <td align="right" colspan="2">
                                            <label id="PrintNet">
                                                0.0
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
                                            <label id="BalanceText">
                                                BALANCE :
                                            </label>
                                        </td>
                                        <td align="right" colspan="2">
                                            <label id="Balance">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="right">
                                            <label id="Balance0Title">
                                                BALANCE :
                                            </label>
                                        </td>
                                        <td align="right" colspan="2">
                                            <label id="Balance0Amunt">
                                                0
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
                        background-color: darkgray;
                        border: 1px solid black !important;
                        -webkit-print-color-adjust: exact;
                        font-weight: bold !important;
                        font-size: 14px;
                    }

                    .lblspecialnot {
                        background-color: #341d1d;
                        color: white;
                        border: 1px solid black !important;
                        -webkit-print-color-adjust: exact;
                        font-weight: bold !important;
                        font-size: 15px;
                        width: 20%;
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
                        font-weight: bold !important;
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
                        text-align: center;
                    }

                    #invoiceDetailFoot2 tr td {
                        border: #333333 solid 1px;
                        font-family: Sans-Serif;
                        font-size: 14px;
                        font-weight: bold !important;
                    }
                </style>
                <table id="SaleInvoice2">
                    <tr>
                        <td align="center" rowspan="4" width="10%">
                            <span style="font-size: 10px;">
                                <img src="<%=hfLocationPic.Value%>" alt="" style="width: 1in; height: 0.8in" />
                            </span>

                        </td>
                        <td width="60%">&nbsp; <span style="font-size: 10px;">
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
                        <td style="font-style: italic; font-family: Sans-Serif; font-size: 12px;" align="left">&nbsp;Tell:<span style="font-size: 10px;">
                            <%=hfContactNo.Value%></span>
                        </td>
                        <td></td>
                    </tr>
                    <tr>

                        <td style="font-style: italic; font-family: Sans-Serif; font-size: 12px;" align="left">&nbsp; Email:<span style="font-size: 10px;">
                            <%=hfaddress2.Value%></span>
                        </td>
                        <td></td>
                    </tr>
                    <tr style="height: 15px">
                        <td colspan="3"></td>
                    </tr>
                    <tr>
                        <td style="font-style: italic; font-family: Sans-Serif; font-size: 12px;" colspan="3">
                            <hr style="border: 1px solid; background-color: #9ceeb1; margin-bottom: 2px; margin-top: 1px;" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td align="center" style="font-size: 14px; font-weight: bold">Sale Invoice</td>
                        <td align="right">Page 1 of 1</td>
                    </tr>
                    <tr>
                        <td style="font-style: italic; font-family: Sans-Serif; font-size: 12px;" colspan="3">
                            <hr style="border: 1px solid; background-color: #9ceeb1; margin-top: 1px;" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">&nbsp;Invoice #</td>
                        <td align="left" style="font-style: italic; font-family: Sans-Serif; font-size: 14px; font-weight: bold;">
                            <label id="hfMaxId2" style="font-style: normal; font-family: Sans-Serif; font-size: 14px; font-weight: bold;"></label>
                        </td>
                        <td style="font-family: Sans-Serif; font-size: 12px;">Date : &nbsp;[<span style="font-style: normal; font-family: Sans-Serif; font-size: 12px;">
                            <%=lbllogintimedate.Text %></span>]
                        </td>
                    </tr>
                    <tr>
                        <td align="left">&nbsp;Customer :</td>
                        <td align="left" style="font-style: italic; font-family: Sans-Serif; font-size: 12px;" colspan="2">
                            <span id="customerNameprint" style="font-style: normal; font-size: 12px;">
                                <label
                                    id="lblCustomerName222">
                                </label>
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td style="font-style: italic; font-family: Sans-Serif; font-size: 12px;" colspan="2" valign="top">
                            <hr style="border: 1px solid; background-color: #9ceeb1; margin-bottom: 2px; margin-top: 1px;" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">&nbsp;Address :</td>
                        <td align="left" style="font-style: italic; font-family: Sans-Serif; font-size: 12px;" colspan="2">
                            <span id="customerAddress" style="font-style: normal; font-size: 12px;">
                                <label
                                    id="lblCustomerAddress">
                                </label>
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td style="font-style: italic; font-family: Sans-Serif; font-size: 12px;" colspan="2" valign="top">
                            <hr style="border: 1px solid; background-color: #9ceeb1; margin-bottom: 2px; margin-top: 1px;" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">&nbsp;Phone :</td>
                        <td align="left" style="font-style: italic; font-family: Sans-Serif; font-size: 12px;" colspan="2">
                            <span id="customerPhone" style="font-style: normal; font-size: 12px;">
                                <label
                                    id="lblCustomerPhone">
                                </label>
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td style="font-style: italic; font-family: Sans-Serif; font-size: 12px;" colspan="2" valign="top">
                            <hr style="border: 1px solid; background-color: #9ceeb1; margin-bottom: 2px; margin-top: 1px;" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <table id="invoiceDetail2">
                                <thead id="invoiceDetailHead2">
                                    <tr>
                                        <td style="width: 5%">No.
                                        </td>
                                        <td style="width: 10%">Item ID
                                        </td>
                                        <td align="center" style="width: 40%">Item Description
                                        </td>
                                        <td align="center" style="width: 8%">Qty
                                        </td>
                                        <td align="center" style="width: 12%">Rate
                                        </td>
                                        <td align="center" style="width: 10%">Dis
                                        </td>
                                        <td align="center" style="width: 15%">Amount
                                        </td>
                                    </tr>
                                </thead>
                                <tbody id="invoiceDetailBody2">
                                </tbody>
                                <tfoot id="invoiceDetailFoot2">

                                    <tr>
                                        <td colspan="4" align="left">                                            
                                        </td>
                                        <td align="right" colspan="2">
                                            <label id="GrandTotal-text2">
                                                Total :&nbsp;
                                            </label>
                                        </td>

                                        <td align="right">
                                            <label id="GrandTotal2">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr id="trExtraDisc2">
                                        <td colspan="4" align="left">
                                            
                                        </td>
                                        <td align="right" colspan="2">
                                            <label>
                                                Extra Disc. :&nbsp;
                                            </label>
                                        </td>
                                        <td align="right">
                                            <label id="PrintExtraDiscount2">
                                                0.0
                                            </label>
                                        </td>
                                    </tr>
                                    <tr id="trNetTotal2"  style="height: 55px">
                                        <td colspan="4" align="left">
                                            <label id="GrandTotalText">
                                            </label>
                                        </td>
                                        <td align="right" colspan="2">
                                            <label>
                                                Net Total :&nbsp;
                                            </label>
                                        </td>
                                        <td align="right">
                                            <label id="PrintNet2">
                                                0.0
                                            </label>
                                        </td>
                                    </tr>
                                </tfoot>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">&nbsp;
                        </td>
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

        <%-- Sales Summary Report --%>
        <div style="display: none; width: 2.6in;">
            <div id="dvSalesSummaryReport">
                <style type="text/css">
                    #dvSalesSummary {
                        width: 2.6in;
                    }

                    #SalesSummary {
                        width: 2.6in;
                    }

                    #SalesSummaryDetail {
                        width: 98%;
                        margin-top: 10px;
                    }

                        #SalesSummaryDetail tr td {
                            border: #333333 solid 1px;
                            font-size: 6px;
                        }

                    .text-right {
                        text-align: right;
                    }

                </style>
                <table id="SalesSummary">
                    <tr>
                        <td colspan="2" align="center">
                            <span style="font-size: 15px;font-weight:bold;">
                                <%=hfCompanyName.Value%></span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <span style="font-size: 10px;">
                                <%=hfAddess.Value%></span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width:50%;">
                            <span style="font-style: italic; font-size: 10px;">
                            From: <label id="lblFromDate"><%=txtstartDate.Text%></label></span>
                        </td>
                        <td style="width:50%;" align="right">
                            <span style="font-style: italic; font-size: 10px;">
                            To:<label id="lblToDate"><%=txtEndDate.Text%></label></span>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:100%;height:18px;" colspan="2" valign="bottom">
                            <span style="font-size: 12px;font-weight:bold;">
                            <label>Daily Sales Summary</label></span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr style="border: 1px solid black; background-color: Black; margin-bottom: 2px; margin-top: 1px;" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width:50%;">
                            <span style="font-style: italic; font-size: 10px;">
                            No. Of Invoices: </span>
                        </td>
                        <td style="width:50%;">
                            <span style="font-style: italic; font-size: 10px;">
                            <label id="lblNoOfInvoices"></label></span>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:50%;">
                            <span style="font-style: italic; font-size: 10px;">
                            Gross Sale: </span>
                        </td>
                        <td style="width:50%;">
                            <span style="font-style: italic; font-size: 10px;">
                            <label id="lblGrossSale"></label></span>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:50%;">
                            <span style="font-style: italic; font-size: 10px;">
                            Discount: </span>
                        </td>
                        <td style="width:50%;">
                            <span style="font-style: italic; font-size: 10px;">
                            <label id="lblDiscount"></label></span>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:50%;">
                            <span style="font-style: italic; font-size: 10px;">
                            Sales Tax: </span>
                        </td>
                        <td style="width:50%;">
                            <span style="font-style: italic; font-size: 10px;">
                            <label id="lblSalesTax"></label></span>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:50%;">
                            <span style="font-style: italic; font-size: 10px;">
                            Net Amount: </span>
                        </td>
                        <td style="width:50%;">
                            <span style="font-style: italic; font-size: 10px;">
                            <label id="lblNetAmount"></label></span>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:50%;">
                            <span style="font-style: italic; font-size: 10px;">
                            Credit Card: </span>
                        </td>
                        <td style="width:50%;">
                            <span style="font-style: italic; font-size: 10px;">
                            <label id="lblCreditCard"></label></span>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:50%;">
                            <span style="font-style: italic; font-size: 10px;">
                            Jazz Cash: </span>
                        </td>
                        <td style="width:50%;">
                            <span style="font-style: italic; font-size: 10px;">
                            <label id="lblJazzCash"></label></span>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:50%;">
                            <span style="font-style: italic; font-size: 10px;">
                            Easy Paisa: </span>
                        </td>
                        <td style="width:50%;">
                            <span style="font-style: italic; font-size: 10px;">
                            <label id="lblEasyPaisa"></label></span>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:50%;">
                            <span style="font-style: italic; font-size: 10px;">
                            Credit Sale: </span>
                        </td>
                        <td style="width:50%;">
                            <span style="font-style: italic; font-size: 10px;">
                            <label id="lblCreditSale"></label></span>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:50%;">
                            <span style="font-style: italic; font-size: 10px;">
                            Cash Sale: </span>
                        </td>
                        <td style="width:50%;">
                            <span style="font-style: italic; font-size: 10px;">
                            <label id="lblCashSale"></label></span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr style="border: 1px solid black; background-color: Black; margin-bottom: 2px; margin-top: 1px;" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width:100%;height:18px;" colspan="2" valign="bottom">
                            <span style="font-size: 12px;font-weight:bold;">
                            <label>Salesperson Wise Breakup</label></span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table id="rptSalesSummaryDetail">
                                <thead id="rptSalesSummaryHead">
                                    <tr>
                                        <td style="text-align: left; font-size: 6px; width: 19%">S-Person
                                        </td>
                                        <td align="center" style="font-size: 6px; width: 9%">No.Of Inv
                                        </td>
                                        <td align="center" style="font-size: 6px; width: 9%">Gross Sale   
                                        </td>
                                        <td align="center" style="font-size: 6px; width: 9%">Discnt
                                        </td>
                                        <td align="center" style="font-size: 6px; width: 9%">Net Amnt
                                        </td>
                                        <td align="center" style="font-size: 6px; width: 9%">Credit Card
                                        </td>
                                        <td align="center" style="font-size: 6px; width: 9%">Jazz Cash
                                        </td>
                                        <td align="center" style="font-size: 6px; width: 9%">Easy Paisa
                                        </td>
                                        <td align="center" style="font-size: 6px; width: 9%">Credit Sale
                                        </td>
                                        <td align="center" style="font-size: 6px; width: 9%">Cash Sale
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="10">
                                            <hr style="border: 1px solid black; background-color: Black; margin-bottom: 2px; margin-top: 1px;" />
                                        </td>
                                    </tr>
                                </thead>
                                <tbody id="rptSalesSummaryBody" style="font-size: 6px;">
                                </tbody>
                                <tfoot id="rptSalesSummaryFoot">
                                    <tr>
                                        <td colspan="10">
                                            <hr style="border: 1px solid black; background-color: Black; margin-bottom: 2px; margin-top: 1px;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:19%; font-size:6px;">
                                            <label id="lblTotal">
                                                Total :
                                            </label>
                                        </td>
                                        <td style="width:9%; font-size:6px;" align="right">
                                            <label id="lblTotalNoOfInvoice">
                                                
                                            </label>
                                        </td>
                                        <td style="width:9%; font-size:6px;" align="right">
                                            <label id="lblTotalGrossSale">
                                                
                                            </label>
                                        </td>
                                        <td style="width:9%; font-size:6px;" align="right">
                                            <label id="lblTotalDiscount">
                                                
                                            </label>
                                        </td>
                                        <td style="width:9%; font-size:6px;" align="right">
                                            <label id="lblTotalNetAmount">
                                                
                                            </label>
                                        </td>
                                        <td style="width:9%; font-size:6px;" align="right">
                                            <label id="lblTotalCreditCard">
                                                
                                            </label>
                                        </td>
                                        <td style="width:9%; font-size:6px;" align="right">
                                            <label id="lblTotalJazz">
                                                
                                            </label>
                                        </td>
                                        <td style="width:9%; font-size:6px;" align="right">
                                            <label id="lblTotalEasypaisa">
                                                
                                            </label>
                                        </td>
                                        <td style="width:9%; font-size:6px;" align="right">
                                            <label id="lblTotalCreditSale">
                                                
                                            </label>
                                        </td>
                                        <td style="width:9%; font-size:6px;" align="right">
                                            <label id="lblTotalCashSale">
                                                
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="10">
                                            <hr style="border: 1px solid black; background-color: Black; margin-bottom: 2px; margin-top: 1px;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="10">Print Date : <%= DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="10" align="center">
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
        <%-- Sales Summary Report --%>
    </form>
</body>
</html>