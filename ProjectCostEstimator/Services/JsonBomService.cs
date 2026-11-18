using System.Collections.ObjectModel;
using System.Text.Json;
using ProjectCostEstimator.Models;

namespace ProjectCostEstimator.Services;

public sealed class JsonBomService : IBomService
{
    private readonly string filePath;
    private readonly SemaphoreSlim fileLock = new SemaphoreSlim(1, 1);

    public JsonBomService(string filePath) 
    {
        this.filePath = filePath;
    }

    private readonly JsonSerializerOptions options = new()
    {
        WriteIndented = true
    };

    public ObservableCollection<MaterialItem> Items { get; } = new();

    public async Task LoadAsync()
    {
        await fileLock.WaitAsync();

        try
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            List<MaterialItem>? loaded;

            await using (var stream = File.OpenRead(filePath))
            {
                loaded = await JsonSerializer.DeserializeAsync<List<MaterialItem>>(stream, options);
                if (loaded is null)
                {
                    throw new JsonException("The BOM file does not contain a list.");
                }
            }

            Items.Clear();
            foreach (var item in loaded)
            {
                Items.Add(item);
            }
        }
        finally
        {
            fileLock.Release();
        }
    }

    public async Task AddItemAsync(MaterialItem item)
    {
        await fileLock.WaitAsync();

        try
        {
            Items.Add(item);

            try
            {
                await SaveAsync();
            }
            catch
            {
                Items.Remove(item);
                throw;
            }
        }
        finally
        {
            fileLock.Release();
        }
    }

    public async Task ReplaceItemAsync(MaterialItem original, MaterialItem replacement)
    {
        await fileLock.WaitAsync();
        try
        {
            int index = Items.IndexOf(original);
            if (index < 0) return;

            Items[index] = replacement;

            try
            {
                await SaveAsync();
            }
            catch
            {
                Items[index] = original;
                throw;
            }
        }
        finally
        {
            fileLock.Release();
        }
    }

    public async Task RemoveItemAsync(MaterialItem item)
    {
        await fileLock.WaitAsync();

        try

        {
            int index = Items.IndexOf(item);
            if (index < 0) return;

            Items.RemoveAt(index);

            try
            {
                await SaveAsync();
            }
            catch
            {
                Items.Insert(index, item);
                throw;
            }
        }
        finally
        {
            fileLock.Release();
        }
    }

    private async Task SaveAsync()
    {
        string? directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }
        
        await using (var stream = File.Create(filePath))
        {
            await JsonSerializer.SerializeAsync(stream, Items.ToList(), options);
        }
    }
}
