using System;
using System.Collections.Generic;

namespace ChangeHomeMVC.Models
{
    public partial class Appraise
    {
        public int AppraiseId { get; set; }
        public int UsersId { get; set; }
        public int ProductId { get; set; }
        public string Content { get; set; }
        public int Grade { get; set; }
        public DateTime? RateTime { get; set; }

        public virtual Product Product { get; set; }
        public virtual Users Users { get; set; }
    }
}
