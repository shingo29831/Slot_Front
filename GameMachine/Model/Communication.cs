using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace http_test_client
{

    class Sever_Communication
    {
        public static async void Post(string url, string request)
        {
            // HttpClientHandlerを作成し、SSL証明書の検証を無効にする
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            try
            {
                using (var client = new HttpClient(handler))
                {
                    // POSTリクエスト
                    var content = new StringContent(request, Encoding.UTF8, "application/json");
                    var res = await client.PostAsync(url, content);

                    // 取得
                    var _response = await res.Content.ReadAsStringAsync();

                    Deseriarize_Data deseriarize_Data = JsonSerializer.Deserialize<Deserize_Data>(_response);

                    Console.WriteLine(_response);
                    Console.WriteLine($"Property1: {responseData.Property1}, Property2: {responseData.Property2}");

                }
            }
            catch ()
            {
                Console.WriteLine("通信が正常に終了しませんでした");
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

            Sever_Communication.Post(BASE_URL + "/user_Login", formatdata);

        }

        public static string Post_Logout(string username, string table_id)
        {
            string formatdata = "{" +
                $" \"username\" : \"{username}\" ," +
                $" \"table\" : \"{table_id}\" " +
                "}";

            Sever_Communication.Post(BASE_URL + "/user_Logout", formatdata);

        }

        public static string Post_Get_Money(string username, string password, string token, string table_id)
        {
            string formatdata = "{" +
                $" \"Key\" : \"aaa\" ," +
                $" \"username\" : \"{username}\" ," +
                $" \"password\" : \"{password}\" ," +
                $" \"token\" : \"{token}\" ," +
                $" \"table\" : \"{table_id}\" ," +
                $" \"money\" : null" +
                "}";

            Sever_Communication.Post(BASE_URL + "/get_user_money", formatdata);

        }


    }
}
