using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Qooba.Framework.Abstractions;

namespace Qooba.Framework
{
    public class JSON : ISerializer
    {
        public static IDictionary<Type, Tuple<object, bool>> objectParsers = new ConcurrentDictionary<Type, Tuple<object, bool>>();

        public static IDictionary<Type, Func<object, string>> objectSerializers = new ConcurrentDictionary<Type, Func<object, string>>();

        private static ExpressionHelper eh = new ExpressionHelper();

        public static string SerializeObject(object o, bool camelCaseProprtiesNames = true)
        {
            var objectType = o.GetType();
            Func<object, string> objectSerializer = null;
            if (!objectSerializers.TryGetValue(objectType, out objectSerializer))
            {
                objectSerializer = objectSerializers[objectType] = PrepareObjectSerializer(objectType, camelCaseProprtiesNames);
            }

            return objectSerializer(o);
        }

        public static object DeserializeObject(Stream stream, Type objectType, bool caseInsensitive = true)
        {
            var o = eh.CreateInstance(objectType);
            Tuple<object, bool> objectParser = null;
            if (!objectParsers.TryGetValue(objectType, out objectParser))
            {
                if (o is IEnumerable)
                {
                    var listProperty = PreparePropertyParser(objectType, caseInsensitive);

                    Func<Stream, object> a = st =>
                     {
                         return listProperty(st);
                     };

                    objectParser = objectParsers[objectType] = new Tuple<object, bool>(a, true);
                }
                else
                {
                    objectParser = objectParsers[objectType] = new Tuple<object, bool>(PrepareObjectParser(objectType, caseInsensitive), false);
                }
            }

            if (objectParser.Item2)
            {
                return ((Func<Stream, object>)objectParser.Item1)(stream);
            }
            else
            {
                (objectParser.Item1 as Action<object, Stream>)(o, stream);
                return o;
            }
        }

        private static Func<object, string> PrepareObjectSerializer(Type objectType, bool camelCaseProprtiesNames = true)
        {
            var ls = new List<Func<object, string>>();
            var outputProperties = objectType.GetTypeInfo().GetProperties();
            foreach (var p in outputProperties)
            {
                var name = p.Name;
                ls.Add(PreparePropertySerializer(name, p.PropertyType, objectType, camelCaseProprtiesNames));
            }

            var propNum = ls.Count;
            var diNum = ls.Count;
            Func<object, string> action = (ob) =>
            {
                var sb = new StringBuilder();
                sb.Append("{");
                for (int i = 0; i < diNum; i++)
                {
                    var sv = ls[i](ob);
                    if (sv != null)
                    {
                        sb.Append(sv);
                        if (i + 1 < diNum)
                        {
                            sb.Append(",");
                        }
                    }
                }

                sb.Append("}");
                return sb.ToString();
            };

            return action;
        }

        private static Action<object, Stream> PrepareObjectParser(Type objectType, bool caseInsensitive = true)
        {
            IDictionary<string, Action<object, Stream>> dict = null;
            if (caseInsensitive)
            {
                dict = new Dictionary<string, Action<object, Stream>>(StringComparer.OrdinalIgnoreCase);
            }
            else
            {
                dict = new Dictionary<string, Action<object, Stream>>();
            }

            var outputProperties = objectType.GetTypeInfo().GetProperties();
            foreach (var p in outputProperties)
            {
                var name = p.Name;
                dict[name] = PreparePropertyParser(name, p.PropertyType, objectType);
            }

            var propNum = dict.Count;

            Action<object, Stream> action = (ob, st) =>
            {
                int i = 0;
                while (i < propNum)
                {
                    var propName = ParsePropertyName(st);
                    if (propName == null)
                    {
                        break;
                    }

                    if (dict.TryGetValue(propName, out Action<object, Stream> parser))
                    {
                        parser(ob, st);
                        i++;
                    }
                    else
                    {
                        var propVal = ParseSkipValue(st);
                    }
                }
            };

            return action;
        }

