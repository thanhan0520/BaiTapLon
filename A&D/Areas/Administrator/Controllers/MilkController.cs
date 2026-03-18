using Microsoft.AspNetCore.Mvc;
using A_D.Models;
using Microsoft.EntityFrameworkCore;

namespace A_D.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class MilkController : Controller
    {
        private readonly QuanLiBanSuaContext _dbContext;
        private readonly IWebHostEnvironment _hostEnvironment;

        public MilkController(QuanLiBanSuaContext dbContext, IWebHostEnvironment hostEnvironment)
        {
            _dbContext = dbContext;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            var milks = _dbContext.Milk.ToList();
            return View(milks);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Milk milk, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
                    Directory.CreateDirectory(uploadsFolder);
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(fileStream);
                    }
                    milk.ImagePath = "/images/" + uniqueFileName;
                }

                _dbContext.Milk.Add(milk);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(milk);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var milk = _dbContext.Milk.FirstOrDefault(m => m.MilkId == id);
            if (milk == null) return NotFound();
            return View(milk);
        }

        [HttpPost]
        public IActionResult Edit(int id, Milk milk, IFormFile imageFile)
        {
            if (id != milk.MilkId) return BadRequest();

            if (ModelState.IsValid)
            {
                var existingMilk = _dbContext.Milk.FirstOrDefault(m => m.MilkId == id);
                if (existingMilk == null) return NotFound();

                existingMilk.MilkName = milk.MilkName;
                existingMilk.Brand = milk.Brand;
                existingMilk.Weight = milk.Weight;
                existingMilk.Price = milk.Price;
                existingMilk.StockQuantity = milk.StockQuantity;
                existingMilk.ExpiryDate = milk.ExpiryDate;

                if (imageFile != null && imageFile.Length > 0)
                {
                    if (!string.IsNullOrEmpty(existingMilk.ImagePath))
                    {
                        string oldFilePath = Path.Combine(_hostEnvironment.WebRootPath, existingMilk.ImagePath.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                            System.IO.File.Delete(oldFilePath);
                    }

                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
                    Directory.CreateDirectory(uploadsFolder);
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(fileStream);
                    }
                    existingMilk.ImagePath = "/images/" + uniqueFileName;
                }

                _dbContext.Milk.Update(existingMilk);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(milk);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var milk = _dbContext.Milk.FirstOrDefault(m => m.MilkId == id);
            if (milk == null) return NotFound();

            if (!string.IsNullOrEmpty(milk.ImagePath))
            {
                string filePath = Path.Combine(_hostEnvironment.WebRootPath, milk.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }

            _dbContext.Milk.Remove(milk);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
