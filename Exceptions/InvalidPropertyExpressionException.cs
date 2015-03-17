using System;
using System.Linq.Expressions;

namespace FrameLog.Exceptions
{
    public class InvalidPropertyExpressionException : Exception
    {
        private Expression lambda;

        public InvalidPropertyExpressionException(Expression lambda)
            : base(string.Format("The expression '{0}' was not a property accessor expression. It must be in the form x => x.a, x => x.a.b, etc., where 'a' and 'b' properties, not fields or methods.", lambda))
        {
            this.lambda = lambda;
        }
    }
}
