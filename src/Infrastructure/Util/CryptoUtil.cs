using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Infrastructure.Util;
public static class CryptoUtil
{
    public static string CreateHash(string? data = null)
    {
        using (SHA256 sha = SHA256.Create())
        {
            string str2 = data == null ? Encoding.UTF8.GetString(GenerateRandomByte()) : data!;
            for (int i = 0; i < 200; i++)
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(str2));
                StringBuilder str = new StringBuilder();
                foreach (byte b in bytes)
                {
                    str.Append(b.ToString("x2"));
                }
                str2 = str.ToString();
            }
            return str2;
        }
    }

    public static string CreateSalt()
    {
        byte[] randomBytes = new byte[16];
        using (var generator = RandomNumberGenerator.Create())
        {
            generator.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }

    public static byte[] GenerateRandomByte(int size = 32)
    {
        using (RandomNumberGenerator provider = RandomNumberGenerator.Create())
        {
            byte[] byteArr = new byte[size];
            provider.GetBytes(byteArr);
            return byteArr;
        }
    }
}
