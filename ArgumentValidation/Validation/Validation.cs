
namespace Validation
{
    using System;
    using System.Collections.Generic;

    //This is testing the code from
    //http://blog.getpaint.net/2008/12/06/a-fluent-approach-to-c-parameter-validation/   

    public static class Validate
    {
        public static Validation Begin()
        {
            return null;
        }
    }

    public sealed class Validation
    {
        private readonly List<Exception> exceptions;

        public IEnumerable<Exception> Exceptions
        {
            get
            {
                return exceptions;
            }
        }

        public Validation AddException(Exception ex)
        {
            lock (exceptions)
            {
                exceptions.Add(ex);
            }
            return this;
        }

        public Validation()
        {
            exceptions = new List<Exception>(1); // optimize for only having 1 exception
        }       
    }
}