using Xunit;

namespace EnterpriseDevelopment.Domain.Tests;

public class QueriesTests : IClassFixture<FoodDeliveryFixture>
{
    private readonly FoodDeliveryFixture _fixture;

    public QueriesTests(FoodDeliveryFixture fixture)
    {
        _fixture = fixture;
    }

    /// <summary>
    /// Проверяет получение пяти ресторанов с максимальным количеством заказов
    /// </summary>
    [Fact]
    public void GetTopFiveRestaurantsByOrderCount()
    {
        var result = _fixture.Data.Orders
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

        Assert.Equal(5, result.Count);

        Assert.Equal(1, result[0].RestaurantId);
        Assert.Equal(4, result[0].OrderCount);

        Assert.Equal(2, result[1].RestaurantId);
        Assert.Equal(3, result[1].OrderCount);

        Assert.Equal(3, result[2].RestaurantId);
        Assert.Equal(3, result[2].OrderCount);

        Assert.Equal(4, result[3].RestaurantId);
        Assert.Equal(1, result[3].OrderCount);

        Assert.Equal(5, result[4].RestaurantId);
        Assert.Equal(1, result[4].OrderCount);
    }

    /// <summary>
    /// Проверяет получение заказов с минимальным временем доставки
    /// </summary>
    [Fact]
    public void GetOrdersWithMinimumDeliveryTime()
    {
        var minimumTime = _fixture.Data.Orders
            .Min(order => order.DeliveryTime);

        var fastestOrders = _fixture.Data.Orders
            .Where(order => order.DeliveryTime == minimumTime)
            .OrderBy(order => order.Id)
            .ToList();

        Assert.Equal(TimeSpan.FromMinutes(15), minimumTime);
        Assert.Equal(2, fastestOrders.Count);
        Assert.Equal(8, fastestOrders[0].Id);
        Assert.Equal(10, fastestOrders[1].Id);
    }

    /// <summary>
    /// Проверяет получение клиентов выбранного ресторана
    /// </summary>
    [Fact]
    public void GetClientsByRestaurant()
    {
        var restaurantId = 1;

        var result = _fixture.Data.Orders
            .Where(order => order.RestaurantId == restaurantId)
            .Select(order => order.Client)
            .Distinct()
            .OrderBy(client => client.FullName)
            .ToList();

        Assert.Equal(3, result.Count);
        Assert.Equal("Иванов Иван Иванович", result[0].FullName);
        Assert.Equal("Петров Пётр Сергеевич", result[1].FullName);
        Assert.Equal("Сидорова Анна Андреевна", result[2].FullName);
    }

    /// <summary>
    /// Проверяет расчёт статистики заказов
    /// </summary>
    [Fact]
    public void GetOrderStatistics()
    {
        var result = new
        {
            OrderCount = _fixture.Data.Orders.Count,
            AverageAmount = _fixture.Data.Orders.Average(order => order.TotalAmount),
            TotalAmount = _fixture.Data.Orders.Sum(order => order.TotalAmount)
        };

        Assert.Equal(12, result.OrderCount);
        Assert.Equal(1087.5m, result.AverageAmount);
        Assert.Equal(13050m, result.TotalAmount);
    }

    /// <summary>
    /// Проверяет поиск клиентов с максимальной общей суммой заказов
    /// </summary>
    [Fact]
    public void GetClientsWithMaximumSpentAmount()
    {
        var clientTotals = _fixture.Data.Orders
            .GroupBy(order => order.ClientId)
            .Select(group => new
            {
                ClientId = group.Key,
                TotalSpent = group.Sum(order => order.TotalAmount)
            })
            .ToList();

        var maximumSpentAmount = clientTotals
            .Max(item => item.TotalSpent);

        var result = clientTotals
            .Where(item => item.TotalSpent == maximumSpentAmount)
            .OrderBy(item => item.ClientId)
            .ToList();

        Assert.Single(result);
        Assert.Equal(1, result[0].ClientId);
        Assert.Equal(5390m, result[0].TotalSpent);
    }
}