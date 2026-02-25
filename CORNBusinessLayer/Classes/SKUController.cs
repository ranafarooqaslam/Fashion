using System;
using System.Data;
using System.IO;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using CORNDatabaseLayer.Classes;

namespace CORNBusinessLayer.Classes
{
    /// <summary>
    /// Class For SKU Related Tasks
    /// <example>
    /// <list type="bullet">
    /// <item>
    /// Insert SKU
    /// </item>
    /// <term>
    /// Update SKU
    /// </term>
    /// <item>
    /// Get SKU
    /// </item>
    /// </list>
    /// </example>
    /// </summary>
	public class SkuController
	{	
		#region Constructors

        /// <summary>
        /// Constructor For SkuController
        /// </summary>
		public SkuController()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		
        #endregion
				
		#region public Methods

        #region Select

        /// <summary>
        /// Gets SKUS Data
        /// </summary>
        /// <remarks>
        /// Returns SKUS Data as Datatable
        /// </remarks>
        /// <param name="p_company_id">Principal</param>
        /// <param name="p_division_id">Dicision</param>
        /// <param name="p_category_id">Category</param>
        /// <param name="p_brand_id">Brand</param>
        /// <param name="Companyid">Company</param>
        /// <returns>SKUS Data as Datatable</returns>
        public DataTable SelectSkuInfo(int p_company_id, int p_division_id, int p_category_id, int p_subcategory_id, int p_brand_id, int Companyid)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspSelectSkuInfo mspSelectSkuInfo = new uspSelectSkuInfo();
                mspSelectSkuInfo.Connection = mConnection;

                mspSelectSkuInfo.brand_id = p_brand_id;
                mspSelectSkuInfo.category_id = p_category_id;
                mspSelectSkuInfo.subcategory_id = p_subcategory_id;
                mspSelectSkuInfo.Principal_id = p_company_id;
                mspSelectSkuInfo.division_id = p_division_id;
                mspSelectSkuInfo.Company_id = Companyid;

                DataTable dt = mspSelectSkuInfo.ExecuteTable();

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

