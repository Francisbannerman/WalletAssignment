namespace Hubtel_App.Infrastructure.CardSecurity;

public interface IEncrypter
{
    string GetSalt();
    string GetHash(string value, string salt);
}