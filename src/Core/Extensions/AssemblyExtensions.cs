using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Oluso.Extensions;

/// <summary>
/// <see cref="Assembly"/> related extensions
/// </summary>
public static class AssemblyExtensions
{
    /// <summary>
    /// returns a list of instances of the implementations of a given class. 
    /// </summary>
    /// <param name="assembly"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerable<T> ToImplementationsOf<T>(this Assembly assembly) =>
        assembly.GetTypes()
            .Where(x => x.GetTypeInfo().IsClass && !x.GetTypeInfo().IsAbstract && typeof(T).IsAssignableFrom(x))
            .Select(type => (T)Activator.CreateInstance(type));

    /// <summary>
    /// returns a list of instances of the implementation of a given contained in the provided
    /// list of assemblies
    /// </summary>
    /// <param name="assemblies"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerable<T> ToImplementationsOf<T>(this IEnumerable<Assembly> assemblies) =>
        from assembly in assemblies
        from type in assembly.ToImplementationsOf<T>()
        select (T)Activator.CreateInstance(type?.GetType()!);

    /// <summary>
    /// returns a list of classes that implement a given type and are annotated with a given attribute
    /// </summary>
    /// <param name="assembly"></param>
    /// <typeparam name="TAnnotation"></typeparam>
    /// <typeparam name="TClass"></typeparam>
    /// <returns></returns>
    public static IEnumerable<TClass> ToAnnotatedImplementationsOf<TAnnotation, TClass>(this Assembly assembly) 
        where TAnnotation : Attribute =>
        assembly.ToImplementationsOf<TClass>()
            .Where(x => x != null && x.GetType().GetCustomAttributes<TAnnotation>().Any());

    /// <summary>
    /// load a list of assemblies by their canonical names
    /// </summary>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IEnumerable<Assembly> LoadAssemblies(this IEnumerable<string> assemblies) =>
        assemblies.IsNullOrAnyNull().Select(x => x.LoadAssembly());
    
    /// <summary>
    /// load an assembly by his canonical name
    /// </summary>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static Assembly LoadAssembly(this string assembly)
        => Assembly.Load(assembly);
}