using EnterpriseDevelopment.Domain.Entities;

namespace EnterpriseDevelopment.Application.Interfaces;

/// <summary>
/// Интерфейс сервиса для управления клиентами
/// </summary>
public interface IClientService
{
    /// <summary>
    /// Получает список всех клиентов
    /// </summary>
    /// <returns>Коллекция клиентов</returns>
    public IEnumerable<Client> GetAll();

    /// <summary>
    /// Получает клиента по его идентификатору
    /// </summary>
    /// <returns>Найденный клиент или null, если клиент отсутствует</returns>
    public Client? GetById(int id);

    /// <summary>
    /// Создаёт нового клиента
    /// </summary>
    /// <returns>Созданный клиент</returns>
    public Client Create(Client client);

    /// <summary>
    /// Обновляет данные существующего клиента
    /// </summary>
    /// <returns>True при успешном обновлении, иначе false</returns>
    public bool Update(int id, Client client);

    /// <summary>
    /// Удаляет клиента по его идентификатору
    /// </summary>
    /// <returns>True при успешном удалении, иначе false</returns>
    public bool Delete(int id);
}
