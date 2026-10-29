using EnterpriseDevelopment.Domain;
using EnterpriseDevelopment.Domain.Entities;

namespace EnterpriseDevelopment.Api.Endpoints;

public static class DishEndpoints
{
    public static void MapDishEndpoints(this WebApplication app)
    {
        app.MapGet("/api/dishes", (DataSeeder data) =>
        {
            return Results.Ok(data.Dishes);
        });

        app.MapGet("/api/dishes/{id:int}", (int id, DataSeeder data) =>
        {
            var dish = data.Dishes.FirstOrDefault(d => d.Id == id);

            if (dish == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(dish);
        });


        app.MapPost("/api/dishes", (Dish dish, DataSeeder data) =>
        {
            if (string.IsNullOrWhiteSpace(dish.Name) ||
                dish.Price <= 0 ||
                dish.Weight <= 0)
            {
                return Results.BadRequest("Некорректные данные блюда");
            }

            var category = data.Categories.FirstOrDefault(
                c => c.Id == dish.CategoryId);

            if (category == null)
            {
                return Results.BadRequest("Категория не найдена");
            }

            var restaurant = data.Restaurants.FirstOrDefault(
                r => r.Id == dish.RestaurantId);

            if (restaurant == null)
            {
                return Results.BadRequest("Ресторан не найден");
            }

            dish.Id = data.Dishes
                .Select(d => d.Id)
                .DefaultIfEmpty(0)
                .Max() + 1;

            dish.Category = category;
            dish.Restaurant = restaurant;

            data.Dishes.Add(dish);

            return Results.Created($"/api/dishes/{dish.Id}", dish);
        });

        app.MapPut("/api/dishes/{id:int}", (int id, Dish updatedDish, DataSeeder data) =>
        {
            var dish = data.Dishes.FirstOrDefault(d => d.Id == id);

            if (dish == null)
            {
                return Results.NotFound("Блюдо не найдено");
            }

            if (string.IsNullOrWhiteSpace(updatedDish.Name) ||
                updatedDish.Price <= 0 ||
                updatedDish.Weight <= 0)
            {
                return Results.BadRequest("Некорректные данные блюда");
            }

            var category = data.Categories.FirstOrDefault(
                c => c.Id == updatedDish.CategoryId);

            if (category == null)
            {
                return Results.BadRequest("Категория не найдена");
            }

            var restaurant = data.Restaurants.FirstOrDefault(
                r => r.Id == updatedDish.RestaurantId);

            if (restaurant == null)
            {
                return Results.BadRequest("Ресторан не найден");
            }

            dish.Name = updatedDish.Name;
            dish.Weight = updatedDish.Weight;
            dish.Price = updatedDish.Price;
            dish.CategoryId = category.Id;
            dish.Category = category;
            dish.RestaurantId = restaurant.Id;
            dish.Restaurant = restaurant;

            return Results.Ok(dish);
        });

        app.MapDelete("/api/dishes/{id:int}", (int id, DataSeeder data) =>
        {
            var dish = data.Dishes.FirstOrDefault(d => d.Id == id);

            if (dish == null)
            {
                return Results.NotFound("Блюдо не найдено");
            }

            if (data.Orders.Any(o => o.Dishes.Any(d => d.Id == id)))
            {
                return Results.Conflict(
                    "Нельзя удалить блюдо, которое используется в заказах");
            }

            data.Dishes.Remove(dish);

            return Results.NoContent();
        });
    }
}
