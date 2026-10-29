using EnterpriseDevelopment.Domain.Entities;

namespace EnterpriseDevelopment.Application.Interfaces;

public interface IDishService
{
    public IEnumerable<Dish> GetAll();

    public Dish? GetById(int id);

    public Dish Create(Dish dish);

    public bool Update(int id, Dish dish);

    public bool Delete(int id);
}
