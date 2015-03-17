using FrameLog.Exceptions;
using FrameLog.Patterns;
using System;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Reflection;
using System.Data.Entity.Infrastructure;

namespace FrameLog.Helpers
{
    /// <summary>
    /// This class replace some extra function in ObjectContextAdapter,
    /// to get information from IDataContext instead of inherit 
    /// IFrameLogContext, just pass IDataContext into it
    /// </summary>
    public class DataContextHelper
    {
        public static object GetObjectByKey(DbContext context, EntityKey key)
        {
            return ((IObjectContextAdapter)context).ObjectContext.GetObjectByKey(key);
        }
        public static string KeyPropertyName
        {
            get { return "Id"; }
        }
        public static object KeyFromReference(string reference)
        {
            return int.Parse(reference);
        }
        public static object GetObjectByReference(DbContext context, Type type, string reference)
        {
            try
            {
                var container = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace.GetEntityContainer(
                            ((IObjectContextAdapter)context).ObjectContext.DefaultContainerName, DataSpace.CSpace);
                var set = container.BaseEntitySets.FirstOrDefault(meta => meta.ElementType.Name == type.Name);
                if (set == null)
                    throw new ObjectTypeDoesNotExistInDataModelException(type);

                var key = new EntityKey(container.Name + "." + set.Name, KeyPropertyName, KeyFromReference(reference));
                return GetObjectByKey(context, key);
            }
            catch (Exception e)
            {
                throw new FailedToRetrieveObjectException(type, reference, e);
            }
        }
        public static string GetReferenceForObject(object entity)
        {
            if (entity == null)
                return null;

            IHasLoggingReference entityWithReference = entity as IHasLoggingReference;
            if (entityWithReference != null)
                return entityWithReference.Reference.ToString();

            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.IgnoreCase;
            string keyPropertyName = GetReferencePropertyForObject(entity);
            var keyProperty = entity.GetType().GetProperty(keyPropertyName, flags);
            if (keyProperty != null)
                return keyProperty.GetGetMethod().Invoke(entity, null).ToString();

            throw new Exception(string.Format("Attempted to log a foreign entity that did not implement IHasLoggingReference and that did not have a property with name '{0}'. It had type {1}, and it was '{2}'",
                    KeyPropertyName, entity.GetType(), entity));
        }
        public static string GetReferencePropertyForObject(object entity)
        {
            return KeyPropertyName;
        }

        public static bool ObjectHasReference(object entity)
        {
            if (entity == null)
                return false;

            IHasLoggingReference entityWithReference = entity as IHasLoggingReference;
            if (entityWithReference != null)
                return true;

            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.IgnoreCase;
            string keyPropertyName = GetReferencePropertyForObject(entity);
            var keyProperty = entity.GetType().GetProperty(keyPropertyName, flags);
            if (keyProperty != null)
                return true;

            return false;
        }
    }
}
