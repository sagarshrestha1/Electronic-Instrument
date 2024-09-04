using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using project.Models;
using project.Services;
using System;


using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext context;

        public object Session { get; private set; }

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            
            this.context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.Setsession = HttpContext.Session.GetString("useremail");
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public IActionResult Login()
        {
            

            return View();
        }
        [HttpPost]
        public IActionResult Login(string email,string password)
        {
            
            var users = context.Users.Where(x => x.Email == email).FirstOrDefault();
            if (users.Password == password)
            {
                HttpContext.Session.SetString("useremail", email);
                HttpContext.Session.SetString("password", password);
                return RedirectToAction("Index", "Home");
                
            }
            else
            {
                ModelState.AddModelError("password", "The username or password is incorrect");
            }
            
            


            return View();
        }
      
        
    
        
        public IActionResult Registration()
        {

            
            return View();
        }
        [HttpPost]
        public IActionResult Registration(User user)
        {
            if(!ModelState.IsValid)
            {
                return View(user);
            }
            User user1 = new User();
            user1.FullName=user.FullName.Trim();
            user1.Email=user.Email.Trim();  
            user1.Password=user.Password.Trim();
            context.Users.Add(user1);
            context.SaveChanges();

            return View(user1);

        }
        
          
    }
}
