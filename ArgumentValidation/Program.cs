
using Validation.Exceptions;

namespace Validation
{
    using System;
    using System.Collections.Generic;  

    

    class Program
    {        
        static void Main(string[] args)
        {
            try
            {
                Copy(new[] { 1, 4, 2, 4 }, 1, new int[] { 1, 54 }, 0, 3);
                Console.WriteLine("No exception(s) in method call");

                //Copy<int>(null, 2, null, 0, 1);
                //Console.WriteLine("No exception(s) in method call");

                //Copy(new[] { 1, 4, 2, 4 }, 5, new int[] { 1, 54 }, -1, 9);
                //Console.WriteLine("No exception(s) in method call");
            }            
            catch (ValidationException vEx)
            {
                Console.WriteLine(vEx.Message);                
                foreach (var exMessage in vEx.GetAllExceptionMessages())
                {
                    Console.WriteLine(exMessage);
                }                
                Console.WriteLine("");
                //Console.WriteLine(ex.Source);
                //Console.WriteLine(vEx.StackTrace);
            }
        }

        public static void Copy<T>(T[] src, long srcOffset, T[] dst, long dstOffset, long length)
        {
            Validate.Begin()
                    .IsNotNull(dst, "dst")
                    .IsNotNull(src, "src")
                    .Check()
                    .IsPositive(length, "length")
                    //.IsGreaterThanZero(length, "length")
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
