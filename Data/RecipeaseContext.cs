using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Recipease.Models;

namespace Recipease.Data
{
    public class RecipeaseContext : DbContext
    {
        public RecipeaseContext (DbContextOptions<RecipeaseContext> options)
            : base(options)
        {
        }

        public DbSet<Recipease.Models.Recipe> Recipe { get; set; } = default!;
    }
}
