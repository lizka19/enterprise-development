using EnterpriseDevelopment.Domain.Entities;

namespace EnterpriseDevelopment.Application.Interfaces;

/// <summary>
/// Интерфейс сервиса для управления блюдами ресторанов
/// </summary>
public interface IDishService
{
    /// <summary>
    /// Получает список всех блюд
    /// </summary>
    /// <returns>Коллекция блюд</returns>
    public IEnumerable<Dish> GetAll();

    /// <summary>
    /// Получает блюдо по его идентификатору
    /// </summary>
    /// <returns>Найденное блюдо или null, если блюдо отсутствует</returns>
    public Dish? GetById(int id);

    /// <summary>
    /// Создаёт новое блюдо
    /// </summary>
    /// <returns>Созданное блюдо</returns>
    public Dish Create(Dish dish);

    /// <summary>
    /// Обновляет данные существующего блюда
    /// </summary>
    /// <returns>True при успешном обновлении, иначе false</returns>
    public bool Update(int id, Dish dish);

    /// <summary>
    /// Удаляет блюдо по его идентификатору
    /// </summary>
    /// <returns>True при успешном удалении, иначе false</returns>
    public bool Delete(int id);
}
