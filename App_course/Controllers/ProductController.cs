using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App_course.Data;
using App_course.Models;
using App_course.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace App_course.Controllers
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

        // GET: /<controller>/
        public IActionResult Index()
        {
            IEnumerable<Product> objList = _db.Product.Include(u =>u.Category).Include(u=>u.ApplicationType);

            //foreach (var obj in objList)
            //{
            //    obj.Category = _db.Category.FirstOrDefault(u => u.Id == obj.CategoryId);
            //    obj.ApplicationType = _db.ApplicationType.FirstOrDefault(u => u.Id == obj.ApplicationTypeId);
            //};

            return View(objList);
        }

        // GET : UPSERT
        public IActionResult Upsert (int? id)
        {
            //IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(i => new SelectListItem
            //{
            //    Text = i.Name,
            //    Value = i.Id.ToString()
            //});

            //ViewBag.CategoryDropDown = CategoryDropDown;

            //Product product = new Product();

            ProductVM productVM = new ProductVM()
            {
                product = new Product(),
                CategorySelectList = _db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                ApplicationTypeSelectList = _db.ApplicationType.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                //this is for create
                return View(productVM);
            }
            else
            {
                productVM.product = _db.Product.Find(id);
                if (productVM.product == null)
                {
                    return NotFound();
                }
                return View(productVM);
            }
            
        }

        // POST : UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productVM.product.Id == 0)
                {
                    //create
                    string upload = webRootPath + WC.ImagePath;
                    string filename = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, filename + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVM.product.Image = filename + extension;

                    _db.Product.Add(productVM.product);
                }

                else
                {
                    //update
                    var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(u => u.Id == productVM.product.Id);

                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WC.ImagePath;
                        string filename = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.Image);

                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, filename + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productVM.product.Image = filename + extension;
                    }
                    else
                    {
                        productVM.product.Image = objFromDb.Image;
                    }
                    _db.Product.Update(productVM.product);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            productVM.CategorySelectList = _db.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            productVM.ApplicationTypeSelectList = _db.ApplicationType.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            return View(productVM);
        }

        

        // GET : DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product product = _db.Product.Include(u=>u.Category).Include(u=>u.ApplicationType).FirstOrDefault(u => u.Id == id);

            //product.Category = _db.Category.Find(product.CategoryId);


            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST : DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id, ProductVM productVM)
        {

            var obj = _db.Product.Find(id);
            if (obj == null)
                return NotFound();

            string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;
            var oldFile = Path.Combine(upload, obj.Image);

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }

            _db.Product.Remove(obj);
            _db.SaveChanges();

            
            return RedirectToAction("Index");
        }
    }
}
