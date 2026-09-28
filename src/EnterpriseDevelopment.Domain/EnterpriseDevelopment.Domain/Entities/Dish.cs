namespace EnterpriseDevelopment.Domain.Entities;

/// <summary>
/// Блюдо ресторана
/// </summary>
public class Dish
{
    /// <summary>
    /// Идентификатор блюда
    /// </summary>
    public int Id { get; set; }


    /// <summary>
    /// Название блюда
    /// </summary>
    public string Name { get; set; } = string.Empty;


    /// <summary>
    /// Вес блюда в граммах
    /// </summary>
    public int WeightInGrams { get; set; }


    /// <summary>
    /// Стоимость блюда
    /// </summary>
    public decimal Price { get; set; }


    /// <summary>
    /// Идентификатор категории блюда
    /// </summary>
    public int CategoryId { get; set; }


    /// <summary>
    /// Категория блюда
    /// </summary>
    public DishCategory? Category { get; set; }


    /// <summary>
    /// Идентификатор ресторана
    /// </summary>
    public int RestaurantId { get; set; }


    /// <summary>
    /// Ресторан, в котором готовится блюдо
    /// </summary>
    public Restaurant? Restaurant { get; set; }
}
