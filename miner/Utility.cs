// Copyright (C) 2024  Erex147
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace miner;

public class Utility
{
    static HttpClient client = Program.client;
    static bool debug_enabled = Program.debug_enabled;

    public static async Task<HttpResponseMessage> GetMessageAsync(string url) // send a http(s) get request to the endpoint
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

    public static async Task<HttpResponseMessage> PostMessageAsync(string url, JsonContent? jsoncontent) // send a http(s) post request to the endpoint
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

    public static async Task<dynamic> ProcessHttpResponse(HttpResponseMessage responseMessage) //process http(s) reponses from post/get requests
    {
        string content = await responseMessage.Content.ReadAsStringAsync();
        dynamic data = JsonConvert.DeserializeObject(content);
        
        return data;
    }

    public static async Task<List<dynamic>> ProcessHttpResponseList(HttpResponseMessage responseMessage) //process http(s) reponses from post/get requests but as a list
    {
        string content = await responseMessage.Content.ReadAsStringAsync();
        List<dynamic> data = JsonConvert.DeserializeObject<List<dynamic>>(content);
        
        return data;
    }

    public static string PromptUser(string prompt) // accept user input
    {
        Console.WriteLine(prompt + "");
        string response = Console.ReadLine() + "";

        return response;
    }
}
