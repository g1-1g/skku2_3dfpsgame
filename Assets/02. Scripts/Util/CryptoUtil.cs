using System;
using System.Security.Cryptography;
using System.Text;

public static class CryptoUtil
{
    private static readonly string SECRET = "MySuperSecretKey"; // 아무 문자열 가능

    private static byte[] GetKey()
    {
        using (SHA256 sha = SHA256.Create())
        {
            return sha.ComputeHash(Encoding.UTF8.GetBytes(SECRET)); // 32 bytes
        }
    }

    private static byte[] GetIV()
    {
        return Encoding.UTF8.GetBytes("1234567890123456"); // 16 bytes 고정
    }

    public static string Encrypt(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = GetKey();
            aes.IV = GetIV();

            var encryptor = aes.CreateEncryptor();
            byte[] input = Encoding.UTF8.GetBytes(plainText);
            byte[] encrypted = encryptor.TransformFinalBlock(input, 0, input.Length);

            return Convert.ToBase64String(encrypted);
        }
    }

    public static string Decrypt(string cipherText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = GetKey();
            aes.IV = GetIV();

            var decryptor = aes.CreateDecryptor();
            byte[] input = Convert.FromBase64String(cipherText);
            byte[] decrypted = decryptor.TransformFinalBlock(input, 0, input.Length);

            return Encoding.UTF8.GetString(decrypted);
        }
    }
}