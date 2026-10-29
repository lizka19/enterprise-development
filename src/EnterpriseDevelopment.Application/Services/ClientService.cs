using EnterpriseDevelopment.Application.Interfaces;
using EnterpriseDevelopment.Domain;
using EnterpriseDevelopment.Domain.Entities;

namespace EnterpriseDevelopment.Application.Services;

public class ClientService : IClientService
{
    private readonly DataSeeder _data;

    public ClientService(DataSeeder data)
    {
        _data = data;
    }

    public IEnumerable<Client> GetAll()
    {
        return _data.Clients;
    }

    public Client? GetById(int id)
    {
        return _data.Clients.FirstOrDefault(c => c.Id == id);
    }

    public Client Create(Client client)
    {
        client.Id = _data.Clients
            .Select(c => c.Id)
            .DefaultIfEmpty(0)
            .Max() + 1;

        _data.Clients.Add(client);

        return client;
    }

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
