using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using SingleCoco.Api.Infrastructure;
using SingleCoco.Dtos.Query.Furniture;
using SingleCoco.Entities.Furniture;
using SingleCoco.Repository.IFurniture;

namespace SingleCoco.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FurnitureAgentController : ControllersBase
    {
        private readonly IAgentsRepository _agentsRepository;
        public FurnitureAgentController(IAgentsRepository agentsRepository)
        {
            _agentsRepository = agentsRepository;
        }


        [HttpPost("gets")]
        public JsonResult GetAgents(AgentsQuery q)
        {

            var agents = _agentsRepository.GetPage(q.PageIndex, q.PageSize, null, null);

            return Json(agents);
        }

        [HttpGet("get")]
        public JsonResult GetAgent(int agentId)
        {
            return Json(_agentsRepository.Get(agentId));
        }

        [HttpGet("delete")]
        public JsonResult DeleteAgent(int agentId)
        {
            return Json(_agentsRepository.Delete(agentId).ToString());
        }

        [HttpPost("save")]
        [ValidateFilter]
        public JsonResult SaveAgent([CustomizeValidator]AgentDto agentDto)
        {
            var p = new Agents()
            {
                Id = agentDto.Id,
                Name = agentDto.Name,
                Phone = agentDto.Phone
            };
            return Json(_agentsRepository.Add(p).ToString());
        }


    }
}