using System;
using System.Data;
using CORNCommon.Classes;
using CORNDatabaseLayer.Classes;
using System.Data.SqlTypes ;
using System.Data.SqlClient;
using System.Collections;
using CORNDataAccessLayer.Classes;

namespace CORNBusinessLayer.Classes
{
    /// <summary>
    /// Class For Order/Invoice/Sale Return Related Tasks
    /// <example>
    /// <list type="bullet">
    /// <item>
    /// Insert Order/Invoice/Sale Return
    /// </item>
    /// <term>
    /// Update Order/Invoice/Sale Return
    /// </term>
    /// <item>
    /// Get Order/Invoice/Sale Return
    /// </item>
    /// </list>
    /// </example>
    /// </summary>
    public class OrderEntryController
    {
        #region Constructor

        /// <summary>
        /// Constructor for OrderEntryController
        /// </summary>
        public OrderEntryController()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #endregion

        #region Select

        /// <summary>
        /// Gets Promotions
        /// </summary>
        /// <remarks>
        /// Returns Promotions as PromotionCollections_Controller
        /// </remarks>
        /// <param name="p_DistId">Location</param>
        /// <param name="Princpal_Id">Principal</param>
        /// <param name="pCurrentDate">Date</param>
        /// <returns>Promotions as PromotionCollections_Controller</returns>
        public PromotionCollections_Controller LoadSchemes(int p_DistId, int Princpal_Id, DateTime pCurrentDate)
        {
            IDbConnection m_Connection = null;
            DataControl dc = new DataControl();
            PromotionCollections_Controller pcc = new PromotionCollections_Controller();
            DataTable dt = null, dt2 = null, dt3 = null, dt4 = null, dt5 = null;

            try
            {

                m_Connection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                m_Connection.Open();

                #region Load Scheme Promotion c0llection

                uspSelectPROMOTIONS mSP = new uspSelectPROMOTIONS();
                mSP.Connection = m_Connection;
                mSP.DISTRIBUTOR_ID = p_DistId;
                mSP.PROMOTION_TYPE = Princpal_Id;
                mSP.IS_ACTIVE = true;
                mSP.START_DATE = DateTime.Parse(pCurrentDate.ToShortDateString() + " 00:00:00");
                mSP.END_DATE = DateTime.Parse(pCurrentDate.ToShortDateString() + " 23:59:59");//System.DateTime.Now ;
                dt2 = mSP.ExecuteTable();

                for (int j = 0; j <= dt2.Rows.Count - 1; j++)
                {	// this loop will get all active promotions against the scheme id and distributor id
                    Promotion_Collection pc = new Promotion_Collection();
                    pc.ObjBasketCol_Cntrl = new BasketCollection_Controller();
                    pc.ObjPromotionCustTypeCol_Cntrl = new PromotionCustTypeColl_Controller();
                    pc.ObjPromotionForCol_Cntrl = new PromotionForCollection_Controller();
                    pc.ObjPromotionVolClassCol_Cntrl = new PromotionCustVolclassColl_Controller();

                    pc.Dist_ID = int.Parse(dt2.Rows[j]["Distributor_ID"].ToString());
                    pc.Promotion_Code = dt2.Rows[j]["PROMOTION_CODE"].ToString();
                    pc.Promotion_Date = DateTime.Parse(dt2.Rows[j]["Promo_Date"].ToString());
                    pc.Promotion_Desc = dt2.Rows[j]["Promotion_Description"].ToString();
                    pc.Promotion_ID = long.Parse(dt2.Rows[j]["Promotion_ID"].ToString());
                    if (dt2.Rows[j]["Promotion_Selection"].ToString() != "")
                    { pc.Promotion_Selection = int.Parse(dt2.Rows[j]["Promotion_Selection"].ToString()); }
                    else
                    { pc.Promotion_Selection = -1; }
                    if (dt2.Rows[j]["Promotion_Type"].ToString() != "")
                    { pc.Promotion_Type = int.Parse(dt2.Rows[j]["Promotion_Type"].ToString()); }
                    else
                    { pc.Promotion_Type = -1; }
                    pc.Scheme_ID = int.Parse(dt2.Rows[j]["Scheme_ID"].ToString());
                    pc.Start_Date = DateTime.Parse(dt2.Rows[j]["Start_Date"].ToString());
                    pc.End_Date = DateTime.Parse(dt2.Rows[j]["End_Date"].ToString());
                    pc.Claimable = bool.Parse(dt2.Rows[j]["Claimable"].ToString());
                    pc.Is_Scheme = bool.Parse(dt2.Rows[j]["IS_SCHEME"].ToString());
                    // this loop will get basket data against the scheme id,PromotionID and distributor id

                    #region Basket Collection

                    uspSelectBASKET_MASTER_dt mBM = new uspSelectBASKET_MASTER_dt();
                    mBM.Connection = m_Connection;
                    mBM.DISTRIBUTOR_ID = pc.Dist_ID; //Configuration.DistributorId ;//System .Convert .ToInt32 (sc.Dist_ID) ;
                    mBM.SCHEME_ID = System.Convert.ToInt32(pc.Scheme_ID);
                    mBM.PROMOTION_ID = System.Convert.ToInt32(pc.Promotion_ID);
                    dt3 = mBM.ExecuteTable();

                    for (int k = 0; k <= dt3.Rows.Count - 1; k++)
                    {
                        Basket_Collection bc = new Basket_Collection();
                        bc.ObjBasketDtlCol_Cntrlr = new BasketDetailCollection_Controller();


                        bc.Basket_ID = long.Parse(dt3.Rows[k]["Basket_ID"].ToString());
                        bc.Basket_On = int.Parse(dt3.Rows[k]["Basket_On"].ToString());
                        if (dt3.Rows[k]["Basket_Selection"].ToString() != "")
                        { bc.Basket_Selection = int.Parse(dt3.Rows[k]["Basket_Selection"].ToString()); }
                        else
                        { bc.Basket_Selection = 0; }
                        bc.Dist_ID = int.Parse(dt3.Rows[k]["Distributor_ID"].ToString());
                        bc.Is_And = bool.Parse(dt3.Rows[k]["IS_AND"].ToString());
                        bc.Is_Basket = bool.Parse(dt3.Rows[k]["IS_Basket"].ToString());
                        bc.Is_Multiple = bool.Parse(dt3.Rows[k]["IS_Multiple"].ToString());
                        bc.Promotion_ID = long.Parse(dt3.Rows[k]["Promotion_ID"].ToString());
                        bc.Scheme_ID = int.Parse(dt2.Rows[j]["Scheme_ID"].ToString());
                        // this loop will get basketDetail data against the BasketID, scheme id,PromotionID and distributor id

                        #region Basket Detail
                        uspSelectBASKET_DETAIL_dt mBD = new uspSelectBASKET_DETAIL_dt();
                        mBD.Connection = m_Connection;
                        mBD.BASKET_ID = System.Convert.ToInt32(bc.Basket_ID);
                        mBD.DISTRIBUTOR_ID = bc.Dist_ID; ///Configuration.DistributorId ;//System .Convert .ToInt32 (sc.Dist_ID) ;
                        mBD.SCHEME_ID = System.Convert.ToInt32(pc.Scheme_ID);
                        mBD.PROMOTION_ID = System.Convert.ToInt32(pc.Promotion_ID);
                        dt4 = mBD.ExecuteTable();

                        for (int l = 0; l <= dt4.Rows.Count - 1; l++)
                        {
                            Basket_Detail_Collection bdc = new Basket_Detail_Collection();
                            bdc.ObjPromotionOfferCol_Cntrl = new PromotionOfferColl_Controller();

                            bdc.Basket_ID = long.Parse(dt4.Rows[l]["Basket_ID"].ToString());
                            bdc.BasketDetail_ID = long.Parse(dt4.Rows[l]["Basket_Detail_ID"].ToString());
                            bdc.Dist_ID = int.Parse(dt4.Rows[l]["Distributor_ID"].ToString());
                            bdc.Max_Val = decimal.Parse(dt4.Rows[l]["Max_Val"].ToString());
                            bdc.Min_Val = decimal.Parse(dt4.Rows[l]["Min_Val"].ToString());
                            bdc.Multiple_Of = int.Parse(dc.chkNull(dt4.Rows[l]["Multiple_of"].ToString()));
                            bdc.Promotion_ID = long.Parse(dt4.Rows[l]["Promotion_ID"].ToString());
                            bdc.Scheme_ID = int.Parse(dt4.Rows[l]["Scheme_ID"].ToString());
                            bdc.SKU_ID = int.Parse(dc.chkNull(dt4.Rows[l]["SKU_ID"].ToString()));
                            bdc.SKUBrand_ID = int.Parse(dc.chkNull(dt4.Rows[l]["Brand_ID"].ToString()));
                            bdc.SKUCatg_ID = int.Parse(dc.chkNull(dt4.Rows[l]["Category_ID"].ToString()));
                            bdc.SKUDiv_ID = int.Parse(dc.chkNull(dt4.Rows[l]["Division_ID"].ToString()));
                            bdc.SKUGroup_ID = int.Parse(dc.chkNull(dt4.Rows[l]["SKU_Group_ID"].ToString()));
                            bdc.SKUProductLine_ID = int.Parse(dc.chkNull(dt4.Rows[l]["Variant_ID"].ToString()));
                            bdc.UOM_ID = int.Parse(dc.chkNull(dt4.Rows[l]["UOM_ID"].ToString()));
                            bdc.SKUCompany_ID = int.Parse(dc.chkNull(dt4.Rows[l]["Company_ID"].ToString()));
                            bc.ObjBasketDtlCol_Cntrlr.Add(bdc);

                            #region Basket Promotion Offer Collection
                            /////////////////////////////////////////////
                            ///
                            // This loop will get Promotion Offer data against the BasketID, scheme id,
                            // PromotionID,basketDetail ID and distributor id
                            uspSelectPROMOTION_OFFER_dt mPO = new uspSelectPROMOTION_OFFER_dt();

                            mPO.Connection = m_Connection;
                            mPO.BASKET_ID = System.Convert.ToInt32(bc.Basket_ID);
                            mPO.DISTRIBUTOR_ID = bc.Dist_ID;//System .Convert .ToInt32 (sc.Dist_ID) ;
                            mPO.SCHEME_ID = System.Convert.ToInt32(pc.Scheme_ID);
                            mPO.PROMOTION_ID = System.Convert.ToInt32(pc.Promotion_ID);
                            mPO.BASKET_DETAIL_ID = System.Convert.ToInt32(bdc.BasketDetail_ID);
                            dt5 = null;		// no need to keep old values in dt4
                            dt5 = mPO.ExecuteTable();
                            for (int m = 0; m <= dt5.Rows.Count - 1; m++)
                            {
                                #region to avoid null values
                                string mDiscount = dt5.Rows[m]["Discount"].ToString();
                                string mOfferValue = dt5.Rows[m]["Offer_Value"].ToString();
                                string mQty = dt5.Rows[m]["Quantity"].ToString();
                                if ((mDiscount == "") || (mDiscount == null))
                                { mDiscount = "0"; }
                                if ((mOfferValue == "") || (mOfferValue == null))
                                { mOfferValue = "0"; }
                                if ((mQty == "") || (mQty == null))
                                { mQty = "0"; }
                                #endregion

                                PromotionOffer_Collection poc = new PromotionOffer_Collection();

                                poc.Basket_ID = long.Parse(dt5.Rows[m]["Basket_ID"].ToString());
                                poc.BasketDetail_ID = long.Parse(dt5.Rows[m]["Basket_Detail_ID"].ToString());
                                poc.Discount = float.Parse(mDiscount);
                                poc.Dist_ID = int.Parse(dt5.Rows[m]["Distributor_ID"].ToString());
                                poc.Is_And = bool.Parse(dt5.Rows[m]["Is_And"].ToString());
                                poc.Offer_Value = decimal.Parse(mOfferValue);
                                poc.Promotion_ID = long.Parse(dt5.Rows[m]["Promotion_ID"].ToString());
                                poc.Promotion_Offer_ID = long.Parse(dt5.Rows[m]["Promotion_Offer_ID"].ToString());
                                poc.Quantity = int.Parse(mQty);
                                poc.Scheme_ID = int.Parse(dt5.Rows[m]["Scheme_ID"].ToString());
                                poc.SKU_ID = int.Parse(dc.chkNull(dt5.Rows[m]["SKU_ID"].ToString()));
                                poc.UOM_ID = int.Parse(dc.chkNull(dt5.Rows[m]["UOM_ID"].ToString()));

                                bdc.ObjPromotionOfferCol_Cntrl.Add(poc);
                            }
                            #endregion

                        }
                        #endregion
                        pc.ObjBasketCol_Cntrl.Add(bc);
                    }
                    #endregion
                    ////////////////////////////////////////	
                    // this loop will get Promotion_For data against the scheme id,PromotionID and distributor id					
                    #region Promotion_For Collection
                    dt3 = null;
                    uspSelectPROMOTION_FOR_dt mPF = new uspSelectPROMOTION_FOR_dt();
                    mPF.Connection = m_Connection;
                    mPF.PROMOTION_FOR_ID = Constants.LongNullValue;
                    mPF.DISTRIBUTOR_ID = pc.Dist_ID;//System .Convert .ToInt32 (sc.Dist_ID) ;
                    mPF.ASSIGNED_DISTRIBUTOR_ID = Configuration.DistributorId;
                    mPF.SCHEME_ID = System.Convert.ToInt32(pc.Scheme_ID);
                    mPF.PROMOTION_ID = System.Convert.ToInt32(pc.Promotion_ID);
                    dt3 = mPF.ExecuteTable();

                    for (int k = 0; k <= dt3.Rows.Count - 1; k++)
                    {
                        PromotionFor_Collection pfc = new PromotionFor_Collection();
                        pfc.Dist_ID = int.Parse(dt3.Rows[k]["Distributor_ID"].ToString());
                        pfc.Assigned_Dist_ID = int.Parse(dt3.Rows[k]["Assigned_Distributor_ID"].ToString());
                        pfc.Promotion_For_ID = long.Parse(dt3.Rows[k]["Promotion_For_ID"].ToString());
                        pfc.Promotion_ID = long.Parse(dt3.Rows[k]["Promotion_ID"].ToString());
                        pfc.Scheme_ID = int.Parse(dt3.Rows[k]["Scheme_ID"].ToString());
                        pc.ObjPromotionForCol_Cntrl.Add(pfc);
                    }

                    #endregion
                    ////////////////////////////////////////
                    ///// this loop will get Promotion_For_Customer_VolClass data against the scheme id,PromotionID and distributor id					
                    #region Promotion_For_VOLUMECLASS Collection
                    dt3 = null;
                    spSelectPROMOTION_CUSTOMER_VOLUMECLASS mPFC = new spSelectPROMOTION_CUSTOMER_VOLUMECLASS();
                    mPFC.Connection = m_Connection;
                    mPFC.DISTRIBUTOR_ID = pc.Dist_ID;
                    mPFC.SCHEME_ID = System.Convert.ToInt32(pc.Scheme_ID);
                    mPFC.PROMOTION_ID = System.Convert.ToInt32(pc.Promotion_ID);
                    dt3 = mPFC.ExecuteTable();

                    for (int k = 0; k <= dt3.Rows.Count - 1; k++)
                    {
                        PromotionCustomerVolClass_Collection pfcc = new PromotionCustomerVolClass_Collection();
                        pfcc.Dist_ID = int.Parse(dt3.Rows[k]["Distributor_ID"].ToString());
                        pfcc.Promotion_ID = long.Parse(dt3.Rows[k]["Promotion_ID"].ToString());
                        pfcc.Scheme_ID = int.Parse(dt3.Rows[k]["Scheme_ID"].ToString());
                        pfcc.Customer_VolClass_ID = int.Parse(dt3.Rows[k]["CUSTOMER_VOLUMECLASS_ID"].ToString());
                        pc.ObjPromotionVolClassCol_Cntrl.Add(pfcc);
                    }

                    #endregion
                    ////////////////////////////////////////
                    //////// this loop will get Promotion_Customer_type data against the scheme id,PromotionID and distributor id					
                    #region Promotion_Customer_Type Collection
                    dt3 = null;
                    uspSelectPROMOTION_CUSTOMER_TYPE_dt mPCT = new uspSelectPROMOTION_CUSTOMER_TYPE_dt();
                    mPCT.Connection = m_Connection;
                    mPCT.DISTRIBUTOR_ID = pc.Dist_ID; // Configuration.DistributorId ;//System .Convert .ToInt32 (sc.Dist_ID) ;
                    mPCT.SCHEME_ID = System.Convert.ToInt32(pc.Scheme_ID);
                    mPCT.PROMOTION_ID = System.Convert.ToInt32(pc.Promotion_ID);
                    dt3 = mPCT.ExecuteTable();

                    for (int k = 0; k <= dt3.Rows.Count - 1; k++)
                    {
                        PromotionCustomerType_Collection pctc = new PromotionCustomerType_Collection();
                        pctc.Dist_ID = int.Parse(dt3.Rows[k]["Distributor_ID"].ToString());
                        pctc.Customer_Type_ID = int.Parse(dt3.Rows[k]["Customer_Type_ID"].ToString());
                        pctc.Promotion_Cust_Type_ID = long.Parse(dt3.Rows[k]["Promotion_Customer_Type_ID"].ToString());
                        pctc.Promotion_ID = long.Parse(dt3.Rows[k]["Promotion_ID"].ToString());
                        pctc.Scheme_ID = int.Parse(dt3.Rows[k]["Scheme_ID"].ToString());
                        pc.ObjPromotionCustTypeCol_Cntrl.Add(pctc);
                    }

                    #endregion
                    ////////////////////////////////////////
                    pcc.Add_PCol(pc);

                }
                #endregion

                return pcc;


            }
            catch (Exception ex)
            {
                ExceptionPublisher.PublishException(ex);
                return null;
            }
            finally
            {
                if (m_Connection != null && m_Connection.State != ConnectionState.Open)
                {
                    m_Connection.Close();
                }
            }
        }

