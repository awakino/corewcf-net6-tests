## CoreWCF, .Net 6 and SOAP

A simple sample showing one possible approach to testing a SOAP/WCF service using CoreWCF and .Net 6.

### Sample.CoreWCF
A simple SOAP service with 2 endpoints. Created using the corewcf dotnet templates and refactored to use a Startup class instead of the minimal API paradigm.

### Sample.CoreWCF.Test
An xUnit project that tests the endpoints exposed by the service.

## Why?
The simplest and easiest way to test web services nowadays tends to be to use WebApplicationFactory<> and run everything in memory. This is fine if you want to make straight HTTP requests to the service, but if your service is using SOAP this means hand-crafting the SOAP request body before sending it to the service. This is fiddly and complicated, and is hard to maintain as the service contract develops. It would be nicer to be able to call the methods via a proxy implementation of the contract.

This can be done using ChannelFactory<> to create a channel instance, typed with the methods defined by the service contract. The problem is that ChannelFactory<> has no idea about WebApplicationFactory and in-memory services. The proxy created by ChannelFactory<> will attempt to make actual network requests and will not be able to communicate with the in-memory service.

To avoid this, we spin up an actual instance of the service using Host.CreateDefaultBuilder and tell it where the Startup class is (which is why this has been refactored to _have_ a Startup class). This service can be contacted by the ChannelFactory<> but will be started and stopped based on the lifecycle of the test run and doesn't need to be deployed or managed separately to the tests.