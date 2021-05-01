using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models
{
    public class PagingInfo
    {
        public int TotalRecords { get; set; }
        public int RecordPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage => (int)Math.Ceiling((decimal)TotalPage / RecordPerPage);
        public string urlParam { get; set; }
    }
}
