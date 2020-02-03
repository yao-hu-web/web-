using System;
using System.Collections.Generic;

namespace ChangeHomeMVC.Models
{
    public partial class AdminUser
    {
        public int AdminUserId { get; set; }
        public string UserName { get; set; }
        public string Pwd { get; set; }
        public int Aurole { get; set; }
    }
}
