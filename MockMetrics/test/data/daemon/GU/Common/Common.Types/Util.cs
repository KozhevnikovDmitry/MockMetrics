using System;
using System.ComponentModel;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using Common.Types.Exceptions;

namespace Common.Types
{
    /// <summary>
    /// Класс содержащий простые общеупотребительные методы  
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// Возвращает имя свойства по Lambda-выражению, возвращаеющему значение этого свойства.
        /// </summary>
        /// <typeparam name="TProperty">Тип свойства</typeparam>
        /// <param name="property">Lambda-выражение, возвращаеющее значение свойства</param>
        /// <returns>Имя свойства</returns>
        /// <exception cref="GUException">Невозможно получить имя свойства. Переданно выражение неверное типа.</exception>
        public static string GetPropertyName<TProperty>(Expression<Func<TProperty>> property)
        {
            try
            {
                var lambda = (LambdaExpression)property;
                MemberExpression memberExpression;
                if (lambda.Body is UnaryExpression)
                {
                    var unaryExpression = (UnaryExpression)lambda.Body;
                    memberExpression = (MemberExpression)unaryExpression.Operand;
                }
                else
                {
                    memberExpression = (MemberExpression)lambda.Body;
                }

                return memberExpression.Member.Name;
            }
            catch (InvalidCastException ex)
            {
                throw new GUException("Невозможно получить имя свойства. Переданно выражение неверное типа.", ex);
            }
        }

        public static T Clone<T>(T source) where T : class 
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            if (ReferenceEquals(source, null))
            {
                return default(T);
            }

            using (var stream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, source);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }

        public static string GetDescription(this Enum enumValue)
        {
            FieldInfo field = enumValue.GetType().GetField(enumValue.ToString());

            var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            return attribute == null ? enumValue.ToString() : attribute.Description;
        }
    }
}
