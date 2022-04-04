using Dominisoft.Nokates.Common.Infrastructure.Extensions;
using Dominisoft.Nokates.Common.Infrastructure.Helpers;
using Dominisoft.Nokates.Common.Models;

namespace Dominisoft.Nokates.Common.Infrastructure.Client
{
    public class BaseClient<TEntity> where TEntity : Entity
    {
        public string BaseUrl { get; set; }
        public string Token { get; set; }

        public TEntity Create(TEntity entity)
        {
            var json = entity.Serialize();
            return HttpHelper.Post<TEntity>($"{BaseUrl}/Create", json, $"{Token}");
        }
        public TEntity Update(TEntity entity)
        {
            var json = entity.Serialize();
            return HttpHelper.Post<TEntity>($"{BaseUrl}/Update", json, $"{Token}");
        }
        public bool Delete(TEntity entity)
        {
            var json = entity.Serialize();
            return HttpHelper.Post<bool>($"{BaseUrl}/Delete", json, $"{Token}");
        }
        public TEntity Get(int id)
        {
            return HttpHelper.Get<TEntity>($"{BaseUrl}/{id}",  $"{Token}");
        }
    }
}
