using EnterpriseDevelopment.Domain;
using EnterpriseDevelopment.Domain.Entities;
using EnterpriseDevelopment.Api;

namespace EnterpriseDevelopment.Api.Endpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this WebApplication app)
    {
        app.MapGet("/api/orders", (DataSeeder data) =>
        {
            return Results.Ok(data.Orders);
        });

        app.MapGet("/api/orders/{id:int}", (int id, DataSeeder data) =>
        {
            var order = data.Orders.FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(order);
        });


        app.MapPost("/api/orders", (CreateOrderRequest request, DataSeeder data) =>
        {
            var client = data.Clients.FirstOrDefault(c => c.Id == request.ClientId);

            if (client == null)
            {
                return Results.BadRequest("Клиент не найден");
            }

            var restaurant = data.Restaurants.FirstOrDefault(r => r.Id == request.RestaurantId);

            if (restaurant == null)
            {
                return Results.BadRequest("Ресторан не найден");
            }

            if (request.DishIds == null || request.DishIds.Count == 0)
            {
                return Results.BadRequest("В заказе должны быть блюда");
            }

            var dishes = new List<Dish>();

            foreach (var dishId in request.DishIds)
            {
                var dish = data.Dishes.FirstOrDefault(d => d.Id == dishId);

                if (dish == null)
                {
                    return Results.BadRequest($"Блюдо с ID {dishId} не найдено");
                }

                if (dish.RestaurantId != restaurant.Id)
                {
                    return Results.BadRequest("Все блюда должны принадлежать выбранному ресторану");
                }

                dishes.Add(dish);
            }

            if (request.DeliveredAt < request.CreatedAt)
            {
                return Results.BadRequest("Время доставки не может быть раньше времени создания заказа");
            }

            var order = new Order
            {
                Id = data.Orders.Select(o => o.Id).DefaultIfEmpty(0).Max() + 1,
                ClientId = client.Id,
                Client = client,
                RestaurantId = restaurant.Id,
                Restaurant = restaurant,
                Dishes = dishes,
                CreatedAt = request.CreatedAt,
                DeliveredAt = request.DeliveredAt
            };

            data.Orders.Add(order);

            return Results.Created($"/api/orders/{order.Id}", order);
        });

        app.MapPut("/api/orders/{id:int}", (int id, CreateOrderRequest request, DataSeeder data) =>
        {
            var order = data.Orders.FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return Results.NotFound("Заказ не найден");
            }

            var client = data.Clients.FirstOrDefault(c => c.Id == request.ClientId);

            if (client == null)
            {
                return Results.BadRequest("Клиент не найден");
            }

            var restaurant = data.Restaurants.FirstOrDefault(r => r.Id == request.RestaurantId);

            if (restaurant == null)
            {
                return Results.BadRequest("Ресторан не найден");
            }

            if (request.DishIds == null || request.DishIds.Count == 0)
            {
                return Results.BadRequest("В заказе должны быть блюда");
            }

            var dishes = new List<Dish>();

            foreach (var dishId in request.DishIds)
            {
                var dish = data.Dishes.FirstOrDefault(d => d.Id == dishId);

                if (dish == null)
                {
                    return Results.BadRequest($"Блюдо с ID {dishId} не найдено");
                }

                if (dish.RestaurantId != restaurant.Id)
                {
                    return Results.BadRequest("Все блюда должны принадлежать выбранному ресторану");
                }

                dishes.Add(dish);
            }

            if (request.DeliveredAt < request.CreatedAt)
            {
                return Results.BadRequest("Время доставки не может быть раньше времени создания заказа");
            }

            order.ClientId = client.Id;
            order.Client = client;
            order.RestaurantId = restaurant.Id;
            order.Restaurant = restaurant;
            order.Dishes = dishes;
            order.CreatedAt = request.CreatedAt;
            order.DeliveredAt = request.DeliveredAt;

            return Results.Ok(order);
        });

        app.MapDelete("/api/orders/{id:int}", (int id, DataSeeder data) =>
        {
            var order = data.Orders.FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return Results.NotFound("Заказ не найден");
            }

            data.Orders.Remove(order);

            return Results.NoContent();
        });
    }
}
