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
