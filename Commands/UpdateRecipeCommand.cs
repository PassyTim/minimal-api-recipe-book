using Chapter12EFCore.ContextModels;

namespace Chapter12EFCore.Commands;

public class UpdateRecipeCommand : EditRecipeBase
{
    public int Id { get; set; }

    public void UpdateRecipe(Recipe recipe)
    {
        recipe.Name = Name;
        recipe.TimeToCook = new TimeSpan(TimeToCookHrs, TimeToCookMins, 0);
        recipe.IsVegan = IsVegan;
        recipe.IsVegetarian = IsVegetarian;
        recipe.Method = Method;
    }
}