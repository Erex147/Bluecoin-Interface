// Copyright (C) 2024  Erex147
using System.Security.Cryptography;
using System.Text;
namespace miner;

public class Miner
{
    public static bool mining = false;
    public static string hashValue = "";
    public static int threadid;

    public class NonceFoundArgs : EventArgs
    {
        public long Nonce { get; set; }
    }

    public static event EventHandler<NonceFoundArgs> NonceFound;

    public static void Mine()
    {
        long nonce = 1; // Start at 1 since it's the only valid format
        long batchSize = 9007199254740991; // Process this many hashes at a time for efficiency
        string targetPrefix = "00000";

        void doWork()
        {
            //System.Threading.Thread.Sleep(1000);
            if (!mining) return;
            for (long i = 0; i <= batchSize; i++)
            {
                string input = hashValue + nonce.ToString();
                string hash = ComputeSHA1(input);

                if (hash.StartsWith(targetPrefix) && mining == true)
                {
                    if (Program.debug_enabled == true)
                    {
                        Console.WriteLine($"Nonce found: {nonce}");
                        Console.WriteLine($"Hash: {hash}");
                    }

                    OnNonceFound(new NonceFoundArgs{ Nonce = nonce });
                    mining = false;
                    return;
                }

                nonce++;
            }
        }
        doWork();
    }

    public static void OnNonceFound(NonceFoundArgs args)
    {
        NonceFound?.Invoke(null, args);
    }

    private static string ComputeSHA1(string input)
    {
        using (SHA1 sha1 = SHA1.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha1.ComputeHash(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
