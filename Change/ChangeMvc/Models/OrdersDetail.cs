using System;
using System.Collections.Generic;

namespace ChangeMvc.Models
{
    public partial class OrdersDetail
    {
        public int OrdersDetailId { get; set; }
        public int OrdersId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int? States { get; set; }
        public string PhotoUrl { get; set; }
        public string Title { get; set; }
        public Decimal Price { get; set; }
        public virtual Orders Orders { get; set; }
        public virtual Product Product { get; set; }
    }
}
