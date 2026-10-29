using EnterpriseDevelopment.Application.Services;
using EnterpriseDevelopment.Domain;
using Xunit;

namespace EnterpriseDevelopment.Application.Tests;

/// <summary>
/// Тесты сервиса управления заказами
/// </summary>
public class OrderServiceTests
{
    private readonly DataSeeder _data;
    private readonly OrderService _service;

    /// <summary>
    /// Создаёт тестовое хранилище данных и сервис заказов
    /// </summary>
    public OrderServiceTests()
    {
        _data = new DataSeeder();
        _service = new OrderService(_data);
    }

    /// <summary>
    /// Проверяет получение списка заказов
    /// </summary>
    [Fact]
    public void GetAll_ReturnsOrders()
    {
        Assert.NotEmpty(_service.GetAll());
    }

    /// <summary>
    /// Проверяет получение существующего заказа
    /// </summary>
    [Fact]
    public void GetById_ExistingOrder_ReturnsOrder()
    {
        Assert.NotNull(_service.GetById(1));
    }

    /// <summary>
    /// Проверяет получение null для несуществующего заказа
    /// </summary>
    [Fact]
    public void GetById_UnknownOrder_ReturnsNull()
    {
        Assert.Null(_service.GetById(999));
    }

    /// <summary>
    /// Проверяет создание заказа с корректными данными
    /// </summary>
    [Fact]
    public void Create_ValidOrder_AddsOrder()
    {
        var createdAt = new DateTimeOffset(
            2026, 10, 20, 12, 0, 0, TimeSpan.Zero);

        var deliveredAt = createdAt.AddMinutes(35);

        var dishId = _data.Dishes
            .First(d => d.RestaurantId == 1).Id;

        var created = _service.Create(
            1,
            1,
            new List<int> { dishId },
            createdAt,
            deliveredAt);

        Assert.True(created.Id > 0);
        Assert.NotNull(_service.GetById(created.Id));
        Assert.Single(created.Dishes);
        Assert.Equal(TimeSpan.FromMinutes(35), created.DeliveryTime);
    }

    /// <summary>
    /// Проверяет изменение времени доставки существующего заказа
    /// </summary>
    [Fact]
    public void Update_ExistingOrder_ChangesDeliveryTime()
    {
        var createdAt = new DateTimeOffset(
            2026, 10, 20, 12, 0, 0, TimeSpan.Zero);

        var deliveredAt = createdAt.AddMinutes(45);

        var dishId = _data.Dishes
            .First(d => d.RestaurantId == 1).Id;

        var result = _service.Update(
            1,
            1,
            1,
            new List<int> { dishId },
            createdAt,
            deliveredAt);

        Assert.True(result);
        Assert.Equal(
            TimeSpan.FromMinutes(45),
            _service.GetById(1)?.DeliveryTime);
    }

    /// <summary>
    /// Проверяет удаление существующего заказа
    /// </summary>
    [Fact]
    public void Delete_ExistingOrder_RemovesOrder()
    {
        Assert.True(_service.Delete(1));
        Assert.Null(_service.GetById(1));
    }

    /// <summary>
    /// Проверяет результат удаления несуществующего заказа
    /// </summary>
    [Fact]
    public void Delete_UnknownOrder_ReturnsFalse()
    {
        Assert.False(_service.Delete(999));
    }

    /// <summary>
    /// Проверяет возникновение исключения при создании заказа
    /// с несуществующим клиентом
    /// </summary>
    [Fact]
    public void Create_UnknownClient_ThrowsArgumentException()
    {
        var createdAt = new DateTimeOffset(
            2026, 10, 20, 12, 0, 0, TimeSpan.Zero);

        var dishId = _data.Dishes
            .First(d => d.RestaurantId == 1).Id;

        Assert.Throws<ArgumentException>(() =>
            _service.Create(
                999,
                1,
                new List<int> { dishId },
                createdAt,
                createdAt.AddMinutes(30)));
    }
}
