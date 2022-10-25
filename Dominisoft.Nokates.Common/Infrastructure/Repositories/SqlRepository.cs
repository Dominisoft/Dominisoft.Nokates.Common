using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using Dominisoft.Nokates.Common.Infrastructure.Extensions;
using Dominisoft.Nokates.Common.Infrastructure.Helpers;
using Dominisoft.Nokates.Common.Models;
using RepoDb;


namespace Dominisoft.Nokates.Common.Infrastructure.Repositories
{
    public interface ISqlRepository<TEntity>
    {
        TEntity Create(TEntity entity);
        TEntity Update(TEntity entity);
        TEntity Get(int Id);
        List<TEntity> GetAll();
        List<TEntity> GetAllMatchingFilter(object filters);
        List<TEntity> GetAllMatchingAnyFilter(object filters);
        bool Delete(TEntity entity);

    }
    public class SqlRepository<TEntity>: BaseRepository<TEntity,SqlConnection>, ISqlRepository<TEntity> where TEntity : Entity, new()
    {
        private readonly string _tableName;
        public SqlRepository(string connectionString) : base(connectionString)
        {
            _tableName = new TEntity().GetTableName();
        }


        public TEntity Create(TEntity entity)
        {
            var id =(int) Insert(entity);
            if (id > 0) return Get(id);
            else return new TEntity();
        }

        public TEntity Update(TEntity entity)
            => Update(entity);

        public TEntity Get(int Id)
            => Query(Id, cacheKey: $"{typeof(TEntity).Name}-{Id}").FirstOrDefault();

        public List<TEntity> GetAll()
            => QueryAll(_tableName, cacheKey: $"{typeof(TEntity).Name}-All").ToList();

        public List<TEntity> GetAllMatchingFilter(object filters)
            => Query(_tableName,where: QueryGroup.Parse(filters), cacheKey: $"{typeof(TEntity).Name}-{filters.Serialize()}").ToList();

        public List<TEntity> GetAllMatchingAnyFilter(object filters)
        {
            var filterDictionary = filters.Serialize().Deserialize<Dictionary<string, string>>();
            var list = new List<TEntity>();
            foreach (var filter in filterDictionary)
            {
                list.AddRange(GetAllMatchingFilter(filter));
            }
            return list.Distinct().ToList();
        }

        public bool Delete(TEntity entity)
            => base.Delete(entity)>0;
    }
}
