using EnterpriseDevelopment.Domain.Entities;

namespace EnterpriseDevelopment.Domain;

/// <summary>
/// генератор текстовых данных
/// </summary>
public class DataSeeder
{
    /// <summary>
    /// Список категорий блюд
    /// </summary>
    public List<DishCategory> Categories { get; }


    /// <summary>
    /// Список ресторанов
    /// </summary>
    public List<Restaurant> Restaurants { get; }


    /// <summary>
    /// Список клиентов
    /// </summary>
    public List<Client> Clients { get; }


    /// <summary>
    /// Список блюд
    /// </summary>
    public List<Dish> Dishes { get; }


    /// <summary>
    /// Список заказов
    /// </summary>
    public List<Order> Orders { get; }

    /// <summary>
    /// Инициализация текстовых данных
    /// </summary>
    public DataSeeder()
    {
        Categories = CreateCategories();

        Restaurants = CreateRestaurants();

        Clients = CreateClients();

        Dishes = CreateDishes(Categories, Restaurants);

        Orders = CreateOrders(Clients, Restaurants);
    }

    /// <summary>
    /// Создаёт список категорий блюд
    /// </summary>
    /// <returns>Список категорий блюд</returns>
    private static List<DishCategory> CreateCategories()
    {
        return new List<DishCategory>
        {
            new()
            {
                Id = 1,
                Name = "Пицца"
            },

            new()
            {
                Id = 2,
                Name = "Суши"
            },

            new()
            {
                Id = 3,
                Name = "Бургеры"
            },

            new()
            {
                Id = 4,
                Name = "Супы"
            },

            new()
            {
                Id = 5,
                Name = "Салаты"
            },

            new()
            {
                Id = 6,
                Name = "Десерты"
            },

            new()
            {
                Id = 7,
                Name = "Напитки"
            },

            new()
            {
                Id = 8,
                Name = "Закуски"
            },

            new()
            {
                Id = 9,
                Name = "Паста"
            },

            new()
            {
                Id = 10,
                Name = "Горячие блюда"
            }
        };
    }

    /// <summary>
    /// Создаёт список ресторанов
    /// </summary>
    /// <returns>Список ресторанов</returns>
    private static List<Restaurant> CreateRestaurants()
    {
        return new List<Restaurant>
        {
            new()
            {
                Id = 1,
                Name = "Сицилия",
                Address = "ул. Гагарина, 10",
                Rating = 4.7,
                OpeningTime = new TimeOnly(10, 0),
                ClosingTime = new TimeOnly(23, 0)
            },

            new()
            {
                Id = 2,
                Name = "Токио",
                Address = "ул. Московская, 25",
                Rating = 4.6,
                OpeningTime = new TimeOnly(11, 0),
                ClosingTime = new TimeOnly(23, 30)
            },

            new()
            {
                Id = 3,
                Name = "Burger House",
                Address = "ул. Победы, 15",
                Rating = 4.4,
                OpeningTime = new TimeOnly(9, 0),
                ClosingTime = new TimeOnly(22, 0)
            },

            new()
            {
                Id = 4,
                Name = "Хачапури",
                Address = "ул. Ленинградская, 40",
                Rating = 4.8,
                OpeningTime = new TimeOnly(10, 0),
                ClosingTime = new TimeOnly(23, 0)
            },

            new()
            {
                Id = 5,
                Name = "Вилка",
                Address = "ул. Советской Армии, 30",
                Rating = 4.3,
                OpeningTime = new TimeOnly(8, 0),
                ClosingTime = new TimeOnly(21, 0)
            },

            new()
            {
                Id = 6,
                Name = "Паста Бар",
                Address = "ул. Самарская, 81",
                Rating = 4.5,
                OpeningTime = new TimeOnly(11, 0),
                ClosingTime = new TimeOnly(23, 0)
            },

            new()
            {
                Id = 7,
                Name = "Шашлык №1",
                Address = "ул. Победы, 90",
                Rating = 4.2,
                OpeningTime = new TimeOnly(10, 0),
                ClosingTime = new TimeOnly(22, 0)
            },


            new()
            {
                Id = 8,
                Name = "Green Food",
                Address = "ул. Осипенко, 12",
                Rating = 4.6,
                OpeningTime = new TimeOnly(9, 0),
                ClosingTime = new TimeOnly(21, 0)
            },

            new()
            {
                Id = 9,
                Name = "Sweet Home",
                Address = "ул. Полевая, 55",
                Rating = 4.9,
                OpeningTime = new TimeOnly(9, 0),
                ClosingTime = new TimeOnly(22, 0)
            },

            new()
            {
                Id = 10,
                Name = "Горячий двор",
                Address = "ул. Авроры, 100",
                Rating = 4.1,
                OpeningTime = new TimeOnly(10, 0),
                ClosingTime = new TimeOnly(23, 0)
            }
        };
    }

