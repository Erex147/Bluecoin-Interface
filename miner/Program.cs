// Copyright (C) 2024  Erex147
using System.Net.Http.Json;
using Newtonsoft.Json;

class Program
{
    static HttpClient client = new HttpClient();
    static bool debug_enabled = false;

    static async Task Main(string[] args)
    {
        string base_url = "https://gabserver.eu";

        if (args.Length > 0)
        {
            try
            {
                debug_enabled = Convert.ToBoolean(args[0]);
            }
            catch
            {
                Console.WriteLine("invalid argument passed");
            }
        }

        Console.WriteLine("bluc-interface  Copyright (C) 2024  Erex147");
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
                           "8: Verify User\n" +
                           "9: Verify Password\n" +
                           "q: Quit\n");

            string user_input = PromptUser("Enter option: ");

            if (user_input == "1")
            {
                // check user balance
                try
                {
                    string url = base_url + "/v1/balance";

                    string username = PromptUser("Enter username: ");

                    var jsonContent = JsonContent.Create(new
                    {
                        username = username,
                    });

                    dynamic data = await ProcessHttpResponse(await PostMessageAsync(url, jsonContent));
                    if (data.error != null)
                    {
                        Console.WriteLine("Error: " + data.error);
                    }
                    else
                    {
                        Console.WriteLine("\nMessage: " + data.message);
                        Console.WriteLine("Balance: " + data.balance);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                PromptUser("\nPress enter to continue..");
            }
            else if(user_input == "2")
            {
                // change user's password
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

                    dynamic data = await ProcessHttpResponse(await PostMessageAsync(url, jsonContent));
                    if (data.error != null)
                    {
                        Console.WriteLine("Error: " + data.error);
                    }
                    else
                    {
                        Console.WriteLine("\nMessage: " + data.message);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                PromptUser("Press enter to continue..");
            }
            else if(user_input == "3")
            {
                // make a transaction
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

                    dynamic data = await ProcessHttpResponse(await PostMessageAsync(url, jsonContent));
                    if (data.error != null)
                    {
                        Console.WriteLine("Error: " + data.error);
                    }
                    else
                    {
                        Console.WriteLine("\nMessage: " + data.message);
                    }
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
                // create account
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

                    dynamic data = await ProcessHttpResponse(await PostMessageAsync(url, jsonContent));
                    if (data.error != null)
                    {
                        Console.WriteLine("Error: " + data.error);
                    }
                    else
                    {
                        Console.WriteLine("\nMessage: " + data.message);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                PromptUser("Press enter to continue..");
            }
            else if(user_input == "5")
            {
                // delete account
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

                    dynamic data = await ProcessHttpResponse(await PostMessageAsync(url, jsonContent));
                    if (data.error != null)
                    {
                        Console.WriteLine("Error: " + data.error);
                    }
                    else
                    {
                        Console.WriteLine("\nMessage: " + data.message);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                PromptUser("Press enter to continue..");
            }
            else if(user_input == "6")
            {
                // list all transactions
                string url = base_url + "/v1/listt";

                List<dynamic> data = await ProcessHttpResponseList(await GetMessageAsync(url));

                foreach (var transaction in data)
                {
                    Console.WriteLine("Sender: " + transaction.sender);
                    Console.WriteLine("Receiver: " + transaction.receiver);
                    Console.WriteLine("Amount: " + transaction.amount);
                    Console.WriteLine("Note: " + transaction.note);

                    if (debug_enabled)
                    {
                        Console.WriteLine("ID: " + transaction.id);
                        Console.WriteLine("timestamp: " + transaction.date);
                    }

                    Console.WriteLine("");
                }

                PromptUser("Press enter to continue..");
            }
            else if(user_input == "7")
            {
                // list users
                string url = base_url + "/v1/listu";

                List<dynamic> data = await ProcessHttpResponseList(await GetMessageAsync(url));

                foreach (var userdata in data)
                {
                    Console.WriteLine("Username: " + userdata.username);
                    Console.WriteLine("Balance: " + userdata.balance + "\n");
                }

                PromptUser("Press enter to continue..");
            }
            else if(user_input == "8")
            {
                try
                {
                    // verify if user exists
                    string url = base_url + "/v1/exists";

                    string username = PromptUser("Enter username: ");
                    var jsonContent = JsonContent.Create(new
                    {
                        username = username,
                    });

                    dynamic data = await ProcessHttpResponse(await PostMessageAsync(url, jsonContent));
                    if (data.error != null)
                    {
                        Console.WriteLine("Error: " + data.error);
                    }
                    else
                    {
                        Console.WriteLine("\nMessage: " + data.message);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                PromptUser("Press enter to continue..");
            }
            else if(user_input == "9")
            {
                // verify password
                try
                {
                    string url = base_url + "/v1/verify";

                    string username = PromptUser("Enter username: ");
                    string password = PromptUser("Enter password: ");

                    var jsonContent = JsonContent.Create(new
                    {
                        username = username,
                        password = password
                    });

                    dynamic data = await ProcessHttpResponse(await PostMessageAsync(url, jsonContent));
                    if (data.error != null)
                    {
                        Console.WriteLine("Error: " + data.error);
                    }
                    else
                    {
                        Console.WriteLine("\nMessage: " + data.message);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
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

    static async Task<HttpResponseMessage> GetMessageAsync(string url) // send a http(s) get request to the endpoint
    {
        HttpResponseMessage response = await client.GetAsync(url);

        if (debug_enabled)
        {
            string content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("URL: " + url);
            Console.WriteLine("Response Code: " + response.StatusCode);
            Console.WriteLine("\nContent: " + content);
        }

        return response;
    }

    static async Task<HttpResponseMessage> PostMessageAsync(string url, JsonContent? jsoncontent) // send a http(s) post request to the endpoint
    {
        HttpResponseMessage response = await client.PostAsync(url, jsoncontent);
    
        if (debug_enabled)
        {
            string content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("URL: " + url);
            Console.WriteLine("Response Code: " + response.StatusCode);
            Console.WriteLine("\nContent: " + content);
        }

        return response;
    }

    static async Task<dynamic> ProcessHttpResponse(HttpResponseMessage responseMessage) //process http(s) reponses from post/get requests
    {
        string content = await responseMessage.Content.ReadAsStringAsync();
        dynamic data = JsonConvert.DeserializeObject(content);
        
        return data;
    }

    static async Task<List<dynamic>> ProcessHttpResponseList(HttpResponseMessage responseMessage) //process http(s) reponses from post/get requests but as a list
    {
        string content = await responseMessage.Content.ReadAsStringAsync();
        List<dynamic> data = JsonConvert.DeserializeObject<List<dynamic>>(content);
        
        return data;
    }

    static string PromptUser(string prompt) // accept user input
    {
        Console.WriteLine(prompt + "");
        string response = Console.ReadLine() + "";

        return response;
    }
}
