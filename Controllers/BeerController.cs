using IntroAsp.Models;
using IntroAsp.Views.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IntroAsp.Controllers
{
    public class BeerController : Controller
    {
        private readonly PubContext _context;

        public BeerController(PubContext context)
        {
            _context = context;
        }   

        //GET DB
        public async Task<IActionResult> Index()
        {
            var beers = _context.Beers.Include(b => b.Brand);
            return View(await beers.ToListAsync());
        }


        //GET SELECT 
        public IActionResult Create() 
        {
            ViewData["Brands"] = new SelectList(_context.Brands, "BrandId", "Name");
            return View();
        }

        //POST FORM
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create(BeerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var beer = new Beer()
                {
                    Name = model.Name,
                    BrandId = model.BrandId
                };
                _context.Add(beer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


            ViewData["Brands"] = new SelectList(_context.Brands, "BrandId", "Name",model.BrandId);
            return View(model);
        }


        // GET: Beer/Edit/{id}
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var beer = await _context.Beers.Include(b => b.Brand).FirstOrDefaultAsync(m => m.BeerId == id);
            if (beer == null)
            {
                return NotFound();
            }

            var model = new BeerViewModel
            {
                BeerId = beer.BeerId,
                Name = beer.Name,
                BrandId = beer.BrandId
            };

            ViewBag.Brands = new SelectList(_context.Brands, "BrandId", "Name", beer.BrandId);
            return View(model);
        }



        // POST: Beer/Edit/{id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BeerViewModel model)
        {
            try
                {
                    var beer = await _context.Beers.FindAsync(id);
                    
                    if (beer == null)
                    {
                        return NotFound();
                    }

                    beer.Name = model.Name;
                    beer.BrandId = model.BrandId;

                    _context.Update(beer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                    { 
                        throw;
                    }
                return RedirectToAction(nameof(Index));
        }


        //DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int beerId)
        {
            var beer = await _context.Beers.FindAsync(beerId);
            if (beer == null)
            {
                return NotFound();
            }

            _context.Beers.Remove(beer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
