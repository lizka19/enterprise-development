using EnterpriseDevelopment.Application.Interfaces;
using EnterpriseDevelopment.Domain;
using EnterpriseDevelopment.Domain.Entities;

namespace EnterpriseDevelopment.Application.Services;

public class OrderService : IOrderService
{
    private readonly DataSeeder _data;

    public OrderService(DataSeeder data)
    {
        _data = data;
    }

    public IEnumerable<Order> GetAll()
    {
        return _data.Orders;
    }

    public Order? GetById(int id)
    {
        return _data.Orders.FirstOrDefault(o => o.Id == id);
    }

    public Order Create(
        int clientId,
        int restaurantId,
        List<int> dishIds,
        DateTimeOffset createdAt,
        DateTimeOffset deliveredAt)
    {
        var client = _data.Clients.First(c => c.Id == clientId);
        var restaurant = _data.Restaurants.First(r => r.Id == restaurantId);

        var dishes = dishIds
            .Select(id => _data.Dishes.First(d => d.Id == id))
            .ToList();

        var order = new Order
        {
            Id = _data.Orders.Select(o => o.Id).DefaultIfEmpty(0).Max() + 1,
            ClientId = client.Id,
            Client = client,
            RestaurantId = restaurant.Id,
            Restaurant = restaurant,
            Dishes = dishes,
            CreatedAt = createdAt,
            DeliveredAt = deliveredAt
        };

        _data.Orders.Add(order);

        return order;
    }

    public bool Update(
        int id,
        int clientId,
        int restaurantId,
        List<int> dishIds,
        DateTimeOffset createdAt,
        DateTimeOffset deliveredAt)
    {
        var order = GetById(id);

        if (order == null)
        {
            return false;
        }

        var client = _data.Clients.First(c => c.Id == clientId);
        var restaurant = _data.Restaurants.First(r => r.Id == restaurantId);

        var dishes = dishIds
            .Select(dishId => _data.Dishes.First(d => d.Id == dishId))
            .ToList();

        order.ClientId = client.Id;
        order.Client = client;
        order.RestaurantId = restaurant.Id;
        order.Restaurant = restaurant;
        order.Dishes = dishes;
        order.CreatedAt = createdAt;
        order.DeliveredAt = deliveredAt;

        return true;
    }

    public bool Delete(int id)
    {
        var order = GetById(id);

        if (order == null)
        {
            return false;
        }

        _data.Orders.Remove(order);

        return true;
    }
}
