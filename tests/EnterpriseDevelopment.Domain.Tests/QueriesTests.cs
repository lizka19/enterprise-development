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

        for (var i = 0; i < expectedCount; i++)
        {
            Assert.Equal(expectedRestaurantIds[i], result[i].RestaurantId);
            Assert.Equal(expectedOrderCounts[i], result[i].OrderCount);
        }
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
        Assert.Equal(expectedOrderIds[0], fastestOrders[0].Id);
        Assert.Equal(expectedOrderIds[1], fastestOrders[1].Id);
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

        for (var i = 0; i < expectedCount; i++)
        {
            Assert.Equal(expectedClientNames[i], result[i].FullName);
        }
    }

    /// <summary>
    /// Проверяет расчёт статистики заказов
    /// </summary>
    [Fact]
    public void GetOrderStatistics()
    {
        var expectedOrderCount = 12;
        var expectedAverageAmount = 1087.5m;
        var expectedTotalAmount = 13050m;

        var result = new
        {
            OrderCount = fixture.Data.Orders.Count,
            AverageAmount = fixture.Data.Orders.Average(order => order.TotalAmount),
            TotalAmount = fixture.Data.Orders.Sum(order => order.TotalAmount)
        };

        Assert.Equal(expectedOrderCount, result.OrderCount);
        Assert.Equal(expectedAverageAmount, result.AverageAmount);
        Assert.Equal(expectedTotalAmount, result.TotalAmount);
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
