using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProdCat.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;



namespace ProdCat.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext = context;
        }
        [HttpGet("products")]
        public IActionResult Products()
        {
            List<Product> AllProducts = dbContext.Products.OrderByDescending(p=>p.CreatedAt).ToList();
            ViewBag.products = AllProducts;
            return View();
        }
        
        [HttpGet("products/{productID}")]
        public IActionResult AddCategoryToProduct(int productID)
        {
            Product currentProduct = dbContext.Products.Where(p=>p.ProductId == productID).FirstOrDefault();
            ViewBag.currentprod = currentProduct;
            Product selectedProd = dbContext.Products.Include(p=>p.CategoriesWithProducts).ThenInclude(p=>p.Category).FirstOrDefault(c => c.ProductId == productID);
            ViewBag.prodcats = selectedProd;
            List<Category> AllCategories = dbContext.Categories.OrderByDescending(c=>c.CategoryId).ToList();
            List<Category> NewList = new List<Category>{};
            foreach(var cat in selectedProd.CategoriesWithProducts)
            {
                NewList.Add(cat.Category);
            }
            List<Category> CatsNotInProd = AllCategories.Except(NewList).OrderByDescending(c=>c.CategoryId).ToList();
            ViewBag.notinprod = CatsNotInProd;
            return View();
        }
        
        [HttpPost("addcategorytoproduct")]
        public IActionResult AddCategory(Association newAssociation)
        {
            dbContext.Add(newAssociation);
            dbContext.SaveChanges();
            return Redirect("/products");
        }

        [HttpPost("newproduct")]
        public IActionResult NewProduct(Product newProduct)
        {
            dbContext.Add(newProduct);
            dbContext.SaveChanges();
            return RedirectToAction("Products");
        }
///////////////////////////////////////////////////////
        [HttpGet("categories")]
        public IActionResult Categories()
        {
            List<Category> AllCategories = dbContext.Categories.OrderByDescending(p=>p.CreatedAt).ToList();
            ViewBag.categories = AllCategories;
            return View();
        }

        [HttpGet("categories/{categoryID}")]
        public IActionResult AddProductToCategory(int categoryID)
        {
            Category currentCategory = dbContext.Categories.Where(c=>c.CategoryId == categoryID).FirstOrDefault();
            ViewBag.currentcat = currentCategory;
            Category selectedCat = dbContext.Categories.Include(c=>c.ProductsWithCategories).ThenInclude(p=>p.Product).FirstOrDefault(c => c.CategoryId == categoryID);
            ViewBag.catprods = selectedCat;
            List<Product> AllProducts = dbContext.Products.OrderByDescending(p=>p.ProductId).ToList();
            List<Product> NewList = new List<Product>{};
            foreach(var prod in selectedCat.ProductsWithCategories)
            {
                NewList.Add(prod.Product);
            }
            List<Product> ProdsNotInCat = AllProducts.Except(NewList).OrderByDescending(p=>p.ProductId).ToList();
            ViewBag.notincat = ProdsNotInCat;
            return View();
        }

        [HttpPost("addproducttocategory")]
        public IActionResult AddProduct(Association newAssociation)
        {
            dbContext.Add(newAssociation);
            dbContext.SaveChanges();
            return Redirect("/categories");
        }

        [HttpPost("newcategory")]
        public IActionResult NewCategory(Category newCategory)
        {
            dbContext.Add(newCategory);
            dbContext.SaveChanges();
            return RedirectToAction("Categories");
        }
///////////////////////////////////////////////////////
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
