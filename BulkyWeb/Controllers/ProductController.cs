using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductController(ApplicationDbContext db)
        {
            _db = db;
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
        public IActionResult Create(Product obj)
        {
            if (ModelState.IsValid)
            {
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
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _db.Products.Update(obj);
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
    }
}
