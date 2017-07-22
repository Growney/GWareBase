using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Business.Entity.Collection
{
    internal enum EntityRelationship
    {
        Parent,
        Child
    }

    interface IMultiEntityTypeStorage : IEnumerable<EntityBase>
    {
        EntityBase Relation { get; set; }
        EntityRelationship Relationship { get; set; }

        void Add<T>(IEnumerable<T> items) where T : EntityBase;
        void Add<T>(T item) where T : EntityBase;
        void Set<T>(int index, int entityTypeID, T item) where T : EntityBase;
        T Get<T>(int index, int entityTypeID) where T : EntityBase, new();
        List<T> Get<T>(int entityTypeID) where T : EntityBase, new();
        bool Exists<T>(int entityTypeID, int index) where T : EntityBase, new();

    }
}
