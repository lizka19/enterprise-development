using EnterpriseDevelopment.Application.Interfaces;
using EnterpriseDevelopment.Domain;
using EnterpriseDevelopment.Domain.Entities;

namespace EnterpriseDevelopment.Application.Services;

/// <summary>
/// Сервис для управления клиентами службы доставки еды
/// </summary>
public class ClientService : IClientService
{
    private readonly DataSeeder _data;

    /// <summary>
    /// Создаёт сервис клиентов с указанным хранилищем данных
    /// </summary>
    public ClientService(DataSeeder data)
    {
        _data = data;
    }

    /// <summary>
    /// Получает список всех клиентов
    /// </summary>
    /// <returns>Коллекция клиентов</returns>
    public IEnumerable<Client> GetAll()
    {
        return _data.Clients;
    }

    /// <summary>
    /// Получает клиента по его идентификатору
    /// </summary>
    /// <returns>Найденный клиент или null, если клиент отсутствует</returns>
    public Client? GetById(int id)
    {
        return _data.Clients.FirstOrDefault(c => c.Id == id);
    }

    /// <summary>
    /// Создаёт нового клиента и добавляет его в хранилище данных
    /// </summary>
    /// <returns>Созданный клиент</returns>
    public Client Create(Client client)
    {
        client.Id = _data.Clients
            .Select(c => c.Id)
            .DefaultIfEmpty(0)
            .Max() + 1;

        _data.Clients.Add(client);

        return client;
    }

    /// <summary>
    /// Обновляет данные существующего клиента
    /// </summary>
    /// <returns>True при успешном обновлении, иначе false</returns>
    public bool Update(int id, Client updatedClient)
    {
        var client = GetById(id);

        if (client == null)
        {
            return false;
        }

        client.FullName = updatedClient.FullName;
        client.PhoneNumber = updatedClient.PhoneNumber;
        client.DeliveryAddress = updatedClient.DeliveryAddress;

        return true;
    }

    /// <summary>
    /// Удаляет клиента, если у него отсутствуют заказы
    /// </summary>
    /// <returns>True при успешном удалении, иначе false</returns>
    public bool Delete(int id)
    {
        var client = GetById(id);

        if (client == null)
        {
            return false;
        }

        if (_data.Orders.Any(o => o.ClientId == id))
        {
            return false;
        }

        _data.Clients.Remove(client);

        return true;
    }
}
