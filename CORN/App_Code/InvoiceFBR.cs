using System;
using System.Collections.Generic;
public partial class InvoiceFBRDetail
{
    public string ItemCode { get; set; }
    public string ItemName { get; set; }
    public int Quantity { get; set; }
    public double TotalAmount { get; set; }
    public double SaleValue { get; set; }
    public double TaxCharged { get; set; }
    public double TaxRate { get; set; }
    public string PCTCode { get; set; }
    public decimal FurtherTax { get; set; }
    public int InvoiceType { get; set; }
    public decimal Discount { get; set; }
    public string RefUSIN { get; set; }
}

public class InvoiceFBR
{
    public string InvoiceNumber { get; set; }
    public string POSID { get; set; }
    public string USIN { get; set; }
    public DateTime DateTime { get; set; }
    public string BuyerNTN { get; set; }
    public string BuyerCNIC { get; set; }
    public string BuyerName { get; set; }
    public string BuyerPhoneNumber { get; set; }
    public int PaymentMode { get; set; }
    public double TotalSaleValue { get; set; }
    public int TotalQuantity { get; set; }
    public double TotalBillAmount { get; set; }
    public double TotalTaxCharged { get; set; }
    public decimal Discount { get; set; }
    public decimal FurtherTax { get; set; }
    public int InvoiceType { get; set; }
    public string RefUSIN { get; set; }
    public List<InvoiceFBRDetail> Items { get; set; }
}