        private static Func<object, string> PreparePropetyToStringGetter(Type objectType, Type propertyType, string propertyName)
        {
            var instance = Expression.Parameter(typeof(object), "instance");
            var meth = propertyType.GetTypeInfo().GetMethod("ToString", new Type[] { });
            var property = Expression.Call(Expression.Property(Expression.Convert(instance, objectType), propertyName), meth);
            return Expression.Lambda<Func<object, string>>(property, instance).Compile();
        }

        private static Func<object, string> PreparePropertySerializer(string propertyName, Type propertyType, Type objectType, bool camelCaseProprtiesNames = true)
        {
            var propertyNameValue = camelCaseProprtiesNames ? string.Concat(propertyName[0].ToString().ToLower(), propertyName.Substring(1)) : propertyName;
            Type nullableType = null;
            Func<object, string> func = null;
            if (propertyType == typeof(string) || propertyType.IsEnum)
            {
                var pGet = PreparePropetyToStringGetter(objectType, propertyType, propertyName);
                func = o =>
                {
                    var v = pGet(o);
                    if (v != null)
                    {
                        return string.Concat("\"", propertyNameValue, "\":\"", v, "\"");
                    }

                    return null;
                };
            }
            else if (IsNumericType(propertyType))
            {
                var pGet = PreparePropetyToStringGetter(objectType, propertyType, propertyName);

                func = o =>
                {
                    var v = pGet(o);
                    if (v != null)
                    {
                        return string.Concat("\"", propertyNameValue, "\":", v);
                    }

                    return null;
                };
            }
            else if (propertyType.GetTypeInfo().GetInterfaces().Contains(typeof(IEnumerable)))
            {
                var instance = Expression.Parameter(typeof(object), "instance");
                var property = Expression.Convert(Expression.Property(Expression.Convert(instance, objectType), propertyName), typeof(object));
                var pGet = Expression.Lambda<Func<object, object>>(property, instance).Compile();

                Func<object, string> serializer;
                //TODO:
                var elementType = propertyType.GetElementType() ?? propertyType.GenericTypeArguments.FirstOrDefault();
                if (elementType == typeof(string) || elementType.IsEnum)
                {
                    serializer = o => string.Concat("\"", o.ToString(), "\"");
                }
                else if (IsNumericType(elementType))
                {
                    serializer = o => o.ToString();
                }
                else if (elementType.IsClass)
                {
                    serializer = PrepareObjectSerializer(elementType, camelCaseProprtiesNames);
                }
                else
                {
                    serializer = o => string.Concat("\"", o.ToString(), "\"");
                }

                func = o =>
                {
                    var v = pGet(o);
                    if (v != null)
                    {
                        var sb = new StringBuilder();
                        sb.Append(string.Concat("\"", propertyNameValue, "\":["));
                        var vl = v as IList;
                        var vlen = vl.Count;
                        var vlenm = vlen - 1;
                        for (var i = 0; i < vlen; i++)
                        {
                            sb.Append(serializer(vl[i]));
                            if (i < vlenm)
                            {
                                sb.Append(",");
                            }
                        }

                        sb.Append("]");
                        return sb.ToString();
                    }

                    return null;
                };
            }
            else if ((nullableType = Nullable.GetUnderlyingType(propertyType)) != null)
            {
                var pGet = PreparePropetyToStringGetter(objectType, propertyType, propertyName);

                func = o =>
                {
                    var v = pGet(o);
                    if (v != null)
                    {
                        return string.Concat("\"", propertyNameValue, "\":", v);
                    }

                    return null;
                };
            }
            else if (propertyType == typeof(object))
            {
                var instance = Expression.Parameter(typeof(object), "instance");
                var property = Expression.Convert(Expression.Property(Expression.Convert(instance, objectType), propertyName), typeof(object));
                var pGet = Expression.Lambda<Func<object, object>>(property, instance).Compile();

                func = o =>
                {
                    var v = pGet(o);
                    Func<object, string> serializer = null;
                    var pType = v.GetType();
                    if (!objectSerializers.TryGetValue(pType, out serializer))
                    {
                        serializer = objectSerializers[pType] = PrepareObjectSerializer(pType);
                    }

                    if (v != null)
                    {
                        return string.Concat("\"", propertyNameValue, "\":", serializer(v));
                    }

                    return null;
                };
            }
            else if (propertyType.IsClass)
            {
                var serializer = PrepareObjectSerializer(propertyType);
                var instance = Expression.Parameter(typeof(object), "instance");
                var property = Expression.Convert(Expression.Property(Expression.Convert(instance, objectType), propertyName), typeof(object));
                var pGet = Expression.Lambda<Func<object, object>>(property, instance).Compile();

                func = o =>
                {
                    var v = pGet(o);
                    if (v != null)
                    {
                        return string.Concat("\"", propertyNameValue, "\":", serializer(v));
                    }

                    return null;
                };
            }
            else
            {
                var pGet = PreparePropetyToStringGetter(objectType, propertyType, propertyName);

                func = o =>
                {
                    var v = pGet(o);
                    if (v != null)
                    {
                        return string.Concat("\"", propertyNameValue, "\":\"", v, "\"");
                    }

                    return null;
                };
            }

            return func;
        }

