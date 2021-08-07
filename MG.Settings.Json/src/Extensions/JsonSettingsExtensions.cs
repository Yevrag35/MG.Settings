using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MG.Settings.Json.Extensions
{
    public static class JsonSettingsExtensions
    {
        /// <summary>
        /// Creates a new <see cref="JProperty"/> instance with a default value.
        /// </summary>
        /// <typeparam name="TClass"></typeparam>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="settings"></param>
        /// <param name="expression">The expression that resolves into the <see cref="MemberInfo"/> of the property or field.</param>
        /// <param name="defaultValue">The value to populate the <see cref="JProperty.Value"/> with.</param>
        /// <returns>
        ///     A new <see cref="JProperty"/> instance.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="expression"/> did not resolve into a property or field 
        ///     or the resolved property/field is not decorated with a <see cref="JsonPropertyAttribute"/>.
        /// </exception>
        public static JProperty GetDefault<TClass, TProp>(this TClass settings, Expression<Func<TClass, TProp>> expression, object defaultValue)
            where TClass : JsonSettings
        {
            string name = GetJsonName(settings, expression);
            return new JProperty(name, defaultValue);
        }

        /// <summary>
        /// Gets the <see cref="string"/> specified in a property/field's <see cref="JsonPropertyAttribute.PropertyName"/>.
        /// </summary>
        /// <remarks>
        ///     If the resolved member is not decorated with a <see cref="JsonPropertyAttribute"/> or does not have 
        ///     <see cref="JsonPropertyAttribute.PropertyName"/> defined, then the name of the member is returned
        ///     as is.
        /// </remarks>
        /// <typeparam name="TClass"></typeparam>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="settings"></param>
        /// <param name="expression">The expression that resolves into the <see cref="MemberInfo"/> of the property or field.</param>
        /// <returns>The <see cref="string"/> specified in the <see cref="JsonPropertyAttribute.PropertyName"/>.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="expression"/> did not resolve into a property or field.
        /// </exception>
        public static string GetJsonName<TClass, TProp>(this TClass settings, Expression<Func<TClass, TProp>> expression)
            where TClass : JsonSettings
        {
            MemberInfo memInfo = GetPropertyInfo(expression);

            JsonPropertyAttribute jsonPropertyAttribute = memInfo
                ?.GetCustomAttributes<JsonPropertyAttribute>()
                    ?.FirstOrDefault();

            if (null == jsonPropertyAttribute || string.IsNullOrWhiteSpace(jsonPropertyAttribute.PropertyName))
            {
                if (null != memInfo)
                    return memInfo.Name;

                else
                    throw new ArgumentException("The expression did not resolve into a property or field");
            }

            return jsonPropertyAttribute.PropertyName;
        }

        private static MemberInfo GetPropertyInfo<TClass, TProp>(Expression<Func<TClass, TProp>> expression)
            where TClass : JsonSettings
        {
            MemberInfo info = null;
            if (expression.Body is MemberExpression memEx)
                info = memEx.Member;

            else if (expression.Body is UnaryExpression unEx && unEx.Operand is MemberExpression unExMem)
                info = unExMem.Member;

            return info;
        }
    }
}
