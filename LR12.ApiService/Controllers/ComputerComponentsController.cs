using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LR12.ApiService.Controllers
{
    [ApiController]
    [Route("api/components")]
    public class ComputerComponentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ComputerComponentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<ComputerComponent>> CreateComponent(ComputerComponent computerComponent)
        {
            _context.ComputerComponents.Add(computerComponent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetComponent), new { id = computerComponent.Id }, computerComponent);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ComputerComponent>> GetComponent(int id)
        {
            var component = await _context.ComputerComponents.FindAsync(id);
            if (component == null) return NotFound();
            return component;
        }


        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<ComputerComponent>>> GetComponents()
        {
            return await _context.ComputerComponents
                .Select(c => new ComputerComponent
                {
                    Id = c.Id,
                    Name = c.Name,
                    Price = c.Price,
                    Manufacturer = c.Manufacturer
                })
                .ToListAsync();
        }

        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetSalesStatistics()
        {
            try
            {
                // Получаем данные без сортировки (IQueryable)
                var statsQuery = _context.Sales
                    .Include(s => s.Component)
                    .GroupBy(s => s.ComponentId)
                    .Select(g => new
                    {
                        ComponentId = g.Key,
                        ComponentName = g.First().Component!.Name,
                        Manufacturer = g.First().Component!.Manufacturer,
                        TotalSold = g.Sum(s => s.Quantity),
                        TotalRevenue = g.Sum(s => s.Quantity * s.UnitPrice),
                        LastSaleDate = g.Max(s => s.SaleDate),
                        AveragePrice = g.Average(s => s.UnitPrice)
                    });

                var statsList = await statsQuery.ToListAsync();

                var sortedStats = statsList
                    .OrderByDescending(s => (double)s.TotalRevenue)
                    .ToList();

                var totalSales = sortedStats.Sum(s => s.TotalSold);
                var totalRevenue = sortedStats.Sum(s => s.TotalRevenue);
                var totalProducts = sortedStats.Count;

                return Ok(new
                {
                    TotalSales = totalSales,
                    TotalRevenue = totalRevenue,
                    TotalProducts = totalProducts,
                    AverageOrderValue = totalSales > 0 ? totalRevenue / totalSales : 0,
                    ComponentsStats = sortedStats
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
