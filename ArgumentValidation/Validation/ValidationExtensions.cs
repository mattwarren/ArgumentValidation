
namespace Validation
{
    using Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class ValidationExtensions
    {
        public static Validation IsNotNull<T>(this Validation validation, T theObject, string paramName)
            where T : class
        {
            if (theObject == null)
                return (validation ?? new Validation()).AddException(new ArgumentNullException(paramName));
            else
                return validation;
        }

        public static Validation IsPositive(this Validation validation, long value, string paramName)
        {
            if (value < 0)
                return (validation ?? new Validation()).AddException(new ArgumentOutOfRangeException(paramName, "must be positive, but was " + value.ToString()));
            else
                return validation;
        }

        public static Validation IsGreaterThanZero(this Validation validation, long value, string paramName)
        {
            if (value <= 0)
                return (validation ?? new Validation()).AddException(new ArgumentOutOfRangeException(paramName, "must be greater than zero, but was " + value.ToString()));
            else
                return validation;
        }

        public static Validation IsIndexInRange<T>(this Validation validation, T[] array, long offset, string paramName)
        {
            if (offset < 0 || offset >= array.Length)
            {
                return (validation ?? new Validation()).AddException(
                    new ArgumentOutOfRangeException(paramName,
                                                    string.Format("must be > 0 and < {0}, but was {1}", array.Length,
                                                                  offset)));
            }
            else
                return validation;
        }        

        public static Validation Check(this Validation validation)
        {            
            if (validation == null)
                return validation;

            const string message = "Validation Failures:";
            if (validation.Exceptions.Take(2).Count() == 1)
            {
                // ValidationException is just a standard Exception-derived class with the usual four constructors
                throw new ValidationException(message, validation.Exceptions.First());
            }
            else
            {
                // implementation shown below
                throw new ValidationException(message, new MultiException(validation.Exceptions));
            }
        }

        public static Boolean AnyNull<T>(this IEnumerable<T> ie) where T : class
        {
            foreach (var x in ie)
                if (x == null) return true;
            return false;
        }

        public static IEnumerable<string> GetAllExceptionMessages(this ValidationException exception)
        {
            if (exception.InnerException is MultiException)
            {
                foreach (var except in (exception.InnerException as MultiException).InnerExceptions)
                    yield return except.ToString();
            }
            else
            {
                yield return exception.InnerException.ToString();
            }
        }
    }
}