        public static Action<object, Stream> PreparePropertyParser(string propertyName, Type propertyType, Type objectType, bool caseInsensitive = true)
        {
            Type nullableType = null;
            Action<object, Stream> act = null;

            if (propertyType == typeof(object))
            {
                var setter = GetValueSetter(propertyName, objectType, propertyType);
                act = (ob, st) =>
                {
                    var val = ParseObjectValue(st);
                    if (val != "null") //Please don't user null string value :P
                    {
                        setter(ob, val.Trim());
                    }
                };
            }
            else if (propertyType == typeof(string))
            {
                var setter = GetValueSetter(propertyName, objectType, propertyType);
                act = (ob, st) =>
                {
                    var val = ParsePropertyValue(st);
                    if (val != "null") //Please don't user null string value :P
                    {
                        setter(ob, val);
                    }
                };
            }
            else if (propertyType.GetTypeInfo().GetInterfaces().Contains(typeof(IEnumerable)))
            {
                var elementType = propertyType.GetElementType() ?? propertyType.GenericTypeArguments.FirstOrDefault();

                var parser = JSON.PreparePropertyParser(elementType, caseInsensitive);
                Type pType = FindBestListType(propertyType, elementType);

                var instance = Expression.Parameter(typeof(object), "instance");
                var argument = Expression.Parameter(typeof(object), "argument");
                var property = Expression.Property(Expression.Convert(instance, objectType), propertyName);
                var assign = Expression.Assign(property, Expression.Convert(argument, propertyType));
                var lam = Expression.Lambda<Action<object, object>>(assign, instance, argument).Compile();

                act = (ob, st) =>
                {
                    var list = (eh.CreateInstance(pType) as IList);
                    int chv;
                    char ch;

                    while (true)
                    {
                        chv = st.ReadByte();
                        ch = (char)chv;
                        if (ch == '[' || chv == -1)
                        {
                            break;
                        }
                    }

                    while (true)
                    {
                        var value = parser(st);
                        list.Add(value);
                        st.Position--; //remove this parser should retrun tuple with the last char value
                        chv = st.ReadByte();
                        ch = (char)chv;
                        if (ch == ']' || (char)st.ReadByte() == ']')
                        {
                            break;
                        }

                        if (ch == ',')
                        {
                            st.Position--;
                        }
                    }

                    lam(ob, list);
                };
            }
            else if ((nullableType = Nullable.GetUnderlyingType(propertyType)) != null)
            {
                var setter = GetTryParseValueSetter(propertyName, objectType, nullableType);
                act = (ob, st) =>
                {
                    var val = ParsePropertyValue(st);
                    if (val != "null") //Please don't user null string value :P
                    {
                        setter(ob, val);
                    }
                };
            }
            else if (propertyType.IsClass)
            {
                var setter = PrepareObjectParser(propertyType, caseInsensitive);
                var instance = Expression.Parameter(typeof(object), "instance");
                var argument = Expression.Parameter(typeof(object), "argument");
                var property = Expression.Property(Expression.Convert(instance, objectType), propertyName);
                var assign = Expression.Assign(property, Expression.Convert(argument, propertyType));
                var lam = Expression.Lambda<Action<object, object>>(assign, instance, argument).Compile();

                act = (ob, st) =>
                {
                    var subOb = eh.CreateInstance(propertyType);
                    setter(subOb, st);
                    lam(ob, subOb);
                };
            }
            else
            {
                var setter = GetTryParseValueSetter(propertyName, objectType, propertyType);
                act = (ob, st) =>
                {
                    var val = ParsePropertyValue(st);
                    if (val != "null") //Please don't user null string value :P
                    {
                        setter(ob, val);
                    }
                };
            }

            return act;
        }

