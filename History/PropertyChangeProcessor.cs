using FrameLog.Helpers;
using FrameLog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Reflection;
using FrameLog.Translation.Binders;
using FrameLog.Patterns.Models;

namespace FrameLog.History
{
    /// <summary>
    /// This class applies the change described in an IPropertyChange to its
    /// host object.
    /// 
    /// It can recurse down through property changes that describe complex
    /// property changes (those in the form Foo.Bar.Baz).
    /// </summary>
    public class PropertyChangeProcessor<TPrincipal>
    {
        private IPropertyChange<TPrincipal> wrapped;

        public PropertyChangeProcessor(IPropertyChange<TPrincipal> wrapped)
        {
            this.wrapped = wrapped;
        }

        /// <summary>
        /// Apply the property change to the given model.
        /// So if the IPropertyChange has property name "Foo", then model.Foo
        /// will be set to the bound value of the IPropertyChange's value.
        /// </summary>
        /// <param name="prefix">
        /// The prefix is used to apply a property change to an object that is an intermediate 
        /// part of a complex property change. For example, if you have a property change 
        /// "Foo.Bar", that relates to an object change on an object of type Example. To 
        /// apply the property change directly to the Example object, you would use an empty 
        /// prefix: "". However, if you instead had the object referenced by Example.Foo, you 
        /// could apply the property change directly to Foo, by using the prefix "Foo". Then, 
        /// this method will treat the property change as if it's name were just "Bar", 
        /// removing the "Foo." prefix.
        /// </param>
        public void ApplyTo<TModel>(TModel model, IBindManager binder, string prefix)
        {
            applyTo(model, binder, typeof(TModel), prefix);
        }
        private void applyTo(object model, IBindManager binder, Type modelType, string prefix)
        {
            string remainder = stripPrefix(prefix);
            var propertyParts = split(remainder);
            string propertyName = propertyParts.First();

            var property = modelType.GetProperty(propertyName, BindingFlags.Public 
                | BindingFlags.NonPublic | BindingFlags.Instance);
            if (property != null)
            {
                applyToProperty(model, property, binder, prefix, remainder);
            }
            else
            {
                throw new UnknownPropertyInLogException<TPrincipal>(wrapped);
            }
        }

        private void applyToProperty(object model, PropertyInfo property, IBindManager binder, string prefix, string remainder)
        {
            // If (after taking the prefix into account) this is a complex property change
            // Recurse, using the object referred to by the first part of the property name
            // as the new model to apply the change to.
            if (split(remainder).Count() > 1)
            {
                var value = getValue(model, property);
                applyTo(value, binder, property.PropertyType, ExpressionHelper.Join(prefix, property.Name));
            }
            // Otherwise, if we are now at a simple property change, just bind the value
            // and set the appropriate property
            else
            {
                var existingValue = property.GetValue(model, null);
                var value = binder.Bind(wrapped.Value, property.PropertyType, existingValue);
                if (isEntityCollection(property) && existingValue != null)
                {
                    // do nothing, its contents were already updated by the binder,
                    // and it's an error to use the property setter for an entity collection
                }
                else
                {
                    property.SetValue(model, value, null);
                }
            }
        }

        private bool isEntityCollection(PropertyInfo property)
        {
            var type = property.PropertyType;
            return type.IsGenericType
                && type.GetGenericTypeDefinition() == typeof(EntityCollection<>);
        }

        private string stripPrefix(string prefix)
        {
            return wrapped.PropertyName.Substring(prefix.Length).TrimStart(new char[] { '.' });
        }

        /// <summary>
        /// Gets the value of the property on the object, instantiating it with a default
        /// value if it is currently null.
        /// </summary>
        private object getValue(object model, PropertyInfo containerProperty)
        {
            var value = containerProperty.GetValue(model, null);
            if (value == null)
            {
                value = HistoryHelpers.Instantiate(containerProperty.PropertyType);
                containerProperty.SetValue(model, value, null);
            }
            return value;
        }

        private IEnumerable<string> split(string text)
        {
            return text.Split(new char[] { '.' });
        }

        public override string ToString()
        {
            return wrapped.ToString();
        }
    }
}
