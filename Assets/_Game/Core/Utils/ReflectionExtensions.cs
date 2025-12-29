#nullable enable
namespace Core.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class ReflectionExtensions
    {
        public static Type GetSingleDerivedType(this Type type)
        {
            return type.GetDerivedTypes().ToArray() switch
            {
                { Length: 0 }   => throw new InvalidOperationException($"No derived type found for {type.Name}"),
                { Length: > 1 } => throw new InvalidOperationException($"Multiple derived types found for {type.Name}"),
                { } types       => types[0],
            };
        }
        public static IEnumerable<Type> GetDerivedTypes(this Type baseType)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                            .Where(asm => !asm.IsDynamic)
                            .SelectMany(baseType.GetDerivedTypes);
        }

        public static IEnumerable<Type> GetDerivedTypes(this Type baseType, Assembly assembly) => assembly.GetTypes().Where(type => !type.IsAbstract && baseType.IsAssignableFrom(type));
    }
}