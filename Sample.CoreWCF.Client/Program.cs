using Sample.CoreWCF.Client.Proxy;
using System.ServiceModel;

namespace Sample.CoreWCF.Client
{
    internal class Program
    {
        // TODO: Move to configuration
        private const string APIM_SUBSCRIPTION_KEY = "f83562991d804906803c361e6cc9f069";

        static void Main(string[] args)
        {
            Console.WriteLine("Create service client");

            var client = new ServiceClient(ServiceClient.EndpointConfiguration.BasicHttpBinding_IService,
                "https://awakino-apim.azure-api.net/corewcf");

            // configure the client to add the APIM Subscription key to the HTTP headers for each request
            client.Endpoint.EndpointBehaviors.Add(new AddApimHeaderBehaviour(APIM_SUBSCRIPTION_KEY));

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
    }
}