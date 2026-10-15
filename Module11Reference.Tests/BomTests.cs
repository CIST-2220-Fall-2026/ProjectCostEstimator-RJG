using ProjectCostEstimator.Models;
using ProjectCostEstimator.Services;
using ProjectCostEstimator.ViewModels;

namespace ProjectCostEstimator.Tests;

public class BomTests
{
    private static MaterialItem Sensor(decimal quantity = 2) =>
        new("Sensor", "https://example.com", "ea", 12.50m, quantity);

    [Fact]
    public async Task JsonServiceSavesAndReloadsChanges()
    {
        string directory = Path.Combine(Path.GetTempPath(), "bom-test-" + Guid.NewGuid());
        string file = Path.Combine(directory, "bom.json");
        try
        {
            var first = new JsonBomService(file);
            MaterialItem original = Sensor();
            await first.AddItemAsync(original);
            await first.ReplaceItemAsync(original, Sensor(3));

            var second = new JsonBomService(file);
            await second.LoadAsync();
            Assert.Single(second.Items);
            Assert.Equal(37.50m, second.Items[0].TotalCost);

            await second.RemoveItemAsync(second.Items[0]);
            var third = new JsonBomService(file);
            await third.LoadAsync();
            Assert.Empty(third.Items);
        }
        finally
        {
            if (Directory.Exists(directory))
                Directory.Delete(directory, true);
        }
    }

    [Fact]
    public async Task CancelAndDeclinedDeleteLeaveTheCollectionAlone()
    {
        var service = new InMemoryBomService();
        MaterialItem original = Sensor();
        await service.AddItemAsync(original);
        var interactions = new FakeInteractions { EditResult = null, ConfirmResult = false };
        var viewModel = new BillOfMaterialsViewModel(service, interactions);

        await viewModel.AddCommand.ExecuteAsync(null);
        await viewModel.EditCommand.ExecuteAsync(original);
        await viewModel.DeleteCommand.ExecuteAsync(original);

        Assert.Single(service.Items);
        Assert.Same(original, service.Items[0]);
    }

    [Fact]
    public async Task ConfirmedDeleteRemovesTheSelectedItem()
    {
        var service = new InMemoryBomService();
        MaterialItem original = Sensor();
        await service.AddItemAsync(original);
        var viewModel = new BillOfMaterialsViewModel(service, new FakeInteractions { ConfirmResult = true });

        await viewModel.DeleteCommand.ExecuteAsync(original);

        Assert.Empty(service.Items);
    }

    private sealed class FakeInteractions : IBomInteractionService
    {
        public MaterialItem? EditResult { get; set; }
        public bool ConfirmResult { get; set; }

        public Task<MaterialItem?> EditItemAsync(MaterialItem? original) =>
            Task.FromResult(EditResult);

        public Task<bool> ConfirmDeleteAsync(MaterialItem item) =>
            Task.FromResult(ConfirmResult);
    }
}
