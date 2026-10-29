using EnterpriseDevelopment.Application.DTOs;
using EnterpriseDevelopment.Application.Interfaces;
using EnterpriseDevelopment.Domain;

namespace EnterpriseDevelopment.Application.Services;

/// <summary>
/// Сервис для получения аналитических данных
/// о ресторанах, заказах, клиентах и категориях блюд
/// </summary>
public class AnalyticsService : IAnalyticsService
{
    private readonly DataSeeder _data;

    /// <summary>
    /// Создаёт сервис аналитики с указанным хранилищем данных
    /// </summary>
    public AnalyticsService(DataSeeder data)
    {
        _data = data;
    }

    /// <summary>
    /// Получает пять ресторанов с наибольшим количеством заказов
    /// </summary>
    /// <returns>Коллекция ресторанов с количеством заказов</returns>
    public IEnumerable<TopRestaurantDto> GetTopRestaurants()
    {
        return _data.Orders
            .GroupBy(o => o.RestaurantId)
            .Select(g => new TopRestaurantDto(
                g.Key,
                _data.Restaurants.First(r => r.Id == g.Key).Name,
                g.Count()))
            .OrderByDescending(x => x.OrderCount)
            .Take(5)
            .ToList();
    }

    /// <summary>
    /// Получает все заказы с минимальным временем доставки
    /// </summary>
    /// <returns>Коллекция заказов с минимальным временем доставки</returns>
    public IEnumerable<FastestOrderDto> GetFastestOrders()
    {
        var orders = _data.Orders.ToList();

        if (orders.Count == 0)
        {
            return [];
        }

        var minTime = orders.Min(o => o.DeliveryTime);

        return orders
            .Where(o => o.DeliveryTime == minTime)
            .Select(o => new FastestOrderDto(
                o.Id,
                o.ClientId,
                o.RestaurantId,
                o.DeliveryTime))
            .ToList();
    }

    /// <summary>
    /// Получает уникальных клиентов указанного ресторана
    /// </summary>
    /// <returns>Коллекция клиентов ресторана</returns>
    public IEnumerable<RestaurantClientDto> GetRestaurantClients(
        int restaurantId)
    {
        return _data.Orders
            .Where(o => o.RestaurantId == restaurantId)
            .Select(o => o.ClientId)
            .Distinct()
            .Join(
                _data.Clients,
                id => id,
                client => client.Id,
                (id, client) => new RestaurantClientDto(
                    client.Id,
                    client.FullName,
                    client.PhoneNumber))
            .ToList();
    }

    /// <summary>
    /// Получает статистику заказанных блюд по категориям за указанный период времени
    /// </summary>
    /// <returns>Коллекция категорий с количеством заказанных блюд</returns>
    public IEnumerable<CategoryStatisticsDto> GetCategoryStatistics(
        DateTimeOffset startDate,
        DateTimeOffset endDate)
    {
        return _data.Orders
            .Where(o => o.CreatedAt >= startDate &&
                        o.CreatedAt <= endDate)
            .SelectMany(o => o.Dishes)
            .GroupBy(d => d.CategoryId)
            .Select(g => new CategoryStatisticsDto(
                g.Key,
                _data.Categories.First(c => c.Id == g.Key).Name,
                g.Count()))
            .OrderByDescending(x => x.OrderCount)
            .ToList();
    }

    /// <summary>
    /// Получает клиента с наибольшей общей суммой заказов
    /// </summary>
    /// <returns>Коллекция клиентов с максимальной суммой заказов</returns>
    public IEnumerable<TopClientDto> GetTopClients()
    {
        return _data.Orders
            .GroupBy(o => o.ClientId)
            .Select(g => new TopClientDto(
                g.Key,
                _data.Clients.First(c => c.Id == g.Key).FullName,
                g.Sum(o => o.TotalAmount)))
            .OrderByDescending(x => x.TotalSpent)
            .Take(1)
            .ToList();
    }
}
