using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RecipeBook.Commands;
using RecipeBook.Data;
using RecipeBook.Services;
using RecipeBook.ViewModels;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x => x.SwaggerDoc("v1", new OpenApiInfo { Title = "Recipe app", Version = "v1" }));

string? connString = builder.Configuration.GetConnectionString("MySqlLocal");
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySQL(connString!));

builder.Services.AddScoped<RecipeService>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();
app.UseSwagger();
app.UseSwaggerUI();

var routes = app.MapGroup("")
    .WithParameterValidation()
    .WithOpenApi()
    .WithTags("Recipes");

routes.MapGet("/", async (RecipeService service) =>
{
    return await service.GetRecipes();
})
.WithSummary("List recipes");

routes.MapGet("/{id}", async (int id, RecipeService service) =>
{
    var recipe = await service.GetRecipeDetail(id);
    return recipe is not null
        ? Results.Ok(recipe)
        : Results.Problem(statusCode: 404);
})
.WithName("view-recipe")
.WithSummary("Get recipe")
.ProducesProblem(404)
.Produces<RecipeDetailViewModel>();

routes.MapPost("/", async (CreateRecipeCommand cmd, RecipeService service) =>
{
    var id = await service.CreateRecipe(cmd);
    return Results.CreatedAtRoute("view-recipe", new {id});
})
.WithSummary("Create recipe")
.Produces(StatusCodes.Status201Created);

routes.MapDelete("/{id}", async (int id, RecipeService service) =>
{
    await service.DeleteRecipe(id);
    return Results.NoContent();
})
.WithSummary("Delete recipe")
.Produces(statusCode:201);

routes.MapPut("/{id}", async (int id, UpdateRecipeCommand input, RecipeService service) =>
{
    if (await service.IsAvailableForUpdate(id))
    {
        await service.UpdateRecipe(input);
        return Results.NoContent();
    }

    return Results.Problem(statusCode: 404);
})
.WithSummary("Update recipe")
.ProducesProblem(404)
.Produces(204);

app.Run();