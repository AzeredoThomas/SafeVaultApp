using System.Security.Cryptography;
using System.Text;

public class EncryptionService
{
    private readonly byte[] _key = Encoding.UTF8.GetBytes("superhemligkrypteringsnyckel1234"); // 32 bytes för AES-256

    public string Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.GenerateIV();

        var encryptor = aes.CreateEncryptor();
        var plainBytes = Encoding.UTF8.GetBytes(plainText);
        var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        var result = Convert.ToBase64String(aes.IV.Concat(encryptedBytes).ToArray());
        return result;
    }

    public string Decrypt(string encryptedText)
    {
        var fullBytes = Convert.FromBase64String(encryptedText);
        var iv = fullBytes.Take(16).ToArray();
        var cipherText = fullBytes.Skip(16).ToArray();

        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = iv;

        var decryptor = aes.CreateDecryptor();
        var decryptedBytes = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
        return Encoding.UTF8.GetString(decryptedBytes);
    }
}
