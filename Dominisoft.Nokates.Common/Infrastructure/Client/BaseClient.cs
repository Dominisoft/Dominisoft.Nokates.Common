using System.Collections.Generic;
using Dominisoft.Nokates.Common.Infrastructure.Helpers;
using Dominisoft.Nokates.Common.Models;

namespace Dominisoft.Nokates.Common.Infrastructure.Client
{
    public interface IBaseClient<TEntity> where TEntity : Entity
    {
        RestResponse<TEntity> Create(TEntity entity);
        RestResponse<TEntity> Update(TEntity entity);
        RestResponse Delete(TEntity entity);
        RestResponse<TEntity> Get(int id);
        RestResponse<List<TEntity>> GetAll();
    }
    public class BaseClient<TEntity> : IBaseClient<TEntity> where TEntity : Entity 
    {
        public string BaseUrl { get; set; }

        public RestResponse<TEntity> Create(TEntity entity)
            => HttpHelper.Post<TEntity>($"{BaseUrl}/Create", entity);

        public RestResponse<TEntity> Update(TEntity entity)
            => HttpHelper.Post<TEntity>($"{BaseUrl}/Update", entity);

        public RestResponse Delete(TEntity entity)
        => HttpHelper.Post($"{BaseUrl}/Delete",entity);
        public RestResponse<TEntity> Get(int id)
            => HttpHelper.Get<TEntity>($"{BaseUrl}/{id}");

        public RestResponse<List<TEntity>> GetAll()
        {

            var result = HttpHelper.Get<List<TEntity>>($"{BaseUrl}/all");

            return result;

        }
    }
}
