using System;
using System.Collections.Generic;
using System.Linq;
using FrameLog.Patterns.Models;
using FrameLog.Translation.Serializers;

namespace FrameLog.Logging
{
    public class DeferredObjectChange<TPrincipal>
    {
        private readonly IObjectChange<TPrincipal> objectChange;
        private readonly DeferredValue futureReference;
        private readonly DeferredValueMap futureValues;
        private readonly ISerializationManager serializer;

        public DeferredObjectChange(IObjectChange<TPrincipal> objectChange, Func<string> deferredReference, ISerializationManager serializer)
        {
            this.objectChange = objectChange;
            this.futureReference = new DeferredValue(deferredReference);
            this.futureValues = new DeferredValueMap(objectChange);
            this.serializer = serializer;
        }

        public void Bake()
        {
            objectChange.ObjectReference = (string)futureReference.CalculateAndRetrieve();

            var bakedValues = futureValues.CalculateAndRetrieve();
            foreach (KeyValuePair<string, object> kv in bakedValues)
            {
                var propertyChange = objectChange.PropertyChanges.SingleOrDefault(pc => pc.PropertyName == kv.Key);
                setValue(propertyChange, kv.Value);
            }
        }
        private void setValue(IPropertyChange<TPrincipal> propertyChange, object value)
        {
            string valueAsString = valueToString(value);
            int valueAsInt;

            propertyChange.Value = valueAsString;
            if (int.TryParse(propertyChange.Value, out valueAsInt))
            {
                propertyChange.ValueAsInt = valueAsInt;
            }
        }
        private string valueToString(object value)
        {
            if (value == null)
                return null;
            else if (serializer != null)
                return serializer.Serialize(value);
            else
                return value.ToString();
        }

        public IObjectChange<TPrincipal> ObjectChange
        {
            get { return objectChange; }
        }
        public DeferredValue FutureReference
        {
            get { return futureReference; }
        }
        public DeferredValueMap FutureValues
        {
            get { return futureValues; }
        }
    }
}
