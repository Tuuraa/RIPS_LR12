using Microsoft.AspNetCore.Mvc;

namespace LR12.ApiService
{
    public class ComputerComponent 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public double Price { get; set; }
    }
}
