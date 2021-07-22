using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ExpanseManagerServiceLibrary.Exceptions
{
    public class UnknownExchangeRateException : Exception
    {
        public UnknownExchangeRateException()
        {
        }

        public UnknownExchangeRateException(string message) : base(message)
        {
        }

        public UnknownExchangeRateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnknownExchangeRateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
