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
        /// Displays a view that lists a page of products
        /// </summary>
        public async Task<IActionResult> Index(int? id)
        {
            // can be condensed further! int pageNum = id ?? 1; null coalesing operator ??
            int pageNum = id.HasValue ? id.Value : 1; // if else ternary operator
            const int PageSize = 3;
            ViewData["CurrentPage"] = pageNum;

            int numProducts = await ProductDB.GetTotalProductsAsync(_context);

            int totalPages = (int)Math.Ceiling((double)numProducts / PageSize);

            ViewData["MaxPage"] = totalPages;


            //Async with Query Syntax
            List<Product> products =
                await ProductDB.GetProductsAsync(_context, PageSize, pageNum);

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
                await ProductDB.AddProductAsync(_context, p);

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
           Product p = await ProductDB.GetSingleProductAsync(_context, id);

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
            Product p = await ProductDB.GetSingleProductAsync(_context, id);
            return View(p);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Product p = await ProductDB.GetSingleProductAsync(_context, id);

            _context.Entry(p).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            TempData["Message"] = $"Your {p.Title} has been deleted!";

            return RedirectToAction("Index");
        }
    }
}
