
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
                return validation.AddExceptionEx(new ArgumentNullException(paramName));
            else
                return validation;
        }

        //Positive numbers are those greater than ZERO???
        //Zero isn't POSITIVE or NEGATIVE see http://en.wikipedia.org/wiki/Negative_and_non-negative_numbers
        //public static Validation IsPositive(this Validation validation, long value, string paramName)
        //{
        //    if (value < 0)
        //        return validation.AddExceptionEx(new ArgumentOutOfRangeException(paramName,
        //                                                                      "must be positive, but was " + value));
        //    else
        //        return validation;
        //}

        public static Validation IsGreaterThanZero(this Validation validation, long value, string paramName)
        {
            if (value <= 0)            
                return validation.AddExceptionEx(new ArgumentOutOfRangeException(paramName,
                                                                              "must be greater than zero, but was " + value));                            
            else
                return validation;
        }

        public static Validation IsIndexInRange<T>(this Validation validation, T[] array, long offset, string paramName)
        {
            if (offset < 0 || offset >= array.Length)
            {               
                return validation.AddExceptionEx(new ArgumentOutOfRangeException(paramName,
                                                                                string.Format(
                                                                                    "must be > 0 and < {0}, but was {1}",
                                                                                    array.Length, offset)));
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
                // implementation is in MultiException.cs
                throw new ValidationException(message, new MultiException(validation.Exceptions));
            }
        }

        public static Boolean AnyNull<T>(this IEnumerable<T> ie) where T : class
        {
            //foreach (var x in ie)
            //    if (x == null) return true;
            //return false;
            return ie.Any(x => x == null);
        }

        public static IEnumerable<Exception> GetAllExceptions(this ValidationException exception)
        {
            if (exception.InnerException is MultiException)
            {
                var innerExceptions = (exception.InnerException as MultiException).InnerExceptions;
                foreach (var innerException in innerExceptions)
                    yield return innerException;
                //Why can't we just do this???
                //return (exception.InnerException as MultiException).InnerExceptions;                
            }
            else
            {
                yield return exception.InnerException;
            }
        }

        //As per comment http://blog.getpaint.net/2008/12/06/a-fluent-approach-to-c-parameter-validation/#comment-5086
        private static Validation AddExceptionEx(this Validation validation, Exception exception)
        {
            return (validation ?? new Validation()).AddException(exception);
        }
    }
}


