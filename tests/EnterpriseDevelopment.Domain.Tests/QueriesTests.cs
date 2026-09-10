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
}
