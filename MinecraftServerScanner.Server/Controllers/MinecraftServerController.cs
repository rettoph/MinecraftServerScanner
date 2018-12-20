using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MinecraftServerScanner.Server.Controllers
{
    public class MinecraftServerController : Controller
    {
        private MincraftContext _db;

        public MinecraftServerController(MincraftContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("api/v1/minecraft-servers")]
        public JsonResult List()
        {
            return Json(_db.MinecraftServers);
        }
    }
}