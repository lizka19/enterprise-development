namespace EnterpriseDevelopment.Domain.Entities;

public class Client
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string DeliveryAddress { get; set; } = string.Empty;
}
