using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

//#define USE_JSON

namespace VdO2013SRCore
{
#if USE_JSON
  public static class JsonSerializerDefaults
  {
    internal static JsonSerializerOptions SerializerDefaults { get; } =
        new JsonSerializerOptions
        {
          DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
          PropertyNameCaseInsensitive = false,
          WriteIndented = true,
          IgnoreNullValues = true,
        };
  }
#endif

  /// <summary>
  /// Provides extension methods to System.Type to provide simple
  /// and efficient access to delegates representing reflection
  /// operations.
  /// </summary>
  public static class TypeExtensions
  {
    private const string joinSeparator = ",";

    public static T Cast<T>(this IConvertible value) => (T)Convert.ChangeType(value, typeof(T));
    public static bool TryCast<T>(this IConvertible value, out T casted, T @default)
    {
      try
      {
        casted = Cast<T>(value);
        return true;
      }
      catch
      {
        casted = @default;
        return false;
      }
    }

    public static bool TryCast<T>(this IConvertible value, out T casted) => TryCast<T>(value, out casted, default);
    public static bool TryCastOrNull<T>(this IConvertible value, out T casted) where T : class => TryCast(value, out casted, null);
    public static T CastOrNull<T>(this IConvertible obj) where T : class => TryCastOrNull(obj, out T casted) ? casted : null;

    public static bool IsNullable(this Type type) => type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>));
#pragma warning disable IDE0060 // Remove unused parameter
    public static bool IsNullable<T>(this T value) => typeof(T).IsNullable();
#pragma warning restore IDE0060 // Remove unused parameter

#pragma warning disable IDE0060 // Remove unused parameter
    public static Type GetValueType<TObject>(this TObject value)
