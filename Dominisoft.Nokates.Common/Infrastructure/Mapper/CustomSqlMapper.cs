

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using Dominisoft.Nokates.Common.Infrastructure.Attributes;
using Dominisoft.Nokates.Common.Infrastructure.Extensions;
using Dominisoft.Nokates.Common.Models;
using Newtonsoft.Json;

namespace Dominisoft.Nokates.Common.Infrastructure.Mapper
{
    public class CustomSqlMapper
    {
        public static TEntity Map<TEntity>(object values) where TEntity : Entity, new()
        {
            var entityType = typeof(TEntity);
            var props = entityType.GetProperties();
            var result = new TEntity();
            foreach (var prop in props)
            {
                object value = null;

                TrySetJson(values,prop, ref value);
                TrySetMap(values,prop, ref value);
                TrySetName(values,prop, ref value);

             prop.SetValue(result, value);   
            }

            return result;
        }

        private static void TrySetName(object values, PropertyInfo prop, ref object? value)
        {
            if (values == null) return;
            if (value != null) return;
            var valuesType = values.GetType();
            var propertyName = prop.Name;
            var valuesProp = valuesType.GetProperty(propertyName);
            var valuesResult = valuesProp?.GetValue(values);
            value = valuesResult;


        }

        private static void TrySetMap(object values, PropertyInfo prop, ref object? value)
        {
            if (values == null) return;
            if (value != null) return;
            var valuesType = values.GetType();
            var propAttribute =(ColumnAttribute) prop.GetCustomAttributes(true)
                .FirstOrDefault(attrib => attrib.GetType() == typeof(ColumnAttribute));
            if (propAttribute == null) return;
            var propertyName = propAttribute.Name;
            var valuesProp = valuesType.GetProperty(propertyName);
            var valuesResult = valuesProp?.GetValue(values);
            value = valuesResult;
        }

        private static void TrySetJson(object values, PropertyInfo prop, ref object? value)
        {
            if (values == null) return;
            if (value != null) return;
            var valuesType = values.GetType();
            var propAttribute = (JsonColumnAttribute)prop.GetCustomAttributes(true)
                .FirstOrDefault(attrib => attrib.GetType() == typeof(JsonColumnAttribute));
            if (propAttribute == null) return;
            var propertyName = propAttribute.Name;
            var valuesProp = valuesType.GetProperty(propertyName);
            var valuesJsonResult =(string) valuesProp?.GetValue(values);
            var propType = prop.PropertyType;
            value = JsonConvert.DeserializeObject(valuesJsonResult,propType);

        }



        public static dynamic ReverseMap<TEntity>(TEntity entity) where TEntity:class
        {
            var eo = new ExpandoObject();
            var eoColl = (ICollection<KeyValuePair<string, object>>)eo;

            var entityType = typeof(TEntity);
            var entityProps = entityType.GetProperties();
            foreach (var prop in entityProps)
            {
                var name = GetPropertyName(prop);
                var value = GetPropertyValue(prop,entity);
                eoColl.Add(new KeyValuePair<string, object>(name,value));
            }

            return (dynamic)eo;
        }
        private static string GetPropertyName(PropertyInfo prop)
        {
            var propertyName = prop.Name;

            var jsonColumnAttribute = (JsonColumnAttribute)prop.GetCustomAttributes(true)
                .FirstOrDefault(attrib => attrib.GetType() == typeof(JsonColumnAttribute));
            if (jsonColumnAttribute != null)
                propertyName = jsonColumnAttribute.Name;


            var columnAttribute = (ColumnAttribute)prop.GetCustomAttributes(true)
                .FirstOrDefault(attrib => attrib.GetType() == typeof(ColumnAttribute));
            if (columnAttribute != null)
                propertyName = columnAttribute.Name;


            return propertyName;

        }

        private static object GetPropertyValue<TEntity>(PropertyInfo prop, TEntity entity) where TEntity : class
        {
            var jsonColumnAttribute = (JsonColumnAttribute)prop.GetCustomAttributes(true)
                .FirstOrDefault(attrib => attrib.GetType() == typeof(JsonColumnAttribute));
            return jsonColumnAttribute != null ? prop.GetValue(entity).Serialize() : prop.GetValue(entity);

        }
    }
}
