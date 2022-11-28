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

        RestResponse<TEntity> Create(TEntity entity,string token);
        RestResponse<TEntity> Update(TEntity entity, string token);
        RestResponse Delete(TEntity entity, string token);
        RestResponse<TEntity> Get(int id, string token);
        RestResponse<List<TEntity>> GetAll(string token);

    }
    public class BaseClient<TEntity> : IBaseClient<TEntity> where TEntity : Entity 
    {
        public string BaseUrl { get; set; }

        public RestResponse<TEntity> Create(TEntity entity)
            => HttpHelper.Post<TEntity>($"{BaseUrl}/Create", entity);

        public RestResponse<TEntity> Update(TEntity entity)
            => HttpHelper.Post<TEntity>($"{BaseUrl}/Update", entity);

        public RestResponse Delete(TEntity entity)
            => HttpHelper.Post($"{BaseUrl}/Delete", entity);
        public RestResponse<TEntity> Get(int id)
            => HttpHelper.Get<TEntity>($"{BaseUrl}/{id}");

        public RestResponse<List<TEntity>> GetAll()
        {

            var result = HttpHelper.Get<List<TEntity>>($"{BaseUrl}/all");

            return result;

        }

        public RestResponse<TEntity> Create(TEntity entity,string token)
            => HttpHelper.Post<TEntity>($"{BaseUrl}/Create", entity,token);

        public RestResponse<TEntity> Update(TEntity entity, string token)
            => HttpHelper.Post<TEntity>($"{BaseUrl}/Update", entity, token);

        public RestResponse Delete(TEntity entity, string token)
            => HttpHelper.Post($"{BaseUrl}/Delete", entity, token);
        public RestResponse<TEntity> Get(int id, string token)
            => HttpHelper.Get<TEntity>($"{BaseUrl}/{id}", token);

        public RestResponse<List<TEntity>> GetAll( string token)
        {

            var result = HttpHelper.Get<List<TEntity>>($"{BaseUrl}/all", token);

            return result;

        }
    }
}
