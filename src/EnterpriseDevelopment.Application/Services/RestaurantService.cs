using EnterpriseDevelopment.Application.Interfaces;
using EnterpriseDevelopment.Domain;
using EnterpriseDevelopment.Domain.Entities;

namespace EnterpriseDevelopment.Application.Services;

public class RestaurantService : IRestaurantService
{
    private readonly DataSeeder _data;

    public RestaurantService(DataSeeder data)
    {
        _data = data;
    }

    public IEnumerable<Restaurant> GetAll()
    {
        return _data.Restaurants;
    }

    public Restaurant? GetById(int id)
    {
        return _data.Restaurants.FirstOrDefault(r => r.Id == id);
    }

    public Restaurant Create(Restaurant restaurant)
    {
        restaurant.Id = _data.Restaurants
            .Select(r => r.Id)
            .DefaultIfEmpty(0)
            .Max() + 1;

        _data.Restaurants.Add(restaurant);

        return restaurant;
    }

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
