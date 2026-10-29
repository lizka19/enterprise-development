
using EnterpriseDevelopment.Application.Interfaces;
using EnterpriseDevelopment.Domain.Entities;

namespace EnterpriseDevelopment.Api.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this WebApplication app)
    {
        app.MapGet("/api/categories", (ICategoryService service) =>
        {
            return Results.Ok(service.GetAll());
        });

        app.MapGet("/api/categories/{id:int}", (int id, ICategoryService service) =>
        {
            var category = service.GetById(id);

            if (category == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(category);
        });

        app.MapPost("/api/categories", (DishCategory category, ICategoryService service) =>
        {
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                return Results.BadRequest("Название категории не может быть пустым");
            }

            var createdCategory = service.Create(category);

            return Results.Created(
                $"/api/categories/{createdCategory.Id}",
                createdCategory);
        });

        app.MapPut("/api/categories/{id:int}", (int id, DishCategory updatedCategory, ICategoryService service) =>
        {
            if (service.GetById(id) == null)
            {
                return Results.NotFound("Категория не найдена");
            }

            if (string.IsNullOrWhiteSpace(updatedCategory.Name))
            {
                return Results.BadRequest("Название категории не может быть пустым");
            }

            service.Update(id, updatedCategory);

            return Results.Ok(service.GetById(id));
        });

        app.MapDelete("/api/categories/{id:int}", (int id, ICategoryService service) =>
        {
            if (service.GetById(id) == null)
            {
                return Results.NotFound("Категория не найдена");
            }

            if (!service.Delete(id))
            {
                return Results.Conflict(
                    "Нельзя удалить категорию, в которой есть блюда");
            }

            return Results.NoContent();
        });
    }
}
