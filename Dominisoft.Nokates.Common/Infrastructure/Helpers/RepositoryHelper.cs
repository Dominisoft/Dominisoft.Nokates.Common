using System.Data.SqlClient;
using System.Linq;
using Dominisoft.Nokates.Common.Infrastructure.Attributes;
using Dominisoft.Nokates.Common.Infrastructure.Configuration;
using Dominisoft.Nokates.Common.Infrastructure.Repositories;
using Dominisoft.Nokates.Common.Models;

namespace Dominisoft.Nokates.Common.Infrastructure.Helpers
{
    public static class RepositoryHelper
    {
        public static SqlRepository<TEntity> CreateRepository<TEntity>() where TEntity:Entity,new()
        {
            var defaultConnectionName = GetDefaultConnectionName<TEntity>();
            var variableName = $"{defaultConnectionName}ConnectionString";
            var hasConnection = ConfigurationValues.TryGetValue(out var connectionString, variableName);
            if (!hasConnection)
                throw new System.Exception($"No Connection String Defined: {variableName}");

            //TODO: Register Interface with DI
            return new SqlRepository<TEntity>(new SqlConnection(connectionString));
        }

        private static string GetDefaultConnectionName<TEntity>() where TEntity : Entity
        {
            var type = typeof(TEntity);
            var attributes = type.GetCustomAttributes(true);
            var defaultConnectionAttribute = (DefaultConnectionString) attributes.FirstOrDefault(a => a.GetType() == typeof(DefaultConnectionString));
            return defaultConnectionAttribute?.Name ?? "Default";
        }
    }
}
