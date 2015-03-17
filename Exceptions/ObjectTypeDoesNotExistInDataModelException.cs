using System;

namespace FrameLog.Exceptions
{
    public class ObjectTypeDoesNotExistInDataModelException : Exception
    {
        public readonly Type Type;

        public const string MessageTemplate = @"Objects of type '{0}' do not exist in the data model.
It is possible that you have removed a class/table from your database without updating existing log tables.
Alternatively, '{0}' may be a 'primitive' datatype, rather than a foreign key relation, in which case this is FrameLog's fault. You can raise an issue with the open source project, or you can patch this yourself by extending BindManager. See https://bitbucket.org/MartinEden/framelog/wiki/HistoryExplorerBinding";

        public ObjectTypeDoesNotExistInDataModelException(Type type)
            : base(string.Format(MessageTemplate, type))
        {
            Type = type;
        }
    }
}
