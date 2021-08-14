using eCommerceSite.Data;
using eCommerceSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductContext _context;

        public ProductController(ProductContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays a view that lists all products
        /// </summary>
        public async Task<IActionResult> Index()
        {
            // Async with Query Syntax
            //List<Product> products =
            //    await (from p in _context.Products
            //           select p).ToListAsync();

            // Get all products from DB
            List<Product> products = await _context.Products.ToListAsync();

            // Send list of products to view to be displayed
            return View(products);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product p) // Use Async with DB code
        {
            if (ModelState.IsValid)
            {
                // Add to DB
                _context.Products.Add(p);
                await _context.SaveChangesAsync();

                // last through one redirect
                TempData["Message"] = $"Your {p.Title} was added successfully!";

                // redirect to catalog page
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
           // Get product with corrisponding ID
           Product p =
               await (from prod in _context.Products
                where prod.ProductId == id
                select prod).SingleAsync();

            // Product p2 = await _context.Products
            //      .Where(prod => prod.ProductID == id)
            //      .SingleAsync();

            // Pass product to view
            return View(p);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product p)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(p).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                // last through one redirect
                TempData["Message"] = $"Your {p.Title} has been edited!";

                // redirect to catalog page
                return RedirectToAction("Index");
            }

            return View(p);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Product p = await (from prod in _context.Products
                         where prod.ProductId == id
                         select prod).SingleAsync();
            return View(p);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Product p = await (from prod in _context.Products
                        where prod.ProductId == id
                        select prod).SingleAsync();

            _context.Entry(p).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            TempData["Message"] = $"Your {p.Title} has been deleted!";

            return RedirectToAction("Index");
        }
    }
}
