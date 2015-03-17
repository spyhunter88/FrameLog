using System;
using System.Data.Entity.Core;

namespace FrameLog.Exceptions
{
    public class ConflictingTransactionException : Exception
    {
        private const string expectedMessageA = "The underlying provider failed on EnlistTransaction.";
        private const string expectedMessageB = "Cannot enlist in the transaction because a local transaction is in progress on the connection.  Finish local transaction and retry.";
        private const string message = @"Attempted to open a transaction on the current connection, but there is already a transaction in progress.
If you are wrapping the call to FrameLog.SaveChanges in an explicit transaction, and not using the TransactionScope API, you will need to pass this transaction in using one of the overloads of FrameLog.SaveChanges.
Alternatively, you can switch to using TransactionScope.
For more information see https://bitbucket.org/MartinEden/framelog/wiki/TransactionOptions.";

        public ConflictingTransactionException(EntityException e)
            : base(message, e) { }

        /// <summary>
        /// Detecting the appropriate exception is messy, because there is not a proper
        /// exception type. We match on the message, which may be unreliable. If it fails,
        /// they will just get the raw EntityException, and have to figure it out from 
        /// there.
        /// </summary>
        public static bool Matches(EntityException e)
        {
            return e.Message == expectedMessageA
                && e.InnerException != null 
                && e.InnerException is InvalidOperationException
                && e.InnerException.Message == expectedMessageB;
        }
    }
}
