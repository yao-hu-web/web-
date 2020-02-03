using System;
using System.Collections.Generic;

namespace ChangeApi.Models
{
    public partial class Product
    {
        public Product()
        {
            Appraise = new HashSet<Appraise>();
            Favorite = new HashSet<Favorite>();
            OrdersDetail = new HashSet<OrdersDetail>();
            Photo = new HashSet<Photo>();
        }

        public int ProductId { get; set; }
        public string Title { get; set; }
        public int CateId { get; set; }
        public decimal MarketPrice { get; set; }
        public decimal Price { get; set; }
        public string Content { get; set; }
        public DateTime? PostTime { get; set; }
        public int Stock { get; set; }

        public virtual Category Cate { get; set; }
        public virtual ICollection<Appraise> Appraise { get; set; }
        public virtual ICollection<Favorite> Favorite { get; set; }
        public virtual ICollection<OrdersDetail> OrdersDetail { get; set; }
        public virtual ICollection<Photo> Photo { get; set; }
        public object CategoryId { get; internal set; }
    }
}
