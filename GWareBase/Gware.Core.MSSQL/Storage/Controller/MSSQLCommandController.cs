using Gware.Core.MSSQL.Database;
using Gware.Standard.Storage.Adapter;
using Gware.Standard.Storage.Adapter.Data;
using Gware.Standard.Storage.Command;
using Gware.Standard.Storage.Controller;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Core.MSSQL.Storage.Controller
{
    public class MSSQLCommandController : ICommandController
    {
        private const char c_storedProcedurePrefix = 'p';
        private const char c_parameterPrefix = '@';
        public string Name
        {
            get
            {
                return DatabaseName;
            }
        }
        public string ServerName { get; private set; }
        public string DatabaseName { get; set; }
        public bool Trusted { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public bool Encrypted { get; set; }
        public int TimeOut { get; set; }
        public MSSQLCommandController()
        {

        }
        public MSSQLCommandController(string serverName, string databaseName, string databaseUsername, string databasePassword)
        {
            ServerName = serverName;
            DatabaseName = databaseName;
            Username = databaseUsername;
            Password = databasePassword;
        }
        public MSSQLCommandController(string serverName, string databaseName)
        {
            Trusted = true;
            ServerName = serverName;
            DatabaseName = databaseName;
        }

        public Task<IDataAdapterCollectionGroup> ExecuteGroupCommandAsync(IDataCommand command)
        {
            return Task<IDataAdapterCollectionGroup>.Factory.StartNew(() =>
            {
                return ExecuteGroupCommand(command);
            });
        }

        public Task<IDataAdapterCollection> ExecuteCollectionCommandAsync(IDataCommand command)
        {
            return Task<IDataAdapterCollection>.Factory.StartNew(() =>
            {
                return ExecuteCollectionCommand(command);
            });
        }

        public Task<int> ExecuteQueryAsync(IDataCommand command)
        {
            return Task<int>.Factory.StartNew(() =>
            {
                return ExecuteQuery(command);
            });
        }

        public IDataAdapterCollection ExecuteCollectionCommand(IDataCommand command)
        {
            return new DataTableDataAdapter(this, ExecuteTableQuery(CreateStoredProcedureFromCommand(command)));
        }

        public IDataAdapterCollectionGroup ExecuteGroupCommand(IDataCommand command)
        {
            return new DataSetDataAdapter(this, ExecuteQuery(CreateStoredProcedureFromCommand(command)));
        }

        public int ExecuteQuery(IDataCommand command)
        {
            return ExecuteNonQuery(CreateStoredProcedureFromCommand(command));
        }

        public static StoredProcedure CreateStoredProcedureFromCommand(IDataCommand command)
        {
            StoredProcedure retVal = new StoredProcedure(GetDatabaseStoredProcedureName(command.Name));
            AddParameterToStoredProcedure(retVal, "Result", command.CommandMethod);
            int parameterCount = command.ParameterCount;
            foreach (IDataCommandParameter param in command)
            {
                AddParameterToStoredProcedure(retVal, param);
            }
            return retVal;
        }
        public static void AddParameterToStoredProcedure(StoredProcedure procedure, string name, object value)
        {
            AddParameterToStoredProcedure(procedure, name, value, DbType.String);
        }
        public static void AddParameterToStoredProcedure(StoredProcedure procedure, string name, object value, DbType type)
        {
            AddParameterToStoredProcedure(procedure, name, value, type, ParameterDirection.Input);
        }
        public static void AddParameterToStoredProcedure(StoredProcedure procedure, string name, object value, DbType type, ParameterDirection direction)
        {
            AddParameterToStoredProcedure(procedure, new DataCommandParameter(name, value, type, direction));
        }
        public static void AddParameterToStoredProcedure(StoredProcedure procedure, IDataCommandParameter command)
        {
            string dbCommandName = GetDatabaseParameterName(command.Name);
            procedure.AddParameter(dbCommandName, ConvertToSqlDBType(command.DataType), command.DataTypeName, command.Direction);
            procedure.SetParameterValue(dbCommandName, command.Value);
        }

        public static SqlDbType ConvertToSqlDBType(DbType dbType)
        {
            SqlDbType retVal;
            switch (dbType)
            {
                case DbType.AnsiString:
                    retVal = SqlDbType.VarChar;
                    break;
                case DbType.Binary:
                    retVal = SqlDbType.Binary;
                    break;
                case DbType.Byte:
                    retVal = SqlDbType.TinyInt;
                    break;
                case DbType.Boolean:
                    retVal = SqlDbType.Bit;
                    break;
                case DbType.Currency:
                    retVal = SqlDbType.Money;
                    break;
                case DbType.Date:
                    retVal = SqlDbType.Date;
                    break;
                case DbType.DateTime:
                    retVal = SqlDbType.DateTime;
                    break;
                case DbType.Decimal:
                    retVal = SqlDbType.Decimal;
                    break;
                case DbType.Double:
                    retVal = SqlDbType.Float;
                    break;
                case DbType.Guid:
                    retVal = SqlDbType.UniqueIdentifier;
                    break;
                case DbType.Int16:
                    retVal = SqlDbType.SmallInt;
                    break;
                case DbType.Int32:
                    retVal = SqlDbType.Int;
                    break;
                case DbType.Int64:
                    retVal = SqlDbType.BigInt;
                    break;
                case DbType.SByte:
                    retVal = SqlDbType.TinyInt;
                    break;
                case DbType.Single:
                    retVal = SqlDbType.Float;
                    break;
                case DbType.String:
                    retVal = SqlDbType.VarChar;
                    break;
                case DbType.Time:
                    retVal = SqlDbType.Time;
                    break;
                case DbType.UInt16:
                    retVal = SqlDbType.Int;
                    break;
                case DbType.UInt32:
                    retVal = SqlDbType.BigInt;
                    break;
                case DbType.UInt64:
                    retVal = SqlDbType.Float;
                    break;
                case DbType.AnsiStringFixedLength:
                    retVal = SqlDbType.VarChar;
                    break;
                case DbType.StringFixedLength:
                    retVal = SqlDbType.VarChar;
                    break;
                case DbType.Xml:
                    retVal = SqlDbType.Xml;
                    break;
                case DbType.DateTime2:
                    retVal = SqlDbType.DateTime2;
                    break;
                case DbType.DateTimeOffset:
                    retVal = SqlDbType.DateTimeOffset;
                    break;
                case DbType.Object:
                    retVal = SqlDbType.Structured;
                    break;
                case DbType.VarNumeric:
                default:
                    throw new NotSupportedException();
            }
            return retVal;
        }
        public static string GetDatabaseParameterName(string parameterName)
        {
            string retVal = parameterName;

            if (parameterName.Length > 0)
            {
                if (parameterName[0] != c_parameterPrefix)
                {
                    retVal = c_parameterPrefix + parameterName;
                }
            }

            return retVal;
        }
        public static string GetDatabaseStoredProcedureName(string commandName)
        {
            string retVal = commandName;

            if (retVal.Length > 0)
            {
                if (retVal[0] != c_storedProcedurePrefix)
                {
                    retVal = c_storedProcedurePrefix + retVal;
                }
            }
            return retVal;
        }

        public string GetInitialisationString()
        {
            return $"{ServerName} {DatabaseName} {Trusted} {Username} {Password}";
        }

        public void Initialise(string initialisationString)
        {
            string[] splits = initialisationString.Split(' ');
            if (splits.Length > 3)
            {
                bool trusted = splits[2].ToLower() == "true";
                if (trusted)
                {
                    ServerName = splits[0];
                    DatabaseName = splits[1];
                    Trusted = trusted;
                }
                if (splits.Length > 4)
                {
                    ServerName = splits[0];
                    DatabaseName = splits[1];
                    Trusted = trusted;
                    Username = splits[3];
                    Password = splits[4];
                }
            }

        }

        public ICommandController Clone()
        {
            if (Trusted)
            {
                return new MSSQLCommandController(ServerName, DatabaseName);
            }
            else
            {
                return new MSSQLCommandController(ServerName, DatabaseName, Username, Password);
            }
        }

        public Task<bool> DeploySchema(string schemaFile, string dbName, bool includeComposite = false)
        {
            TaskCompletionSource<bool> source = new TaskCompletionSource<bool>();
            try
            {
                var process = new System.Diagnostics.Process();
                var startInfo = new System.Diagnostics.ProcessStartInfo
                {
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                    FileName = @"C:\Program Files (x86)\Microsoft SQL Server\140\DAC\bin\SqlPackage.exe",
                    Arguments = $"/Action:Publish  /SourceFile:\"{schemaFile}\" /TargetConnectionString:\"{GetConnectionString()}\" /Properties:IncludeCompositeObjects={includeComposite}"
                };
                process.StartInfo = startInfo;
                process.EnableRaisingEvents = true;
                process.Exited += (x, y) =>
                {
                    if (process.ExitCode == 0)
                    {
                        source.SetResult(true);
                    }
                    else
                    {
                        source.SetResult(false);
                    }
                };
                process.Start();
                if (process.HasExited)
                {
                    source.SetResult(false);
                }
                #region ---- Awaiting .NET Core 2.0 Support -----
                //Microsoft.SqlServer.Dac.DacServices dacServices = new Microsoft.SqlServer.Dac.DacServices(Connection.GetConnectionString());
                //dacServices.ProgressChanged += (x, y) =>
                //{
                //    switch (y.Status)
                //    {
                //        case Microsoft.SqlServer.Dac.DacOperationStatus.Completed:
                //            source.SetResult(true);
                //            break;
                //        case Microsoft.SqlServer.Dac.DacOperationStatus.Faulted:
                //        case Microsoft.SqlServer.Dac.DacOperationStatus.Cancelled:
                //            source.SetResult(false);
                //            break;
                //        default:
                //            break;
                //    }
                //};

                //Microsoft.SqlServer.Dac.DacPackage package = Microsoft.SqlServer.Dac.DacPackage.Load(schemaFile);

                //Microsoft.SqlServer.Dac.DacDeployOptions options = new Microsoft.SqlServer.Dac.DacDeployOptions()
                //{
                //    SqlCommandVariableValues =
                //    {
                //        new KeyValuePair< string, string >( "debug", "false" )
                //    },
                //    CreateNewDatabase = true,
                //    BlockOnPossibleDataLoss = false,
                //    BlockWhenDriftDetected = false
                //};
                //dacServices.Deploy(package, dbName, upgradeExisting: true, options: options);
                #endregion ---- Awaiting .NET Core 2.0 Support -----
            }
            catch (Exception ex)
            {
                source.SetException(ex);
            }
            return source.Task;
        }

        public int ExecuteNonQuery(StoredProcedure sp)
        {

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand comm = conn.CreateCommand())
                {
                    comm.CommandType = System.Data.CommandType.StoredProcedure;
                    comm.CommandText = sp.Name;
                    foreach (SqlParameter param in sp.Parameters)
                    {
                        comm.Parameters.Add(param);
                    }

                    return comm.ExecuteNonQuery();
                }
            }
        }
        public int ExecuteNonQuery(string query)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand comm = conn.CreateCommand())
                {
                    comm.CommandType = System.Data.CommandType.Text;
                    comm.CommandText = query;
                    return comm.ExecuteNonQuery();
                }
            }
        }
        public int ExecuteNonQuery(string queryFormat, params string[] parameters)
        {
            return ExecuteNonQuery(string.Format(queryFormat, parameters));
        }
        public DataSet ExecuteQuery(string query)
        {
            DataSet retVal = new DataSet();
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand comm = conn.CreateCommand())
                {
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = query;
                    using (SqlDataAdapter reader = new SqlDataAdapter(comm))
                    {
                        reader.Fill(retVal);
                    }
                }
            }
            return retVal;
        }
        public DataTable ExecuteTableQuery(string query)
        {
            return GetDataTable(ExecuteQuery(query));
        }
        public DataTable ExecuteTableQuery(StoredProcedure sp)
        {
            return GetDataTable(ExecuteQuery(sp));
        }
        private DataTable GetDataTable(DataSet data)
        {
            DataTable retVal = null;
            if (data != null)
            {
                if (data.Tables.Count > 0)
                {
                    retVal = data.Tables[0];
                }
            }
            return retVal;
        }
        public DataSet ExecuteQuery(StoredProcedure sp)
        {
            DataSet retVal = new DataSet();
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand comm = conn.CreateCommand())
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.CommandText = sp.Name;
                    foreach (SqlParameter param in sp.Parameters)
                    {
                        comm.Parameters.Add(param);
                    }
                    using (SqlDataAdapter reader = new SqlDataAdapter(comm))
                    {
                        reader.Fill(retVal);
                    }
                }
            }
            return retVal;
        }
        public DataSet ExecuteQuery(string queryFormat, params string[] parameters)
        {
            return ExecuteQuery(string.Format(queryFormat, parameters));
        }
        public SqlConnection GetConnection()
        {
            string connectionString = GetConnectionString();
            return new SqlConnection(connectionString);
        }
        public string GetConnectionString()
        {
            if (Trusted)
            {
                return $"Server={ServerName};Database={DatabaseName};Trusted_Connection=True;Encrypt={Encrypted};Connection Timeout={TimeOut};";
            }
            else
            {
                return $"Server={ServerName};Database={DatabaseName};User id={Username};Password={Password};Trusted_Connection=False;Encrypt={Encrypted};Connection Timeout={TimeOut};";
            }

        }

        public void SetName(string name)
        {
            DatabaseName = name;
        }


    }
}
