using System.Data.SqlClient;
using Dominisoft.Nokates.Common.Infrastructure.Configuration;
using Dominisoft.Nokates.Common.Infrastructure.Repositories;
using Dominisoft.Nokates.Common.Models;

namespace Dominisoft.Nokates.Common.Infrastructure.Helpers
{
    public static class RepositoryHelper
    {
        public static SqlRepository<TEntity> CreateRepository<TEntity>() where TEntity:Entity
        {
            var hasConnection = ConfigurationValues.TryGetValue(out var connectionString, "ConnectionString");
            if (!hasConnection)
                throw new System.Exception("No Connection String Defined");

            //TODO: Register Interface with DI
            return new SqlRepository<TEntity>(new SqlConnection(connectionString));
        }
    }
}
