using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Recipease.Data;
using Recipease.Models;

namespace Recipease.Controllers
{
    public class RecipesController : Controller
    {
        private readonly RecipeaseContext _context;

        public RecipesController(RecipeaseContext context)
        {
            _context = context;
        }

        // GET: Recipes
        public async Task<IActionResult> Index(string searchString, string recipeCuisine)
        {
            if (_context.Recipe == null)
            {
                return Problem("Entity set 'Recipease.Recipe'  is null.");
            }

            IQueryable<string> cuisineQuery = from r in _context.Recipe
                                            orderby r.Cuisine
                                            select r.Cuisine;

            var recipes = from r in _context.Recipe
                         select r;

            if (!String.IsNullOrEmpty(searchString))
            {
                recipes = recipes.Where(s => s.Title!.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(recipeCuisine))
            {
                recipes = recipes.Where(x => x.Cuisine == recipeCuisine);
            }

            var recipeCuisineVM = new CuisineViewModel
            {
                Cuisines = new SelectList(await cuisineQuery.Distinct().ToListAsync()),
                Recipes = await recipes.ToListAsync()
            };

            return View(recipeCuisineVM);
        }

        [HttpPost]
        public string Index(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Recipe == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe
                .FirstOrDefaultAsync(r => r.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // GET: Recipes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,DateAdded,Cuisine,Price,Rating")] Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recipe);
        }

        // GET: Recipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Recipe == null)
            {
                return NotFound();
            }
            

            var recipe = await _context.Recipe.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
            return View(recipe);
        }

        public async Task<IActionResult> Rate(int? id)
        {
            var rand = new Random();
            var rand_id = new int();

            do
            {
                rand_id = _context.Recipe.OrderBy(o => Guid.NewGuid()).First().Id;
                Console.WriteLine(rand_id);
            }
            while (rand_id == id);




            if (rand_id == null || _context.Recipe == null)
            {
                
            }
            


            var recipe = await _context.Recipe
                .FirstOrDefaultAsync(r => r.Id == rand_id);

            
            if (recipe == null)
            {
                return NotFound();
            }


            if (_context.Recipe == null)
            {
                return Problem("Entity set 'Recipease.Recipe'  is null.");
            }

            var recipes = from r in _context.Recipe
                          select r;

            
            return View(recipe);
        }

        [HttpPost, ActionName("Good")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Good(int id)
        {


            if (_context.Recipe == null)
            {
                return Problem("Entity set 'RecipeaseContext.Recipe'  is null.");
            }
            var recipe = await _context.Recipe.FindAsync(id);
            if (recipe != null)
            {
                recipe.Rating = recipe.Rating + 1;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Rate));
            
            
        }
        [HttpPost, ActionName("Bad")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Bad(int id)
        {


            if (_context.Recipe == null)
            {
                return Problem("Entity set 'RecipeaseContext.Recipe'  is null.");
            }
            var recipe = await _context.Recipe.FindAsync(id);
            if (recipe != null)
            {
                recipe.Rating = recipe.Rating - 1;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Rate));


        }
        

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,DateAdded,Cuisine,Price")] Recipe recipe)
        {
            if (id != recipe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.Id))
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
            return View(recipe);
        }

        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Recipe == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Recipe == null)
            {
                return Problem("Entity set 'RecipeaseContext.Recipe'  is null.");
            }
            var recipe = await _context.Recipe.FindAsync(id);
            if (recipe != null)
            {
                _context.Recipe.Remove(recipe);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeExists(int id)
        {
          return (_context.Recipe?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
