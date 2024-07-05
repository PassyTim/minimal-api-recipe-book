using Chapter12EFCore.ContextModels;
using Chapter12EFCore.Services;

namespace Chapter12EFCore.Commands;

public class CreateRecipeCommand : EditRecipeBase
{
    public IList<CreateIngredientCommand> Ingredients { get; set; } = new List<CreateIngredientCommand>();

    public Recipe ToRecipe()
    {
        return new Recipe
        {
            Name = Name,
            TimeToCook = new TimeSpan(TimeToCookHrs, TimeToCookMins, 0),
            Method = Method,
            IsVegan = IsVegan,
            IsVegetarian = IsVegetarian,
            Ingredients = Ingredients.Select(x => x.ToIngredient()).ToList()
        };
    }
}