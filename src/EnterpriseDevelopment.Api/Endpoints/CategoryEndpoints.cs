using EnterpriseDevelopment.Domain;
using EnterpriseDevelopment.Domain.Entities;

namespace EnterpriseDevelopment.Api.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this WebApplication app)
    {
        app.MapGet("/api/categories", (DataSeeder data) =>
        {
            return Results.Ok(data.Categories);
        }); app.MapGet("/api/categories/{id:int}", (int id, DataSeeder data) =>
        {
            var category = data.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(category);
        });

        app.MapPost("/api/categories", (DishCategory category, DataSeeder data) =>
        {
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                return Results.BadRequest("Название категории не может быть пустым");
            }

            category.Id = data.Categories
                .Select(c => c.Id)
                .DefaultIfEmpty(0)
                .Max() + 1;

            data.Categories.Add(category);

            return Results.Created($"/api/categories/{category.Id}", category);
        });

        app.MapPut("/api/categories/{id:int}", (int id, DishCategory updatedCategory, DataSeeder data) =>
        {
            var category = data.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return Results.NotFound("Категория не найдена");
            }

            if (string.IsNullOrWhiteSpace(updatedCategory.Name))
            {
                return Results.BadRequest("Название категории не может быть пустым");
            }

            category.Name = updatedCategory.Name;

            return Results.Ok(category);
        });

        app.MapDelete("/api/categories/{id:int}", (int id, DataSeeder data) =>
        {
            var category = data.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return Results.NotFound("Категория не найдена");
            }

            if (data.Dishes.Any(d => d.CategoryId == id))
            {
                return Results.Conflict(
                    "Нельзя удалить категорию, в которой есть блюда");
            }

            data.Categories.Remove(category);

            return Results.NoContent();
        });
    }
}
