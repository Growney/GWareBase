using Gware.Common.Storage.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Business.Commands
{
    public class TokenCommandFactory : DataCommandFactory
    {
        public TokenCommandFactory()
            : base("Token", true)
        {

        }

        public static DataCommand SaveToken(int id,DateTime expiry,string key,DateTime created)
        {
            DataCommand retVal = new TokenCommandFactory().CreateCommand("Save");
            retVal.AddParameter("Id", System.Data.DbType.Int32).Value = id;
            retVal.AddParameter("Expiry", System.Data.DbType.DateTime).Value = expiry;
            retVal.AddParameter("Key", System.Data.DbType.String).Value = key;
            retVal.AddParameter("Created", System.Data.DbType.DateTime).Value = created;

            return retVal;
        }
        
        
    }
}
