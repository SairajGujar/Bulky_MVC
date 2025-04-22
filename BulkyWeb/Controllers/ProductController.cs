using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            
            List<Product> products = _db.Products.Include(p=>p.Category).ToList();
            return View(products);
        }

        public IActionResult Create()
        {
            ViewBag.CategoryList = new SelectList(_db.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product obj, IFormFile? image)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\products");
                    using(var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        image.CopyTo(fileStream);
                    }
                    obj.ImageURL = @"\images\products\" + fileName;
                }
                _db.Products.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            ViewBag.Error = ModelState.ErrorCount;
            ViewBag.CategoryList = new SelectList(_db.Categories, "Id", "Name");
            return View();
        }


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            var product = _db.Products.Find(id);
            if (product == null) return NotFound();
            ViewBag.CategoryList = new SelectList(_db.Categories, "Id", "Name");
            return View(product);
        }
        [HttpPost]
        public IActionResult Edit(Product product, IFormFile? image)
        {
            if (ModelState.IsValid)
            {
                if (image != null && image.Length > 0)
                {
                    // Generate unique filename
                    string wwwRootPath = _webHostEnvironment.WebRootPath;

                    string uploadsFolder = Path.Combine(wwwRootPath, "images", "products");
                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Create directory if not exists
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    // Optional: Delete the old image
                    if (!string.IsNullOrEmpty(product.ImageURL))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, product.ImageURL.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                            System.IO.File.Delete(oldImagePath);
                    }

                    // Save new image
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyTo(fileStream);
                    }

                    // Update image URL in the database model
                    product.ImageURL = "/images/products/" + uniqueFileName;
                }
                _db.Products.Update(product);
                _db.SaveChanges();
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index");
            }
            ViewBag.CategoryList = new SelectList(_db.Categories, "Id", "Name");
            return View();

        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();
            var product = _db.Products.Include(p=>p.Category).FirstOrDefault(p=>p.Id==id);
            if (product == null) return NotFound();
            return View(product);
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? obj = _db.Products.Find(id);
            if (obj == null) return NotFound();
            _db.Products.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");

        }

        #region API Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _db.Products.Include(p => p.Category).ToList();
            return Json(new { data = productList });
        }
        #endregion

    }
}
