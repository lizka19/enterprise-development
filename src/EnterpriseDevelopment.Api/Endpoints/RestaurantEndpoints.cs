using EnterpriseDevelopment.Application.Interfaces;
using EnterpriseDevelopment.Domain;
using EnterpriseDevelopment.Domain.Entities;
namespace EnterpriseDevelopment.Api.Endpoints;

public static class RestaurantEndpoints
{
    public static void MapRestaurantEndpoints(this WebApplication app)
    {

        app.MapGet("/api/restaurants", (IRestaurantService service) =>
        {
            return Results.Ok(service.GetAll());
        });

        app.MapGet("/api/restaurants/{id:int}", (int id, IRestaurantService service) =>
        {
            var restaurant = service.GetById(id);

            if (restaurant == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(restaurant);
        });


        app.MapPost("/api/restaurants", (Restaurant restaurant, IRestaurantService service) =>
        {
            if (string.IsNullOrWhiteSpace(restaurant.Name) ||
                string.IsNullOrWhiteSpace(restaurant.Address) ||
                restaurant.Rating < 0 ||
                restaurant.Rating > 5)
            {
                return Results.BadRequest("Некорректные данные ресторана");
            }

            var createdRestaurant = service.Create(restaurant);

            return Results.Created(
                $"/api/restaurants/{createdRestaurant.Id}",
                createdRestaurant);
        });

        app.MapPut("/api/restaurants/{id:int}", (int id, Restaurant updatedRestaurant, IRestaurantService service) =>
        {
            if (service.GetById(id) == null)
            {
                return Results.NotFound("Ресторан не найден");
            }

            if (string.IsNullOrWhiteSpace(updatedRestaurant.Name) ||
                string.IsNullOrWhiteSpace(updatedRestaurant.Address) ||
                updatedRestaurant.Rating < 0 ||
                updatedRestaurant.Rating > 5)
            {
                return Results.BadRequest("Некорректные данные ресторана");
            }

            service.Update(id, updatedRestaurant);

            return Results.Ok(service.GetById(id));
        });

        app.MapDelete("/api/restaurants/{id:int}", (int id, IRestaurantService service) =>
        {
            var restaurant = service.GetById(id);

            if (restaurant == null)
            {
                return Results.NotFound("Ресторан не найден");
            }

            if (!service.Delete(id))
            {
                return Results.Conflict(
                    "Нельзя удалить ресторан, у которого есть блюда или заказы");
            }

            return Results.NoContent();
        });
    }
}
