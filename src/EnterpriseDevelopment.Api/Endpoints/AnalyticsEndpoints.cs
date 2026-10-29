using EnterpriseDevelopment.Domain;
using EnterpriseDevelopment.Domain.Entities;

namespace EnterpriseDevelopment.Api.Endpoints;

public static class AnalyticsEndpoints
{
    public static void MapAnalyticsEndpoints(this WebApplication app)
    {
        app.MapGet("/api/analytics/top-restaurants", (DataSeeder data) =>
        {
            var result = data.Orders
                .GroupBy(order => order.RestaurantId)
                .Select(group => new
                {
                    RestaurantId = group.Key,
                    OrderCount = group.Count()
                })
                .OrderByDescending(item => item.OrderCount)
                .ThenBy(item => item.RestaurantId)
                .Take(5)
                .ToList();

            return Results.Ok(result);
        });

        app.MapGet("/api/analytics/fastest-orders", (DataSeeder data) =>
        {
            if (!data.Orders.Any())
            {
                return Results.Ok(new List<object>());
            }

            var minDeliveryTime = data.Orders.Min(o => o.DeliveryTime);

            var result = data.Orders
                .Where(o => o.DeliveryTime == minDeliveryTime)
                .Select(o => new
                {
                    OrderId = o.Id,
                    ClientId = o.ClientId,
                    RestaurantId = o.RestaurantId,
                    DeliveryTime = o.DeliveryTime
                })
                .ToList();

            return Results.Ok(result);
        });

        app.MapGet("/api/analytics/restaurant-clients/{restaurantId:int}",
            (int restaurantId, DataSeeder data) =>
            {
                var restaurant = data.Restaurants.FirstOrDefault(
                    r => r.Id == restaurantId);

                if (restaurant == null)
                {
                    return Results.NotFound("Ресторан не найден");
                }

                var clients = data.Orders
                    .Where(o => o.RestaurantId == restaurantId)
                    .Select(o => o.Client)
                    .DistinctBy(c => c.Id)
                    .OrderBy(c => c.FullName)
                    .ToList();

                return Results.Ok(clients);
            });

        app.MapGet("/api/analytics/category-statistics",
            (DateTimeOffset startDate, DateTimeOffset endDate, DataSeeder data) =>
            {
                if (startDate > endDate)
                {
                    return Results.BadRequest("Начальная дата не может быть позже конечной");
                }

                var result = data.Orders
                    .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate)
                    .SelectMany(o => o.Dishes)
                    .GroupBy(d => d.CategoryId)
                    .Select(g => new
                    {
                        CategoryId = g.Key,
                        CategoryName = data.Categories
                            .FirstOrDefault(c => c.Id == g.Key)?.Name,
                        DishCount = g.Count(),
                        TotalAmount = g.Sum(d => d.Price)
                    })
                    .OrderByDescending(x => x.DishCount)
                    .ToList();

                return Results.Ok(result);
            });

        app.MapGet("/api/analytics/top-clients", (DataSeeder data) =>
        {
            if (!data.Orders.Any())
            {
                return Results.Ok(new List<object>());
            }

            var clientTotals = data.Orders
                .GroupBy(o => o.ClientId)
                .Select(g => new
                {
                    ClientId = g.Key,
                    TotalAmount = g.Sum(o => o.TotalAmount)
                })
                .ToList();

            var maxAmount = clientTotals.Max(c => c.TotalAmount);

            var result = clientTotals
                .Where(c => c.TotalAmount == maxAmount)
                .Join(
                    data.Clients,
                    total => total.ClientId,
                    client => client.Id,
                    (total, client) => new
                    {
                        ClientId = client.Id,
                        FullName = client.FullName,
                        TotalAmount = total.TotalAmount
                    })
                .OrderBy(c => c.ClientId)
                .ToList();

            return Results.Ok(result);
        });

    }
}
