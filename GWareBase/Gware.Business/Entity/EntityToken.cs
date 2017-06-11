using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gware.Common.Storage.Adapter;
using Gware.Common.Storage.Command;
using Gware.Business.Commands;
using Gware.Common.Application;
using Gware.Common.Security;

namespace Gware.Business.Entity
{
    public abstract class EntityToken : EntityBase
    {
        private DateTime m_expiry;
        private string m_key;
        private DateTime m_created;

        public bool Expired
        {
            get
            {
                return m_expiry < DateTime.UtcNow;
            }
        }

        public DateTime Expiry
        {
            get { return m_expiry; }
            protected set { m_expiry = value; }
        }

        public string Key
        {
            get { return m_key; }
            protected set { m_key = value; }
        }

        public DateTime Created
        {
            get { return m_created; }
            protected set { m_created = value; }
        }

        public EntityToken()
        {

        }

        protected override void OnLoadFrom(IDataAdapter adapter)
        {
            m_expiry = adapter.GetValue("Expiry", DateTime.MinValue);
            m_key = adapter.GetValue("Key", string.Empty);
            m_created = adapter.GetValue("Created", DateTime.MinValue);
        }

        

        public override DataCommand CreateSaveCommand()
        {
            return TokenCommandFactory.SaveToken(Id, Expiry, Key, Created);
        }
        

    }
}
