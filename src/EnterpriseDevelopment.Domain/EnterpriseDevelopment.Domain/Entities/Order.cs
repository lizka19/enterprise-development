namespace EnterpriseDevelopment.Domain.Entities;

/// <summary>
/// Заказ клиента
/// </summary>
public class Order
{
    /// <summary>
    /// Идентификатор заказа
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    public int ClientId { get; set; }

    /// <summary>
    /// Клиент, оформивший заказ
    /// </summary>
    public required Client Client { get; set; }

    /// <summary>
    /// Идентификатор ресторана
    /// </summary>
    public int RestaurantId { get; set; }

    /// <summary>
    /// Ресторан, из которого оформлен заказ
    /// </summary>
    public required Restaurant Restaurant { get; set; }

    /// <summary>
    /// Блюда, входящие в заказ
    /// </summary>
    public List<Dish> Dishes { get; set; } = [];

    /// <summary>
    /// Время создания заказа
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Время доставки заказа
    /// </summary>
    public DateTimeOffset DeliveredAt { get; set; }

    /// <summary>
    /// Итоговая сумма заказа
    /// </summary>
    public decimal TotalAmount => Dishes.Sum(dish => dish.Price);

    /// <summary>
    /// Время доставки заказа
    /// </summary>
    public TimeSpan DeliveryTime => DeliveredAt - CreatedAt;
}
