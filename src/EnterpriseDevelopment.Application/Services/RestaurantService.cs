using EnterpriseDevelopment.Application.Interfaces;
using EnterpriseDevelopment.Domain;
using EnterpriseDevelopment.Domain.Entities;

namespace EnterpriseDevelopment.Application.Services;

/// <summary>
/// Сервис для управления ресторанами
/// </summary>
public class RestaurantService : IRestaurantService
{
    private readonly DataSeeder _data;

    /// <summary>
    /// Создаёт сервис ресторанов с указанным хранилищем данных
    /// </summary>
    public RestaurantService(DataSeeder data)
    {
        _data = data;
    }

    /// <summary>
    /// Получает список всех ресторанов
    /// </summary>
    /// <returns>Коллекция ресторанов</returns>
    public IEnumerable<Restaurant> GetAll()
    {
        return _data.Restaurants;
    }

    /// <summary>
    /// Получает ресторан по его идентификатору
    /// </summary>
    /// <returns>Найденный ресторан или null, если ресторан отсутствует</returns>
    public Restaurant? GetById(int id)
    {
        return _data.Restaurants.FirstOrDefault(r => r.Id == id);
    }

    /// <summary>
    /// Создаёт новый ресторан и добавляет его в хранилище данных
    /// </summary>
    /// <returns>Созданный ресторан</returns>
    public Restaurant Create(Restaurant restaurant)
    {
        restaurant.Id = _data.Restaurants
            .Select(r => r.Id)
            .DefaultIfEmpty(0)
            .Max() + 1;

        _data.Restaurants.Add(restaurant);

        return restaurant;
    }

    /// <summary>
    /// Обновляет данные существующего ресторана
    /// </summary>
    /// <returns>True при успешном обновлении, иначе false</returns>
    public bool Update(int id, Restaurant updatedRestaurant)
    {
        var restaurant = GetById(id);

        if (restaurant == null)
        {
            return false;
        }

        restaurant.Name = updatedRestaurant.Name;
        restaurant.Address = updatedRestaurant.Address;
        restaurant.Rating = updatedRestaurant.Rating;
        restaurant.OpeningTime = updatedRestaurant.OpeningTime;
        restaurant.ClosingTime = updatedRestaurant.ClosingTime;

        return true;
    }

    /// <summary>
    /// Удаляет ресторан, если с ним не связаны блюда или заказы
    /// </summary>
    /// <returns>True при успешном удалении, иначе false</returns>
    public bool Delete(int id)
    {
        var restaurant = GetById(id);

        if (restaurant == null)
        {
            return false;
        }

        if (_data.Dishes.Any(d => d.RestaurantId == id) ||
            _data.Orders.Any(o => o.RestaurantId == id))
        {
            return false;
        }

        _data.Restaurants.Remove(restaurant);

        return true;
    }
}
