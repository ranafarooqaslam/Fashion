using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;

namespace CORNDatabaseLayer.Classes
{
	public class spUpdateVENDOR
	{
		#region Private Members
		private string sp_Name = "spUpdateVENDOR" ;
		private IDbConnection m_connection;
		private IDbTransaction m_transaction;
		private int m_VENDOR_ID;
		private int m_PRINCIPAL_ID;
		
		private bool m_IS_ACTIVE;
		private bool m_IS_DELETED;
		private string m_VENDOR_NAME;
		private string m_ADDRESS1;
		private string m_ADDRESS2;
		private string m_ADDRESS3;
		
		private string m_CONTACT_PERSON;
		private string m_CONTACT_NO;

		#endregion
		#region Public Properties
        
		public int VENDOR_ID
		{
			set
			{
				m_VENDOR_ID = value ;
			}
			get
			{
				return m_VENDOR_ID;
			}
		}
		public int PRINCIPAL_ID
		{
			set
			{
				m_PRINCIPAL_ID = value ;
			}
			get
			{
				return m_PRINCIPAL_ID;
			}
		}
		public bool IS_ACTIVE
		{
			set
			{
				m_IS_ACTIVE = value ;
			}
			get
			{
				return m_IS_ACTIVE;
			}
		}
		public bool IS_DELETED
		{
			set
			{
				m_IS_DELETED = value ;
			}
			get
			{
				return m_IS_DELETED;
			}
		}
		public  string VENDOR_NAME
		{
			set
			{
				m_VENDOR_NAME = value ;
			}
			get
			{
				return m_VENDOR_NAME;
			}
		}
		public  string ADDRESS1
		{
			set
			{
				m_ADDRESS1 = value ;
			}
			get
			{
				return m_ADDRESS1;
			}
		}
		public  string ADDRESS2
		{
			set
			{
				m_ADDRESS2 = value ;
			}
			get
			{
				return m_ADDRESS2;
			}
		}
		public  string ADDRESS3
		{
			set
			{
				m_ADDRESS3 = value ;
			}
			get
			{
				return m_ADDRESS3;
			}
		}
		public  string CONTACT_PERSON
		{
			set
			{
				m_CONTACT_PERSON = value ;
			}
			get
			{
				return m_CONTACT_PERSON;
			}
		}
		public  string CONTACT_NO
		{
			set
			{
				m_CONTACT_NO = value ;
			}
			get
			{
				return m_CONTACT_NO;
			}
		}


