namespace EnterpriseDevelopment.Domain.Entities;

/// <summary>
/// Заказ клиента в службе доставки еды
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
    public Client? Client { get; set; }


    /// <summary>
    /// Идентификатор ресторана
    /// </summary>
    public int RestaurantId { get; set; }


    /// <summary>
    /// Ресторан, из которого оформлен заказ
    /// </summary>
    public Restaurant? Restaurant { get; set; }


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
    public decimal TotalAmount { get; set; }


    /// <summary>
    /// Время доставки заказа
    /// </summary>
    public TimeSpan DeliveryTime => DeliveredAt - CreatedAt;
}
