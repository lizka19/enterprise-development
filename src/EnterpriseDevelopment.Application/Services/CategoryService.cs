using EnterpriseDevelopment.Application.Interfaces;
using EnterpriseDevelopment.Domain;
using EnterpriseDevelopment.Domain.Entities;

namespace EnterpriseDevelopment.Application.Services;

/// <summary>
/// Сервис для управления категориями блюд
/// </summary>
public class CategoryService : ICategoryService
{
    private readonly DataSeeder _data;

    /// <summary>
    /// Создаёт сервис категорий с указанным хранилищем данных
    /// </summary>
    public CategoryService(DataSeeder data)
    {
        _data = data;
    }

    /// <summary>
    /// Получает список всех категорий блюд
    /// </summary>
    /// <returns>Коллекция категорий блюд</returns>
    public IEnumerable<DishCategory> GetAll()
    {
        return _data.Categories;
    }

    /// <summary>
    /// Получает категорию блюда по её идентификатору
    /// </summary>
    /// <returns>Найденная категория или null, если категория отсутствует</returns>
    public DishCategory? GetById(int id)
    {
        return _data.Categories.FirstOrDefault(c => c.Id == id);
    }

    /// <summary>
    /// Создаёт новую категорию блюд
    /// </summary>
    /// <returns>Созданная категория</returns>
    public DishCategory Create(DishCategory category)
    {
        category.Id = _data.Categories
            .Select(c => c.Id)
            .DefaultIfEmpty(0)
            .Max() + 1;

        _data.Categories.Add(category);

        return category;
    }

    /// <summary>
    /// Обновляет название существующей категории
    /// </summary>
    /// <returns>True при успешном обновлении, иначе false</returns>
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

    /// <summary>
    /// Удаляет категорию, если в ней отсутствуют блюда
    /// </summary>
    /// <returns>True при успешном удалении, иначе false</returns>
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
