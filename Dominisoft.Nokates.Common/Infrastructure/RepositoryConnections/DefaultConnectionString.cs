using System;
using System.Collections.Generic;
using System.Text;
using Dominisoft.Nokates.Common.Infrastructure.Configuration;

namespace Dominisoft.Nokates.Common.Infrastructure.RepositoryConnections
{
    public interface IConnectionString
    {
        string GetConnectionString();

    }

    public interface IDefaultConnectionString : IConnectionString
    {

    }
    public class DefaultConnectionString : IDefaultConnectionString
    {
        public string GetConnectionString()
            => ConfigurationValues.Values["DefaultConnectionString"];
    }
}
