using Gware.Common.Client.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gware.Common.Storage.Adapter;
using Gware.Common.Database;
using System.Data;
using Gware.Common.Storage.Command.Interface;

namespace Gware.Common.Storage.Command
{
    public class MSSQLCommandController : MSSQLDBClientBase, ICommandController
    {
        private static int m_commandsExecuted = 0;
        private const char c_storedProcedurePrefix = 'p';
        private const char c_parameterPrefix = '@';

        public MSSQLCommandController()
            :base(string.Empty,string.Empty,string.Empty,string.Empty)
        {

        }
        public MSSQLCommandController(string serverName, string databaseName, string databaseUsername, string databasePassword)
            : base(serverName, databaseName, databaseUsername, databasePassword)
        {

        }

        private void DisplayCommandExcuted()
        {
            m_commandsExecuted++;
        }
        public IDataAdapterCollection ExecuteCollectionCommand(IDataCommand command)
        {
            DisplayCommandExcuted();
            return new DataTableDataAdapter(this,Connection.ExecuteTableQuery(CreateStoredProcedureFromCommand(command)));
        }

        public IDataAdapterCollectionGroup ExecuteGroupCommand(IDataCommand command)
        {
            DisplayCommandExcuted();
            return new DataSetDataAdapter(this,Connection.ExecuteQuery(CreateStoredProcedureFromCommand(command)));
        }

        public int ExecuteQuery(IDataCommand command)
        {
            DisplayCommandExcuted();
            return Connection.ExecuteNonQuery(CreateStoredProcedureFromCommand(command));
        }

        public static StoredProcedure CreateStoredProcedureFromCommand(IDataCommand command)
        {
            StoredProcedure retVal = new StoredProcedure(GetDatabaseStoredProcedureName(command.Name));
            AddParameterToStoredProcedure(retVal, "Result", command.CommandMethod);
            int parameterCount = command.ParameterCount;
            foreach(IDataCommandParameter param in command)
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
            procedure.AddParameter(dbCommandName, ConvertToSqlDBType(command.DataType),command.DataTypeName, command.Direction);
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

            if(retVal.Length > 0)
            {
                if(retVal[0] != c_storedProcedurePrefix)
                {
                    retVal = c_storedProcedurePrefix + retVal;
                }
            }
            return retVal;
        }

        public string GetInitialisationString()
        {
            return $"{Connection.ServerName} {Connection.DatabaseName} {Connection.Username} {Connection.Password}";
        }

        public void Initialise(string initialisationString)
        {
            string[] splits = initialisationString.Split(' ');
            if(splits.Length > 3)
            {
                SetDetails(splits[0], splits[1], splits[2], splits[3]);
            }
        }

        public ICommandController Clone()
        {
            return new MSSQLCommandController(ServerName, DatabaseName, DatabaseUsername, DatabasePassword);
        }

        public void SetName(string name)
        {
            SetDetails(ServerName, name, DatabaseUsername, DatabasePassword);
        }

        public Task<bool> DeploySchema(string schemaFile,string dbName,bool includeComposite = false)
        {
            TaskCompletionSource<bool> source = new TaskCompletionSource<bool>();
            try
            {

                var process = new System.Diagnostics.Process();
                var startInfo = new System.Diagnostics.ProcessStartInfo
                {
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                    FileName = @"C:\Program Files (x86)\Microsoft SQL Server\140\DAC\bin\SqlPackage.exe",
                    Arguments = $"/Action:Publish  /SourceFile:\"{schemaFile}\" /TargetConnectionString:\"{Connection.GetConnectionString()}\" /Properties:IncludeCompositeObjects={includeComposite}"
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
    }
}