#pragma warning restore IDE0060 // Remove unused parameter
      => value != null ? value.GetType() : Nullable.GetUnderlyingType(typeof(TObject));

    /// <summary>
    /// Throws an ArgumentNullException if the given data item is null.
    /// </summary>
    /// <param name="data">The item to check for nullity.</param>
    /// <param name="name">The name to use when throwing an exception, if necessary</param>
    public static void ThrowIfNull<T>(this T data, [CallerMemberName] string name = null) where T : class
    {
      if (data == null)
      {
        throw new ArgumentNullException(name);
      }
    }

    public static IEnumerable<MethodInfo> FindMethods(this Type type, string name, BindingFlags bindingAttrs, params Type[] parameterTypes)
    {
      var methods = from method
                    in type.GetMethods(bindingAttrs)
                    where method.Name == name
                          && method.GetParameters()
                                   .Select(parameter => parameter.ParameterType)
                                   .Select(t => t.IsGenericType ? t.GetGenericTypeDefinition() : t)
                                   .SequenceEqual(parameterTypes)
                    select method;
      return methods;
    }
    public static IEnumerable<MethodInfo> FindStaticMethods(this Type type, string name, BindingFlags bindingAttrs, params Type[] parameterTypes)
      => FindMethods(type
          , name
          , bindingAttrs: bindingAttrs | BindingFlags.Static | BindingFlags.Public
          , parameterTypes: parameterTypes)
        .Union(FindMethods(type
          , name
          , bindingAttrs: bindingAttrs | BindingFlags.Static | BindingFlags.NonPublic
          , parameterTypes: parameterTypes));
    public static IEnumerable<MethodInfo> FindClassMethods(this Type type, string name, BindingFlags bindingAttrs, params Type[] parameterTypes)
      => FindMethods(type
          , name
          , bindingAttrs: bindingAttrs | BindingFlags.Instance | BindingFlags.Public
          , parameterTypes: parameterTypes)
        .Union(FindMethods(type
          , name
          , bindingAttrs: bindingAttrs | BindingFlags.Instance | BindingFlags.NonPublic
          , parameterTypes: parameterTypes));

    public static MethodInfo FindMethod(this Type type, string name, BindingFlags bindingAttrs, params Type[] parameterTypes)
    {
      var methods = FindMethods(type, name, bindingAttrs | BindingFlags.Public, parameterTypes)
        .Union(FindMethods(type, name, bindingAttrs | BindingFlags.NonPublic, parameterTypes));

      if (!methods.Any())
        throw new InvalidOperationException($"Method {name} with parameters {string.Join(joinSeparator, parameterTypes.Cast<object>().ToArray())} not found.");

      try
      {
        return methods.SingleOrDefault();
      }
      catch (InvalidOperationException)
      {
        throw new AmbiguousMatchException($"Found multiple methods '{name}' with same {string.Join(joinSeparator, parameterTypes.Cast<object>().ToArray())} parameters.");
      }
    }
    public static MethodInfo FindMethod(this Type type, string name, params Type[] parameterTypes)
      => FindMethod(type, name, bindingAttrs: BindingFlags.Default, parameterTypes);
    public static MethodInfo FindMethod(this Type type, string name, BindingFlags bindingAttrs, params object[] parameterValues)
      => FindMethod(type, name, bindingAttrs, parameterValues.Select(v => v.GetValueType()).ToArray());
    public static MethodInfo FindMethod(this Type type, string name, params object[] parameterValues)
      => FindMethod(type, name, BindingFlags.Default, parameterValues);


    public static MethodInfo FindStaticMethod(this Type type, string name, BindingFlags bindingAttrs, params Type[] parameterTypes)
    {
      var methods = FindStaticMethods(type, name, bindingAttrs, parameterTypes);

      if (!methods.Any())
        throw new InvalidOperationException($"Method {name} with parameters {string.Join(joinSeparator, parameterTypes.Cast<object>().ToArray())} not found.");

      try
      {
        return methods.SingleOrDefault();
      }
      catch (InvalidOperationException)
      {
        throw new AmbiguousMatchException($"Found multiple methods '{name}' with same {string.Join(joinSeparator, parameterTypes.Cast<object>().ToArray())} parameters.");
      }
    }
    public static MethodInfo FindStaticMethod(this Type type, string name, params Type[] parameterTypes)
      => FindStaticMethod(type, name, bindingAttrs: BindingFlags.Default, parameterTypes);
    public static MethodInfo FindStaticMethod(this Type type, string name, BindingFlags bindingAttrs, params object[] parameterValues)
      => FindStaticMethod(type, name, bindingAttrs, parameterValues.Select(v => v.GetValueType()).ToArray());
    public static MethodInfo FindStaticMethod(this Type type, string name, params object[] parameterValues)
      => FindStaticMethod(type, name, BindingFlags.Default, parameterValues);

    public static MethodInfo FindClassMethod(this Type type, string name, BindingFlags bindingAttrs, params Type[] parameterTypes)
    {
      var methods = FindClassMethods(type, name, bindingAttrs, parameterTypes);

      if (!methods.Any())
        throw new InvalidOperationException($"Method {name} with parameters {string.Join(joinSeparator, parameterTypes.Cast<object>().ToArray())} not found.");

      try
      {
        return methods.SingleOrDefault();
      }
      catch (InvalidOperationException)
      {
        throw new AmbiguousMatchException($"Found multiple methods '{name}' with same {string.Join(joinSeparator, parameterTypes.Cast<object>().ToArray())} parameters.");
      }
    }
    public static MethodInfo FindClassMethod(this Type type, string name, params Type[] parameterTypes)
      => FindClassMethod(type, name, bindingAttrs: BindingFlags.Default, parameterTypes);
    public static MethodInfo FindClassMethod(this Type type, string name, BindingFlags bindingAttrs, params object[] parameterValues)
      => FindClassMethod(type, name, bindingAttrs, parameterValues.Select(v => v.GetValueType()).ToArray());
    public static MethodInfo FindClassMethod(this Type type, string name, params object[] parameterValues)
      => FindClassMethod(type, name, BindingFlags.Default, parameterValues);

    /// <summary>
    /// Copies all readable properties from the source to a new instance
    /// of TTarget.
    /// </summary>
    public static TTarget CopyTo<TSource, TTarget>(this TSource source) where TSource : class where TTarget : class, new()
      => PropertyCopier<TSource, TTarget>.Copy(source);

    /// <summary>
    /// Static class to efficiently store the compiled delegate which can
    /// do the copying. We need a bit of work to ensure that exceptions are
    /// appropriately propagated, as the exception is generated at type initialization
    /// time, but we wish it to be thrown as an ArgumentException.
    /// </summary>
    private static class PropertyCopier<TSource, TTarget> where TSource : class where TTarget : class, new()
    {
      private static readonly Func<TSource, TTarget> copier;
      private static readonly Exception initializationException;

      internal static TTarget Copy(TSource source)
      {
        if (initializationException != null)
        {
          throw initializationException;
        }
        if (source == null)
        {
          throw new ArgumentNullException(nameof(source));
        }
        return copier(source);
      }

      static PropertyCopier()
      {
        try
        {
          copier = BuildCopier();
          initializationException = null;
        }
        catch (Exception ex)
        {
          copier = null;
          initializationException = ex;
        }
      }

      private static Func<TSource, TTarget> BuildCopier()
      {
        var sourceParameter = Expression.Parameter(typeof(TSource), "source");
        var bindings = new List<MemberBinding>();
        foreach (var sourceProperty in typeof(TSource).GetProperties())
        {
          if (!sourceProperty.CanRead)
          {
            continue;
          }

          var targetProperty = typeof(TTarget).GetProperty(sourceProperty.Name);
          if (targetProperty == null)
          {
            throw new ArgumentException("Property " + sourceProperty.Name + " is not present and accessible in " + typeof(TTarget).FullName);
          }

          if (!targetProperty.CanWrite)
          {
            throw new ArgumentException("Property " + sourceProperty.Name + " is not writable in " + typeof(TTarget).FullName);
          }

          if (!targetProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
          {
            throw new ArgumentException("Property " + sourceProperty.Name + " has an incompatible type in " + typeof(TTarget).FullName);
          }

          bindings.Add(Expression.Bind(targetProperty, Expression.Property(sourceParameter, sourceProperty)));
        }
        var initializer = Expression.MemberInit(Expression.New(typeof(TTarget)), bindings);
        return Expression.Lambda<Func<TSource, TTarget>>(initializer, sourceParameter).Compile();
      }
    }

#pragma warning disable IDE0060 // Remove unused parameter
    public static object MakeGeneric<TGeneric, TType>(this Type type) where TGeneric : class
#pragma warning restore IDE0060 // Remove unused parameter
    {
      //Get the generic type definition from this object.
      var generic = typeof(TGeneric).GetGenericTypeDefinition();

      //Create an array of type arguments for the generic parameters
      var typeArgs = new[] { typeof(TType) };

      //Make the generic type
      var constructed = generic.MakeGenericType(typeArgs);

      //Instantiate an object of the constructed generic type
      var result = Activator.CreateInstance(constructed);
      return result;
    }

#if USE_JSON
    public static string ToJson<T>(this T instance, JsonSerializerOptions options = null)
        => JsonSerializer.Serialize(instance, options);
    public static string ToJson<T>(this T instance)
        => JsonSerializer.Serialize(instance, JsonSerializerDefaults.SerializerDefaults);
    public static T FromJson<T>(this string instance, JsonSerializerOptions options = null)
        => (T)JsonSerializer.Deserialize(instance, typeof(T), options);
    public static T FromJson<T>(this string instance)
        => (T)JsonSerializer.Deserialize(instance, typeof(T), JsonSerializerDefaults.SerializerDefaults);
#endif

    private static void ThrowMethodNotFound(string typeName, string methodName, params Type[] argumentTypes)
    {
      var msg = $"Type {typeName ?? "<none>"} has no {("ctor".Equals(methodName.ToLower()) ? "method " : "")} {methodName ?? "<none>"}({string.Join(joinSeparator, argumentTypes.Cast<object>())})";
      throw new InvalidOperationException(msg);
    }

    #region Ctor
    /// <summary>
    /// Searches for a public instance constructor whose parameters match the types in the specified array.
    /// </summary>
    /// <param name="type">The Type to be searched</param>
    /// <param name="argumentTypes">The arguments Type to be passed</param>
    /// <returns>An object representing the public instance constructor whose parameters match the types in the parameter type array, if found; otherwise, null.</returns>
    public static ConstructorInfo GetConstructor(this Type type, params Type[] argumentTypes)
    {
      type.ThrowIfNull(nameof(type));
      argumentTypes.ThrowIfNull(nameof(argumentTypes));

      var ci = type.GetConstructor(argumentTypes);
      if (ci == null)
        ThrowMethodNotFound(nameof(type), "ctor", argumentTypes);
      return ci;
    }

    public static ConstructorInfo GetConstructor(this Type type, params object[] arguments)
      => GetConstructor(type, arguments.Select(a => a == null ? a.GetValueType() : a.GetType()).ToArray());

    /// <summary>
    /// Obtains a delegate to invoke a parameterless constructor
    /// </summary>
    /// <typeparam name="TResult">The base/interface type to yield as the
    /// new value; often object except for factory pattern implementations</typeparam>
    /// <param name="type">The Type to be created</param>
    /// <returns>A delegate to the constructor if found, else null</returns>
    public static Func<TResult> Ctor<TResult>(this Type type)
    {
      var ci = GetConstructor(type, Type.EmptyTypes);
      return Expression.Lambda<Func<TResult>>(Expression.New(ci)).Compile();
    }

    private static void GetCtorExpression(ConstructorInfo constructor, Type[] argTypes, out Expression ctorExpression, out ParameterExpression[] argsExpression)
    {
      argsExpression = (from at in argTypes select Expression.Parameter(at, at.Name.ToLower())).ToArray();
      ctorExpression = Expression.New(constructor, argsExpression);
    }

    /// <summary>
    /// Obtains a delegate to invoke a constructor which takes a parameter
    /// </summary>
    /// <typeparam name="TResult">The base/interface type to yield as the new value; often object except for factory pattern implementations</typeparam>
    /// <typeparam name="TArg1">The type of the constructor parameter</typeparam>
    /// <param name="type">The Type to be created</param>
    /// <returns>A delegate to the constructor if found, else null</returns>
    public static Func<TArg1, TResult> Ctor<TResult, TArg1>(this Type type)
    {
      var argTypes = new[] { typeof(TArg1) };
      var ci = GetConstructor(type, argTypes);
      GetCtorExpression(ci, argTypes, out var ctor, out var args);
      return Expression.Lambda<Func<TArg1, TResult>>(ctor, args).Compile();
    }
    /// <summary>
    /// Obtains a delegate to invoke a constructor with multiple parameters
    /// </summary>
    /// <typeparam name="TResult">The base/interface type to yield as the new value; often object except for factory pattern implementations</typeparam>
    /// <typeparam name="TArg1">The type of the first constructor parameter</typeparam>
    /// <typeparam name="TArg2">The type of the second constructor parameter</typeparam>
    /// <param name="type">The Type to be created</param>
    /// <returns>A delegate to the constructor if found, else null</returns>
    public static Func<TArg1, TArg2, TResult> Ctor<TResult, TArg1, TArg2>(this Type type)
    {
      var argTypes = new[] { typeof(TArg1), typeof(TArg2) };
      var ci = GetConstructor(type, argTypes);
      GetCtorExpression(ci, argTypes, out var ctor, out var args);
      return Expression.Lambda<Func<TArg1, TArg2, TResult>>(ctor, args).Compile();
    }
    /// <summary>
    /// Obtains a delegate to invoke a constructor with multiple parameters
    /// </summary>
    /// <typeparam name="TResult">The base/interface type to yield as the new value; often object except for factory pattern implementations</typeparam>
    /// <typeparam name="TArg1">The type of the first constructor parameter</typeparam>
    /// <typeparam name="TArg2">The type of the second constructor parameter</typeparam>
    /// <typeparam name="TArg3">The type of the third constructor parameter</typeparam>
    /// <param name="type">The Type to be created</param>
    /// <returns>A delegate to the constructor if found, else null</returns>
    public static Func<TArg1, TArg2, TArg3, TResult> Ctor<TResult, TArg1, TArg2, TArg3>(this Type type)
    {
      var argTypes = new[] { typeof(TArg1), typeof(TArg2), typeof(TArg3) };
      var ci = GetConstructor(type, argTypes);
      GetCtorExpression(ci, argTypes, out var ctor, out var args);
      return Expression.Lambda<Func<TArg1, TArg2, TArg3, TResult>>(ctor, args).Compile();
    }
    /// <summary>
    /// Obtains a delegate to invoke a constructor with multiple parameters
    /// </summary>
    /// <typeparam name="TResult">The base/interface type to yield as the new value; often object except for factory pattern implementations</typeparam>
    /// <typeparam name="TArg1">The type of the first constructor parameter</typeparam>
    /// <typeparam name="TArg2">The type of the second constructor parameter</typeparam>
    /// <typeparam name="TArg3">The type of the third constructor parameter</typeparam>
    /// <typeparam name="TArg4">The type of the fourth constructor parameter</typeparam>
    /// <param name="type">The Type to be created</param>
    /// <returns>A delegate to the constructor if found, else null</returns>
    public static Func<TArg1, TArg2, TArg3, TArg4, TResult> Ctor<TResult, TArg1, TArg2, TArg3, TArg4>(this Type type)
    {
      var argTypes = new[] { typeof(TArg1), typeof(TArg2), typeof(TArg3), typeof(TArg4) };
      var ci = GetConstructor(type, argTypes);
      GetCtorExpression(ci, argTypes, out var ctor, out var args);
      return Expression.Lambda<Func<TArg1, TArg2, TArg3, TArg4, TResult>>(ctor, args).Compile();
    }
    /// <summary>
    /// Obtains a delegate to invoke a constructor with multiple parameters
    /// </summary>
    /// <typeparam name="TResult">The base/interface type to yield as the new value; often object except for factory pattern implementations</typeparam>
    /// <typeparam name="TArg1">The type of the first constructor parameter</typeparam>
    /// <typeparam name="TArg2">The type of the second constructor parameter</typeparam>
    /// <typeparam name="TArg3">The type of the third constructor parameter</typeparam>
    /// <typeparam name="TArg4">The type of the fourth constructor parameter</typeparam>
    /// <typeparam name="TArg5">The type of the fifth constructor parameter</typeparam>
    /// <param name="type">The Type to be created</param>
    /// <returns>A delegate to the constructor if found, else null</returns>
    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TResult> Ctor<TResult, TArg1, TArg2, TArg3, TArg4, TArg5>(this Type type)
    {
      var argTypes = new[] { typeof(TArg1), typeof(TArg2), typeof(TArg3), typeof(TArg4), typeof(TArg5) };
      var ci = GetConstructor(type, argTypes);
      GetCtorExpression(ci, argTypes, out var ctor, out var args);
      return Expression.Lambda<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>>(ctor, args).Compile();
    }
    #endregion
  }
}
