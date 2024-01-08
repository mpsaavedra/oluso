using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Oluso.Extensions;

/// <summary>
/// Object related extensions
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// check if obj is instance of a given type
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="checkType"></param>
    /// <returns></returns>
    public static bool IsInstanceOf(this object obj, Type checkType) =>
        obj.GetType().IsOfType(checkType);
    
    /// <summary>
    /// returns true if the object has the provided property
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static bool HasProperty(this object obj, string name)
    {
        return obj.GetType().GetRuntimeProperty(name) != null;
    }

    /// <summary>
    /// returns true if the requested property hs a value set
    /// </summary>
    /// <param name="obj">object to check for value of property</param>
    /// <param name="name">name of the property yo validate</param>
    /// <returns></returns>
    public static bool HasValue(this object obj, string name)
    {
        var currentProperty = obj.GetType().GetRuntimeProperty(name);
        if (currentProperty == null)
            return false;
        var currentValue = currentProperty.GetValue(obj);
        var defaultValue = Activator.CreateInstance(obj.GetType()).GetType()
            .GetRuntimeProperty(name).GetValue(obj);
        return currentValue != defaultValue;
    }

    /// <summary>
    /// Sets a value to an objects property using his name
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public static void SetValue(this object obj, string name, object value)
    {
        obj.GetType().GetRuntimeProperty(name).SetValue(obj, value);
    }

    /// <summary>
    /// Check if the object is one of the provided values
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="values"></param>
    /// <param name="equalityComparer"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool IsOneOf<T>(this T obj, T[] values, IEqualityComparer<T> equalityComparer)
    {
        return values.Contains(obj, equalityComparer);
    }

    /// <summary>
    /// sets some property value to this object 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="propertyName"></param>
    /// <param name="value"></param>
    public static void SetPropertyValue(this object obj, string propertyName, object value)
    {
        obj.GetType().GetProperty(propertyName)?.SetValue(obj, value);
    }
    
    /// <summary>
    /// returns true if the source is nullable
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static bool IsNullable<TSource>(this TSource source)
    {
        if (source == null) return true;
        Type type = typeof(TSource);
        if (!type.IsValueType) return true;
        if (Nullable.GetUnderlyingType(type) != null) return true;
        return false;
        // return default(TSource) == null;
    }
    
    /// <summary>
    /// returns true if any of the provided values is Null or Empty
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(params object[] values)
    {
        if (values == null || !values.Any()) return true;

        var fails = values.Where(value =>
        {
            if (value == null)
            {
                return true;
            }

            if (value.IsInstanceOfType(typeof(string)))
            {
                return string.IsNullOrEmpty(((string)value));
            }

            //if (value.InstanceOfType(typeof(Array)))
            //{
            //    return ((Array)value).Length < 1;
            //}
            //
            //if (value.InstanceOfType(typeof(ICollection<>)))
            //{
            //    return ((ICollection)value).Count < 1;
            //}
            //
            //if (value.InstanceOfType(typeof(IEnumerable<>)))
            //{
            //  return ((IEnumerable)value).Length < 1;
            //  return false;
            //}
            if (value.IsInstanceOfType(typeof(IEnumerable<>)) ||
                value.IsInstanceOfType(typeof(ICollection)) ||
                value.IsInstanceOfType(typeof(ICollection<>)) ||
                value.IsInstanceOfType(typeof(IEnumerable)) ||
                value.IsInstanceOfType(typeof(Array)))
            {
                return Is.NullOrEmpty(value);
            }

            return false;
        });

        return fails.Any();
    }
    
    
    
    /// <summary>
    /// returns true if the objects is of provided type
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="objectType"></param>
    /// <returns></returns>
    public static bool IsInstanceOfType(this object obj, Type objectType)
    {
        return obj.GetType().IsOfType(objectType);
    }


    /// <summary>
    /// returns all properties of a given type
    /// </summary>
    /// <param name="obj">this object</param>
    /// <param name="type">type of the properties to return</param>
    /// <returns></returns>
    public static IEnumerable<PropertyInfo> GetPropertiesOfType(this object obj, Type type) =>
        obj.GetType().GetProperties().Where(p => p.PropertyType == type);

    /// <summary>
    /// returns all properties of a given type
    /// </summary>
    /// <param name="obj"></param>
    /// <typeparam name="TPropType"></typeparam>
    /// <returns></returns>
    public static IEnumerable<PropertyInfo> GetPropertiesOfType<TPropType>(this object obj) =>
        obj.GetType().GetProperties().Where(p => p.PropertyType is TPropType);
    
    /// <summary>
    /// if source is null use set the optional value
    /// </summary>
    /// <param name="source"></param>
    /// <param name="optional"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static TSource ThenIfNullOrEmpty<TSource>(this TSource source, TSource optional)
    {
        if (IsNullOrEmpty(source!))
            return optional;
        if (!Is.IsNullable(source))
            return source;
        return source;

        // !NullOrEmpty(source) ? (
        //     source.IsNullable() ? source : source
        // ) : optional;
    }
    
    /// <summary>
    /// Populate an object with data from another object keeping the data of the original
    /// object if properties does not appear or are null or empty.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="mappedData"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static TSource PopulateWithMappedData<TSource>(this TSource source, object mappedData)
    {
        foreach (var dbProperty in source!.GetType().GetProperties()
                     .Where(x => x.CanWrite &&
                                 x.PropertyType.IsPublic))
        {
            if (mappedData.GetType().GetProperties().Any(p => p.Name == dbProperty.Name))
            {
                var mappedValue = mappedData.GetType().GetProperty(dbProperty.Name)?.GetValue(mappedData);
                var sourceValue = source.GetType().GetProperty(dbProperty.Name)?.GetValue(source);
                sourceValue = mappedValue.ThenIfNullOrEmpty(sourceValue);
                source
                    .GetType()
                    .GetProperty(dbProperty.Name)?
                    .SetValue(source, sourceValue);
            }
        }

        return source;
    }
}