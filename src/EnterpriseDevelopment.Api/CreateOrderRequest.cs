namespace EnterpriseDevelopment.Api;

/// <summary>
/// Модель данных для создания и изменения заказа
/// </summary>
public class CreateOrderRequest
{
    /// <summary>
    /// Идентификатор клиента, оформляющего заказ
    /// </summary>
    public int ClientId { get; set; }

    /// <summary>
    /// Идентификатор ресторана, в котором оформляется заказ
    /// </summary>
    public int RestaurantId { get; set; }

    /// <summary>
    /// Список идентификаторов блюд, входящих в заказ
    /// </summary>
    public List<int> DishIds { get; set; } = new();

    /// <summary>
    /// Дата и время создания заказа
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Дата и время доставки заказа
    /// </summary>
    public DateTimeOffset DeliveredAt { get; set; }
}
