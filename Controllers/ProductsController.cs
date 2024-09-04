using Microsoft.AspNetCore.Mvc;
using project.Models;
using project.Services;

namespace project.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public ProductsController(ApplicationDbContext context,IWebHostEnvironment environment)
        {
           this.context = context;
            this.environment = environment;
        }
        public IActionResult Index()
        {
            string setsession = HttpContext.Session.GetString("useremail");
            ViewBag.Setsession = setsession;
            var products = context.Products.OrderByDescending(p=>p.Id).ToList();

            return View(products);
        }
        public IActionResult Create() {
        return View();
        }
        [HttpPost]
        public IActionResult Create(ProductDto productDto)
        {
            string setsession = HttpContext.Session.GetString("useremail");
            ViewBag.Setsession = setsession;
            if (productDto.ImageFile==null)
            {
                ModelState.AddModelError("ImageFile", "This image file is required");

            }
            if(!ModelState.IsValid)
            {
                return View(productDto);
            }
            //save the image file
            string newFileName = DateTime.Now.ToString("yyyMMddHHmmssfff");
            newFileName += Path.GetExtension(productDto.ImageFile!.FileName);
            string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath)) 
            {
                productDto.ImageFile.CopyTo(stream);
            }
            //save the new product in the database
            Product product = new Product()
            {
                Name = productDto.Name,
                Brand = productDto.Brand,
                Category = productDto.Category,
                Price = productDto.Price,
                Description = productDto.Description,
                ImageFileName = newFileName,
                CreatedAt = DateTime.Now,
            };
            context.Products.Add(product);
            context.SaveChanges();
            return RedirectToAction("Index", "Products");
            
        }
        public IActionResult Edit(int id)
        {
            string setsession = HttpContext.Session.GetString("useremail");
            ViewBag.setsession = setsession;
            var product = context.Products.Find(id);
            if(product == null)
            {
                RedirectToAction("Index", "Products");
            }
            var productDto = new ProductDto()
            {
                Name = product.Name,
                Brand = product.Brand,
                Category = product.Category,
                Price = product.Price,
                Description = product.Description,
            };
            ViewData["ProductId"] = product.Id;
            ViewData["ImageFileName"] = product.ImageFileName;
            ViewData["CreatedAt"]= product.CreatedAt.ToString("MM/dd/yyyy");
            return View(productDto);
        }
        [HttpPost]
        public IActionResult Edit(int id,ProductDto productDto) 
        {
            string setsession = HttpContext.Session.GetString("useremail");
            ViewBag.setsession = setsession;
            var product =context.Products.Find(id);
            if(product == null)
            {
                return RedirectToAction("Index", "Products");
            }
            if(!ModelState.IsValid)
            {
                ViewData["ProductId"]=product.Id;
                ViewData["ImageFileName"] = product.ImageFileName;
                ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");
                return View(productDto);
            }
            //update the image file if we have a new image file
            string newFileName = product.ImageFileName;
            if(productDto.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(productDto.ImageFile.FileName);
                string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
                using(var stream = System.IO.File.Create(imageFullPath))
                {
                    productDto.ImageFile.CopyTo(stream);    

                }
                //delete the old image
                string oldImageFullPath = environment.WebRootPath + "/products/" + product.ImageFileName;
                System.IO.File.Delete(imageFullPath);
            }
            //update the product in the database
            product.Name = productDto.Name;
            product.Brand = productDto.Brand;
            product.Category = productDto.Category;
            product.Price = productDto.Price;
            product.Description = productDto.Description;
            product.ImageFileName = newFileName;
            context.SaveChanges();
            return RedirectToAction("Index","Products");


        }
        public IActionResult Delete(int id)
        {
            string setsession = HttpContext.Session.GetString("useremail");
            ViewBag.setsession = setsession;
            var product = context.Products.Find(id);
            if(product == null)
            {
                return RedirectToAction("Index", "Products");

            }
            string imageFullPath = environment.WebRootPath + "/products/" + product.ImageFileName;
            System.IO.File.Delete(imageFullPath);
            context.Products.Remove(product);
            context.SaveChanges();
            return RedirectToAction("Index", "Products");

        }
    }
        
}
