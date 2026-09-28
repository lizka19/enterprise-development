namespace EnterpriseDevelopment.Domain.Entities;

/// <summary>
/// Ресторан службы доставки еды
/// </summary>
public class Restaurant
{
    /// <summary>
    /// Идентификатор ресторана
    /// </summary>
    public int Id { get; set; }


    /// <summary>
    /// Название ресторана
    /// </summary>
    public string Name { get; set; } = string.Empty;


    /// <summary>
    /// Адрес ресторана
    /// </summary>
    public string Address { get; set; } = string.Empty;


    /// <summary>
    /// Рейтинг ресторана
    /// </summary>
    public double Rating { get; set; }


    /// <summary>
    /// Время открытия ресторана
    /// </summary>
    public TimeOnly OpeningTime { get; set; }


    /// <summary>
    /// Время закрытия ресторана
    /// </summary>
    public TimeOnly ClosingTime { get; set; }
}
