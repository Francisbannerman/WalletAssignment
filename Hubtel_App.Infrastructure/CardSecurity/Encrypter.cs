using System.Security.Cryptography;

namespace Hubtel_App.Infrastructure.CardSecurity;

public class Encrypter : IEncrypter
{
    private readonly string Salt = "2iZEXoAbLvlNiiICEtw3nulIKuABd76.zwhR.ec1-OLk.=9#@_";
    
    public string GetHash(string value, string salt)
    {
        var derivedBytes = new Rfc2898DeriveBytes(value, GetBytes(salt), 1000);
        return Convert.ToBase64String(derivedBytes.GetBytes(50));
    }
    
    public string GetSalt()
    {
        return Salt;
    }

    private static byte[] GetBytes(string value)
    {
        var bytes = new Byte[value.Length];
        Buffer.BlockCopy(value.ToCharArray(), 0, bytes, 0, bytes.Length);
        return bytes;
    }
}