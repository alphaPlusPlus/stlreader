using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using STLReader.Models;

namespace STLReader.Controllers
{
    public class HomeController : Controller
    {
        private IHostingEnvironment _env;
        public HomeController(IHostingEnvironment env)
        {
            _env = env;
        }


        public async Task<IActionResult> Index()
        {

            /*var webRoot = _env.WebRootPath;
            var file = System.IO.Path.Combine(webRoot, "files\\PumpkinsCombined.stl");

            var stlFile = Kaitai.Stl.FromFile(file);
            var json = JsonConvert.SerializeObject(stlFile, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            });*/

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile file)
        {

            // full path to file in temp location
            var filePath = Path.GetTempFileName();
            var json = "";
            if (file.Length > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var stlFile = Kaitai.Stl.FromFile(filePath);
                json = JsonConvert.SerializeObject(stlFile, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                });
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return new JsonResult(new { STL = json });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
