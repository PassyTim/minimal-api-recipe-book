using Chapter12EFCore.Commands;
using Chapter12EFCore.ContextModels;
using Chapter12EFCore.Data;
using Chapter12EFCore.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Chapter12EFCore.Services;

public class RecipeService
{
    private readonly AppDbContext _dbContext;

    public RecipeService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsAvailableForUpdate(int recipeId)
    {
        return await _dbContext.Recipes
            .Where(x => x.RecipeId == recipeId)
            .Where(x => !x.IsDeleted)
            .AnyAsync();
    }

    public async Task<int> CreateRecipe(CreateRecipeCommand cmd)
    {
        var recipe = cmd.ToRecipe();
        _dbContext.AddAsync(recipe);
        await _dbContext.SaveChangesAsync();
        return recipe.RecipeId;
    }
    public async Task<List<RecipeSummaryViewModel>> GetRecipes()
    {
        return await _dbContext.Recipes
            .Where(r => !r.IsDeleted)
            .Select(x => new RecipeSummaryViewModel
            {
                Id = x.RecipeId,
                Name = x.Name,
                TimeToCook = $"{x.TimeToCook.Hours} hours {x.TimeToCook.Minutes} mins",
                NumberOfIngredients = x.Ingredients.Count()
            })
            .ToListAsync();
    }

    public async Task<RecipeDetailViewModel?> GetRecipeDetail(int id)
    {
        return await _dbContext.Recipes
            .Where(x => x.RecipeId == id)
            .Where(x => !x.IsDeleted)
            .Select(x => new RecipeDetailViewModel
            {
                Id = x.RecipeId,
                Name = x.Name,
                Method = x.Method,
                Ingredients = x.Ingredients
                    .Select(item => new RecipeDetailViewModel.Item
                    {
                        Name = item.Name,
                        Quantity = $"{item.Quantity} {item.Unit}"
                    })
            })
            .SingleOrDefaultAsync();
    }

    public async Task DeleteRecipe(int id)
    {
        var recipe = await _dbContext.Recipes.FindAsync(id);
        if (recipe is null)
        {
            throw new Exception("Unable to find the recipe");
        }

        recipe.IsDeleted = true;
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateRecipe(UpdateRecipeCommand cmd)
    {
        var recipe = await _dbContext.Recipes.FindAsync(cmd.Id);
        if (recipe is null) throw new Exception("Unable to find the recipe");
        if (recipe.IsDeleted) throw new Exception("Unable to update deleted recipe");
        
        cmd.UpdateRecipe(recipe);
        await _dbContext.SaveChangesAsync();
    }
}