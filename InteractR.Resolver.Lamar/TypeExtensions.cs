using System;
using System.Collections.Generic;

namespace InteractR.Resolver.Lamar
{
    /// <summary>
    ///     Returns all base types and implemented interfaces on type
    /// </summary>
    internal static class TypeExtensions
    {
        public static IEnumerable<Type> GetBaseTypes(this Type type)
        {
            var currentBaseType = type;
            while (currentBaseType != null && currentBaseType != typeof(object))
            {
                foreach (var i in type.GetInterfaces())
                {
                    yield return i;
                }

                yield return currentBaseType;
                currentBaseType = currentBaseType.BaseType;
            }
        }
    }
}
