using Xunit.Abstractions;

namespace Sample.CoreWCF.Test
{
    public class ServiceTests : IClassFixture<ServiceFixture>
    {
        private readonly ServiceFixture _service;
        private readonly ChannelFactory<IService> _factory;
        private readonly ITestOutputHelper _output;

        public ServiceTests(ServiceFixture service, ITestOutputHelper output)
        {
            _service = service;
            _output = output;

            _factory = new ChannelFactory<IService>(
                new BasicHttpBinding(BasicHttpSecurityMode.Transport),
                new EndpointAddress(new Uri($"{_service.ServerUri}TestService/Service.svc"))
            );
        }

        private IService GetChannel()
        {
            return _factory.CreateChannel();
        }

        [Fact]
        public void GetDataTest()
        {
            string result = GetChannel().GetData(5);
            Assert.Equal("You entered: 5", result);
        }

        [Fact]
        public void DataContractSuffixTest()
        {
            var result = GetChannel().GetDataUsingDataContract(new CompositeType
            {
                BoolValue = true,
                StringValue = "Badger"
            });

            Assert.Equal("BadgerSuffix", result.StringValue);
        }

        [Fact]
        public void DataContractNoSuffixTest()
        {
            var result = GetChannel().GetDataUsingDataContract(new CompositeType
            {
                BoolValue = false,
                StringValue = "Weasel"
            });

            Assert.Equal("Weasel", result.StringValue);
        }
    }
}