    /// <summary>
    /// Создаёт список клиентов
    /// </summary>
    /// <returns>Список клиентов</returns>
    private static List<Client> CreateClients()
    {
        return new List<Client>
        {
            new()
            {
                Id = 1,
                FullName = "Иванов Иван Иванович",
                PhoneNumber = "+79991234501",
                DeliveryAddress = "ул. Гагарина, 10"
            },

            new()
            {
                Id = 2,
                FullName = "Петров Пётр Сергеевич",
                PhoneNumber = "+79991234502",
                DeliveryAddress = "ул. Московская, 25"
            },

            new()
            {
                Id = 3,
                FullName = "Сидорова Анна Андреевна",
                PhoneNumber = "+79991234503",
                DeliveryAddress = "ул. Победы, 15"
            },

            new()
            {
                Id = 4,
                FullName = "Кузнецов Алексей Олегович",
                PhoneNumber = "+79991234504",
                DeliveryAddress = "ул. Ленинградская, 40"
            },

            new()
            {
                Id = 5,
                FullName = "Смирнова Екатерина Павловна",
                PhoneNumber = "+79991234505",
                DeliveryAddress = "ул. Советской Армии, 30"
            },

            new()
            {
                Id = 6,
                FullName = "Попов Дмитрий Игоревич",
                PhoneNumber = "+79991234506",
                DeliveryAddress = "ул. Самарская, 81"
            },

            new()
            {
                Id = 7,
                FullName = "Соколова Мария Олеговна",
                PhoneNumber = "+79991234507",
                DeliveryAddress = "ул. Осипенко, 12"
            },

            new()
            {
                Id = 8,
                FullName = "Морозов Артём Сергеевич",
                PhoneNumber = "+79991234508",
                DeliveryAddress = "ул. Полевая, 55"
            },

            new()
            {
                Id = 9,
                FullName = "Волкова Алина Романовна",
                PhoneNumber = "+79991234509",
                DeliveryAddress = "ул. Авроры, 100"
            },

            new()
            {
                Id = 10,
                FullName = "Орлов Максим Евгеньевич",
                PhoneNumber = "+79991234510",
                DeliveryAddress = "Московское шоссе, 20"
            }
        };
    }

