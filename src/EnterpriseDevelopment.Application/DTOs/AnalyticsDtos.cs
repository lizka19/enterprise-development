namespace EnterpriseDevelopment.Application.DTOs;

/// <summary>
/// Модель данных для отображения ресторанов с наибольшим количеством заказов
/// </summary>
public record TopRestaurantDto(
    int RestaurantId,
    string RestaurantName,
    int OrderCount
);

/// <summary>
/// Модель данных для отображения заказов с минимальным временем доставки
/// </summary>
public record FastestOrderDto(
    int OrderId,
    int ClientId,
    int RestaurantId,
    TimeSpan DeliveryTime
);

/// <summary>
/// Модель данных для отображения клиентов определённого ресторана
/// </summary>
public record RestaurantClientDto(
    int ClientId,
    string FullName,
    string PhoneNumber
);

/// <summary>
/// Модель данных для отображения статистики категорий блюд
/// </summary>
public record CategoryStatisticsDto(
    int CategoryId,
    string CategoryName,
    int OrderCount
);

/// <summary>
/// Модель данных для отображения клиентов с наибольшей суммой заказов
/// </summary>
public record TopClientDto(
    int ClientId,
    string FullName,
    decimal TotalSpent
);
