using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ExpanseManagerServiceLibrary.Exceptions
{
    public class InsufficientBallanceException : Exception
    {
        public InsufficientBallanceException()
        {
        }

        public InsufficientBallanceException(string message) : base(message)
        {
        }

        public InsufficientBallanceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InsufficientBallanceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