        public DataTable SelectSkuInfo2(int p_company_id, int p_division_id, int p_category_id, int p_brand_id, int Companyid,int p_TAGID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspSelectSkuInfo2 mspSelectSkuInfo = new uspSelectSkuInfo2();
                mspSelectSkuInfo.brand_id = p_brand_id;
                mspSelectSkuInfo.category_id = p_category_id;
                mspSelectSkuInfo.Principal_id = p_company_id;
                mspSelectSkuInfo.Connection = mConnection;
                mspSelectSkuInfo.division_id = p_division_id;
                mspSelectSkuInfo.Company_id = Companyid;
                mspSelectSkuInfo.TAG_ID = p_TAGID;

                DataTable dt = mspSelectSkuInfo.ExecuteTable();

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
        /// Gets SKU Data
        /// </summary>
        /// <remarks>
        /// Returns SKUS Data as Datatable
        /// </remarks>
        /// <param name="p_SKU_Id">SKU</param>
        /// <param name="Companyid">Company</param>
        /// <returns>SKUS Data as Datatable</returns>
        public DataTable SelectSkuData(int p_SKU_Id, int Companyid)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectSKUS mSkuInfo = new spSelectSKUS();
                mSkuInfo.Connection = mConnection;
                mSkuInfo.SKU_ID = p_SKU_Id;
                mSkuInfo.COMPANY_ID = Companyid;
                DataTable dt = mSkuInfo.ExecuteTable();
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
        /// Gets SKU UOM
        /// </summary>
        /// <param name="p_UOM_Id">UOM</param>
        /// <param name="p_UOM_Desc">Description</param>
        /// <returns>SKU UOM</returns>
        public DataTable SelectUOMs(int p_UOM_Id, string p_UOM_Desc)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spSelectUOMS mUOMs = new spSelectUOMS();
                mUOMs.Connection = mConnection;

                mUOMs.UOM_ID = p_UOM_Id;
                mUOMs.UOM_DESC = p_UOM_Desc;
                mUOMs.TIME_STAMP = Constants.DateNullValue;
                mUOMs.STATUS = Constants.IntNullValue;

                DataTable dt = mUOMs.ExecuteTable();
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

        public DataTable SelectSkuCountry()
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spSelectCountrySku mUOMs = new spSelectCountrySku();
                mUOMs.Connection = mConnection;
                 DataTable dt = mUOMs.ExecuteTable();
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
        public DataTable SearchProduct(string pSearchText)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspSearchProduct mspSelectSkuInfo = new uspSearchProduct();
                mspSelectSkuInfo.Connection = mConnection;
                mspSelectSkuInfo.SEARCH_TEXT = pSearchText;
                DataTable dt = mspSelectSkuInfo.ExecuteTable();

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
        public DataTable SearchProduct(string pSearchText,int pDistributorId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspSearchProduct mspSelectSkuInfo = new uspSearchProduct();
                mspSelectSkuInfo.Connection = mConnection;
                mspSelectSkuInfo.SEARCH_TEXT = pSearchText;
                mspSelectSkuInfo.DistributorId = pDistributorId;
                DataTable dt = mspSelectSkuInfo.ExecuteTable();

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
        #region Insert, Update

        /// <summary>
        /// Inserts Or Updates SKU Price From Excel File
        /// </summary>
        /// Returns True On Success And False On Failure
        /// <param name="p_DistributorId">Location</param>
        /// <param name="pFileName">ExcelFile</param>
        /// <param name="p_Principal_Id">Principal</param>
        /// <returns>True On Success And False On Failure</returns>
        public bool ImportSKUS(int p_DistributorId, string pFileName, int p_Principal_Id, int p_Company_ID, int p_UserId)
        {
            IDbConnection mConnection = null;
            FileStream Sourcefile = null;
            StreamReader ReadSourceFile = null;
            IDbTransaction mTransaction = null;
            DataControl DC = new DataControl();

            mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
            mConnection.Open();
            mTransaction = ProviderFactory.GetTransaction(mConnection);

            Sourcefile = new FileStream(pFileName, FileMode.Open);
            ReadSourceFile = new StreamReader(Sourcefile);
            string FileContents = "";

            int count = 0;
            try
            {
                while ((FileContents = ReadSourceFile.ReadLine()) != null)
                {

                    string[] ParametersArr = FileContents.Split(Constants.File_Delimiter);
                    uspImportSKUS mSKUS = new uspImportSKUS();
                    mSKUS.Connection = mConnection;
                    mSKUS.Transaction = mTransaction;
                    mSKUS.PRINCIPAL_ID = p_Principal_Id;
                    mSKUS.ISEXEMPTED = true;
                    mSKUS.ISACTIVE = true;
                    mSKUS.GST_ON = 'T';
                    mSKUS.COMPANY_ID = p_Company_ID;
                    mSKUS.SKU_CODE = ParametersArr[0].ToString();
                    mSKUS.SKU_NAME = ParametersArr[1].ToString();
                    mSKUS.BAR_CODE = ParametersArr[2].ToString();
                    mSKUS.DIVISION_ID = Convert.ToInt32(DC.chkNull_0(ParametersArr[3]));
                    mSKUS.CATEGORY_ID = Convert.ToInt32(DC.chkNull_0(ParametersArr[4]));
                    mSKUS.SUBCATEGORY_ID = Convert.ToInt32(DC.chkNull_0(ParametersArr[5]));
                    mSKUS.BRAND_ID = Convert.ToInt32(DC.chkNull_0(ParametersArr[6]));
                    mSKUS.SKU_TAG_ID = int.Parse(DC.chkNull_0(ParametersArr[7].ToString()));
                    mSKUS.COLOR = ParametersArr[8].ToString();                    
                    mSKUS.PACKSIZE = ParametersArr[9].ToString();
                    mSKUS.SKU_SEASON = ParametersArr[10].ToString();
                    mSKUS.SKU_COUNTRY = ParametersArr[11].ToString();
                    mSKUS.SKU = ParametersArr[12].ToString();
                    mSKUS.year = ParametersArr[13].ToString();
                    mSKUS.Material = ParametersArr[14].ToString();
                    mSKUS.Fit = ParametersArr[15].ToString();
                    mSKUS.Weight = ParametersArr[16].ToString();
                    mSKUS.Karat = ParametersArr[17].ToString();
                    mSKUS.MakeCharge = ParametersArr[18].ToString();

                    mSKUS.GST_RATE_REG = 0;
                    mSKUS.GST_RATE_UNREG = 0;                    
                    mSKUS.TIME_STAMP = System.DateTime.Now;
                    mSKUS.LASTUPDATE_DATE = System.DateTime.Now;
                    mSKUS.IP_ADDRESS = null;                    
                    mSKUS.USER_ID = p_UserId;

                    count++;

                    mSKUS.ExecuteQuery();
                }
                mTransaction.Commit();
                return true;
                

            }

            catch (Exception excp)
            {
                mTransaction.Rollback();
                ReadSourceFile.Close();
                mConnection.Close();
                //ExceptionPublisher.PublishException(excp);
               // throw;
                return false;

            }
            finally
            {
                ReadSourceFile.Close();
                mConnection.Close();

            }
        }	
        
        /// <summary>
        /// Insert SKU
        /// </summary>
        /// <remarks>
        /// Returns Inserted SKU ID as String
        /// </remarks>
        /// <param name="p_IsExempted">IsExempted</param>
        /// <param name="p_IsActive">IsActive</param>
        /// <param name="p_Gst_On">GSTOn</param>
        /// <param name="p_Company_Id">Principal</param>
        /// <param name="p_Division_Id">Division</param>
        /// <param name="p_Category_Id">Category</param>
        /// <param name="p_Brand_Id">Brand</param>
        /// <param name="p_Variant_Id">Variant</param>
        /// <param name="p_GST_Rate_Reg">GSTReg</param>
        /// <param name="p_GST_Rate_Unreg">GSTUnReg</param>
        /// <param name="p_Units_In_Case">Units</param>
        /// <param name="p_Sku_Code">Code</param>
        /// <param name="p_Sku_Name">Name</param>
        /// <param name="p_Ip_Address">Address</param>
        /// <param name="p_packSize">Packing</param>
        /// <param name="p_UserId">InsertedBy</param>
        /// <param name="Companyid">Company</param>
        /// <returns>Inserted SKU ID as String</returns>
        public string InsertSKUS2(bool p_IsExempted, bool p_IsActive, char p_Gst_On, int p_Company_Id, int p_Division_Id, int p_Category_Id, int p_SubCategory_Id, int p_Brand_Id, int p_Variant_Id, decimal p_GST_Rate_Reg, decimal p_GST_Rate_Unreg, string p_Units_In_Case, string p_Sku_Code, string p_Sku_Name,
            string p_Ip_Address, string p_packSize, int p_UserId, int Companyid, string p_BarCode,
            string p_color,int p_skuTagId,string p_skuCountry,string p_skuSeason,string pYear,string pSKU
            ,int pSHowOnPos, string p_material, string p_fit, string p_weight, string p_karat,
            string p_makeCharge)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertSKUS2 mSkus = new spInsertSKUS2();

                mSkus.Connection = mConnection;
                mSkus.PRINCIPAL_ID = p_Company_Id;
                mSkus.ISEXEMPTED = p_IsExempted;
                mSkus.ISACTIVE = p_IsActive;
                mSkus.GST_ON = p_Gst_On;
                mSkus.COMPANY_ID = Companyid;
                mSkus.DIVISION_ID = p_Division_Id;
                mSkus.BRAND_ID = p_Brand_Id;
                mSkus.CATEGORY_ID = p_Category_Id;
                mSkus.SUBCATEGORY_ID = p_SubCategory_Id;
                mSkus.COLOR=p_Units_In_Case;
                mSkus.BAR_CODE = p_BarCode;
                mSkus.COLOR = p_color;
                mSkus.SKU_TAG_ID = p_skuTagId;
                mSkus.SKU_SEASON = p_skuSeason;
                mSkus.SKU_COUNTRY = p_skuCountry;
                mSkus.year = pYear;
                mSkus.SKU = pSKU;
                if (!p_IsExempted)
                {
                    mSkus.GST_RATE_REG = p_GST_Rate_Reg;
                    mSkus.GST_RATE_UNREG = p_GST_Rate_Unreg;
                }
                else
                {
                    mSkus.GST_RATE_REG = 0;
                    mSkus.GST_RATE_UNREG = 0;
                }
                //mSkus.UNITS_IN_CASE = p_Units_In_Case;
                mSkus.SKU_NAME = p_Sku_Name;
                mSkus.SKU_CODE = p_Sku_Code;
                mSkus.TIME_STAMP = System.DateTime.Now;
                mSkus.LASTUPDATE_DATE = System.DateTime.Now;
                mSkus.IP_ADDRESS = p_Ip_Address;
                mSkus.PACKSIZE = p_packSize;
                mSkus.USER_ID = p_UserId;
                mSkus.SHOW_ON_POS = pSHowOnPos;
                mSkus.Material = p_material;
                mSkus.Fit = p_fit;
                mSkus.Weight = p_weight;
                mSkus.Karat = p_karat;
                mSkus.MakeCharge = p_makeCharge;
            
                mSkus.ExecuteQuery();

                return mSkus.SKU_ID.ToString();

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


        public string InsertSKUS(bool p_IsExempted, bool p_IsActive, char p_Gst_On, int p_Company_Id, int p_Division_Id, int p_Category_Id, int p_Brand_Id, int p_Variant_Id, decimal p_GST_Rate_Reg, decimal p_GST_Rate_Unreg, short p_Units_In_Case, string p_Sku_Code, string p_Sku_Name, string p_Ip_Address, string p_packSize, int p_UserId, int Companyid)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertSKUS mSkus = new spInsertSKUS();

                mSkus.Connection = mConnection;
                mSkus.PRINCIPAL_ID = p_Company_Id;
                mSkus.ISEXEMPTED = p_IsExempted;
                mSkus.ISACTIVE = p_IsActive;
                mSkus.GST_ON = p_Gst_On;
                mSkus.COMPANY_ID = Companyid;
                mSkus.DIVISION_ID = p_Division_Id;
                mSkus.BRAND_ID = p_Brand_Id;
                mSkus.CATEGORY_ID = p_Category_Id;
            

                if (!p_IsExempted)
                {
                    mSkus.GST_RATE_REG = p_GST_Rate_Reg;
                    mSkus.GST_RATE_UNREG = p_GST_Rate_Unreg;
                }
                else
                {
                    mSkus.GST_RATE_REG = 0;
                    mSkus.GST_RATE_UNREG = 0;
                }
                mSkus.UNITS_IN_CASE = p_Units_In_Case;
                mSkus.SKU_NAME = p_Sku_Name;
                mSkus.SKU_CODE = p_Sku_Code;
                mSkus.TIME_STAMP = System.DateTime.Now;
                mSkus.LASTUPDATE_DATE = System.DateTime.Now;
                mSkus.IP_ADDRESS = p_Ip_Address;
                mSkus.PACKSIZE = p_packSize;
                mSkus.USER_ID = p_UserId;

                mSkus.ExecuteQuery();

                return mSkus.SKU_ID.ToString();

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
        /// Updates SKU
        /// </summary>
        /// <remarks>
        /// Returns "Record Updated" On Success And Null On Failure
        /// </remarks>
        /// <param name="p_IsExempted">IsExempted</param>
        /// <param name="p_IsActive">IsActive</param>
        /// <param name="p_Gst_On">GSTOn</param>
        /// <param name="p_Company_Id">Principal</param>
        /// <param name="p_Division_Id">Division</param>
        /// <param name="p_Category_Id">Category</param>
        /// <param name="p_Brand_Id">Brand</param>
        /// <param name="p_Variant_Id">Variant</param>
        /// <param name="p_GST_Rate_Reg">GSTReg</param>
        /// <param name="p_GST_Rate_Unreg">GSTUnReg</param>
        /// <param name="p_Units_In_Case">Units</param>
        /// <param name="p_Sku_Id">SKU</param>
        /// <param name="p_Sku_Code">Code</param>
        /// <param name="p_Sku_Name">Name</param>
        /// <param name="p_Ip_Address">Address</param>
        /// <param name="p_packSize">Packing</param>
        /// <param name="p_UserId">InsertedBy</param>
        /// <param name="CompanyId">Company</param>
        /// <returns>"Record Updated" On Success And Null On Failure</returns>
        public string UpdateSKUS(bool p_IsExempted, bool p_IsActive, char p_Gst_On, int p_Company_Id, int p_Division_Id, int p_Category_Id, int p_Brand_Id, int p_Variant_Id, decimal p_GST_Rate_Reg, decimal p_GST_Rate_Unreg, short p_Units_In_Case, int p_Sku_Id, string p_Sku_Code, string p_Sku_Name, string p_Ip_Address, string p_packSize, int p_UserId, int CompanyId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spUpdateSKUS mSkus = new spUpdateSKUS();

                mSkus.Connection = mConnection;
                mSkus.ISEXEMPTED = p_IsExempted;
                mSkus.ISACTIVE = p_IsActive;
                mSkus.GST_ON = p_Gst_On;
                mSkus.COMPANY_ID = CompanyId;
                mSkus.PRINCIPAL_ID = p_Company_Id;
                mSkus.DIVISION_ID = p_Division_Id;
                mSkus.BRAND_ID = p_Brand_Id;
                mSkus.CATEGORY_ID = p_Category_Id;
                if (!p_IsExempted)
                {
                    mSkus.GST_RATE_REG = p_GST_Rate_Reg;
                    mSkus.GST_RATE_UNREG = p_GST_Rate_Unreg;
                }
                else
                {
                    mSkus.GST_RATE_REG = 0;
                    mSkus.GST_RATE_UNREG = 0;
                }
                mSkus.UNITS_IN_CASE = p_Units_In_Case;
                mSkus.SKU_ID = p_Sku_Id;
                mSkus.SKU_NAME = p_Sku_Name;
                mSkus.SKU_CODE = p_Sku_Code;
                mSkus.TIME_STAMP = System.DateTime.Now;
                mSkus.LASTUPDATE_DATE = System.DateTime.Now;
                mSkus.IP_ADDRESS = p_Ip_Address;
                mSkus.PACKSIZE = p_packSize;
                mSkus.USER_ID = p_UserId;
                mSkus.ExecuteQuery();
                return "Record Updated";

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


        public string UpdateSKUS2(bool p_IsExempted, bool p_IsActive, char p_Gst_On, int p_Company_Id, int p_Division_Id,
            int p_Category_Id, int p_SubCategory_Id, int p_Brand_Id, int p_Variant_Id, decimal p_GST_Rate_Reg, decimal p_GST_Rate_Unreg, string p_Units_In_Case,
            int p_Sku_Id, string p_Sku_Code, string p_Sku_Name, string p_Ip_Address, string p_packSize, int p_UserId, int CompanyId, string p_BarCode, string p_color, int p_skuTagId, string p_skuCountry, string p_skuSeason
            ,string pYear,string pSKU,int p_SHOW_ON_POS, string p_material, string p_fit, string p_weight, string p_karat,
            string p_makeCharge)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spUpdateSKUS2 mSkus = new spUpdateSKUS2();

                mSkus.Connection = mConnection;
                mSkus.ISEXEMPTED = p_IsExempted;
                mSkus.ISACTIVE = p_IsActive;
                mSkus.GST_ON = p_Gst_On;
                mSkus.COMPANY_ID = CompanyId;
                mSkus.PRINCIPAL_ID = p_Company_Id;
                mSkus.DIVISION_ID = p_Division_Id;
                mSkus.BRAND_ID = p_Brand_Id;
                mSkus.CATEGORY_ID = p_Category_Id;
                mSkus.SUBCATEGORY_ID = p_SubCategory_Id;
                mSkus.BAR_CODE = p_BarCode;
                mSkus.COLOR = p_color;
                mSkus.SKU_TAG_ID = p_skuTagId;
                mSkus.SKU_SEASON = p_skuSeason;
                mSkus.SKU_COUNTRY = p_skuCountry;
                mSkus.year = pYear;
                mSkus.SKU = pSKU;
                if (!p_IsExempted)
                {
                    mSkus.GST_RATE_REG = p_GST_Rate_Reg;
                    mSkus.GST_RATE_UNREG = p_GST_Rate_Unreg;
                }
                else
                {
                    mSkus.GST_RATE_REG = 0;
                    mSkus.GST_RATE_UNREG = 0;
                }
                mSkus.UNITS_IN_CASE = p_Units_In_Case;
                mSkus.SKU_ID = p_Sku_Id;
                mSkus.SKU_NAME = p_Sku_Name;
                mSkus.SKU_CODE = p_Sku_Code;
                mSkus.TIME_STAMP = System.DateTime.Now;
                mSkus.LASTUPDATE_DATE = System.DateTime.Now;
                mSkus.IP_ADDRESS = p_Ip_Address;
                mSkus.PACKSIZE = p_packSize;
                mSkus.USER_ID = p_UserId;
                mSkus.SHOW_ON_POS = p_SHOW_ON_POS;
                mSkus.Material = p_material;
                mSkus.Fit = p_fit;
                mSkus.Weight = p_weight;
                mSkus.Karat = p_karat;
                mSkus.MakeCharge = p_makeCharge;

                mSkus.ExecuteQuery();
                return "Record Updated";

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
        public bool InsertBarcode(string p_Company_Name, string p_Product_Name, 
            string p_Product_price, string size, string color, byte[] p_image)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertBARCODE mSkus = new spInsertBARCODE();

                mSkus.Connection = mConnection;
                mSkus.COMPANY_NAME= p_Company_Name;
                mSkus.PRODUCT_NAME = p_Product_Name;
                mSkus.PRODUCT_PRICE = p_Product_price;
                mSkus.PRODUCT_SIZE = size;
                mSkus.PRODUCT_COLOR = color;
                mSkus.BARCODE_IMAGE = p_image;
                
                mSkus.ExecuteQuery();
                return true;

            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                //return exp.Message;
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
        public bool InsertBarcodeBulk(string p_Company_Name, string p_Product_Name, string p_Product_price,
            string size, string color, byte[] p_image)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertBARCODE mSkus = new spInsertBARCODE();

                mSkus.Connection = mConnection;
                mSkus.COMPANY_NAME = p_Company_Name;
                mSkus.PRODUCT_NAME = p_Product_Name;
                mSkus.PRODUCT_PRICE = p_Product_price;
                mSkus.BARCODE_IMAGE = p_image;
                mSkus.PRODUCT_SIZE = size;
                mSkus.PRODUCT_COLOR = color;

                mSkus.ExecuteQuery();
                return true;

            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                //return exp.Message;
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
        public bool TruncateBarcode()
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spTruncateBARCODE mSkus = new spTruncateBARCODE();
                mSkus.Connection = mConnection;
                mSkus.ExecuteQuery();
                return true;

            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                //return exp.Message;
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
        public DataTable SelectSkuBarcode()
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spSelectBARCODE mBarcode = new spSelectBARCODE();
                mBarcode.Connection = mConnection;
                //mBarcode.PRODUCT_NAME = p_ROWNO;
                DataTable dt = mBarcode.ExecuteTable();
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
    }
}
