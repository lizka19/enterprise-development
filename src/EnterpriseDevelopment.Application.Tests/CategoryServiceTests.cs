using EnterpriseDevelopment.Application.Services;
using EnterpriseDevelopment.Domain;
using EnterpriseDevelopment.Domain.Entities;
using Xunit;

namespace EnterpriseDevelopment.Application.Tests;

/// <summary>
/// Тесты сервиса управления категориями блюд
/// </summary>
public class CategoryServiceTests
{
    private readonly CategoryService _service;

    /// <summary>
    /// Создаёт сервис категорий с тестовыми данными
    /// </summary>
    public CategoryServiceTests()
    {
        _service = new CategoryService(new DataSeeder());
    }

    /// <summary>
    /// Проверяет получение списка категорий
    /// </summary>
    [Fact]
    public void GetAll_ReturnsCategories()
    {
        Assert.NotEmpty(_service.GetAll());
    }

    /// <summary>
    /// Проверяет получение существующей категории по идентификатору
    /// </summary>
    [Fact]
    public void GetById_ExistingCategory_ReturnsCategory()
    {
        Assert.NotNull(_service.GetById(1));
    }

    /// <summary>
    /// Проверяет получение null для несуществующей категории
    /// </summary>
    [Fact]
    public void GetById_UnknownCategory_ReturnsNull()
    {
        Assert.Null(_service.GetById(999));
    }

    /// <summary>
    /// Проверяет создание новой категории
    /// </summary>
    [Fact]
    public void Create_ValidCategory_AddsCategory()
    {
        var created = _service.Create(new DishCategory
        {
            Name = "Тестовая категория"
        });

        Assert.True(created.Id > 0);
        Assert.NotNull(_service.GetById(created.Id));
    }

    /// <summary>
    /// Проверяет изменение названия существующей категории
    /// </summary>
    [Fact]
    public void Update_ExistingCategory_ChangesName()
    {
        Assert.True(_service.Update(1, new DishCategory
        {
            Name = "Обновлённая категория"
        }));

        Assert.Equal(
            "Обновлённая категория",
            _service.GetById(1)?.Name);
    }

    /// <summary>
    /// Проверяет запрет удаления категории с блюдами
    /// </summary>
    [Fact]
    public void Delete_CategoryWithDishes_ReturnsFalse()
    {
        Assert.False(_service.Delete(1));
    }

    /// <summary>
    /// Проверяет успешное удаление новой категории без блюд
    /// </summary>
    [Fact]
    public void Delete_NewCategory_RemovesCategory()
    {
        var created = _service.Create(new DishCategory
        {
            Name = "Категория для удаления"
        });

        Assert.True(_service.Delete(created.Id));
        Assert.Null(_service.GetById(created.Id));
    }
}
