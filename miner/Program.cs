using System;
using System.Net.Http.Json;
using System.Threading.Tasks;

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

            string user_input = PromptUser("Enter option: ");

            if (user_input == "1")
            {
                try
                {
                    string url = base_url + "/v1/balance";

                    string username = PromptUser("Enter username: ");

                    var jsonContent = JsonContent.Create(new
                    {
                        username = username,
                    });

                    await PostMessageAsync(url, jsonContent);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                PromptUser("Press enter to continue..");
            }
            else if(user_input == "2")
            {
                try
                {
                    string url = base_url + "/v1/changepass";

                    string username = PromptUser("Enter username: ");
                    string password = PromptUser("Enter password: ");
                    string new_password = PromptUser("Enter new password: ");

                    var jsonContent = JsonContent.Create(new
                    {
                        username = username,
                        password = password,
                        new_password = new_password,
                    });

                    await PostMessageAsync(url, jsonContent);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                PromptUser("Press enter to continue..");
            }
            else if(user_input == "3")
            {
                try
                {
                    string url = base_url + "/v1/create_transaction";

                    string sender = PromptUser("Enter sender username: ");
                    string password = PromptUser("Enter password: ");
                    string receiver = PromptUser("Enter receiver username: ");
                    int amount = Convert.ToInt32(PromptUser("Enter amount: "));
                    string note = PromptUser("Enter note (optional): ");

                    var jsonContent = JsonContent.Create(new
                    {
                        sender_username = sender,
                        password = password,
                        receiver_username = receiver,
                        amount = amount,
                        note = note,
                    });

                    await PostMessageAsync(url, jsonContent);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("It is also possible that the amount entered was in an invalid format");
                }
                PromptUser("Press enter to continue..");
            }
            else if(user_input == "4")
            {
                try
                {
                    string url = base_url + "/v1/signup";

                    string username = PromptUser("Enter username: ");
                    string password = PromptUser("Enter password: ");

                    var jsonContent = JsonContent.Create(new
                    {
                        username = username,
                        password = password
                    });

                    await PostMessageAsync(url, jsonContent);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                PromptUser("Press enter to continue..");
            }
            else if(user_input == "5")
            {
                try
                {
                    string url = base_url + "/v1/delete";

                    string username = PromptUser("Enter username: ");
                    string password = PromptUser("Enter password: ");

                    var jsonContent = JsonContent.Create(new
                    {
                        username = username,
                        password = password
                    });

                    await PostMessageAsync(url, jsonContent);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                PromptUser("Press enter to continue..");
            }
            else if(user_input == "6")
            {
                string url = base_url + "/v1/listt";
                await GetMessageAsync(url);
                PromptUser("Press enter to continue..");
            }
            else if(user_input == "7")
            {
                string url = base_url + "/v1/listu";
                await GetMessageAsync(url);
                PromptUser("Press enter to continue..");
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

    static string PromptUser(string prompt)
    {
        Console.WriteLine(prompt + "");
        string response = Console.ReadLine() + "";

        return response;
    }
}
