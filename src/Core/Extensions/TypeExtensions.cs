#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Oluso.Extensions;

/// <summary>
/// Type realted extensions
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// Returns the default value of the type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static object? ToDefaultValue(this Type type)
    {
        if (type is null)
            return default;

        return type.IsValueType ? Activator.CreateInstance(type) : default;
    }

    /// <summary>
    /// returns an IEnumerable with the <see cref="PropertyInfo"/> that are from a given type
    /// </summary>
    /// <param name="t"></param>
    /// <typeparam name="TAttr"></typeparam>
    /// <returns></returns>
    public static IEnumerable<PropertyInfo> ToPropertiesWithAttribute<TAttr>(this Type t)
    {
        var result =
            from prop in t.GetProperties()
            from attr in prop.GetCustomAttributes().Where(x => x.IsInstanceOf(typeof(TAttr)))
            select prop;

        return result.Distinct();
    }


    /// <summary>
    /// returns the Assembly where the type is contained
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static Assembly ToAssembly(this Type type) => type.Assembly;

    /// <summary>
    /// checks if is of a given type 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="checkType"></param>
    /// <returns></returns>
    public static bool IsOfType(this Type source, Type checkType) =>
        source.IsInstanceOfType(checkType);


    /// <summary>
    /// returns a dictionary with the property names and values
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IDictionary? ToDictionary(this Type type) =>
        type is null ? default : type.GetProperties().ToDictionaryPropertyInfo();


    /// <summary>
    /// Returns an IEnumerable of <see cref="PropertyInfo"/> with the properties
    /// that correspond with a given Type of Attribute
    /// </summary>
    /// <returns>The properties with attribute.</returns>
    /// <param name="t">Type base.</param>
    /// <typeparam name="TAttr">The 1st type parameter.</typeparam>
    /// <example>
    /// <code>
    ///   MyType
    ///     .GetPropertiesWithAttribute{ApiNullifyOnCreateAttribute}()
    ///     .Select(t => { /*  do something */ });
    /// </code>
    /// </example>
    public static IEnumerable<PropertyInfo> GetPropertiesWithAttribute<TAttr>(this Type t)
        where TAttr : Attribute
    {
        List<PropertyInfo> result = new List<PropertyInfo>();

        foreach (var propertyInfo in t.GetProperties())
        {
            foreach (var attribute in propertyInfo.GetCustomAttributes(true))
            {
                if (attribute.IsInstanceOfType(typeof(TAttr)) && !result.Contains(propertyInfo))
                {
                    result.Add(propertyInfo);
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Get properties that are Collection/ICollections
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IEnumerable<PropertyInfo> GetICollectionOrICollectionOfTProperties(this Type type)
    {
        // Get properties with PropertyType declared as interface
        var interfaceProps =
            from prop in type.GetProperties()
            from interfaceType in prop.PropertyType.GetInterfaces()
            where interfaceType.IsGenericType
            let baseInterface = interfaceType.GetGenericTypeDefinition()
            where (baseInterface == typeof(ICollection<>)) || (baseInterface == typeof(ICollection))
            select prop;

        // Get properties with PropertyType declared(probably) as solid types.
        var nonInterfaceProps =
            from prop in type.GetProperties()
            where typeof(ICollection).IsAssignableFrom(prop.PropertyType) ||
                  typeof(ICollection<>).IsAssignableFrom(prop.PropertyType)
            select prop;

        // Combine both queries into one result
        // return interfaceProps.Union(nonInterfaceProps);                

        return type.GetProperties()
            .Where(p => p.PropertyType
                .GetInterfaces()
                .Any(i => i.IsGenericType &&
                          //(i.GetGenericTypeDefinition() == typeof(Collection) ||
                          (i.GetGenericTypeDefinition() == typeof(ICollection))
                )
            );
    }

    /// <summary>
    /// return the real name of the class
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string SimplifyName(this Type type)
    {
        // TODO: Check if this works in every single case
        return type.ToString().LastPart('.');
    }

    /// <summary>
    /// reurns an string representation of the type to include in the
    /// source code
    /// </summary>
    /// <param name="type"></param>
    /// <param name="nullable">if true the ? sign will be included in the name</param>
    public static string StringifyType(this Type type, bool nullable = false)
    {
        var result = "";
        if (type == typeof(string))
        {
            result = type.Name.ToLower();
        }
        else if (type == typeof(int))
        {
            result = "int";
        }
        else if (type == typeof(DateTime))
        {
            result = "DateTime";
        }
        else if (type.IsGenericType &&
                 type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
#pragma warning disable CS8604
            result = StringifyType(Nullable.GetUnderlyingType(type), true);
#pragma warning restore CS8604
        }
        else if (type.IsGenericType &&
                 type.GetGenericTypeDefinition() == typeof(ICollection<>))
        {
            var typeName = type.ToString();
            result = typeName.ToFormatCollectionEntry().ToCollectionEntry();
        }
        else if (type == typeof(decimal))
        {
            result = "decimal";
        }
        else if (type == typeof(double))
        {
            result = "double";
        }
        else if (type == typeof(byte))
        {
            result = "byte";
        }
        else if (type == typeof(byte[]))
        {
            result = "byte[]";
        }
        else if (type == typeof(bool))
        {
            result = "bool";
        }
        else
        {
            result = type.ToString().LastPart('.');
        }

        return result + (nullable ? "?" : "");
    }


    /// <summary>
    /// return the annotation value of type in the class
    /// </summary>
    /// <param name="type"></param>
    /// <typeparam name="TAttr">Annotation type to locate</typeparam>
    /// <returns></returns>
    public static TAttr GetAnnotation<TAttr>(this Type type) where TAttr : Attribute
    {
        return type.GetCustomAttributes<TAttr>().First();
    }

    /// <summary>
    /// returns methods that has a given attribute
    /// </summary>
    /// <param name="type"></param>
    /// <typeparam name="TAttr"></typeparam>
    /// <returns></returns>
    public static IEnumerable<MethodInfo> GetMethodsWithAttribute<TAttr>(this Type type)
        where TAttr : Attribute
    {
        return type.GetMethods().Where(m => m.GetCustomAttributes<TAttr>().Any());
    }
}