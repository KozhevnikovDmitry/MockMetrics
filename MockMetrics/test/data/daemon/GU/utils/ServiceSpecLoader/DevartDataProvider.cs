using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

using BLToolkit.Aspects;
using BLToolkit.Common;
using BLToolkit.Mapping;
using BLToolkit.Reflection;


namespace BLToolkit.Data.DataProvider
{
    using Sql.SqlProvider;
    using Devart.Data.Oracle;

    /// <summary>
    /// Implements access to the Data Provider for Oracle.
    /// </summary>
    /// <remarks>
    /// See the <see cref="DbManager.AddDataProvider(DataProviderBase)"/> method to find an example.
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
            //con.Direct = true;
            //return con;
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
