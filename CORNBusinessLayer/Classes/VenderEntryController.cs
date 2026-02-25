using System;
using System.Data;
using CORNCommon.Classes;
using CORNDatabaseLayer.Classes;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Collections;
using CORNDataAccessLayer.Classes;


namespace CORNBusinessLayer.Classes
{
   public  class VenderEntryController
    {

        IDbTransaction mTransaction;
        IDbConnection mConnection;
        CORNCommon.Classes.Configuration ConfigClass;
      
       #region Constructor

        /// <summary>
        /// Constructor for OrderEntryController
        /// </summary>
       public VenderEntryController()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#endregion

       public DataTable GetVendor(int p_VENDOR_ID)
       {
           IDbConnection mConnection = null;
           try
           {
               mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
               mConnection.Open();
               spSelectVENDOR mData = new spSelectVENDOR();
               mData.Connection = mConnection;
               mData.VENDOR_ID = p_VENDOR_ID;
               DataTable dt = mData.ExecuteTable();
               return dt;

           }
           catch (Exception exp)
           {
               ExceptionPublisher.PublishException(exp);
               return null;
           }
           finally
           {
               if (mConnection != null && mConnection.State == ConnectionState.Open)
               {
                   mConnection.Close();
               }
           }

       }
       public DataTable GetVendor(int p_VENDOR_ID, int p_PRINCIPAL_ID)
       {
           IDbConnection mConnection = null;
           try
           {
               mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
               mConnection.Open();
               spSelectVENDOR mData = new spSelectVENDOR();
               mData.Connection = mConnection;
               mData.VENDOR_ID = p_VENDOR_ID;
               mData.PRINCIPAL_ID = p_PRINCIPAL_ID;
               DataTable dt = mData.ExecuteTable();
               return dt;

           }
           catch (Exception exp)
           {
               ExceptionPublisher.PublishException(exp);
               return null;
           }
           finally
           {
               if (mConnection != null && mConnection.State == ConnectionState.Open)
               {
                   mConnection.Close();
               }
           }

       }

       #region Vendor

       public int InsertVendor(string p_VENDOR_NAME, string p_ADDRESS1, string p_ADDRESS2, string p_ADDRESS3,
           string p_CONTACT_PERSON, string p_CONTACT_NO, int p_PRINCIPAL_ID)
       {
           IDbConnection mConnection = null;
           try
           {
               mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
               mConnection.Open();
               spInsertVENDOR mVendor = new spInsertVENDOR();
               mVendor.Connection = mConnection;

               mVendor.VENDOR_NAME = p_VENDOR_NAME;
               mVendor.ADDRESS1 = p_ADDRESS1;
               mVendor.ADDRESS2 = p_ADDRESS2;
               mVendor.ADDRESS3 = p_ADDRESS3;
               
               mVendor.CONTACT_PERSON = p_CONTACT_PERSON;
               mVendor.CONTACT_NO = p_CONTACT_NO;
               mVendor.PRINCIPAL_ID = p_PRINCIPAL_ID;
               
               mVendor.ExecuteQuery();
               return mVendor.VENDOR_ID;

           }
           catch (Exception exp)
           {
               ExceptionPublisher.PublishException(exp);
               throw exp;
               return Constants.IntNullValue;
           }
           finally
           {
               if (mConnection != null && mConnection.State == ConnectionState.Open)
               {
                   mConnection.Close();
               }
           }

       }

       public bool UpdateVendor(int p_VENDOR_ID, string p_VENDOR_NAME, string p_ADDRESS1, string p_ADDRESS2, string p_ADDRESS3
            , string p_CONTACT_PERSON, string p_CONTACT_NO, int p_PRINCIPAL_ID
            ,bool pIsActive)
       {
           IDbConnection mConnection = null;
           try
           {
               mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
               mConnection.Open();
               spUpdateVENDOR mVendor = new spUpdateVENDOR();
               mVendor.Connection = mConnection;
               mVendor.PRINCIPAL_ID = p_PRINCIPAL_ID;
               mVendor.VENDOR_ID = p_VENDOR_ID;
               mVendor.VENDOR_NAME = p_VENDOR_NAME;
               mVendor.ADDRESS1 = p_ADDRESS1;
               mVendor.ADDRESS2 = p_ADDRESS2;
               mVendor.ADDRESS3 = p_ADDRESS3;
               mVendor.CONTACT_PERSON = p_CONTACT_PERSON;
               mVendor.CONTACT_NO = p_CONTACT_NO;
               mVendor.IS_ACTIVE = pIsActive;
               return mVendor.ExecuteQuery();

           }
           catch (Exception exp)
           {
               ExceptionPublisher.PublishException(exp);
               throw exp;
               return false;
           }
           finally
           {
               if (mConnection != null && mConnection.State == ConnectionState.Open)
               {
                   mConnection.Close();
               }
           }

       }



       #endregion

       #region Vendor Invoice