		public IDbConnection  Connection
		{
			set
			{
				m_connection = value;
			}
			get
			{
				return m_connection;
			}
		}
		public IDbTransaction  Transaction
		{
			set
			{
				m_transaction = value;
			}
			get
			{
				return m_transaction;
			}
		}
		#endregion
		#region Constructor
		public spUpdateVENDOR()
		{
		}
		#endregion
		#region public Methods
		public bool  ExecuteQuery()
		{
			try
			{
			    IDbCommand cmd = ProviderFactory.GetCommand(EnumProviders.SQLClient);
				cmd.CommandType =  CommandType.StoredProcedure;
				cmd.CommandText = sp_Name;
				cmd.Connection =   m_connection;
				if(m_transaction!=null)
				{
					cmd.Transaction = m_transaction;
				}
				GetParameterCollection(ref cmd);
				cmd.ExecuteNonQuery();
				return true;
			}
			catch(Exception e)
			{
				throw e;
			}
			finally
			{
			}
		}
		public IDataReader ExecuteReader()
		{
			try
			{
				IDbCommand command = ProviderFactory.GetCommand(EnumProviders.SQLClient);
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = sp_Name;
				command.Connection = m_connection;
				if(m_transaction!=null)
				{
					command.Transaction = m_transaction;
				}
				GetParameterCollection(ref command);
				IDataReader dr = command.ExecuteReader();
				return dr;
			}
			catch(Exception exp)
			{
				throw exp;
			}
			finally
			{
			}
		}
		public DataTable ExecuteTable()
		{
			try
			{
				IDbCommand command = ProviderFactory.GetCommand(EnumProviders.SQLClient);
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = sp_Name;
				command.Connection = m_connection;
				if(m_transaction!=null)
				{
					command.Transaction = m_transaction;
				}
				GetParameterCollection(ref command);
				IDbDataAdapter da = ProviderFactory.GetAdapter(EnumProviders.SQLClient);
				da.SelectCommand = command;
				DataSet ds = new DataSet();
				da.Fill(ds);
				return ds.Tables[0];
			}
			catch(Exception exp)
			{
				throw exp;
			}
			finally
			{
			}
		}
		public string ExecuteScalar()
		{
			try
			{
				IDbCommand command = ProviderFactory.GetCommand(EnumProviders.SQLClient);
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = sp_Name;
				command.Connection = m_connection;
				if(m_transaction!=null)
				{
					command.Transaction = m_transaction;
				}
				GetParameterCollection(ref command);
				object o;
				o = command.ExecuteScalar();


				return o.ToString();
			}
			catch(Exception exp)
			{
				throw exp;
			}
			finally
			{
			}
		}
		public void FirstReader(IDataReader dr)
		{
			if(dr.Read())
			{
				m_VENDOR_ID= Convert.ToInt32(dr["VENDOR_ID"]);
			
				m_PRINCIPAL_ID= Convert.ToInt32(dr["PRINCIPAL_ID"]);
				m_IS_ACTIVE=Convert.ToBoolean(dr["IS_ACTIVE"]);
				m_IS_DELETED=Convert.ToBoolean(dr["IS_DELETED"]);
				m_VENDOR_NAME= Convert.ToString(dr["VENDOR_NAME"]);
				m_ADDRESS1= Convert.ToString(dr["ADDRESS1"]);
				m_ADDRESS2= Convert.ToString(dr["ADDRESS2"]);
				m_ADDRESS3= Convert.ToString(dr["ADDRESS3"]);
				m_CONTACT_PERSON= Convert.ToString(dr["CONTACT_PERSON"]);
				m_CONTACT_NO= Convert.ToString(dr["CONTACT_NO"]);}
		}
		public void GetParameterCollection(ref IDbCommand cmd)
		{
			IDataParameterCollection pparams = cmd.Parameters;
			IDataParameter parameter ;


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@VENDOR_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_VENDOR_ID == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_VENDOR_ID;
            }
            pparams.Add(parameter);



			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@PRINCIPAL_ID" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
			if(m_PRINCIPAL_ID==Constants.IntNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_PRINCIPAL_ID;
			}
			pparams.Add(parameter);


			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@IS_ACTIVE" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
				parameter.Value = m_IS_ACTIVE;
			pparams.Add(parameter);


			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@IS_DELETED" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
				parameter.Value = m_IS_DELETED;
			pparams.Add(parameter);


			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@VENDOR_NAME" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
			if(m_VENDOR_NAME== null)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_VENDOR_NAME;
			}
			pparams.Add(parameter);


			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@ADDRESS1" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
			if(m_ADDRESS1== null)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_ADDRESS1;
			}
			pparams.Add(parameter);


			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@ADDRESS2" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
			if(m_ADDRESS2== null)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_ADDRESS2;
			}
			pparams.Add(parameter);


			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@ADDRESS3" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
			if(m_ADDRESS3== null)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_ADDRESS3;
			}
			pparams.Add(parameter);




			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@CONTACT_PERSON" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
			if(m_CONTACT_PERSON== null)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_CONTACT_PERSON;
			}
			pparams.Add(parameter);


			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@CONTACT_NO" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
			if(m_CONTACT_NO== null)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_CONTACT_NO;
			}
			pparams.Add(parameter);


		}
		#endregion
	}
}
