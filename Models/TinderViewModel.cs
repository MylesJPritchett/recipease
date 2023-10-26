using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Recipease.Models
{
    public class TinderViewModel
    {

        public List<Recipe>? Recipes { get; set; }
        public SelectList? Cuisines { get; set; }
        public string? RecipeCuisine { get; set; }
        public string? SearchString { get; set; }
    }
}

