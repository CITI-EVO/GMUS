using System;
using System.Configuration;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Globalization;

namespace CITI.EVO.Tools.Utils
{
    public static class DcFactory
	{
        public static TDataContext Create<TDataContext>() where TDataContext : DataContext
		{
			return (TDataContext)Create(typeof(TDataContext));
		}
		public static TDataContext Create<TDataContext>(String connectionStringOrName) where TDataContext : DataContext
		{
			return (TDataContext)Create(typeof(TDataContext), connectionStringOrName);
		}
		public static TDataContext Create<TDataContext>(int commandTimeout) where TDataContext : DataContext
		{
			return (TDataContext)Create(typeof(TDataContext), commandTimeout);
		}
		public static TDataContext Create<TDataContext>(DataLoadOptions loadOptions) where TDataContext : DataContext
		{
			return (TDataContext)Create(typeof(TDataContext), loadOptions);
		}
		public static TDataContext Create<TDataContext>(String connectionStringOrName, int commandTimeout) where TDataContext : DataContext
		{
			return (TDataContext)Create(typeof(TDataContext), connectionStringOrName, commandTimeout);
		}
		public static TDataContext Create<TDataContext>(int commandTimeout, DataLoadOptions loadOptions) where TDataContext : DataContext
		{
			return (TDataContext)Create(typeof(TDataContext), commandTimeout, loadOptions);
		}
		public static TDataContext Create<TDataContext>(String connectionStringOrName, DataLoadOptions loadOptions) where TDataContext : DataContext
		{
			return (TDataContext)Create(typeof(TDataContext), connectionStringOrName, loadOptions);
		}
		public static TDataContext Create<TDataContext>(String connectionStringOrName, int commandTimeout, DataLoadOptions loadOptions) where TDataContext : DataContext
		{
			return (TDataContext)Create(typeof(TDataContext), connectionStringOrName, loadOptions);
		}

		public static DataContext Create(Type type)
		{
			var defaultConnectionStringName = GetDefaultConnectionString(type);
			var defaultCommandTimeout = GetDefaultCommandTimeout(type);

			return Create(type, defaultConnectionStringName, defaultCommandTimeout);
		}
		public static DataContext Create(Type type, String connectionStringOrName)
		{
			var defaultCommandTimeout = GetDefaultCommandTimeout(type);

			return Create(type, connectionStringOrName, defaultCommandTimeout, null);
		}
		public static DataContext Create(Type type, int commandTimeout)
		{
			var defaultConnectionStringName = GetDefaultConnectionString(type);

			return Create(type, defaultConnectionStringName, commandTimeout);
		}
		public static DataContext Create(Type type, DataLoadOptions loadOptions)
		{
			var defaultConnectionStringName = GetDefaultConnectionString(type);
			var defaultCommandTimeout = GetDefaultCommandTimeout(type);

			return Create(type, defaultConnectionStringName, defaultCommandTimeout, loadOptions);
		}
		public static DataContext Create(Type type, String connectionStringOrName, int commandTimeout)
		{
			return Create(type, connectionStringOrName, commandTimeout, null);
		}
		public static DataContext Create(Type type, String connectionStringOrName, DataLoadOptions loadOptions)
		{
			var defaultCommandTimeout = GetDefaultCommandTimeout(type);

			return Create(type, connectionStringOrName, defaultCommandTimeout, loadOptions);
		}
		public static DataContext Create(Type type, int commandTimeout, DataLoadOptions loadOptions)
		{
			var defaultConnectionStringName = GetDefaultConnectionString(type);

			return Create(type, defaultConnectionStringName, commandTimeout, loadOptions);
		}
		public static DataContext Create(Type type, String connectionStringOrName, int commandTimeout, DataLoadOptions loadOptions)
		{
			var connectionString = GetConnectionString(connectionStringOrName);

			var dataContext = (DataContext)Activator.CreateInstance(type, connectionString);
			dataContext.CommandTimeout = commandTimeout;

			if (loadOptions != null)
			{
				dataContext.LoadOptions = loadOptions;
			}

			return dataContext;
		}

		private static String GetDefaultConnectionString(Type type)
		{
			var settingName = String.Format("{0}.ConnectionStringName", type.Name);

			var defaultConnectionStringName = ConfigurationManager.AppSettings[settingName];
			return defaultConnectionStringName;
		}

		private static int GetDefaultCommandTimeout(Type type)
		{
			var settingName = String.Format("{0}.CommandTimeout", type.Name);

			var defaultCommandTimeout = ConfigurationManager.AppSettings[settingName];

			int commndTimeout;
			if (int.TryParse(defaultCommandTimeout, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out commndTimeout))
			{
				return commndTimeout;
			}

			return 600;
		}

		private static String GetConnectionString(String connectionStringOrName)
		{
			var connStringSettings = ConfigurationManager.ConnectionStrings[connectionStringOrName];
			if (connStringSettings != null)
			{
				return connStringSettings.ConnectionString;
			}

			var connStringBuilder = new SqlConnectionStringBuilder(connectionStringOrName);
			return connStringBuilder.ToString();
		}
	}
}
