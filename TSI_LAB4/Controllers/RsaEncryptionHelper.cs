using System.Security.Cryptography;
using System.Text;

public static class RsaEncryptionHelper
{
    private static RSA rsa;

    public static void AssignNewKey()
    {
        rsa = RSA.Create(2048); // This creates a new 2048-bit key pair.
    }

    public static string GetPublicKey()
    {
        return rsa.ToXmlString(false); // Export the public key
    }

    public static string GetPrivateKey()
    {
        return rsa.ToXmlString(true); // Export the private key
    }

    public static string Encrypt(string dataToEncrypt, string publicKeyXml)
    {
        using (var rsaEncryptor = RSA.Create())
        {
            rsaEncryptor.FromXmlString(publicKeyXml);
            var encryptedData = rsaEncryptor.Encrypt(Encoding.UTF8.GetBytes(dataToEncrypt), RSAEncryptionPadding.OaepSHA1);
            return Convert.ToBase64String(encryptedData);
        }
    }

    public static string Decrypt(string dataToDecrypt, string privateKeyXml)
    {
        using (var rsaDecryptor = RSA.Create())
        {
            rsaDecryptor.FromXmlString(privateKeyXml);
            var decryptedData = rsaDecryptor.Decrypt(Convert.FromBase64String(dataToDecrypt), RSAEncryptionPadding.OaepSHA1);
            return Encoding.UTF8.GetString(decryptedData);
        }
    }
}