        public static Func<Stream, object> PreparePropertyParser(Type propertyType, bool caseInsensitive = true)
        {
            Type nullableType = null;
            Func<Stream, object> func = null;

            if (propertyType == typeof(object))
            {
                func = st =>
                {
                    var val = ParseObjectValue(st);
                    if (val != "null") //Please don't user null string value :P
                    {
                        return val.Trim();
                    }

                    return null;
                };
            }
            else if (propertyType == typeof(string))
            {
                func = st =>
                {
                    var val = ParsePropertyValue(st);
                    if (val != "null") //Please don't user null string value :P
                    {
                        return val;
                    }

                    return null;
                };
            }
            else if (propertyType.GetTypeInfo().GetInterfaces().Contains(typeof(IEnumerable)))
            {
                var elementType = propertyType.GetElementType() ?? propertyType.GenericTypeArguments.FirstOrDefault();
                var setter = JSON.PreparePropertyParser(elementType, caseInsensitive);

                Type pType = FindBestListType(propertyType, elementType);

                func = st =>
                {
                    var list = (eh.CreateInstance(pType) as IList);
                    int chv;
                    char ch;

                    while (true)
                    {
                        chv = st.ReadByte();
                        ch = (char)chv;
                        if (ch == '[' || chv == -1)
                        {
                            break;
                        }
                    }

                    while (true)
                    {
                        var value = setter(st);
                        list.Add(value);
                        st.Position--;
                        chv = st.ReadByte();
                        ch = (char)chv;
                        if (ch == ']' || (char)st.ReadByte() == ']')
                        {
                            break;
                        }

                        if (ch == ',')
                        {
                            st.Position--;
                        }
                    }

                    return list;
                };
            }
            else if ((nullableType = Nullable.GetUnderlyingType(propertyType)) != null)
            {
                func = st =>
                {
                    var val = ParsePropertyValue(st);
                    if (val != "null") //Please don't user null string value :P
                    {
                        return val;
                    }

                    return null;
                };
            }
            else if (propertyType.IsClass)
            {
                var setter = PrepareObjectParser(propertyType, caseInsensitive);

                func = st =>
                {
                    var subOb = eh.CreateInstance(propertyType);
                    setter(subOb, st);
                    return subOb;
                };
            }
            else
            {
                var setter = GetTryParseValueSetter(propertyType);
                func = st =>
                {
                    var val = ParsePropertyValue(st);
                    if (val != "null") //Please don't user null string value :P
                    {
                        return setter(val);
                    }

                    return null;
                };
            }

            return func;
        }

