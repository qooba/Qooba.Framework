using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;
using System.Collections;

namespace Qooba.Framework
{
    public class ObjectActivator
    {
        public static T GetType<T>(IDictionary<Type, Func<Type, object>> container)
        {
            var paramType = typeof(T);
            var valueBag = Expression.Constant(container);
            var key = Expression.Constant(paramType);
            var property = Expression.Property(valueBag, "Item", key);
            var invokeExp = Expression.Invoke(property, key);
            var paramCastExp = Expression.Convert(invokeExp, paramType);
            var lambda = Expression.Lambda(typeof(Func<Type, T>), paramCastExp);

            return ((Func<Type, T>)lambda.Compile())(paramType);
        }

        public static Func<Type, T> GetActivator<T>(IDictionary<Type, IDictionary<object, Func<Type, object>>> container, ConstructorInfo constructor)
        {
            var newExpression = GetNewExpresion(container, constructor);
            var lambda = Expression.Lambda(typeof(Func<Type, T>), newExpression);
            var compiled = (Func<Type, T>)lambda.Compile();
            return compiled;
        }

        public static object GetActivator(Type type, IDictionary<Type, IDictionary<object, Func<Type, object>>> container, ConstructorInfo constructor)
        {
            var newExpression = GetNewExpresion(container, constructor);
            var ft = typeof(Func<,>).MakeGenericType(new[] { typeof(Type), type });
            var lambda = Expression.Lambda(ft, newExpression, Expression.Parameter(typeof(Type)));
            return lambda.Compile();
        }

        public static IEnumerable<T> Invoke<T>(IEnumerable<Func<Type, object>> input)
        {
            return input.Select(x => (T)x(typeof(T)));
        }

        private static NewExpression GetNewExpresion(IDictionary<Type, IDictionary<object, Func<Type, object>>> container, ConstructorInfo constructor)
        {
            var parameters = constructor.GetParameters();
            var numberOfParameters = parameters.Length;
            var arguments = new Expression[numberOfParameters];

            for (int i = 0; i < numberOfParameters; i++)
            {
                var parameterType = parameters[i].ParameterType;
                var containerExpression = Expression.Constant(container);
                var key = Expression.Constant(parameterType);
                var nameKey = Expression.Constant("");

                Expression keyGeneric = null;
                if (parameterType.GetTypeInfo().IsGenericType && parameterType.GetInterfaces().Contains(typeof(IEnumerable)))
                {
                    var inType = parameterType.GenericTypeArguments.FirstOrDefault();
                    var meth = typeof(ObjectActivator).GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).FirstOrDefault(x => x.Name == "Invoke").MakeGenericMethod(inType);
                    var inKey = Expression.Constant(inType);
                    arguments[i] = Expression.Call(null, meth, Expression.Property(Expression.Property(containerExpression, "Item", inKey), "Values"));
                }
                else if (parameterType.GetTypeInfo().IsGenericType)
                {
                    keyGeneric = Expression.Constant(parameterType.GetGenericTypeDefinition());
                    var meth = container.GetType().GetMethod("ContainsKey");
                    var outputLocal = Expression.Parameter(parameterType, "outputLocal");
                    arguments[i] =
                        Expression.Block(
                            new[] { outputLocal },
                        Expression.IfThenElse(Expression.Call(containerExpression, meth, key),
                            Expression.Assign(outputLocal, Expression.Convert(
                                Expression.Invoke(
                                    Expression.Property(
                                        Expression.Property(containerExpression, "Item", key),
                                        "Item", nameKey),
                                    key),
                                parameterType)),
                            Expression.Assign(outputLocal, Expression.Convert(
                                Expression.Invoke(
                                    Expression.Property(
                                        Expression.Property(containerExpression, "Item", keyGeneric),
                                        "Item", nameKey),
                                    key),
                                parameterType))),
                            outputLocal
                        );
                }
                else
                {
                    arguments[i] = Expression.Convert(
                                Expression.Invoke(
                                    Expression.Property(
                                        Expression.Property(containerExpression, "Item", key),
                                        "Item", nameKey),
                                    key),
                                parameterType);
                }
            }

            return Expression.New(constructor, arguments);
        }
    }
}