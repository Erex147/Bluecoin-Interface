// Copyright (C) 2024  Erex147
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

using miner;

class Program
{
    public static HttpClient client = new HttpClient();
    public static bool debug_enabled = false;
    static string base_url = "https://gabserver.eu";
    static string username = "";
    static string password = "";

    static async Task Main(string[] args)
    {
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
        Miner.NonceFound += async(sender, args) => await Miner_NonceFound(sender, args);

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
                           "10: Toggle miner\n" +
                           "q: Quit\n");

            string user_input = Utility.PromptUser("Enter option: ");

            if (user_input == "1")
            {
                // check user balance
                try
                {
                    string url = base_url + "/v1/balance";

                    string username = Utility.PromptUser("Enter username: ");

                    var jsonContent = JsonContent.Create(new
                    {
                        username = username,
                    });

                    dynamic data = await Utility.ProcessHttpResponse(await Utility.PostMessageAsync(url, jsonContent));
                    if (data.error != null)
                    {
                        Console.WriteLine("\nError: " + data.error);
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
                Utility.PromptUser("\nPress enter to continue..");
            }
            else if(user_input == "2")
            {
                // change user's password
                try
                {
                    string url = base_url + "/v1/changepass";

                    string username = Utility.PromptUser("Enter username: ");
                    string password = Utility.PromptUser("Enter password: ");
                    string new_password = Utility.PromptUser("Enter new password: ");

                    var jsonContent = JsonContent.Create(new
                    {
                        username = username,
                        password = password,
                        new_password = new_password,
                    });

                    dynamic data = await Utility.ProcessHttpResponse(await Utility.PostMessageAsync(url, jsonContent));
                    if (data.error != null)
                    {
                        Console.WriteLine("\nError: " + data.error);
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
                Utility.PromptUser("\nPress enter to continue..");
            }
            else if(user_input == "3")
            {
                // make a transaction
                try
                {
                    string url = base_url + "/v1/create_transaction";

                    string sender = Utility.PromptUser("Enter sender username: ");
                    string password = Utility.PromptUser("Enter password: ");
                    string receiver = Utility.PromptUser("Enter receiver username: ");
                    int amount = Convert.ToInt32(Utility.PromptUser("Enter amount: "));
                    string note = Utility.PromptUser("Enter note (optional): ");

                    var jsonContent = JsonContent.Create(new
                    {
                        sender_username = sender,
                        password = password,
                        receiver_username = receiver,
                        amount = amount,
                        note = note,
                    });

                    dynamic data = await Utility.ProcessHttpResponse(await Utility.PostMessageAsync(url, jsonContent));
                    if (data.error != null)
                    {
                        Console.WriteLine("\nError: " + data.error);
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
                Utility.PromptUser("\nPress enter to continue..");
            }
            else if(user_input == "4")
            {
                // create account
                try
                {
                    string url = base_url + "/v1/signup";

                    string username = Utility.PromptUser("Enter username: ");
                    string password = Utility.PromptUser("Enter password: ");

                    var jsonContent = JsonContent.Create(new
                    {
                        username = username,
                        password = password
                    });

                    dynamic data = await Utility.ProcessHttpResponse(await Utility.PostMessageAsync(url, jsonContent));
                    if (data.error != null)
                    {
                        Console.WriteLine("\nError: " + data.error);
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
                Utility.PromptUser("\nPress enter to continue..");
            }
            else if(user_input == "5")
            {
                // delete account
                try
                {
                    string url = base_url + "/v1/delete";

                    string username = Utility.PromptUser("Enter username: ");
                    string password = Utility.PromptUser("Enter password: ");

                    var jsonContent = JsonContent.Create(new
                    {
                        username = username,
                        password = password
                    });

                    dynamic data = await Utility.ProcessHttpResponse(await Utility.PostMessageAsync(url, jsonContent));
                    if (data.error != null)
                    {
                        Console.WriteLine("\nError: " + data.error);
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
                Utility.PromptUser("\nPress enter to continue..");
            }
            else if(user_input == "6")
            {
                // list all transactions
                try
                {
                    string url = base_url + "/v1/listt";

                    List<dynamic> data = await Utility.ProcessHttpResponseList(await Utility.GetMessageAsync(url));

                    foreach (var transaction in data)
                    {
                        Console.WriteLine("\nSender: " + transaction.sender);
                        Console.WriteLine("Receiver: " + transaction.receiver);
                        Console.WriteLine("Amount: " + transaction.amount);
                        Console.WriteLine("Note: " + transaction.note);

                        if (debug_enabled)
                        {
                            Console.WriteLine("ID: " + transaction.id);
                            Console.WriteLine("timestamp: " + transaction.date);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Utility.PromptUser("\nPress enter to continue..");
            }
            else if(user_input == "7")
            {
                // list users
                try
                {
                    string url = base_url + "/v1/listu";

                    List<dynamic> data = await Utility.ProcessHttpResponseList(await Utility.GetMessageAsync(url));

                    foreach (var userdata in data)
                    {
                        Console.WriteLine("\nUsername: " + userdata.username);
                        Console.WriteLine("Balance: " + userdata.balance);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Utility.PromptUser("\nPress enter to continue..");
            }
            else if(user_input == "8")
            {
                try
                {
                    // verify if user exists
                    string url = base_url + "/v1/exists";

                    string username = Utility.PromptUser("Enter username: ");
                    var jsonContent = JsonContent.Create(new
                    {
                        username = username,
                    });

                    dynamic data = await Utility.ProcessHttpResponse(await Utility.PostMessageAsync(url, jsonContent));
                    if (data.error != null)
                    {
                        Console.WriteLine("\nError: " + data.error);
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
                Utility.PromptUser("\nPress enter to continue..");
            }
            else if(user_input == "9")
            {
                // verify password
                try
                {
                    string url = base_url + "/v1/verify";

                    string username = Utility.PromptUser("Enter username: ");
                    string password = Utility.PromptUser("Enter password: ");

                    var jsonContent = JsonContent.Create(new
                    {
                        username = username,
                        password = password
                    });

                    dynamic data = await Utility.ProcessHttpResponse(await Utility.PostMessageAsync(url, jsonContent));
                    if (data.error != null)
                    {
                        Console.WriteLine("\nError: " + data.error);
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
                Utility.PromptUser("\nPress enter to continue..");
            }
            else if(user_input == "10")
            {
                // miner
                try
                {
                    if( Miner.mining == false)
                    {
                        string username_ = Utility.PromptUser("Enter username: ");
                        string password_ = Utility.PromptUser("Enter password: ");

                        string url = base_url + "/v1/verify";

                        var jsonContent = JsonContent.Create(new
                        {
                            username = username,
                            password = password
                        });

                        dynamic data = await Utility.ProcessHttpResponse(await Utility.PostMessageAsync(url, jsonContent));
                        if (data.error != null)
                        {
                            Console.WriteLine("\nError: " + data.error);
                        }
                        else
                        {
                            username = username_;
                            password = password_;

                            Console.WriteLine("\nStarted a mining process");
                            Thread thread = new Thread(async () => await Mine());
                            thread.Start();
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nEnded a mining process");
                        Miner.mining = false;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Utility.PromptUser("\nPress enter to continue..");
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

    static async Task Mine()
    {
        string url = base_url + "/v1/userhashget";
        string hash = "";

        var jsonContent = JsonContent.Create(new
        {
            username = username,
            password = password,
            threadid = 1,
        });

        dynamic data = await Utility.ProcessHttpResponse(await Utility.PostMessageAsync(url, jsonContent));

        if (debug_enabled == true)
        {
            if (data.error != null)
            {
                Console.WriteLine("\nError: " + data.error);
            }
            else
            {
                Console.WriteLine("\nHash: " + data.hash);
                hash = data.hash;
            }
        }

        Miner.mining = true;
        Miner.hashValue = hash;
        Miner.threadid = 1;
        Miner.Mine();
    }

    static async Task Miner_NonceFound(object? sender, Miner.NonceFoundArgs args)
    {
        string url = base_url + "/v1/userhashset";

        if (debug_enabled == true)
        {
            Console.WriteLine($"Found Nonce: {args.Nonce}");
        }
        
        var jsonContent = JsonContent.Create(new
        {
            username = username,
            password = password,
            nonce = args.Nonce,
            threadid = 1,
        });

        dynamic data = await Utility.ProcessHttpResponse(await Utility.PostMessageAsync(url, jsonContent));

        if (debug_enabled == true)
        {
            if (data.error != null)
            {
                Console.WriteLine("\nError: " + data.error);
            }
        }

        await Mine();
    }
}
