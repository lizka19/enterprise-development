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
    public string FullName { get; set; } = string.Empty;


    /// <summary>
    /// Номер телефона клиента
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;


    /// <summary>
    /// Адрес доставки клиента
    /// </summary>
    public string DeliveryAddress { get; set; } = string.Empty;
}
