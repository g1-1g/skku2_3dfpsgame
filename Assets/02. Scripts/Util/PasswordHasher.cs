using System;
using System.Security.Cryptography;
using System.Text;


public static class PasswordHasher
{
    // salt 생성
    public static string GenerateSalt(int size = 16)
    {
        byte[] saltBytes = new byte[size];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }
        return Convert.ToBase64String(saltBytes);
    }

    // 비밀번호 해싱
    public static string HashPassword(string password, string salt)
    {
        using (SHA256 sha = SHA256.Create())
        {
            string combined = password + salt;
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(combined));
            return Convert.ToBase64String(hash);
        }
    }

    // 로그인 검증
    public static bool VerifyPassword(string inputPassword, string storedHash, string storedSalt)
    {
        string inputHash = HashPassword(inputPassword, storedSalt);
        return inputHash == storedHash;
    }
}