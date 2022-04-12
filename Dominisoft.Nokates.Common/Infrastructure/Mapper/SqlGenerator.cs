using System;
using System.Collections.Generic;
using System.Text;

namespace Dominisoft.Nokates.Common.Infrastructure.Mapper
{
    public static class SqlGenerator
    {
        public static string GetInsertStatement(string tableName, dynamic values)
        {
            var columnNames = new List<string>();
            var columnValues = new List<string>();
            var type = typeof(object);
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                columnNames.Add("["+property.Name+"]");
                columnValues.Add($"'{property.GetValue(values)}'");
            }

            var statement = $"Insert into {tableName} ({string.Join(',', columnNames)}) values ({string.Join(',', columnValues)})";
            return statement;
        }
    }
}
