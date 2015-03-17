using System.Collections.Generic;

namespace FrameLog.Patterns.Models
{
    public interface IObjectChange<TPrincipal>
    {
        IChangeSet<TPrincipal> ChangeSet { get; set; }
        IEnumerable<IPropertyChange<TPrincipal>> PropertyChanges { get; }
        void Add(IPropertyChange<TPrincipal> propertyChange);

        string TypeName { get; set; }
        string ObjectReference { get; set; }        
    }
}
