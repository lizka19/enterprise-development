namespace EnterpriseDevelopment.Api;

public class CreateOrderRequest
{
    public int ClientId { get; set; }

    public int RestaurantId { get; set; }

    public List<int> DishIds { get; set; } = [];

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset DeliveredAt { get; set; }
}
