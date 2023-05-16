using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace CheckoutGateway.BusinessLogic.Services;

public class EncryptedRequestData
{
    public string EncryptedData { get; set; }
    public string Iv { get; set; }
}
public static class EncryptionHelper
{
    public static EncryptedRequestData EncryptRequest(dynamic requestData, string encryptionKey)
    {
        // Serialize the request data to JSON
        var json = JsonConvert.SerializeObject(requestData);

        // Encrypt the JSON string using AES encryption
        var secretKey = encryptionKey;
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(secretKey);
        aes.GenerateIV();
        var iv = Convert.ToBase64String(aes.IV);
        using var encryptor = aes.CreateEncryptor();
        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        using var writer = new StreamWriter(cryptoStream);
        writer.Write(json);
        writer.Flush();
        cryptoStream.FlushFinalBlock();
        var encryptedRequest = Convert.ToBase64String(memoryStream.ToArray());

        // Create an EncryptedRequestData object to send in the request body
        var encryptedRequestData = new EncryptedRequestData
        {
            EncryptedData = encryptedRequest,
            Iv = iv
        };

        // Serialize the EncryptedRequestData object to JSON and return the JSON string
        //var encryptedJson = JsonConvert.SerializeObject(encryptedRequestData);
        return encryptedRequestData;
    }

    public static string DecryptRequest(string encryptedRequest, string iv, string encryptionKey)
    {
        // Decrypt the request using AES encryption and the IV
        var secretKey = encryptionKey;
        var bytes = Convert.FromBase64String(encryptedRequest);
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(secretKey);
        aes.IV = Convert.FromBase64String(iv);
        using var decryptor = aes.CreateDecryptor();
        using var memoryStream = new MemoryStream(bytes);
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var reader = new StreamReader(cryptoStream);
        var decryptedRequest = reader.ReadToEnd();
        return decryptedRequest;
    }
}
