using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Sample.CoreWCF.Client.Proxy;
using System.ServiceModel;

namespace Sample.CoreWCF.Client
{
    internal class Program
    {
        private static string? _serviceUri;

        private static string? _keyvaultUri;

        static void Main(string[] args)
        {
            ParseArgs(args);
            Console.WriteLine("Create service client");

            var client = new ServiceClient(ServiceClient.EndpointConfiguration.BasicHttpBinding_IService,
                _serviceUri);

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

        private static void ParseArgs(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-v":
                    case "--vaultUri":
                        {
                            _keyvaultUri = args[++i];
                            break;
                        }
                    case "-s":
                    case "--serviceUri":
                        {
                            _serviceUri = args[++i];
                            break;
                        }
                    default:
                        throw new ApplicationException("Unrecognised command line argument");
                }
            }

            if (string.IsNullOrEmpty(_keyvaultUri) || string.IsNullOrEmpty(_serviceUri))
            {
                throw new ApplicationException("Required command line arguments not specified");
            }
        }

        private static string GetSubscriptionKey()
        {
            var client = new SecretClient(new Uri(_keyvaultUri), 
                new DefaultAzureCredential());

            var secret = client.GetSecret("core-wcf-subscription-key");
            return secret.Value.Value;
        }
    }
}