using System;
using System.Data.Entity;
using FrameLog.Patterns.Logging.ValuePairs;

namespace FrameLog.Logging.ValuePairs
{
    internal class ValuePair : IValuePair
    {
        protected readonly object originalValue;
        protected readonly object newValue;
        protected readonly string propertyName;
        protected readonly EntityState state;

        internal ValuePair(object originalValue, object newValue, string propertyName, EntityState state)
        {
            this.originalValue = get(originalValue);
            this.newValue = get(newValue);
            this.propertyName = propertyName;
            this.state = state;
        }

        private object get(object value)
        {
            if (value is DBNull)
                return null;
            return value;
        }

        internal IChangeType Type
        {
            get
            {
                var value = originalValue ?? newValue;
                return value.GetChangeType();
            }
        }

        public bool HasChanged
        {
            get
            {
                return state == EntityState.Added
                    || state == EntityState.Deleted
                    || !object.Equals(newValue, originalValue);
            }
        }

        public string PropertyName
        {
            get { return propertyName; }
        }

        public object NewValue
        {
            get { return newValue; }
        }

        public object OriginalValue
        {
            get { return originalValue; }
        }

        public EntityState State
        {
            get { return state; }
        }
    }
}