using Qooba.Framework.Abstractions;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Qooba.Framework
{
    public class Mapper : IMapper
    {
        private static IDictionary<Type, ConcurrentDictionary<Type, object>> mappers = new ConcurrentDictionary<Type, ConcurrentDictionary<Type, object>>();

        public TOut Map<TIn, TOut>(TIn input)
        {
            var mapper = CreateMapper<TIn, TOut>();
            return mapper(input);
        }

        public static Func<TIn, TOut> CreateMapper<TIn, TOut>()
        {
            var inputType = typeof(TIn);
            var outputType = typeof(TOut);

            ConcurrentDictionary<Type, object> mapperdDict;
            if (!mappers.TryGetValue(inputType, out mapperdDict))
            {
                mapperdDict = new ConcurrentDictionary<Type, object>();
                mappers[inputType] = mapperdDict;
            }

            object mapper;
            if (!mapperdDict.TryGetValue(outputType, out mapper))
            {
                var input = Expression.Parameter(inputType, "input");
                var block = InitilizeMapper(input, inputType, outputType);
                mapper = Expression.Lambda<Func<TIn, TOut>>(block, input).Compile();
                mapperdDict[outputType] = mapper;
            }

            return mapper as Func<TIn, TOut>;
        }

        public static IList<TOut> ParseList<TIn, TOut>(IEnumerable<TIn> input)
        {
            if (input == null)
            {
                return null;
            }

            var mapper = CreateMapper<TIn, TOut>();
            return input.Select(mapper).ToList();
        }

        public static TOut[] ParseArray<TIn, TOut>(IEnumerable<TIn> input)
        {
            if (input == null)
            {
                return null;
            }

            var mapper = CreateMapper<TIn, TOut>();
            return input.Select(mapper).ToArray();
        }

        public static Expression InitilizeMapper(Expression input, Type inputType, Type outputType)
        {
            var ctor = outputType.GetTypeInfo().GetConstructors().FirstOrDefault();
            var outputInstance = Expression.New(ctor);
            var outputLocal = Expression.Parameter(outputType, "outputLocal");
            var assignExpressions = new List<Expression>();
            var localVariables = new List<ParameterExpression>();
            localVariables.Add(outputLocal);
            assignExpressions.Add(Expression.Assign(outputLocal, outputInstance));
            var inputProperties = inputType.GetTypeInfo().GetProperties();
            var outputProperties = outputType.GetTypeInfo().GetProperties();

            var properties = inputProperties.Where(x => outputProperties.Any(o => o.Name == x.Name));

            foreach (var property in properties)
            {
                var name = property.Name;
                var inputPropertyType = inputProperties.FirstOrDefault(x => x.Name == name).PropertyType;
                var outputPropertyType = outputProperties.FirstOrDefault(x => x.Name == name).PropertyType;
                Type nullableType;
                if (inputPropertyType == outputPropertyType)
                {
                    var inputPropertyGetter = Expression.Property(input, name);
                    var outputPropertyGetter = Expression.Property(outputLocal, name);
                    assignExpressions.Add(CheckIsNull(inputPropertyGetter, inputPropertyType, Expression.Assign(outputPropertyGetter, inputPropertyGetter)));
                }
                else if (outputPropertyType == typeof(string))
                {
                    var inputProperty = Expression.Property(input, name);
                    var inputPropertyGetter = Expression.Call(inputProperty, inputPropertyType.GetTypeInfo().GetMethod("ToString", new Type[] { }));
                    var outputPropertyGetter = Expression.Property(outputLocal, name);
                    assignExpressions.Add(CheckIsNull(inputProperty, inputPropertyType, Expression.Assign(outputPropertyGetter, inputPropertyGetter)));
                }
                else if (outputPropertyType.GetTypeInfo().GetInterfaces().Contains(typeof(IEnumerable)))
                {
                    var inputPropertyGetter = Expression.Property(input, name);
                    var outputPropertyGetter = Expression.Property(outputLocal, name);
                    var inputElementType = inputPropertyType.GetElementType() ?? inputPropertyType.GenericTypeArguments.FirstOrDefault();
                    var outputElementType = outputPropertyType.GetElementType() ?? outputPropertyType.GenericTypeArguments.FirstOrDefault();
                    MethodInfo meth;

                    if (outputPropertyType.IsArray)
                    {
                        meth = typeof(Mapper).GetTypeInfo().GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).FirstOrDefault(x => x.Name == "ParseArray").MakeGenericMethod(inputElementType, outputElementType);
                    }
                    else
                    {
                        meth = typeof(Mapper).GetTypeInfo().GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).FirstOrDefault(x => x.Name == "ParseList").MakeGenericMethod(inputElementType, outputElementType);
                    }

                    var parsingExpression = Expression.Call(null, meth, inputPropertyGetter);
                    assignExpressions.Add(CheckIsNull(inputPropertyGetter, inputPropertyType, Expression.Assign(outputPropertyGetter, parsingExpression)));
                }
                else if ((nullableType = Nullable.GetUnderlyingType(outputPropertyType)) != null)
                {
                    var inputPropertyGetter = Expression.Property(input, name);
                    var outputPropertyGetter = Expression.Property(outputLocal, name);

                    var parsed = Expression.Parameter(nullableType, string.Concat(name, "Parsed"));
                    localVariables.Add(parsed);
                    var meth = nullableType.GetTypeInfo().GetMethod("TryParse", new[] { typeof(string), nullableType.MakeByRefType() });
                    if (meth != null)
                    {
                        var parsingExpression = Expression.Call(null, meth, Expression.Call(inputPropertyGetter, inputPropertyType.GetTypeInfo().GetMethod("ToString", new Type[] { })), parsed);
                        assignExpressions.Add(CheckIsNull(inputPropertyGetter, inputPropertyType, parsingExpression));
                        assignExpressions.Add(CheckIsNull(inputPropertyGetter, inputPropertyType, Expression.Assign(outputPropertyGetter, Expression.Convert(parsed, outputPropertyType))));
                    }
                }
                else if (outputPropertyType.GetTypeInfo().IsClass)
                {
                    var inputPropertyGetter = Expression.Property(input, name);
                    var outputPropertyGetter = Expression.Property(outputLocal, name);
                    var nestedMapper = InitilizeMapper(inputPropertyGetter, inputPropertyType, outputPropertyType);
                    assignExpressions.Add(CheckIsNull(inputPropertyGetter, inputPropertyType, Expression.Assign(outputPropertyGetter, nestedMapper)));
                }
                else if (outputPropertyType.GetTypeInfo().IsEnum)
                {
                    var inputPropertyGetter = Expression.Property(input, name);
                    var outputPropertyGetter = Expression.Property(outputLocal, name);
                    var parsed = Expression.Parameter(outputPropertyType, string.Concat(name, "Parsed"));
                    localVariables.Add(parsed);
                    var meth = outputPropertyType.GetTypeInfo().GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).FirstOrDefault(x => x.Name == "TryParse" && x.GetParameters().Length == 2).MakeGenericMethod(outputPropertyType);
                    if (meth != null)
                    {
                        var parsingExpression = Expression.Call(null, meth, Expression.Call(inputPropertyGetter, inputPropertyType.GetTypeInfo().GetMethod("ToString", new Type[] { })), parsed);
                        assignExpressions.Add(CheckIsNull(inputPropertyGetter, inputPropertyType, parsingExpression));
                        assignExpressions.Add(CheckIsNull(inputPropertyGetter, inputPropertyType, Expression.Assign(outputPropertyGetter, parsed)));
                    }
                }
                else
                {
                    var inputPropertyGetter = Expression.Property(input, name);
                    var outputPropertyGetter = Expression.Property(outputLocal, name);
                    var parsed = Expression.Parameter(outputPropertyType, string.Concat(name, "Parsed"));
                    localVariables.Add(parsed);
                    var meth = outputPropertyType.GetTypeInfo().GetMethod("TryParse", new[] { typeof(string), outputPropertyType.MakeByRefType() });
                    if (meth != null)
                    {
                        var parsingExpression = Expression.Call(null, meth, Expression.Call(inputPropertyGetter, inputPropertyType.GetTypeInfo().GetMethod("ToString", new Type[] { })), parsed);
                        assignExpressions.Add(CheckIsNull(inputPropertyGetter, inputPropertyType, parsingExpression));
                        assignExpressions.Add(CheckIsNull(inputPropertyGetter, inputPropertyType, Expression.Assign(outputPropertyGetter, parsed)));
                    }
                    //Expression<Func<string, int>> e = x => this.ParseInt(x);
                    //var outputPropertyGetter = Expression.Property(outputLocal, name);
                    //var inputPropertyGetter = Expression.Call(Expression.Property(input, name), inputPropertyType.GetMethod("ToString", new Type[] { }));
                    //var invocation = Expression.Invoke(e, inputPropertyGetter);
                    //assignExpressions.Add(Expression.Assign(outputPropertyGetter, invocation));
                }

            }

            assignExpressions.Add(outputLocal);

            return Expression.Block(localVariables, assignExpressions);
        }

        public static Expression CheckIsNull(Expression input, Type inputType, Expression expression)
        {
            if (Nullable.GetUnderlyingType(inputType) != null || inputType == typeof(string))
            {
                return Expression.IfThen(Expression.NotEqual(input, Expression.Constant(null, inputType)), expression);
            }

            return expression;
        }

        private int ParseInt(string input)
        {
            int i;
            int.TryParse(input, out i);
            return i;
        }

        private double ParseDouble(string input)
        {
            double i;
            double.TryParse(input, out i);
            return i;
        }

        private decimal ParseDecimal(string input)
        {
            decimal i;
            decimal.TryParse(input, out i);
            return i;
        }
    }
}