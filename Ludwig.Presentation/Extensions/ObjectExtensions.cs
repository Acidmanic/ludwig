using System;
using Acidmanic.Utilities.Reflection;

namespace Ludwig.Presentation.Extensions
{
    public static class ObjectExtensions
    {


        public static T Clone<T>(this T value)
        {
            if (value == null)
            {
                return default;
            }
            var type = typeof(T);
            
            return (T)value.Clone(type) ;
        }

        public static object Clone(this object value,Type type)
        {
            
            if (TypeCheck.IsEffectivelyPrimitive(type))
            {
                return value;
            }
            
            var properties = type.GetProperties();

            var clone = new ObjectInstantiator().BlindInstantiate(type);
            
            foreach (var property in properties)
            {
                if (property.CanRead && property.CanWrite)
                {
                    var propertyReadValue = property.GetValue(value);

                    var propertyWriteValue = propertyReadValue.Clone(property.PropertyType);
                    
                    property.SetValue(clone,propertyWriteValue);
                }    
            }

            return clone;
        }
    }
}