using EnterpriseDevelopment.Application.Interfaces;
using EnterpriseDevelopment.Domain;
using EnterpriseDevelopment.Domain.Entities;

namespace EnterpriseDevelopment.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly DataSeeder _data;

    public CategoryService(DataSeeder data)
    {
        _data = data;
    }

    public IEnumerable<DishCategory> GetAll()
    {
        return _data.Categories;
    }

    public DishCategory? GetById(int id)
    {
        return _data.Categories.FirstOrDefault(c => c.Id == id);
    }

    public DishCategory Create(DishCategory category)
    {
        category.Id = _data.Categories
            .Select(c => c.Id)
            .DefaultIfEmpty(0)
            .Max() + 1;

        _data.Categories.Add(category);

        return category;
    }

    public bool Update(int id, DishCategory updatedCategory)
    {
        var category = GetById(id);

        if (category == null)
        {
            return false;
        }

        category.Name = updatedCategory.Name;

        return true;
    }

    public bool Delete(int id)
    {
        var category = GetById(id);

        if (category == null)
        {
            return false;
        }

        if (_data.Dishes.Any(d => d.CategoryId == id))
        {
            return false;
        }

        _data.Categories.Remove(category);

        return true;
    }
}

