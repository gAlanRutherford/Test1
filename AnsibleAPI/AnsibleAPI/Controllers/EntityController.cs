using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using AnsibleAPI.Domain.Abstract;
using AnsibleAPI.Domain.Entities;

namespace AnsibleAPI.Controllers
{
    public class EntityController : ApiController
    {
        public readonly IEntityRepository _entityRepository;
        public EntityController(IEntityRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        [HttpGet]
        [Route("api/v1/entities")]
        public IHttpActionResult GetEntities()
        {
            return Ok(_entityRepository.GetEntities());
        }

        [HttpGet]
        [Route("api/v1/entities/{entityId}")]
        public IHttpActionResult GetEntity(int entityId)
        {
            Entity entity = _entityRepository.GetEntiy(entityId);
            if (entity == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(entity);
            }
        }

        [HttpPost]
        [Route("api/v1/entity")]
        public IHttpActionResult AddEntity([FromBody] Entity entity)
        {
            if (entity == null || string.IsNullOrWhiteSpace(entity.Name) || entity.Id != null)
            {
                return BadRequest("Invalid data");
            }
            var result = _entityRepository.AddEntity(entity);
            return Ok(result);
        }

        [HttpPost]
        [Route("api/v1/entities")]
        public IHttpActionResult AddEntities(IEnumerable<Entity> entities)
        {
            if(entities == null || entities.Any(x => string.IsNullOrWhiteSpace(x.Name) || x.Id != null))
            {
                return BadRequest("Invalid data");
            }

            var result = _entityRepository.AddEntities(entities);
            return Ok(result);
        }
    }
}
