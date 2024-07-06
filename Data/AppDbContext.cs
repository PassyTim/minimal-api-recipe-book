using Microsoft.EntityFrameworkCore;
using RecipeBook.ContextModels;

namespace RecipeBook.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options)
{
    public DbSet<Recipe> Recipes { get; set; }
    // public DbSet<Ingredient> Ingredients { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new RecipeConfiguration());
        // builder.Entity(typeof(Ingredient)).ToTable("ingredients");
        // builder.ApplyConfiguration(new IngredientConfiguration());
    }

}

