using System;
using System.Collections.Generic;

namespace ChangeHomeMVC.Models
{
    public partial class News
    {
        public int NewsId { get; set; }
        public string Title { get; set; }
        public string Ntypes { get; set; }
        public string Content { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime? PushTime { get; set; }
        public int? States { get; set; }
    }
}
