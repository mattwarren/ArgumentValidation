

namespace Validation
{
    using System;
    using Exceptions;    

    //Missing from this implementation, and other kinks to work out:

    //* Could use lots of additional methods within ValidationExtensions. 
    //  (some were omitted for brevity in this blog post)
    //* Calling ValidationExtensions.Check() is itself not validated. So, if you forget to 
    //  put a call to it at the end of your validation expression then the exception will 
    //  not be thrown. Often you’ll end up plowing into a null reference and getting a 
    //  NullReferenceException, especially if you were relying on ValidationExtensions.IsNotNull(),
    //  but this isn’t guaranteed for the other validations (esp. when dealing with unmanaged 
    //  data types). It would be simple to add code to Validation to ensure that its list of 
    //  exceptions was “observed”, and if not then in the finalizer it could yell and scream with an exception.
    //* The exception type coming out of any method that uses this will be ValidationException. 
    //  This isn’t an issue for crash logs, but it is for when you call a method and want to
    //  discriminate among multiple exception types and decide what to do next (e.g., 
    //  FileNotFoundException vs. AccessDeniedException). I’m sure there’s a way to fix that, 
    //  with better aggregation, and (hopefully) without reflection.
    //* Should probably change the IEnumerable<Exception> in Validation to be Exception[].

    
    class Program
    {        
        static void Main(string[] args)
        {
            TestItem(() => Copy(new[] {1, 4, 2, 4}, 1, new int[] {1, 54}, 0, 1));

            TestItem(() => Copy<int>(null, 2, null, 0, -5));
            
            TestItem(() => Copy(new[] { 1, 4, 2, 4 }, 5, new int[] { 1, 54 }, -1, 9));                
        }

        /// <summary>
        /// A simple helper function for testing only, just allows you to wrap up
        /// all the extra overhead (exception handling and printing to the console)
        /// </summary>
        /// <param name="action">The function call to execute</param>
        private static void TestItem(Action action)
        {
            try
            {                           
                action();
                Console.WriteLine("No exception(s) in method call\n");
            }
            catch (ValidationException vEx)
            {
                Console.WriteLine(vEx.Message);
                foreach (var exception in vEx.GetAllExceptions())
                {
                    Console.WriteLine(exception.ToString());
                }
                Console.WriteLine("");
                //Console.WriteLine(ex.Source);
                //Console.WriteLine(vEx.StackTrace);
            }
        }

        /// <summary>
        /// A sample function that we can use to test out the validation
        /// </summary>
        /// <typeparam name="T">The type of the generic array</typeparam>
        /// <param name="src">The source array to copy FROM</param>
        /// <param name="srcOffset">The location to start at in the source</param>
        /// <param name="dst">The destination array to copy TO</param>
        /// <param name="dstOffset">The location to start at in the destination</param>
        /// <param name="length">The number of items to copy from source to destination</param>
        public static void Copy<T>(T[] src, long srcOffset, T[] dst, long dstOffset, long length)
        {
            Validate.Begin()
                    .IsNotNull(dst, "dst")
                    .IsNotNull(src, "src")
                    .Check() //having this here means we throw straight away if either of the first 2 fail
                    //.IsPositive(length, "length")
                    .IsGreaterThanZero(length, "length")
                    .IsIndexInRange(src, srcOffset, "srcOffset")
                    .IsIndexInRange(src, srcOffset + length, "srcOffset + length")
                    .IsIndexInRange(dst, dstOffset, "dstOffset")
                    .IsIndexInRange(dst, dstOffset + length, "dstOffset + length")
                    .Check();

            for (int di = (int)dstOffset; di < dstOffset + length; ++di)
                dst[di] = src[di - dstOffset + srcOffset];
        }
    }
}
