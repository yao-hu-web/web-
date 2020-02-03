using System;
using System.Collections.Generic;

namespace ChangeHomeMVC.Models
{
    public partial class Favorite
    {
        public int FavoriteId { get; set; }
        public int ProductId { get; set; }
        public int UsersId { get; set; }

        public virtual Product Product { get; set; }
        public virtual Users Users { get; set; }
    }
}
