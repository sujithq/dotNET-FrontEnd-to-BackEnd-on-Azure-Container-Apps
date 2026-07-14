using Microsoft.AspNetCore.Mvc.Testing;

namespace Store.InventoryApi.Tests;

public class InventoryEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public InventoryEndpointTests(WebApplicationFactory<Program> factory) => _factory = factory;

    [Fact]
    public async Task GetInventory_ReturnsOk_WithQuantityBetween1And100()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/inventory/some-product");

        response.EnsureSuccessStatusCode();
        var quantity = int.Parse(await response.Content.ReadAsStringAsync());
        Assert.InRange(quantity, 1, 100);
    }

    [Fact]
    public async Task GetInventory_ReturnsSameQuantity_ForSameProduct()
    {
        var client = _factory.CreateClient();

        var first = await client.GetStringAsync("/inventory/cached-product");
        var second = await client.GetStringAsync("/inventory/cached-product");

        Assert.Equal(first, second);
    }
}
