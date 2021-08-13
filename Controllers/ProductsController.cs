using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Storage.Data;
using Storage.Models;
using System.Web;


namespace Storage.Controllers
{
    public class ProductsController : Controller
    {
        private readonly StorageContext _context;

        public ProductsController(StorageContext context)
        {
             _context = context;
           
        }


        // GET: Products
        /*
        public async Task<IActionResult> Index()
        {
            return View(await _context.Product.ToListAsync());
        }
        */


        /*
        public async Task<IActionResult> Index(string Category)
        {
            var milkChocolate = _context.Product.Where(i => i.Category == "Milk");       
            return View(await milkChocolate.AsNoTracking().ToListAsync());
        }
        */


            public ActionResult Index(string Name, string Category)       
        {     
            var CategoryList = new List<string>();
            var CategoryQuery = from q in _context.Product orderby q.Category select q.Category;        

            CategoryList.AddRange(CategoryQuery.Distinct()); 
            
            ViewBag.Name = new SelectList(_context.Product, "Name");
            ViewBag.Category = new SelectList(CategoryList);  

            var product = from q in _context.Product select q;
            if (!String.IsNullOrEmpty(Name))
            {
                product = product.Where(s => s.Name.StartsWith(Name));
            }
         
            if (!String.IsNullOrEmpty(Category))            {
                product = product.Where(s => s.Category == Category);
            }

            var myProductList = product.ToList();
            IList<Product> prodList = new List<Product>();
            foreach (var item in myProductList)
            {
                prodList.Add(new Product()
                {
                    Id = item.Id,           
                    Name = item.Name,
                    Price = item.Price,
                    Orderdate = item.Orderdate,
                    Category = item.Category,
                    Shelf = item.Shelf,
                    Count = item.Count,
                    Description = item.Description
                });
            }
            return View(nameof(Index), prodList);
        }   


        public async Task<IActionResult> List()
        {
            var model = _context.Product.Select(e => new ProductViewModel
            {
                Id = e.Id,
                Name = e.Name,
                Price = e.Price,
                Count = e.Count,
                InventoryValue = e.Price * e.Count
            });
            return View(await model.ToListAsync());
        }





        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Orderdate,Category,Shelf,Count,Description")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Orderdate,Category,Shelf,Count,Description")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }


    }
}
