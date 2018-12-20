using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MinecraftServerScanner.Library;
using MinecraftServerScanner.Server.Models;

namespace MinecraftServerScanner.Server.Controllers
{
    public class MinecraftServerController : Controller
    {
        private MinecraftContext _db;

        public MinecraftServerController(MinecraftContext db)
        {
            _db = db;
        }
        

        public IActionResult Index()
        {
            return View();
        }

        [Route("api/v1/minecraft-servers")]
        public JsonResult List([FromQuery(Name = "page")] Int32 page, [FromQuery(Name = "size")] Int32 size)
        {
            var items = _db.MinecraftServers
                .OrderByDescending(s => s.Scanned)
                .Skip(page * size)
                .Take(size)
                .ToList();

            return Json(
                new PageOverview<MinecraftServer>(page, size, items));
        }

        [Route("api/v1/minecraft-servers/{id}/sync")]
        public JsonResult Sync(Int32 id)
        {
            var server = _db.MinecraftServers.Where(s => s.Id == id).First();

            server.Scan();

            _db.SaveChanges();

            return Json(server);
        }
    }
}