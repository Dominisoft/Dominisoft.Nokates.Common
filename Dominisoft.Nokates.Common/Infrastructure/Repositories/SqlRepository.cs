using Dapper.Contrib.Extensions;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Dominisoft.Nokates.Common.Infrastructure.Helpers;
using Dominisoft.Nokates.Common.Infrastructure.Mapper;
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
            
            var sqlParams = CustomSqlMapper.ReverseMap(entity);
            var table = entity.GetTableName();
            var sqlStatement = SqlGenerator.GetInsertStatement(table, sqlParams);
            var id = SqlMapper.QueryFirstOrDefault<int>(_connection, sqlStatement, sqlParams, commandType:CommandType.Text);        
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
