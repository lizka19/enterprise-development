using EnterpriseDevelopment.Application.Interfaces;

namespace EnterpriseDevelopment.Api.Endpoints;

/// <summary>
/// Содержит HTTP-обработчики для получения аналитической информации о работе сервиса доставки еды
/// </summary>
public static class AnalyticsEndpoints
{
    /// <summary>
    /// Регистрирует аналитические маршруты для получения статистики ресторанов, заказов, клиентов и категорий блюд
    /// </summary>
    public static void MapAnalyticsEndpoints(this WebApplication app)
    {
        app.MapGet("/api/analytics/top-restaurants",
            (IAnalyticsService service) =>
            {
                return Results.Ok(service.GetTopRestaurants());
            });

        app.MapGet("/api/analytics/fastest-orders",
            (IAnalyticsService service) =>
            {
                return Results.Ok(service.GetFastestOrders());
            });

        app.MapGet("/api/analytics/restaurant-clients/{restaurantId:int}",
            (int restaurantId, IAnalyticsService service) =>
            {
                return Results.Ok(
                    service.GetRestaurantClients(restaurantId));
            });

        app.MapGet("/api/analytics/category-statistics",
            (
                DateTimeOffset startDate,
                DateTimeOffset endDate,
                IAnalyticsService service
            ) =>
            {
                if (startDate > endDate)
                {
                    return Results.BadRequest(
                        "Начальная дата не может быть позже конечной");
                }

                return Results.Ok(
                    service.GetCategoryStatistics(startDate, endDate));
            });

        app.MapGet("/api/analytics/top-clients",
            (IAnalyticsService service) =>
            {
                return Results.Ok(service.GetTopClients());
            });
    }
}
