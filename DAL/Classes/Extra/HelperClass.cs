using System.Security.Cryptography;
using System.Text;

namespace DAL.Classes.Extra;

public static class HelperClass
{
    private const string tempKey = "Cit$$50/*Sed#dN#_JeX!!X0$c145^2@@@!!eCaeNaappa";
    public static string HashMd5(string pass)
    {
        var sha1 = MD5.Create();
        var step1 = Encoding.UTF8.GetBytes(pass + tempKey);
        var step2 = sha1.ComputeHash(step1);
        return string.Join("", step2.Select(x => x.ToString("X2"))).ToLower();
    }
}