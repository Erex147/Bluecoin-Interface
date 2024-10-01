// Copyright (C) 2024  Erex147
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
namespace miner;

public class Miner
{
    public bool mining;
    public string hashValue = "";
    public int threadid;
    public long validNonce;

    public string username;
    public string password;

    static string getHashUrl = Program.base_url + "/v1/userhashget";
    static string setHashUrl = Program.base_url + "/v1/userhashset";
    static string targetPrefix = "00000";
    static long batchSize = 9007199254740991;

    // public static async Task Mine(string username, string password)
    // {
    //     long nonce = 1; // Start at 1 since it's the only valid format
    //     long batchSize = 9007199254740991; // Process this many hashes at a time for efficiency
    //     string targetPrefix = "00000";

    //     var jsonContent = JsonContent.Create(new
    //     {
    //         username = username,
    //         password = password,
    //         threadid = 1
    //     });

    //     dynamic data = await Utility.ProcessHttpResponse(await Utility.PostMessageAsync(getHashUrl, jsonContent));
    //     if (data.error != null)
    //     {
    //         Console.WriteLine("\nError: " + data.error);
    //     }
    //     else
    //     {
    //         hashValue = data.hash;
    //     }

    //     async Task doWork()
    //     {
    //         //System.Threading.Thread.Sleep(1000);
    //         if (!mining) return;
    //         for (long i = 0; i <= batchSize; i++)
    //         {
    //             string input = hashValue + nonce.ToString();
    //             string hash = ComputeSHA1(input);

    //             if (hash.StartsWith(targetPrefix) && mining == true)
    //             {
    //                  var jsonContent = JsonContent.Create(new
    //                 {
    //                     username = username,
    //                     password = password,
    //                     threadid = 1
    //                 });

    //                 dynamic data = await Utility.ProcessHttpResponse(await Utility.PostMessageAsync(setHashUrl, jsonContent));
    //                 if (data.error != null)
    //                 {
    //                     Console.WriteLine("\nError: " + data.error);
    //                 }
    //                 else
    //                 {
    //                     hashValue = data.hash;
    //                 }

    //                 return;
    //             }

    //             nonce++;
    //         }
    //     }
    //     await doWork();
    // }
    public Miner(int id, string user, string pwd)
    {
        this.threadid = id;
        this.username = user;
        this.password = pwd;
        this.mining = true;
    }

    public async Task StartMining()
    {
        await GetHashAsync();

        while (true)
        {
            for (long nonce = 0; nonce < batchSize; nonce++)
            {
                if (!this.mining) break;

                string hash = ComputeSHA1(hashValue + nonce.ToString());
                if (hash.StartsWith(targetPrefix))
                {
                    await SetHashAsync(nonce);
                    nonce = 0;
                }
            }
        }
    }

    private async Task GetHashAsync()
    {
        var jsonContent = JsonContent.Create(new {
            username = this.username,
            password = this.password,
            threadid = this.threadid,
        });

        dynamic data = await Utility.ProcessHttpResponse(await Utility.PostMessageAsync(getHashUrl, jsonContent));
        if (data.error != null)
        {
            Console.WriteLine("\nError: " + data.error);
        }
        else
        {
            this.hashValue = data.hash;
        }
    }

    private async Task SetHashAsync(long nonce)
    {
        var jsonContent = JsonContent.Create(new {
            username = this.username,
            password = this.password,
            nonce = nonce,
            threadid = this.threadid,
        });

        if (Program.debug_enabled == true)
        {
            Console.WriteLine(nonce);
        }

        dynamic data = await Utility.ProcessHttpResponse(await Utility.PostMessageAsync(setHashUrl, jsonContent));
        if (data.error != null)
        {
            Console.WriteLine("\nError: " + data.error);
        }
        else
        {
            this.hashValue = data.newhash;
        }
    }

    private static string ComputeSHA1(string input)
    {
        using SHA1 sha1 = SHA1.Create();
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        byte[] hashBytes = sha1.ComputeHash(inputBytes);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }
}
