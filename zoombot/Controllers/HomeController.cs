using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using zoombot.Models;
using zoombot.Services;

namespace zoombot.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var model = new BotModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(
            BotModel model
            )
        {
            Console.WriteLine("Launching bot");
            BotLauncher.LaunchBot(model);
            return Redirect("/");
        }

        public IActionResult Delete(
            string id
            )
        {
            Console.WriteLine($"Deleting bot for {id}");
            BotLauncher.DeleteBot(id);
            return Redirect("/");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
