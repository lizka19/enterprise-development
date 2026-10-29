using EnterpriseDevelopment.Application.Interfaces;
using EnterpriseDevelopment.Domain.Entities;

namespace EnterpriseDevelopment.Api.Endpoints;

public static class ClientEndpoints
{
    public static void MapClientEndpoints(this WebApplication app)
    {
        app.MapGet("/api/clients", (IClientService service) =>
        {
            return Results.Ok(service.GetAll());
        });

        app.MapGet("/api/clients/{id:int}", (int id, IClientService service) =>
        {
            var client = service.GetById(id);

            if (client == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(client);
        });

        app.MapPost("/api/clients", (Client client, IClientService service) =>
        {
            if (string.IsNullOrWhiteSpace(client.FullName) ||
                string.IsNullOrWhiteSpace(client.PhoneNumber) ||
                string.IsNullOrWhiteSpace(client.DeliveryAddress))
            {
                return Results.BadRequest("Некорректные данные клиента");
            }

            var createdClient = service.Create(client);

            return Results.Created($"/api/clients/{createdClient.Id}", createdClient);
        });

        app.MapPut("/api/clients/{id:int}", (int id, Client updatedClient, IClientService service) =>
        {
            if (service.GetById(id) == null)
            {
                return Results.NotFound("Клиент не найден");
            }

            if (string.IsNullOrWhiteSpace(updatedClient.FullName) ||
                string.IsNullOrWhiteSpace(updatedClient.PhoneNumber) ||
                string.IsNullOrWhiteSpace(updatedClient.DeliveryAddress))
            {
                return Results.BadRequest("Некорректные данные клиента");
            }

            service.Update(id, updatedClient);

            return Results.Ok(service.GetById(id));
        });

        app.MapDelete("/api/clients/{id:int}", (int id, IClientService service) =>
        {
            if (service.GetById(id) == null)
            {
                return Results.NotFound("Клиент не найден");
            }

            if (!service.Delete(id))
            {
                return Results.Conflict("Нельзя удалить клиента, у которого есть заказы");
            }

            return Results.NoContent();
        });
    }
}
