using System;
using System.Data.Common;
using System.Runtime.Serialization;

namespace ExpanseManagerServiceLibrary.Exceptions
{
    [Serializable]
    public class CouldntCreateCurrencyException : Exception
    {
        private DbException ex;

        public CouldntCreateCurrencyException()
        {
        }

        public CouldntCreateCurrencyException(DbException ex)
        {
            this.ex = ex;
        }

        public CouldntCreateCurrencyException(string message) : base(message)
        {
        }

        public CouldntCreateCurrencyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CouldntCreateCurrencyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}