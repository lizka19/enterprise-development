using EnterpriseDevelopment.Application.Interfaces;
using EnterpriseDevelopment.Domain;
using EnterpriseDevelopment.Domain.Entities;

namespace EnterpriseDevelopment.Api.Endpoints;

/// <summary>
/// Содержит HTTP-обработчики для управления блюдами ресторанов
/// </summary>
public static class DishEndpoints
{
    /// <summary>
    /// Регистрирует маршруты для получения, создания, изменения и удаления блюд
    /// </summary>
    public static void MapDishEndpoints(this WebApplication app)
    {
        app.MapGet("/api/dishes", (IDishService service) =>
        {
            return Results.Ok(service.GetAll());
        });

        app.MapGet("/api/dishes/{id:int}", (int id, IDishService service) =>
        {
            var dish = service.GetById(id);

            if (dish == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(dish);
        });

        app.MapPost("/api/dishes", (Dish dish, IDishService service, DataSeeder data) =>
        {
            if (string.IsNullOrWhiteSpace(dish.Name) ||
                dish.Price <= 0 ||
                dish.Weight <= 0)
            {
                return Results.BadRequest("Некорректные данные блюда");
            }

            if (!data.Categories.Any(c => c.Id == dish.CategoryId))
            {
                return Results.BadRequest("Категория не найдена");
            }

            if (!data.Restaurants.Any(r => r.Id == dish.RestaurantId))
            {
                return Results.BadRequest("Ресторан не найден");
            }

            var createdDish = service.Create(dish);

            return Results.Created($"/api/dishes/{createdDish.Id}", createdDish);
        });

        app.MapPut("/api/dishes/{id:int}", (int id, Dish updatedDish, IDishService service, DataSeeder data) =>
        {
            if (service.GetById(id) == null)
            {
                return Results.NotFound("Блюдо не найдено");
            }

            if (string.IsNullOrWhiteSpace(updatedDish.Name) ||
                updatedDish.Price <= 0 ||
                updatedDish.Weight <= 0)
            {
                return Results.BadRequest("Некорректные данные блюда");
            }

            if (!data.Categories.Any(c => c.Id == updatedDish.CategoryId))
            {
                return Results.BadRequest("Категория не найдена");
            }

            if (!data.Restaurants.Any(r => r.Id == updatedDish.RestaurantId))
            {
                return Results.BadRequest("Ресторан не найден");
            }

            service.Update(id, updatedDish);

            return Results.Ok(service.GetById(id));
        });

        app.MapDelete("/api/dishes/{id:int}", (int id, IDishService service) =>
        {
            if (service.GetById(id) == null)
            {
                return Results.NotFound("Блюдо не найдено");
            }

            if (!service.Delete(id))
            {
                return Results.Conflict(
                    "Нельзя удалить блюдо, которое используется в заказах");
            }

            return Results.NoContent();
        });
    }
}
