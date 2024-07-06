using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RecipeBook.ContextModels;

public class Recipe
{
    public int RecipeId { get; set; }
    public string Name { get; set; }
    public TimeSpan TimeToCook { get; set; }
    public bool IsDeleted { get; set; }
    public string Method { get; set; }
    public bool IsVegan { get; set; }
    public bool IsVegetarian { get; set; }
    public required List<Ingredient> Ingredients { get; set; }
}

public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.Property(p => p.Name).IsRequired();
            // .HasColumnName("name");
        builder.Property(p => p.Method).IsRequired();
        //     .HasColumnName("method");
        // builder.Property(p => p.TimeToCook)
        //     .HasColumnName("time_to_cook");
        // builder.Property(p => p.IsDeleted)
        //     .HasColumnName("is_deleted");
        // builder.Property(p => p.RecipeId)
        //     .HasColumnName("recipe_id");
        // builder.Property(p => p.IsVegan)
        //     .HasColumnName("is_vegan");
        // builder.Property(p => p.IsVegetarian)
        //     .HasColumnName("is_vegetarian");
    }
}