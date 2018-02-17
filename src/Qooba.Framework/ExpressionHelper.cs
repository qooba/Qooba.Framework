using System;
using System.Reflection;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Qooba.Framework.Abstractions;

namespace Qooba.Framework
{
    using Expression = System.Linq.Expressions.Expression;

    public class ExpressionHelper : IExpressionHelper
    {
        private static IDictionary<Type, Func<object[], object>> activatorsCache = new ConcurrentDictionary<Type, Func<object[], object>>();

        public object CreateInstance(Type type, params object[] arguments)
        {
            Func<object[], object> activator;
            if (!activatorsCache.TryGetValue(type, out activator))
            {
                var constructor = type.GetTypeInfo().GetConstructors().First();
                activator = GetActivator(constructor);
                activatorsCache[type] = activator;
            }

            var instance = activator(arguments);
            return instance;
        }

        public T CreateInstance<T>(params object[] arguments) where T : class
        {
            return CreateInstance(typeof(T), arguments) as T;
        }

        public Func<T, TProperty> GetValueGetter<T, TProperty>(string propertyName)
        {
            var instance = Expression.Parameter(typeof(T), "instance");
            var property = Expression.Property(instance, propertyName);
            return Expression.Lambda<Func<T, TProperty>>(property, instance).Compile();
        }

        public Func<object, TProperty> GetValueGetter<TProperty>(string propertyName, Type instanceType)
        {
            var instance = Expression.Parameter(instanceType, "instance");
            var property = Expression.Property(instance, propertyName);
            return Expression.Lambda<Func<object, TProperty>>(property, instance).Compile();
        }

        public Action<T, TProperty> GetValueSetter<T, TProperty>(string propertyName)
        {
            var instance = Expression.Parameter(typeof(T), "instance");
            var argument = Expression.Parameter(typeof(TProperty), "argument");
            var property = Expression.Property(instance, propertyName);
            var assign = Expression.Assign(property, argument);
            return Expression.Lambda<Action<T, TProperty>>(assign, instance, argument).Compile();
        }

        private Func<object[], object> GetActivator(ConstructorInfo constructor)
        {
            var parameters = constructor.GetParameters();
            var parametersExpression = Expression.Parameter(typeof(object[]), "constructorArguments");
            var constructorArguments = parameters.Select((p, index) => Expression.Convert(Expression.ArrayIndex(parametersExpression, Expression.Constant(index)), p.ParameterType)).ToArray();
            var newExpression = Expression.New(constructor, constructorArguments);
            return Expression.Lambda<Func<object[], object>>(newExpression, parametersExpression).Compile();
        }
    }
}
