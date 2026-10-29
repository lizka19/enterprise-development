using EnterpriseDevelopment.Domain.Entities;

namespace EnterpriseDevelopment.Application.Interfaces;

/// <summary>
/// Интерфейс сервиса для управления ресторанами
/// </summary>
public interface IRestaurantService
{
    /// <summary>
    /// Получает список всех ресторанов
    /// </summary>
    /// <returns>Коллекция ресторанов</returns>
    public IEnumerable<Restaurant> GetAll();

    /// <summary>
    /// Получает ресторан по его идентификатору
    /// </summary>
    /// <returns>Найденный ресторан или null, если ресторан отсутствует</returns>
    public Restaurant? GetById(int id);

    /// <summary>
    /// Создаёт новый ресторан
    /// </summary>
    /// <returns>Созданный ресторан</returns>
    public Restaurant Create(Restaurant restaurant);

    /// <summary>
    /// Обновляет данные существующего ресторана
    /// </summary>
    /// <returns>True при успешном обновлении, иначе false</returns>
    public bool Update(int id, Restaurant restaurant);

    /// <summary>
    /// Удаляет ресторан по его идентификатору
    /// </summary>
    /// <returns>True при успешном удалении, иначе false</returns>
    public bool Delete(int id);
}
