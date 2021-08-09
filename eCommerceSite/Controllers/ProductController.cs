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
    }
}
