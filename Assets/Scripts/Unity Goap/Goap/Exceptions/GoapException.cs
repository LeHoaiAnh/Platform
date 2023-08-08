using System;

namespace Goap.Exceptions
{
    public class GoapException : Exception
    {
        public GoapException(string message) : base(message)
        {
        }
    }
}