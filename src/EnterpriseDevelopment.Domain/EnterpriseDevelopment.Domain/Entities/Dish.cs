namespace EnterpriseDevelopment.Domain.Entities;

public class Dish
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int WeightInGrams { get; set; }

    public decimal Price { get; set; }

    public int CategoryId { get; set; }

    public DishCategory? Category { get; set; }

    public int RestaurantId { get; set; }

    public Restaurant? Restaurant { get; set; }
}