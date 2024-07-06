using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RecipeBook.Commands;
using RecipeBook.Data;
using RecipeBook.Services;
using RecipeBook.ViewModels;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x => x.SwaggerDoc("v1", new OpenApiInfo { Title = "Recipe app", Version = "v1" }));

var connString = builder.Configuration.GetConnectionString("MySqlLocal");
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySQL(connString!));

builder.Services.AddScoped<RecipeService>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var routes = app.MapGroup("")
    .WithParameterValidation()
    .WithOpenApi()
    .WithTags("Recipes");

routes.MapGet("/", async (RecipeService service) => await service.GetRecipes())
.WithSummary("List recipes");

routes.MapGet("/{id:int}", async (int id, RecipeService service) =>
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

routes.MapDelete("/{id:int}", async (int id, RecipeService service) =>
{
    await service.DeleteRecipe(id);
    return Results.NoContent();
})
.WithSummary("Delete recipe")
.Produces(statusCode:201);

routes.MapPut("/{id:int}", async (int id, UpdateRecipeCommand input, RecipeService service) =>
{
    if (!await service.IsAvailableForUpdate(id)) return Results.Problem(statusCode: 404);
    await service.UpdateRecipe(input);
    return Results.NoContent();

})
.WithSummary("Update recipe")
.ProducesProblem(404)
.Produces(204);

app.Run();