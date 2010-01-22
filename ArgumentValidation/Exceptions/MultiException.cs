
namespace Validation.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    [Serializable]
    public sealed class MultiException
        : Exception
    {
        private readonly Exception[] innerExceptions;

        public IEnumerable<Exception> InnerExceptions
        {
            get
            {
                if (innerExceptions != null)
                {
                    foreach (Exception t in innerExceptions)
                    {
                        yield return t;
                    }
                }
            }
        }

        public MultiException()
            : base()
        {
        }

        public MultiException(string message)
            : base()
        {
        }

        public MultiException(string message, Exception innerException)
            : base(message, innerException)
        {
            this.innerExceptions = new Exception[1] { innerException };
        }

        public MultiException(IEnumerable<Exception> innerExceptions)
            : this(null, innerExceptions)
        {
        }

        public MultiException(Exception[] innerExceptions)
            : this(null, (IEnumerable<Exception>)innerExceptions)
        {
        }

        public MultiException(string message, Exception[] innerExceptions)
            : this(message, (IEnumerable<Exception>)innerExceptions)
        {
        }

        public MultiException(string message, IEnumerable<Exception> innerExceptions)
            : base(message, innerExceptions.FirstOrDefault())
        {
            if (innerExceptions.AnyNull())
            {
                throw new ArgumentNullException();
            }

            this.innerExceptions = innerExceptions.ToArray();
        }

        private MultiException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}