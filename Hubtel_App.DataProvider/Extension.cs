using Hubtel_App.Infrastructure.CardSecurity;
using Hubtel_App.Infrastructure.Models;

namespace Hubtel_App.DataProvider;

public static class Extension
{
    public static Wallets SetCardNumber(this Wallets wallet, IEncrypter encrypter)
    {
        var salt = encrypter.GetSalt();
        
        string firstSixDigitsOfCardNumber = wallet.AccountNumber.Substring(0, 6);
        string lastTenDigitsOfCardNumber = wallet.AccountNumber.Substring(wallet.AccountNumber.Length - 10);
        var hashedSectionOfCardNumber = encrypter.GetHash(lastTenDigitsOfCardNumber, salt);
        wallet.AccountNumber = firstSixDigitsOfCardNumber + "-" + hashedSectionOfCardNumber;
        
        return wallet;
    }

    public static string SetCardNumberForHash(this Wallets wallet, IEncrypter encrypter)
    {
        var salt = encrypter.GetSalt();
        
        string firstSixDigitsOfCardNumber = wallet.AccountNumber.Substring(0, 6);
        string lastTenDigitsOfCardNumber = wallet.AccountNumber.Substring(wallet.AccountNumber.Length - 10);
        var hashedSectionOfCardNumber = encrypter.GetHash(lastTenDigitsOfCardNumber, salt);
        wallet.AccountNumber = firstSixDigitsOfCardNumber + "-" + hashedSectionOfCardNumber;

        return wallet.AccountNumber;
    }

    public static bool ValidateCardNumber(this Wallets wallet, string cardNumber, IEncrypter encrypter)
    {
        string salt = encrypter.GetSalt();
        string hashedCardNumber = encrypter.GetHash(cardNumber, salt);
        return wallet.AccountNumber.Equals(hashedCardNumber);
    }
}