using System.Net;
using System.Text;

namespace Store.Tests;

public class StoreBackendClientTests
{
    private sealed class StubHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string json = request.RequestUri!.AbsolutePath switch
            {
                "/products" => """[{"productId":"p1","productName":"Widget","quantity":0}]""",
                "/inventory/p1" => "42",
                _ => throw new InvalidOperationException($"Unexpected request: {request.RequestUri}")
            };

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });
        }
    }

    private sealed class StubHttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string name) =>
            new(new StubHttpMessageHandler()) { BaseAddress = new Uri("http://localhost") };
    }

    [Fact]
    public async Task GetProducts_DeserializesProductsFromBackend()
    {
        var client = new StoreBackendClient(new StubHttpClientFactory());

        var products = await client.GetProducts();

        var product = Assert.Single(products);
        Assert.Equal("p1", product.ProductId);
        Assert.Equal("Widget", product.ProductName);
    }

    [Fact]
    public async Task GetInventory_ReturnsQuantityFromBackend()
    {
        var client = new StoreBackendClient(new StubHttpClientFactory());

        var quantity = await client.GetInventory("p1");

        Assert.Equal(42, quantity);
    }

    [Fact]
    public void Product_Defaults_AreNotNull()
    {
        var product = new Product();

        Assert.NotNull(product.ProductId);
        Assert.NotNull(product.ProductName);
        Assert.Equal(0, product.Quantity);
    }
}
