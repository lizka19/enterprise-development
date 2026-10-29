using EnterpriseDevelopment.Domain.Entities;

namespace EnterpriseDevelopment.Application.Interfaces;

public interface IClientService
{
    public IEnumerable<Client> GetAll();

    public Client? GetById(int id);

    public Client Create(Client client);

    public bool Update(int id, Client client);

    public bool Delete(int id);
}
