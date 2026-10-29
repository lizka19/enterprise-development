using EnterpriseDevelopment.Application.DTOs;

namespace EnterpriseDevelopment.Application.Interfaces;

/// <summary>
/// Интерфейс сервиса для получения аналитических данных
/// о работе сервиса доставки еды
/// </summary>
public interface IAnalyticsService
{
    /// <summary>
    /// Получает пять ресторанов с наибольшим количеством заказов
    /// </summary>
    /// <returns>Коллекция ресторанов с количеством заказов</returns>
    public IEnumerable<TopRestaurantDto> GetTopRestaurants();

    /// <summary>
    /// Получает все заказы с минимальным временем доставки
    /// </summary>
    /// <returns>Коллекция заказов с минимальным временем доставки</returns>
    public IEnumerable<FastestOrderDto> GetFastestOrders();

    /// <summary>
    /// Получает уникальных клиентов указанного ресторана
    /// </summary>
    /// <returns>Коллекция клиентов ресторана</returns>
    public IEnumerable<RestaurantClientDto> GetRestaurantClients(int restaurantId);

    /// <summary>
    /// Получает статистику заказанных блюд по категориям за указанный период
    /// </summary>
    /// <returns>Коллекция категорий с количеством заказанных блюд</returns>
    public IEnumerable<CategoryStatisticsDto> GetCategoryStatistics(
        DateTimeOffset startDate,
        DateTimeOffset endDate);

    /// <summary>
    /// Получает клиента с наибольшей суммой заказов
    /// </summary>
    /// <returns>Коллекция клиентов с максимальной суммой заказов</returns>
    public IEnumerable<TopClientDto> GetTopClients();
}
