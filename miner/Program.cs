using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

class Program
{
    static HttpClient client = new HttpClient();

    static async Task Main(string[] args)
    {
        string base_url = "https://gabserver.eu";

        while (true)
        {
            Console.WriteLine("\nWhat do you want to do?\n" +
                           "1: Check Balance\n" +
                           "2: Change Password\n" +
                           "3: Create Transaction\n" +
                           "4: Signup\n" +
                           "5: Delete Account\n" +
                           "6: List Transactions\n" +
                           "7: List Users\n" +
                           "q: Quit\n");

            string user_input = AcceptUserInput("Enter option: ");

            if (user_input == "1")
            {
                string url = base_url + "/v1/balance";

                string username = AcceptUserInput("Enter username: ");

                var jsonContent = JsonContent.Create(new
                {
                    username = username,
                });

                await PostMessageAsync(url, jsonContent);
            }
            else if(user_input == "2"){
                string url = base_url + "/v1/changepass";

                string username = AcceptUserInput("Enter username: ");
                string password = AcceptUserInput("Enter password: ");
                string new_password = AcceptUserInput("Enter new password: ");

                var jsonContent = JsonContent.Create(new
                {
                    username = username,
                    password = password,
                    new_password = new_password,
                });

                await PostMessageAsync(url, jsonContent);
            }
            else if(user_input == "3"){
                string url = base_url + "/v1/create_transaction";

                string sender = AcceptUserInput("Enter sender username: ");
                string receiver = AcceptUserInput("Enter receiver username: ");
                int amount = Convert.ToInt32(AcceptUserInput("Enter amount: "));
                string note = AcceptUserInput("Enter note (optional): ");

                var jsonContent = JsonContent.Create(new
                {
                    sender_username = sender,
                    receiver_username = receiver,
                    amount = amount,
                    note = note,
                });

                await PostMessageAsync(url, jsonContent);
            }
            else if(user_input == "4"){
                string url = base_url + "/v1/signup";

                string username = AcceptUserInput("Enter username: ");
                string password = AcceptUserInput("Enter password: ");

                var jsonContent = JsonContent.Create(new
                {
                    username = username,
                    password = password
                });

                await PostMessageAsync(url, jsonContent);
            }
            else if(user_input == "5"){
                string url = base_url + "/v1/delete";

                string username = AcceptUserInput("Enter username: ");
                string password = AcceptUserInput("Enter password: ");

                var jsonContent = JsonContent.Create(new
                {
                    username = username,
                    password = password
                });

                await PostMessageAsync(url, jsonContent);
            }
            else if(user_input == "6")
            {
                string url = base_url + "/v1/listt";
                await GetMessageAsync(url);
            }
            else if(user_input == "7")
            {
                string url = base_url + "/v1/listu";
                await GetMessageAsync(url);
            }
            else if(user_input == "q")
            {
                Console.WriteLine("Exiting...");
                break;
            }
            else
            {
                Console.WriteLine("Invalid input");
            }
        }
    }

    static async Task<HttpResponseMessage> GetMessageAsync(string url)
    {
        Console.WriteLine("URL: " + url);
        HttpResponseMessage response = await client.GetAsync(url);

        string content = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Response Code: " + response.StatusCode);
        Console.WriteLine("Content: " + content);

        return response;
    }

    static async Task<HttpResponseMessage> PostMessageAsync(string url, JsonContent? jsoncontent)
    {
        Console.WriteLine("URL: " + url);
        HttpResponseMessage response = await client.PostAsync(url, jsoncontent);

        string content = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Response Code: " + response.StatusCode);
        Console.WriteLine("Content: " + content);

        return response;
    }

    static string AcceptUserInput(string prompt)
    {
        Console.WriteLine(prompt + "");
        string response = Console.ReadLine() + "";

        return response;
    }
}
