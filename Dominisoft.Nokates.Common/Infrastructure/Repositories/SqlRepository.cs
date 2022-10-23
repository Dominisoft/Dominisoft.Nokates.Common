using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using Dominisoft.Nokates.Common.Infrastructure.Extensions;
using Dominisoft.Nokates.Common.Infrastructure.Helpers;
using Dominisoft.Nokates.Common.Infrastructure.RepositoryConnections;
using Dominisoft.Nokates.Common.Models;
using Dominisoft.SqlBuilder;
using Microsoft.IdentityModel.Logging;


namespace Dominisoft.Nokates.Common.Infrastructure.Repositories
{
    public interface ISqlRepository<TEntity>
    {
        int Create(TEntity entity);
        TEntity Update(TEntity entity);
        TEntity Get(int Id);
        List<TEntity> GetAll();
        List<TEntity> GetAllMatchingFilter(object filters);
        List<TEntity> GetAllMatchingAnyFilter(object filters);
        bool Delete(TEntity id);

    }
    public class SqlRepository<TEntity>: ISqlRepository<TEntity> where TEntity : Entity, new()
    {
        private readonly ISqlConnectionWrapper _connection;

        public SqlRepository(ISqlConnectionWrapper connection)
        {
            _connection = connection;
        }


        public int Create(TEntity entity)
        {

            var id = _connection.Insert(entity);  
             return id;
        }


        public TEntity Get(int id)
        {
            var e = new TEntity {Id = id};
            return _connection.Get(e);
        }
        public List<TEntity> GetAll()
            => _connection.GetAll<TEntity>();

        public List<TEntity> GetAllMatchingFilter(object filters)
            => _connection.GetAllFilter<TEntity>(filters);

        public List<TEntity> GetAllMatchingAnyFilter(object filters)
            => _connection.GetAnyFilter<TEntity>(filters);

        public TEntity Update(TEntity entity)
        {
            _connection.Update(entity);
            return _connection.Get(entity);
        }
        public bool Delete(TEntity entity)
        {
            var removed =  _connection.Delete(entity);
            return true;
        }
    }
}
