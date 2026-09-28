using Xunit;


namespace EnterpriseDevelopment.Domain.Tests;


public class QueriesTests : IClassFixture<FoodDeliveryFixture>
{
    private readonly FoodDeliveryFixture _fixture;


    public QueriesTests(FoodDeliveryFixture fixture)
    {
        _fixture = fixture;
    }


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
            .Take(5)
            .ToList();


        Assert.Equal(5, result.Count);

        Assert.Equal(1, result[0].RestaurantId);

        Assert.Equal(4, result[0].OrderCount);
    }


    [Fact]
    public void GetOrdersWithMinimumDeliveryTime()
    {
        var result = _fixture.Data.Orders
            .Select(order => new
            {
                Order = order,
                DeliveryTime = order.DeliveredAt - order.CreatedAt
            })
            .OrderBy(item => item.DeliveryTime)
            .ToList();


        var minimumTime = result.First().DeliveryTime;


        var fastestOrders = result
            .Where(item => item.DeliveryTime == minimumTime)
            .ToList();


        Assert.NotEmpty(fastestOrders);

        Assert.Equal(
            TimeSpan.FromMinutes(15),
            minimumTime
        );
    }


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


        Assert.NotEmpty(result);

        Assert.Equal(
            result.OrderBy(client => client.FullName),
            result
        );
    }


    [Fact]
    public void GetOrderStatisticsByRestaurant()
    {
        var result = _fixture.Data.Orders
            .GroupBy(order => order.RestaurantId)
            .Select(group => new
            {
                RestaurantId = group.Key,
                OrderCount = group.Count(),
                AverageAmount = group.Average(order => order.TotalAmount),
                TotalAmount = group.Sum(order => order.TotalAmount)
            })
            .ToList();


        Assert.NotEmpty(result);


        Assert.All(result, item =>
        {
            Assert.True(item.OrderCount > 0);
            Assert.True(item.AverageAmount > 0);
            Assert.True(item.TotalAmount > 0);
        });
    }

    [Fact]
    public void GetClientWithMaximumSpentAmount()
    {
        var result = _fixture.Data.Orders
            .GroupBy(order => order.ClientId)
            .Select(group => new
            {
                ClientId = group.Key,
                TotalSpent = group.Sum(order => order.TotalAmount)
            })
            .OrderByDescending(item => item.TotalSpent)
            .First();


        Assert.NotNull(result);

        Assert.Equal(1, result.ClientId);

        Assert.Equal(7300, result.TotalSpent);
    }
}
