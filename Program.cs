using System;
using System.Threading.Tasks;
using Azure.DigitalTwins.Core;
using Azure.Identity;
using System.Text.Json;
using Azure;

namespace dtlab
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string tenantId = "your azure tenant Id ";
            string clientId = "your service principle client Id ";
            string clientSecret = "your service principle client secret";
            string dtUri = "your digital twins uri";

            var cred = new ClientSecretCredential(
                tenantId,
                clientId,
                clientSecret
            );
            var dtClient = new DigitalTwinsClient(new Uri(dtUri), cred);

            string queryString = "SELECT * FROM DigitalTwins";

            AsyncPageable<BasicDigitalTwin> queryTask = dtClient.QueryAsync<BasicDigitalTwin>(queryString);


            await foreach (var page in queryTask.AsPages(null, 10))
            {
                if (QueryChargeHelper.TryGetQueryCharge(page, out float queryCharge))
                {
                    Console.WriteLine($"Query charge was: {queryCharge}");
                }
                string jsonString = JsonSerializer.Serialize(page, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine(jsonString);
            }
        }

    }
}
