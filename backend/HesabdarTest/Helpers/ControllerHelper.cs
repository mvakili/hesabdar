using FsCheck;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;

namespace HesabdarTest.Helpers
{
    public static class ControllerHelper
    {
        public static T GetObject<T>(this IActionResult result)
        {
            Assert.IsInstanceOf<ObjectResult>(result);

            if (result is ObjectResult res)
            {
                // assert the result has same type
                Assert.IsInstanceOf<T>(res.Value);
                //return an object from result
                if (res.Value is T obj)
                {
                    return obj;
                }
                else
                {
                    throw new InvalidCastException(typeof(T).ToString());
                }
            }
            else
            {
                throw new InvalidCastException(typeof(OkObjectResult).ToString());
            }

        }

        public static Gen<T> ChooseFrom<T>(T[] xs)
        {
            try
            {
                return from i in Gen.Choose(0, xs.Length - 1)
                       select xs[i];
            } catch (Exception ex)
            {
                throw;
            }
        }
    }
}