        private static bool IsNumericType(Type type)
        {
            var code = Type.GetTypeCode(type);
            return code == TypeCode.Int32 ||
                   code == TypeCode.Double ||
                   code == TypeCode.Byte ||
                   code == TypeCode.Int64 ||
                   code == TypeCode.Decimal ||
                   code == TypeCode.SByte ||
                   code == TypeCode.UInt16 ||
                   code == TypeCode.UInt32 ||
                   code == TypeCode.UInt64 ||
                   code == TypeCode.Int16 ||
                   code == TypeCode.Single;
        }

        private static Type FindBestListType(Type propertyType, Type elementType)
        {
            return typeof(List<>).MakeGenericType(elementType);
        }

        public static Action<object, object> GetValueSetter(string propertyName, Type objectType, Type propertyType)
        {
            var instance = Expression.Parameter(typeof(object), "instance");
            var argument = Expression.Parameter(typeof(object), "argument");
            var property = Expression.Property(Expression.Convert(instance, objectType), propertyName);

            var assign = Expression.Assign(property, Expression.Convert(argument, propertyType));
            return Expression.Lambda<Action<object, object>>(assign, instance, argument).Compile();
        }

        public static Action<object, object> GetTryParseValueSetter(string propertyName, Type objectType, Type propertyType)
        {
            var stringType = typeof(string);
            var instance = Expression.Parameter(typeof(object), "instance");
            var argument = Expression.Parameter(typeof(object), "argument");
            var property = Expression.Property(Expression.Convert(instance, objectType), propertyName);

            var localVariables = new List<ParameterExpression>();
            var assignExpressions = new List<Expression>();
            var parsed = Expression.Parameter(propertyType, string.Concat(propertyName, "Parsed"));
            localVariables.Add(parsed);
            MethodInfo meth;
            if (propertyType.IsEnum)
            {
                meth = propertyType.GetTypeInfo().GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).FirstOrDefault(x => x.Name == "TryParse" && x.GetParameters().Length == 2).MakeGenericMethod(propertyType);
            }
            else
            {
                meth = propertyType.GetTypeInfo().GetMethod("TryParse", new[] { stringType, propertyType.MakeByRefType() });
            }

            if (meth != null)
            {
                var parsingExpression = Expression.Call(null, meth, Expression.Convert(argument, stringType), parsed);
                assignExpressions.Add(parsingExpression);
                if (property.Type == propertyType)
                {
                    assignExpressions.Add(Expression.Assign(property, parsed));
                }
                else
                {
                    assignExpressions.Add(Expression.Assign(property, Expression.Convert(parsed, property.Type)));
                }
            }

            assignExpressions.Add(Expression.Empty());

            var block = Expression.Block(localVariables, assignExpressions);
            return Expression.Lambda<Action<object, object>>(block, instance, argument).Compile();
        }

        public static Func<object, object> GetTryParseValueSetter(Type propertyType)
        {
            var stringType = typeof(string);

            var argument = Expression.Parameter(typeof(object), "argument");


            var localVariables = new List<ParameterExpression>();
            var assignExpressions = new List<Expression>();
            var parsed = Expression.Parameter(propertyType, "Parsed");
            localVariables.Add(parsed);
            MethodInfo meth;
            if (propertyType.IsEnum)
            {
                meth = propertyType.GetTypeInfo().GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).FirstOrDefault(x => x.Name == "TryParse" && x.GetParameters().Length == 2).MakeGenericMethod(propertyType);
            }
            else
            {
                meth = propertyType.GetTypeInfo().GetMethod("TryParse", new[] { stringType, propertyType.MakeByRefType() });
            }

            if (meth != null)
            {
                var parsingExpression = Expression.Call(null, meth, Expression.Convert(argument, stringType), parsed);
                assignExpressions.Add(parsingExpression);
                assignExpressions.Add(Expression.Convert(parsed, typeof(object)));
            }

