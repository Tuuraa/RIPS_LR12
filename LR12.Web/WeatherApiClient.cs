namespace LR12.Web;

public class WeatherApiClient(HttpClient httpClient)
{
    // Метод для получения прогноза погоды (без изменений)
    public async Task<WeatherForecast[]> GetWeatherAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        List<WeatherForecast>? forecasts = null;

        await foreach (var forecast in httpClient.GetFromJsonAsAsyncEnumerable<WeatherForecast>("/weatherforecast", cancellationToken))
        {
            if (forecasts?.Count >= maxItems) break;
            if (forecast is not null)
            {
                forecasts ??= [];
                forecasts.Add(forecast);
            }
        }

        return forecasts?.ToArray() ?? [];
    }

    // Метод для получения всех компонентов
    public async Task<ComputerComponent[]> GetComponentsAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            return await httpClient.GetFromJsonAsync<ComputerComponent[]>(
                "api/components/get-all", // Соответствует [HttpGet("get-all")]
                cancellationToken) ?? Array.Empty<ComputerComponent>();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error fetching components: {ex.Message}");
            return Array.Empty<ComputerComponent>();
        }
    }

    // Метод для получения статистики продаж
    public async Task<object> GetSalesStatisticsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await httpClient.GetFromJsonAsync<object>(
                "api/components/stats",
                cancellationToken) ?? new
                {
                    TotalSales = 0,
                    TotalRevenue = 0m,
                    TotalProducts = 0,
                    AverageOrderValue = 0m,
                    ComponentsStats = Array.Empty<object>()
                };
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error fetching sales statistics: {ex.Message}");
            return new
            {
                TotalSales = 0,
                TotalRevenue = 0m,
                TotalProducts = 0,
                AverageOrderValue = 0m,
                ComponentsStats = Array.Empty<object>()
            };
        }
    }

    // Метод для создания компонента
    public async Task<ComputerComponent> CreateComponentAsync(ComputerComponent component, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync(
                "api/components", // Соответствует [HttpPost]
                component,
                cancellationToken);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ComputerComponent>() ?? throw new Exception("Invalid response");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating component: {ex.Message}");
            throw;
        }
    }

    // Метод для получения компонента по ID
    public async Task<ComputerComponent?> GetComponentAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await httpClient.GetFromJsonAsync<ComputerComponent>(
                $"api/components/{id}", // Соответствует [HttpGet("{id}")]
                cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error fetching component {id}: {ex.Message}");
            return null;
        }
    }
}

// Модели данных остаются без изменений
public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public record ComputerComponent(
    int Id,
    string Name,
    string Manufacturer,
    double Price,
    string Type
);

public class SalesStatistics
{
    public int TotalSales { get; set; }
    public decimal TotalRevenue { get; set; }
    public int TotalProducts { get; set; }
    public decimal AverageOrderValue { get; set; }
    public List<ComponentStats> ComponentsStats { get; set; } = new();
}

public class ComponentStats
{
    public int ComponentId { get; set; }
    public string ComponentName { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public int TotalSold { get; set; }
    public decimal TotalRevenue { get; set; }
    public DateTime LastSaleDate { get; set; }
    public decimal AveragePrice { get; set; }
}
public record ProductStat
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public int UnitsSold { get; set; }
    public double TotalRevenue { get; set; }
    public DateTime LastSaleDate { get; set; }
}