using EnterpriseDevelopment.Domain.Entities;

namespace EnterpriseDevelopment.Application.Interfaces;

public interface ICategoryService
{
    public IEnumerable<DishCategory> GetAll();

    public DishCategory? GetById(int id);

    public DishCategory Create(DishCategory category);

    public bool Update(int id, DishCategory category);

    public bool Delete(int id);
}

