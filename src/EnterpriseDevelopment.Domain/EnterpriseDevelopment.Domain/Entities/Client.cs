namespace EnterpriseDevelopment.Domain.Entities;

/// <summary>
/// Клиент службы доставки еды
/// </summary>
public class Client
{
    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ФИО клиента
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Номер телефона клиента
    /// </summary>
    public required string PhoneNumber { get; set; }

    /// <summary>
    /// Адрес доставки клиента
    /// </summary>
    public required string DeliveryAddress { get; set; }
}
