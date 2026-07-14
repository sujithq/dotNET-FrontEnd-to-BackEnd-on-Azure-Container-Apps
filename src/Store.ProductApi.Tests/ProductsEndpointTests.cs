using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Store.ProductApi.Tests;

public class ProductsEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ProductsEndpointTests(WebApplicationFactory<Program> factory) => _factory = factory;

    [Fact]
    public async Task GetProducts_ReturnsTenProducts_WithNames()
    {
        var client = _factory.CreateClient();

        var products = await client.GetFromJsonAsync<List<Product>>("/products");

        Assert.NotNull(products);
        Assert.Equal(10, products.Count);
        Assert.All(products, p => Assert.False(string.IsNullOrWhiteSpace(p.ProductName)));
    }

    [Fact]
    public async Task GetProducts_ReturnsSameList_OnEveryCall()
    {
        var client = _factory.CreateClient();

        var first = await client.GetFromJsonAsync<List<Product>>("/products");
        var second = await client.GetFromJsonAsync<List<Product>>("/products");

        Assert.NotNull(first);
        Assert.NotNull(second);
        Assert.Equal(first.Select(p => p.ProductName), second.Select(p => p.ProductName));
    }
}
