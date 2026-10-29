using EnterpriseDevelopment.Api;
using EnterpriseDevelopment.Application.Interfaces;
using EnterpriseDevelopment.Domain;
using EnterpriseDevelopment.Domain.Entities;

namespace EnterpriseDevelopment.Api.Endpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this WebApplication app)
    {
        app.MapGet("/api/orders", (IOrderService service) =>
        {
            return Results.Ok(service.GetAll());
        });

        app.MapGet("/api/orders/{id:int}", (int id, IOrderService service) =>
        {
            var order = service.GetById(id);

            if (order == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(order);
        });

        app.MapPost("/api/orders",
            (CreateOrderRequest request, IOrderService service, DataSeeder data) =>
            {
                var error = ValidateOrder(request, data);

                if (error != null)
                {
                    return Results.BadRequest(error);
                }

                var order = service.Create(
                    request.ClientId,
                    request.RestaurantId,
                    request.DishIds,
                    request.CreatedAt,
                    request.DeliveredAt);

                return Results.Created($"/api/orders/{order.Id}", order);
            });

        app.MapPut("/api/orders/{id:int}",
            (int id, CreateOrderRequest request, IOrderService service, DataSeeder data) =>
            {
                if (service.GetById(id) == null)
                {
                    return Results.NotFound("Заказ не найден");
                }

                var error = ValidateOrder(request, data);

                if (error != null)
                {
                    return Results.BadRequest(error);
                }

                service.Update(
                    id,
                    request.ClientId,
                    request.RestaurantId,
                    request.DishIds,
                    request.CreatedAt,
                    request.DeliveredAt);

                return Results.Ok(service.GetById(id));
            });

        app.MapDelete("/api/orders/{id:int}", (int id, IOrderService service) =>
        {
            if (!service.Delete(id))
            {
                return Results.NotFound("Заказ не найден");
            }

            return Results.NoContent();
        });
    }

    private static string? ValidateOrder(CreateOrderRequest request, DataSeeder data)
    {
        if (!data.Clients.Any(c => c.Id == request.ClientId))
        {
            return "Клиент не найден";
        }

        if (!data.Restaurants.Any(r => r.Id == request.RestaurantId))
        {
            return "Ресторан не найден";
        }

        if (request.DishIds == null || request.DishIds.Count == 0)
        {
            return "В заказе должны быть блюда";
        }

        foreach (var dishId in request.DishIds)
        {
            var dish = data.Dishes.FirstOrDefault(d => d.Id == dishId);

            if (dish == null)
            {
                return $"Блюдо с ID {dishId} не найдено";
            }

            if (dish.RestaurantId != request.RestaurantId)
            {
                return "Все блюда должны принадлежать выбранному ресторану";
            }
        }

        if (request.DeliveredAt < request.CreatedAt)
        {
            return "Время доставки не может быть раньше времени создания заказа";
        }

        return null;
    }
}
