using System.Collections.ObjectModel;
using System.Text.Json;
using ProjectCostEstimator.Models;

namespace ProjectCostEstimator.Services;

public sealed class JsonBomService(string filePath) : IBomService
{
    private readonly SemaphoreSlim gate = new(1, 1);
    private readonly JsonSerializerOptions options = new() { WriteIndented = true };

    public ObservableCollection<MaterialItem> Items { get; } = new();

    public async Task LoadAsync()
    {
        await gate.WaitAsync();
        try
        {
            if (!File.Exists(filePath))
                return;

            await using var stream = File.OpenRead(filePath);
            var loaded = await JsonSerializer.DeserializeAsync<List<MaterialItem>>(stream, options)
                ?? throw new JsonException("The BOM file does not contain a list.");

            Items.Clear();
            foreach (var item in loaded)
                Items.Add(item);
        }
        finally
        {
            gate.Release();
        }
    }

    public async Task AddItemAsync(MaterialItem item)
    {
        await gate.WaitAsync();
        try
        {
            Items.Add(item);
            try { await SaveAsync(); }
            catch { Items.Remove(item); throw; }
        }
        finally { gate.Release(); }
    }

    public async Task ReplaceItemAsync(MaterialItem original, MaterialItem replacement)
    {
        await gate.WaitAsync();
        try
        {
            int index = Items.IndexOf(original);
            if (index < 0) return;
            Items[index] = replacement;
            try { await SaveAsync(); }
            catch { Items[index] = original; throw; }
        }
        finally { gate.Release(); }
    }

    public async Task RemoveItemAsync(MaterialItem item)
    {
        await gate.WaitAsync();
        try
        {
            int index = Items.IndexOf(item);
            if (index < 0) return;
            Items.RemoveAt(index);
            try { await SaveAsync(); }
            catch { Items.Insert(index, item); throw; }
        }
        finally { gate.Release(); }
    }

    private async Task SaveAsync()
    {
        string? directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);

        string temporaryPath = filePath + ".tmp";
        try
        {
            await using (var stream = File.Create(temporaryPath))
                await JsonSerializer.SerializeAsync(stream, Items.ToList(), options);
            File.Move(temporaryPath, filePath, true);
        }
        finally
        {
            if (File.Exists(temporaryPath))
                File.Delete(temporaryPath);
        }
    }
}
