using System.Web;
using Newtonsoft.Json;

namespace RealTimeCart.Models
{
    public class Book : Entity
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public int SelectedQuantity { get; set; }
        public string Category { get; set; }
    }
}