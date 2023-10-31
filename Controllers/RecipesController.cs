using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public async Task<IActionResult> Create([Bind("Id,Title,Image,DateAdded,Cuisine,Price,Rating,Summary")] Recipe recipe)
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


            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.spoonacular.com/recipes/random?number=1&apiKey=fd93271a34a24f448d0d2c0973595b0b");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var response_string = (await response.Content.ReadAsStringAsync());

            JObject jo = JObject.Parse(response_string);
            Recipe_New[] recipeArray = jo.SelectToken("recipes", false).ToObject<Recipe_New[]>();

            //List <Recipe_New> result_list = JsonConvert.DeserializeObject<List<Recipe_New>>(response_string);
            Recipe_New result = recipeArray[0];

            Recipe r = new Recipe();

            r.Title = result.Title;
                r.Image = result.Image;
                r.Rating = 0;
                r.Summary = result.Summary;
                r.Id = result.Id;
                r.Price = (decimal)result.PricePerServing;
                r.DateAdded = DateTime.Now;
     

            _context.Add(r);
            await _context.SaveChangesAsync();


            var rand = new Random();
            var total = _context.Recipe.Count();
            var rand_id = new int();

            do
            {
                rand_id = _context.Recipe.Skip(rand.Next(total)).First().Id;
            }
            while (rand_id == id);

            int new_id = result.Id;

            if (new_id == null || new_id == 0)
            {
                new_id = rand_id;
            }


            var recipe = await _context.Recipe
                .FirstOrDefaultAsync(r => r.Id == new_id);

            
            if (recipe == null)
            {
                return NotFound();
            }


            if (_context.Recipe == null)
            {
                return Problem("Entity set 'Recipease.Recipe'  is null.");
            }

      

            
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
