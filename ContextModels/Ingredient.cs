using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chapter12EFCore.ContextModels;

public class Ingredient
{
    public int IngredientId { get; set; }
    public int RecipeId { get; set; }
    [Required]
    public string Name { get; set; }
    public decimal Quantity { get; set; }
    [Required]
    public string Unit { get; set; }
}

public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
{
    public void Configure(EntityTypeBuilder<Ingredient> builder)
    {
        builder.Property(p => p.IngredientId).HasColumnName("ingredient_id");
        builder.Property(p => p.RecipeId).HasColumnName("recipe_id");
        builder.Property(p => p.Quantity).HasColumnName("quantity");
        builder.Property(p => p.Name).HasColumnName("name").IsRequired();
        builder.Property(p => p.Unit).HasColumnName("unit").IsRequired();
    }
}
