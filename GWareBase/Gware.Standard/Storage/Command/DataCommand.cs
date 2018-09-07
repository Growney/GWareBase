using Gware.Standard.Collections.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Gware.Standard.Storage.Command
{
    public class DataCommand : IDataCommand
    {
        private Dictionary<string, IDataCommandParameter> m_parameters = new Dictionary<string, IDataCommandParameter>();
        public string CommandMethod { get; }
        public string Name { get; }
        public Exception Exception { get; set; }
        public bool Success { get; set; }
        public int ParameterCount
        {
            get
            {
                return m_parameters.Count;
            }
        }
        

        public IDataCommandParameter this[string name]
        {
            get
            {
                return GetParameter(name);
            }
        }
        
        public DataCommand(string commandName, string commandMethod)
        {
            Name = commandName;
            CommandMethod = commandMethod;
        }
        public IDataCommandParameter AddParameter(string name, DbType dataType)
        {
            return AddParameter(new DataCommandParameter(name, null, dataType));
        }
        public IDataCommandParameter AddParameter(IDataCommandParameter param)
        {
            string nameLower = param.Name.ToLower();
            if (GetParameter(nameLower) == null)
            {
                m_parameters.Set(nameLower, param);
                return param;
            }
            else
            {
                throw new ArgumentException("An element with the same key already exists in the collection");
            }
        }
        public IDataCommandParameter AddParameter(string name, DbType dataType, ParameterDirection direction)
        {
            return AddParameter(new DataCommandParameter(name, null, dataType, direction));
        }
        public IDataCommandParameter AddParameter(string name, string datatypeName)
        {
            return AddParameter(new DataCommandParameter(name, null, DbType.Object, ParameterDirection.Input));
        }



        public object GetParameterValue(string name)
        {
            return GetParameter(name).Value;
        }

        public IDataCommandParameter GetParameter(string name)
        {
            return m_parameters.Get(name);
        }

        public void SetParameter(string name, object value)
        {
            GetParameter(name).Value = value;
        }

        public IEnumerator<IDataCommandParameter> GetEnumerator()
        {
            foreach (string key in m_parameters.Keys)
            {
                yield return m_parameters[key];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        
    }
}
