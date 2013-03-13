using System.Collections.Generic;

namespace RealTimeCart.Models
{
    public class Order : Entity
    {
        public string CustomerName { get; set; }
        public double OrderTotal { get; set; }
        public bool Approved { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}