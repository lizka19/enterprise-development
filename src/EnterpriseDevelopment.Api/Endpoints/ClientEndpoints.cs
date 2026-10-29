using EnterpriseDevelopment.Domain;
using EnterpriseDevelopment.Domain.Entities;

namespace EnterpriseDevelopment.Api.Endpoints;

public static class ClientEndpoints
{
    public static void MapClientEndpoints(this WebApplication app)
    {
        app.MapGet("/api/clients", (DataSeeder data) =>
        {
            return Results.Ok(data.Clients);
        });

        app.MapGet("/api/clients/{id:int}", (int id, DataSeeder data) =>
        {
            var client = data.Clients.FirstOrDefault(c => c.Id == id);

            if (client == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(client);
        });

        app.MapPost("/api/clients", (Client client, DataSeeder data) =>
        {
            if (string.IsNullOrWhiteSpace(client.FullName) ||
                string.IsNullOrWhiteSpace(client.PhoneNumber) ||
                string.IsNullOrWhiteSpace(client.DeliveryAddress))
            {
                return Results.BadRequest("Некорректные данные клиента");
            }

            client.Id = data.Clients.Select(c => c.Id).DefaultIfEmpty(0).Max() + 1;

            data.Clients.Add(client);

            return Results.Created($"/api/clients/{client.Id}", client);
        });

        app.MapPut("/api/clients/{id:int}", (int id, Client updatedClient, DataSeeder data) =>
        {
            var client = data.Clients.FirstOrDefault(c => c.Id == id);

            if (client == null)
            {
                return Results.NotFound("Клиент не найден");
            }

            if (string.IsNullOrWhiteSpace(updatedClient.FullName) ||
                string.IsNullOrWhiteSpace(updatedClient.PhoneNumber) ||
                string.IsNullOrWhiteSpace(updatedClient.DeliveryAddress))
            {
                return Results.BadRequest("Некорректные данные клиента");
            }

            client.FullName = updatedClient.FullName;
            client.PhoneNumber = updatedClient.PhoneNumber;
            client.DeliveryAddress = updatedClient.DeliveryAddress;

            return Results.Ok(client);
        });

        app.MapDelete("/api/clients/{id:int}", (int id, DataSeeder data) =>
        {
            var client = data.Clients.FirstOrDefault(c => c.Id == id);

            if (client == null)
            {
                return Results.NotFound("Клиент не найден");
            }

            if (data.Orders.Any(o => o.ClientId == id))
            {
                return Results.Conflict(
                    "Нельзя удалить клиента, у которого есть заказы");
            }

            data.Clients.Remove(client);

            return Results.NoContent();
        });
    }
}