       public bool Add_VenderInvoice(int p_DISTRIBUTOR_ID, int p_PRINCIPAL_ID, int p_UserId, DateTime p_DocumentDate, string pInvoiceNo, DateTime pInvoiceDate, string pDC_No
          ,int pVendorId, decimal pAmount)
       {

           IDbConnection mConnection = null;
         
           try
           {
               mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
               mConnection.Open();
               mTransaction = ProviderFactory.GetTransaction(mConnection);

               spInsertVENDER_INVOICE_MASTER mISom = new spInsertVENDER_INVOICE_MASTER();
                 mISom.Connection = mConnection;
                 mISom.Transaction = mTransaction;

                   mISom.PRINCIPAL_ID = p_PRINCIPAL_ID;
                   mISom.VENDOR_ID = pVendorId;
                   mISom.DOCUMENT_DATE = p_DocumentDate;
                   mISom.IS_DELETED = false;
                   mISom.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                   mISom.TIME_STAMP = DateTime.Now;
                   mISom.LASTUPDATE_DATE = System.DateTime.Now;
                   mISom.INVOICE_DATE = pInvoiceDate;
                   mISom.INVOICE_NO = pInvoiceNo;
                   mISom.DC_NO = pDC_No;
                   mISom.AMOUNT = pAmount;
                   mISom.DEBIT_AMOUNT = pAmount;
                   mISom.USER_ID = p_UserId;
                   mISom.ExecuteQuery();

                   #region Account Posting
                   
                       LedgerController LController = new LedgerController();
                       Configuration.GetAccountHead();
                       string VoucherNo = LController.SelectLedgerMaxDocumentId(Constants.Journal_Voucher, p_DISTRIBUTOR_ID, 1);

                       LController.PostingPrinvipalInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.PurchaseAccount), p_DISTRIBUTOR_ID, 0, pAmount, p_DocumentDate, pDC_No, DateTime.Now, pVendorId, p_PRINCIPAL_ID, mISom.VENDER_INVOICE_ID, "0", Constants.Document_Purchase, p_UserId, mTransaction, mConnection, Constants.Document_Purchase, "");
                       LController.PostingPrinvipalInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.PayableAccount), p_DISTRIBUTOR_ID, pAmount, 0, p_DocumentDate, pDC_No, DateTime.Now, pVendorId, p_PRINCIPAL_ID, mISom.VENDER_INVOICE_ID, "0", Constants.Document_Purchase, p_UserId, mTransaction, mConnection, Constants.Document_Purchase, "");

                   #endregion

                    mTransaction.Commit();
                   return true;
               
           }
           catch (Exception exp)
           {
               mTransaction.Rollback();
               ExceptionPublisher.PublishException(exp);
               return false;// exp.Message;
           }
           finally
           {
               if (mConnection != null && mConnection.State == ConnectionState.Open)
               {
                   mConnection.Close();
               }
           }
          
       }

       public bool Update_VenderInvoice(int p_DISTRIBUTOR_ID,long pVendorInvoiceId,int p_PRINCIPAL_ID, int p_UserId, string pInvoiceNo
           , DateTime pInvoiceDate, string pDC_No, int pVendorId, decimal pAmount, bool pDeleted, DateTime p_DocumentDate)
       {
           IDbConnection mConnection = null;
           try
           {

               mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
               mConnection.Open();
               mTransaction = ProviderFactory.GetTransaction(mConnection);


               LedgerController LController = new LedgerController();
               string VoucherNo = LController.SelectLedgerMaxDocumentId(Constants.Journal_Voucher, p_DISTRIBUTOR_ID,1);

               spUpdateVENDER_INVOICE_MASTER mISom = new spUpdateVENDER_INVOICE_MASTER();
               mISom.Connection = mConnection;
               mISom.Transaction = mTransaction;

               mISom.VENDER_INVOICE_ID = pVendorInvoiceId;
               
               mISom.IS_DELETED = pDeleted;
               mISom.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
               mISom.LASTUPDATE_DATE = System.DateTime.Now;
               mISom.INVOICE_DATE = pInvoiceDate;
               mISom.INVOICE_NO = pInvoiceNo;
               mISom.DC_NO = pDC_No;
               mISom.AMOUNT = pAmount;
               mISom.DEBIT_AMOUNT = pAmount;
               mISom.USER_ID = p_UserId;
               mISom.ExecuteQuery();


               #region Account Posting

               Configuration.GetAccountHead();

               LController.PostingPrinvipalInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.PurchaseAccount), p_DISTRIBUTOR_ID, 0, pAmount, p_DocumentDate, pDC_No, DateTime.Now, pVendorId, p_PRINCIPAL_ID, mISom.VENDER_INVOICE_ID, "0", Constants.Document_Purchase, p_UserId, mTransaction, mConnection, Constants.Document_Purchase, "");
               LController.PostingPrinvipalInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.PayableAccount), p_DISTRIBUTOR_ID, pAmount, 0, p_DocumentDate, pDC_No, DateTime.Now, pVendorId, p_PRINCIPAL_ID, mISom.VENDER_INVOICE_ID, "0", Constants.Document_Purchase, p_UserId, mTransaction, mConnection, Constants.Document_Purchase, "");

               #endregion

               mTransaction.Commit();
               return true;

           }
           catch (Exception exp)
           {
               mTransaction.Rollback();
               ExceptionPublisher.PublishException(exp);
               return false;// exp.Message;
           }
           finally
           {
               if (mConnection != null && mConnection.State == ConnectionState.Open)
               {
                   mConnection.Close();
               }
           }
           return true;
       }

       public void Update_VenderInvoice(long pVendorInvoiceId,int p_DISTRIBUTOR_ID,  decimal pAmount)
       {
           IDbConnection mConnection = null;
           try
           {

               mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
               mConnection.Open();
               

               spUpdateVENDER_INVOICE_MASTER mISom = new spUpdateVENDER_INVOICE_MASTER();
               mISom.Connection = mConnection;
          
               mISom.VENDER_INVOICE_ID = pVendorInvoiceId;
               mISom.IS_DELETED = false;
               mISom.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
               mISom.DEBIT_AMOUNT = pAmount;
               mISom.ExecuteQuery();

           }
           catch (Exception exp)
           {
               ExceptionPublisher.PublishException(exp);
           }
          
           
       }

       public DataTable SelectVendorMaster(int pDistributorId, int pPrincipalId, int pVendorId)
       {
           IDbConnection mConnection = null;
           try
           {
               mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
               mConnection.Open();
               UspSelectPendingVender mOrder = new UspSelectPendingVender();
               mOrder.Connection = mConnection;

               mOrder.PRINCIPAL_ID = pPrincipalId;
               mOrder.VENDOR_ID = pVendorId;
               mOrder.DISTRIBUTOR_ID = pDistributorId;
               DataTable dt = mOrder.ExecuteTable();
               return dt;

           }
           catch (Exception exp)
           {
               ExceptionPublisher.PublishException(exp);
               return null;
           }
           finally
           {
               if (mConnection != null && mConnection.State == ConnectionState.Open)
               {
                   mConnection.Close();
               }
           }

       }

       #region Vendoer Ledger

       public DataSet GetVendorLedger(int p_VENDOR_ID, int p_DISTRIBUTOR_ID, DateTime p_FROM_DATE, DateTime p_TO_DATE)
       {
           IDbConnection mConnection = null;
           try
           {
               mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
               mConnection.Open();
               CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();

               uspGetVendorLedgerReport mLedger = new uspGetVendorLedgerReport();

               mLedger.Connection = mConnection;
               mLedger.VENDOR_ID = p_VENDOR_ID;
               mLedger.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
               mLedger.FROM_DATE = p_FROM_DATE;
               mLedger.TO_DATE = p_TO_DATE;

               DataTable DT = mLedger.ExecuteTable();

               //DataView dv = new DataView(DT);
               //dv.Sort = "debit";
               //DT = dv.ToTable();
               foreach (DataRow dr in DT.Rows)
               {
                   ds.Tables["RptCustomerLedgerView"].ImportRow(dr);

               }

               spSelectCHEQUE_PROCESS2 mLedgerSub = new spSelectCHEQUE_PROCESS2();

               mLedgerSub.Connection = mConnection;
               mLedgerSub.PRINCIPAL_ID = p_VENDOR_ID;
               mLedgerSub.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
              
               mLedgerSub.STATUS_ID = 527528;// Recieved and deposit Cheques

               DataTable dtPro = mLedgerSub.ExecuteTable();

               foreach (DataRow dr in dtPro.Rows)
               {
                   ds.Tables["spSelectCHEQUE_PROCESS2"].ImportRow(dr);
               }

               return ds;
           }
           catch (Exception exp)
           {
               ExceptionPublisher.PublishException(exp);
               return null;
           }
           finally
           {
               if (mConnection != null && mConnection.State == ConnectionState.Open)
               {
                   mConnection.Close();
               }
           }
       }

       public DataTable GetVendoerOpening(int p_VENDOR_ID, int p_DISTRIBUTOR_ID, DateTime p_FROM_DATE)
       {
           IDbConnection mConnection = null;
           try
           {
               mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
               mConnection.Open();
               CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();

               uspGetVendorLedgerOpening mLedger = new uspGetVendorLedgerOpening();

               mLedger.Connection = mConnection;
               mLedger.VENDOR_ID = p_VENDOR_ID;
               mLedger.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
               mLedger.FROM_DATE = p_FROM_DATE;

               DataTable DT = mLedger.ExecuteTable();
               return DT;
           }
           catch (Exception exp)
           {
               ExceptionPublisher.PublishException(exp);
               return null;
           }
           finally
           {
               if (mConnection != null && mConnection.State == ConnectionState.Open)
               {
                   mConnection.Close();
               }
           }
       }

       #endregion
        #endregion
    }
}
