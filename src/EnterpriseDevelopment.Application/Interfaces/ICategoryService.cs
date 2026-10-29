using EnterpriseDevelopment.Domain.Entities;

namespace EnterpriseDevelopment.Application.Interfaces;

/// <summary>
/// Интерфейс сервиса для управления категориями блюд
/// </summary>
public interface ICategoryService
{
    /// <summary>
    /// Получает список всех категорий блюд
    /// </summary>
    /// <returns>Коллекция категорий блюд</returns>
    public IEnumerable<DishCategory> GetAll();

    /// <summary>
    /// Получает категорию блюда по её идентификатору
    /// </summary>
    /// <returns>Найденная категория или null, если категория отсутствует</returns>
    public DishCategory? GetById(int id);

    /// <summary>
    /// Создаёт новую категорию блюд
    /// </summary>
    /// <returns>Созданная категория</returns>
    public DishCategory Create(DishCategory category);

    /// <summary>
    /// Обновляет данные существующей категории
    /// </summary>
    /// <returns>True при успешном обновлении, иначе false</returns>
    public bool Update(int id, DishCategory category);

    /// <summary>
    /// Удаляет категорию блюда по её идентификатору
    /// </summary>
    /// <returns>True при успешном удалении, иначе false</returns>
    public bool Delete(int id);
}