    /// <summary>
    /// Создаёт список блюд
    /// </summary>
    /// <returns>Список блюд</returns>
    private static List<Dish> CreateDishes(
    List<DishCategory> categories,
    List<Restaurant> restaurants)
    {
        return new List<Dish>
        {
            new()
            {
                Id = 1,
                Name = "Пепперони",
                WeightInGrams = 450,
                Price = 520,

                CategoryId = 1,
                Category = categories[0],

                RestaurantId = 1,
                Restaurant = restaurants[0]
            },

            new()
            {
                Id = 2,
                Name = "Маргарита",
                WeightInGrams = 420,
                Price = 450,

                CategoryId = 1,
                Category = categories[0],

                RestaurantId = 1,
                Restaurant = restaurants[0]
            },

            new()
            {
                Id = 3,
                Name = "Филадельфия",
                WeightInGrams = 260,
                Price = 650,

                CategoryId = 2,
                Category = categories[1],

                RestaurantId = 2,
                Restaurant = restaurants[1]
            },

            new()
            {
                Id = 4,
                Name = "Калифорния",
                WeightInGrams = 240,
                Price = 590,

                CategoryId = 2,
                Category = categories[1],

                RestaurantId = 2,
                Restaurant = restaurants[1]
            },

            new()
            {
                Id = 5,
                Name = "Чизбургер",
                WeightInGrams = 320,
                Price = 390,

                CategoryId = 3,
                Category = categories[2],

                RestaurantId = 3,
                Restaurant = restaurants[2]
            },

            new()
            {
                Id = 6,
                Name = "Хачапури по-аджарски",
                WeightInGrams = 400,
                Price = 480,

                CategoryId = 10,
                Category = categories[9],

                RestaurantId = 4,
                Restaurant = restaurants[3]
            },

            new()
            {
                Id = 7,
                Name = "Борщ",
                WeightInGrams = 350,
                Price = 300,

                CategoryId = 4,
                Category = categories[3],

                RestaurantId = 5,
                Restaurant = restaurants[4]
            },

            new()
            {
                Id = 8,
                Name = "Карбонара",
                WeightInGrams = 330,
                Price = 510,

                CategoryId = 9,
                Category = categories[8],

                RestaurantId = 6,
                Restaurant = restaurants[5]
            },

            new()
            {
                Id = 9,
                Name = "Шашлык из свинины",
                WeightInGrams = 300,
                Price = 560,

                CategoryId = 10,
                Category = categories[9],

                RestaurantId = 7,
                Restaurant = restaurants[6]
            },

            new()
            {
                Id = 10,
                Name = "Цезарь",
                WeightInGrams = 250,
                Price = 420,

                CategoryId = 5,
                Category = categories[4],

                RestaurantId = 8,
                Restaurant = restaurants[7]
            },

            new()
            {
                Id = 11,
                Name = "Чизкейк",
                WeightInGrams = 180,
                Price = 350,

                CategoryId = 6,
                Category = categories[5],

                RestaurantId = 9,
                Restaurant = restaurants[8]
            },

            new()
            {
                Id = 12,
                Name = "Лимонад",
                WeightInGrams = 500,
                Price = 230,

                CategoryId = 7,
                Category = categories[6],

                RestaurantId = 10,
                Restaurant = restaurants[9]
            }
        };


    }

