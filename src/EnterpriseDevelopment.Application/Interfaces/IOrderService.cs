using EnterpriseDevelopment.Domain.Entities;

namespace EnterpriseDevelopment.Application.Interfaces;

public interface IOrderService
{
    public IEnumerable<Order> GetAll();

    public Order? GetById(int id);

    public Order Create(
        int clientId,
        int restaurantId,
        List<int> dishIds,
        DateTimeOffset createdAt,
        DateTimeOffset deliveredAt);

    public bool Update(
        int id,
        int clientId,
        int restaurantId,
        List<int> dishIds,
        DateTimeOffset createdAt,
        DateTimeOffset deliveredAt);

    public bool Delete(int id);
}

