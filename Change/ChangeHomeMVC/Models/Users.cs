using System;
using System.Collections.Generic;

namespace ChangeHomeMVC.Models
{
    public partial class Users
    {
        public Users()
        {
            Appraise = new HashSet<Appraise>();
            Delivery = new HashSet<Delivery>();
            Favorite = new HashSet<Favorite>();
            Orders = new HashSet<Orders>();
        }

        public int UsersId { get; set; }
        public string UserName { get; set; }
        public string Pwd { get; set; }
        public string Email { get; set; }
        public string Nick { get; set; }
        public int? DeliveryId { get; set; }

        public virtual Delivery DeliveryNavigation { get; set; }
        public virtual ICollection<Appraise> Appraise { get; set; }
        public virtual ICollection<Delivery> Delivery { get; set; }
        public virtual ICollection<Favorite> Favorite { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