    /// <summary>
    /// Создаёт список заказов
    /// </summary>
    /// <returns>Список заказов</returns>
    private static List<Order> CreateOrders(List<Client> clients, List<Restaurant> restaurants)
    {
        return new List<Order>
        {
            new()
            {
                Id = 1,

                ClientId = 1,
                Client = clients[0],

                RestaurantId = 1,
                Restaurant = restaurants[0],

                CreatedAt = new DateTimeOffset(2026, 9, 1, 12, 0, 0, TimeSpan.Zero),

                DeliveredAt = new DateTimeOffset(2026, 9, 1, 12, 30, 0, TimeSpan.Zero),

                TotalAmount = 1200
            },

            new()
            {
                Id = 2,

                ClientId = 2,
                Client = clients[1],

                RestaurantId = 1,
                Restaurant = restaurants[0],

                CreatedAt = new DateTimeOffset(2026, 9, 2, 13, 0, 0, TimeSpan.Zero),

                DeliveredAt = new DateTimeOffset(2026, 9, 2, 13, 25, 0, TimeSpan.Zero),

                TotalAmount = 900
            },

            new()
            {
                Id = 3,

                ClientId = 3,
                Client = clients[2],

                RestaurantId = 1,
                Restaurant = restaurants[0],

                CreatedAt = new DateTimeOffset(2026, 9, 3, 18, 0, 0, TimeSpan.Zero),

                DeliveredAt = new DateTimeOffset(2026, 9, 3, 18, 20, 0, TimeSpan.Zero),

                TotalAmount = 1500
            },

            new()
            {
                Id = 4,

                ClientId = 1,
                Client = clients[0],

                RestaurantId = 1,
                Restaurant = restaurants[0],

                CreatedAt = new DateTimeOffset(2026, 9, 4, 19, 0, 0, TimeSpan.Zero),

                DeliveredAt = new DateTimeOffset(2026, 9, 4, 19, 20, 0, TimeSpan.Zero),

                TotalAmount = 1800
            },


            new()
            {
                Id = 5,

                ClientId = 4,
                Client = clients[3],

                RestaurantId = 2,
                Restaurant = restaurants[1],

                CreatedAt = new DateTimeOffset(2026, 9, 5, 12, 0, 0, TimeSpan.Zero),

                DeliveredAt = new DateTimeOffset(2026, 9, 5, 12, 40, 0, TimeSpan.Zero),

                TotalAmount = 1100
            },


            new()
            {
                Id = 6,

                ClientId = 5,
                Client = clients[4],

                RestaurantId = 2,
                Restaurant = restaurants[1],

                CreatedAt = new DateTimeOffset(2026, 9, 6, 14, 0, 0, TimeSpan.Zero),

                DeliveredAt = new DateTimeOffset(2026, 9, 6, 14, 35, 0, TimeSpan.Zero),

                TotalAmount = 1400
            },


            new()
            {
                Id = 7,

                ClientId = 1,
                Client = clients[0],

                RestaurantId = 2,
                Restaurant = restaurants[1],

                CreatedAt = new DateTimeOffset(2026, 9, 7, 18, 0, 0, TimeSpan.Zero),

                DeliveredAt = new DateTimeOffset(2026, 9, 7, 18, 45, 0, TimeSpan.Zero),

                TotalAmount = 2000
            },


            new()
            {
                Id = 8,

                ClientId = 6,
                Client = clients[5],

                RestaurantId = 3,
                Restaurant = restaurants[2],

                CreatedAt = new DateTimeOffset(2026, 9, 8, 16, 0, 0, TimeSpan.Zero),

                DeliveredAt = new DateTimeOffset(2026, 9, 8, 16, 15, 0, TimeSpan.Zero),

                TotalAmount = 750
            },


            new()
            {
                Id = 9,

                ClientId = 7,
                Client = clients[6],

                RestaurantId = 3,
                Restaurant = restaurants[2],

                CreatedAt = new DateTimeOffset(2026, 9, 9, 16, 0, 0, TimeSpan.Zero),

                DeliveredAt = new DateTimeOffset(2026, 9, 9, 16, 30, 0, TimeSpan.Zero),

                TotalAmount = 800
            },


            new()
            {
                Id = 10,

                ClientId = 1,
                Client = clients[0],

                RestaurantId = 3,
                Restaurant = restaurants[2],

                CreatedAt = new DateTimeOffset(2026, 9, 10, 16, 0, 0, TimeSpan.Zero),

                DeliveredAt = new DateTimeOffset(2026, 9, 10, 16, 15, 0, TimeSpan.Zero),

                TotalAmount = 2300
            },

            new()
            {
                Id = 11,

                ClientId = 8,
                Client = clients[7],

                RestaurantId = 4,
                Restaurant = restaurants[3],

                CreatedAt = new DateTimeOffset(2026, 9, 11, 12, 0, 0, TimeSpan.Zero),

                DeliveredAt = new DateTimeOffset(2026, 9, 11, 12, 25, 0, TimeSpan.Zero),

                TotalAmount = 950
            },

            new()
            {
                Id = 12,

                ClientId = 9,
                Client = clients[8],

                RestaurantId = 5,
                Restaurant = restaurants[4],

                CreatedAt = new DateTimeOffset(2026, 9, 12, 13, 0, 0, TimeSpan.Zero),

                DeliveredAt = new DateTimeOffset(2026, 9, 12, 13, 40, 0, TimeSpan.Zero),

                TotalAmount = 700
            }

        };
    }
}