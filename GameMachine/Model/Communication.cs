using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Model;

class Sever_Communication
{
public static async Task<Deserialize_Data> Post(string url, string request)
{
    // HttpClientHandlerを作成し、SSL証明書の検証を無効にする
    var handler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
    };

        using (var client = new HttpClient(handler))
        {
            // POSTリクエスト
            var content = new StringContent(request, Encoding.UTF8, "application/json");
            var res = await client.PostAsync(url, content);

            // 取得
            var _response = await res.Content.ReadAsStringAsync();

            Deserialize_Data? deseriarize_Data = JsonSerializer.Deserialize<Deserialize_Data>(_response);

            Console.WriteLine(_response);

            return deseriarize_Data;
        }

    }
}

class Client_Communication
{

    public static string BASE_URL = "https://localhost:8443";

    public static string Post_Login(string username, string password)
    {
        string formatdata = "{" +
            $" \"Key\" : \"aaa\" ," +
            $" \"username\" : \"{username}\" ," +
            $" \"password\" : \"{password}\" ," +
            $" \"token\" : \"\" ," +
            $" \"table\" : \"table_120\" ," +
            $" \"money\" : null" +
            "}";

        var result = Sever_Communication.Post(BASE_URL + "/user_Login", formatdata);

        return $"{result.Result.Token}";
    }

    public static string Post_Logout(string username, string table_id)
    {
        string formatdata = "{" +
            $" \"username\" : \"{username}\" ," +
            $" \"table\" : \"{table_id}\" " +
            "}";

        var result = Sever_Communication.Post(BASE_URL + "/user_Logout", formatdata);

        return $"{result.Result.Result}";
    }

    public static int Post_Get_Money(string username, string password, string token, string table_id)
    {
        string formatdata = "{" +
            $" \"Key\" : \"aaa\" ," +
            $" \"username\" : \"{username}\" ," +
            $" \"password\" : \"{password}\" ," +
            $" \"token\" : \"{token}\" ," +
            $" \"table\" : \"{table_id}\" ," +
            $" \"money\" : null" +
            "}";

        var result = Sever_Communication.Post(BASE_URL + "/get_user_money", formatdata);

        return (int)result.Result.Money;
    }
}
