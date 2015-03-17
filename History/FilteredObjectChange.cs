
using System;
using System.Collections.Generic;
using FrameLog.Patterns.Models;

namespace FrameLog.History
{
    /// <summary>
    /// Re-present an IObjectChange with only some of the property changes.
    /// This is used when reconstructing the history of only some properties.
    /// </summary>
    public class FilteredObjectChange<TPrincipal> : IObjectChange<TPrincipal>
    {
        private IObjectChange<TPrincipal> objectChange;
        private IEnumerable<IPropertyChange<TPrincipal>> propertyChanges;

        public FilteredObjectChange(IObjectChange<TPrincipal> objectChange, IEnumerable<IPropertyChange<TPrincipal>> propertyChanges)
        {
            this.objectChange = objectChange;
            this.propertyChanges = propertyChanges;
        }

        public IChangeSet<TPrincipal> ChangeSet
        {
            get { return objectChange.ChangeSet; }
            set { objectChange.ChangeSet = value; }
        }

        public IEnumerable<IPropertyChange<TPrincipal>> PropertyChanges
        {
            get { return propertyChanges; }
        }

        public void Add(IPropertyChange<TPrincipal> propertyChange)
        {
            throw new NotImplementedException();
        }

        public string TypeName
        {
            get { return objectChange.TypeName; }
            set { objectChange.TypeName = value; }
        }

        public string ObjectReference
        {
            get { return objectChange.ObjectReference; }
            set { objectChange.ObjectReference = value; }
        }
    }
}
