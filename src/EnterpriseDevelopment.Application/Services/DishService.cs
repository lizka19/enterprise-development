using EnterpriseDevelopment.Application.Interfaces;
using EnterpriseDevelopment.Domain;
using EnterpriseDevelopment.Domain.Entities;

namespace EnterpriseDevelopment.Application.Services;

/// <summary>
/// Сервис для управления блюдами ресторанов
/// </summary>
public class DishService : IDishService
{
    private readonly DataSeeder _data;

    /// <summary>
    /// Создаёт сервис блюд с указанным хранилищем данных
    /// </summary>
    public DishService(DataSeeder data)
    {
        _data = data;
    }

    /// <summary>
    /// Получает список всех блюд
    /// </summary>
    /// <returns>Коллекция блюд</returns>
    public IEnumerable<Dish> GetAll()
    {
        return _data.Dishes;
    }

    /// <summary>
    /// Получает блюдо по его идентификатору
    /// </summary>
    /// <returns>Найденное блюдо или null, если блюдо отсутствует</returns>
    public Dish? GetById(int id)
    {
        return _data.Dishes.FirstOrDefault(d => d.Id == id);
    }

    /// <summary>
    /// Создаёт новое блюдо после проверки его данных
    /// </summary>
    /// <returns>Созданное блюдо</returns>
    public Dish Create(Dish dish)
    {
        ValidateDish(dish);

        dish.Id = _data.Dishes
            .Select(d => d.Id)
            .DefaultIfEmpty(0)
            .Max() + 1;

        dish.Category = _data.Categories
            .First(c => c.Id == dish.CategoryId);

        dish.Restaurant = _data.Restaurants
            .First(r => r.Id == dish.RestaurantId);

        _data.Dishes.Add(dish);

        return dish;
    }

    /// <summary>
    /// Обновляет данные существующего блюда после проверки
    /// </summary>
    /// <returns>True при успешном обновлении, иначе false</returns>
    public bool Update(int id, Dish updatedDish)
    {
        var dish = GetById(id);

        if (dish == null)
        {
            return false;
        }

        ValidateDish(updatedDish);

        dish.Name = updatedDish.Name;
        dish.Weight = updatedDish.Weight;
        dish.Price = updatedDish.Price;
        dish.CategoryId = updatedDish.CategoryId;
        dish.RestaurantId = updatedDish.RestaurantId;

        dish.Category = _data.Categories
            .First(c => c.Id == dish.CategoryId);

        dish.Restaurant = _data.Restaurants
            .First(r => r.Id == dish.RestaurantId);

        return true;
    }

    /// <summary>
    /// Удаляет блюдо, если оно не используется в заказах
    /// </summary>
    /// <returns>True при успешном удалении, иначе false</returns>
    public bool Delete(int id)
    {
        var dish = GetById(id);

        if (dish == null)
        {
            return false;
        }

        if (_data.Orders.Any(o => o.Dishes.Any(d => d.Id == id)))
        {
            return false;
        }

        _data.Dishes.Remove(dish);

        return true;
    }

    /// <summary>
    /// Проверяет корректность данных блюда и наличие связанных
    /// категории и ресторана
    /// </summary>
    private void ValidateDish(Dish dish)
    {
        if (string.IsNullOrWhiteSpace(dish.Name) ||
            dish.Weight <= 0 ||
            dish.Price <= 0)
        {
            throw new ArgumentException("Некорректные данные блюда");
        }

        if (!_data.Categories.Any(c => c.Id == dish.CategoryId))
        {
            throw new ArgumentException("Категория не найдена");
        }

        if (!_data.Restaurants.Any(r => r.Id == dish.RestaurantId))
        {
            throw new ArgumentException("Ресторан не найден");
        }
    }
}
