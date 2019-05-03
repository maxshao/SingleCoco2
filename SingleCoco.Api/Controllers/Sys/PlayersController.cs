using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SingleCoco.Api.Infrastructure;
using SingleCoco.Entities.Systerm;
using SingleCoco.Infrastructure;
using SingleCoco.Repository;
using SingleCoco.Repository.ISystem;

namespace SingleCoco.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllersBase
    {
        private readonly IPlayersRepository _playersRepository;

        public PlayersController(IPlayersRepository playersRepository)
        {
            _playersRepository = playersRepository;
        }

        [HttpGet("getplayers")]
        public JsonResult GetPlayers()
        {
            var palyers = _playersRepository.GetAll();
            return Json(palyers);
        }

        [HttpGet("getplayer/{playerId}")]
        public JsonResult GetPlayer(int playerId)
        {
            var player = _playersRepository.Get(playerId);
            dynamic obj = new
            {
                player.Id,
                player.Name,
                player.IsEable,
                Keys = player.Keys.Split(',')
            };
            return Json(obj);
        }

        [HttpPost("save")]
        [ValidateFilter]
        public JsonResult SavePlayers([CustomizeValidator]Players play)
        {
            Dictionary<string, string[]> dic = new Dictionary<string, string[]>();// 用于排序


            // object to where
            //long x = _playersRepository.Count(t => t.Id);
            //_playersRepository.GetPage(new Page(), where: new
            //{
            //    Id = 1
            //}, order: t => t.Id);


            return Json("111");
        }

    }
}