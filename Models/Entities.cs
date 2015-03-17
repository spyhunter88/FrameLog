using System;
using System.Collections.Generic;
using FrameLog.Patterns.Models;

namespace FrameLog.Models
{
    public class ChangeSet : IChangeSet<User>
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public User Author { get; set; }
        public string Action { get; set; }
        public virtual List<ObjectChange> ObjectChanges { get; set; }

        IEnumerable<IObjectChange<User>> IChangeSet<User>.ObjectChanges
        {
            get { return ObjectChanges; }
        }

        void IChangeSet<User>.Add(IObjectChange<User> objectChange)
        {
            ObjectChanges.Add((ObjectChange)objectChange);
        }
    }

    public class ObjectChange : IObjectChange<User>
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string ObjectReference { get; set; }
        public string Action { get; set; }
        public virtual ChangeSet ChangeSet { get; set; }
        public virtual List<PropertyChange> PropertyChanges { get; set; }



        IEnumerable<IPropertyChange<User>> IObjectChange<User>.PropertyChanges
        {
            get { return PropertyChanges; }
        }

        void IObjectChange<User>.Add(IPropertyChange<User> propertyChange)
        {
            PropertyChanges.Add((PropertyChange)propertyChange);
        }
        IChangeSet<User> IObjectChange<User>.ChangeSet
        {
            get { return ChangeSet; }
            set { ChangeSet = (ChangeSet)value; }
        }
    }

    public class PropertyChange : IPropertyChange<User>
    {
        public int Id { get; set; }
        public virtual ObjectChange ObjectChange { get; set; }
        public string PropertyName { get; set; }
        public string OldValue { get; set; }
        public string Value { get; set; }
        public int? ValueAsInt { get; set; }

        IObjectChange<User> IPropertyChange<User>.ObjectChange
        {
            get { return ObjectChange; }
            set { ObjectChange = (ObjectChange)value; }
        }
    }

    public class ChangeSetFactory : IChangeSetFactory<ChangeSet, User>
    {
        public ChangeSet ChangeSet()
        {
            var set = new ChangeSet();
            set.ObjectChanges = new List<ObjectChange>();
            return set;
        }

        public IObjectChange<User> ObjectChange()
        {
            var o = new ObjectChange();
            o.PropertyChanges = new List<PropertyChange>();
            return o;
        }

        public IPropertyChange<User> PropertyChange()
        {
            return new PropertyChange();
        }
    }
}
