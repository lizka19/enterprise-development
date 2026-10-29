using EnterpriseDevelopment.Domain.Entities;

namespace EnterpriseDevelopment.Application.Interfaces;

public interface IRestaurantService
{
    public IEnumerable<Restaurant> GetAll();

    public Restaurant? GetById(int id);

    public Restaurant Create(Restaurant restaurant);

    public bool Update(int id, Restaurant restaurant);

    public bool Delete(int id);
}
