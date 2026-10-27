namespace EnterpriseDevelopment.Domain.Tests;

public class QueriesTests(FoodDeliveryFixture fixture)
    : IClassFixture<FoodDeliveryFixture>
{
    /// <summary>
    /// Проверяет получение пяти ресторанов с максимальным количеством заказов
    /// </summary>
    [Fact]
    public void GetTopFiveRestaurantsByOrderCount()
    {
        var expectedCount = 5;

        var expectedRestaurantIds = new[] { 1, 2, 3, 4, 5 };
        var expectedOrderCounts = new[] { 4, 3, 3, 1, 1 };

        var result = fixture.Data.Orders
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

        Assert.Equal(expectedCount, result.Count);

        Assert.Equal(expectedRestaurantIds, result.Select(r => r.RestaurantId));
        Assert.Equal(expectedOrderCounts, result.Select(r => r.OrderCount));
    }

    /// <summary>
    /// Проверяет получение заказов с минимальным временем доставки
    /// </summary>
    [Fact]
public void GetOrdersWithMinimumDeliveryTime()
{
    var expectedMinimumTime = TimeSpan.FromMinutes(15);
    var expectedCount = 2;
    var expectedOrderIds = new[] { 8, 10 };

    var minimumTime = fixture.Data.Orders
        .Min(order => order.DeliveryTime);

    var fastestOrders = fixture.Data.Orders
        .Where(order => order.DeliveryTime == minimumTime)
        .OrderBy(order => order.Id)
        .ToList();

    Assert.Equal(expectedMinimumTime, minimumTime);
    Assert.Equal(expectedCount, fastestOrders.Count);
    Assert.Equal(expectedOrderIds, fastestOrders.Select(order => order.Id));
}

/// <summary>
/// Проверяет получение клиентов выбранного ресторана
/// </summary>
[Fact]
public void GetClientsByRestaurant()
{
    var restaurantId = 1;
    var expectedCount = 3;

    var expectedClientNames = new[]
    {
            "Иванов Иван Иванович",
            "Петров Пётр Сергеевич",
            "Сидорова Анна Андреевна"
        };

    var result = fixture.Data.Orders
        .Where(order => order.RestaurantId == restaurantId)
        .Select(order => order.Client)
        .Distinct()
        .OrderBy(client => client.FullName)
        .ToList();

    Assert.Equal(expectedCount, result.Count);

    Assert.Equal(expectedClientNames, result.Select(r => r.FullName));
}

/// <summary>
/// Проверяет расчёт статистики заказов по категориям блюд за период
/// </summary>
[Fact]
public void GetOrderStatistics()
{
    var startDate = new DateTimeOffset(2026, 9, 1, 0, 0, 0, TimeSpan.Zero);
    var endDate = new DateTimeOffset(2026, 9, 11, 0, 0, 0, TimeSpan.Zero);

    var result = fixture.Data.Orders
        .Where(order => order.CreatedAt >= startDate && order.CreatedAt < endDate)
        .SelectMany(order => order.Dishes.Select(dish => new
        {
            OrderId = order.Id,
            CategoryId = dish.CategoryId,
            CategoryName = dish.Category.Name,
            Price = dish.Price
        }))
        .GroupBy(item => new { item.CategoryId, item.CategoryName })
        .Select(group => new
        {
            CategoryName = group.Key.CategoryName,
            OrderCount = group.Select(item => item.OrderId).Distinct().Count(),
            AverageAmount = group.Sum(item => item.Price) /
                group.Select(item => item.OrderId).Distinct().Count(),
            TotalAmount = group.Sum(item => item.Price)
        })
        .OrderBy(item => item.CategoryName)
        .ToList();

    Assert.Equal(3, result.Count);
    Assert.Equal(new[] { "Бургеры", "Пицца", "Суши" },
        result.Select(item => item.CategoryName));
    Assert.Equal(new[] { 3, 4, 3 },
        result.Select(item => item.OrderCount));
    Assert.Equal(new[] { 780m, 1195m, 4370m / 3m },
        result.Select(item => item.AverageAmount));
    Assert.Equal(new[] { 2340m, 4780m, 4370m },
        result.Select(item => item.TotalAmount));
}

/// <summary>
/// Проверяет поиск клиентов с максимальной общей суммой заказов
/// </summary>
[Fact]
public void GetClientsWithMaximumSpentAmount()
{
    var expectedCount = 1;
    var expectedClientId = 1;
    var expectedTotalSpent = 5390m;

    var clientTotals = fixture.Data.Orders
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

    Assert.Equal(expectedCount, result.Count);
    Assert.Equal(expectedClientId, result[0].ClientId);
    Assert.Equal(expectedTotalSpent, result[0].TotalSpent);
}
}
