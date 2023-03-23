using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Sample.CoreWCF.Client.Proxy;
using System.ServiceModel;

namespace Sample.CoreWCF.Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create service client");

            var client = new ServiceClient(ServiceClient.EndpointConfiguration.BasicHttpBinding_IService,
                "https://awakino-apim.azure-api.net/corewcf");

            // configure the client to add the APIM Subscription key to the HTTP headers for each request
            client.Endpoint.EndpointBehaviors.Add(new AddApimHeaderBehaviour(GetSubscriptionKey()));

            Console.WriteLine("Calling basic endpoint...");
            string result = client.GetDataAsync(8).GetAwaiter().GetResult();
            Console.WriteLine($"Received '{result}'");

            Console.WriteLine("Calling data contract endpoint...");
            var output = client.GetDataUsingDataContractAsync(new CompositeType
            {
                StringValue = "Stoat",
                BoolValue = true
            }).GetAwaiter().GetResult();
            Console.WriteLine($"Received '{output.StringValue}'");

            Console.WriteLine("Closing remote connection");
            client.Abort();
        }

        private static string GetSubscriptionKey()
        {
            var client = new SecretClient(new Uri("https://awakino-key-vault.vault.azure.net/"), 
                new DefaultAzureCredential());

            var secret = client.GetSecret("core-wcf-subscription-key");
            return secret.Value.Value;
        }
    }
}