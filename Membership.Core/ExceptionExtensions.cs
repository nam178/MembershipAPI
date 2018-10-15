using System;
using System.Text;

namespace Membership.Core
{
    public static class ExceptionExtensions
    {
        public static string ToStringWithInnerExceptions(this Exception ex)
        {
            var t = new StringBuilder();
            while(ex != null)
            {
                t.AppendLine(ex.ToString());
                ex = ex.InnerException;
            }
            return t.ToString();
        }
    }
}
