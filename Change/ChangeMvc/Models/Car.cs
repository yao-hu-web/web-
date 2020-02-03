using System;
using System.Collections.Generic;

namespace ChangeMvc.Models
{
    public partial class Car
    {
        public int CarId { get; set; }
        public int ProductId { get; set; }
        public DateTime AddDay { get; set; }
        public int ProductNum { get; set; }
        public int UsersId { get; set; }
        public string PhotoUrl { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public virtual Product Product { get; set; }
        public virtual Users Users { get; set; }
    }
}
