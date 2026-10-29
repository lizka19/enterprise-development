using EnterpriseDevelopment.Application.Services;
using EnterpriseDevelopment.Domain;
using EnterpriseDevelopment.Domain.Entities;
using Xunit;

namespace EnterpriseDevelopment.Application.Tests;

/// <summary>
/// Тесты сервиса управления клиентами
/// </summary>
public class ClientServiceTests
{
    private readonly ClientService _service;

    /// <summary>
    /// Создаёт сервис клиентов с тестовыми данными
    /// </summary>
    public ClientServiceTests()
    {
        _service = new ClientService(new DataSeeder());
    }

    /// <summary>
    /// Проверяет получение списка клиентов
    /// </summary>
    [Fact]
    public void GetAll_ReturnsClients()
    {
        Assert.NotEmpty(_service.GetAll());
    }

    /// <summary>
    /// Проверяет получение существующего клиента
    /// </summary>
    [Fact]
    public void GetById_ExistingClient_ReturnsClient()
    {
        Assert.NotNull(_service.GetById(1));
    }

    /// <summary>
    /// Проверяет получение null для несуществующего клиента
    /// </summary>
    [Fact]
    public void GetById_UnknownClient_ReturnsNull()
    {
        Assert.Null(_service.GetById(999));
    }

    /// <summary>
    /// Проверяет создание нового клиента
    /// </summary>
    [Fact]
    public void Create_ValidClient_AddsClient()
    {
        var client = new Client
        {
            FullName = "Тестовый Клиент",
            PhoneNumber = "+79990000000",
            DeliveryAddress = "Самара, ул. Тестовая, 1"
        };

        var created = _service.Create(client);

        Assert.True(created.Id > 0);
        Assert.NotNull(_service.GetById(created.Id));
    }

    /// <summary>
    /// Проверяет изменение имени существующего клиента
    /// </summary>
    [Fact]
    public void Update_ExistingClient_ChangesName()
    {
        var updated = new Client
        {
            FullName = "Новое имя",
            PhoneNumber = "+79990000001",
            DeliveryAddress = "Самара, ул. Тестовая, 2"
        };

        Assert.True(_service.Update(1, updated));
        Assert.Equal("Новое имя", _service.GetById(1)?.FullName);
    }

    /// <summary>
    /// Проверяет запрет удаления клиента с заказами
    /// </summary>
    [Fact]
    public void Delete_ClientWithOrders_ReturnsFalse()
    {
        Assert.False(_service.Delete(1));
    }

    /// <summary>
    /// Проверяет удаление клиента без заказов
    /// </summary>
    [Fact]
    public void Delete_NewClient_RemovesClient()
    {
        var created = _service.Create(new Client
        {
            FullName = "Для удаления",
            PhoneNumber = "+79990000002",
            DeliveryAddress = "Самара"
        });

        Assert.True(_service.Delete(created.Id));
        Assert.Null(_service.GetById(created.Id));
    }
}
