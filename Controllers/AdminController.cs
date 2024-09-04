using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project.Models;
using project.Services;

namespace project.Controllers

{
    
    [Route("admin")]
    public class AdminController : Controller
    {
        ApplicationDbContext context;
        public AdminController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            return View();
        }
        [HttpGet("users")]
        public IActionResult Users()
        {
            return View();
        }
        [Route("Products")]
        public IActionResult Products()
        {
            HttpContext.Session.SetString("admin", "admin");
            string admin = HttpContext.Session.GetString("admin");
            ViewBag.Setsession = admin;
            var products = context.Products.OrderByDescending(p => p.Id).ToList();

            return View(products);

            
        }
    }
}
