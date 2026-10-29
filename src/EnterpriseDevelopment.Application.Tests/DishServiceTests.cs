using EnterpriseDevelopment.Application.Services;
using EnterpriseDevelopment.Domain;
using EnterpriseDevelopment.Domain.Entities;
using Xunit;

namespace EnterpriseDevelopment.Application.Tests;

/// <summary>
/// Тесты сервиса управления блюдами ресторанов
/// </summary>
public class DishServiceTests
{
    private readonly DishService _service;

    /// <summary>
    /// Создаёт сервис блюд с тестовыми данными
    /// </summary>
    public DishServiceTests()
    {
        _service = new DishService(new DataSeeder());
    }

    /// <summary>
    /// Проверяет получение списка блюд
    /// </summary>
    [Fact]
    public void GetAll_ReturnsDishes()
    {
        Assert.NotEmpty(_service.GetAll());
    }

    /// <summary>
    /// Проверяет получение существующего блюда по идентификатору
    /// </summary>
    [Fact]
    public void GetById_ExistingDish_ReturnsDish()
    {
        Assert.NotNull(_service.GetById(1));
    }

    /// <summary>
    /// Проверяет получение null для несуществующего блюда
    /// </summary>
    [Fact]
    public void GetById_UnknownDish_ReturnsNull()
    {
        Assert.Null(_service.GetById(999));
    }

    /// <summary>
    /// Проверяет создание блюда и установку связей с рестораном и категорией
    /// </summary>
    [Fact]
    public void Create_ValidDish_AddsDish()
    {
        var created = _service.Create(new Dish
        {
            Name = "Тестовая пицца",
            Weight = 400,
            Price = 500,
            CategoryId = 1,
            RestaurantId = 1
        });

        Assert.True(created.Id > 0);
        Assert.NotNull(_service.GetById(created.Id));
        Assert.NotNull(created.Category);
        Assert.NotNull(created.Restaurant);
    }

    /// <summary>
    /// Проверяет изменение названия существующего блюда
    /// </summary>
    [Fact]
    public void Update_ExistingDish_ChangesName()
    {
        var updated = new Dish
        {
            Name = "Обновлённое блюдо",
            Weight = 350,
            Price = 450,
            CategoryId = 1,
            RestaurantId = 1
        };

        Assert.True(_service.Update(1, updated));
        Assert.Equal("Обновлённое блюдо", _service.GetById(1)?.Name);
    }

    /// <summary>
    /// Проверяет запрет удаления блюда, входящего в заказ
    /// </summary>
    [Fact]
    public void Delete_DishUsedInOrder_ReturnsFalse()
    {
        var data = new DataSeeder();
        var service = new DishService(data);

        var dishId = data.Orders
            .SelectMany(o => o.Dishes)
            .First().Id;

        Assert.False(service.Delete(dishId));
    }

    /// <summary>
    /// Проверяет удаление блюда, не используемого в заказах
    /// </summary>
    [Fact]
    public void Delete_NewDish_RemovesDish()
    {
        var created = _service.Create(new Dish
        {
            Name = "Блюдо для удаления",
            Weight = 200,
            Price = 250,
            CategoryId = 1,
            RestaurantId = 1
        });

        Assert.True(_service.Delete(created.Id));
        Assert.Null(_service.GetById(created.Id));
    }
}
