using System.Collections.Generic;
using System.Web.Http;
using Dominisoft.Nokates.Common.Infrastructure.Repositories;
using Dominisoft.Nokates.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dominisoft.Nokates.Common.Infrastructure.Controllers
{
    [ApiController]
    public class BaseController<TEntity> : ApiController where TEntity : Entity,new()
    {
        protected readonly SqlRepository<TEntity> Repository;

        public BaseController(SqlRepository<TEntity> repository)
        {
            this.Repository = repository;
        }
        [Microsoft.AspNetCore.Mvc.HttpPost("Create")]
        public virtual TEntity Create([Microsoft.AspNetCore.Mvc.FromBody] TEntity e)
        {
            var id = Repository.Create(e);
            return Get(id);
        }
        [Microsoft.AspNetCore.Mvc.HttpGet("{id}")]
        public virtual TEntity Get(long id)
            => Repository.Get(id);
        [Microsoft.AspNetCore.Mvc.HttpGet("All")]
        public virtual List<TEntity> GetAll()
            => Repository.GetAll();

        [Microsoft.AspNetCore.Mvc.HttpPost("Update")]
        public virtual TEntity Update(TEntity entity)
            => Repository.Update(entity);

        [Microsoft.AspNetCore.Mvc.HttpPost("Delete")]
        public virtual bool Delete(TEntity entity)
            => Repository.Delete(entity);
    }
}