            var block = Expression.Block(localVariables, assignExpressions);
            return Expression.Lambda<Func<object, object>>(block, argument).Compile();
        }

        public static Expression CheckIsNull(Expression input, Type inputType, Expression expression)
        {
            if (Nullable.GetUnderlyingType(inputType) != null || inputType == typeof(string))
            {
                return Expression.IfThen(Expression.NotEqual(input, Expression.Constant(null, inputType)), expression);
            }

            return expression;
        }

        public static string ParsePropertyName(Stream stream)
        {
            var sb = new StringBuilder();
            char ch;
            int chv;
            do
            {
                chv = stream.ReadByte();
                ch = (char)chv;
                if (chv == -1)
                {
                    return null;
                }
            }
            while (ch != '"');

            while (true)
            {
                chv = stream.ReadByte();
                ch = (char)chv;

                if (chv == -1)
                {
                    return null;
                }

                if (ch == '"')
                {
                    break;
                }

                sb.Append(ch);
            }

            do
            {
                chv = stream.ReadByte();
                ch = (char)chv;

                if (chv == -1)
                {
                    return null;
                }
            }
            while (ch != ':');

            return sb.ToString();
        }

        public static string ParseObjectValue(Stream stream)
        {
            var sb = new StringBuilder();
            char ch;
            int chv;
            int nq = 0;
            int ncb = 0;
            int nsb = 0;
            while (true)
            {
                chv = stream.ReadByte();
                ch = (char)chv;

                if (ch == '"')
                {
                    nq = nq == 0 ? 1 : 0;
                }

                if (ch == '{')
                {
                    ncb++;
                }

                if (ch == '}')
                {
                    ncb--;
                }

                if (ch == '[')
                {
                    nsb++;
                }

                if (ch == ']')
                {
                    nsb--;
                }

                if ((ch == ',' && nq == 0 && ncb == 0 && nsb == 0) || ncb == -1)
                {
                    return sb.ToString();
                }

                sb.Append(ch);
            }
        }

        public static string ParseSkipValue(Stream stream)
        {
            char ch;
            int chv;
            int nq = 0;
            int ncb = 0;
            int nsb = 0;
            while (true)
            {
                chv = stream.ReadByte();
                ch = (char)chv;
                if (ch == '"')
                {
                    nq = nq == 0 ? 1 : 0;
                }

                if (ch == '{')
                {
                    ncb++;
                }

                if (ch == '}')
                {
                    ncb--;
                }

                if (ch == '[')
                {
                    nsb++;
                }

                if (ch == ']')
                {
                    nsb--;
                }

                if ((ch == ',' && nq == 0 && ncb == 0 && nsb == 0) || ncb == -1)
                {
                    return null;
                }
            }
        }

        public static string ParsePropertyValue(Stream stream)
        {
            var sb = new StringBuilder();
            char ch;
            int chv;

            while (true)
            {
                chv = stream.ReadByte();
                ch = (char)chv;

                if (chv == -1)
                {
                    return null;
                }

                if (ch == '"' || ch != ' ')
                {
                    break;
                }
            }

            if (ch != '"')
            {
                sb.Append(ch);
            }

            while (true)
            {
                chv = stream.ReadByte();
                ch = (char)chv;

                if (chv == -1)
                {
                    return null;
                }

                if (ch == '"')
                {
                    break;
                }

                if (ch == ',' || ch == '}' || ch == ']')
                {
                    return sb.ToString();
                }

                sb.Append(ch);
            }

            while (true)
            {
                if (ch == ',' || ch == '}' || ch == ']')
                {
                    return sb.ToString();
                }

                chv = stream.ReadByte();
                ch = (char)chv;

                if (chv == -1)
                {
                    return null;
                }
            }
        }

        public string Serialize<T>(T input) => JSON.SerializeObject(input);

        public string Serialize(object input) => JSON.SerializeObject(input);

        public T Deserialize<T>(string input) => (T)this.Deserialize(input, typeof(T));

        public object Deserialize(string input, Type type)
        {
            using (Stream s = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(input)))
            {
                return JSON.DeserializeObject(s, type);
            }
        }
    }
}