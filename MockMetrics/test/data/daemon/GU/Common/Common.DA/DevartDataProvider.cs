using System;
using System.Data;
using System.Data.Common;
using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Sql.SqlProvider;
using Devart.Data.Oracle;
using DataException = BLToolkit.Data.DataException;

namespace Common.DA
{
    /// <summary>
	/// Implements access to the Data Provider for Oracle.
	/// </summary>
	/// <remarks>
	/// See the <see cref="DbManager.AddDataProvider(BLToolkit.Data.DataProvider.DataProviderBase)"/> method to find an example.
	/// </remarks>
	/// <seealso cref="DbManager.AddDataProvider(DataProviderBase)">AddDataManager Method</seealso>
	public class DevartDataProvider : DataProviderBase
	{
        public override Type ConnectionType
        {
            get { return typeof(OracleConnection); }
        }

        public override string Name
        {
            get { return "Devart"; }
        }

        public override IDbConnection CreateConnectionObject()
        {
            return new OracleConnection();
        }

        public override DbDataAdapter CreateDataAdapterObject()
        {
            return new OracleDataAdapter();
        }

        public override bool DeriveParameters(IDbCommand command)
        {
            var oraCommand = command as OracleCommand;

            if (null != oraCommand)
            {
                try
                {
                    OracleCommandBuilder.DeriveParameters(oraCommand);
                }
                catch (Exception ex)
                {
                    // Make Oracle less laconic.
                    //
                    throw new DataException(string.Format("{0}\nCommandText: {1}", ex.Message, oraCommand.CommandText), ex);
                }

                return true;
            }

            return false;
        }

        public override ISqlProvider CreateSqlProvider()
        {
            return new OracleSqlProvider();
        }
    }
}
