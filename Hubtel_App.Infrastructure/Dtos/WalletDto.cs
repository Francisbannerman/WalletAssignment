namespace Hubtel_App.Infrastructure.Dtos;

public class WalletDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string AccountNumber { get; set; }
    public string AccountScheme { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string Owner { get; set; }
}