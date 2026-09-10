namespace EnterpriseDevelopment.Domain.Entities;

public class Order
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public Client? Client { get; set; }

    public int RestaurantId { get; set; }

    public Restaurant? Restaurant { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset DeliveredAt { get; set; }

    public decimal TotalAmount { get; set; }

    public TimeSpan DeliveryTime => DeliveredAt - CreatedAt;
}