using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Sample.CoreWCF.Client
{
    internal class AddApimHeaderBehaviour : IEndpointBehavior
    {
        private readonly string _subscription;

        internal AddApimHeaderBehaviour(string subscriptionKey)
        {
            _subscription = subscriptionKey;
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            bindingParameters.Add(new Func<HttpClientHandler, HttpMessageHandler>(handler =>
            {
                return new AddApimHeader(handler, _subscription);
            }));
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            return;
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            return;
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            return;
        }
    }
}
