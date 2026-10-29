using EnterpriseDevelopment.Application.Interfaces;
using EnterpriseDevelopment.Domain;
using EnterpriseDevelopment.Domain.Entities;

namespace EnterpriseDevelopment.Application.Services;

public class DishService : IDishService
{
    private readonly DataSeeder _data;

    public DishService(DataSeeder data)
    {
        _data = data;
    }

    public IEnumerable<Dish> GetAll()
    {
        return _data.Dishes;
    }

    public Dish? GetById(int id)
    {
        return _data.Dishes.FirstOrDefault(d => d.Id == id);
    }

    public Dish Create(Dish dish)
    {
        dish.Id = _data.Dishes
            .Select(d => d.Id)
            .DefaultIfEmpty(0)
            .Max() + 1;

        dish.Category = _data.Categories.First(c => c.Id == dish.CategoryId);
        dish.Restaurant = _data.Restaurants.First(r => r.Id == dish.RestaurantId);

        _data.Dishes.Add(dish);

        return dish;
    }

    public bool Update(int id, Dish updatedDish)
    {
        var dish = GetById(id);

        if (dish == null)
        {
            return false;
        }

        dish.Name = updatedDish.Name;
        dish.Weight = updatedDish.Weight;
        dish.Price = updatedDish.Price;
        dish.CategoryId = updatedDish.CategoryId;
        dish.RestaurantId = updatedDish.RestaurantId;

        dish.Category = _data.Categories.First(c => c.Id == dish.CategoryId);
        dish.Restaurant = _data.Restaurants.First(r => r.Id == dish.RestaurantId);

        return true;
    }

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
}
