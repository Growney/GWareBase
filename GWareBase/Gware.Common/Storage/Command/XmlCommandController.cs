using Gware.Common.Storage.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gware.Common.Storage.Adapter;

namespace Gware.Common.Storage.Command
{
    public abstract class XmlCommandController : ICommandController
    {

        public string Directory { get; set; }
        public string Filename { get; set; }

        public bool Exists
        {
            get
            {
                return System.IO.File.Exists(System.IO.Path.Combine(Directory, Filename));
            }
        }

        public XmlCommandController()
        {

        }
        public XmlCommandController(string directory,string filename)
        {
            Directory = directory;
            Filename = filename;
        }


        public IDataAdapterCollection ExecuteCollectionCommand(IDataCommand command)
        {
            throw new NotImplementedException();
        }

        public IDataAdapterCollectionGroup ExecuteGroupCommand(IDataCommand command)
        {
            throw new NotImplementedException();
        }

        public int ExecuteQuery(IDataCommand command)
        {
            throw new NotImplementedException();
        }

        public string GetInitialisationString()
        {
            return $"{Directory}#{Filename}";
        }

        public void Initialise(string initialisationString)
        {
            string[] split = initialisationString.Split('#');
            if(split.Length > 1)
            {
                Directory = split[0];
                Filename = split[1];
            }
        }

        public abstract ICommandController Clone();

        public void SetName(string name)
        {
            Filename = name;
        }
    }
}
