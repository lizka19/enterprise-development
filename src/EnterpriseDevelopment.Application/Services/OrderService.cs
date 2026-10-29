using EnterpriseDevelopment.Application.Interfaces;
using EnterpriseDevelopment.Domain;
using EnterpriseDevelopment.Domain.Entities;

namespace EnterpriseDevelopment.Application.Services;

/// <summary>
/// Сервис для управления заказами службы доставки еды
/// </summary>
public class OrderService : IOrderService
{
    private readonly DataSeeder _data;

    /// <summary>
    /// Создаёт сервис заказов с указанным хранилищем данных
    /// </summary>
    public OrderService(DataSeeder data)
    {
        _data = data;
    }

    /// <summary>
    /// Получает список всех заказов
    /// </summary>
    /// <returns>Коллекция заказов</returns>
    public IEnumerable<Order> GetAll()
    {
        return _data.Orders;
    }

    /// <summary>
    /// Получает заказ по его идентификатору
    /// </summary>
    /// <returns>Найденный заказ или null, если заказ отсутствует</returns>
    public Order? GetById(int id)
    {
        return _data.Orders.FirstOrDefault(o => o.Id == id);
    }

    /// <summary>
    /// Создаёт новый заказ после проверки клиента, ресторана, списка блюд и времени доставки
    /// </summary>
    /// <returns>Созданный заказ</returns>
    public Order Create(
        int clientId,
        int restaurantId,
        List<int> dishIds,
        DateTimeOffset createdAt,
        DateTimeOffset deliveredAt)
    {
        var (client, restaurant, dishes) =
            ValidateOrder(
                clientId,
                restaurantId,
                dishIds,
                createdAt,
                deliveredAt);

        var order = new Order
        {
            Id = _data.Orders
                .Select(o => o.Id)
                .DefaultIfEmpty(0)
                .Max() + 1,

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

    /// <summary>
    /// Обновляет данные существующего заказа после проверки
    /// </summary>
    /// <returns>True при успешном обновлении, иначе false</returns>
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

        var (client, restaurant, dishes) =
            ValidateOrder(
                clientId,
                restaurantId,
                dishIds,
                createdAt,
                deliveredAt);

        order.ClientId = client.Id;
        order.Client = client;

        order.RestaurantId = restaurant.Id;
        order.Restaurant = restaurant;

        order.Dishes = dishes;

        order.CreatedAt = createdAt;
        order.DeliveredAt = deliveredAt;

        return true;
    }

    /// <summary>
    /// Удаляет заказ по его идентификатору
    /// </summary>
    /// <returns>True при успешном удалении, иначе false</returns>
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

    /// <summary>
    /// Проверяет наличие клиента, ресторана и блюд, принадлежность блюд ресторану и корректность времени доставки
    /// </summary>
    /// <returns>Клиент, ресторан и список блюд проверенного заказа</returns>
    private (Client Client, Restaurant Restaurant, List<Dish> Dishes)
        ValidateOrder(
            int clientId,
            int restaurantId,
            List<int> dishIds,
            DateTimeOffset createdAt,
            DateTimeOffset deliveredAt)
    {
        var client = _data.Clients
            .FirstOrDefault(c => c.Id == clientId);

        if (client == null)
        {
            throw new ArgumentException("Клиент не найден");
        }

        var restaurant = _data.Restaurants
            .FirstOrDefault(r => r.Id == restaurantId);

        if (restaurant == null)
        {
            throw new ArgumentException("Ресторан не найден");
        }

        if (dishIds == null || dishIds.Count == 0)
        {
            throw new ArgumentException(
                "Заказ должен содержать блюда");
        }

        var dishes = new List<Dish>();

        foreach (var dishId in dishIds)
        {
            var dish = _data.Dishes
                .FirstOrDefault(d => d.Id == dishId);

            if (dish == null)
            {
                throw new ArgumentException(
                    $"Блюдо с ID {dishId} не найдено");
            }

            if (dish.RestaurantId != restaurantId)
            {
                throw new ArgumentException(
                    "Все блюда должны принадлежать выбранному ресторану");
            }

            dishes.Add(dish);
        }

        if (deliveredAt < createdAt)
        {
            throw new ArgumentException(
                "Время доставки не может быть раньше времени создания заказа");
        }

        return (client, restaurant, dishes);
    }
}
