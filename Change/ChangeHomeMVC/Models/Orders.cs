using System;
using System.Collections.Generic;

namespace ChangeHomeMVC.Models
{
    public partial class Orders
    {
        public Orders()
        {
            OrdersDetail = new HashSet<OrdersDetail>();
        }

        public string OrdersId { get; set; }
        public DateTime Orderdate { get; set; }
        public int UsersId { get; set; }
        public decimal? Total { get; set; }
        public int? DeliveryId { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int? States { get; set; }
        public string Remark { get; set; }

        public virtual Delivery Delivery { get; set; }
        public virtual Users Users { get; set; }
        public virtual ICollection<OrdersDetail> OrdersDetail { get; set; }
    }
}
