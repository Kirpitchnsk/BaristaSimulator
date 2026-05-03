using System;
using System.Linq;


public static partial class Extensions
{
    /// <summary>Returns non-abstract and non-generic implementations of type.</summary>
    public static Type[] GetImplementations(this Type baseType)
    {
        bool ImplementationCondition(Type type) =>
            baseType.IsAssignableFrom(type) && type != baseType
                                            && !type.IsAbstract && !type.IsGenericType;

        return baseType.GetAllMatchingTypes(ImplementationCondition);
    }


    /// <summary>Returns implementations of type including non-abstract and non-generic.</summary>
    public static Type[] GetAllImplementations(this Type baseType)
    {
        bool ImplementationCondition(Type type) =>
            baseType.IsAssignableFrom(type) && type != baseType;

        return baseType.GetAllMatchingTypes(ImplementationCondition);
    }


    public static Type[] GetAssignableTypes(this Type baseType)
    {
        bool AssignableCondition(Type type) =>
            baseType.IsAssignableFrom(type) && !type.IsAbstract;

        return baseType.GetAllMatchingTypes(AssignableCondition);
    }


    private static Type[] GetAllMatchingTypes(this Type baseType, Func<Type, bool> predicate)
    {
        return AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(predicate)
            .ToArray();
    }
}