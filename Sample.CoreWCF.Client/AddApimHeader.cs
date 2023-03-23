namespace Sample.CoreWCF.Client
{
    internal class AddApimHeader :  DelegatingHandler
    {
        private readonly string _subscription;

        internal AddApimHeader(HttpClientHandler handler, string subscription)
        {
            InnerHandler = handler;
            _subscription = subscription;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // add the required HTTP header
            request.Headers.Add("Ocp-Apim-Subscription-Key", _subscription);

            // send the message
            return base.SendAsync(request, cancellationToken);
        }
    }
}
