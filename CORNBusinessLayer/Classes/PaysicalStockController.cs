using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using CORNDatabaseLayer.Classes;

namespace CORNBusinessLayer.Classes
{
    /// <summary>
    /// Class For Physical Stock Related Tasks
    /// <example>
    /// <list type="bullet">
    /// <item>
    /// Insert Stock
    /// </item>
    /// <term>
    /// Update Stock
    /// </term>
    /// <item>
    /// Get Stock
    /// </item>
    /// </list>
    /// </example>
    /// </summary>
    public class PhaysicalStockController
    {

        #region Constructors

        /// <summary>
        /// Constructor For PhaysicalStockController
        /// </summary>
        public PhaysicalStockController()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		#endregion

        #region Public Methods

        #region Select

        /// <summary>
        /// Gets SKU Closing Stock
        /// </summary>
        /// <remarks>
        /// Returns SKU Closing Stock as Datatable
        /// </remarks>
        /// <param name="p_DISTRIBUTOR_ID">Location</param>
        /// <param name="p_SKU_ID">SKU</param>
        /// <param name="p_BatchNo">Batch</param>
        /// <param name="p_StockDate">Date</param>
        /// <returns>SKU Closing Stock as Datatable</returns>
        public DataTable SelectSKUClosingStock(int p_DISTRIBUTOR_ID, int p_SKU_ID, string p_BatchNo, DateTime p_StockDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspProcessStockRegister mStockUpdate = new UspProcessStockRegister();
                mStockUpdate.Connection = mConnection;
                mStockUpdate.TYPE_ID = 12;
                mStockUpdate.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mStockUpdate.SKU_ID = p_SKU_ID;
                mStockUpdate.BATCHNO = p_BatchNo;
                mStockUpdate.STOCK_DATE = p_StockDate;
                DataTable dt = mStockUpdate.ExecuteTable();
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


        public DataTable SelectSKUClosingStock2(int p_DISTRIBUTOR_ID, int p_SKU_ID, string p_BatchNo, DateTime p_StockDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspProcessStockRegister mStockUpdate = new UspProcessStockRegister();
                mStockUpdate.Connection = mConnection;
                mStockUpdate.TYPE_ID = 15;
                mStockUpdate.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mStockUpdate.SKU_ID = p_SKU_ID;
                mStockUpdate.BATCHNO = p_BatchNo;
                mStockUpdate.STOCK_DATE = p_StockDate;
                DataTable dt = mStockUpdate.ExecuteTable();
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
        public DataTable GetItemMaxStockLevel(int p_DISTRIBUTOR_ID, int p_SKU_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetItemMaxStockLevel mStockLevel = new uspGetItemMaxStockLevel();
                mStockLevel.Connection = mConnection;
                mStockLevel.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mStockLevel.SKU_ID = p_SKU_ID;
                DataTable dt = mStockLevel.ExecuteTable();
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

        /// <summary>
        /// Gets Physical Stock
        /// </summary>
        /// <param name="p_DISTRIBUTOR_ID">Location</param>
        /// <param name="p_POST">Post</param>
        /// <param name="PRINCIPAL_ID">Principal</param>
        /// <returns>Physical Stock Data as Datatable</returns>
        public DataTable SelectPysicalStock(int p_DISTRIBUTOR_ID, int p_POST, int PRINCIPAL_ID, DateTime p_workDate, long p_Physical_Stock_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spSelectPAYSICAL_STOCK mPaysical = new spSelectPAYSICAL_STOCK();
                mPaysical.Connection = mConnection;
                mPaysical.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mPaysical.PRINCIPAL_ID = PRINCIPAL_ID;
                mPaysical.POST = 0;
                mPaysical.STOCK_DATE = p_workDate;
                mPaysical.Physical_Stock_ID = p_Physical_Stock_ID;
                DataTable dt = mPaysical.ExecuteTable();
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
        public DataTable SelectMaxDocNo()
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spSelectPAYSICAL_STOCK mPaysical = new spSelectPAYSICAL_STOCK();
                mPaysical.Connection = mConnection;
                DataTable dt = mPaysical.ExecuteTableForMaxDocNo();
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

        #endregion

        #region Insert, Update, Delete

        /// <summary>
        /// Inserts Physical Stock
        /// </summary>
        /// <param name="p_DISTRIBUTOR_ID">Location</param>
        /// <param name="p_STOCK_DATE">Date</param>
        /// <param name="p_SKU_ID">SKU</param>
        /// <param name="p_SQUANTITY">SaleQuantity</param>
        /// <param name="p_UQUANTITY">UnSaleQuantity</param>
        /// <param name="p_UNIT_RATE">Rate</param>
        /// <param name="p_POST">Post</param>
        /// <param name="PRINCIPAL_ID">Principal</param>
        /// <returns>Null On Success And Exception.Message On Failure</returns>
        public string InsertPysicalStock(int p_DISTRIBUTOR_ID, DateTime p_STOCK_DATE, int p_PRINCIPAL_ID,
            int p_POST, long p_Doc_NO, DataTable details)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spInsertPAYSICAL_STOCK mPaysical = new spInsertPAYSICAL_STOCK();
                mPaysical.Connection = mConnection;
                foreach (DataRow item in details.Rows)
                {
                    mPaysical.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                    mPaysical.STOCK_DATE = p_STOCK_DATE;
                    mPaysical.SKU_ID = int.Parse(item["SKU_ID"].ToString());
                    mPaysical.SALEABLE_QUANTITY = decimal.Parse(item["SALEABLE_QUANTITY"].ToString());
                    mPaysical.UNSALEABLE_QUANTITY = 0;
                    mPaysical.UNIT_RATE = decimal.Parse(item["UNIT_RATE"].ToString()); ;
                    mPaysical.PRINCIPAL_ID = p_PRINCIPAL_ID;
                    mPaysical.POST = p_POST;
                    mPaysical.DOC_NO = p_Doc_NO;
                    mPaysical.ExecuteQuery();
                }
                
                return "true";
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return exp.Message;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }
        
        /// <summary>
        /// Updates Physical Stock
        /// </summary>
        /// <param name="p_DISTRIBUTOR_ID">Location</param>
        /// <param name="p_STOCK_DATE">Date</param>
        /// <param name="p_SKU_ID">SKU</param>
        /// <param name="p_SQUANTITY">SaleQuantity</param>
        /// <param name="p_UQUANTITY">UnSaleQuantity</param>
        /// <param name="p_UNIT_RATE">Rate</param>
        /// <param name="p_POST">Post</param>
        /// <param name="PRINCIPAL_ID">Principal</param>
        /// <returns>Null On Success And Exception.Message On Failure</returns>
        public string UpdatePysicalStock(int p_DISTRIBUTOR_ID, DateTime p_STOCK_DATE, int p_PRINCIPAL_ID,
            int p_POST, DataTable details)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spUpdatePAYSICAL_STOCK mPaysical = new spUpdatePAYSICAL_STOCK();
                mPaysical.Connection = mConnection;
                foreach (DataRow item in details.Rows)
                {
                    mPaysical.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                    mPaysical.STOCK_DATE = p_STOCK_DATE;
                    mPaysical.SKU_ID = int.Parse(item["SKU_ID"].ToString());
                    mPaysical.SALEABLE_QUANTITY = decimal.Parse(item["SALEABLE_QUANTITY"].ToString());
                    mPaysical.UNSALEABLE_QUANTITY = 0;
                    mPaysical.UNIT_RATE = decimal.Parse(item["UNIT_RATE"].ToString()); ;
                    mPaysical.PRINCIPAL_ID = p_PRINCIPAL_ID;
                    mPaysical.POST = p_POST;
                    mPaysical.ExecuteQuery();
                }
                return null;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return exp.Message;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }



        /// <summary>
        /// Updates  AND CALCULATE StockREGISTER
        /// </summary>
        /// <param name="pDistributorId">Location</param>
        /// <param name="pStockDate">Date</param>
        /// <param name="p_SKU_ID">SKU</param>
        /// <param name="p_SQUANTITY">SaleQuantity</param>
        /// <param name="p_UQUANTITY">UnSaleQuantity</param>
        /// <param name="p_UNIT_RATE">Rate</param>
        /// <param name="p_POST">Post</param>
        /// <param name="PRINCIPAL_ID">Principal</param>
        /// <returns>Null On Success And Exception.Message On Failure</returns>
        public string CalculateStockRegister(int pDistributorId, DateTime pStockDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                uspCalculateStockregister2 mPaysical = new uspCalculateStockregister2();
                mPaysical.Connection = mConnection;
                mPaysical.DISTRIBUTOR_ID = pDistributorId;
                mPaysical.STOCK_DATE = pStockDate;
               
                mPaysical.ExecuteQuery();
                return null;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return exp.Message;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }
        
        /// <summary>
        /// Deletes Physical Stock
        /// </summary>
        /// <param name="p_DISTRIBUTOR_ID">Location</param>
        /// <param name="p_STOCK_DATE">Date</param>
        /// <param name="SKU_ID">SKU</param>
        /// <returns>True On Success And False On Failure</returns>
        public bool  DELETEPysicalStock(int p_DISTRIBUTOR_ID, DateTime  p_STOCK_DATE, int SKU_ID, long p_docNo)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spDeletePAYSICAL_STOCK mPaysical = new spDeletePAYSICAL_STOCK();
                mPaysical.Connection = mConnection;
                mPaysical.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mPaysical.SKU_ID = SKU_ID;
                mPaysical.STOCK_DATE  = p_STOCK_DATE;
                mPaysical.DOC_NO = p_docNo;
                mPaysical.ExecuteQuery();
                return true;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
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

        #endregion
    }
}
