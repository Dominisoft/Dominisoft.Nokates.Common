using Dapper.Contrib.Extensions;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using Dominisoft.Nokates.Common.Models;


namespace Dominisoft.Nokates.Common.Infrastructure.Repositories
{
    public class SqlRepository<TEntity> where TEntity : Entity
    {
        private readonly SqlConnection _connection;

        public SqlRepository(SqlConnection connection)
        {
            _connection = connection;
        }


        public long Create(TEntity entity)
        {
            var id = _connection.Insert(entity);            
            return id;
        }


        public TEntity Get(long id)
        {
            return _connection.Get<TEntity>(id);
        }
        public List<TEntity> GetAll()
        {
            return _connection.GetAll<TEntity>().ToList();
        }
        public TEntity Update(TEntity entity)
        {
            var id = entity.Id;
            _connection.Update(entity);
            return _connection.Get<TEntity>(id);
        }
        public bool Delete(TEntity entity)
        {
            var removed =  _connection.Delete(entity);
            return removed;
        }

    }
}
