using System.Security.Cryptography;
using System.Text;
using ESCenter.Application.Interfaces.Authentications;

namespace ESCenter.Infrastructure.ServiceImpls.Authentication;

public class Validator : IValidator
{
    public string GenerateValidationCode()
    {
        using RandomNumberGenerator rg = RandomNumberGenerator.Create();
        byte[] rno = new byte[5];
        rg.GetBytes(rno);
        int randomValue = BitConverter.ToInt32(rno, 0);

        return randomValue.ToString();
    }

    public string HashPassword(string input)
    {
        // Calculate MD5 hash from input
        MD5 md5 = MD5.Create();
        byte[] inputBytes = Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        // Convert byte array to hex string
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < hashBytes.Length; i++)
        {
            sb.Append(hashBytes[i].ToString("X2"));
        }

        return sb.ToString();
    }
}
