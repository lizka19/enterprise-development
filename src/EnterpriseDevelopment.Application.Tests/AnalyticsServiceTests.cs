using EnterpriseDevelopment.Application.Services;
using EnterpriseDevelopment.Domain;
using Xunit;

namespace EnterpriseDevelopment.Application.Tests;

/// <summary>
/// Тесты аналитических методов сервиса доставки еды
/// </summary>
public class AnalyticsServiceTests
{
    private readonly DataSeeder _data;
    private readonly AnalyticsService _service;

    /// <summary>
    /// Создаёт тестовое хранилище данных и сервис аналитики
    /// </summary>
    public AnalyticsServiceTests()
    {
        _data = new DataSeeder();
        _service = new AnalyticsService(_data);
    }

    /// <summary>
    /// Проверяет получение пяти ресторанов с сортировкой по количеству заказов
    /// </summary>
    [Fact]
    public void GetTopRestaurants_ReturnsFiveRestaurantsInDescendingOrder()
    {
        var result = _service.GetTopRestaurants().ToList();

        Assert.Equal(5, result.Count);

        Assert.Equal(
            result.OrderByDescending(r => r.OrderCount).Select(r => r.RestaurantId),
            result.Select(r => r.RestaurantId));

        Assert.Equal(1, result[0].RestaurantId);
        Assert.Equal(4, result[0].OrderCount);
    }

    /// <summary>
    /// Проверяет получение всех заказов с минимальным временем доставки
    /// </summary>
    [Fact]
    public void GetFastestOrders_ReturnsAllOrdersWithMinimumDeliveryTime()
    {
        var result = _service.GetFastestOrders().ToList();

        Assert.Equal(2, result.Count);

        Assert.Contains(result, o => o.OrderId == 8);
        Assert.Contains(result, o => o.OrderId == 10);

        Assert.All(result, o =>
            Assert.Equal(TimeSpan.FromMinutes(15), o.DeliveryTime));
    }

    /// <summary>
    /// Проверяет получение уникальных клиентов ресторана
    /// </summary>
    [Fact]
    public void GetRestaurantClients_ReturnsUniqueClients()
    {
        var result = _service.GetRestaurantClients(1).ToList();

        Assert.Equal(3, result.Count);

        Assert.Equal(
            result.Count,
            result.Select(c => c.ClientId).Distinct().Count());
    }

    /// <summary>
    /// Проверяет статистику категорий блюд за выбранный период
    /// </summary>
    [Fact]
    public void GetCategoryStatistics_ReturnsCategoriesForSelectedPeriod()
    {
        var startDate = new DateTimeOffset(
            2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

        var endDate = new DateTimeOffset(
            2026, 12, 31, 23, 59, 59, TimeSpan.Zero);

        var result = _service.GetCategoryStatistics(
            startDate, endDate).ToList();

        Assert.Equal(5, result.Count);

        var pizza = Assert.Single(result, c => c.CategoryId == 1);
        Assert.Equal(10, pizza.OrderCount);
    }

    /// <summary>
    /// Проверяет определение клиента с максимальной суммой заказов
    /// </summary>
    [Fact]
    public void GetTopClients_ReturnsClientWithHighestSpending()
    {
        var result = _service.GetTopClients().ToList();

        var client = Assert.Single(result);

        Assert.Equal(1, client.ClientId);
        Assert.Equal(5390m, client.TotalSpent);
    }

    /// <summary>
    /// Проверяет отсутствие клиентов у несуществующего ресторана
    /// </summary>
    [Fact]
    public void GetRestaurantClients_UnknownRestaurant_ReturnsEmptyCollection()
    {
        var result = _service.GetRestaurantClients(999).ToList();

        Assert.Empty(result);
    }

    /// <summary>
    /// Проверяет отсутствие статистики за период без заказов
    /// </summary>
    [Fact]
    public void GetCategoryStatistics_FuturePeriod_ReturnsEmptyCollection()
    {
        var startDate = new DateTimeOffset(
            2099, 1, 1, 0, 0, 0, TimeSpan.Zero);

        var endDate = new DateTimeOffset(
            2099, 12, 31, 23, 59, 59, TimeSpan.Zero);

        var result = _service.GetCategoryStatistics(
            startDate, endDate).ToList();

        Assert.Empty(result);
    }
}
