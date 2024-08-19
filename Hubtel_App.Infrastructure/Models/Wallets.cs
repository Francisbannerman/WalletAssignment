using System.Diagnostics;

namespace Hubtel_App.Infrastructure.Models;

public class Wallets
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string AccountNumber { get; set; }
    public string AccountScheme { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string Owner { get; set; }
}