        /// <summary>
        /// Gets Promotion Offers
        /// </summary>
        /// <remarks>
        /// Returns Promotion Offers as ArrayList
        /// </remarks>
        /// <param name="pc">PromotionCollections_Controller</param>
        /// <param name="p_CustomerVoldClass">PromotionClass</param>
        /// <param name="p_CustomerId">Customer</param>
        /// <param name="p_OrderDetail">OrderDetailDatatable</param>
        /// <param name="p_IsScheme">IsScheme</param>
        /// <returns>Promotion Offers as ArrayList</returns>
        public ArrayList GetPromotionOffers(PromotionCollections_Controller pc, int p_CustomerVoldClass, int p_CustomerId, DataTable p_OrderDetail, bool p_IsScheme)
        {

            int PromotionIdx = 0;
            DataRow drOrderDetail = null;

            PromoOffersCol_Controller POffersCol_Cntrlr = new PromoOffersCol_Controller();
            SKUGroupController GroupCtl = new SKUGroupController();

            ArrayList arrPromotionOffers = new ArrayList();



            while (PromotionIdx < pc.Count)
            {


                bool IsValidCustomerTypeId = false;
                bool IsValidCustomerVolCla = false;

                DataTable dtGroup = new DataTable();
                dtGroup.Columns.Add("GroupId", typeof(long));

                for (int j = 0; j < pc.Get_PCol(PromotionIdx).ObjPromotionCustTypeCol_Cntrl.Count; j++)
                {
                    if (pc.Get_PCol(PromotionIdx).ObjPromotionCustTypeCol_Cntrl.Get(j).Customer_Type_ID == p_CustomerId)
                    {
                        IsValidCustomerTypeId = true;
                        break;
                    }
                }

                for (int j = 0; j < pc.Get_PCol(PromotionIdx).ObjPromotionVolClassCol_Cntrl.Count; j++)
                {
                    if (pc.Get_PCol(PromotionIdx).ObjPromotionVolClassCol_Cntrl.Get(j).Customer_VolClass_ID == p_CustomerVoldClass)
                    {
                        IsValidCustomerVolCla = true;
                        break;
                    }
                }
                if (IsValidCustomerTypeId == true && IsValidCustomerVolCla == true)
                {

                    for (int idxOrderDetail = 0; idxOrderDetail < p_OrderDetail.Rows.Count; idxOrderDetail++)
                    {


                        drOrderDetail = p_OrderDetail.Rows[idxOrderDetail];


                        #region Slab

                        BasketCollection_Controller ObjBasket = pc.Get_PCol(PromotionIdx).ObjBasketCol_Cntrl;

                        for (int i = 0; i < ObjBasket.Count; i++)
                        {
                            bool Applygroup = false;
                            decimal dValueToCompare = 0;
                            int dMultipalGroupItem = 0;

                            BasketDetailCollection_Controller objBasketDetail = ObjBasket.Get(i).ObjBasketDtlCol_Cntrlr;

                            for (int j = 0; j < objBasketDetail.Count; j++)
                            {

                                if (objBasketDetail.Get(j).SKU_ID > 0)
                                {
                                    #region Single SKU
                                    //If SLAB applied at single SKU

                                    if (objBasketDetail.Get(j).SKU_ID == int.Parse(drOrderDetail["SKU_ID"].ToString()))
                                    {
                                        //Check at Amount or Quantity SLAB is Applied
                                        if (ObjBasket.Get(i).Basket_On == Constants.Basket_On_Amount)
                                        {
                                            dValueToCompare = decimal.Parse(drOrderDetail["Amount"].ToString());
                                        }
                                        else if (ObjBasket.Get(i).Basket_On == Constants.Basket_On_Quantity)
                                        {
                                            dValueToCompare = decimal.Parse(drOrderDetail["QUANTITY_UNIT"].ToString());
                                        }

                                        //Check if SLAB is applicable

                                        if (dValueToCompare >= objBasketDetail.Get(j).Min_Val && (dValueToCompare <= objBasketDetail.Get(j).Max_Val || objBasketDetail.Get(j).Max_Val == 0))
                                        {

                                            //Add applied Promotion offer in array										

                                            PromotionOfferColl_Controller objPromotionOffer = objBasketDetail.Get(j).ObjPromotionOfferCol_Cntrl;
                                            PromoOffers_Collection AppProCol = new PromoOffers_Collection();

                                            if (dValueToCompare > objBasketDetail.Get(j).Multiple_Of && objBasketDetail.Get(j).Multiple_Of > 0)
                                            {
                                                int iMultiply = Convert.ToInt32(Math.Floor(Convert.ToDouble(dValueToCompare / objBasketDetail.Get(j).Multiple_Of)));
                                                AppProCol.Quantity = objPromotionOffer.Get(j).Quantity * iMultiply;
                                                AppProCol.Offer_Value = objPromotionOffer.Get(j).Offer_Value * iMultiply;

                                            }
                                            else
                                            {
                                                AppProCol.Quantity = objPromotionOffer.Get(j).Quantity;
                                                AppProCol.Offer_Value = objPromotionOffer.Get(j).Offer_Value;

                                            }

                                            AppProCol.SKU_ID = int.Parse(drOrderDetail["SKU_ID"].ToString());
                                            AppProCol.Group_ID = Constants.IntNullValue;
                                            AppProCol.Promotion_ID = int.Parse(objPromotionOffer.Get(j).Promotion_ID.ToString());
                                            AppProCol.Scheme_ID = objPromotionOffer.Get(j).Scheme_ID;
                                            AppProCol.Basket_ID = objPromotionOffer.Get(j).Basket_ID;
                                            AppProCol.BasketDetail_ID = objPromotionOffer.Get(j).BasketDetail_ID;
                                            AppProCol.Free_SKU_ID = objPromotionOffer.Get(j).SKU_ID;
                                            AppProCol.Discount = objPromotionOffer.Get(j).Discount;
                                            AppProCol.Is_And = pc.Get_PCol(PromotionIdx).Is_Scheme;
                                            AppProCol.Is_Claimable = pc.Get_PCol(PromotionIdx).Claimable;

                                            arrPromotionOffers.Add(AppProCol);

                                        }


                                    }
                                    #endregion
                                }
                                else if (objBasketDetail.Get(j).SKUGroup_ID > 0)
                                {
                                    SKUGroupController mGroup = new SKUGroupController();

                                    //check if Already Apply Group then return 

                                    foreach (DataRow drGroup in dtGroup.Rows)
                                    {
                                        if (drGroup[0].ToString() == objBasketDetail.Get(j).SKUGroup_ID.ToString())
                                        {
                                            Applygroup = true;
                                        }
                                    }
                                    #region Group
                                    if (Applygroup == false)
                                    {
                                        dValueToCompare = 0;
                                        dMultipalGroupItem = 0;

                                        if (ObjBasket.Get(i).Basket_On == Constants.Basket_On_Amount)
                                        {
                                            foreach (DataRow dg in p_OrderDetail.Rows)
                                            {
                                                if (GroupCtl.ExistsInGroup(Constants.IntNullValue, objBasketDetail.Get(j).SKUGroup_ID, int.Parse(dg["SKU_ID"].ToString())))
                                                {
                                                    dValueToCompare += decimal.Parse(dg["AMOUNT"].ToString());
                                                    dMultipalGroupItem += 1;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            foreach (DataRow dg in p_OrderDetail.Rows)
                                            {
                                                if (GroupCtl.ExistsInGroup(Constants.IntNullValue, objBasketDetail.Get(j).SKUGroup_ID, int.Parse(dg["SKU_ID"].ToString())))
                                                {
                                                    dValueToCompare += decimal.Parse(dg["QUANTITY_UNIT"].ToString());
                                                    dMultipalGroupItem += 1;
                                                }
                                            }
                                        }

                                        if (dValueToCompare >= objBasketDetail.Get(j).Min_Val && (dValueToCompare <= objBasketDetail.Get(j).Max_Val || objBasketDetail.Get(j).Max_Val == 0))
                                        {

                                            //Add applied Promotion offer in array										

                                            PromotionOfferColl_Controller objPromotionOffer = objBasketDetail.Get(j).ObjPromotionOfferCol_Cntrl;
                                            PromoOffers_Collection AppProCol = new PromoOffers_Collection();

                                            if (dValueToCompare > objBasketDetail.Get(j).Multiple_Of && objBasketDetail.Get(j).Multiple_Of > 0)
                                            {
                                                int iMultiply = Convert.ToInt32(Math.Floor(Convert.ToDouble(dValueToCompare / objBasketDetail.Get(j).Multiple_Of)));
                                                AppProCol.Quantity = (objPromotionOffer.Get(j).Quantity * iMultiply);
                                                AppProCol.Offer_Value = (objPromotionOffer.Get(j).Offer_Value * iMultiply) / dMultipalGroupItem;

                                            }
                                            else
                                            {
                                                AppProCol.Quantity = objPromotionOffer.Get(j).Quantity;
                                                AppProCol.Offer_Value = objPromotionOffer.Get(j).Offer_Value / dMultipalGroupItem;

                                            }

                                            AppProCol.SKU_ID = objBasketDetail.Get(j).SKU_ID;
                                            AppProCol.Group_ID = objBasketDetail.Get(j).SKUGroup_ID;
                                            AppProCol.Promotion_ID = int.Parse(objPromotionOffer.Get(j).Promotion_ID.ToString());
                                            AppProCol.Scheme_ID = objPromotionOffer.Get(j).Scheme_ID;
                                            AppProCol.Basket_ID = objPromotionOffer.Get(j).Basket_ID;
                                            AppProCol.BasketDetail_ID = objPromotionOffer.Get(j).BasketDetail_ID;
                                            AppProCol.Free_SKU_ID = objPromotionOffer.Get(j).SKU_ID;
                                            AppProCol.Discount = objPromotionOffer.Get(j).Discount;
                                            AppProCol.Is_And = pc.Get_PCol(PromotionIdx).Is_Scheme;
                                            AppProCol.Is_Claimable = pc.Get_PCol(PromotionIdx).Claimable;
                                            arrPromotionOffers.Add(AppProCol);


                                            DataRow drNewGroup = dtGroup.NewRow();
                                            drNewGroup[0] = AppProCol.Group_ID.ToString();
                                            dtGroup.Rows.Add(drNewGroup);

                                        }
                                        #endregion
                                    }
                                }

                            }
                        }
                        #endregion
                    }
                }
                PromotionIdx++;
            }
            return arrPromotionOffers;
        }

        /// <summary>
        /// Gets Pending Orders
        /// </summary>
        /// <param name="p_Distributor_Id">Loation</param>
        /// <param name="p_Area_Id">Route</param>
        /// <param name="p_Principal_Id">Principal</param>
        /// <param name="p_Order_Booker">OrderBooker</param>
        /// <param name="p_DeliveryMan_Id">DeliveryMan</param>
        /// <param name="p_OrderStatus">Status</param>
        /// <param name="p_Ordertype">Type</param>
        /// <param name="p_UserId">InsertedBy</param>
        /// <param name="p_DOCUMENT_DATE">Date</param>
        /// <returns>Pending Orders as Datatable</returns>
        public DataTable SelectPendingOrder(int p_Distributor_Id, int p_Area_Id, int p_Principal_Id, int p_Order_Booker, int p_DeliveryMan_Id, int p_OrderStatus, int p_Ordertype, int p_UserId, DateTime p_DOCUMENT_DATE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspSelectPendingOrder mOrder = new UspSelectPendingOrder();
                mOrder.Connection = mConnection;
                mOrder.DISTRIBUTOR_ID = p_Distributor_Id;
                mOrder.AREA_ID = p_Area_Id;
                mOrder.ORDERBOOKER_ID = p_Order_Booker;
                mOrder.DELIVERYMAN_ID = p_DeliveryMan_Id;
                mOrder.USER_ID = p_UserId;
                mOrder.PRINCIPAL_ID = p_Principal_Id;
                mOrder.STATUS_ID = p_OrderStatus;
                mOrder.ORDER_TYPE_ID = p_Ordertype;
                mOrder.DOCUMENT_DATE = p_DOCUMENT_DATE;
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

        /// <summary>
        /// Gets Order Detail
        /// </summary>
        /// <param name="p_Distributor_Id">Location</param>
        /// <param name="p_SaleOrder_Id">Order</param>
        /// <returns>Order Detail as Datatable</returns>
        public DataTable SelectOrderDetail(int p_Distributor_Id, long p_SaleOrder_Id)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectSALE_ORDER_DETAIL mOrderDetail = new spSelectSALE_ORDER_DETAIL();
                mOrderDetail.Connection = mConnection;
                mOrderDetail.DISTRIBUTOR_ID = p_Distributor_Id;
                mOrderDetail.SALE_ORDER_ID = p_SaleOrder_Id;
                mOrderDetail.IS_DELETED = false;
                DataTable dt = mOrderDetail.ExecuteTable();
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
        /// Gets Promotions Of Order
        /// </summary>
        /// <param name="p_Distributor_Id">Location</param>
        /// <param name="p_SaleOrder_Id">Order</param>
        /// <returns>Promotion Of Order as Datatable</returns>
        public DataTable SelectOrderPromotion(int p_Distributor_Id, long p_SaleOrder_Id)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectSALE_ORDER_PROMOTION mOrderDetail = new spSelectSALE_ORDER_PROMOTION();
                mOrderDetail.Connection = mConnection;
                mOrderDetail.DISTRIBUTOR_ID = p_Distributor_Id;
                mOrderDetail.SALE_ORDER_ID = p_SaleOrder_Id;
                DataTable dt = mOrderDetail.ExecuteTable();
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
        /// Gets Promotions Of Invoice
        /// </summary>
        /// <param name="p_Distributor_Id">Location</param>
        /// <param name="p_SaleInvoice_Id">Invoice</param>
        /// <returns>Promotion Of Invoice as Datatable</returns>
        public DataTable SelectInvoicePromotion(int p_Distributor_Id, long p_SaleInvoice_Id)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectSALE_INVOICE_PROMOTION mOrderDetail = new spSelectSALE_INVOICE_PROMOTION();
                mOrderDetail.Connection = mConnection;
                mOrderDetail.DISTRIBUTOR_ID = p_Distributor_Id;
                mOrderDetail.SALE_INVOICE_ID = p_SaleInvoice_Id;
                DataTable dt = mOrderDetail.ExecuteTable();
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
        /// Gets Legend
        /// </summary>
        /// <returns>Legend as Datatable</returns>
        public DataTable SelectLegend()
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectLEGEND mOrderDetail = new spSelectLEGEND();
                mOrderDetail.Connection = mConnection;
                mOrderDetail.LEGEND_ID = Constants.IntNullValue;
                mOrderDetail.LEGEND_TYPE_ID = Constants.IntNullValue;
                mOrderDetail.TIMESTAMP = Constants.DateNullValue;
                mOrderDetail.LAST_UPDATE_DATE = Constants.DateNullValue;
                mOrderDetail.LEGEND_DESCRIPTION = null;
                mOrderDetail.LEGEND_NAME = null;
                mOrderDetail.IS_ACTIVE = true;
                mOrderDetail.USER_ID = Constants.IntNullValue;
                DataTable dt = mOrderDetail.ExecuteTable();
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
        /// Gets Tranporter
        /// </summary>
        /// <returns>Transporter as Datatable</returns>
        public DataTable SelectTranspoter()
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectTRANSPOTER mTranspoter = new spSelectTRANSPOTER();
                mTranspoter.Connection = mConnection;
                DataTable dt = mTranspoter.ExecuteTable();
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
        /// Gets Transporter Invoices
        /// </summary>
        /// <param name="p_Distributor_id">Location</param>
        /// <param name="p_CustomerId">Customer</param>
        /// <param name="p_type">Type</param>
        /// <returns>Transporter Invoices as Datatable</returns>
        public DataTable SelectTranspoterInvoice(int p_Distributor_id, long p_CustomerId, int p_type)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspSelectTranspoterInvoice mInvoice = new UspSelectTranspoterInvoice();
                mInvoice.Connection = mConnection;
                mInvoice.DISTRIBUTOR_ID = p_Distributor_id;
                mInvoice.CUSTOMER_ID = p_CustomerId;
                mInvoice.TypeId = p_type;
                DataTable dt = mInvoice.ExecuteTable();
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
        /// Converts Orders To Invoices
        /// </summary>
        /// <param name="p_Distributor_Id">Location</param>
        /// <param name="p_MANUAL_ORDER_ID">ManualInvocie</param>
        /// <param name="p_Customer_Id">Customer</param>
        /// <param name="p_Principal_Id">Principal</param>
        /// <param name="p_SaleOrder_Id">Order</param>
        /// <param name="p_Document_Date">Date</param>
        /// <param name="p_GrossSale">Sale</param>
        /// <param name="p_Discount">Discount</param>
        /// <param name="p_scheme">Scheme</param>
        /// <param name="p_GstAmt">GST</param>
        /// <param name="p_Net_Amount">NetAmount</param>
        /// <param name="p_OrderStatus">Status</param>
        /// <param name="p_OrderTypeId">Type</param>
        /// <param name="p_UserId">InsertedBy</param>
        /// <param name="p_PayeesName">Payee</param>
        /// <returns></returns>
        public DataTable ConvertOrder_to_Invoice(int p_Distributor_Id, string p_MANUAL_ORDER_ID, long p_Customer_Id, int p_Principal_Id,
            long p_SaleOrder_Id, DateTime p_Document_Date, decimal p_GrossSale, decimal p_Discount, decimal p_scheme,
            decimal p_GstAmt, decimal p_Net_Amount, int p_OrderStatus, int p_OrderTypeId, int p_UserId, string p_PayeesName)
        {
            #region variables
            IDbTransaction mTransaction = null;
            IDbConnection mConnection = null;
            #endregion

            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspConvertOrdertoInvoice mOrder = new UspConvertOrdertoInvoice();
                mTransaction = ProviderFactory.GetTransaction(mConnection);
                mOrder.Connection = mConnection;
                mOrder.Transaction = mTransaction;
                mOrder.DISTRIBUTOR_ID = p_Distributor_Id;
                mOrder.PRINCIPAL_ID = p_Principal_Id;
                mOrder.CUSTOMER_ID = p_Customer_Id;
                mOrder.DOCUMENT_DATE = p_Document_Date;
                mOrder.SALE_ORDER_ID = p_SaleOrder_Id;

                if (p_OrderTypeId == Constants.Credit_Order_Id)
                {
                    mOrder.NET_AMOUNT = p_Net_Amount;
                }
                else
                {
                    mOrder.NET_AMOUNT = 0;
                }
                mOrder.ORDER_STATUS = p_OrderStatus;
                DataTable dt = mOrder.ExecuteTable();
                if (dt.Columns.Count > 1)
                {
                    mTransaction.Rollback();
                    return dt;
                }

                #region Account Posting

                LedgerController LController = new LedgerController();
                Configuration.GetAccountHead();
                string VoucherNo = LController.SelectLedgerMaxDocumentId(Constants.Journal_Voucher, p_Distributor_Id, 0);

                if (p_OrderTypeId == Constants.Advance_PaymentOrder_id)
                {
                    LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.SaleAccount), p_Distributor_Id, 0, p_GrossSale, p_Document_Date, "Gross Sale Value", DateTime.Now, p_Principal_Id, int.Parse(p_Customer_Id.ToString()), long.Parse(dt.Rows[0][0].ToString()), p_MANUAL_ORDER_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.Cash_Advance, p_PayeesName);
                    if (p_Discount > 0)
                    {
                        LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.SaleDiscount), p_Distributor_Id, p_Discount, 0, p_Document_Date, "Standard Discount", DateTime.Now, p_Principal_Id, int.Parse(p_Customer_Id.ToString()), long.Parse(dt.Rows[0][0].ToString()), p_MANUAL_ORDER_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.Cash_Advance, p_PayeesName);
                    }
                    if (p_scheme > 0)
                    {
                        LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.SaleScheme), p_Distributor_Id, p_scheme, 0, p_Document_Date, "Extra Discount", DateTime.Now, p_Principal_Id, int.Parse(p_Customer_Id.ToString()), long.Parse(dt.Rows[0][0].ToString()), p_MANUAL_ORDER_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.Cash_Advance, p_PayeesName);
                    }
                    if (p_GstAmt > 0)
                    {
                        LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.GSTAccount), p_Distributor_Id, 0, p_GstAmt, p_Document_Date, "GST Tax", DateTime.Now, p_Principal_Id, int.Parse(p_Customer_Id.ToString()), long.Parse(dt.Rows[0][0].ToString()), p_MANUAL_ORDER_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.Cash_Advance, p_PayeesName);
                    }

                    LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.AccountReceivable), p_Distributor_Id, p_Net_Amount, 0, p_Document_Date, "Recevieable from Customer", DateTime.Now, p_Principal_Id, int.Parse(p_Customer_Id.ToString()), long.Parse(dt.Rows[0][0].ToString()), p_MANUAL_ORDER_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.Cash_Advance, p_PayeesName);

                }
                else if (p_OrderTypeId == Constants.Credit_Order_Id)
                {

                    LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.AccountReceivable), p_Distributor_Id, p_Net_Amount, 0, p_Document_Date, "Credit Sale Default", DateTime.Now, p_Principal_Id, int.Parse(p_Customer_Id.ToString()), long.Parse(dt.Rows[0][0].ToString()), p_MANUAL_ORDER_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.CreditSale, p_PayeesName);
                    LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.SaleAccount), p_Distributor_Id, 0, p_Net_Amount, p_Document_Date, "Credit Sale Default", DateTime.Now, p_Principal_Id, int.Parse(p_Customer_Id.ToString()), long.Parse(dt.Rows[0][0].ToString()), p_MANUAL_ORDER_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.CreditSale, p_PayeesName);
                }
                #endregion

                mTransaction.Commit();
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

        public DataTable GetDocumentNo(DateTime p_DOCUMENT_DATE, int p_USER_ID, int p_TYPE_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetDocumentNo mDoc = new uspGetDocumentNo();
                mDoc.Connection = mConnection;
                mDoc.DOCUMENT_DATE = p_DOCUMENT_DATE;
                mDoc.USER_ID = p_USER_ID;
                mDoc.TYPE_ID = p_TYPE_ID;
                DataTable dt = mDoc.ExecuteTable();
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

        public DataTable GetDocumentDetail(long p_DocID, int p_TYPE_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetDocumentDetail mOrderDetail = new uspGetDocumentDetail();
                mOrderDetail.Connection = mConnection;
                mOrderDetail.DocID = p_DocID;
                mOrderDetail.TYPE_ID = p_TYPE_ID;
                DataTable dt = mOrderDetail.ExecuteTable();
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

        public int Add_CreditNote(decimal p_netAmount, int p_CustomerId, int p_UserId, int p_TypeId)
        {
            IDbConnection mConnection = null;
            int Credit_Note_Id;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                SPSEL_INS_UPD_CREDIT_NOTE mOrderDetail = new SPSEL_INS_UPD_CREDIT_NOTE();
                mOrderDetail.Connection = mConnection;
                mOrderDetail.CUSTOMER_ID = p_CustomerId;
                mOrderDetail.TYPE_ID = p_TypeId;
                mOrderDetail.NET_AMOUNT = p_netAmount;
                mOrderDetail.USER_ID = p_UserId;
                mOrderDetail.TIME_STAMP = DateTime.Now;
                mOrderDetail.MODIFY_DATE = DateTime.Now;
                Credit_Note_Id = mOrderDetail.ExecuteQuery();
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return -1;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
            return Credit_Note_Id;
        }

        public DataTable SelectSaleReport(int p_Distributor_Id, int p_UserId, DateTime p_StartDate, DateTime p_EndDate, long p_SALE_INVOICE_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                sp_SelectSaleReport mOrder = new sp_SelectSaleReport();
                mOrder.Connection = mConnection;
                mOrder.DISTRIBUTOR_ID = p_Distributor_Id;
                mOrder.SALE_INVOICE_ID = p_SALE_INVOICE_ID;
                mOrder.USER_ID = p_UserId;
                mOrder.STARTDATE = p_StartDate;
                mOrder.ENDDATE = p_EndDate;
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



        /// <summary>
        /// Gets Orders And Invoices Summary
        /// </summary>
        /// <param name="p_Distributor_ID">Location</param>
        /// <param name="p_Areaid">Market</param>
        /// <param name="p_Principal_Id">Principal</param>
        /// <param name="FromDocNo">DateFrom</param>
        /// <param name="ToDocNo">DateTo</param>
        /// <param name="DocumentTypeId">Type</param>
        /// <param name="p_IS_REGISTERED">IsRegistered</param>
        /// <param name="p_DELIVERYMAN_ID">Deliveryman</param>
        /// <returns>Orders And Invoices Summary as Datatable</returns>
        public DataTable SelectDocumentforView(int p_Distributor_ID, int p_Areaid, int p_Principal_Id, DateTime FromDocNo, DateTime ToDocNo, int DocumentTypeId, int p_IS_REGISTERED, int p_DELIVERYMAN_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspDocumentView ObjPrint = new UspDocumentView();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBTOR_ID = p_Distributor_ID;
                ObjPrint.AREA_ID = p_Areaid;
                ObjPrint.PRINCIPAL_ID = p_Principal_Id;
                ObjPrint.FROM_DATE = FromDocNo;
                ObjPrint.TO_DATE = ToDocNo;
                ObjPrint.TYPE_ID = DocumentTypeId;
                ObjPrint.IS_REGISTERED = p_IS_REGISTERED;
                ObjPrint.DELIVERYMAN_ID = p_DELIVERYMAN_ID;
                DataTable dt = ObjPrint.ExecuteTable();
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
        public static string GetMaxInvoiceId()
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspSelectMaxInvoiceNO mInvoiceDetail = new uspSelectMaxInvoiceNO();

                mInvoiceDetail.Connection = mConnection;
                string MaxId;
                return MaxId = mInvoiceDetail.ExecuteScalar();

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

        public static string GetMaxInvoiceId(int Distributor_id)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspSelectMaxInvoiceNODist mInvoiceDetail = new uspSelectMaxInvoiceNODist();

                mInvoiceDetail.Connection = mConnection;
                mInvoiceDetail.DISTRIBUTOR_ID = Distributor_id;
                string MaxId;
                return MaxId = mInvoiceDetail.ExecuteScalar();

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

        public DataTable GetPendingTaxAuthorityInvoices(int p_Distributor_ID,int p_Type, DateTime p_FromDate, DateTime p_To_Date)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetPendingTaxAuthorityInvoices ObjPrint = new uspGetPendingTaxAuthorityInvoices();

                ObjPrint.Connection = mConnection;
                ObjPrint.Distributor_id = p_Distributor_ID;
                ObjPrint.Type = p_Type;
                ObjPrint.From_date = p_FromDate;
                ObjPrint.To_Date = p_To_Date;


                DataTable dt = ObjPrint.ExecuteTable();

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

        #region Rollback

        /// <summary>
        /// Gets Rollback Data For Order, Invoice And Sale Return
        /// </summary>
        /// <param name="p_Distributor_Id">Location</param>
        /// <param name="p_Principal_Id">Principal</param>
        /// <param name="p_Order_Booker">OrderBooker</param>
        /// <param name="p_TypeId">Type</param>
        /// <param name="p_DocumentDate">Date</param>
        /// <returns>Rollback Data For Order, Invoice And Sale Return as Datatable</returns>
        public DataTable SelectRollBackDocument(int p_Distributor_Id, int p_Principal_Id, int p_Order_Booker, int p_TypeId, DateTime p_DocumentDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspSelectRollBackDocument mOrder = new UspSelectRollBackDocument();
                mOrder.Connection = mConnection;
                mOrder.DISTRIBUTOR_ID = p_Distributor_Id;
                mOrder.DOCUMENT_TYPE = p_TypeId;
                mOrder.PRINCIPAL_ID = p_Principal_Id;
                mOrder.ORDERBOOKER_ID = p_Order_Booker;
                mOrder.DOCUMENT_DATE = p_DocumentDate;
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

        #endregion

        #region Added By Hazrat Ali

        /// <summary>
        /// Checks Manual Order No And Manual Invoice No
        /// </summary>
        /// <param name="p_Distributor_Id">Location</param>
        /// <param name="p_MANUAL_ORDER_ID">ManualOrder</param>
        /// <param name="p_TYPE">Type</param>
        /// <returns>Manual Order No And Manual Invoice No as Datatable</returns>
        public DataTable SelectBillBookNo(int p_Distributor_Id, string p_MANUAL_ORDER_ID, int p_TYPE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspIsBillBookNoExist mBillBookNo = new uspIsBillBookNoExist();
                mBillBookNo.Connection = mConnection;
                mBillBookNo.DISTRIBUTOR_ID = p_Distributor_Id;
                mBillBookNo.MANUAL_ID = p_MANUAL_ORDER_ID;
                mBillBookNo.TYPE = p_TYPE;
                DataTable dt = mBillBookNo.ExecuteTable();
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

        public DataTable GetPromotion(int p_Distributor_Id, DateTime p_Working_Date)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetPromotionDetail mPromotionDetail = new uspGetPromotionDetail();
                mPromotionDetail.Connection = mConnection;
                mPromotionDetail.DISTRIBUTOR_ID = p_Distributor_Id;
                mPromotionDetail.WORKING_DATE = p_Working_Date;
                DataTable dt = mPromotionDetail.ExecuteTable();
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

        #endregion

        #region Insert, Update, Deleted

        /// <summary>
        /// Inserts Order
        /// </summary>
        /// <param name="p_Distributor_id">Location</param>
        /// <param name="p_MANUAL_ORDER_ID">ManualOrder</param>
        /// <param name="p_TOWN_ID">Town</param>
        /// <param name="p_AREA_ID">Route</param>
        /// <param name="p_PRINCIPAL_ID">Principal</param>
        /// <param name="p_SOLD_TO">Customer</param>
        /// <param name="p_SHIP_TO">ShipTo</param>
        /// <param name="p_ORDERBOOKER_ID">OrderBooker</param>
        /// <param name="p_DELIVERYMAN_ID">Deliveryman</param>
        /// <param name="p_OrderTypeId">Type</param>
        /// <param name="p_TOTAL_AMOUNT">Amount</param>
        /// <param name="p_EXTRA_DISCOUNT_AMOUNT">ExtraDiscount</param>
        /// <param name="p_STANDARD_DISCOUNT_AMOUNT">StandardDiscount</param>
        /// <param name="p_GST_AMOUNT">GST</param>
        /// <param name="p_TOTAL_NET_AMOUNT">NetAmount</param>
        /// <param name="p_SCHEME_AMOUNT">SchemeAmount</param>
        /// <param name="p_STATUS_ID">Status</param>
        /// <param name="dtOrderDetail">OrderDetailDatatable</param>
        /// <param name="dtFreeSKU">FreeSKUDatatable</param>
        /// <param name="p_UserId">InsertedBy</param>
        /// <param name="p_Document_Date">Date</param>
        /// <param name="p_SEDAmount">SEDAmount</param>
        /// <param name="p_TSTAmount">TSTAmount</param>
        /// <returns>True On Success And False On Failure</returns>
        public bool Add_Order(int p_Distributor_id, string p_MANUAL_ORDER_ID, int p_TOWN_ID, long p_AREA_ID, int p_PRINCIPAL_ID, long p_SOLD_TO, long p_SHIP_TO, int p_ORDERBOOKER_ID, int p_DELIVERYMAN_ID, int p_OrderTypeId,
            decimal p_TOTAL_AMOUNT, decimal p_EXTRA_DISCOUNT_AMOUNT, decimal p_STANDARD_DISCOUNT_AMOUNT, decimal p_GST_AMOUNT, decimal p_TOTAL_NET_AMOUNT, decimal p_SCHEME_AMOUNT, int p_STATUS_ID, DataTable dtOrderDetail, int p_UserId, DateTime p_Document_Date, decimal p_SEDAmount, decimal p_TSTAmount)
        {

            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            decimal TotalAmt = 0, DiscountAmount = 0, ExtraDiscount = 0, GSTAmount = 0, TotalNetAmt = 0;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);

                spInsertSALE_ORDER_MASTER mISom = new spInsertSALE_ORDER_MASTER();
                mISom.Connection = mConnection;
                mISom.Transaction = mTransaction;

                //------------Insert into Sale Order Master----------

                if (dtOrderDetail.Rows.Count > 0)
                {
                    mISom.DISTRIBUTOR_ID = p_Distributor_id;
                    mISom.MANUAL_ORDER_ID = p_MANUAL_ORDER_ID;
                    mISom.PRINCIPAL_ID = p_PRINCIPAL_ID;
                    mISom.AREA_ID = p_AREA_ID;
                    mISom.DELIVERYMAN_ID = p_DELIVERYMAN_ID;
                    mISom.ORDERBOOKER_ID = p_ORDERBOOKER_ID;
                    mISom.DOCUMENT_DATE = p_Document_Date;
                    mISom.SHIP_TO = p_SHIP_TO;
                    mISom.SOLD_TO = p_SOLD_TO;
                    mISom.TOTAL_AMOUNT = p_TOTAL_AMOUNT;
                    mISom.EXTRA_DISCOUNT_AMOUNT = p_EXTRA_DISCOUNT_AMOUNT;
                    mISom.STANDARD_DISCOUNT_AMOUNT = p_STANDARD_DISCOUNT_AMOUNT;
                    mISom.GST_AMOUNT = p_GST_AMOUNT;
                    mISom.SCHEME_AMOUNT = p_SCHEME_AMOUNT;
                    mISom.TOTAL_NET_AMOUNT = p_TOTAL_NET_AMOUNT;
                    mISom.TOWN_ID = p_TOWN_ID;
                    mISom.STATUS_ID = p_STATUS_ID;
                    mISom.USER_ID = p_UserId;
                    mISom.TST_AMOUNT = p_TSTAmount;
                    mISom.SED_AMOUNT = p_SEDAmount;
                    mISom.ORDER_TYPE_ID = p_OrderTypeId;
                    mISom.TIME_STAMP = DateTime.Now;
                    mISom.LASTUPDATE_DATE = System.DateTime.Now;
                    mISom.ExecuteQuery();


                    //----------------Insert into sale order detail-------------
                    spInsertSALE_ORDER_DETAIL mSaleOrderDetail = new spInsertSALE_ORDER_DETAIL();
                    mSaleOrderDetail.Connection = mConnection;
                    mSaleOrderDetail.Transaction = mTransaction;

                    foreach (DataRow dr in dtOrderDetail.Rows)
                    {
                        //SaleOrderDetail_Collection mSod_Col=new SaleOrderDetail_Collection ();
                        mSaleOrderDetail.SALE_ORDER_ID = mISom.SALE_ORDER_ID;
                        mSaleOrderDetail.DISTRIBUTOR_ID = p_Distributor_id;
                        mSaleOrderDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                        mSaleOrderDetail.BATCH_NO = dr["BATCH_NO"].ToString();
                        mSaleOrderDetail.QUANTITY_UNIT = int.Parse(dr["QUANTITY_UNIT"].ToString());
                        mSaleOrderDetail.UNIT_PRICE = decimal.Parse(dr["UNIT_PRICE"].ToString());
                        mSaleOrderDetail.GST_RATE = float.Parse(dr["GST_RATE"].ToString());
                        mSaleOrderDetail.AMOUNT = decimal.Parse(dr["AMOUNT"].ToString());
                        mSaleOrderDetail.EXTRA_DISCOUNT = 0;//decimal.Parse(dr["EXTRA_DISCOUNT"].ToString());
                        mSaleOrderDetail.STANDARD_DISCOUNT = decimal.Parse(dr["STANDARD_DISCOUNT"].ToString());
                        mSaleOrderDetail.GST_AMOUNT = decimal.Parse(dr["GST_AMOUNT"].ToString());
                        mSaleOrderDetail.TST_AMOUNT = decimal.Parse(dr["TST_AMOUNT"].ToString());

                        mSaleOrderDetail.NET_AMOUNT = decimal.Parse(dr["NET_AMOUNT"].ToString());
                        mSaleOrderDetail.IS_DELETED = false;
                        mSaleOrderDetail.TIME_STAMP = p_Document_Date;
                        TotalAmt += decimal.Parse(dr["AMOUNT"].ToString());

                        GSTAmount += decimal.Parse(dr["GST_AMOUNT"].ToString());
                        TotalNetAmt += decimal.Parse(dr["NET_AMOUNT"].ToString());
                        mSaleOrderDetail.ExecuteQuery();



                    }


                    mTransaction.Commit();
                    return true;
                }
            }
            catch (Exception exp)
            {
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

        public bool Add_Order(int p_Distributor_id, string p_MANUAL_ORDER_ID, int p_TOWN_ID, long p_AREA_ID, int p_PRINCIPAL_ID, long p_SOLD_TO, long p_SHIP_TO, int p_ORDERBOOKER_ID, int p_DELIVERYMAN_ID, int p_OrderTypeId,
            decimal p_TOTAL_AMOUNT, decimal p_EXTRA_DISCOUNT_AMOUNT, decimal p_STANDARD_DISCOUNT_AMOUNT, decimal p_GST_AMOUNT, decimal p_TOTAL_NET_AMOUNT, decimal p_SCHEME_AMOUNT, int p_STATUS_ID, DataTable dtOrderDetail, DataTable dtFreeSKU, int p_UserId, DateTime p_Document_Date, decimal p_SEDAmount, decimal p_TSTAmount)
        {

            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            decimal TotalAmt = 0, DiscountAmount = 0, ExtraDiscount = 0, GSTAmount = 0, TotalNetAmt = 0;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);

                spInsertSALE_ORDER_MASTER mISom = new spInsertSALE_ORDER_MASTER();
                mISom.Connection = mConnection;
                mISom.Transaction = mTransaction;

                //------------Insert into Sale Order Master----------

                if (dtOrderDetail.Rows.Count > 0)
                {
                    mISom.DISTRIBUTOR_ID = p_Distributor_id;
                    mISom.MANUAL_ORDER_ID = p_MANUAL_ORDER_ID;
                    mISom.PRINCIPAL_ID = p_PRINCIPAL_ID;
                    mISom.AREA_ID = p_AREA_ID;
                    mISom.DELIVERYMAN_ID = p_DELIVERYMAN_ID;
                    mISom.ORDERBOOKER_ID = p_ORDERBOOKER_ID;
                    mISom.DOCUMENT_DATE = p_Document_Date;
                    mISom.SHIP_TO = p_SHIP_TO;
                    mISom.SOLD_TO = p_SOLD_TO;
                    mISom.TOTAL_AMOUNT = p_TOTAL_AMOUNT;
                    mISom.EXTRA_DISCOUNT_AMOUNT = p_EXTRA_DISCOUNT_AMOUNT;
                    mISom.STANDARD_DISCOUNT_AMOUNT = p_STANDARD_DISCOUNT_AMOUNT;
                    mISom.GST_AMOUNT = p_GST_AMOUNT;
                    mISom.SCHEME_AMOUNT = p_SCHEME_AMOUNT;
                    mISom.TOTAL_NET_AMOUNT = p_TOTAL_NET_AMOUNT;
                    mISom.TOWN_ID = p_TOWN_ID;
                    mISom.STATUS_ID = p_STATUS_ID;
                    mISom.USER_ID = p_UserId;
                    mISom.TST_AMOUNT = p_TSTAmount;
                    mISom.SED_AMOUNT = p_SEDAmount;
                    mISom.ORDER_TYPE_ID = p_OrderTypeId;
                    mISom.TIME_STAMP = DateTime.Now;
                    mISom.LASTUPDATE_DATE = System.DateTime.Now;
                    mISom.ExecuteQuery();


                    //----------------Insert into sale order detail-------------
                    spInsertSALE_ORDER_DETAIL mSaleOrderDetail = new spInsertSALE_ORDER_DETAIL();
                    mSaleOrderDetail.Connection = mConnection;
                    mSaleOrderDetail.Transaction = mTransaction;

                    foreach (DataRow dr in dtOrderDetail.Rows)
                    {
                        //SaleOrderDetail_Collection mSod_Col=new SaleOrderDetail_Collection ();
                        mSaleOrderDetail.SALE_ORDER_ID = mISom.SALE_ORDER_ID;
                        mSaleOrderDetail.DISTRIBUTOR_ID = p_Distributor_id;
                        mSaleOrderDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                        mSaleOrderDetail.BATCH_NO = dr["BATCH_NO"].ToString();
                        mSaleOrderDetail.QUANTITY_UNIT = int.Parse(dr["QUANTITY_UNIT"].ToString());
                        mSaleOrderDetail.UNIT_PRICE = decimal.Parse(dr["UNIT_PRICE"].ToString());
                        mSaleOrderDetail.GST_RATE = float.Parse(dr["GST_RATE"].ToString());
                        mSaleOrderDetail.AMOUNT = decimal.Parse(dr["AMOUNT"].ToString());
                        mSaleOrderDetail.EXTRA_DISCOUNT = 0;//decimal.Parse(dr["EXTRA_DISCOUNT"].ToString());
                        mSaleOrderDetail.STANDARD_DISCOUNT = decimal.Parse(dr["STANDARD_DISCOUNT"].ToString());
                        mSaleOrderDetail.GST_AMOUNT = decimal.Parse(dr["GST_AMOUNT"].ToString());
                        mSaleOrderDetail.TST_AMOUNT = decimal.Parse(dr["TST_AMOUNT"].ToString());

                        mSaleOrderDetail.NET_AMOUNT = decimal.Parse(dr["NET_AMOUNT"].ToString());
                        mSaleOrderDetail.IS_DELETED = false;
                        mSaleOrderDetail.TIME_STAMP = p_Document_Date;
                        TotalAmt += decimal.Parse(dr["AMOUNT"].ToString());

                        GSTAmount += decimal.Parse(dr["GST_AMOUNT"].ToString());
                        TotalNetAmt += decimal.Parse(dr["NET_AMOUNT"].ToString());
                        mSaleOrderDetail.ExecuteQuery();



                    }
                    foreach (DataRow df in dtFreeSKU.Rows)
                    {
                        //----------------Insert into sale order Promotion-------------
                        spInsertSALE_ORDER_PROMOTION mSaleOrderPromo = new spInsertSALE_ORDER_PROMOTION();
                        mSaleOrderPromo.Connection = mConnection;
                        mSaleOrderPromo.Transaction = mTransaction;

                        mSaleOrderPromo.BASKET_DETAIL_ID = int.Parse(df["BASKET_DETAIL_ID"].ToString());
                        mSaleOrderPromo.BASKET_ID = int.Parse(df["BASKET_ID"].ToString());
                        mSaleOrderPromo.DISTRIBUTOR_ID = p_Distributor_id;
                        mSaleOrderPromo.GST_AMOUNT = decimal.Parse(df["GST_AMOUNT"].ToString());
                        mSaleOrderPromo.GST_RATE = float.Parse(df["GST_RATE"].ToString());
                        mSaleOrderPromo.PROMOTION_ID = int.Parse(df["PROMOTION_ID"].ToString());
                        mSaleOrderPromo.PROMOTION_OFFER_ID = int.Parse(df["PROMOTION_OFFER_ID"].ToString());
                        mSaleOrderPromo.QUANTITY = int.Parse(df["Quantity"].ToString());
                        mSaleOrderPromo.SKU_ID = int.Parse(df["SKU_ID"].ToString());
                        mSaleOrderPromo.UNIT_PRICE = decimal.Parse(df["UNIT_PRICE"].ToString());
                        mSaleOrderPromo.SALE_ORDER_ID = mISom.SALE_ORDER_ID;
                        mSaleOrderPromo.AMOUNT = decimal.Parse(df["AMOUNT"].ToString());
                        mSaleOrderPromo.TST_AMOUNT = decimal.Parse(df["TST_AMOUNT"].ToString());
                        mSaleOrderPromo.SED_AMOUNT = 0;
                        mSaleOrderPromo.ExecuteQuery();
                    }

                    mTransaction.Commit();
                    return true;
                }
            }
            catch (Exception exp)
            {
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

        /// <summary>
        /// Inserts Invoice
        /// </summary>
        /// <param name="p_Distributor_id">Location</param>
        /// <param name="p_MANUAL_INVOICE_ID">ManualInvoice</param>
        /// <param name="p_TOWN_ID">Town</param>
        /// <param name="p_AREA_ID">Route</param>
        /// <param name="p_PRINCIPAL_ID">Principal</param>
        /// <param name="p_SOLD_TO">Customer</param>
        /// <param name="p_SHIP_TO">ShipTo</param>
        /// <param name="p_ORDERBOOKER_ID">OrderBooker</param>
        /// <param name="p_DELIVERYMAN_ID">Deliveryman</param>
        /// <param name="p_Orderid">Order</param>
        /// <param name="p_TOTAL_AMOUNT">Amount</param>
        /// <param name="p_EXTRA_DISCOUNT_AMOUNT">ExtraDiscount</param>
        /// <param name="p_STANDARD_DISCOUNT_AMOUNT">Discount</param>
        /// <param name="p_GST_AMOUNT">GST</param>
        /// <param name="p_TOTAL_NET_AMOUNT">NetAmount</param>
        /// <param name="p_SCHEME_AMOUNT">SchemeAmount</param>
        /// <param name="InvoiceTypeId">Type</param>
        /// <param name="dtOrderDetail">OrderDetailDatatable</param>
        /// <param name="dtFreeSKU">FreeSKUDatatable</param>
        /// <param name="p_UserId">InsertedBy</param>
        /// <param name="p_CashReceived">Cash</param>
        /// <param name="p_DocumentDate">Date</param>
        /// <param name="p_TSTAmount">TSTAmount</param>
        /// <param name="p_SEDAmount">SEDAmount</param>
        /// <returns>True On Success And False On Failure</returns>
        public bool Add_Invoice(int p_Distributor_id, string p_MANUAL_INVOICE_ID, int p_TOWN_ID, long p_AREA_ID, int p_PRINCIPAL_ID, long p_SOLD_TO, long p_SHIP_TO, int p_ORDERBOOKER_ID, int p_DELIVERYMAN_ID, long p_Orderid,
         decimal p_TOTAL_AMOUNT, decimal p_EXTRA_DISCOUNT_AMOUNT, decimal p_STANDARD_DISCOUNT_AMOUNT, decimal p_GST_AMOUNT, decimal p_TOTAL_NET_AMOUNT, decimal p_SCHEME_AMOUNT, int InvoiceTypeId, DataTable dtOrderDetail, DataTable dtFreeSKU, int p_UserId, decimal p_CashReceived, DateTime p_DocumentDate, decimal p_TSTAmount, decimal p_SEDAmount)
        {

            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            decimal TotalAmt = 0, DiscountAmount = 0, ExtraDiscount = 0, GSTAmount = 0, TotalNetAmt = 0;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);

                spInsertSALE_INVOICE_MASTER mISom = new spInsertSALE_INVOICE_MASTER();
                mISom.Connection = mConnection;
                mISom.Transaction = mTransaction;

                //------------Insert into Sale Invoice Master----------

                if (dtOrderDetail.Rows.Count > 0)
                {
                    mISom.DISTRIBUTOR_ID = p_Distributor_id;
                    mISom.MANUAL_INVOICE_ID = p_MANUAL_INVOICE_ID;
                    mISom.PRINCIPAL_ID = p_PRINCIPAL_ID;
                    mISom.AREA_ID = p_AREA_ID;
                    mISom.DELIVERYMAN_ID = p_DELIVERYMAN_ID;
                    mISom.ORDERBOOKER_ID = p_ORDERBOOKER_ID;
                    mISom.DOCUMENT_DATE = p_DocumentDate;
                    mISom.SOLD_TO = p_SOLD_TO;
                    mISom.TOTAL_AMOUNT = p_TOTAL_AMOUNT;
                    mISom.EXTRA_DISCOUNT_AMOUNT = p_EXTRA_DISCOUNT_AMOUNT;
                    mISom.STANDARD_DISCOUNT_AMOUNT = p_STANDARD_DISCOUNT_AMOUNT;
                    mISom.GST_AMOUNT = p_GST_AMOUNT;
                    mISom.SCHEME_AMOUNT = p_SCHEME_AMOUNT;
                    mISom.IS_DELETED = false;
                    mISom.TOTAL_NET_AMOUNT = p_TOTAL_NET_AMOUNT;
                    if (InvoiceTypeId == Constants.Credit_Order_Id)
                    {
                        mISom.CREDIT_AMOUNT = p_TOTAL_NET_AMOUNT - p_CashReceived;
                        mISom.CURRENT_CREDIT_AMOUNT = p_TOTAL_NET_AMOUNT - p_CashReceived;
                    }
                    else
                    {
                        mISom.CREDIT_AMOUNT = 0;
                        mISom.CURRENT_CREDIT_AMOUNT = 0;
                    }
                    mISom.TOWN_ID = p_TOWN_ID;
                    mISom.USER_ID = p_UserId;
                    mISom.SALE_ORDER_ID = p_Orderid;
                    mISom.TST_AMOUNT = p_TSTAmount;
                    mISom.SED_AMOUNT = p_SEDAmount;
                    mISom.TIME_STAMP = DateTime.Now;
                    mISom.LASTUPDATE_DATE = System.DateTime.Now;
                    mISom.IS_DELETED = false;
                    mISom.POSTING = 0;
                    mISom.ExecuteQuery();

                    //------------------Ledger Posting--------------------------\\



                    //----------------Insert into sale order detail-------------
                    spInsertSALE_INVOICE_DETAIL mSaleOrderDetail = new spInsertSALE_INVOICE_DETAIL();
                    mSaleOrderDetail.Connection = mConnection;
                    mSaleOrderDetail.Transaction = mTransaction;

                    foreach (DataRow dr in dtOrderDetail.Rows)
                    {
                        //SaleOrderDetail_Collection mSod_Col=new SaleOrderDetail_Collection ();
                        mSaleOrderDetail.SALE_INVOICE_ID = mISom.SALE_INVOICE_ID;
                        mSaleOrderDetail.DISTRIBUTOR_ID = p_Distributor_id;
                        mSaleOrderDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                        mSaleOrderDetail.BATCH_NO = dr["BATCH_NO"].ToString();
                        mSaleOrderDetail.QUANTITY_UNIT = int.Parse(dr["QUANTITY_UNIT"].ToString());
                        mSaleOrderDetail.UNIT_PRICE = decimal.Parse(dr["UNIT_PRICE"].ToString());
                        mSaleOrderDetail.GST_RATE = float.Parse(dr["GST_RATE"].ToString());
                        mSaleOrderDetail.AMOUNT = decimal.Parse(dr["AMOUNT"].ToString());
                        mSaleOrderDetail.GST_AMOUNT = decimal.Parse(dr["GST_AMOUNT"].ToString());
                        mSaleOrderDetail.TST_AMOUNT = decimal.Parse(dr["TST_AMOUNT"].ToString());
                        mSaleOrderDetail.NET_AMOUNT = decimal.Parse(dr["NET_AMOUNT"].ToString());
                        mSaleOrderDetail.IS_DELETED = false;
                        mSaleOrderDetail.TIME_STAMP = p_DocumentDate;
                        TotalAmt += decimal.Parse(dr["AMOUNT"].ToString());
                        GSTAmount += decimal.Parse(dr["GST_AMOUNT"].ToString());
                        TotalNetAmt += decimal.Parse(dr["NET_AMOUNT"].ToString());
                        mSaleOrderDetail.ExecuteQuery();

                        UspProcessStockRegister mStockUpdate = new UspProcessStockRegister();
                        mStockUpdate.Connection = mConnection;
                        mStockUpdate.Transaction = mTransaction;
                        if (mSaleOrderDetail.QUANTITY_UNIT < 0)
                        {
                            mStockUpdate.TYPE_ID = Constants.Document_Sale_Return;
                            mStockUpdate.STOCK_QTY = mSaleOrderDetail.QUANTITY_UNIT * (-1);
                        }
                        else
                        {
                            mStockUpdate.TYPE_ID = Constants.Document_Invoice;
                            mStockUpdate.STOCK_QTY = mSaleOrderDetail.QUANTITY_UNIT;
                        }
                        //mStockUpdate.TYPE_ID = Constants.Document_Invoice;
                        mStockUpdate.DISTRIBUTOR_ID = p_Distributor_id;
                        mStockUpdate.STOCK_DATE = p_DocumentDate;
                        mStockUpdate.SKU_ID = mSaleOrderDetail.SKU_ID;
                        mStockUpdate.BATCHNO = mSaleOrderDetail.BATCH_NO;
                        mStockUpdate.FREE_QTY = 0;
                        mStockUpdate.ExecuteQuery();



                    }
                    foreach (DataRow df in dtFreeSKU.Rows)
                    {
                        //----------------Insert into sale order Promotion-------------
                        spInsertSALE_INVOICE_PROMOTION mSaleOrderPromo = new spInsertSALE_INVOICE_PROMOTION();
                        mSaleOrderPromo.Connection = mConnection;
                        mSaleOrderPromo.Transaction = mTransaction;

                        mSaleOrderPromo.BASKET_DETAIL_ID = int.Parse(df["BASKET_DETAIL_ID"].ToString());
                        mSaleOrderPromo.BASKET_ID = int.Parse(df["BASKET_ID"].ToString());
                        mSaleOrderPromo.DISTRIBUTOR_ID = p_Distributor_id;
                        mSaleOrderPromo.GST_AMOUNT = decimal.Parse(df["GST_AMOUNT"].ToString());
                        mSaleOrderPromo.GST_RATE = float.Parse(df["GST_RATE"].ToString());
                        mSaleOrderPromo.PROMOTION_ID = int.Parse(df["PROMOTION_ID"].ToString());
                        mSaleOrderPromo.PROMOTION_OFFER_ID = int.Parse(df["PROMOTION_OFFER_ID"].ToString());
                        mSaleOrderPromo.QUANTITY = int.Parse(df["Quantity"].ToString());
                        mSaleOrderPromo.SKU_ID = int.Parse(df["SKU_ID"].ToString());
                        mSaleOrderPromo.UNIT_PRICE = decimal.Parse(df["UNIT_PRICE"].ToString());
                        mSaleOrderPromo.TST_AMOUNT = decimal.Parse(df["TST_AMOUNT"].ToString());
                        mSaleOrderPromo.SED_AMOUNT = 0;
                        mSaleOrderPromo.SALE_INVOICE_ID = mISom.SALE_INVOICE_ID;
                        mSaleOrderPromo.AMOUNT = decimal.Parse(df["AMOUNT"].ToString());
                        mSaleOrderPromo.ExecuteQuery();

                        UspProcessStockRegister mStockUpdate = new UspProcessStockRegister();
                        mStockUpdate.Connection = mConnection;
                        mStockUpdate.Transaction = mTransaction;

                        mStockUpdate.DISTRIBUTOR_ID = p_Distributor_id;
                        mStockUpdate.FREE_QTY = mSaleOrderPromo.QUANTITY;
                        mStockUpdate.STOCK_DATE = p_DocumentDate;
                        mStockUpdate.SKU_ID = mSaleOrderPromo.SKU_ID;
                        mStockUpdate.BATCHNO = "N/A";
                        mStockUpdate.STOCK_QTY = 0;

                        mStockUpdate.ExecuteQuery();
                    }

                    #region Account Posting
                    LedgerController LController = new LedgerController();
                    Configuration.GetAccountHead();
                    DistributorController Dcontroller = new DistributorController();


                    string VoucherNo = LController.SelectLedgerMaxDocumentId(Constants.Journal_Voucher, p_Distributor_id, 0);

                    if (InvoiceTypeId == Constants.Advance_PaymentOrder_id)
                    {
                        LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.SaleAccount), p_Distributor_id, 0, p_TOTAL_AMOUNT, mISom.DOCUMENT_DATE, "Gross Sale Value", DateTime.Now, p_PRINCIPAL_ID, int.Parse(p_SOLD_TO.ToString()), mISom.SALE_INVOICE_ID, mISom.MANUAL_INVOICE_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.Cash_Advance, p_DELIVERYMAN_ID.ToString());

                        if (p_STANDARD_DISCOUNT_AMOUNT > 0)
                        {
                            LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.SaleDiscount), p_Distributor_id, p_STANDARD_DISCOUNT_AMOUNT, 0, mISom.DOCUMENT_DATE, "Commision/Discount", DateTime.Now, p_PRINCIPAL_ID, int.Parse(p_SOLD_TO.ToString()), mISom.SALE_INVOICE_ID, mISom.MANUAL_INVOICE_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.Cash_Advance, p_DELIVERYMAN_ID.ToString());
                        }
                        if (p_EXTRA_DISCOUNT_AMOUNT > 0)
                        {
                            LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.SaleScheme), p_Distributor_id, p_EXTRA_DISCOUNT_AMOUNT, 0, mISom.DOCUMENT_DATE, "Extra Discount", DateTime.Now, p_PRINCIPAL_ID, int.Parse(p_SOLD_TO.ToString()), mISom.SALE_INVOICE_ID, mISom.MANUAL_INVOICE_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.Cash_Advance, p_DELIVERYMAN_ID.ToString());
                        }
                        if (p_GST_AMOUNT + p_TSTAmount > 0)
                        {
                            LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.GSTAccount), p_Distributor_id, 0, p_GST_AMOUNT + p_TSTAmount, mISom.DOCUMENT_DATE, "Sales Tax", DateTime.Now, p_PRINCIPAL_ID, int.Parse(p_SOLD_TO.ToString()), mISom.SALE_INVOICE_ID, mISom.MANUAL_INVOICE_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.Cash_Advance, p_DELIVERYMAN_ID.ToString());
                        }
                        LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.AccountReceivable), p_Distributor_id, p_TOTAL_NET_AMOUNT, 0, mISom.DOCUMENT_DATE, "Credit Sale Default", DateTime.Now, p_PRINCIPAL_ID, int.Parse(p_SOLD_TO.ToString()), mISom.SALE_INVOICE_ID, mISom.MANUAL_INVOICE_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.Cash_Advance, p_DELIVERYMAN_ID.ToString());

                    }
                    else if (InvoiceTypeId == Constants.Credit_Order_Id)
                    {
                        LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.AccountReceivable), p_Distributor_id, p_TOTAL_NET_AMOUNT - p_CashReceived, 0, mISom.DOCUMENT_DATE, "Credit Sale Default", DateTime.Now, p_PRINCIPAL_ID, int.Parse(p_SOLD_TO.ToString()), mISom.SALE_INVOICE_ID, mISom.MANUAL_INVOICE_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.CreditSale, p_DELIVERYMAN_ID.ToString());
                        LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.SaleAccount), p_Distributor_id, 0, p_TOTAL_NET_AMOUNT - p_CashReceived, mISom.DOCUMENT_DATE, "Credit Sale Default", DateTime.Now, p_PRINCIPAL_ID, int.Parse(p_SOLD_TO.ToString()), mISom.SALE_INVOICE_ID, mISom.MANUAL_INVOICE_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.CreditSale, p_DELIVERYMAN_ID.ToString());

                    }
                    #endregion

                    #region Update Pending Order

                    spUpdateSALE_ORDER_MASTER mOrderUpdate = new spUpdateSALE_ORDER_MASTER();
                    mOrderUpdate.Connection = mConnection;
                    mOrderUpdate.Transaction = mTransaction;
                    mOrderUpdate.DISTRIBUTOR_ID = p_Distributor_id;
                    mOrderUpdate.SALE_ORDER_ID = p_Orderid;
                    mOrderUpdate.STATUS_ID = Constants.Order_Posted_Id;
                    mOrderUpdate.ExecuteQuery();

                    #endregion

                    mTransaction.Commit();
                    return true;
                }
            }
            catch (Exception exp)
            {
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

        //  public long Add_Invoice2(int p_Distributor_id, string p_MANUAL_INVOICE_ID, int p_TOWN_ID, long p_AREA_ID, int p_PRINCIPAL_ID, long p_SOLD_TO, long p_SHIP_TO, int p_ORDERBOOKER_ID, int p_DELIVERYMAN_ID, long p_Orderid,
        //decimal p_TOTAL_AMOUNT, decimal p_EXTRA_DISCOUNT_AMOUNT, decimal p_STANDARD_DISCOUNT_AMOUNT, decimal p_GST_AMOUNT, decimal p_TOTAL_NET_AMOUNT, decimal p_SCHEME_AMOUNT, int InvoiceTypeId, DataTable dtOrderDetail, int p_UserId, decimal p_CashReceived, DateTime p_DocumentDate, decimal p_TSTAmount, decimal p_SEDAmount,string p_AuthorisedBy)
        //  {

        //      IDbConnection mConnection = null;
        //      IDbTransaction mTransaction = null;
        //      decimal TotalAmt = 0, GSTAmount = 0, TotalNetAmt = 0;
        //      try
        //      {
        //          mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
        //          mConnection.Open();
        //          mTransaction = ProviderFactory.GetTransaction(mConnection);

        //          spInsertSALE_INVOICE_MASTER2 mISom = new spInsertSALE_INVOICE_MASTER2();
        //          mISom.Connection = mConnection;
        //          mISom.Transaction = mTransaction;

        //          //------------Insert into Sale Invoice Master----------

        //          if (dtOrderDetail.Rows.Count > 0)
        //          {
        //              mISom.DISTRIBUTOR_ID = p_Distributor_id;
        //              mISom.MANUAL_INVOICE_ID = p_MANUAL_INVOICE_ID;
        //              mISom.PRINCIPAL_ID = p_PRINCIPAL_ID;
        //              mISom.AREA_ID = p_AREA_ID;
        //              mISom.DELIVERYMAN_ID = p_DELIVERYMAN_ID;
        //              mISom.ORDERBOOKER_ID = p_ORDERBOOKER_ID;
        //              mISom.DOCUMENT_DATE = p_DocumentDate;
        //              mISom.SOLD_TO = p_SOLD_TO;
        //              mISom.TOTAL_AMOUNT = p_TOTAL_AMOUNT;
        //              mISom.EXTRA_DISCOUNT_AMOUNT = p_EXTRA_DISCOUNT_AMOUNT;
        //              mISom.STANDARD_DISCOUNT_AMOUNT = p_STANDARD_DISCOUNT_AMOUNT;
        //              mISom.GST_AMOUNT = p_GST_AMOUNT;
        //              mISom.SCHEME_AMOUNT = p_SCHEME_AMOUNT;
        //              mISom.IS_DELETED = false;
        //              mISom.TOTAL_NET_AMOUNT = p_TOTAL_NET_AMOUNT;

        //              if (InvoiceTypeId == Constants.Credit_Order_Id)
        //              {
        //                  mISom.CREDIT_AMOUNT = p_TOTAL_NET_AMOUNT - p_CashReceived;
        //                  mISom.CURRENT_CREDIT_AMOUNT = p_TOTAL_NET_AMOUNT - p_CashReceived;
        //              }
        //              else if (InvoiceTypeId == Constants.CashandCredit_Order_Id)
        //              {
        //                  mISom.CREDIT_AMOUNT = p_TOTAL_NET_AMOUNT - p_STANDARD_DISCOUNT_AMOUNT;
        //                  mISom.CURRENT_CREDIT_AMOUNT = p_TOTAL_NET_AMOUNT - p_STANDARD_DISCOUNT_AMOUNT;
        //              }
        //              else
        //              {
        //                  mISom.CREDIT_AMOUNT = 0;
        //                  mISom.CURRENT_CREDIT_AMOUNT = 0;
        //              }
        //              mISom.TOWN_ID = p_TOWN_ID;
        //              mISom.USER_ID = p_UserId;
        //              mISom.SALE_ORDER_ID = p_Orderid;
        //              mISom.TST_AMOUNT = p_TSTAmount;
        //              mISom.SED_AMOUNT = p_SEDAmount;
        //              mISom.TIME_STAMP = DateTime.Now;
        //              mISom.LASTUPDATE_DATE = System.DateTime.Now;
        //              mISom.AUTHORISED_PERSON = p_AuthorisedBy;
        //              mISom.IS_DELETED = false;
        //              mISom.POSTING = 0;
        //              mISom.ExecuteQuery();

        //              //------------------Ledger Posting--------------------------\\



        //              //----------------Insert into sale order detail-------------
        //              spInsertSALE_INVOICE_DETAIL mSaleOrderDetail = new spInsertSALE_INVOICE_DETAIL();
        //              mSaleOrderDetail.Connection = mConnection;
        //              mSaleOrderDetail.Transaction = mTransaction;

        //              foreach (DataRow dr in dtOrderDetail.Rows)
        //              {
        //                  //SaleOrderDetail_Collection mSod_Col=new SaleOrderDetail_Collection ();
        //                  mSaleOrderDetail.SALE_INVOICE_ID = mISom.SALE_INVOICE_ID;
        //                  mSaleOrderDetail.DISTRIBUTOR_ID = p_Distributor_id;
        //                  mSaleOrderDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
        //                  mSaleOrderDetail.BATCH_NO = dr["BATCH_NO"].ToString();
        //                  mSaleOrderDetail.QUANTITY_UNIT = int.Parse(dr["QUANTITY_UNIT"].ToString());
        //                  mSaleOrderDetail.UNIT_PRICE = decimal.Parse(dr["UNIT_PRICE"].ToString());
        //                  mSaleOrderDetail.GST_RATE = float.Parse(dr["GST_RATE"].ToString());
        //                  mSaleOrderDetail.AMOUNT = decimal.Parse(dr["AMOUNT"].ToString());
        //                  mSaleOrderDetail.EXTRA_DISCOUNT = decimal.Parse(dr["STANDARD_DISCOUNT"].ToString());
        //                  mSaleOrderDetail.GST_AMOUNT = decimal.Parse(dr["GST_AMOUNT"].ToString());
        //                  mSaleOrderDetail.TST_AMOUNT = decimal.Parse(dr["TST_AMOUNT"].ToString());
        //                  mSaleOrderDetail.NET_AMOUNT = decimal.Parse(dr["NET_AMOUNT"].ToString());
        //                  mSaleOrderDetail.IS_DELETED = false;
        //                  mSaleOrderDetail.IS_VOID = true;
        //                  mSaleOrderDetail.TIME_STAMP = p_DocumentDate;
        //                  if ((dr["CHECK_DELETE"].ToString()) == "0")
        //                  {
        //                      TotalAmt += decimal.Parse(dr["AMOUNT"].ToString());
        //                      GSTAmount += decimal.Parse(dr["GST_AMOUNT"].ToString());
        //                      TotalNetAmt += decimal.Parse(dr["NET_AMOUNT"].ToString());
        //                      mSaleOrderDetail.IS_VOID = false;
        //                  }
        //                  mSaleOrderDetail.ExecuteQuery();
        //                  if ((dr["CHECK_DELETE"].ToString()) == "0")
        //                  {
        //                      UspProcessStockRegister mStockUpdate = new UspProcessStockRegister();
        //                      mStockUpdate.Connection = mConnection;
        //                      mStockUpdate.Transaction = mTransaction;
        //                      if (mSaleOrderDetail.QUANTITY_UNIT < 0)
        //                      {
        //                          mStockUpdate.TYPE_ID = Constants.Document_Sale_Return;
        //                          mStockUpdate.STOCK_QTY = mSaleOrderDetail.QUANTITY_UNIT * (-1);
        //                      }
        //                      else
        //                      {
        //                          mStockUpdate.TYPE_ID = Constants.Document_Invoice;
        //                          mStockUpdate.STOCK_QTY = mSaleOrderDetail.QUANTITY_UNIT;
        //                      }
        //                      //mStockUpdate.TYPE_ID = Constants.Document_Invoice;
        //                      mStockUpdate.DISTRIBUTOR_ID = p_Distributor_id;
        //                      mStockUpdate.STOCK_DATE = p_DocumentDate;
        //                      mStockUpdate.SKU_ID = mSaleOrderDetail.SKU_ID;
        //                      mStockUpdate.BATCHNO = mSaleOrderDetail.BATCH_NO;
        //                      mStockUpdate.FREE_QTY = 0;
        //                      mStockUpdate.ExecuteQuery();

        //                  }

        //              }
        //              //foreach (DataRow df in dtFreeSKU.Rows)
        //              //{
        //              //    //----------------Insert into sale order Promotion-------------
        //              //    spInsertSALE_INVOICE_PROMOTION mSaleOrderPromo = new spInsertSALE_INVOICE_PROMOTION();
        //              //    mSaleOrderPromo.Connection = mConnection;
        //              //    mSaleOrderPromo.Transaction = mTransaction;

        //              //    mSaleOrderPromo.BASKET_DETAIL_ID = int.Parse(df["BASKET_DETAIL_ID"].ToString());
        //              //    mSaleOrderPromo.BASKET_ID = int.Parse(df["BASKET_ID"].ToString());
        //              //    mSaleOrderPromo.DISTRIBUTOR_ID = p_Distributor_id;
        //              //    mSaleOrderPromo.GST_AMOUNT = decimal.Parse(df["GST_AMOUNT"].ToString());
        //              //    mSaleOrderPromo.GST_RATE = float.Parse(df["GST_RATE"].ToString());
        //              //    mSaleOrderPromo.PROMOTION_ID = int.Parse(df["PROMOTION_ID"].ToString());
        //              //    mSaleOrderPromo.PROMOTION_OFFER_ID = int.Parse(df["PROMOTION_OFFER_ID"].ToString());
        //              //    mSaleOrderPromo.QUANTITY = int.Parse(df["Quantity"].ToString());
        //              //    mSaleOrderPromo.SKU_ID = int.Parse(df["SKU_ID"].ToString());
        //              //    mSaleOrderPromo.UNIT_PRICE = decimal.Parse(df["UNIT_PRICE"].ToString());
        //              //    mSaleOrderPromo.TST_AMOUNT = decimal.Parse(df["TST_AMOUNT"].ToString());
        //              //    mSaleOrderPromo.SED_AMOUNT = 0;
        //              //    mSaleOrderPromo.SALE_INVOICE_ID = mISom.SALE_INVOICE_ID;
        //              //    mSaleOrderPromo.AMOUNT = decimal.Parse(df["AMOUNT"].ToString());

        //              //    if ((df["CHECK_DELETE"].ToString()) == "0")
        //              //    {
        //              //        TotalAmt += decimal.Parse(df["AMOUNT"].ToString());
        //              //        GSTAmount += decimal.Parse(df["GST_AMOUNT"].ToString());
        //              //        mSaleOrderDetail.IS_VOID = false;
        //              //    }

        //              //    mSaleOrderPromo.ExecuteQuery();

        //              //    if ((df["CHECK_DELETE"].ToString()) == "0")
        //              //    {
        //              //        UspProcessStockRegister mStockUpdate = new UspProcessStockRegister();
        //              //        mStockUpdate.Connection = mConnection;
        //              //        mStockUpdate.Transaction = mTransaction;

        //              //        if (mSaleOrderDetail.QUANTITY_UNIT < 0)
        //              //        {
        //              //            mStockUpdate.TYPE_ID = Constants.Document_Sale_Return;
        //              //            mStockUpdate.STOCK_QTY = mSaleOrderDetail.QUANTITY_UNIT * (-1);
        //              //        }
        //              //        else
        //              //        {
        //              //            mStockUpdate.TYPE_ID = Constants.Document_Invoice;
        //              //            mStockUpdate.STOCK_QTY = mSaleOrderDetail.QUANTITY_UNIT;
        //              //        }

        //              //        mStockUpdate.DISTRIBUTOR_ID = p_Distributor_id;
        //              //        mStockUpdate.TYPE_ID = Constants.Document_Invoice;
        //              //        mStockUpdate.FREE_QTY = 0;//mSaleOrderPromo.QUANTITY;
        //              //        mStockUpdate.STOCK_DATE = p_DocumentDate;
        //              //        mStockUpdate.SKU_ID = mSaleOrderPromo.SKU_ID;
        //              //        mStockUpdate.BATCHNO = "N/A";
        //              //        mStockUpdate.STOCK_QTY = 0;

        //              //        mStockUpdate.ExecuteQuery();
        //              //    }
        //             // }

        //              #region Account Posting
        //              LedgerController LController = new LedgerController();
        //              Configuration.GetAccountHead();
        //              DistributorController Dcontroller = new DistributorController();


        //              string VoucherNo = LController.SelectLedgerMaxDocumentId(Constants.Journal_Voucher, p_Distributor_id);

        //              if (InvoiceTypeId == Constants.Advance_PaymentOrder_id)
        //              {
        //                  LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.SaleAccount), p_Distributor_id, 0, p_TOTAL_AMOUNT, mISom.DOCUMENT_DATE, "Gross Sale Value", DateTime.Now, p_PRINCIPAL_ID, int.Parse(p_SOLD_TO.ToString()), mISom.SALE_INVOICE_ID, mISom.MANUAL_INVOICE_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.Cash_Advance, p_DELIVERYMAN_ID.ToString());

        //                  if (p_STANDARD_DISCOUNT_AMOUNT > 0)
        //                  {
        //                      LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.SaleDiscount), p_Distributor_id, p_STANDARD_DISCOUNT_AMOUNT, 0, mISom.DOCUMENT_DATE, "Commision/Discount", DateTime.Now, p_PRINCIPAL_ID, int.Parse(p_SOLD_TO.ToString()), mISom.SALE_INVOICE_ID, mISom.MANUAL_INVOICE_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.Cash_Advance, p_DELIVERYMAN_ID.ToString());
        //                  }
        //                  if (p_EXTRA_DISCOUNT_AMOUNT > 0)
        //                  {
        //                      LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.SaleScheme), p_Distributor_id, p_EXTRA_DISCOUNT_AMOUNT, 0, mISom.DOCUMENT_DATE, "Extra Discount", DateTime.Now, p_PRINCIPAL_ID, int.Parse(p_SOLD_TO.ToString()), mISom.SALE_INVOICE_ID, mISom.MANUAL_INVOICE_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.Cash_Advance, p_DELIVERYMAN_ID.ToString());
        //                  }
        //                  if (p_GST_AMOUNT + p_TSTAmount > 0)
        //                  {
        //                      LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.GSTAccount), p_Distributor_id, 0, p_GST_AMOUNT + p_TSTAmount, mISom.DOCUMENT_DATE, "Sales Tax", DateTime.Now, p_PRINCIPAL_ID, int.Parse(p_SOLD_TO.ToString()), mISom.SALE_INVOICE_ID, mISom.MANUAL_INVOICE_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.Cash_Advance, p_DELIVERYMAN_ID.ToString());
        //                  }
        //                  LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.AccountReceivable), p_Distributor_id, p_TOTAL_NET_AMOUNT, 0, mISom.DOCUMENT_DATE, "Credit Sale Default", DateTime.Now, p_PRINCIPAL_ID, int.Parse(p_SOLD_TO.ToString()), mISom.SALE_INVOICE_ID, mISom.MANUAL_INVOICE_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.Cash_Advance, p_DELIVERYMAN_ID.ToString());

        //              }
        //              else if (InvoiceTypeId == Constants.Credit_Order_Id)
        //              {
        //                  LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.AccountReceivable), p_Distributor_id, p_TOTAL_NET_AMOUNT - p_CashReceived, 0, mISom.DOCUMENT_DATE, "Credit Sale Default", DateTime.Now, p_PRINCIPAL_ID, int.Parse(p_SOLD_TO.ToString()), mISom.SALE_INVOICE_ID, mISom.MANUAL_INVOICE_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.CreditSale, p_DELIVERYMAN_ID.ToString());
        //                  LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.SaleAccount), p_Distributor_id, 0, p_TOTAL_NET_AMOUNT - p_CashReceived, mISom.DOCUMENT_DATE, "Credit Sale Default", DateTime.Now, p_PRINCIPAL_ID, int.Parse(p_SOLD_TO.ToString()), mISom.SALE_INVOICE_ID, mISom.MANUAL_INVOICE_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.CreditSale, p_DELIVERYMAN_ID.ToString());

        //              }
        //              else if (InvoiceTypeId == Constants.CashandCredit_Order_Id)
        //              {
        //                  LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.AccountReceivable), p_Distributor_id, p_TOTAL_NET_AMOUNT - p_STANDARD_DISCOUNT_AMOUNT, 0, mISom.DOCUMENT_DATE, "Cash and Credit Sale Default", DateTime.Now, p_PRINCIPAL_ID, int.Parse(p_SOLD_TO.ToString()), mISom.SALE_INVOICE_ID, mISom.MANUAL_INVOICE_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.CreditSale, p_DELIVERYMAN_ID.ToString());
        //                  LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.SaleAccount), p_Distributor_id, 0, p_TOTAL_NET_AMOUNT - p_STANDARD_DISCOUNT_AMOUNT, mISom.DOCUMENT_DATE, "Cash and Credit Sale Default", DateTime.Now, p_PRINCIPAL_ID, int.Parse(p_SOLD_TO.ToString()), mISom.SALE_INVOICE_ID, mISom.MANUAL_INVOICE_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.CreditSale, p_DELIVERYMAN_ID.ToString());

        //              }
        //              #endregion

        //              #region Update Pending Order

        //              //spUpdateSALE_ORDER_MASTER mOrderUpdate = new spUpdateSALE_ORDER_MASTER();
        //              //mOrderUpdate.Connection = mConnection;
        //              //mOrderUpdate.Transaction = mTransaction;
        //              //mOrderUpdate.DISTRIBUTOR_ID = p_Distributor_id;
        //              //mOrderUpdate.SALE_ORDER_ID = p_Orderid;
        //              //mOrderUpdate.STATUS_ID = Constants.Order_Posted_Id;
        //              //mOrderUpdate.ExecuteQuery();

        //              #endregion

        //              mTransaction.Commit();

        //              return mISom.SALE_INVOICE_ID;
        //          }
        //      }
        //      catch (Exception exp)
        //      {
        //          ExceptionPublisher.PublishException(exp);
        //          return -2;// exp.Message;
        //      }
        //      finally
        //      {
        //          if (mConnection != null && mConnection.State == ConnectionState.Open)
        //          {
        //              mConnection.Close();
        //          }
        //      }
        //      return -1;
        //  }

        public static long Add_Invoice2(int p_Distributor_id, string p_MANUAL_INVOICE_ID, int p_PRINCIPAL_ID, long p_SOLD_TO, long p_SHIP_TO, int p_ORDERBOOKER_ID, int p_DELIVERYMAN_ID, long p_Orderid, decimal p_TOTAL_AMOUNT, decimal p_EXTRA_DISCOUNT_AMOUNT, decimal p_STANDARD_DISCOUNT_AMOUNT, decimal p_GST_AMOUNT, decimal p_TOTAL_NET_AMOUNT, decimal p_SCHEME_AMOUNT, int InvoiceTypeId, DataTable dtOrderDetail, int p_UserId, decimal p_CashReceived, DateTime p_DocumentDate, decimal p_TSTAmount, decimal p_SEDAmount, string p_AuthorisedBy, int p_PaymentMode, string p_InvoiceNumberFBR,string InvoiceCalculation)
        {
            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            decimal TotalAmt = 0, GSTAmount = 0, TotalNetAmt = 0;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);

                spInsertSALE_INVOICE_MASTER2 mISom = new spInsertSALE_INVOICE_MASTER2();
                mISom.Connection = mConnection;
                mISom.Transaction = mTransaction;

                //------------Insert into Sale Invoice Master----------
                decimal ReverseAmount = 0;
                decimal ReverseTax = 0;
                if(InvoiceCalculation == "1")
                {
                    foreach(DataRow dr in dtOrderDetail.Rows)
                    {
                        decimal CurrentTax = Convert.ToDecimal(dr["AMOUNT"]) - (Convert.ToDecimal(dr["AMOUNT"]) / (100 + Convert.ToDecimal(dr["GST_RATE"]))) * 100;
                        ReverseTax += CurrentTax;
                        ReverseAmount += Convert.ToDecimal(dr["AMOUNT"]) - CurrentTax;
                    }                    
                }
                if (dtOrderDetail.Rows.Count > 0)
                {
                    mISom.DISTRIBUTOR_ID = p_Distributor_id;
                    mISom.MANUAL_INVOICE_ID = p_MANUAL_INVOICE_ID;
                    mISom.PRINCIPAL_ID = p_PRINCIPAL_ID;
                    mISom.AREA_ID = InvoiceTypeId;
                    mISom.DELIVERYMAN_ID = p_DELIVERYMAN_ID;
                    mISom.ORDERBOOKER_ID = p_ORDERBOOKER_ID;
                    mISom.DOCUMENT_DATE = p_DocumentDate;
                    mISom.SOLD_TO = p_SOLD_TO;
                    if (InvoiceCalculation == "1")
                    {
                        mISom.TOTAL_AMOUNT = ReverseAmount;
                        mISom.GST_AMOUNT = ReverseTax;
                    }
                    else
                    {
                        mISom.TOTAL_AMOUNT = p_TOTAL_AMOUNT;
                        mISom.GST_AMOUNT = p_GST_AMOUNT;
                    }
                    mISom.EXTRA_DISCOUNT_AMOUNT = p_EXTRA_DISCOUNT_AMOUNT;
                    mISom.STANDARD_DISCOUNT_AMOUNT = p_STANDARD_DISCOUNT_AMOUNT;                    
                    mISom.SCHEME_AMOUNT = p_SCHEME_AMOUNT;
                    mISom.IS_DELETED = false;
                    mISom.TOTAL_NET_AMOUNT = p_TOTAL_NET_AMOUNT;
                    if (InvoiceTypeId == Constants.Credit)
                    {
                        mISom.CREDIT_AMOUNT = p_TOTAL_NET_AMOUNT;
                        mISom.CURRENT_CREDIT_AMOUNT = p_TOTAL_NET_AMOUNT;
                    }
                    else if (InvoiceTypeId == Constants.Credit_Order_Id)
                    {
                        mISom.CREDIT_AMOUNT = p_TOTAL_NET_AMOUNT;
                        mISom.CURRENT_CREDIT_AMOUNT = p_TOTAL_NET_AMOUNT;
                    }
                    else if (InvoiceTypeId == 230 || InvoiceTypeId == 231)
                    {
                        mISom.CREDIT_AMOUNT = p_TOTAL_NET_AMOUNT - p_STANDARD_DISCOUNT_AMOUNT;
                        mISom.CURRENT_CREDIT_AMOUNT = p_TOTAL_NET_AMOUNT - p_STANDARD_DISCOUNT_AMOUNT;
                    }
                    else if (InvoiceTypeId == Constants.CashandCredit_Order_Id)
                    {
                        mISom.CREDIT_AMOUNT = p_TOTAL_NET_AMOUNT - p_STANDARD_DISCOUNT_AMOUNT;
                        mISom.CURRENT_CREDIT_AMOUNT = p_TOTAL_NET_AMOUNT - p_STANDARD_DISCOUNT_AMOUNT;
                    }
                    else
                    {
                        mISom.CREDIT_AMOUNT = 0;
                        mISom.CURRENT_CREDIT_AMOUNT = 0;
                    }
                    mISom.TOWN_ID = 0;
                    mISom.USER_ID = p_UserId;
                    mISom.SALE_ORDER_ID = p_Orderid;
                    mISom.TST_AMOUNT = p_TSTAmount;
                    mISom.SED_AMOUNT = p_SEDAmount;
                    mISom.TIME_STAMP = DateTime.Now;
                    mISom.LASTUPDATE_DATE = System.DateTime.Now;
                    mISom.AUTHORISED_PERSON = p_AuthorisedBy;
                    mISom.IS_DELETED = false;
                    mISom.POSTING = 0;
                    mISom.InvoiceNumberFBR = p_InvoiceNumberFBR;
                    mISom.ExecuteQuery();
                    //----------------Insert into sale order detail-------------
                    spInsertSALE_INVOICE_DETAILDecimal mSaleOrderDetail = new spInsertSALE_INVOICE_DETAILDecimal();
                    mSaleOrderDetail.Connection = mConnection;
                    mSaleOrderDetail.Transaction = mTransaction;
                    foreach (DataRow dr in dtOrderDetail.Rows)
                    {
                        mSaleOrderDetail.SALE_INVOICE_ID = mISom.SALE_INVOICE_ID;
                        mSaleOrderDetail.DISTRIBUTOR_ID = p_Distributor_id;
                        mSaleOrderDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                        mSaleOrderDetail.BATCH_NO = dr["BATCH_NO"].ToString();
                        mSaleOrderDetail.QUANTITY_UNIT = decimal.Parse(dr["QUANTITY_UNIT"].ToString());
                        mSaleOrderDetail.UNIT_PRICE = decimal.Parse(dr["UNIT_PRICE"].ToString());
                        mSaleOrderDetail.GST_RATE = float.Parse(dr["GST_RATE"].ToString());
                        mSaleOrderDetail.AMOUNT = decimal.Parse(dr["AMOUNT"].ToString());
                        mSaleOrderDetail.EXTRA_DISCOUNT = decimal.Parse(dr["STANDARD_DISCOUNT"].ToString());
                        mSaleOrderDetail.GST_AMOUNT = decimal.Parse(dr["GST_AMOUNT"].ToString());
                        mSaleOrderDetail.TST_AMOUNT = decimal.Parse(dr["TST_AMOUNT"].ToString());
                        mSaleOrderDetail.NET_AMOUNT = decimal.Parse(dr["NET_AMOUNT"].ToString());
                        mSaleOrderDetail.IS_DELETED = false;
                        mSaleOrderDetail.IS_VOID = true;
                        mSaleOrderDetail.TIME_STAMP = p_DocumentDate;
                        if ((dr["CHECK_DELETE"].ToString()) == "0")
                        {
                            TotalAmt += decimal.Parse(dr["AMOUNT"].ToString());
                            GSTAmount += decimal.Parse(dr["GST_AMOUNT"].ToString());
                            TotalNetAmt += decimal.Parse(dr["NET_AMOUNT"].ToString());
                            mSaleOrderDetail.IS_VOID = false;
                        }
                        mSaleOrderDetail.ExecuteQuery();
                        if ((dr["CHECK_DELETE"].ToString()) == "0")
                        {
                            UspProcessStockRegister mStockUpdate = new UspProcessStockRegister();
                            mStockUpdate.Connection = mConnection;
                            mStockUpdate.Transaction = mTransaction;
                            if (mSaleOrderDetail.QUANTITY_UNIT < 0)
                            {
                                mStockUpdate.TYPE_ID = Constants.Document_Sale_Return;
                                mStockUpdate.STOCK_QTY = mSaleOrderDetail.QUANTITY_UNIT * (-1);
                            }
                            else
                            {
                                mStockUpdate.TYPE_ID = Constants.Document_Invoice;
                                mStockUpdate.STOCK_QTY = mSaleOrderDetail.QUANTITY_UNIT;
                            }
                            mStockUpdate.DISTRIBUTOR_ID = p_Distributor_id;
                            mStockUpdate.STOCK_DATE = p_DocumentDate;
                            mStockUpdate.SKU_ID = mSaleOrderDetail.SKU_ID;
                            mStockUpdate.BATCHNO = mSaleOrderDetail.BATCH_NO;
                            mStockUpdate.FREE_QTY = 0;
                            mStockUpdate.ExecuteQuery();
                        }
                    }

                    #region Account Posting
                    LedgerController LController = new LedgerController();
                    Configuration.GetAccountHead();
                    string VoucherNo = LController.SelectLedgerMaxDocumentId(Constants.Journal_Voucher, p_Distributor_id, 0);
                    if (InvoiceTypeId == Constants.Credit)//Credit
                    {
                        if (p_MANUAL_INVOICE_ID == "2")
                        {
                            LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.AccountReceivable), p_Distributor_id, p_TOTAL_NET_AMOUNT, 0, mISom.DOCUMENT_DATE, "Credit Sale Default", DateTime.Now, p_PRINCIPAL_ID, int.Parse(p_SOLD_TO.ToString()), mISom.SALE_INVOICE_ID, mISom.MANUAL_INVOICE_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.CreditSale, p_DELIVERYMAN_ID.ToString());
                            LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.SaleAccount), p_Distributor_id, 0, p_TOTAL_NET_AMOUNT, mISom.DOCUMENT_DATE, "Credit Sale Default", DateTime.Now, p_PRINCIPAL_ID, int.Parse(p_SOLD_TO.ToString()), mISom.SALE_INVOICE_ID, mISom.MANUAL_INVOICE_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.CreditSale, p_DELIVERYMAN_ID.ToString());
                        }
                        else
                        {
                            LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.AccountReceivable), p_Distributor_id, 0, p_TOTAL_NET_AMOUNT, mISom.DOCUMENT_DATE, "Credit Refund Default", DateTime.Now, p_PRINCIPAL_ID, int.Parse(p_SOLD_TO.ToString()), mISom.SALE_INVOICE_ID, mISom.MANUAL_INVOICE_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.CreditSale, p_DELIVERYMAN_ID.ToString());
                            LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.SaleAccount), p_Distributor_id, p_TOTAL_NET_AMOUNT, 0, mISom.DOCUMENT_DATE, "Credit Refund Default", DateTime.Now, p_PRINCIPAL_ID, int.Parse(p_SOLD_TO.ToString()), mISom.SALE_INVOICE_ID, mISom.MANUAL_INVOICE_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.CreditSale, p_DELIVERYMAN_ID.ToString());
                        }

                    }
                    if (p_PaymentMode == 1 || p_PaymentMode == 2 || p_PaymentMode == 5)//Cash,Credit Card,Mix Mode
                    {
                        string VoucherNo2 = LController.SelectMaxVoucherId(Constants.Journal_Voucher, p_Distributor_id, p_DocumentDate);
                        if (p_MANUAL_INVOICE_ID == "2")
                        {
                            if (LController.PostingGLMaster(p_Distributor_id, 0, VoucherNo2, Constants.Journal_Voucher, p_DocumentDate, Constants.Document_SaleInvoice, Convert.ToString(mISom.SALE_INVOICE_ID), "Sale Voucher", p_UserId, "Sale", Constants.Document_SaleInvoice, mISom.SALE_INVOICE_ID))
                            {
                                if (p_PaymentMode == 1)//Cash
                                {
                                    //Dr Cash in Hand
                                    //Cr Cash Sale
                                    if (p_TOTAL_AMOUNT + p_GST_AMOUNT - p_EXTRA_DISCOUNT_AMOUNT - p_SCHEME_AMOUNT > 0)
                                    {
                                        //352-3002120004-Cash
                                        LController.PostingGLDetail(p_Distributor_id, 0, Constants.Journal_Voucher, VoucherNo2, 352, p_TOTAL_AMOUNT + p_GST_AMOUNT - p_EXTRA_DISCOUNT_AMOUNT - p_SCHEME_AMOUNT, 0, "Cash In Hand Sale Voucher");
                                    }
                                    if (p_TOTAL_AMOUNT > 0)
                                    {
                                        //762-4001010013-Cash Sales
                                        LController.PostingGLDetail(p_Distributor_id, 0, Constants.Journal_Voucher, VoucherNo2, 762, 0, p_TOTAL_AMOUNT, "Sale Voucher");
                                    }
                                    if (p_EXTRA_DISCOUNT_AMOUNT + p_SCHEME_AMOUNT > 0)
                                    {
                                        //764-4001020001-Discount on Sale
                                        LController.PostingGLDetail(p_Distributor_id, 0, Constants.Journal_Voucher, VoucherNo2, 764, p_EXTRA_DISCOUNT_AMOUNT + p_SCHEME_AMOUNT, 0, "Discount Sale Voucher");
                                    }
                                    if (p_GST_AMOUNT > 0)
                                    {
                                        //73-2002020004-Sales Tax Paid
                                        LController.PostingGLDetail(p_Distributor_id, 0, Constants.Journal_Voucher, VoucherNo2, 73, 0, p_GST_AMOUNT, "GST Sale Voucher");
                                    }
                                }
                                else//Credit Card,Mixed Mode
                                {
                                    if (p_STANDARD_DISCOUNT_AMOUNT > 0)
                                    {
                                        //352-3002120004-Cash
                                        LController.PostingGLDetail(p_Distributor_id, 0, Constants.Journal_Voucher, VoucherNo2, 352, p_STANDARD_DISCOUNT_AMOUNT, 0, "Cash In Hand Sale Voucher");
                                    }
                                    if (p_TOTAL_AMOUNT - p_STANDARD_DISCOUNT_AMOUNT > 0)
                                    {
                                        //765-3002130003-Credit Card Payments
                                        LController.PostingGLDetail(p_Distributor_id, 0, Constants.Journal_Voucher, VoucherNo2, 765, (p_TOTAL_NET_AMOUNT - p_STANDARD_DISCOUNT_AMOUNT), 0, "Credit Card Sale Voucher");
                                    }
                                    if (p_TOTAL_AMOUNT + p_GST_AMOUNT - p_EXTRA_DISCOUNT_AMOUNT - p_SCHEME_AMOUNT + p_STANDARD_DISCOUNT_AMOUNT > 0)
                                    {
                                        //708-4001010012-Credit Card Sales
                                        LController.PostingGLDetail(p_Distributor_id, 0, Constants.Journal_Voucher, VoucherNo2, 708, 0, p_TOTAL_NET_AMOUNT - p_GST_AMOUNT + p_EXTRA_DISCOUNT_AMOUNT + p_SCHEME_AMOUNT, "Credit Card Sale Voucher");
                                    }
                                    if (p_EXTRA_DISCOUNT_AMOUNT + p_SCHEME_AMOUNT > 0)
                                    {
                                        //764-4001020001-Discount on Sale
                                        LController.PostingGLDetail(p_Distributor_id, 0, Constants.Journal_Voucher, VoucherNo2, 764, p_EXTRA_DISCOUNT_AMOUNT + p_SCHEME_AMOUNT, 0, "Discount Sale Voucher");
                                    }
                                    if (p_GST_AMOUNT > 0)
                                    {
                                        //73-2002020004-Sales Tax Paid
                                        LController.PostingGLDetail(p_Distributor_id, 0, Constants.Journal_Voucher, VoucherNo2, 73, 0, p_GST_AMOUNT, "GST Sale Voucher");
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (LController.PostingGLMaster(p_Distributor_id, 0, VoucherNo2, Constants.Journal_Voucher, p_DocumentDate, Constants.Document_SaleReturn, Convert.ToString(mISom.SALE_INVOICE_ID), "Sale Refund Voucher", p_UserId, "Refund", Constants.Document_SaleReturn, mISom.SALE_INVOICE_ID))
                            {
                                if (p_PaymentMode == 1)//Cash
                                {
                                    p_TOTAL_AMOUNT = Math.Abs(p_TOTAL_AMOUNT);
                                    p_GST_AMOUNT = Math.Abs(p_GST_AMOUNT);
                                    p_EXTRA_DISCOUNT_AMOUNT = Math.Abs(p_EXTRA_DISCOUNT_AMOUNT);
                                    p_SCHEME_AMOUNT = Math.Abs(p_SCHEME_AMOUNT);
                                    p_STANDARD_DISCOUNT_AMOUNT = Math.Abs(p_STANDARD_DISCOUNT_AMOUNT);
                                    //Dr Cash in Hand
                                    //Cr Cash Sale
                                    if (p_TOTAL_AMOUNT + p_GST_AMOUNT - p_EXTRA_DISCOUNT_AMOUNT - p_SCHEME_AMOUNT > 0)
                                    {
                                        //352-3002120004-Cash
                                        LController.PostingGLDetail(p_Distributor_id, 0, Constants.Journal_Voucher, VoucherNo2, 352, 0, p_TOTAL_AMOUNT + p_GST_AMOUNT - p_EXTRA_DISCOUNT_AMOUNT - p_SCHEME_AMOUNT, "Cash In Hand Sale Refund Voucher");
                                    }
                                    if (p_TOTAL_AMOUNT > 0)
                                    {
                                        //762-4001010013-Cash Sales
                                        LController.PostingGLDetail(p_Distributor_id, 0, Constants.Journal_Voucher, VoucherNo2, 762, p_TOTAL_AMOUNT, 0, "Sale Refund Voucher");
                                    }
                                    if (p_EXTRA_DISCOUNT_AMOUNT + p_SCHEME_AMOUNT > 0)
                                    {
                                        //764-4001020001-Discount on Sale
                                        LController.PostingGLDetail(p_Distributor_id, 0, Constants.Journal_Voucher, VoucherNo2, 764, 0, p_EXTRA_DISCOUNT_AMOUNT + p_SCHEME_AMOUNT, "Discount Sale Refund Voucher");
                                    }
                                    if (p_GST_AMOUNT > 0)
                                    {
                                        //73-2002020004-Sales Tax Paid
                                        LController.PostingGLDetail(p_Distributor_id, 0, Constants.Journal_Voucher, VoucherNo2, 73, p_GST_AMOUNT, 0, "GST Sale Refund Voucher");
                                    }
                                }
                                else//Credit Card,Mixed Mode
                                {
                                    if (p_STANDARD_DISCOUNT_AMOUNT > 0)
                                    {
                                        //352-3002120004-Cash
                                        LController.PostingGLDetail(p_Distributor_id, 0, Constants.Journal_Voucher, VoucherNo2, 352, 0, p_STANDARD_DISCOUNT_AMOUNT, "Cash In Hand Sale Refund Voucher");
                                    }
                                    if (p_TOTAL_AMOUNT - p_STANDARD_DISCOUNT_AMOUNT > 0)
                                    {
                                        //765-3002130003-Credit Card Payments
                                        LController.PostingGLDetail(p_Distributor_id, 0, Constants.Journal_Voucher, VoucherNo2, 765, 0, (p_TOTAL_NET_AMOUNT - p_STANDARD_DISCOUNT_AMOUNT), "Credit Card Sale Refund Voucher");
                                    }
                                    if (p_TOTAL_AMOUNT + p_GST_AMOUNT - p_EXTRA_DISCOUNT_AMOUNT - p_SCHEME_AMOUNT + p_STANDARD_DISCOUNT_AMOUNT > 0)
                                    {
                                        //708-4001010012-Credit Card Sales
                                        LController.PostingGLDetail(p_Distributor_id, 0, Constants.Journal_Voucher, VoucherNo2, 708, p_TOTAL_NET_AMOUNT - p_GST_AMOUNT + p_EXTRA_DISCOUNT_AMOUNT + p_SCHEME_AMOUNT, 0, "Credit Card Sale Refund Voucher");
                                    }
                                    if (p_EXTRA_DISCOUNT_AMOUNT + p_SCHEME_AMOUNT > 0)
                                    {
                                        //764-4001020001-Discount on Sale
                                        LController.PostingGLDetail(p_Distributor_id, 0, Constants.Journal_Voucher, VoucherNo2, 764, 0, p_EXTRA_DISCOUNT_AMOUNT + p_SCHEME_AMOUNT, "Discount Sale Refund Voucher");
                                    }
                                    if (p_GST_AMOUNT > 0)
                                    {
                                        //73-2002020004-Sales Tax Paid
                                        LController.PostingGLDetail(p_Distributor_id, 0, Constants.Journal_Voucher, VoucherNo2, 73, p_GST_AMOUNT, 0, "GST Sale Refund Voucher");
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    mTransaction.Commit();

                    return mISom.SALE_INVOICE_ID;
                }
            }
            catch (Exception exp)
            {
                mTransaction.Rollback();
                ExceptionPublisher.PublishException(exp);

                return -2;// exp.Message;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
            return -1;
        }

        public bool Update_Invoice2(long p_SALE_INVOICE_ID, DateTime p_DocumentDate, int p_Distributor_id, string p_MANUAL_INVOICE_ID, long p_SOLD_TO, int p_DELIVERYMAN_ID,
       decimal p_TOTAL_AMOUNT, decimal p_EXTRA_DISCOUNT_AMOUNT, decimal p_STANDARD_DISCOUNT_AMOUNT, decimal p_GST_AMOUNT, decimal p_TOTAL_NET_AMOUNT, decimal p_SCHEME_AMOUNT, int InvoiceTypeId, DataTable dtOrderDetail, int p_UserId, decimal p_CashReceived, decimal p_TSTAmount, decimal p_SEDAmount, int p_DCNumber, int p_RefNumber, DateTime p_PODate)
        {
            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;

            try
            {
                try
                {
                    mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                    mConnection.Open();
                    foreach (DataRow dr in dtOrderDetail.Rows)
                    {
                        uspDeleteInvoiceDetail mOrder = new uspDeleteInvoiceDetail();
                        mOrder.Connection = mConnection;
                        mOrder.SALE_INVOICE_ID = p_SALE_INVOICE_ID;
                        mOrder.SKU_ID = Convert.ToInt32(dr["SKU_ID"]);
                        mOrder.ExecuteQuery();
                    }

                }
                catch (Exception exp)
                {
                    ExceptionPublisher.PublishException(exp);
                }
                finally
                {
                    if (mConnection != null && mConnection.State == ConnectionState.Open)
                    {
                        mConnection.Close();
                    }
                }


                decimal TotalAmt = 0, DiscountAmount = 0, ExtraDiscount = 0, GSTAmount = 0, TotalNetAmt = 0;

                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);

                uspUpdateInvoiceMaster2 mISom = new uspUpdateInvoiceMaster2();
                mISom.Connection = mConnection;
                mISom.Transaction = mTransaction;

                //------------Insert into Sale Order Master----------

                if (dtOrderDetail.Rows.Count > 0)
                {
                    mISom.DISTRIBUTOR_ID = p_Distributor_id;
                    mISom.MANUAL_INVOICE_ID = p_MANUAL_INVOICE_ID;
                    mISom.SALE_INVOICE_ID = p_SALE_INVOICE_ID;
                    mISom.DELIVERYMAN_ID = p_DELIVERYMAN_ID;
                    mISom.SOLD_TO = p_SOLD_TO;
                    mISom.TOTAL_AMOUNT = p_TOTAL_AMOUNT;
                    mISom.EXTRA_DISCOUNT_AMOUNT = p_EXTRA_DISCOUNT_AMOUNT;
                    mISom.STANDARD_DISCOUNT_AMOUNT = p_STANDARD_DISCOUNT_AMOUNT;
                    mISom.GST_AMOUNT = p_GST_AMOUNT;
                    mISom.SCHEME_AMOUNT = p_SCHEME_AMOUNT;
                    mISom.TOTAL_NET_AMOUNT = p_TOTAL_NET_AMOUNT;
                    mISom.PoDate = p_PODate;
                    mISom.RefNumber = p_RefNumber;
                    mISom.DCNumber = p_DCNumber;
                    if (InvoiceTypeId == Constants.Credit_Order_Id)
                    {
                        mISom.CREDIT_AMOUNT = p_TOTAL_NET_AMOUNT - p_CashReceived;
                        mISom.CURRENT_CREDIT_AMOUNT = p_TOTAL_NET_AMOUNT - p_CashReceived;
                    }
                    else
                    {
                        mISom.CREDIT_AMOUNT = 0;
                        mISom.CURRENT_CREDIT_AMOUNT = 0;
                    }
                    mISom.USER_ID = p_UserId;
                    mISom.TST_AMOUNT = p_TSTAmount;
                    mISom.SED_AMOUNT = p_SEDAmount;
                    mISom.ExecuteQuery();

                    //----------------Insert into sale order detail-------------
                    spInsertSALE_INVOICE_DETAIL mSaleOrderDetail = new spInsertSALE_INVOICE_DETAIL();
                    mSaleOrderDetail.Connection = mConnection;
                    mSaleOrderDetail.Transaction = mTransaction;

                    foreach (DataRow dr in dtOrderDetail.Rows)
                    {
                        //SaleOrderDetail_Collection mSod_Col=new SaleOrderDetail_Collection ();
                        mSaleOrderDetail.SALE_INVOICE_ID = p_SALE_INVOICE_ID;
                        mSaleOrderDetail.DISTRIBUTOR_ID = p_Distributor_id;
                        mSaleOrderDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                        mSaleOrderDetail.BATCH_NO = dr["BATCH_NO"].ToString();
                        mSaleOrderDetail.QUANTITY_UNIT = int.Parse(dr["QUANTITY_UNIT"].ToString());
                        mSaleOrderDetail.UNIT_PRICE = decimal.Parse(dr["UNIT_PRICE"].ToString());
                        mSaleOrderDetail.GST_RATE = float.Parse(dr["GST_RATE"].ToString());
                        mSaleOrderDetail.AMOUNT = decimal.Parse(dr["AMOUNT"].ToString());
                        mSaleOrderDetail.EXTRA_DISCOUNT = decimal.Parse(dr["EXTRA_DISCOUNT"].ToString());
                        mSaleOrderDetail.STANDARD_DISCOUNT = decimal.Parse(dr["STANDARD_DISCOUNT"].ToString());
                        mSaleOrderDetail.GST_AMOUNT = 0;
                        mSaleOrderDetail.TST_AMOUNT = 0;
                        mSaleOrderDetail.SED_AMOUNT = 0;
                        mSaleOrderDetail.NET_AMOUNT = decimal.Parse(dr["NET_AMOUNT"].ToString());
                        mSaleOrderDetail.IS_DELETED = false;
                        mSaleOrderDetail.TIME_STAMP = p_DocumentDate;
                        TotalAmt += decimal.Parse(dr["AMOUNT"].ToString());
                        ExtraDiscount += decimal.Parse(dr["EXTRA_DISCOUNT"].ToString());
                        DiscountAmount += decimal.Parse(dr["STANDARD_DISCOUNT"].ToString());
                        GSTAmount += decimal.Parse(dr["GST_AMOUNT"].ToString());
                        TotalNetAmt += decimal.Parse(dr["NET_AMOUNT"].ToString());
                        mSaleOrderDetail.ExecuteQuery();

                        UspProcessStockRegister mStockUpdate = new UspProcessStockRegister();
                        mStockUpdate.Connection = mConnection;
                        mStockUpdate.Transaction = mTransaction;
                        if (mSaleOrderDetail.QUANTITY_UNIT < 0)
                        {
                            mStockUpdate.TYPE_ID = Constants.Document_Sale_Return;
                            mStockUpdate.FREE_QTY = mSaleOrderDetail.QUANTITY_UNIT * (-1);
                        }
                        else
                        {
                            mStockUpdate.TYPE_ID = Constants.Document_Invoice;
                            mStockUpdate.FREE_QTY = mSaleOrderDetail.QUANTITY_UNIT;
                        }
                        mStockUpdate.DISTRIBUTOR_ID = p_Distributor_id;
                        mStockUpdate.STOCK_DATE = p_DocumentDate;
                        mStockUpdate.SKU_ID = mSaleOrderDetail.SKU_ID;
                        mStockUpdate.BATCHNO = mSaleOrderDetail.BATCH_NO;
                        mStockUpdate.STOCK_QTY = 0;
                        mStockUpdate.ExecuteQuery();
                    }

                    #region Account Posting
                    LedgerController LController = new LedgerController();
                    Configuration.GetAccountHead();
                    DistributorController Dcontroller = new DistributorController();


                    string VoucherNo = LController.SelectLedgerMaxDocumentId(Constants.Journal_Voucher, p_Distributor_id, 0);

                    if (InvoiceTypeId == Constants.Advance_PaymentOrder_id)
                    {
                        LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.SaleAccount), p_Distributor_id, 0, p_TOTAL_AMOUNT, p_DocumentDate, "Gross Sale Value", DateTime.Now, 0, int.Parse(p_SOLD_TO.ToString()), mISom.SALE_INVOICE_ID, mISom.MANUAL_INVOICE_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.Cash_Advance, p_DELIVERYMAN_ID.ToString());

                        if (p_STANDARD_DISCOUNT_AMOUNT > 0)
                        {
                            LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.SaleDiscount), p_Distributor_id, p_STANDARD_DISCOUNT_AMOUNT, 0, p_DocumentDate, "Commision/Discount", DateTime.Now, 0, int.Parse(p_SOLD_TO.ToString()), mISom.SALE_INVOICE_ID, mISom.MANUAL_INVOICE_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.Cash_Advance, p_DELIVERYMAN_ID.ToString());
                        }
                        if (p_EXTRA_DISCOUNT_AMOUNT > 0)
                        {
                            LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.SaleScheme), p_Distributor_id, p_EXTRA_DISCOUNT_AMOUNT, 0, p_DocumentDate, "Extra Discount", DateTime.Now, 0, int.Parse(p_SOLD_TO.ToString()), mISom.SALE_INVOICE_ID, mISom.MANUAL_INVOICE_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.Cash_Advance, p_DELIVERYMAN_ID.ToString());
                        }
                        if (p_GST_AMOUNT + p_TSTAmount > 0)
                        {
                            LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.GSTAccount), p_Distributor_id, 0, p_GST_AMOUNT + p_TSTAmount, p_DocumentDate, "Sales Tax", DateTime.Now, 0, int.Parse(p_SOLD_TO.ToString()), mISom.SALE_INVOICE_ID, mISom.MANUAL_INVOICE_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.Cash_Advance, p_DELIVERYMAN_ID.ToString());
                        }
                        LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.AccountReceivable), p_Distributor_id, p_TOTAL_NET_AMOUNT, 0, p_DocumentDate, "Credit Sale Default", DateTime.Now, 0, int.Parse(p_SOLD_TO.ToString()), mISom.SALE_INVOICE_ID, mISom.MANUAL_INVOICE_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.Cash_Advance, p_DELIVERYMAN_ID.ToString());

                    }
                    else if (InvoiceTypeId == Constants.Credit_Order_Id)
                    {
                        LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.AccountReceivable), p_Distributor_id, p_TOTAL_NET_AMOUNT - p_CashReceived, 0, p_DocumentDate, "Credit Sale Default", DateTime.Now, 0, int.Parse(p_SOLD_TO.ToString()), mISom.SALE_INVOICE_ID, mISom.MANUAL_INVOICE_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.CreditSale, p_DELIVERYMAN_ID.ToString());
                        LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.SaleAccount), p_Distributor_id, 0, p_TOTAL_NET_AMOUNT - p_CashReceived, p_DocumentDate, "Credit Sale Default", DateTime.Now, 0, int.Parse(p_SOLD_TO.ToString()), mISom.SALE_INVOICE_ID, mISom.MANUAL_INVOICE_ID, Constants.Document_SaleInvoice, p_UserId, mTransaction, mConnection, Constants.CreditSale, p_DELIVERYMAN_ID.ToString());

                    }
                    #endregion

                    mTransaction.Commit();
                    return true;
                }
            }
            catch (Exception exp)
            {
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

        public bool Update_Order(long p_SALE_ORDER_ID)
        {
            IDbConnection mConnection = null;


            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spUPDATESALEORDER mOrder = new spUPDATESALEORDER();
                mOrder.Connection = mConnection;
                mOrder.SALE_ORDER_ID = p_SALE_ORDER_ID;

                mOrder.ExecuteQuery();

            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
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

        /// <summary>
        /// Inserts Sale Returns
        /// </summary>
        /// <param name="p_Distributor_id">Location</param>
        /// <param name="p_TOWN_ID">Town</param>
        /// <param name="p_AREA_ID">Market</param>
        /// <param name="p_PRINCIPAL_ID">Principal</param>
        /// <param name="p_SOLD_TO">Customer</param>
        /// <param name="p_SHIP_TO">ShipTo</param>
        /// <param name="p_ORDERBOOKER_ID">OrderBooker</param>
        /// <param name="p_DELIVERYMAN_ID">Deliveryman</param>
        /// <param name="p_Orderid">Order</param>
        /// <param name="p_TOTAL_AMOUNT">Amount</param>
        /// <param name="p_EXTRA_DISCOUNT_AMOUNT">ExtraDiscount</param>
        /// <param name="p_STANDARD_DISCOUNT_AMOUNT">Discount</param>
        /// <param name="p_GST_AMOUNT">GST</param>
        /// <param name="p_TOTAL_NET_AMOUNT">NetAmount</param>
        /// <param name="p_SCHEME_AMOUNT">Scheme</param>
        /// <param name="InvoiceTypeId">Type</param>
        /// <param name="dtOrderDetail">OrderDetailDatatable</param>
        /// <param name="dtFreeSKU">FreeSKUDatatable</param>
        /// <param name="p_UserId">InsertedBy</param>
        /// <param name="p_DocumentDate">Date</param>
        /// <param name="p_TstAmount">TSTAmount</param>
        /// <param name="p_SEDAmount">SEDAmount</param>
        /// <returns>True On Success And False On Failure</returns>
        public bool Add_SaleReturn(int p_Distributor_id, int p_TOWN_ID, long p_AREA_ID, int p_PRINCIPAL_ID, long p_SOLD_TO, long p_SHIP_TO, int p_ORDERBOOKER_ID, int p_DELIVERYMAN_ID, long p_Orderid,
        decimal p_TOTAL_AMOUNT, decimal p_EXTRA_DISCOUNT_AMOUNT, decimal p_STANDARD_DISCOUNT_AMOUNT, decimal p_GST_AMOUNT, decimal p_TOTAL_NET_AMOUNT, decimal p_SCHEME_AMOUNT, int InvoiceTypeId, DataTable dtOrderDetail, DataTable dtFreeSKU, int p_UserId, DateTime p_DocumentDate, decimal p_TstAmount, decimal p_SEDAmount)
        {

            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;

            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);

                spInsertSALES_RETURN_MASTER mISom = new spInsertSALES_RETURN_MASTER();
                mISom.Connection = mConnection;
                mISom.Transaction = mTransaction;

                //------------Insert into Sale Return Master----------

                if (dtOrderDetail.Rows.Count > 0)
                {
                    mISom.DISTRIBUTOR_ID = p_Distributor_id;
                    mISom.PRINCIPAL_ID = p_PRINCIPAL_ID;
                    mISom.AREA_ID = int.Parse(p_AREA_ID.ToString());
                    mISom.DELIVERYMAN_ID = p_DELIVERYMAN_ID;
                    mISom.ORDERBOOKER_ID = p_ORDERBOOKER_ID;
                    mISom.DOCUMENT_DATE = p_DocumentDate;
                    mISom.CUSTOMER_ID = p_SOLD_TO;
                    mISom.TOTAL_AMOUNT = p_TOTAL_AMOUNT;
                    mISom.EXTRA_DISCOUNT_AMOUNT = p_EXTRA_DISCOUNT_AMOUNT;
                    mISom.STANDARD_DISCOUNT_AMOUNT = p_STANDARD_DISCOUNT_AMOUNT;
                    mISom.GST_AMOUNT = p_GST_AMOUNT;
                    mISom.TOTAL_NET_AMOUNT = p_TOTAL_NET_AMOUNT;
                    mISom.TOWN_ID = p_TOWN_ID;
                    mISom.TIME_STAMP = DateTime.Now;
                    mISom.LASTUPDATE_DATE = System.DateTime.Now;
                    mISom.TST_AMOUNT = p_TstAmount;
                    mISom.SED_AMOUNT = p_SEDAmount;
                    mISom.IS_DELETED = false;
                    mISom.POSTING = 0;
                    mISom.ExecuteQuery();

                    //----------------Insert into sales return detail-------------
                    spInsertSALES_RETURN_DETAIL mSaleOrderDetail = new spInsertSALES_RETURN_DETAIL();
                    mSaleOrderDetail.Connection = mConnection;
                    mSaleOrderDetail.Transaction = mTransaction;

                    foreach (DataRow dr in dtOrderDetail.Rows)
                    {
                        mSaleOrderDetail.SALES_RETURN_ID = mISom.SALES_RETURN_ID;
                        mSaleOrderDetail.DISTRIBUTOR_ID = p_Distributor_id;
                        mSaleOrderDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                        mSaleOrderDetail.BATCH_NO = dr["BATCH_NO"].ToString();
                        mSaleOrderDetail.QUANTITY_UNIT = int.Parse(dr["QUANTITY_UNIT"].ToString());
                        mSaleOrderDetail.UNIT_PRICE = decimal.Parse(dr["UNIT_PRICE"].ToString());
                        mSaleOrderDetail.GST_RATE = float.Parse(dr["GST_RATE"].ToString());
                        mSaleOrderDetail.AMOUNT = decimal.Parse(dr["AMOUNT"].ToString());
                        mSaleOrderDetail.EXTRA_DISCOUNT = decimal.Parse(dr["EXTRA_DISCOUNT"].ToString());
                        mSaleOrderDetail.STANDARD_DISCOUNT = decimal.Parse(dr["STANDARD_DISCOUNT"].ToString());
                        mSaleOrderDetail.GST_AMOUNT = decimal.Parse(dr["GST_AMOUNT"].ToString());
                        mSaleOrderDetail.TST_AMOUNT = decimal.Parse(dr["TST_AMOUNT"].ToString());
                        // mSaleOrderDetail.SED_AMOUNT = decimal.Parse(dr["SED_AMOUNT"].ToString());
                        mSaleOrderDetail.NET_AMOUNT = decimal.Parse(dr["NET_AMOUNT"].ToString());
                        mSaleOrderDetail.TIME_STAMP = p_DocumentDate;
                        mSaleOrderDetail.ExecuteQuery();

                    }

                    #region Account Posting
                    LedgerController LController = new LedgerController();
                    Configuration.GetAccountHead();
                    DistributorController Dcontroller = new DistributorController();


                    string VoucherNo = LController.SelectLedgerMaxDocumentId(Constants.Journal_Voucher, p_Distributor_id, 0);

                    LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.SaleReturnAccount), p_Distributor_id, p_TOTAL_AMOUNT, 0, mISom.DOCUMENT_DATE, "Sales Return Value", DateTime.Now, p_PRINCIPAL_ID, int.Parse(p_SOLD_TO.ToString()), mISom.SALES_RETURN_ID, mISom.SALES_RETURN_ID.ToString(), Constants.Document_Sale_Return, p_UserId, mTransaction, mConnection, Constants.CashSaleReturn, p_DELIVERYMAN_ID.ToString());

                    if (p_STANDARD_DISCOUNT_AMOUNT > 0 || p_EXTRA_DISCOUNT_AMOUNT > 0)
                    {
                        LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.SaleReturnDiscount), p_Distributor_id, 0, p_STANDARD_DISCOUNT_AMOUNT + p_EXTRA_DISCOUNT_AMOUNT, mISom.DOCUMENT_DATE, "Sales Return Discount/Extra Discount", DateTime.Now, p_PRINCIPAL_ID, int.Parse(p_SOLD_TO.ToString()), mISom.SALES_RETURN_ID, mISom.SALES_RETURN_ID.ToString(), Constants.CashSaleReturn, p_UserId, mTransaction, mConnection, Constants.Document_Sale_Return, p_DELIVERYMAN_ID.ToString());
                    }
                    if (p_GST_AMOUNT + p_TstAmount > 0)
                    {
                        LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.SaleReturnGST), p_Distributor_id, p_GST_AMOUNT + p_TstAmount, 0, mISom.DOCUMENT_DATE, "Sales Return Tax", DateTime.Now, p_PRINCIPAL_ID, int.Parse(p_SOLD_TO.ToString()), mISom.SALES_RETURN_ID, mISom.SALES_RETURN_ID.ToString(), Constants.Document_Sale_Return, p_UserId, mTransaction, mConnection, Constants.CashSaleReturn, p_DELIVERYMAN_ID.ToString());
                    }
                    LController.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), long.Parse(Configuration.AccountReceivable), p_Distributor_id, 0, p_TOTAL_NET_AMOUNT, mISom.DOCUMENT_DATE, "Sales Return Default", DateTime.Now, p_PRINCIPAL_ID, int.Parse(p_SOLD_TO.ToString()), mISom.SALES_RETURN_ID, mISom.SALES_RETURN_ID.ToString(), Constants.Document_Sale_Return, p_UserId, mTransaction, mConnection, Constants.CashSaleReturn, p_DELIVERYMAN_ID.ToString());

                    #endregion

                    mTransaction.Commit();
                    return true;
                }
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                mTransaction.Rollback();
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

        /// <summary>
        /// Delets Free SKU Form Invoice
        /// </summary>
        /// <param name="p_Distributor_Id">Location</param>
        /// <param name="p_Invoice_Id">Invoice</param>
        /// <param name="p_SalePromotionId">Promotion</param>
        /// <param name="p_SKU_id">SKU</param>
        /// <param name="p_Qty">Quantity</param>
        /// <returns>True On Success And False On Failure</returns>
        public bool DeleteFreeSKUFromInvoice(int p_Distributor_Id, long p_Invoice_Id, long p_SalePromotionId, int p_SKU_id, int p_Qty)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspDeleteFreeSKUOut mOrder = new UspDeleteFreeSKUOut();
                mOrder.Connection = mConnection;
                mOrder.Distributor_id = p_Distributor_Id;
                mOrder.InvoiceNo = p_Invoice_Id;
                mOrder.SALE_INVOICE_PROMOTION_ID = p_SalePromotionId;
                mOrder.SKU_id = p_SKU_id;
                mOrder.Qty = p_Qty;
                mOrder.ExecuteQuery();
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

        /// <summary>
        /// Inserts Transport Expenses
        /// </summary>
        /// <param name="p_Distributor_id">Location</param>
        /// <param name="p_SaleInvoiceId">Invoice</param>
        /// <param name="p_Transport_ID">Transport</param>
        /// <param name="p_Bilty_no">Builty</param>
        /// <param name="p_DilveryChallan">DeliveryChalan</param>
        /// <param name="p_Exp">Expenses</param>
        /// <returns>True On Success And False On Failure</returns>
        public bool PostTranspoterExp(int p_Distributor_id, long p_SaleInvoiceId, int p_Transport_ID, string p_Bilty_no, string p_DilveryChallan, decimal p_Exp)
        {

            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spInsertDTINVOICE_BILLTY_DETAIL InsertExp = new spInsertDTINVOICE_BILLTY_DETAIL();
                InsertExp.Connection = mConnection;

                InsertExp.DISTRIBUTOR_ID = p_Distributor_id;
                InsertExp.SALE_INVOICE_NO = p_SaleInvoiceId;
                InsertExp.TRANSPOTER_NO = p_Transport_ID;
                InsertExp.BILTY_NO = p_Bilty_no;
                InsertExp.DELIVERY_CHALLAN_NO = p_DilveryChallan;
                InsertExp.TOTAL_EXPENCESS = p_Exp;
                return InsertExp.ExecuteQuery();
            }
            catch (Exception exp)
            {
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

        /// <summary>
        /// Inserts Free SKU
        /// </summary>
        /// <param name="p_Distributor_id">Location</param>
        /// <param name="p_SaleInvoiceId">Invoice</param>
        /// <param name="p_sku_ID">SKU</param>
        /// <param name="p_QUANTITY">Quantitiy</param>
        /// <param name="p_UNIT_PRICE">Price</param>
        /// <param name="p_AMOUNT">Amount</param>
        /// <param name="p_GST_RATE">GSTRate</param>
        /// <param name="p_GST_AMOUNT">GSTAmount</param>
        /// <param name="p_DocumentDate">Date</param>
        /// <param name="p_TstAmount">TSTAmount</param>
        /// <param name="p_SedAmount">SEDAmount</param>
        /// <returns>True On Success And False On Failure</returns>
        public bool InsertFreeSKU(int p_Distributor_id, long p_SaleInvoiceId, int p_sku_ID, int p_QUANTITY, decimal p_UNIT_PRICE, decimal p_AMOUNT, float p_GST_RATE, decimal p_GST_AMOUNT, DateTime p_DocumentDate, decimal p_TstAmount, decimal p_SedAmount)
        {

            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                //----------------Insert into sale order Promotion-------------
                spInsertManualSALE_INVOICE_PROMOTION mSaleOrderPromo = new spInsertManualSALE_INVOICE_PROMOTION();
                mSaleOrderPromo.Connection = mConnection;
                mSaleOrderPromo.BASKET_DETAIL_ID = -1;
                mSaleOrderPromo.BASKET_ID = -1;
                mSaleOrderPromo.DISTRIBUTOR_ID = p_Distributor_id;
                mSaleOrderPromo.GST_AMOUNT = p_GST_AMOUNT;
                mSaleOrderPromo.GST_RATE = p_GST_RATE;
                mSaleOrderPromo.PROMOTION_ID = -1;
                mSaleOrderPromo.PROMOTION_OFFER_ID = -1;
                mSaleOrderPromo.QUANTITY = p_QUANTITY;
                mSaleOrderPromo.SKU_ID = p_sku_ID;
                mSaleOrderPromo.UNIT_PRICE = p_UNIT_PRICE;
                mSaleOrderPromo.SALE_INVOICE_ID = p_SaleInvoiceId;
                mSaleOrderPromo.AMOUNT = p_AMOUNT;
                mSaleOrderPromo.SED_AMOUNT = p_SedAmount;
                mSaleOrderPromo.TST_AMOUNT = p_TstAmount;
                mSaleOrderPromo.ExecuteQuery();

                UspProcessStockRegister mStockUpdate = new UspProcessStockRegister();
                mStockUpdate.Connection = mConnection;
                mStockUpdate.TYPE_ID = Constants.Document_Invoice;
                mStockUpdate.DISTRIBUTOR_ID = p_Distributor_id;
                mStockUpdate.STOCK_DATE = p_DocumentDate;
                mStockUpdate.SKU_ID = mSaleOrderPromo.SKU_ID;
                mStockUpdate.BATCHNO = "N/A";
                mStockUpdate.STOCK_QTY = 0;
                mStockUpdate.FREE_QTY = p_QUANTITY;
                mStockUpdate.ExecuteQuery();

            }
            catch (Exception exp)
            {
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

        #region Rollback

        /// <summary>
        /// Rollbacks Order, Invoice And Sale Return
        /// </summary>
        /// <param name="p_DocumentId">Document</param>
        /// <param name="p_Type_Id">Type</param>
        /// <param name="p_LegendId">Legend</param>
        /// <returns>True On Success And False On Failure</returns>
        public bool UpdateRollBackDocument(long p_DocumentId, int p_Type_Id, int p_LegendId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspRollBackDocument mOrder = new UspRollBackDocument();
                mOrder.Connection = mConnection;
                mOrder.DOCUMENT_ID = p_DocumentId;
                mOrder.DOCUMENT_TYPE = p_Type_Id;
                mOrder.LEGEND_ID = p_LegendId;
                mOrder.ExecuteQuery();
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

        public bool UpdateInvoiceNumberFBR(long p_SaleInvoiceId, string p_InvoiceNumberFBR)
        {

            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                //----------------Insert into sale order Promotion-------------
                uspUpdateInvoiceNumberFBR mSaleOrderPromo = new uspUpdateInvoiceNumberFBR();
                mSaleOrderPromo.Connection = mConnection;
                mSaleOrderPromo.SALE_INVOICE_ID = p_SaleInvoiceId;
                mSaleOrderPromo.InvoiceNumberFBR = p_InvoiceNumberFBR;
                mSaleOrderPromo.ExecuteQuery();
            }
            catch (Exception exp)
            {
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

        public bool UpdateInvoiceNumberRollBackTaxAuthority(long p_SALE_INVOICE_ID, string p_InvoiceNumberRollBackTaxAuthority,int TypeiID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspUpdateInvoiceNumberRollBackTaxAuthority mOrder = new uspUpdateInvoiceNumberRollBackTaxAuthority();
                mOrder.Connection = mConnection;
                mOrder.SALE_INVOICE_ID = p_SALE_INVOICE_ID;
                mOrder.InvoiceNumberRollBackTaxAuthority = p_InvoiceNumberRollBackTaxAuthority;
                mOrder.TypeiID = TypeiID;
                mOrder.ExecuteQuery();
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
    }
}