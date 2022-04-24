using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using Dominisoft.Nokates.Common.Models;
using Dominisoft.SqlBuilder;


namespace Dominisoft.Nokates.Common.Infrastructure.Repositories
{
    public class SqlRepository<TEntity> where TEntity : Entity, new()
    {
        private readonly SqlConnection _connection;

        public SqlRepository(SqlConnection connection)
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
        {
            return _connection.GetAll<TEntity>();
        }
        public TEntity Update(TEntity entity)
        {
            _connection.Update(entity);
            return _connection.Get(entity);
        }
        public bool Delete(TEntity entity)
        {
            var removed =  _connection.Delete(entity);
            return removed>0;
        }

    }
}
