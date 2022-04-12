using System.Collections.Generic;
using Dominisoft.Nokates.Common.Infrastructure.Extensions;
using Dominisoft.Nokates.Common.Infrastructure.Helpers;
using Dominisoft.Nokates.Common.Models;

namespace Dominisoft.Nokates.Common.Infrastructure.Client
{
    public interface IBaseClient<TEntity> where TEntity : Entity
    {
        TEntity Create(TEntity entity);
        TEntity Update(TEntity entity);
        bool Delete(TEntity entity);
        TEntity Get(int id);
        List<TEntity> GetAll();
    }
    public class BaseClient<TEntity> : IBaseClient<TEntity> where TEntity : Entity 
    {
        public string BaseUrl { get; set; }

        public TEntity Create(TEntity entity)
            => HttpHelper.Post<TEntity>($"{BaseUrl}/Create", entity);

        public TEntity Update(TEntity entity)
            => HttpHelper.Post<TEntity>($"{BaseUrl}/Update", entity);

        public bool Delete(TEntity entity)
        => HttpHelper.Post<bool>($"{BaseUrl}/Delete");
        public TEntity Get(int id)
            => HttpHelper.Get<TEntity>($"{BaseUrl}/{id}");

        public List<TEntity> GetAll()
            => HttpHelper.Get<List<TEntity>>($"{BaseUrl}/all");
    }
}
