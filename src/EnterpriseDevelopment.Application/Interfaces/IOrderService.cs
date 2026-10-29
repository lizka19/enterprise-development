using EnterpriseDevelopment.Domain.Entities;

namespace EnterpriseDevelopment.Application.Interfaces;

/// <summary>
/// Интерфейс сервиса для управления заказами
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Получает список всех заказов
    /// </summary>
    /// <returns>Коллекция заказов</returns>
    public IEnumerable<Order> GetAll();

    /// <summary>
    /// Получает заказ по его идентификатору
    /// </summary>
    /// <returns>Найденный заказ или null, если заказ отсутствует</returns>
    public Order? GetById(int id);

    /// <summary>
    /// Создаёт новый заказ с указанными клиентом, рестораном и блюдами
    /// </summary>
    /// <returns>Созданный заказ</returns>
    public Order Create(
        int clientId,
        int restaurantId,
        List<int> dishIds,
        DateTimeOffset createdAt,
        DateTimeOffset deliveredAt);

    /// <summary>
    /// Обновляет данные существующего заказа
    /// </summary>
    /// <returns>True при успешном обновлении, иначе false</returns>
    public bool Update(
        int id,
        int clientId,
        int restaurantId,
        List<int> dishIds,
        DateTimeOffset createdAt,
        DateTimeOffset deliveredAt);

    /// <summary>
    /// Удаляет заказ по его идентификатору
    /// </summary>
    /// <returns>True при успешном удалении, иначе false</returns>
    public bool Delete(int id);
}
